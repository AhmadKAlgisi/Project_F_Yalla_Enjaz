using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Poject_F_Data_Acsses_Yalla_Enjaz;

namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Student_Controller : ControllerBase
    {


      



        [HttpGet("GET_STUDENT_PY_ID{id}", Name = "GET_STUDENT_PY_ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Person_DTO> GET_STUDENT_BY_ID(int id)
        {

            if (id < 1)
            {
                return BadRequest("ERROR: enter data.... ");
            }


            Businees_Student student = Businees_Student.GET_Student_BY_ID(id);





            if (student != null)
            {
                Business_Person person = Business_Person.GET_PERSON_BY_ID(student.ID_person);

                Student_ADD_and_find_DTO Full_data_object = student.create_full_object_student_find_and_person(person);
                return Ok(Full_data_object);
            }
            else
            {
                return NotFound("Data Not Found ....");
            }




        }




        [HttpGet("GET_INFO_FROM_STUDENT_UNVIRSTY_PERSON_USED_SHOW_SERVES_BY_ID_STUDENT{id_student}", Name = "GET_INFO_FROM_STUDENT_UNVIRSTY_PERSON_USED_SHOW_SERVES_BY_ID_STUDENT")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Person_DTO> GET_INFO_FROM_STUDENT_UNVIRSTY_PERSON_USED_SHOW_SERVES_BY_ID_STUDENT(int id_student)
        {

            if (id_student < 1)
            {
                return BadRequest("ERROR: enter data.... ");
            }


            INFO_FROM_STUDENT_UNVIRSTY_PERSON_USED_SHOW_SERVES_DTO object_DTO = Businees_Student.GET_INFO_FROM_STUDENT_UNVIRSTY_PERSON_USED_SHOW_SERVES_BY_ID_STUDENT(id_student);





            if (object_DTO != null)
            {
             
                return Ok(object_DTO);
            }
            else
            {
                return NotFound("Data Not Found ....");
            }




        }



        [HttpPut("ADMAIN_UPDATE_STUDENT_SET_UN_ACTIVE", Name = "ADMAIN_UPDATE_STUDENT_SET_UN_ACTIVE")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<string> ADMAIN_UPDATE_STUDENT_SET_UN_ACTIVE()
        {
            try
            {
                bool result = Businees_Student.ADMAIN_UPDATE_STUDENT_SET_UN_ACTIVE();

                if (result)
                {
                    return Ok("✅ تم تحديث حالة الطلاب بنجاح.");
                }
                else
                {
                    return StatusCode(500, new { message = "❌ لم يتم تنفيذ التحديث. تحقق من الإجراء المخزن أو البيانات." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "⚠️ خطأ تقني في الخادم", error = ex.Message });
            }
        }





        [HttpGet("ADMIN_GET_INFO_FROM_STUDENT_IN_WORK_CONVERT_EMAIL_BY_ADMIN{Email}", Name = "ADMIN_GET_INFO_FROM_STUDENT_IN_WORK_CONVERT_EMAIL_BY_ADMIN")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Person_DTO> ADMIN_GET_INFO_FROM_STUDENT_IN_WORK_CONVERT_EMAIL_BY_ADMIN(string Email)
        {

            if (string.IsNullOrEmpty(Email))
            {
                return BadRequest("ERROR: enter data.... ");
            }


            GET_ALL_INFO_FROM_STUDENT_IN_ADMIN_WORK object_DTO = Businees_Student.ADMIN_GET_INFO_FROM_STUDENT_IN_WORK_CONVERT_EMAIL_BY_ADMIN(Email);





            if (object_DTO != null)
            {

                return Ok(object_DTO);
            }
            else
            {
                return NotFound("Data Not Found ....");
            }




        }




        [HttpGet("ADMIN_GET_ALL_INFO_statistics_By_Id_Student{id_student}", Name = "ADMIN_GET_ALL_INFO_statistics_By_Id_Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Person_DTO> ADMIN_GET_ALL_INFO_statistics_By_Id_Student(int id_student)
        {

            if (id_student < 1)
            {
                return BadRequest("ERROR: enter data.... ");
            }


            INFO_statistics_DTO info = Businees_Student.ADMIN_GET_ALL_INFO_statistics_By_Id_Student(id_student);





            if (info != null)
            {
                return Ok(info);
            }
            else
            {
                return NotFound("Data Not Found ....");
            }




        }




    }
}
