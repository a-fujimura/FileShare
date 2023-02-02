using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace FileShare.Controllers
{
    public class DownloadController : Controller
    {
        private readonly IWebHostEnvironment environment;

        public DownloadController(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        [Route("api/getfiles")]
        public IActionResult GetFiles()
        {
            try
            {
                var rslt = new List<string>();
                var files = Directory.GetFiles(Path.Combine(environment.WebRootPath + @"\share"));
                foreach (var i in files)
                {
                    rslt.Add(Path.GetFileName(i));
                }
                return Content(JsonSerializer.Serialize(rslt.ToArray()), "application/json", Encoding.UTF8);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [Route("api/download/file")]
        public async Task<IActionResult> DownloadDocument(IFormFile file)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    byte[] buffer = memoryStream.ToArray();
                    var fileName = JsonSerializer.Deserialize<string>(Encoding.ASCII.GetString(buffer));
                    var filePath = Path.Combine(environment.WebRootPath + @"\share", fileName);

                    return File(System.IO.File.ReadAllBytes(filePath), "application/octet-stream", fileName);
                };

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
