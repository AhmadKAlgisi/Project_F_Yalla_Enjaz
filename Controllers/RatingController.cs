using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/Rating")]
    [ApiController]
    public class RatingController : ControllerBase
    {
        [HttpGet("Get_Avrge_Rating_Sarves_By_Id_Student {id_student}", Name = "Get_Avrge_Rating_Sarves_By_Id_Student")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<float> Get_Avrge_Rating_Sarves_By_Id_Student(int id_student)
        {
            if(id_student<1)
            {
                return BadRequest("ERROR :Invaild Data Input ... ");

            }
            try
            {

                float avarge_rating = Business_Rating.Get_Avrge_Rating_Sarves(id_student);
                return Ok(avarge_rating);
            }
            catch
            {
                return   StatusCode(500, new { Message = "EROOR : SERVER ERROR...." });
            }

        }


    }
}
