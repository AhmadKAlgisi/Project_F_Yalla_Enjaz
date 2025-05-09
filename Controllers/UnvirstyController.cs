using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Poject_F_Data_Acsses_Yalla_Enjaz;

namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/Unvirsty")]
    [ApiController]
    public class UnvirstyController : ControllerBase
    {

        [HttpGet("GET_ALL_UNVIRSTY_BY_ID_LOCATION", Name = "GET_ALL_UNVIRSTY_BY_ID_LOCATION")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Branch_Serves_DTO> GET_ALL_UNVIRSTY_BY_ID_LOCATION(int id_location)
        {
            if(id_location<0)
            {
                return BadRequest("id not vaild...");
            }
            List<UNVIRSTY_DTO> list_Unvirsty = Businees_Unvirsty.GET_ALL_UNVIRSTY_BY_ID_LOCATION(id_location);

            if (list_Unvirsty.Count != 0)
            {
                return Ok(list_Unvirsty);
            }
            else
                return NotFound("THE DATA NOT FOUND");
        }


        [HttpGet("GET_ALL_UNVIRSTY", Name = "GET_ALL_UNVIRSTY")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Branch_Serves_DTO> GET_ALL_UNVIRSTY()
        {

            List<UNVIRSTY_DTO> list_Unvirsty = Businees_Unvirsty.GET_ALL_UNVIRSTY();

            if (list_Unvirsty.Count != 0)
            {
                return Ok(list_Unvirsty);
            }
            else
                return NotFound("THE DATA NOT FOUND");
        }


    }
}
