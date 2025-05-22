using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Poject_F_Data_Acsses_Yalla_Enjaz;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/Request_Order")]
    [ApiController]
    public class Request_Order_Controller : ControllerBase
    {

        [HttpPost("ADD_REQUEST_ORDER", Name = "ADD_REQUEST_ORDER")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task <ActionResult<Serves_Student_DTO>> ADD_REQUEST_ORDER(Request_Order_DTO requset_order)
        {
            if (string.IsNullOrEmpty(requset_order.Titel_serves) || string.IsNullOrEmpty(requset_order.Delivery_time) || string.IsNullOrEmpty(requset_order.Describtion_Serves) || requset_order.ID_branch_Serves < 0 || requset_order.ID_Name_Serves < 0 || requset_order.ID_state_Order < 0 || requset_order.ID_pesron_Presenter_Order < 0 || string.IsNullOrEmpty(requset_order.ID_Student_Service_provider))
            {
                return BadRequest("Invalid person data.");
            }
            Businees_Request_Order B_Requset_Order = new Businees_Request_Order(requset_order, Businees_Request_Order.enmode.ADDNEW);

            try
            {


                if (B_Requset_Order.save())
                {
                    requset_order.ID = B_Requset_Order.ID;


                    string data = B_Requset_Order.ID_Student_Service_provider;

                    
                    data = data.Trim('(', ')');
                    List<int> numbers_ID_Providers = data.Split(',')
                                            .Select(s => int.Parse(s))
                                            .ToList();

                    //هاي عشان يبعت hhdldghj
                    Businnes_Send_Email send_email = new Businnes_Send_Email();

                    foreach (var id_student in numbers_ID_Providers)
                    {//تجيب معلومات student 
                        INFO_FROM_STUDENT_UNVIRSTY_PERSON_USED_SHOW_SERVES_DTO Student = Businees_Student.GET_INFO_FROM_STUDENT_UNVIRSTY_PERSON_USED_SHOW_SERVES_BY_ID_STUDENT(id_student);
                        

                    string subject = "🔔 طلب خدمة جديد على منصة يلا إنجاز";

                    string body = $@"
مرحباً {Student.FullName} 👋   

لديك طلب خدمة جديد من أحد المستخدمين!

يرجى الدخول إلى حسابك على منصة ""يلا إنجاز"" لمراجعة التفاصيل والتواصل مع صاحب الطلب.

📅 تاريخ الطلب: {B_Requset_Order.Dtae_Order}  
🛠️ اسم الخدمة المطلوبة: {B_Requset_Order.Titel_serves};

بالتوفيق ✨  
فريق يلا إنجاز
";

                      
                    await  send_email.SendEmailAsync(Student.Email, subject, body);


                    }


                    return CreatedAtRoute("ADD_SERVESS_STUDENT", new { id = B_Requset_Order.ID }, requset_order);
                }

                else
                    return StatusCode(500, new { message = "ERROR: NOT COMPLETED DELETE OBJECT..." });

            }
            catch { return StatusCode(500, new { Message = "EROOR :لم يتم اضافة الطلب " }); }
        }



        [HttpGet("GET_LIST_ID_REQUEST_ORDER_BY_ID_STUDENT{id_student}", Name = "GET_LIST_ID_REQUEST_ORDER_BY_ID_STUDENT")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Name_Serves_DTO> GET_LIST_ID_REQUEST_ORDER_BY_ID_STUDENT(int id_student)
        {
            var list_request_order = Businees_Request_Order.GET_LIST_ID_REQUEST_ORDER_BY_ID_STUDENT(id_student);

            if (list_request_order.Count == 0)
            {
                return NotFound("لا يوجد طلبات حاليا لك .... ");
            }
            else
                return Ok(list_request_order);

        }




        [HttpGet("GET_INFO_REQUEST_ORDER_BY_ID_REQUEST_ORDER{id_requset_order}", Name = "GET_INFO_REQUEST_ORDER_BY_ID_REQUEST_ORDER")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<INFO_REQUEST_ORDER_DTO> GET_INFO_REQUEST_ORDER_BY_ID_REQUEST_ORDER(int id_requset_order)
        {

            if (id_requset_order < 1)
            {
                return BadRequest("ERROR: ENTER DATA NOT VAILD ....");
            }


            INFO_REQUEST_ORDER_DTO Request_order = Businees_Request_Order.GET_INFO_REQUEST_ORDER_BY_ID_REQUEST_ORDER(id_requset_order);

            if (Request_order != null)
            {
               

                return Ok(Request_order);
            }
            else
            {
                return NotFound("لا يوجد معلومات لهاذا الطلب ");
            }


        }



        [HttpGet("GET_LIST_ID_REQUEST_ORDER_BY_ID_Person{ID_PERSON}", Name = "GET_LIST_ID_REQUEST_ORDER_BY_ID_Person")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Name_Serves_DTO> GET_LIST_ID_REQUEST_ORDER_BY_ID_Person(int ID_PERSON)
        {
            var list_request_order = Businees_Request_Order.GET_LIST_ID_REQUEST_ORDER_BY_ID_Person(ID_PERSON);

            if (list_request_order.Count == 0)
            {
                return NotFound("لا يوجد طلبات حاليا لك .... ");
            }
            else
                return Ok(list_request_order);

        }




        [HttpPut("Update_Request_Order {ID_REQUEST_ORDER}", Name = "Update_Request_Order")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Person_DTO> Update_Request_Order(int ID_REQUEST_ORDER, Request_Order_DTO request)
        {

            if (string.IsNullOrEmpty(request.Titel_serves) || string.IsNullOrEmpty(request.Describtion_Serves) || string.IsNullOrEmpty(request.Delivery_time) || string.IsNullOrEmpty(request.ID_Student_Service_provider)||request.ID_Name_Serves<0||request.ID_pesron_Presenter_Order<0||request.ID_state_Order<0)
            {
                return BadRequest("Invalid Request Data.");
            }

          

          

            Businees_Request_Order request_order = Businees_Request_Order.GET_Request_Order(ID_REQUEST_ORDER);


            if (request_order != null)
            {
                request_order.Titel_serves = request.Titel_serves;
                request_order.Describtion_Serves = request.Describtion_Serves;
                request_order.Price = request.Price;
                request_order.Delivery_time = request.Delivery_time;
                request_order.Type_serves = request.Type_serves;
                request_order.ID_Location = request.ID_Location;
                request_order.ID_Files = request.ID_Files;





                if (request_order.save())

                    return Ok(request_order.SDTO);

                else
                {
                    return StatusCode(500, new { Message = "EROOR : NOT UBDATE DATA ...." });
                }

            }
            else
                return NotFound("Not Found Object ...");


        }




        [HttpGet("GET_INFO_REQUEST_ORDER/{id_requset_order}", Name = "GET_INFO_REQUEST_ORDER")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Request_Order_DTO> GET_INFO_REQUEST_ORDER(int id_requset_order)
        {

            if (id_requset_order < 1)
            {
                return BadRequest("ERROR: ENTER DATA NOT VAILD ....");
            }


            Businees_Request_Order req_order = Businees_Request_Order.GET_Request_Order(id_requset_order);

            if (req_order != null)
            {


                return Ok(req_order.SDTO);
            }
            else
            {
                return NotFound("لا يوجد معلومات لهاذا الطلب ");
            }


        }



        [HttpDelete("Delete_Request_Order {ID_Request_Order}", Name = "Delete_Request_Order")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //     [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string> Delete_Request_Order(int ID_Request_Order)
        {

            if (ID_Request_Order < 1)
            {
                return BadRequest($"THE ID {ID_Request_Order} Bad Data ...");
            }
            else
            {
                Businees_Request_Order request_Order = Businees_Request_Order.GET_Request_Order(ID_Request_Order);


                if (request_Order != null)
                {
                    if (request_Order.Delete_Request_Order(ID_Request_Order))
                        return Ok("تم حذف الطلب بنجاح  ");

                    else
                        return StatusCode(500, new { messege = "ERROR : لم يتم حذف الطلب راجع هل تم قبول الطلب " });

                }
                else
                    return NotFound("لا يوجد طلب في هاذا الرقم ");

            }


        }



    }

}
