using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Poject_F_Data_Acsses_Yalla_Enjaz;

namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/ Name_Serves")]
    [ApiController]
    public class Name_Serves_Controller : ControllerBase
    {
        [HttpGet("GET_ALL_NAME_SERVES",Name = "GET_ALL_NAME_SERVES")]
        [ProducesResponseType (StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Name_Serves_DTO> GetAllServesName()
        {
            var Serves_NameList = Businees_Logic_Project.Businees_Name_Serves.GetAllServesName();

            if (Serves_NameList.Count == 0)
            {
                return NotFound("No Serves Found ");
            }
            else
                return Ok(Serves_NameList);

        }





        [HttpGet("GET_SERVES {id}", Name = "GET_SERVES By ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public ActionResult<Name_Serves_DTO> GetServesNameById(int id)
        {

            if(id<0)
            {
                return BadRequest("The entered data is incorrect");
            }

            Businees_Name_Serves serves = Businees_Logic_Project.Businees_Name_Serves.GetServesNameById(id);

            if (serves != null)
            {
                return Ok(serves);
            }
            else
                return NotFound("The ID Not Found ");


        }






    }
}
