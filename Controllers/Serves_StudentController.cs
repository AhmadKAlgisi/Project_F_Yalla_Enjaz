using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Poject_F_Data_Acsses_Yalla_Enjaz;

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
        public ActionResult<Branch_Serves_DTO> ADD_SERVESS_STUDENT(Serves_Student_DTO serves_student)
        {
            //if (string.IsNullOrEmpty(person.F_name) || string.IsNullOrEmpty(person.L_name) || string.IsNullOrEmpty(person.Email) || string.IsNullOrEmpty(person.Password))
            //{
            //    return BadRequest("Invalid person data.");
            //}
            Businees_Serves_Student B_serves = new Businees_Serves_Student(serves_student, Businees_Serves_Student.enmode.UBDATE);


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
        public ActionResult<Branch_Serves_DTO> Get_ALL_INFO_SAMPLE_STUDENT_PROVEDER_BRANCH_SERVER(int id_branch_serves)
        {
            var ALL_Info_Student = Businees_Serves_Student.Get_ALL_INFO_SAMPLE_STUDENT_PROVEDER_BRANCH_SERVER(id_branch_serves);

            if (ALL_Info_Student.Count!=0)
            {
                return Ok(ALL_Info_Student);
            }
            else
                return NotFound("لا يوجد مقدمين للخدمة متاحين ");
        }

    }
}
