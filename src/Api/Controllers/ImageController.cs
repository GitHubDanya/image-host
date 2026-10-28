using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace image_host.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ImageController : ControllerBase
{
    private readonly string _storagePath;

    public ImageController(IWebHostEnvironment environment)
    {
        _storagePath = Path.Combine(environment.ContentRootPath, "ImageStorage");
    }
    
    [HttpGet("{path}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileResult))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFromPath(string path)
    {
        string safePath = Path.GetFileName(path);
        string filePath = Path.Combine(_storagePath, safePath);
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound(new { message = "not found" });
        }

        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(safePath, out string? contentType))
        {
            // default to raw stream of 8-bit bytes
            contentType = "application/octet-stream";
        }

        return PhysicalFile(filePath, contentType, enableRangeProcessing: true);
    }
}