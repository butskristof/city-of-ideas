using System;
using System.IO;
using System.Threading.Tasks;
using COI.BL.User;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace COI.UI.MVC.Services
{
	public interface IFileService
	{
		Task<string> SetUserProfilePicture(string userId, IFormFile picture);
	}
	
	public class FileService : IFileService
	{
		private readonly IUserManager _userManager;
		private readonly IHostingEnvironment _hostingEnvironment;

		public FileService(IUserManager userManager, IHostingEnvironment hostingEnvironment)
		{
			_userManager = userManager;
			_hostingEnvironment = hostingEnvironment;
		}

		public async Task<string> SetUserProfilePicture(string userId, IFormFile picture)
		{
            var basepath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "users");
            var userfolderpath = Path.Combine(basepath, userId);

            // create directory if it doesn't exist yet
            Directory.CreateDirectory(userfolderpath);

            if (picture != null && picture.Length > 0)
            {
                // generate unique filename
                var extension = Path.GetExtension(picture.FileName);
                var filename = Guid.NewGuid() + extension;
                // TODO check extension and file type
                var filepath = Path.Combine(userfolderpath, filename);
                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    await picture.CopyToAsync(fileStream);
                }

                var imgpath = $"/uploads/users/{userId}/{filename}";
                _userManager.AddPictureLocationToUser(userId, imgpath);

                return imgpath;
            }

            return null;
		}

		public async Task<string> UploadNewField(string fieldId, IFormFile file)
		{
            var basepath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", "fields");
            var userfolderpath = Path.Combine(basepath, fieldId);

            // create directory if it doesn't exist yet
            Directory.CreateDirectory(userfolderpath);

            if (file != null && file.Length > 0)
            {
                // generate unique filename
                var extension = Path.GetExtension(file.FileName);
                var filename = Guid.NewGuid() + extension;
                // TODO check extension and file type
                var filepath = Path.Combine(userfolderpath, filename);
                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                var imgpath = $"/uploads/fields/{fieldId}/{filename}";
//                _userManager.AddPictureLocationToUser(userId, imgpath);

                return imgpath;
            }

            return null;
		}
	}
}