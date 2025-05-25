using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Poject_F_Data_Acsses_Yalla_Enjaz;

namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/Transaction")]
    [ApiController]
    public class TransactionController : ControllerBase
    {


        [HttpGet("GET_ALL_TRANSACTION_BY_ID_ORDERS{ID_order}", Name = "GET_ALL_TRANSACTION_BY_ID_ORDERS")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Branch_Serves_DTO> GET_ALL_TRANSACTION_BY_ID_ORDERS(int ID_order)
        {
            List<Transaction_DTO> Liast_Transaction = Businnes_Transaction.GET_ALL_TRANSACTION_BY_ID_ORDERS(ID_order);

            if (Liast_Transaction.Count != 0)
            {
                return Ok(Liast_Transaction);
            }
            else
                return NotFound("لا يوجد حركات دفع على هاد الطلب ");
        }

    }


}
