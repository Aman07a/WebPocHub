namespace WebPocHub.WebApi.DTO
{
	public class UpdateEventDTO
	{
		public int EventId { get; set; }
		public string EventCode { get; set; } = string.Empty;
		public string EventName { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public decimal Fees { get; set; }
		public int SeatsFilled { get; set; }
		public string Logo { get; set; } = string.Empty;
	}
}
