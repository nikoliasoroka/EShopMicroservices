namespace Ordering.Infrastructure.Data.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, dbId => ProductId.Of(dbId));

        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
    }
}