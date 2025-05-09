using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Poject_F_Data_Acsses_Yalla_Enjaz;

namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/Complaints")]
    [ApiController]
    public class ComplaintsController : ControllerBase
    {
        [HttpGet("GET_Complaints_BY_ID_ORDERS{ID_Order}", Name = "GET_Complaints_BY_ID_ORDERS")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Complaints_QUERE_DTO> GET_Complaints_BY_ID_ORDERS(int ID_Order)
        {
            if (ID_Order < 0)
                return BadRequest("ID غير صالح");
            try
            {
                Complaints_QUERE_DTO complaints = Businees_Complaints.Get_Complaints_By_ID_Order(ID_Order);

                if (complaints == null)
                {
                    return NotFound("لا يوجد اي شكاوي على الطلب متاحة  ");
                }
                else
                    return Ok(complaints);
            }
            catch { return StatusCode(500, new { Message = "EROOR :Server Error" }); }
        }



    }
}
