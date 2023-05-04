namespace WOF.Domain.Events;

public class ProductDetailWalmartIdUpdatedEvent : BaseEvent
{
    public ProductDetailWalmartIdUpdatedEvent(Product product)
    {
        Product = product;
    }

    public Product Product { get; }
}
