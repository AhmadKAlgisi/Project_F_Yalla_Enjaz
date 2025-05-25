using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Poject_F_Data_Acsses_Yalla_Enjaz;

namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuymentController : ControllerBase
    {

        [HttpGet("GET_INFO_Buyment_By_ID_Orders{ID_Orders}", Name = "GET_INFO_Buyment_By_ID_Orders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Person_DTO> GET_INFO_Buyment_By_ID_Orders(int ID_Orders)
        {

            if (ID_Orders < 1)
            {
                return BadRequest("ERROR: enter data.... ");
            }


            Buyment_DTO info_bument = Businees_Buyment.GET_INFO_Buyment_By_ID_Orders(ID_Orders);





            if (info_bument != null)
            { 
                return Ok(info_bument);
            }
            else
            {
                return NotFound("لا يوجد تفاصيل للدغع عن الطلب الان ");
            }




        }




    }
}
