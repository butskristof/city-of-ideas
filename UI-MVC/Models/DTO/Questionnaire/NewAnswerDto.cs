namespace COI.UI.MVC.Models.DTO.Questionnaire
{
	public class NewAnswerDto
	{
		public string Content { get; set; }
		public int OptionId { get; set; }
		public int QuestionId { get; set; }
		public string UserId { get; set; }
	}
}