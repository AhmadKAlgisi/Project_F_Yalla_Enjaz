using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Businees_Logic_Project;
using Poject_F_Data_Acsses_Yalla_Enjaz;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/Person")]
    [ApiController]
    public class Person_Controller : ControllerBase
    {
        [HttpGet("GET_PERSON_PY_ID{id}",Name = "GET_PERSON_PY_ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Person_DTO> GET_PERSON_BY_ID(int id)
        {

            if(id<1)
            {
                return BadRequest("ERROR: enter data ");
            }


            Business_Person person = Business_Person.GET_PERSON_BY_ID(id);

            if (person!=null)
            {
                Person_DTO DTO = person.SDTO;

                return Ok(DTO);
            }
            else
            {
                return NotFound("Data Not Found ....");
            }


        }


        [HttpPost("ADD PERSON",Name ="ADD PERSON")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Person_DTO> AddPesron(Person_DTO person)
        {
            if(string.IsNullOrEmpty(person.F_name) || string.IsNullOrEmpty(person.L_name) || string.IsNullOrEmpty(person.Email)|| string.IsNullOrEmpty(person.Password))
            {
                return BadRequest("Invalid person data.");
            }

            Business_Person B_PERSON = new Business_Person(person, Businees_Logic_Project.Business_Person.enMode.AddNew);

            B_PERSON.save();
            person.ID = B_PERSON.ID;
            Cradet_DTO card = Businees_Cradt_Card.Create_New_DTO_Cradete_Card(B_PERSON.ID);

            Businees_Cradt_Card B_Cradte_Card = new Businees_Cradt_Card(card, Businees_Cradt_Card.enmode.ADDNEW);
            B_Cradte_Card.save();
           


             return CreatedAtRoute("ADD PERSON", new {id = person.ID}, person);

          

        }


        [HttpPut("UPDATE_PERSON {id}",Name = "UPDATE_PERSON")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Person_DTO> Ubdate_Person(int id,Person_DTO person)
        {

            if (id<1||string.IsNullOrEmpty(person.F_name) || string.IsNullOrEmpty(person.L_name) || string.IsNullOrEmpty(person.Email) || string.IsNullOrEmpty(person.Password))
            {
                return BadRequest("Invalid person data.");
            }

            Business_Person B_PERSON = Business_Person.GET_PERSON_BY_ID(id);//find object if success change mode ubdate 

            if (B_PERSON != null)
            {
                //ID NOT ALLOW CHANGE .....
                B_PERSON.F_name = person.F_name;
                B_PERSON.L_name = person.L_name;
                B_PERSON.Email = person.Email;
                B_PERSON.Password = person.Password;

                if (B_PERSON.save())

                    return Ok(B_PERSON.SDTO);

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

            if(id < 1)
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





    }
}
