using Microsoft.AspNetCore.Mvc;

namespace FileShare.Controllers
{
    public class UploadController : Controller
    {
        private readonly IWebHostEnvironment environment;
        public UploadController(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        [Route("api/upload/file")]
        public async Task<IActionResult> UploadDocument(IFormFile file)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var data2 = memoryStream.ToArray();

                    var filePath = Path.Combine(environment.WebRootPath + @"\share", file.FileName);
                    using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(data2, 0, data2.Length);
                        fs.Close();
                    }
                };

                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
