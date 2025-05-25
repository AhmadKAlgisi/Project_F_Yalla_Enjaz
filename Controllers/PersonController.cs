using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Businees_Logic_Project;
using Poject_F_Data_Acsses_Yalla_Enjaz;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authentication.BearerToken;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Formats.Asn1;

namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/Person")]
    [ApiController]
    public class Person_Controller : ControllerBase
    {
        private readonly Cloudinary _cloudinary;

        public Person_Controller()
        {
            Account account = new Account(
                "dlazpanst",     // استبدلها بـ Cloud name من Cloudinary
                "413456772532198",        // استبدلها بـ API Key
                "IUAKhdveNw7Sz4SLWrmj2qMQBCk"      // استبدلها بـ API Secret
            );
            _cloudinary = new Cloudinary(account);
        }




        [HttpGet("GET_PERSON_PY_ID{id}", Name = "GET_PERSON_PY_ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Person_DTO> GET_PERSON_BY_ID(int id)
        {

            if (id < 1)
            {
                return BadRequest("ERROR: enter data ");
            }


            Business_Person person = Business_Person.GET_PERSON_BY_ID(id);

            if (person != null)
            {
                Info_Person_In_Profile_Person_DTO_and_update DTO = person.SDTO_MAIN_PROFILE;

                return Ok(DTO);
            }
            else
            {
                return NotFound("Data Not Found ....");
            }


        }


        [HttpPost("ADD PERSON", Name = "ADD PERSON")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Person_DTO> AddPesron(Person_DTO person)
        {
            if (string.IsNullOrEmpty(person.F_name) || string.IsNullOrEmpty(person.L_name) || string.IsNullOrEmpty(person.Email) || string.IsNullOrEmpty(person.Password))
            {
                return BadRequest("Invalid person data.");
            }

            Business_Person B_PERSON = new Business_Person(person, Businees_Logic_Project.Business_Person.enMode.AddNew);

            B_PERSON.save();
            person.ID = B_PERSON.ID;
            Cradet_DTO card = Businees_Cradt_Card.Create_New_DTO_Cradete_Card(B_PERSON.ID);

            Businees_Cradt_Card B_Cradte_Card = new Businees_Cradt_Card(card, Businees_Cradt_Card.enmode.ADDNEW);
            B_Cradte_Card.save();



            return CreatedAtRoute("ADD PERSON", new { id = person.ID }, person);



        }


        [HttpPut("UPDATE_PERSON {id}", Name = "UPDATE_PERSON")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Person_DTO> Ubdate_Person(int id, Info_Person_In_Profile_Person_DTO_and_update person)
        {

            if (string.IsNullOrEmpty(person.F_name) || string.IsNullOrEmpty(person.L_name) || string.IsNullOrEmpty(person.Email) || string.IsNullOrEmpty(person.Phone))
            {
                return BadRequest("Invalid person data.");
            }

            // Business_Person B_PERSON = Business_Person.GET_PERSON_BY_ID(id);//find object if success change mode ubdate 

            Info_Person_In_Profile_Person_DTO_and_update B_PERSON_DTO = Business_Person.GET_Info_PERSON_BY_ID_Person_Using_Profile_Person(id);


            if (B_PERSON_DTO != null)
            {

                Business_Person B_berson = new Business_Person(B_PERSON_DTO, Business_Person.enMode.Update);
                B_berson.F_name = person.F_name;
                B_berson.L_name = person.L_name;
                B_berson.Gender = person.Gender;
                B_berson.Phone = person.Phone;
                B_berson.Personal_profile = person.Personal_profile;


                if (B_berson.save())

                    return Ok(B_berson.SDTO_MAIN_PROFILE);

                else
                {
                    return StatusCode(500, new { Message = "EROOR : NOT UBDATE DATA ...." });
                }

            }
            else
                return NotFound("Not Found Object ...");


        }




        [HttpPut("Ubdate_Email_BY_Admin {id_Person}", Name = "Ubdate_Email_BY_Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> Ubdate_Email_BY_Admin(int id_Person,string New_Email)
        {

            if (string.IsNullOrEmpty(New_Email))
            {
                return BadRequest("Invalid person data.");
            }

            // Business_Person B_PERSON = Business_Person.GET_PERSON_BY_ID(id);//find object if success change mode ubdate 

            Info_Person_In_Profile_Person_DTO_and_update B_PERSON_DTO = Business_Person.GET_Info_PERSON_BY_ID_Person_Using_Profile_Person(id_Person);


            if (B_PERSON_DTO != null)
            {

                Business_Person B_berson = new Business_Person(B_PERSON_DTO, Business_Person.enMode.Update);
                string Old_Email = B_berson.Email;
                B_berson.Email = New_Email;


                if (B_berson.save())

                {
                    YallaEnjazMailer send_email = new YallaEnjazMailer();

                    string subject = "تم تحديث بريدك الإلكتروني في منصة يلا إنجاز";
                    string body = $@"
مرحباً {B_berson.F_name+" "+B_berson.L_name}،

نحيطك علماً بأنه تم تحديث بريدك الإلكتروني الجامعي إلى بريد شخصي بناءً على طلبك عبر الدعم الفني.

يمكنك الآن تسجيل الدخول باستخدام بريدك الجديد: {B_berson.Email}

في حال لم تكن أنت من طلب التغيير، يرجى التواصل معنا فوراً.

مع تحياتنا،
فريق يلا إنجاز
";

                    await send_email.SendEmailAsync(B_berson.Email, subject, body);

                    return Ok("تم نغير الايميل بنجاح");
                }

                else
                {
                    return StatusCode(500, new { Message = "EROOR : NOT UBDATE DATA ...." });
                }

            }
            else
                return NotFound("Not Found Object ...");


        }





        [HttpDelete("DELETE_PERSON {id}", Name = "DELETE_PERSON")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //     [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string> Delete_Person(int id)
        {

            if (id < 1)
            {
                return BadRequest($"THE ID {id} Bad Data ...");
            }
            else
            {
                Business_Person person = Business_Person.GET_PERSON_BY_ID(id);


                if (person != null)
                {
                    if (person.Delete_Person(id))
                        return Ok($"The Person {id} Completed Delete ... ");

                    else
                        return StatusCode(500, new { messege = "ERROR : Not Completed Delete ...." });

                }
                else
                    return NotFound($"The Id {id} Not Found...");

            }


        }







        [HttpGet("GET_Info_PERSON_BY_ID_Person_Using_Profile_Person{ID_Person}", Name = "GET_Info_PERSON_BY_ID_Person_Using_Profile_Person")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Person_DTO> GET_Info_PERSON_BY_ID_Person_Using_Profile_Person(int ID_Person)
        {

            if (ID_Person < 1)
            {
                return BadRequest("ERROR: enter data ");
            }

            Info_Person_In_Profile_Person_DTO_and_update Person_Info = Business_Person.GET_Info_PERSON_BY_ID_Person_Using_Profile_Person(ID_Person);

            if (Person_Info != null)
            {


                return Ok(Person_Info);
            }
            else
            {
                return NotFound("Data Not Found ....");
            }


        }




        [HttpPut("Update_Imege_Profile {id_Person}", Name = "Update_Imege_Profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task< ActionResult>Update_Imege_Profile(int id_Person, IFormFile imageFile)
        {

            if (imageFile == null || imageFile.Length == 0 || id_Person < 0)
                return BadRequest("No file uploaded.");



            //get all info from data base 

            Info_Person_In_Profile_Person_DTO_and_update B_PERSON_DTO = Business_Person.GET_Info_PERSON_BY_ID_Person_Using_Profile_Person(id_Person);


            if (B_PERSON_DTO != null)
            {
                //create object from business logic becuse use save methode
                Business_Person B_berson = new Business_Person(B_PERSON_DTO, Business_Person.enMode.Update);

                //work cloundary

                //name folder exite all imege
                string folder = "student_images";

                //if exite older imege in data base delete note(ممكن تكةن null عشان هيك حطناها null)
                if (!string.IsNullOrEmpty(B_berson.Main_Imege_Url))
                {
                    var uri = new Uri(B_berson.Main_Imege_Url);
                    var publicId = Path.GetFileNameWithoutExtension(uri.AbsolutePath);//اخذ extintion عشان يعرف id تبع الصوؤة
                    string fullPublicId = $"{folder}/{publicId}";

                    var deleteParams = new DeletionParams(fullPublicId);
                    var deletionResult = await _cloudinary.DestroyAsync(deleteParams);

                    if (deletionResult.Result != "ok" && deletionResult.Result != "not found")
                        return StatusCode(500, new { Message = "Failed to delete old image from Cloudinary." });
                }



                using var stream = imageFile.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(imageFile.FileName, stream),
                    Folder = folder
                };


                var uploadResult = await _cloudinary.UploadAsync(uploadParams);//اخذ الصورة والباث وايضا اسم الفولدر الي بدو يخزن عليه 

                if (uploadResult.Error != null)
                    return StatusCode(500, new { Message = "Cloudinary Error: " + uploadResult.Error.Message });

                // أخذ الرابط النهائي للصورة
                string cloudinaryUrl = uploadResult.SecureUrl.ToString();//path end 

                // save in database  

                B_berson.Main_Imege_Url = cloudinaryUrl;


                //save data in data base 

                if (B_berson.save())

                    return Ok(B_berson.Main_Imege_Url);

                else
                {
                    return StatusCode(500, new { Message = "EROOR : NOT UBDATE DATA ...." });
                }

            }

            else
                return NotFound("Not Found Object ...");


        }





        [HttpPut("Change_Password {id_person}", Name = "Change_Password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Person_DTO> Change_Password(int id_person,string Current_Password,string New_Password)
        {

            if (id_person <0|| string.IsNullOrEmpty(Current_Password) || string.IsNullOrEmpty(New_Password))
            {
                return BadRequest("بيانات غير صالحة");
            }

            Business_Person B_PERSON = Business_Person.GET_PERSON_BY_ID(id_person);//find object if success change mode ubdate 

          

            if (B_PERSON != null)
            {

                
                if (B_PERSON.Get_Passowrd_By_ID_Person(B_PERSON.ID) == Current_Password)
                {
                    B_PERSON.Password = New_Password;

                    if (B_PERSON.change_passowrd(id_person, B_PERSON.Password))

                        return Ok("تم تغير كلمة المرور بنجاح");

                    else
                    {
                        return StatusCode(500, new { Message = "EROOR :حدث خطأ أثناء حفظ البيانات." });
                    }

                }

                else
                    return BadRequest("كلمة المرور الحالية غير صحيحة");

            }
            else
                return NotFound("المستخدم غير موجود");


        }



    }
}
