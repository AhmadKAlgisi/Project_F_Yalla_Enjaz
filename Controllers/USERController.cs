using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Poject_F_Data_Acsses_Yalla_Enjaz;

namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/USER")]
    [ApiController]
    public class USERController : ControllerBase
    {

        [HttpGet("GET_USER_PY_ID{id}", Name = "GET_USER_PY_ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Person_DTO> GET_USER_BY_ID(int id)
        {

            if (id < 1)
            {
                return BadRequest("ERROR: enter data.... ");
            }


           Business_User user = Business_User.GET_USER_BY_ID(id);
            




            if (user != null)
            {
                Business_Person person = Business_Person.GET_PERSON_BY_ID(user.id_person);

                USER_FULL_DTO DTO = new USER_FULL_DTO(user.id,person.ID,person.F_name+" "+person.L_name ,person.Email);



                return Ok(DTO);
            }
            else
            {
                return NotFound("Data Not Found ....");
            }




        }




        [HttpPost("ADD USER", Name = "ADD USER")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<USER_ADD_DTO> AddPesron(USER_ADD_DTO USER)
        {
            if ( string.IsNullOrEmpty(USER.F_name) || string.IsNullOrEmpty(USER.L_name) || string.IsNullOrEmpty(USER.Email) || string.IsNullOrEmpty(USER.passowrd))
            {
                return BadRequest("Invalid person data.");
            }


            Person_DTO person = new Person_DTO(0, USER.F_name, USER.L_name, USER.Email, USER.passowrd);
            Business_Person B_PERSON = new Business_Person(person, Businees_Logic_Project.Business_Person.enMode.AddNew);

            if (B_PERSON.save())
            {
                person.ID = B_PERSON.ID;

                User_DTO user = new User_DTO(0, person.ID);
                Business_User B_user = new Business_User(user, Business_User.enmode.ADDNEW);
                 if(B_user.save())
                {///عشان ارجع الداتا لليوزر احدث شي 
                    USER.id_person = B_PERSON.ID;
                    USER.id_user = B_user.id;
                    return CreatedAtRoute("ADD User", new { id = B_user.id}, USER);
                }
                ///اذا كان في مشكلة في اضافة المستخدم 
                else
                    return StatusCode(500, new { message = "ERROR : NOT COMPLETED ADD USER ..." });


              
            }
            ///اذا كان في مشكلة في اضافة الشخص  
            else
                return StatusCode(500,new { message = "ERROR : NOT COMPLETED ADD USER ..." });
           



        }



    }
}
