namespace COI.BL.Domain.Ideation
{
	/// <summary>
	/// Interface to denote that an object can be voted on and a way to access its current voteCount
	/// </summary>
	public interface IVotable
	{
		int GetScore();
	}
}