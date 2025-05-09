using Azure.Core;
using Businees_Logic_Project;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Poject_F_Data_Acsses_Yalla_Enjaz;
namespace Project_F_Yalla_Enjaz.Controllers
{
    [Route("api/Branch_Serves")]
    [ApiController]
    public class Branch_ServesController : ControllerBase
    {
        [HttpGet("GET_ALL_BRANCH_SERVES",Name = "GET_ALL_BRANCH_SERVES")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Branch_Serves_DTO> GetAllBranchServes()
        {
            List<Branch_Serves_DTO> Serves = Business_Branch_Serves.GetAllBranchServes();

            if (Serves != null)
            {
                return Ok(Serves);
            }
            else
                return NotFound("THE DATA NOT FOUND");
        }


        [HttpGet("GET_ALL_BRANCH_SERVES_USING_ID_NAME_SERVES{ID}", Name = "GET_ALL_BRANCH_SERVES_USING_ID_NAME_SERVES")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<Branch_Serves_DTO> GET_ALL_BRANCH_SERVES_USING_ID_NAME_SERVES(int ID)
        {
            var All_Branch_Serves = Business_Branch_Serves.GET_ALL_BRANCH_SERVES_USING_ID_NAME_SERVES(ID);

            if(All_Branch_Serves.Count==0)
            
                return NotFound("NOT FOUND DATA");
            

            else
            
                return Ok(All_Branch_Serves);
         


        }


        [HttpGet("GET_BRANCH_SERVES_BY_ID{iD_Branch_Serves}", Name = "GET_BRANCH_SERVES_BY_ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Person_DTO> GET_BRANCH_SERVES_BY_ID(int iD_Branch_Serves)
        {

            if (iD_Branch_Serves < 1)
            {
                return BadRequest("ERROR: enter data.... ");
            }


            Business_Branch_Serves branch_serves = Business_Branch_Serves.GET_Branch_Serves_By_ID(iD_Branch_Serves);

            if (branch_serves != null)
            {
                Branch_Serves_DTO serves_branch_SDTO = branch_serves.SDTO;

                return Ok(serves_branch_SDTO);
            }
            else
            {
                return NotFound("لا يوجد بيانات ......");
            }


        }


    }
}
