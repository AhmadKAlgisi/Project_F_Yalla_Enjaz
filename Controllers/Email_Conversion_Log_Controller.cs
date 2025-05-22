using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Poject_F_Data_Acsses_Yalla_Enjaz;

namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Email_Conversion_Log_Controller : ControllerBase
    {


        [HttpPost("ADD_Email_Conversion_Log", Name = "ADD_Email_Conversion_Log")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Email_Conversion_Log_DTO> ADD_Email_Conversion_Log(Email_Conversion_Log_DTO Log)
        {
            if (string.IsNullOrEmpty(Log.OldEmail) || string.IsNullOrEmpty(Log.NewEmail) || string.IsNullOrEmpty(Log.Guide_image))
            {
                return BadRequest("Invalid person data.");
            }
            Businnes_Email_Conversion_Log B_log_email = new Businnes_Email_Conversion_Log(Log, Businnes_Email_Conversion_Log.enMode.AddNew);


            if (B_log_email.save())
            {
                Log.ID = B_log_email.ID;





                return CreatedAtRoute("ADD_Email_Conversion_Log", new { id = B_log_email.ID }, B_log_email.SDTO);
            }

            else
                return StatusCode(500, new { message = "ERROR: NOT COMPLETED DELETE OBJECT..." });


        }



    }
}
