namespace Domain.StoreSystem.models;

public class OrderItem
{
        public Guid Id { get; set; }
        public int Amount { get; set; }
        
        public Guid ProductId { get; set; } // foreign key
        
        public Guid OrderId { get; set; } // foreign key
        public Order Order { get; set; } // navigation property
}