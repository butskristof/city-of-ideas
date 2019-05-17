using System;
using System.IO;
using System.Threading.Tasks;
using COI.BL.Organisation;
using COI.BL.User;
using COI.UI.MVC.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace COI.UI.MVC.Services
{
	public interface IFileService
	{
		Task<string> SetUserProfilePicture(string userId, IFormFile picture);
		Task<string> SetOrganisationLogo(int organisationId, IFormFile picture);
		Task<string> ConvertFileToLocation(IFormFile file, string folder = FolderConstants.FilePath);
	}
	
	public class FileService : IFileService
	{
		private readonly IUserManager _userManager;
		private readonly IOrganisationManager _organisationManager;
		private readonly IHostingEnvironment _hostingEnvironment;

		public FileService(IUserManager userManager, IOrganisationManager organisationManager, IHostingEnvironment hostingEnvironment)
		{
			_userManager = userManager;
			_organisationManager = organisationManager;
			_hostingEnvironment = hostingEnvironment;
		}

		public async Task<string> SetUserProfilePicture(string userId, IFormFile picture)
		{
			var folderpath = Path.Combine(FolderConstants.UserPath, userId);
			var imgpath = await ConvertFileToLocation(picture, folderpath);
			_userManager.AddPictureLocationToUser(userId, imgpath);
			return imgpath;
		}

		public async Task<string> SetOrganisationLogo(int organisationId, IFormFile picture)
		{
			var folderpath = Path.Combine(FolderConstants.OrganisationPath, organisationId.ToString());
			var imgpath = await ConvertFileToLocation(picture, folderpath);
			_organisationManager.AddLogoToOrganisation(organisationId, imgpath);
			return imgpath;
		}

		public async Task<string> ConvertFileToLocation(IFormFile file, string folder = FolderConstants.FilePath)
		{
            var basepath = Path.Combine(_hostingEnvironment.WebRootPath, FolderConstants.UploadPath, folder);
            
            // create directory if it doesn't exist yet
            Directory.CreateDirectory(basepath);

            if (file != null && file.Length > 0)
            {
	            // generate unique filename
	            var extension = Path.GetExtension(file.FileName);
	            var filename = Guid.NewGuid() + extension;

	            var filepath = Path.Combine(basepath, filename);
	            using (var fileStream = new FileStream(filepath, FileMode.Create))
	            {
		            await file.CopyToAsync(fileStream);
	            }

	            var webpath = $"/{FolderConstants.UploadPath}/{folder}/{filename}";
	            return webpath;
            }
            
            throw new ArgumentException("No file.");
		}
	}
}