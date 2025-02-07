using Ordering.Domain.Enums;

namespace Ordering.Infrastructure.Data.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(id => id.Value, dbId => OrderId.Of(dbId));

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .IsRequired();

        builder.HasMany<OrderItem>()
            .WithOne()
            .HasForeignKey(o => o.OrderId);

        builder.ComplexProperty(
            o => o.OrderName, nameBuilder =>
            {
                nameBuilder.Property(n => n.Value)
                    .HasColumnName(nameof(Order.OrderName))
                    .HasMaxLength(100)
                    .IsRequired();
            });

        builder.ComplexProperty(
            o => o.ShippingAddress, addressBuilder =>
            {
                addressBuilder.Property(n => n.FirstName)
                    .HasMaxLength(50)
                    .IsRequired();

                addressBuilder.Property(n => n.LastName)
                    .HasMaxLength(50)
                    .IsRequired();

                addressBuilder.Property(n => n.EmailAddress)
                    .HasMaxLength(50);

                addressBuilder.Property(n => n.AddressLine)
                    .HasMaxLength(180)
                    .IsRequired();

                addressBuilder.Property(n => n.Country)
                    .HasMaxLength(50);

                addressBuilder.Property(n => n.State)
                    .HasMaxLength(50);

                addressBuilder.Property(n => n.ZipCode)
                    .HasMaxLength(5)
                    .IsRequired();
            });

        builder.ComplexProperty(
            o => o.BillingAddress, addressBuilder =>
            {
                addressBuilder.Property(n => n.FirstName)
                    .HasMaxLength(50)
                    .IsRequired();

                addressBuilder.Property(n => n.LastName)
                    .HasMaxLength(50)
                    .IsRequired();

                addressBuilder.Property(n => n.EmailAddress)
                    .HasMaxLength(50);

                addressBuilder.Property(n => n.AddressLine)
                    .HasMaxLength(180)
                    .IsRequired();

                addressBuilder.Property(n => n.Country)
                    .HasMaxLength(50);

                addressBuilder.Property(n => n.State)
                    .HasMaxLength(50);

                addressBuilder.Property(n => n.ZipCode)
                    .HasMaxLength(5)
                    .IsRequired();
            });

        builder.ComplexProperty(
            o => o.Payment, paymentBuilder =>
            {
                paymentBuilder.Property(n => n.CardName)
                    .HasMaxLength(50);

                paymentBuilder.Property(n => n.CardNumber)
                    .HasMaxLength(24)
                    .IsRequired();

                paymentBuilder.Property(n => n.Expiration)
                    .HasMaxLength(10);

                paymentBuilder.Property(n => n.CVV)
                    .HasMaxLength(3);

                paymentBuilder.Property(n => n.PaymentMethod);
            });

        builder.Property(x => x.Status)
            .HasDefaultValue(OrderStatus.Draft)
            .HasConversion(
                s => s.ToString(), 
                dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus));

        builder.Property(x => x.TotalPrice);
    }
}