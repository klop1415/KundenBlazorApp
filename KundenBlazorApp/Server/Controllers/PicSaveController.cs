using KundenBlazorApp.Server.Data;
using KundenBlazorApp.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace KundenBlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicSaveController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment env;
        private readonly ILogger<PicSaveController> logger;

        public PicSaveController(IWebHostEnvironment env,
            ApplicationDbContext context,
            ILogger<PicSaveController> logger)
        {
            this.context = context;
            this.env = env;
            this.logger = logger;

        }

        [HttpPost]
        public async Task<ActionResult<IList<UploadResult>>> PostFile(
            [FromForm] IEnumerable<IFormFile> files)
        {
            var resourcePath = new Uri($"{Request.Scheme}://{Request.Host}/");

            List<UploadResult> uploadResults = await SavePics(files);

            return new CreatedResult(resourcePath, uploadResults);
        }

        string dir = "files";
        async Task<List<UploadResult>> SavePics(IEnumerable<IFormFile> files)
        {
            var maxAllowedFiles = 3;
            long maxFileSize = UploadResult.FileMaxSize;
            var filesProcessed = 0;

            List<UploadResult> uploadResults = new();
            foreach (var file in files)
            {
                var uploadResult = new UploadResult();
                string trustedFileNameForFileStorage;
                var untrustedFileName = file.FileName;
                uploadResult.FileName = untrustedFileName;
                var trustedFileNameForDisplay =
                    WebUtility.HtmlEncode(untrustedFileName);

                if (filesProcessed < maxAllowedFiles)
                {
                    if (file.Length == 0)
                    {
                        logger.LogInformation("{FileName} length is 0 (Err: 1)",
                            trustedFileNameForDisplay);
                        uploadResult.ErrorCode = 1;
                    }
                    else if (file.Length > maxFileSize)
                    {
                        logger.LogInformation("{FileName} of {Length} bytes is " +
                            "larger than the limit of {Limit} bytes (Err: 2)",
                            trustedFileNameForDisplay, file.Length, maxFileSize);
                        uploadResult.ErrorCode = 2;
                    }
                    else
                    {
                        try
                        {
                            /*                            var path = Path.Combine(env.ContentRootPath,
                                                            env.EnvironmentName, "unsafe_uploads",
                                                            trustedFileNameForFileStorage);
                            */
                            trustedFileNameForFileStorage =
                                Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".png"; // important!!!
                            var path1 = Path.Combine(dir, trustedFileNameForFileStorage);
                            var path = Path.Combine(env.WebRootPath, path1);

                            await using FileStream fs = new(path, FileMode.Create);
                            await file.CopyToAsync(fs);

                            logger.LogInformation("{FileName} saved at {Path}", path1, path);
                            uploadResult.Uploaded = true;
                            uploadResult.StoredFileName = path1;
                        }
                        catch (IOException ex)
                        {
                            logger.LogError("{FileName} error on upload (Err: 3): {Message}",
                                trustedFileNameForDisplay, ex.Message);
                            uploadResult.ErrorCode = 3;
                        }
                    }
                    filesProcessed++;
                }
                else
                {
                    logger.LogInformation("{FileName} not uploaded because the " +
                        "request exceeded the allowed {Count} of files (Err: 4)",
                        trustedFileNameForDisplay, maxAllowedFiles);
                    uploadResult.ErrorCode = 4;
                }
                uploadResults.Add(uploadResult);
            }
            return uploadResults;
        }
    }
}
