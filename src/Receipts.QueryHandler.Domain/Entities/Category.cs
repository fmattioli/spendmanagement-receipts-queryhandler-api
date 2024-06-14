namespace Receipts.QueryHandler.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; set; }
        public Tenant Tenant { get; set; } = null!;
        public string Name { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}
