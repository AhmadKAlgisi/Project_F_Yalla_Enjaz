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



        [HttpGet("GET_Knowing_the_outstanding_balance_in_the_system_By_Id_Order{ID_order}", Name = "GET_Knowing_the_outstanding_balance_in_the_system_By_Id_Order")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<double> GET_Knowing_the_outstanding_balance_in_the_system_By_Id_Order(int ID_order)
        {

            List<Transaction_DTO> Liast_Transaction = Businnes_Transaction.GET_ALL_TRANSACTION_BY_ID_ORDERS(ID_order);
            if (Liast_Transaction.Count != 0)
            {
                double? value = Businnes_Transaction.GET_Knowing_the_outstanding_balance_in_the_system_By_Id_Order(ID_order);

                if (value != null)
                {
                    return Ok(value);
                }
                else
                    return NotFound("لا يوجد اي عملية على الطلب");
            }
            else
                return NotFound("لا يوجد اي عملية على الطلب");

        }





        [HttpPost("ADD_TRANSACTION_IN_ADMAIN", Name = "ADD_TRANSACTION_IN_ADMAIN")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Rating_DTO> ADD_TRANSACTION_IN_ADMAIN(TRansaction_ADMIN_DTO Info_Transaction)
        {
            if (string.IsNullOrEmpty(Info_Transaction.Note) || Info_Transaction.AMMOUNT < 0 || Info_Transaction.into_transfar < 0|| Info_Transaction.ID_ORDERS<0)
            {
                return BadRequest("Invalid person data.");
            }


          
            if (Businnes_Transaction.ADD_TRANSACTION_IN_ADMAIN(Info_Transaction))
            {
              
                return Ok("تمت عملية التحويل بنجاح ");

            }
            else
                return StatusCode(500, new { Message = "EROOR : NOT UBDATE DATA ...." });


        }

    }


}
