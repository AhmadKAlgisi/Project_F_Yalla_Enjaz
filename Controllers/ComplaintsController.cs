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




        [HttpPost("Add_Complaints", Name = "Add_Complaints")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Rating_DTO> Add_Complaints(Complaints_DTO Complaint)
        {
            if (string.IsNullOrEmpty(Complaint.Description) || Complaint.Orders_ID< 0)
            {
                return BadRequest("Invalid person data.");
            }


            Businees_Complaints comple = new Businees_Complaints(Complaint, Businees_Complaints.enmode.ADDNEW);
            if (comple.save())
            {
                Complaint.ID = comple.ID;
                Complaint.ID_stute = 18;
                comple.ID_stute = 18;
                return Ok(comple.SDTO);
            }
            else
                return StatusCode(500, new { Message = "EROOR : NOT UBDATE DATA ...." });


        }




        [HttpGet("GET_ID_ORDERS_FROM_EXITE_COMPLEMENT", Name = "GET_ID_ORDERS_FROM_EXITE_COMPLEMENT")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<int> GET_ID_ORDERS_FROM_EXITE_COMPLEMENT()
        {
            var list_ID_orders = Businees_Complaints.GET_ID_ORDERS_FROM_EXITE_COMPLEMENT();

            if (list_ID_orders.Count == 0)
            {
                return NotFound("لا يوجد اي طلب معلقة حاليا .....");
            }
            else
                return Ok(list_ID_orders);

        }



        [HttpGet("End_the_complaint{ID_Order}", Name = "End_the_complaint")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Complaints_QUERE_DTO> End_the_complaint(int ID_Order)
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

                {
                    if(Businees_Complaints.End_the_complaint(ID_Order))
                    {
                        return Ok("تم انهاء الشكوى بنجاح");
                    }
                    else 
                    { 
                       return StatusCode(500, new { Message = "EROOR :Server Error" });
                    }
                }
            }
            catch { return StatusCode(500, new { Message = "EROOR :Server Error" }); }
        }



    }
}
