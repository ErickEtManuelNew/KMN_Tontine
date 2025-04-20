using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace KMN_Tontine.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class FileSystemController : ControllerBase
    {
        [HttpGet("list")]
        public IActionResult ListFiles([FromQuery] string path = "/data")
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    return NotFound($"Le répertoire {path} n'existe pas");
                }

                var files = Directory.GetFiles(path)
                    .Select(file => new
                    {
                        Name = Path.GetFileName(file),
                        Path = file,
                        Size = new FileInfo(file).Length,
                        LastModified = System.IO.File.GetLastWriteTime(file)
                    });

                var directories = Directory.GetDirectories(path)
                    .Select(dir => new
                    {
                        Name = Path.GetFileName(dir),
                        Path = dir,
                        IsDirectory = true,
                        LastModified = Directory.GetLastWriteTime(dir)
                    });

                var result = new
                {
                    CurrentPath = path,
                    Files = files,
                    Directories = directories
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors de la lecture du répertoire : {ex.Message}");
            }
        }

        [HttpGet("download")]
        public IActionResult DownloadFile([FromQuery] string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    return BadRequest("Le chemin du fichier est requis");
                }

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound($"Le fichier {filePath} n'existe pas");
                }

                var fileInfo = new FileInfo(filePath);
                var contentType = GetContentType(fileInfo.Extension);

                return PhysicalFile(filePath, contentType, fileInfo.Name);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erreur lors du téléchargement du fichier : {ex.Message}");
            }
        }

        private string GetContentType(string extension)
        {
            return extension.ToLower() switch
            {
                ".pdf" => "application/pdf",
                ".txt" => "text/plain",
                ".csv" => "text/csv",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".zip" => "application/zip",
                _ => "application/octet-stream"
            };
        }
    }
} 