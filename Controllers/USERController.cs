using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Poject_F_Data_Acsses_Yalla_Enjaz;
using System.ComponentModel;

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



        [HttpPut("Update USER", Name = "Update USER")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]


        public ActionResult<USER_ADD_DTO>Update_User(int id_user,USER_ADD_DTO USER)
        {
            if(id_user < 1 || string.IsNullOrEmpty(USER.F_name) || string.IsNullOrEmpty(USER.L_name) || string.IsNullOrEmpty(USER.Email) || string.IsNullOrEmpty(USER.passowrd))
            {
                return BadRequest("Invalid User Data.... ");
            }

            else
            {
                Business_User B_user = Business_User.GET_USER_BY_ID(id_user);

                if (B_user != null)
                {
                    Business_Person B_person = Business_Person.GET_PERSON_BY_ID(B_user.id_person);
                    B_person.F_name = USER.F_name;
                    B_person.L_name = USER.L_name;
                    B_person.Email = USER.Email;
                    B_person.Password = USER.passowrd;
                    B_person.save();
                    USER.id_person = B_user.id_person;
                    return Ok(USER);


                }
                else
                    return NotFound("The Object Not Found....");



            }
        }


        [HttpDelete("Delete USER", Name = "Delete USER")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        ///هنا فقط اعملت حذف للمستخدم عند t_sql في كود delete person 
        public ActionResult<string>Delete_user(int id_user)
        {
            if (id_user<1)
            {
                return BadRequest("Invaild Data ....");

            }
            else
            {

                Business_User B_user = Business_User.GET_USER_BY_ID(id_user);
                if (B_user != null)
                {
                    Business_Person B_person = Business_Person.GET_PERSON_BY_ID(B_user.id_person);
                    if (B_person.Delete_Person(B_person.ID))

                    {
                        return Ok("Completed Delete Object ....");
                    }
                    else
                        return StatusCode(500, new { message = "ERROR: NOT COMPLETED DELETE OBJECT..." });

                }
                else
                    return NotFound("The Object Not Found....");


            }

        }



    }
}
