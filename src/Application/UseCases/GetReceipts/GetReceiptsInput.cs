using Application.UseCases.Common;

namespace Application.UseCases.GetReceipts
{
    public class GetReceiptsInput
    {
        public IEnumerable<Guid> ReceiptIds { get; set; } = new List<Guid>();
        public IEnumerable<Guid> ReceiptItemIds { get; set; } = new List<Guid>();
        public IEnumerable<string> EstablishmentNames { get; set; } = new List<string>();
        public IEnumerable<string> ItemNames { get; set; } = new List<string>();
        public DateTime ReceiptDate { get; set; }
        public DateTime ReceiptDateFinal { get; set; }
        public PageFilter PageFilter { get; set; } = null!;
    }
}
