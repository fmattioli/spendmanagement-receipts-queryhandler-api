namespace Receipts.QueryHandler.Domain.Entities
{
    public class Category(Guid id, Guid userId, Tenant tenant, string name, DateTime createdDate)
    {
        public Guid Id { get; set; } = id;
        public Guid UserId { get; set; } = userId;
        public Tenant Tenant { get; set; } = tenant;
        public string Name { get; set; } = name;
        public DateTime CreatedDate { get; set; } = createdDate;
    }
}
