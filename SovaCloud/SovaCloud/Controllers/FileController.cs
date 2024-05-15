using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SovaCloud.Data;
using SovaCloud.Repositories;
using System.Security.Claims;

namespace SovaCloud.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName = "sovacloud.s3bucket";
        private readonly SovaCloudDbContext _context;

        public FileController(SovaCloudDbContext context)
        {
            _s3Client = new AmazonS3Client(RegionEndpoint.EUCentral1);
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> UploadNewFile(IFormFile file)
        {
			var userRepository = new UserRepository(_context);
			var currentUser = userRepository.GetByEmail(GetCurrentUserPrefix()).Result;

			var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, _bucketName);
            if (!bucketExists)
            {
                return NotFound($"Bucket {_bucketName} does not found!");
            }
            var request = new PutObjectRequest()
            {
                BucketName = _bucketName,
                Key = $"{GetCurrentUserPrefix()}/{file.FileName}",
                InputStream = file.OpenReadStream()
            };

			var fileRepository = new FileRepository(_context);
			fileRepository.AddNew(file, $"{GetCurrentUserPrefix()}/{file.FileName}");

			await _s3Client.PutObjectAsync(request);

            try
            {
                var fileOwnerhipRepository = new FileOwnershipRepository(_context);
                fileOwnerhipRepository.AddNew(currentUser, fileRepository.GetByName(file.FileName).Result);
            }
            catch (Exception)
            {
                return View("YourFiles");
            }
            return RedirectToAction("YourFiles", "File");
        }


        public async Task DownloadFileAsync(string s3FilePath, string localFilePath)
        {
            using (var transferUtility = new TransferUtility(_s3Client))
            {
               /* await transferUtility.DownloadAsync()*/
            }
        }

        public IActionResult YourFiles()
        {
            return View();
        }

        private string? GetCurrentUserPrefix()
        {
			var currentUser = HttpContext.User;
			var userEmail = currentUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return userEmail;
		}
    }
}
