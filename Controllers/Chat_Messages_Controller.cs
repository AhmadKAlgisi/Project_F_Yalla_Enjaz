using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Poject_F_Data_Acsses_Yalla_Enjaz;

namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/Chat_Messages")]
    [ApiController]
    public class Chat_Messages_Controller : ControllerBase
    {



        [HttpGet("Get_All_Message_By_Id_Order{ID_ORDER}", Name = "Get_All_Message_By_Id_Order")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Messages_Quere_DTO> Get_All_Message_By_Id_Order(int ID_ORDER)
        {
            List<Messages_Quere_DTO> list_meseege = Businnes_Chat_Messages.Get_All_Message_By_Id_Order(ID_ORDER);

            if (list_meseege.Count != 0)
            {
                return Ok(list_meseege);
            }
            else
                return NotFound("لا يوجد رسائل حاليا");
        }


        [HttpPost("ADD_message",Name = "ADD_message")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult<Chat_Messages_DTO> ADD_message(Chat_Messages_DTO message)
        {
            if(string.IsNullOrEmpty(message.Message_Text)||message.ID_Serves_Provider<0||message.ID_Order<0||message.ID_Person_Presnter<0||message.ID_Serves_Provider==message.ID_Person_Presnter||message.Sender_ID<0)
            {
                return BadRequest("خطاء في ادخال البيانات ");
            }
            try
            {
                //DEPUNSE INJECTION 
                Businnes_Chat_Messages B_message = new Businnes_Chat_Messages(message, Businnes_Chat_Messages.enmode.ADDNEW);

                if (B_message.Save())
                {
                    message.ID = B_message.ID;
                    return CreatedAtRoute("ADD_message", new { id = B_message.ID }, message);

                }
                else
                    return StatusCode(500, new { message = "مشكلة تقنية في الخادم" });
            }
            catch
            {
                return StatusCode(500, new { message = "مشكلة تقنية في الخادم" });
            }


        }
    }
}
