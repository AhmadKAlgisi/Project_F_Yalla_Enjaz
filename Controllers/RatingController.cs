using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Poject_F_Data_Acsses_Yalla_Enjaz;

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

        [HttpGet("Get_Ratings_By_Student_ID{ID_STUDENT}", Name = "Get_Ratings_By_Student_ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Name_Serves_DTO> Get_Ratings_By_Student_ID(int ID_STUDENT)
        {
            if (ID_STUDENT < 0)
                return BadRequest("ID غير صالح");
            try
            {
                var comment_serves_list = Business_Rating.Get_Ratings_By_Student_ID(ID_STUDENT);

                if (comment_serves_list.Count == 0)
                {
                    return NotFound("لا يوجد تعليقات على الخدمة متاحة  ");
                }
                else
                    return Ok(comment_serves_list);
            }
            catch { return StatusCode(500, new { Message = "EROOR :Server Error" }); }
        }




        [HttpGet("GET_RATING_BY_ID_ORDERS{ID_Order}", Name = "GET_RATING_BY_ID_ORDERS")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List_Rating_DTO> GET_RATING_BY_ID_ORDERS(int ID_Order)
        {
            if (ID_Order < 0)
                return BadRequest("ID غير صالح");
            try
            {
                List_Rating_DTO rating_order = Business_Rating.Get_Ratings_By_ID_Order(ID_Order);

                if (rating_order==null)
                {
                    return NotFound("لا يوجد تعليقات على الطلب متاحة  ");
                }
                else
                    return Ok(rating_order);
            }
            catch { return StatusCode(500, new { Message = "EROOR :Server Error" }); }
        }




        [HttpPost("Add_Rating", Name = "Add_Rating")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Rating_DTO> Add_Rating(Rating_DTO Rating)
        {
            if (string.IsNullOrEmpty(Rating.Comment) ||Rating.ID_Orders<0|| Rating.Rating<0)
            {
                return BadRequest("Invalid person data.");
            }

         
            Business_Rating B_rating = new Business_Rating(Rating, Business_Rating.enmode.ADDNEW);
            if (B_rating.save())
            {
                Rating.ID = B_rating.ID;
                return Ok(B_rating.SDTO);
            }
            else
                return StatusCode(500, new { Message = "EROOR : NOT UBDATE DATA ...." });


        }



    }
}
