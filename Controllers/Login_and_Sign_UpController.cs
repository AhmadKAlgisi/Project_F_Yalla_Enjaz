using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Poject_F_Data_Acsses_Yalla_Enjaz;
using System.Diagnostics.Eventing.Reader;
using System.Linq.Expressions;

namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/Login_and_Sign_Up")]
    [ApiController]


    public class Login_and_Sign_UpController : ControllerBase
    {
       
       
            [HttpGet("Send_OTP_Into_Email_And_Return/{email}")]
            [ProducesResponseType(StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            public async Task<ActionResult<int>> Send_OTP_Into_Email_And_Return(string email)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(email))
                    {
                        return BadRequest("خطأ: الإيميل غير صالح أو مفقود.");
                    }

                    // توليد رمز OTP
                    Random random = new Random();
                    int pin = random.Next(10000, 99999);

                    // إعداد الرسالة
                    string subject = "🔐 رمز التحقق الخاص بك - منصة يلا إنجاز";

                    string body = $@"
مرحبًا 👋،<br><br>

رمز التحقق (OTP) الخاص بك لتأكيد العملية على منصة <b>يلا إنجاز</b> هو:<br><br>

<h2 style='color:#2d89ef;'>{pin}</h2><br>

يرجى إدخال هذا الرمز خلال <b>5 دقائق</b> لضمان أمان حسابك.<br><br>

إذا لم تطلب هذا الرمز، يمكنك تجاهل هذه الرسالة.<br><br>

تحياتنا،<br>
فريق يلا إنجاز
";

                    // إرسال البريد الإلكتروني
                    Businnes_Send_Email send_email = new Businnes_Send_Email();
                    await send_email.SendEmailAsync(email, subject, body);

                    return Ok(pin); // إرسال الـ OTP للفرونت
                }
                catch (Exception ex)
                {
                    return BadRequest("حدث خطأ أثناء إرسال رمز التحقق: " + ex.Message);
                }
            }






        [HttpPost("Sign_Up_Add_User", Name = "Sign_Up_Add_User")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<context_info>> Sign_Up_Add_User(USER_ADD_DTO USER)
        {
            if (string.IsNullOrEmpty(USER.F_name) || string.IsNullOrEmpty(USER.L_name) || string.IsNullOrEmpty(USER.Email) || string.IsNullOrEmpty(USER.passowrd))
            {
                return BadRequest("Invalid User data.");
            }


            Person_DTO person = new Person_DTO(0, USER.F_name, USER.L_name, USER.Email, USER.passowrd,USER.Account_Type);
            Business_Person B_PERSON = new Business_Person(person, Businees_Logic_Project.Business_Person.enMode.AddNew);

            if (B_PERSON.save())
            {
                person.ID = B_PERSON.ID;


                Cradet_DTO card = Businees_Cradt_Card.Create_New_DTO_Cradete_Card(B_PERSON.ID);

                Businees_Cradt_Card B_Cradte_Card = new Businees_Cradt_Card(card, Businees_Cradt_Card.enmode.ADDNEW);
                B_Cradte_Card.save();

                Businnes_Send_Email Email = new Businnes_Send_Email();
                string subject = "🎉 مرحباً بك في منصة يلا إنجاز - معلومات بطاقتك البنكية الافتراضية";

                string body = $@"
مرحباً عزيزي {B_PERSON.F_name} {B_PERSON.L_name} 👋،

يسعدنا انضمامك إلى منصة <b>يلا إنجاز</b>، المنصة التي تفتح لك آفاقاً جديدة لتحقيق دخل من خلال تقديم خدماتك بكل احترافية.

نرجو منك الاحتفاظ بمعلومات بطاقتك البنكية الافتراضية التالية بأمان، حيث ستُستخدم لإجراء المعاملات المالية داخل المنصة:

🔹 <b>رقم البطاقة</b>: <span style='color:#2d89ef;'>{B_Cradte_Card.Number_Card}</span>  
🔹 <b>رمز الحماية (PIN)</b>: <span style='color:#2d89ef;'>{B_Cradte_Card.Pin_Code}</span>  
🔹 <b>الرصيد المبدئي</b>: <span style='color:#2d89ef;'>{B_Cradte_Card.Palnce} دينار</span>  

🚨 <b>تنبيه:</b> لا تشارك هذه المعلومات مع أي شخص للحفاظ على أمان حسابك.

نتمنى لك تجربة موفقة ومثمرة معنا!

تحياتنا،  
فريق <b>يلا إنجاز</b>
";

                await Email.SendEmailAsync(B_PERSON.Email, subject, body);





                User_DTO user = new User_DTO(0, person.ID);
                Business_User B_user = new Business_User(user, Business_User.enmode.ADDNEW);
                if (B_user.save())
                {///عشان ارجع الداتا لليوزر احدث شي 
                    USER.id_person = B_PERSON.ID;
                    USER.id_user = B_user.id;

                    context_info Context = new context_info(B_PERSON.ID, null,2, B_PERSON.Account_Type);

                    return CreatedAtRoute("Sign_Up_Add_User", new { id = B_user.id }, Context);
                }
                ///اذا كان في مشكلة في اضافة المستخدم 
                else
                    return StatusCode(500, new { message = "ERROR : NOT COMPLETED ADD USER ..." });



            }
            ///اذا كان في مشكلة في اضافة الشخص  
            else
                return StatusCode(500, new { message = "ERROR : NOT COMPLETED ADD USER ..." });




        }




        [HttpPost("Sign_Up_ADD_Student", Name = "Sign_Up_ADD_Student")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<context_info>>Sign_Up_ADD_Student(Student_ADD_and_find_DTO student)
        {
            try
            {
                if (student.ID_Unversty < 1 || string.IsNullOrEmpty(student.University_mejor) || string.IsNullOrEmpty(student.Switch_Email) || string.IsNullOrEmpty(student.F_name) || string.IsNullOrEmpty(student.L_name) || string.IsNullOrEmpty(student.Email) || string.IsNullOrEmpty(student.password))
                {
                    return BadRequest("Invalid User data.");
                }


                Person_DTO person = new Person_DTO(0, student.F_name, student.L_name, student.Email, student.password, student.Account_Type);
                Business_Person B_PERSON = new Business_Person(person, Businees_Logic_Project.Business_Person.enMode.AddNew);

                if (B_PERSON.save())
                {
                    person.ID = B_PERSON.ID;//update data object after adding and saving data in data base


                    Cradet_DTO card = Businees_Cradt_Card.Create_New_DTO_Cradete_Card(B_PERSON.ID);

                    Businees_Cradt_Card B_Cradte_Card = new Businees_Cradt_Card(card, Businees_Cradt_Card.enmode.ADDNEW);
                    B_Cradte_Card.save();



                    Student_DTO Student = new Student_DTO(0, student.Switch_Email, student.University_mejor, person.ID, student.ID_Unversty);
                    Businees_Student B_student = new Businees_Student(Student, Businees_Student.enmode.ADDNEW);

                    if (B_student.save())
                    {
                        Businnes_Send_Email Email = new Businnes_Send_Email();
                        student.ID_person = B_PERSON.ID;
                        student.ID_student = B_student.ID;
                        student_login_and_sigup_DTO info_student = new student_login_and_sigup_DTO(student.ID_person, student.ID_student);

                        string subject = "🎉 مرحباً بك في منصة يلا إنجاز - معلومات بطاقتك البنكية الافتراضية";

                        string body = $@"
مرحباً عزيزي {B_PERSON.F_name} {B_PERSON.L_name} 👋،

يسعدنا انضمامك إلى منصة <b>يلا إنجاز</b>، المنصة التي تمكّنك كطالب من الاستفادة من الخدمات التي يقدمها زملاؤك، وأيضاً من تقديم خدماتك للآخرين بكل احترافية.

تم إنشاء بطاقتك البنكية الافتراضية، والتي ستُستخدم لإجراء المعاملات المالية داخل المنصة، سواء كنت تطلب خدمة أو تقدّمها:

🔹 <b>رقم البطاقة</b>: <span style='color:#2d89ef;'>{B_Cradte_Card.Number_Card}</span>  
🔹 <b>رمز الحماية (PIN)</b>: <span style='color:#2d89ef;'>{B_Cradte_Card.Pin_Code}</span>  
🔹 <b>الرصيد المبدئي</b>: <span style='color:#2d89ef;'>{B_Cradte_Card.Palnce} دينار</span>  

🚨 <b>تنبيه:</b> يرجى الاحتفاظ بهذه المعلومات بسرّية وعدم مشاركتها مع أي جهة للحفاظ على أمان حسابك.

نتمنى لك تجربة ناجحة ومثرية معنا، سواء في طلب الخدمات أو تقديمها 💼🛠️

تحياتنا،  
فريق <b>يلا إنجاز</b>
";

                        await Email.SendEmailAsync(B_PERSON.Email, subject, body);


                        context_info Context = new context_info(B_PERSON.ID,B_student.ID, 2, B_PERSON.Account_Type);

                        return CreatedAtRoute("Sign_Up_ADD_Student", new { id = B_student.ID }, Context);
                    }

                    ///اذا كان في مشكلة في اضافة المستخدم 
                    else
                        return StatusCode(500, new { message = "ERROR : NOT COMPLETED ADD USER ..." });



                }
                ///اذا كان في مشكلة في اضافة الشخص  
                else
                    return StatusCode(500, new { message = "ERROR : NOT COMPLETED ADD USER ..." });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }




        [HttpGet("check-email")]
        public ActionResult<bool> CheckEmail(string email)
        {
            bool emailExists = Business_Person.Check_If_Email_Exists(email); // افترضت وجود دالة بالبزنس لير

            return Ok(emailExists);
        }


        //[HttpGet("Check_Login{Email},{Passowrd}", Name = "Check_Login")]
        //public ActionResult<context_info> Check_Login(string Email, string Passowrd)
        //{
        //    if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Passowrd))
        //    {
        //        return BadRequest("خطاء في ادخال الايميل او كلمة المرور ");
        //    }
        //    else
        //    {
        //        GET_ALL_INFO_FROM_STUDENT_IN_ADMIN_WORK object_DTO = Businees_Student.ADMIN_GET_INFO_FROM_STUDENT_IN_WORK_CONVERT_EMAIL_BY_ADMIN(Email);
        //        if (object_DTO != null)
        //        {

        //            Business_Person B_PERSON = Business_Person.GET_PERSON_BY_ID(object_DTO.ID_Person);//find object if success change mode ubdate 
        //            if (B_PERSON.Account_Type.ToUpper() == "USER" || B_PERSON.Account_Type.ToLower() == "user")
        //            {
        //                if (B_PERSON.Get_Passowrd_By_ID_Person(B_PERSON.ID) == Passowrd)
        //                {
        //                    context_info Info = new context_info(B_PERSON.ID, null, B_PERSON.ID_Staute ?? 0, "USER");
        //                    return Ok(Info);
        //                }
        //                else
        //                {
        //                    return BadRequest("خطاء في كلمة المرور");
        //                }

        //            }
        //            else
        //            {
        //                return BadRequest("حساب student ");
        //            }






        //        }
        //        else
        //        {

        //            return NotFound("لا يوجد حساب مسجل في هذه المعلومات ");


        //        }





        //    }

        //}

    }
}
