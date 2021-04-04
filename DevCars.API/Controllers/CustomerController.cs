using DevCars.API.Entities;
using DevCars.API.InputModels;
using DevCars.API.Persistence;
using DevCars.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevCars.API.Controllers
{
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly DevCarsDbContext _dbContext;

        public CustomerController(DevCarsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public IActionResult Post([FromBody] AddCustomerInputModel model)
        {
            var customer = new Customer(model.FullName, model.Document, model.BirthDate);

            _dbContext.Customers.Add(customer);
            _dbContext.SaveChanges();

            return Ok();
        }

        [HttpPost("{id}/orders")]
        public IActionResult PostOrder(int id, [FromBody] AddOrderInputModel model)
        {
            var extraItens = model.ExtraItens
                .Select(x => new ExtraOrdemItem(x.Description, x.Price))
                .ToList();

            var car = _dbContext.Cars.SingleOrDefault(x => x.Id == id);
            var order = new Order(model.IdCar, model.IdCustomer, car.Price, extraItens); 
            
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();

            return CreatedAtAction(
                nameof(GetOrder),
                new { id = order.IdCustomer, orderId = order.Id},
                model
                );
        }

        [HttpGet("{id}/orders{orderId}")]
        public IActionResult GetOrder(int id, int orderId)
        {
            var order = _dbContext.Orders
                .Include(x => x.ExtraItens)
                .SingleOrDefault(o => o.Id == orderId);

            if (order == null)
                return NotFound();

            var extraItens = order.ExtraItens.Select(x => x.Description).ToList();

            var orderViewModel = new OrderDetailsViewModel(order.IdCar, order.IdCustomer, order.TotalCost, extraItens);

            return Ok();
        }
    }
}
