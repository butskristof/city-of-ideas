using System;
using System.Collections.Generic;
using System.Linq;
using COI.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace COI.DAL.User.EF
{
	public class UserRepository : EfRepository, IUserRepository
	{
		public UserRepository(CityOfIdeasDbContext ctx) : base(ctx)
		{
		}

		public IEnumerable<BL.Domain.User.User> ReadUsers()
		{
			return _ctx.Users.AsEnumerable();
		}

		public BL.Domain.User.User ReadUser(int userId)
		{
			return _ctx.Users.Find(userId);
		}

		public BL.Domain.User.User CreateUser(BL.Domain.User.User user)
		{
			if (ReadUser(user.UserId) != null)
			{
				throw new ArgumentException("User already in database.");
			}

			try
			{
				_ctx.Users.Add(user);
				_ctx.SaveChanges();

				return user;
			}
			catch (DbUpdateException exception)
			{
				var msg = exception.InnerException == null ? "Invalid object." : exception.InnerException.Message;
				throw new ArgumentException(msg);
			}
		}

		public BL.Domain.User.User UpdateUser(BL.Domain.User.User user)
		{
			var entryToUpdate = ReadUser(user.UserId);

			if (entryToUpdate == null)
			{
				throw new ArgumentException("User to update not found.");
			}

			_ctx.Entry(entryToUpdate).CurrentValues.SetValues(user);
			_ctx.SaveChanges();

			return ReadUser(user.UserId);
		}

		public BL.Domain.User.User DeleteUser(int userId)
		{
			var userToDelete = ReadUser(userId);
			if (userToDelete == null)
			{
				throw new ArgumentException("User to delete not found.");
			}

			_ctx.Users.Remove(userToDelete);
			_ctx.SaveChanges();

			return userToDelete;
		}
	}
}