using DevCars.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevCars.API.Persistence.Configurations
{
    public class ExtraOrderItemDbConfiguration : IEntityTypeConfiguration<ExtraOrdemItem>
    {
        public void Configure(EntityTypeBuilder<ExtraOrdemItem> builder)
        {
            builder.HasKey(c => c.Id);
        }
    }
}
