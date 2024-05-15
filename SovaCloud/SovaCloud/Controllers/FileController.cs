using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
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
            await _s3Client.PutObjectAsync(request);

			var fileRepository = new FileRepository(_context);
			try
            {
                var userRepository = new UserRepository(_context);
                var currentUser = userRepository.GetByEmail(GetCurrentUserPrefix()).Result;

                
                fileRepository.AddNew(file, $"{GetCurrentUserPrefix()}/{file.FileName}");

                await Task.Delay(700);

                var currentFile = fileRepository.GetByName(file.FileName).Result;

                await Task.Delay(700);

                var fileOwnerhipRepository = new FileOwnershipRepository(_context);
                fileOwnerhipRepository.AddNew(currentUser, currentFile);
            }
            catch (Exception)
            {
                return RedirectToAction("YourFiles", "File");
            }
			var fileList = fileRepository.GetListByEmail(GetCurrentUserPrefix()).Result;

			return RedirectToAction("YourFiles", "File");
        }

        [HttpPost]
        public async Task<IActionResult> DownloadFile([FromForm] string fileName)
        {
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = $"{GetCurrentUserPrefix()}/{fileName}"
                };

                using GetObjectResponse responce = await _s3Client.GetObjectAsync(request);

                await responce.WriteResponseStreamToFileAsync(
                    $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\{fileName}", true, CancellationToken.None);
            }
            catch (Exception)
            {
                return RedirectToAction("YourFiles", "File");
            }
            return RedirectToAction("YourFiles", "File");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFile([FromForm] string fileName)
        {
            try
            {
                var request = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = $"{GetCurrentUserPrefix()}/{fileName}"
				};

                await _s3Client.DeleteObjectAsync(request);

                var filesRepository = new FileRepository(_context);
                filesRepository.DeleteOne($"{GetCurrentUserPrefix()}/{fileName}");

                await Task.Delay(700);
            }
            catch (Exception)
            {
                return RedirectToAction("YourFiles", "File");
            }
            return RedirectToAction("YourFiles", "File");
        }

        [Authorize]
        public IActionResult YourFiles()
        {
            var fileRepository = new FileRepository(_context);
            var fileList = fileRepository.GetListByEmail(GetCurrentUserPrefix()).Result;

            return View("YourFiles", fileList);
        }

        private string? GetCurrentUserPrefix()
        {
			var currentUser = HttpContext.User;
			var userEmail = currentUser.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return userEmail;
		}
    }
}
