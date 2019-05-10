using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

		public void AddAnswerToUser(Answer answer, string userId)
		{
			Domain.User.User user = this.GetUser(userId);
			if (user == null)
			{
				throw new ArgumentException("User not found.");
			}

			answer.User = user;
			user.Answers.Add(answer);
			_userRepository.UpdateUser(user);
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

			value = Helpers.Helpers.LimitToRange(value, 
				Helpers.Helpers.MinVoteValue, 
				Helpers.Helpers.MaxVoteValue);
			
			Vote vote = new Vote()
			{
				Value = value,
				User = user
			};

			return AddVote(vote);
		}

		public Vote AddVoteWithEmail(int value, string email)
		{
			value = Helpers.Helpers.LimitToRange(value, 
				Helpers.Helpers.MinVoteValue, 
				Helpers.Helpers.MaxVoteValue);
			
			Vote vote = new Vote()
			{
				Value = value,
				Email = email
			};

			return AddVote(vote);
		}

		public Vote AddAnonymousVote(int value)
		{
			value = Helpers.Helpers.LimitToRange(value, 
				Helpers.Helpers.MinVoteValue, 
				Helpers.Helpers.MaxVoteValue);
			
			Vote vote = new Vote()
			{
				Value = value
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

		public Vote GetVoteForIdea(int ideaId, string userId)
		{
			IEnumerable<Vote> ideaVotes = _voteRepository.ReadVotesForIdea(ideaId);
			return ideaVotes.FirstOrDefault(v => v.User.Id == userId);
		}

		public Vote GetEmailVoteForIdea(int ideaId, string email)
		{
			IEnumerable<Vote> ideaVotes = _voteRepository.ReadVotesForIdea(ideaId);
			return ideaVotes.FirstOrDefault(v => v.Email == email);
		}

		public Vote GetEmailVoteForComment(int commentId, string email)
		{
			IEnumerable<Vote> ideaVotes = _voteRepository.ReadVotesForComment(commentId);
			return ideaVotes.FirstOrDefault(v => v.Email == email);
		}

		public Vote GetEmailVoteForIdeation(int ideationId, string email)
		{
			IEnumerable<Vote> ideaVotes = _voteRepository.ReadVotesForIdeation(ideationId);
			return ideaVotes.FirstOrDefault(v => v.Email == email);
		}

		public Vote GetVoteForIdeation(int ideationId, string userId)
		{
			IEnumerable<Vote> ideationVotes = _voteRepository.ReadVotesForIdeation(ideationId);
			return ideationVotes.FirstOrDefault(v => v.User.Id == userId);
		}

		public Vote GetVoteForComment(int commentId, string userId)
		{
			IEnumerable<Vote> commentVotes = _voteRepository.ReadVotesForComment(commentId);
			return commentVotes.FirstOrDefault(v => v.User.Id == userId);
		}

		private void Validate(Vote vote)
		{
			Validator.ValidateObject(vote, new ValidationContext(vote), true);
		}

		public Vote ChangeVoteValue(int voteId, int value)
		{
			Vote toChange = GetVote(voteId);
			if (toChange != null)
			{
				toChange.Value = value;

				Validate(toChange);
				return _voteRepository.UpdateVote(toChange);
			}
			
			throw new ArgumentException("Vote not found.", "voteId");
		}

		#endregion

		public void AddPictureLocationToUser(string userId, string imgpath)
		{
			Domain.User.User user = this.GetUser(userId);
			if (user == null)
			{
				throw new ArgumentException("User not found.");
			}

			user.ProfilePictureLocation = imgpath;
			_userRepository.UpdateUser(user);
		}
	}
}