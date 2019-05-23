using COI.BL.Domain.Ideation;

namespace COI.BL.Domain.User
{
	public class Flag
	{
		public int FlagId { get; set; }
		public virtual User User { get; set; }
		public virtual Idea Idea { get; set; }
		public virtual Comment Comment { get; set; }
	}
}