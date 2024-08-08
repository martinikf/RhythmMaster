using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(p=>p.Id);
        builder.Property(p => p.Id).HasConversion(
            id => id.Value,
            value => new PersonId(value));
        builder.Property(p => p.Id).ValueGeneratedOnAdd();

    }
}