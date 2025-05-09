using Businees_Logic_Project;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Poject_F_Data_Acsses_Yalla_Enjaz;

namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/Files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;

        public FilesController()
        {
            Account account = new Account(
                "dlazpanst",     // استبدلها بـ Cloud name من Cloudinary
                "413456772532198",        // استبدلها بـ API Key
                "IUAKhdveNw7Sz4SLWrmj2qMQBCk"      // استبدلها بـ API Secret
            );
            _cloudinary = new Cloudinary(account);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();

            var uploadParams = new RawUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "Files_Orders" // مجلد في Cloudinary - ممكن تغيره حسب الحاجة
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams); // رفع الملف

            if (uploadResult.Error != null)
                return StatusCode(500, new { Message = "Cloudinary Error: " + uploadResult.Error.Message });

            // رابط الملف المرفوع
            string cloudinaryUrl = uploadResult.SecureUrl.ToString();

           
            Files_DTO fileDTO = new Files_DTO(0, cloudinaryUrl);
            Businnes_Files B_File = new Businnes_Files(fileDTO, Businnes_Files.enMode.AddNew);

            if (B_File.save())
            {
                fileDTO.ID = B_File.ID;
                return Ok(fileDTO);
            }
            else
                return StatusCode(500, new { Message = "ERROR: Not saved to database." });
        }








    }
}
