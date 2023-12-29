using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServerSide.Database.Models;

namespace ServerSide.Database.ModelsConfiguration;

public class ItemTypeConfiguration : IEntityTypeConfiguration<ItemType>
{
    public void Configure(EntityTypeBuilder<ItemType> builder)
    {
        builder.HasKey(i => i.Id);
    }
}