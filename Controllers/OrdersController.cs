using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Poject_F_Data_Acsses_Yalla_Enjaz;
using System.Security.Cryptography.X509Certificates;

namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/Orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        [HttpPost("Accept_Request_orders_And_Add_Orders{ID_Request_Orders},{ID_Student_Provider}", Name = "Accept_Request_orders_And_Add_Orders")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Serves_Student_DTO>> Accept_Request_orders_And_Add_Orders(int ID_Request_Orders,int ID_Student_Provider)
        {
            if(ID_Request_Orders<1|| ID_Student_Provider<1)
            {
                return BadRequest("Invalid person data.");
            }
         

            try
            {
                INFO_REQUEST_ORDER_DTO request_order = Businees_Request_Order.GET_INFO_REQUEST_ORDER_BY_ID_REQUEST_ORDER(ID_Request_Orders);



                if (request_order != null)
                {
                    int id_orders = Businees_Orders.Accept_Request_orders_And_Add_Orders(ID_Request_Orders, ID_Student_Provider);
                    if (id_orders != -1)
                    {
                        Complete_Bument_Order complete_orders = new Complete_Bument_Order(id_orders, request_order.ID_pesron_Presenter_Order, ID_Student_Provider, request_order.Price);
                        Businees_Orders.Complete_Buyment_Order_from_Accept_Orders(complete_orders);


                        //هاي عشان يبعت hhdldghj
                        YallaEnjazMailer send_email = new YallaEnjazMailer();

                        Business_Person person_present_order = Business_Person.GET_PERSON_BY_ID(request_order.ID_pesron_Presenter_Order);

                        INFO_FROM_STUDENT_UNVIRSTY_PERSON_USED_SHOW_SERVES_DTO Student = Businees_Student.GET_INFO_FROM_STUDENT_UNVIRSTY_PERSON_USED_SHOW_SERVES_BY_ID_STUDENT(ID_Student_Provider);

                        string subject = "🔔 قبول طلبك على منصة يلا إنجاز";

                        string body = $@"
مرحباً {person_present_order.F_name} {person_present_order.L_name} 👋،

تم قبول طلبك على منصة <b>يلا إنجاز</b> بعنوان: <b>{request_order.Titel_serves}</b>  
من قِبل مقدم الخدمة: <b>{Student.FullName}</b>.

💳 تم خصم مبلغ <b style='color:#2d89ef;'>{request_order.Price} دينار</b> من بطاقتك الافتراضية، وسيتم تحويل نصف المبلغ إلى مقدم الخدمة الآن، والنصف الآخر بعد اكتمال الخدمة دون وجود شكوى.

📅 تاريخ الطلب: {DateTime.Now:yyyy-MM-dd HH:mm}  

يرجى تسجيل الدخول إلى حسابك على المنصة لمراجعة تفاصيل الطلب والتواصل مع مقدم الخدمة.

بالتوفيق ✨  
فريق <b>يلا إنجاز</b>
";





                        await send_email.SendEmailAsync(person_present_order.Email, subject, body);

                        return Ok("تم اضافة الطلب بنجاح");

                    }
                    else
                    {
                        return StatusCode(500, new { message = "ERROR: NOT COMPLETED DELETE OBJECT..." });
                    }
                }

               




                else
                    return StatusCode(500, new { message = "ERROR: NOT COMPLETED DELETE OBJECT..." });

            }

            catch { return StatusCode(500, new { Message = "EROOR :لم يتم اضافة الطلب " }); }
        }




        [HttpGet("GET_INFO_ORDER_BY_ID_ORDER{ID_Order}", Name = "GET_INFO_ORDER_BY_ID_ORDER")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<INFO_ORDER_DTO> GET_INFO_ORDER_BY_ID_ORDER(int ID_Order)
        {

            if (ID_Order < 1)
            {
                return BadRequest("ERROR: ENTER DATA NOT VAILD ....");
            }


            INFO_ORDER_DTO order = Businees_Orders.GET_INFO_ORDER_BY_ID_ORDER(ID_Order);

            if (order != null)
            {


                return Ok(order);
            }
            else
            {
                return NotFound("لا يوجد معلومات لهاذا الطلب ");
            }


        }



        [HttpGet("GET_LIST_ID_ORDER_complemnt_BY_ID_STUDENT{id_student}", Name = "GET_LIST_ID_ORDER_complemnt_BY_ID_STUDENT")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Name_Serves_DTO> GET_LIST_ID_ORDER_complemnt_BY_ID_STUDENT(int id_student)
        {
            var list_order = Businees_Orders.GET_LIST_ID_ORDER_Complement_BY_ID_STUDENT(id_student);

            if (list_order.Count == 0)
            {
                return NotFound("لا يوجد طلبات حاليا لك .... ");
            }
            else
                return Ok(list_order);

        }





        [HttpPut("Update_Order_and_Enrool_Data_Complete_Order{ID_Order}", Name = "Update_Order_and_Enrool_Data_Complete_Order")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task <ActionResult<string>> Update_Order_and_Enrool_Data_Complete_Order(int ID_Order)
        {

            if (ID_Order < 1)
            {
                return BadRequest("ERROR: ENTER DATA NOT VAILD ....");
            }


           

            if (Businees_Orders.Update_Order_and_Enrool_Data_Complete_Order(ID_Order))
            {
                INFO_ORDER_DTO orders= Businees_Orders.GET_INFO_ORDER_BY_ID_ORDER(ID_Order);

                string email = Business_Person.GET_Email_By_id_person(orders.ID_pesron_Presenter_Order);
                string subject = "تم إنهاء طلبك بنجاح";

                string body =
                @"مرحبًا،

نود إعلامك بأنه قد تم إنهاء الخدمة المتعلقة بطلبك بنجاح.

شكرًا لاستخدامك منصتنا (يلا إنجاز). في حال وجود أي استفسار أو ملاحظة، لا تتردد في التواصل معنا.

مع التحية،
فريق يلا إنجاز";


                YallaEnjazMailer send_email = new YallaEnjazMailer();
                await send_email.SendEmailAsync(email, subject, body);

                return Ok("تم إنهاء الخدمة بنجاح");

            }
            else
            {
           

                return StatusCode(500, new { messege = "ERROR : خطا في النظام يجب عليك انهاء خدمتك في وقت لاحق" });
                }


        }




        [HttpGet("GET_LIST_ID_ORDERS_In_progress_BY_ID_STUDENT{ID_student}", Name = "GET_LIST_ID_ORDERS_In_progress_BY_ID_STUDENT")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Name_Serves_DTO> GET_LIST_ID_ORDERS_In_progress_BY_ID_STUDENT(int ID_student)
        {
            var list_request_order = Businees_Orders.GET_LIST_ID_ORDERS_In_progress_BY_ID_STUDENT(ID_student);

            if (list_request_order.Count == 0)
            {
                return NotFound("لا يوجد طلبات حاليا لك .... ");
            }
            else
                return Ok(list_request_order);

        }





        [HttpGet("GET_LIST_ID_ORDERS_In_PROGGRES_BY_ID_Person{ID_Person}", Name = "GET_LIST_ID_ORDERS_In_PROGGRES_BY_ID_Person")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Name_Serves_DTO> GET_LIST_ID_ORDERS_In_PROGGRES_BY_ID_Person(int ID_Person)
        {
            var list_order = Businees_Orders.GET_LIST_ID_ORDERS_In_PROGGRES_BY_ID_Person(ID_Person);

            if (list_order.Count == 0)
            {
                return NotFound("لا يوجد طلبات حاليا لك .... ");
            }
            else
                return Ok(list_order);

        }




        [HttpGet("GET_LIST_ID_ORDER_complemnt_BY_ID_PERSON{iD_Person}", Name = "GET_LIST_ID_ORDER_complemnt_BY_ID_PERSON")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Name_Serves_DTO> GET_LIST_ID_ORDER_complemnt_BY_ID_PERSON(int iD_Person)
        {
            var list_order = Businees_Orders.GET_LIST_ID_ORDER_Complement_BY_ID_PERSON(iD_Person);

            if (list_order.Count == 0)
            {
                return NotFound("لا يوجد طلبات حاليا لك .... ");
            }
            else
                return Ok(list_order);

        }





    }
}
