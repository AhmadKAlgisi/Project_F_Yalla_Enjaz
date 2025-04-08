using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Poject_F_Data_Acsses_Yalla_Enjaz;
using System.IO;

namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/Imege")]
    [ApiController]
    public class ImegeController : ControllerBase
    {

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadImage_in_server_student(IFormFile imageFile,int serves_id,int Imeg_Order)
        {
            // Check if no file is uploaded
            if (imageFile == null || imageFile.Length == 0 || serves_id < 0 || Imeg_Order < 0)
                return BadRequest("No file uploaded.");

            var uploadDirectory = @"C:\Users\DELL\Downloads\Project_F_Yalla_Enjaz\All_Imege_In_project";

          
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadDirectory, fileName);

            if (!Directory.Exists(uploadDirectory))
            {
                Directory.CreateDirectory(uploadDirectory);
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);

                Imege_DTO imege = new Imege_DTO(0, filePath, serves_id, Imeg_Order);

                Businnes_Imege B_Imege = new Businnes_Imege(imege, Businnes_Imege.enMode.AddNew);

                imege.ID = B_Imege.ID;//update data 

                if (B_Imege.save())
                    return Ok(imege);
                else
                      return StatusCode(500, new { Message = "EROOR : NOT UPLOUDE IMEGE ...." });

            }

           
        }




        [HttpGet("GetImage/{fileName}")]
        public IActionResult GetImage(string fileName)
        {
            // Directory where files are stored
            var uploadDirectory = @"C:\Users\DELL\Downloads\Project_F_Yalla_Enjaz\All_Imege_In_project";
            var filePath = Path.Combine(uploadDirectory, fileName);

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
                return NotFound("Image not found.");

            // Open the image file for reading
            var image = System.IO.File.OpenRead(filePath);
            var mimeType = GetMimeType(filePath);

            // Return the file with the correct MIME type
            return File(image, mimeType);
        }

        // Helper method to get the MIME type based on file extension
        /*
         This code defines a  method called GetMimeType that takes a file path as a parameter 
         and returns the corresponding MIME type as a string. 
         MIME types are used to indicate the nature and format of a file, 
         especially in web contexts where you need to specify the type of content you're sending, 
         like images, text, etc.

        MIME type stands for Multipurpose Internet Mail Extensions type. 
        It's a standard way to indicate the nature and format of a file or content. 
        MIME types are used to tell browsers, email clients, and 
        other software about the type of data they're handling, so they can process it correctly.
         */
        private string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream",
            };
        }



    }
}
