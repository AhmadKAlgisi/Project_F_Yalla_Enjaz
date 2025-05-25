using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Poject_F_Data_Acsses_Yalla_Enjaz;
using System;
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
                string subject = "رمز التحقق الخاص بك - منصة يلا إنجاز";

                string body = $@"
    <html>
        <body style='font-family: Arial, sans-serif; direction: rtl; text-align: right;'>
            <h2 style='color: #2c3e50;'>مرحبًا،</h2>
            <p>رمز التحقق الخاص بك لتأكيد العملية هو:</p>
            <div style='font-size: 24px; font-weight: bold; color: #e74c3c; margin: 10px 0;'>{pin}</div>
            <p>يرجى إدخال هذا الرمز خلال <strong>5 دقائق</strong>.</p>
            <p>إذا لم تطلب هذا الرمز، يمكنك تجاهل هذه الرسالة.</p>
            <br />
            <p>تحياتنا،<br />فريق <strong>يلا إنجاز</strong></p>
        </body>
    </html>";


                // إرسال البريد الإلكتروني

                YallaEnjazMailer send_email = new YallaEnjazMailer();
               // Businnes_Send_Email send_email = new Businnes_Send_Email();

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

                YallaEnjazMailer send_email = new YallaEnjazMailer();
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

                await send_email.SendEmailAsync(B_PERSON.Email, subject, body);





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
                        YallaEnjazMailer send_email = new YallaEnjazMailer();
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

                        await send_email.SendEmailAsync(B_PERSON.Email, subject, body);


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





        [HttpGet("LOGIN_BERSON_BY_EMAIL_AND_PASSOWRD/{Email},{Passowrd}", Name = "LOGIN_BERSON_BY_EMAIL_AND_PASSOWRD")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<context_info> LOGIN_BERSON_BY_EMAIL_AND_PASSOWRD(string Email, string Passowrd)
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Passowrd))
            {
                return BadRequest("⚠️ يرجى إدخال البريد الإلكتروني وكلمة المرور.");
            }

            bool emailExists = Business_Person.Check_If_Email_Exists(Email);

            if (!emailExists)
            {
                return NotFound("❌ لا يوجد حساب مرتبط بهذا البريد الإلكتروني.");
            }

            // التحقق من تطابق البريد وكلمة المرور
            Business_Person person = Business_Person.Login_BERSON_BY_EMAIL_AND_Passowrd(Email, Passowrd);

            if (person == null)
            {
                return NotFound("❌ البريد الإلكتروني أو كلمة المرور غير صحيحة.");
            }

            context_info info = new context_info(
                person.ID_Person,
                person.ID_student,
                person.Is_Stutas_Accunt,
                person.Account_Type
            );

            return Ok(info);
        }


        [HttpGet("Forget_Passowrd/{Email},{new_passowrd}", Name = "Forget_Passowrd")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string> Forget_Passowrd(string Email, string new_passowrd)
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(new_passowrd))
            {
                return BadRequest("⚠️ يرجى إدخال البريد الإلكتروني وكلمة المرور.");
            }

            bool emailExists = Business_Person.Check_If_Email_Exists(Email);
            if (!emailExists)
            {
                return NotFound("❌ لا يوجد حساب مرتبط بهذا البريد الإلكتروني.");
            }

            int ID_Person = Business_Person.GET_ID_PERSON_BY_EMAIL(Email);
            if (ID_Person == 0)
            {
                return NotFound("❌ لا يوجد حساب مرتبط بهذا البريد الإلكتروني.");
            }

            Business_Person person = Business_Person.GET_PERSON_BY_ID(ID_Person);
            if (person == null)
            {
                return StatusCode(500, new { Message = "🚨 حصل خطأ أثناء معالجة الطلب. يرجى المحاولة لاحقاً." });
            }

            bool changed = person.change_passowrd(ID_Person, new_passowrd);
            if (!changed)
            {
                return BadRequest("❌ لم يتم تغيير كلمة المرور.");
            }

            // إرسال الإيميل بعد النجاح
            YallaEnjazMailer send_email = new YallaEnjazMailer();

            string subject = "🔐 تم تغيير كلمة المرور الخاصة بك على منصة يلا إنجاز";
            string body = $@"
مرحباً عزيزي {person.F_name} {person.L_name} 👋،

نود إعلامك بأنه تم <b>تغيير كلمة المرور الخاصة بحسابك</b> على منصة <b>يلا إنجاز</b> بنجاح.

إذا قمت أنت بطلب هذا التغيير، فلا داعي لأي إجراء آخر.  
أما إذا لم تكن أنت من قام بهذا التغيير، فقد يكون هناك نشاط غير معتاد على حسابك. في هذه الحالة نوصيك فوراً بـ:
<ul>
  <li>محاولة تسجيل الدخول وتغيير كلمة المرور مجددًا.</li>
  <li>التواصل معنا عبر الدعم الفني إذا واجهت أي مشكلة.</li>
</ul>

🚨 <b>تنبيه:</b> الحفاظ على أمان حسابك مسؤوليتنا ومسؤوليتك، لذا لا تشارك معلوماتك مع أي جهة غير موثوقة.

نتمنى لك تجربة آمنة ومميزة معنا 💼🔒

تحياتنا،<br/>
<b>فريق يلا إنجاز</b>
";
            _ = send_email.SendEmailAsync(person.Email, subject, body); // Fire and forget

            return Ok("✅ تم تغيير كلمة المرور بنجاح، وتم إرسال إشعار على بريدك الإلكتروني.");
        }

        [HttpGet("Update_Student_Is_Avtive{ID_Person}")]
        public ActionResult<bool> Update_Student_Is_Avtive(int ID_Person)
        {

            Info_Person_In_Profile_Person_DTO_and_update B_PERSON_DTO = Business_Person.GET_Info_PERSON_BY_ID_Person_Using_Profile_Person(ID_Person);


            if (B_PERSON_DTO != null)
            {

                Business_Person B_berson = new Business_Person(B_PERSON_DTO, Business_Person.enMode.Update);
                B_berson.ID_Staute = 2;


                if (B_berson.save())

                    return Ok("تم تغير حالة الحساب الى نشط بنجاح");

                else
                {
                    return StatusCode(500, new { Message = "EROOR : NOT UBDATE DATA ...." });
                }

            }
            else
                return NotFound("لا يوجد حساب ");
        }





    }
}
