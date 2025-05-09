using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Poject_F_Data_Acsses_Yalla_Enjaz;

namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {


        [HttpGet("GET_ALL_LOCATION", Name = "GET_ALL_LOCATION")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Branch_Serves_DTO> GET_ALL_LOCATION()
        {
            List<Location_DTO> list_Location = Businees_Location.GET_ALL_LOCATION();

            if (list_Location.Count != 0)
            {
                return Ok(list_Location);
            }
            else
                return NotFound("THE DATA NOT FOUND");
        }
    }
}
