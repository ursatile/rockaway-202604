namespace Rockaway.Messages {

	public class TicketOrderMailMessage {

		public TicketOrderMailMessage(string customerEmail,
			string artistName,
			string venueName,
			string showDate) {
			this.CustomerEmail = customerEmail;
			this.ArtistName = artistName;
			this.VenueName = venueName;
			this.ShowDate = showDate;
		}

		public TicketOrderMailMessage() { }
		public string ArtistName { get; set; } = "";
		public string VenueName { get; set; } = "";
		public string ShowDate { get; set; } = "";

		public string CustomerEmail { get; set; } = "";
	}
}
