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
	/// <summary>
	/// Various helpers for handling file access and binding the file location to the object.
	/// </summary>
	public interface IFileService
	{
		/// <summary>
		/// Places file in the correct location on disk and saves the location to the user object.
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="picture"></param>
		/// <returns></returns>
		Task<string> SetUserProfilePicture(string userId, IFormFile picture);
		/// <summary>
		/// Places file in the correct location on disk and saves the location to the organisation object.
		/// </summary>
		/// <param name="organisationId"></param>
		/// <param name="picture"></param>
		/// <returns></returns>
		Task<string> SetOrganisationLogo(int organisationId, IFormFile picture);
		/// <summary>
		/// Places file in the correct location on disk and saves the location to the organisation object.
		/// </summary>
		/// <param name="organisationId"></param>
		/// <param name="picture"></param>
		/// <returns></returns>
		Task<string> SetOrganisationImage(int organisationId, IFormFile picture);
		/// <summary>
		/// Takes in a file, writes it to an appropriate location on disk and returns the location as a string starting
		/// from basepath.
		/// </summary>
		/// <param name="file"></param>
		/// <param name="folder"></param>
		/// <returns></returns>
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

		public async Task<string> SetOrganisationImage(int organisationId, IFormFile picture)
		{
			var folderpath = Path.Combine(FolderConstants.OrganisationPath, organisationId.ToString());
			var imgpath = await ConvertFileToLocation(picture, folderpath);
			_organisationManager.AddImageToOrganisation(organisationId, imgpath);
			return imgpath;
		}

		public async Task<string> ConvertFileToLocation(IFormFile file, string folder = FolderConstants.FilePath)
		{
			// establish standard upload path, optionally followed by a specific folder
            var basepath = Path.Combine(_hostingEnvironment.WebRootPath, FolderConstants.UploadPath, folder);
            
            // create directory if it doesn't exist yet
            Directory.CreateDirectory(basepath);

            // only proceed if file is valid
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

	            // build location for web
	            var webpath = $"/{FolderConstants.UploadPath}/{folder}/{filename}";
	            return webpath;
            }
            
            throw new ArgumentException("No file.");
		}
	}
}