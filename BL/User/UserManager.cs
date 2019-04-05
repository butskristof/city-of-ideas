using System;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.Questionnaire;
using COI.BL.Domain.User;
using COI.DAL.User;

namespace COI.BL.User
{
	public class UserManager : IUserManager
	{
		private readonly IUserRepository _userRepository;
		private readonly IVoteRepository _voteRepository;

		public UserManager(IUserRepository userRepository, IVoteRepository voteRepository)
		{
			_userRepository = userRepository;
			_voteRepository = voteRepository;
		}

		public Domain.User.User GetUser(int userId)
		{
			return _userRepository.ReadUser(userId);
		}

		public Vote AddVoteToUser(int userId, int value) {
			Vote vote = new Vote()
			{
				Value = value
			};
			
			AddVoteToUser(userId, vote);
			
			return _voteRepository.CreateVote(vote);
		}

		public void AddVoteToUser(int userId, Vote vote)
		{
			Domain.User.User u = this.GetUser(userId);
			if (u != null)
			{
				vote.User = u;
				u.Votes.Add(vote);
				_userRepository.UpdateUser(u);
			}
			else
			{
				throw new ArgumentException("User not found.");
			}
		}

		public void AddCommentToUser(int userId, Comment comment)
		{
			Domain.User.User u = this.GetUser(userId);
			if (u != null)
			{
				comment.User = u;
				u.Comments.Add(comment);
				_userRepository.UpdateUser(u);
			}
			else
			{
				throw new ArgumentException("User not found.");
			}
		}

		public void AddAnswerToUser(int userId, Answer answer)
		{
			Domain.User.User u = this.GetUser(userId);
			if (u != null)
			{
				answer.User = u;
				u.Answers.Add(answer);
				_userRepository.UpdateUser(u);
			}
			else
			{
				throw new ArgumentException("User not found.");
			}
		}
	}
}