using System;
using System.ComponentModel.DataAnnotations;
using COI.BL.Domain.Ideation;
using COI.BL.Domain.Questionnaire;
using COI.BL.Domain.User;
using COI.BL.Ideation;
using COI.DAL.User;

namespace COI.BL.User
{
	public class UserManager : IUserManager
	{
		private readonly IUserRepository _userRepository;
		private readonly IVoteRepository _voteRepository;
		private readonly IIdeationManager _ideationManager;

		public UserManager(IUserRepository userRepository, IVoteRepository voteRepository, IIdeationManager ideationManager)
		{
			_userRepository = userRepository;
			_voteRepository = voteRepository;
			_ideationManager = ideationManager;
		}

		public Domain.User.User GetUser(string userId)
		{
			return _userRepository.ReadUser(userId);
		}

		public void AddVoteToUser(string userId, Vote vote)
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

		public void AddCommentToUser(Comment comment, string userId)
		{
			Domain.User.User user = GetUser(userId);
			if (user == null)
			{
				throw new ArgumentException("User not found.");
			}
			
			comment.User = user;
			user.Comments.Add(comment);
			_userRepository.UpdateUser(user);
		}

		public void AddAnswerToUser(string userId, Answer answer)
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

		#region Votes

		public Vote GetVote(int voteId)
		{
			return _voteRepository.ReadVote(voteId);
		}

		public Vote AddVoteToUser(int value, string userId)
		{
			Domain.User.User user = GetUser(userId);
			if (user == null)
			{
				throw new ArgumentException("User not found.", "userId");
			}
			
			Vote vote = new Vote()
			{
				Value = value,
				User = user
			};

			return AddVote(vote);
		}

		private Vote AddVote(Vote vote)
		{
			Validate(vote);
			return _voteRepository.CreateVote(vote);
		}

		public Vote RemoveVote(int voteId)
		{
			return _voteRepository.DeleteVote(voteId);
		}

		private void Validate(Vote vote)
		{
			Validator.ValidateObject(vote, new ValidationContext(vote), true);
		}

		#endregion
	}
}