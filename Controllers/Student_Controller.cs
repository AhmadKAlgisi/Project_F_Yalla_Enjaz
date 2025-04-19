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


        [HttpPost("ADD Student", Name = "ADD Student")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<USER_ADD_DTO> AddPesron(Student_ADD_and_find_DTO student)
        {
            if (student.ID_Unversty<1|| string.IsNullOrEmpty(student.University_mejor) || string.IsNullOrEmpty(student.Switch_Email) || string.IsNullOrEmpty(student.F_name) || string.IsNullOrEmpty(student.L_name) || string.IsNullOrEmpty(student.Email) || string.IsNullOrEmpty(student.password))
            {
                return BadRequest("Invalid User data.");
            }


            Person_DTO person = new Person_DTO(0, student.F_name, student.L_name, student.Email, student.password);
            Business_Person B_PERSON = new Business_Person(person, Businees_Logic_Project.Business_Person.enMode.AddNew);

            if (B_PERSON.save())
            {
                person.ID = B_PERSON.ID;//update data object after adding and saving data in data base


                Cradet_DTO card = Businees_Cradt_Card.Create_New_DTO_Cradete_Card(B_PERSON.ID);

                Businees_Cradt_Card B_Cradte_Card = new Businees_Cradt_Card(card, Businees_Cradt_Card.enmode.ADDNEW);
                B_Cradte_Card.save();



                Student_DTO Student = new Student_DTO(0, student.Switch_Email, student.University_mejor, person.ID, student.ID_Unversty);
                Businees_Student B_student = new Businees_Student(Student, Businees_Student.enmode.ADDNEW);

                if(B_student.save())
                {
                   
                    student.ID_person = B_PERSON.ID;
                    student.ID_student = B_student.ID;
                    return CreatedAtRoute("ADD User", new { id = B_student.ID}, student);
                }

                ///اذا كان في مشكلة في اضافة المستخدم 
                else
                    return StatusCode(500, new { message = "ERROR : NOT COMPLETED ADD USER ..." });



            }
            ///اذا كان في مشكلة في اضافة الشخص  
            else
                return StatusCode(500, new { message = "ERROR : NOT COMPLETED ADD USER ..." });




        }




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






    }
}
