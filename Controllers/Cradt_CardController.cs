using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Poject_F_Data_Acsses_Yalla_Enjaz;

namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Cradt_CardController : ControllerBase
    {


        [HttpGet("GET_CRADET_CARD_BY_ID{ID_Cradet}", Name = "GET_CRADET_CARD_BY_ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Cradet_Info_DTO>GET_CRADET_CARD_BY_ID(int ID_Cradet)
        {
            if (ID_Cradet < 1)
                return BadRequest("BAD DATA INPUT ....");


            Businees_Cradt_Card B_card = Businees_Cradt_Card.GET_CRADTE_CARD_BY_ID(ID_Cradet);

            if (B_card != null)
            {
                Business_Person B_person = Business_Person.GET_PERSON_BY_ID(B_card.ID_person);
                Cradet_Info_DTO Info_card = B_card.Create_Object_Cradte_Info_Dto(B_person);
                return Ok(Info_card);
            }
            else
                return NotFound("THE ID OBJECT NOT FOUND ....");



        }




        [HttpGet("GET_CRADET_CARD_BY_ID_Person", Name = "GET_CRADET_CARD_BY_ID_Person")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Cradet_Info_DTO> GET_CRADET_CARD_BY_ID_Person(int ID_Person)
        {
            if (ID_Person < 1)
                return BadRequest("BAD DATA INPUT ....");


            Cradet_Info_DTO card = Businees_Cradt_Card.GET_CRADTE_CARD_BY_ID_Person(ID_Person);

            if (card != null)
            {
              
                return Ok(card);
            }
            else
                return NotFound("THE ID OBJECT NOT FOUND ....");



        }



    }
}
