using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Poject_F_Data_Acsses_Yalla_Enjaz;
using System.IO;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/Imege")]
    [ApiController]
    public class ImegeController : ControllerBase
    {

        private readonly Cloudinary _cloudinary;

        public ImegeController()
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

        [HttpPost("Upload_Imege_In_Servs_Student")]
        public async Task<IActionResult> UploadImage_in_server_student(IFormFile imageFile, int serves_id, int Imeg_Order)
        {
            if (imageFile == null || imageFile.Length == 0 || serves_id < 0 || Imeg_Order < 0)
                return BadRequest("No file uploaded.");

            // قراءة الصورة كـ Stream
            using var stream = imageFile.OpenReadStream();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageFile.FileName, stream),
                Folder = "student_images" // مجلد داخل Cloudinary (اختياري)
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);//اخذ الصورة والباث وايضا اسم الفولدر الي بدو يخزن عليه 

            if (uploadResult.Error != null)
                return StatusCode(500, new { Message = "Cloudinary Error: " + uploadResult.Error.Message });

            // أخذ الرابط النهائي للصورة
            string cloudinaryUrl = uploadResult.SecureUrl.ToString();//path end 

            // save in database  
            Imege_DTO imege = new Imege_DTO(0, cloudinaryUrl, serves_id, Imeg_Order);
            Businnes_Imege B_Imege = new Businnes_Imege(imege, Businnes_Imege.enMode.AddNew);



            if (B_Imege.save())
            {
                imege.ID = B_Imege.ID;
                return Ok(imege);
            }
            else
                return StatusCode(500, new { Message = "ERROR: Not saved to database." });
        }



        [HttpGet("GetImage")]
        public async Task<IActionResult> GetImage([FromQuery] string url)
        {
            if (string.IsNullOrEmpty(url))
                return BadRequest("Image URL is required.");

            try
            {
                // تحميل الصورة من الرابط
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return NotFound("Image not found on Cloudinary.");

                var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/octet-stream";
                var imageStream = await response.Content.ReadAsStreamAsync();

                return File(imageStream, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching image:"+ex.Message});
            }
        }



        [HttpGet("GET_ALL_IMEGES_BY_ID_SERVES {ID_Serves}",Name = "GET_ALL_IMEGES_BY_ID_SERVES")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Imege_DTO> GET_ALL_IMEGES_BY_ID_SERVES(int ID_Serves)
        {
            if(ID_Serves<1)
            {
                return BadRequest("Invalid Id Serves ....");
            }
            var ALL_Imege = Businnes_Imege.GET_ALL_IMEGES_BY_ID_SERVES(ID_Serves);

            if(ALL_Imege!=null)
            {
                if (ALL_Imege.Count != 0)
                    return Ok(ALL_Imege);
                else
                    return NotFound("لا يوجد صور لهذه الخدمة ");

            }
         
                return StatusCode(500, new { Message = "ERROR: server error ....." });





        }






        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        [HttpPut("Updata_imege")]
        public async Task<IActionResult> Updata_imege(int id_imege,IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0 || id_imege < 0 )
                return BadRequest("No file uploaded.");


            Businnes_Imege B_imege = Businnes_Imege.Get_Imeg_By_Id(id_imege);

            var uri = new Uri(B_imege.Imeg_Url);
            var publicId = Path.GetFileNameWithoutExtension(uri.AbsolutePath); // اسم الصورة بدون امتداد
            string folder = "student_images";
            string fullPublicId = $"{folder}/{publicId}";

            var deleteParams = new DeletionParams(fullPublicId);
            var deletionResult = await _cloudinary.DestroyAsync(deleteParams);

            if (deletionResult.Result != "ok" && deletionResult.Result != "not found")
                return StatusCode(500, new { Message = "Failed to delete old image from Cloudinary." });




            // قراءة الصورة كـ Stream
            using var stream = imageFile.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageFile.FileName, stream),
                Folder = folder
            };


            var uploadResult = await _cloudinary.UploadAsync(uploadParams);//اخذ الصورة والباث وايضا اسم الفولدر الي بدو يخزن عليه 

            if (uploadResult.Error != null)
                return StatusCode(500, new { Message = "Cloudinary Error: " + uploadResult.Error.Message });

            // أخذ الرابط النهائي للصورة
            string cloudinaryUrl = uploadResult.SecureUrl.ToString();//path end 

            // save in database  
            B_imege.Imeg_Url = cloudinaryUrl;


            if (B_imege.save())
            {
               
                return Ok(B_imege.SDTO);
            }
            else
                return StatusCode(500, new { Message = "ERROR: Not saved to database." });
        }




    }
}
