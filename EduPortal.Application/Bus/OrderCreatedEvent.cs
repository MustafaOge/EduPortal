namespace EduPortal.Application.Bus
{
    public record OrderCreatedEvent(
        string OrderId,
        DateTime Created,
        int UserId,
        decimal TotalPrice,
        List<OrderItem> OrderItems);


    public record OrderItem(int ProductId, int Count);
}