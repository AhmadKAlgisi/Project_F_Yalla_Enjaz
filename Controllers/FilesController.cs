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



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("UploadFile_from_connection")]
        public async Task<IActionResult> UploadFile_from_connection(IFormFile file, int ID_order, string? Description=null)
        {
            if (file == null || file.Length == 0|| ID_order<0)
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

            Files_ADD_DTO file_dto = new Files_ADD_DTO(0, cloudinaryUrl, Description, ID_order);
            Businnes_Files B_File = new Businnes_Files(file_dto, Businnes_Files.enMode.AddNew);

            if (B_File.ADD_Files(file_dto))
            {
                file_dto.ID = B_File.ID;
                return Ok(file_dto);
            }
            else
                return StatusCode(500, new { Message = "ERROR: Not saved to database." });
        }




        [HttpGet("GET_ALL_GET_ALL_FILES_BY_ID_ORDERS{ID_Order}", Name = "GET_ALL_GET_ALL_FILES_BY_ID_ORDERS")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Branch_Serves_DTO> GET_ALL_GET_ALL_FILES_BY_ID_ORDERS(int ID_Order)
        {
            List<Files_ADD_DTO> list_File = Businnes_Files.GET_ALL_GET_ALL_FILES_BY_ID_ORDERS(ID_Order);

            if (list_File.Count != 0)
            {
                return Ok(list_File);
            }
            else
                return NotFound("THE DATA NOT FOUND");
        }







    }
}
