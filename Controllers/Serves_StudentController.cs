using Businees_Logic_Project;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Poject_F_Data_Acsses_Yalla_Enjaz;
using System.Linq.Expressions;

namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/Serves_Student")]
    [ApiController]
    public class Serves_StudentController : ControllerBase
    {

       

      



        [HttpGet("GET_Serves_Student_By_Id_Student{id}", Name = "GET_Serves_Student_By_Id_Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Serves_Student_DTO> GET_Serves_Student_By_Id_Student(int id)
        {

            if (id < 1)
            {
                return BadRequest("ERROR: enter data ");
            }


            Businees_Serves_Student serves_student = Businees_Serves_Student.GET_Serves_Student_By_Id_Student(id);

            if (serves_student != null)
            {
                Serves_Student_DTO DTO = serves_student.SDTO;

                return Ok(DTO);
            }
            else
            {
                return NotFound("Data Not Found ....");
            }


        }



        [HttpPost("ADD_SERVESS_STUDENT", Name = "ADD_SERVESS_STUDENT")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Serves_Student_DTO> ADD_SERVESS_STUDENT(Serves_Student_DTO serves_student)
        {
            //if (string.IsNullOrEmpty(person.F_name) || string.IsNullOrEmpty(person.L_name) || string.IsNullOrEmpty(person.Email) || string.IsNullOrEmpty(person.Password))
            //{
            //    return BadRequest("Invalid person data.");
            //}
            Businees_Serves_Student B_serves = new Businees_Serves_Student(serves_student, Businees_Serves_Student.enmode.ADDNEW);


            if (B_serves.save())
            {
                serves_student.ID = B_serves.ID;





                return CreatedAtRoute("ADD_SERVESS_STUDENT", new { id = B_serves.ID }, serves_student);
            }

            else
                return StatusCode(500, new { message = "ERROR: NOT COMPLETED DELETE OBJECT..." });


        }



        [HttpGet("Get_ALL_INFO_SAMPLE_STUDENT_PROVEDER_BRANCH_SERVER", Name = "Get_ALL_INFO_SAMPLE_STUDENT_PROVEDER_BRANCH_SERVER")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Semple_Cradte_Info_ON_Server_Student_By_Pranch_Server> Get_ALL_INFO_SAMPLE_STUDENT_PROVEDER_BRANCH_SERVER(int id_branch_serves)
        {
            var ALL_Info_Student = Businees_Serves_Student.Get_ALL_INFO_SAMPLE_STUDENT_PROVEDER_BRANCH_SERVER(id_branch_serves);

            if (ALL_Info_Student.Count!=0)
            {
                return Ok(ALL_Info_Student);
            }
            else
                return NotFound("لا يوجد مقدمين للخدمة متاحين ");
        }






        [HttpPut("UPDATE_Serves_Student{id_student}", Name = "UPDATE_Serves_Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Serves_Student_DTO> UPDATE_Serves_Student(int id_student, Serves_Student_DTO serves)
        {

            if (id_student < 1 || string.IsNullOrEmpty(serves.Service_Address) || serves.SERVES_ID<1|| serves.Branch_Server_Id < 1 || serves.price < 1 || string.IsNullOrEmpty(serves.Service_Description) || string.IsNullOrEmpty(serves.Service_Features) || string.IsNullOrEmpty(serves.Number_Phone) || string.IsNullOrEmpty(serves.Description_works)||serves.ID_Statue_Serves<1||serves.ID_Student<1)
            {
                return BadRequest("Invalid Serves Student .....");
            }



            Businees_Serves_Student B_serves = Businees_Serves_Student.GET_Serves_Student_By_Id_Student(id_student);

            if (B_serves != null)
            {
                //ubdate in the inside Memorey
                //ID NOT ALLOW CHANGE .....
                B_serves.Service_Address = serves.Service_Address;
                B_serves.SERVES_ID = serves.SERVES_ID;
                B_serves.Branch_Server_Id = serves.Branch_Server_Id;
                B_serves.price = serves.price;
                B_serves.Service_Description = serves.Service_Description;
                B_serves.Service_Features = serves.Service_Features;
                B_serves.Preview_link = serves.Preview_link;
                B_serves.Number_Phone = serves.Number_Phone;
                B_serves.Description_works = serves.Description_works;
                B_serves.ID_Statue_Serves = serves.ID_Statue_Serves;
              // not ubdate id_student and data_enrool and id_serves لا يتم تحديثهم 





                //ubdate in the data base 
                if (B_serves.save())

                    return Ok(B_serves.SDTO);

                else
                {
                    return StatusCode(500, new { Message = "EROOR : NOT UBDATE DATA ...." });
                }

            }
            else
                return NotFound("لا يوجد خدمة لعرضها ......");


        }



        [HttpDelete("Delete_Serves_Student {id_student}", Name = "Delete_Serves_Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string> Delete_Serves_Student(int id_student)
        {

            if (id_student < 1)
            {
                return BadRequest($"THE ID {id_student} Bad Data ...");
            }
            else
            {
                Businees_Serves_Student B_serves = Businees_Serves_Student.GET_Serves_Student_By_Id_Student(id_student);


                if (B_serves != null)
                {

                        if (B_serves.Delete_Serves_Student(id_student))
                            return Ok($"The Person {id_student} Completed Delete ... ");

                        else
                            return StatusCode(500, new { messege = "ERROR : Not Completed Delete ...." });

                   



                }
                else
                    return NotFound($"The Id {id_student} Not Found...");

            }


        }



        [HttpGet("GET_SAMPLE_INFO_FROM_SERVER_STUDENT_BY_ID_Branch_serves_and_ID_Unvirsty_For_modern_services", Name = "GET_SAMPLE_INFO_FROM_SERVER_STUDENT_BY_ID_Branch_serves_and_ID_Unvirsty_For_modern_services")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Semple_Cradte_Info_ON_Server_Student_By_Pranch_Server> GET_SAMPLE_INFO_FROM_SERVER_STUDENT_BY_ID_Branch_serves_and_ID_Unvirsty_For_modern_services(int id_branch_serves,int id_unvirsty)
        {
            if (id_branch_serves < 0 || id_unvirsty < 0)
                return BadRequest("Invaild data input ...");

            var ALL_Info_Student = Businees_Serves_Student.GET_SAMPLE_INFO_FROM_SERVER_STUDENT_BY_ID_Branch_serves_and_ID_Unvirsty_For_modern_services(id_branch_serves, id_unvirsty);

            if (ALL_Info_Student.Count != 0)
            {
                return Ok(ALL_Info_Student);
            }
            else
                return NotFound("لا يوجد مقدمين للخدمة متاحين ");
        }




        [HttpGet("GET_SAMPLE_INFO_FROM_SERVER_STUDENT_BY_ID_Branch_serves_and_ID_Unvirsty_For_The_most_distinguished", Name = "GET_SAMPLE_INFO_FROM_SERVER_STUDENT_BY_ID_Branch_serves_and_ID_Unvirsty_For_The_most_distinguished")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Semple_Cradte_Info_ON_Server_Student_By_Pranch_Server> GET_SAMPLE_INFO_FROM_SERVER_STUDENT_BY_ID_Branch_serves_and_ID_Unvirsty_For_The_most_distinguished(int id_branch_serves, int id_unvirsty)
        {
            if (id_branch_serves < 0 || id_unvirsty < 0)
                return BadRequest("Invaild data input ...");

            var ALL_Info_Student = Businees_Serves_Student.GET_SAMPLE_INFO_FROM_SERVER_STUDENT_BY_ID_Branch_serves_and_ID_Unvirsty_For_The_most_distinguished(id_branch_serves, id_unvirsty);

            if (ALL_Info_Student.Count != 0)
            {
                return Ok(ALL_Info_Student);
            }
            else
                return NotFound("لا يوجد مقدمين للخدمة متاحين ");
        }





        [HttpGet("GET_SAMPLE_INFO_FROM_SERVER_STUDENT_BY_ID_Branch_serves_and_ID_Unvirsty_For_OLder_services", Name = "GET_SAMPLE_INFO_FROM_SERVER_STUDENT_BY_ID_Branch_serves_and_ID_Unvirsty_For_OLder_services")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Semple_Cradte_Info_ON_Server_Student_By_Pranch_Server> GET_SAMPLE_INFO_FROM_SERVER_STUDENT_BY_ID_Branch_serves_and_ID_Unvirsty_For_OLder_services(int id_branch_serves, int id_unvirsty)
        {
            if (id_branch_serves < 0 || id_unvirsty < 0)
                return BadRequest("Invaild data input ...");

            var ALL_Info_Student = Businees_Serves_Student.GET_SAMPLE_INFO_FROM_SERVER_STUDENT_BY_ID_Branch_serves_and_ID_Unvirsty_For_OLder_services(id_branch_serves, id_unvirsty);

            if (ALL_Info_Student.Count != 0)
            {
                return Ok(ALL_Info_Student);
            }
            else
                return NotFound("لا يوجد مقدمين للخدمة متاحين ");
        }



        [HttpGet("ADMIN_Function_GET_LIST_ID_SERVES_STUDENT_IN_PROGRASS_processing", Name = "ADMIN_Function_GET_LIST_ID_SERVES_STUDENT_IN_PROGRASS_processing")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Name_Serves_DTO> ADMIN_Function_GET_LIST_ID_SERVES_STUDENT_IN_PROGRASS_processing()
        {
            var list_ID_SERVES_STUDENT = Businees_Serves_Student.ADMIN_Function_GET_LIST_ID_SERVES_STUDENT_IN_PROGRASS_processing();

            if (list_ID_SERVES_STUDENT.Count == 0)
            {
                return NotFound("لا يوجد اي خدمة معلقة حاليا .....");
            }
            else
                return Ok(list_ID_SERVES_STUDENT);

        }




        [HttpGet("GET_Serves_Student_By_Id/{id}", Name = "GET_Serves_Student_By_Id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Serves_Student_DTO> GET_Serves_Student_By_Id(int id)
        {

            if (id < 1)
            {
                return BadRequest("ERROR: enter data ");
            }


            Businees_Serves_Student serves_student = Businees_Serves_Student.GET_SERVES_STUDENT_BY_ID(id);

            if (serves_student != null)
            {
                Serves_Student_DTO DTO = serves_student.SDTO;

                return Ok(DTO);
            }
            else
            {
                return NotFound("Data Not Found ....");
            }


        }




        [HttpPut("Accept_And_to_publish_Serves_Student{id_Serves_student}", Name = "Accept_And_to_publish_Serves_Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Accept_And_to_publish_Serves_Student(int id_Serves_student)
        {

            if (id_Serves_student < 1)
            {
                return BadRequest("Invalid Serves Student .....");
            }



            Businees_Serves_Student B_serves = Businees_Serves_Student.GET_SERVES_STUDENT_BY_ID(id_Serves_student);
            if (B_serves != null)
            {
                //ubdate in the inside Memorey
                //ID NOT ALLOW CHANGE .....
                B_serves.ID_Statue_Serves = 2;






                //ubdate in the data base 
                if (B_serves.save())
                {
                    Businees_Student student = Businees_Student.GET_Student_BY_ID(B_serves.ID_Student);
                    Business_Person person = Business_Person.GET_PERSON_BY_ID(student.ID_person);
                    YallaEnjazMailer send_email = new YallaEnjazMailer();

                    string subject = "✅ تم قبول خدمتك على منصة يلا إنجاز";

                    string body = $@"
مرحباً {person.F_name} {person.L_name} 👋

نود إعلامك بأن خدمتك بعنوان: ""{B_serves.Service_Address}""  
تمت مراجعتها والموافقة على نشرها من قبل فريق الدعم الفني في منصة ""يلا إنجاز"" ✅

أصبح بإمكان المستخدمين الآن مشاهدة خدمتك وطلبها عبر المنصة.

📅 تاريخ الموافقة: {DateTime.Now:yyyy-MM-dd HH:mm}

مع تمنياتنا لك بالتوفيق والنجاح ✨  
فريق يلا إنجاز
";



                    await send_email.SendEmailAsync(person.Email, subject, body);


                    return Ok("تم قبول الخدمة ....");


                }

                else
                {
                    return StatusCode(500, new { Message = "EROOR : NOT UBDATE DATA ...." });
                }

            }
            else
                return NotFound("لا يوجد خدمة لعرضها ......");


        }



        [HttpDelete("ADMIN_Delete_Serves_Student_By_Id_Serve_From_Admin {ID_serves_student}", Name = "ADMIN_Delete_Serves_Student_By_Id_Serve_From_Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> ADMIN_Delete_Serves_Student_By_Id_Serve_From_Admin(int ID_serves_student)
        {

            if (ID_serves_student < 1)
            {
                return BadRequest($"THE ID {ID_serves_student} Bad Data ...");
            }
            else
            {
                Businees_Serves_Student serves_student = Businees_Serves_Student.GET_SERVES_STUDENT_BY_ID(ID_serves_student);


                if (serves_student != null)
                {

                    if (serves_student.ADMIN_Delete_Serves_Student_By_Id_Serve_From_Admin(ID_serves_student))
                    {
                         YallaEnjazMailer send_email = new YallaEnjazMailer();
                        Businees_Student student = Businees_Student.GET_Student_BY_ID(serves_student.ID_Student);
                        Business_Person person = Business_Person.GET_PERSON_BY_ID(student.ID_person);


                        string subject = "❌ تم رفض خدمتك على منصة يلا إنجاز";

                        string body = $@"
مرحباً {person.F_name} {person.L_name} 👋

نأسف لإبلاغك بأنه بعد مراجعة خدمتك بعنوان: ""{serves_student.Service_Address}""، قررت إدارة الدعم الفني في منصة ""يلا إنجاز"" عدم الموافقة على نشرها في الوقت الحالي.


ندعوك إلى مراجعة تفاصيل خدمتك، والتأكد من مطابقتها لمعايير المنصة، ثم إعادة إرسالها بعد التعديل.

📌 لمساعدتك، يمكنك التواصل معنا عبر الدعم الفني في المنصة.

مع أطيب التحيات ✨  
فريق يلا إنجاز
";
                        if(person.Email!=null)
                        await send_email.SendEmailAsync(person.Email, subject, body);




                        return Ok($"The serevs_student {ID_serves_student} Completed Delete ... ");


                    }

                    else
                        return StatusCode(500, new { messege = "ERROR : Not Completed Delete ...." });





                }
                else
                    return NotFound($"The Id {serves_student} Not Found...");

            }


        }



    }
}
