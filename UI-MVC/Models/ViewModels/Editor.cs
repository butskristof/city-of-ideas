namespace COI.UI.MVC.Models
{
	public class Editor
	{
		public Editor()
		{
			LoginRequired = true;
		}
		
		public string Type { get; set; }
		public string FormId { get; set; }
		public bool LoginRequired { get; set; }
	}
}