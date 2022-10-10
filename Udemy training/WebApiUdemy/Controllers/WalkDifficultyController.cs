using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiUdemy.Repositories;

namespace WebApiUdemy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultyController(Repositories.IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.mapper = mapper;
            this. walkDifficultyRepository = walkDifficultyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GettAllWalkDifficulties()
        {
            var walkDif = await walkDifficultyRepository.GetWalkDifficultyListAsync();   
            var walkDifDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDif);

            return Ok(walkDifDTO);

         

        }

        [HttpGet]
        [Route("{id:guid}")]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetWalkDifficultyById(Guid id)
        {
            var walkDif = await walkDifficultyRepository.GetAsync(id);
            if(walkDif==null)
            {
                return NotFound();
            }

            //Convert domain to DTO
            var WalkDifDTO = new Models.Domain.WalkDifficulty
            {
                Id = walkDif.Id,
                Code = walkDif.Code,
            };

            return Ok(WalkDifDTO);
        }


        [HttpPost]
        [ActionName("AddWalkDifficulty")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalkDifficulty(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {

            //validtae input request
            if(!ValidateAddWalkDifficulty(addWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }

            var walkDifDomain = new Models.Domain.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code,
            };

            walkDifDomain= await walkDifficultyRepository.AddWalkDifficultyAsync(walkDifDomain);

            var walkDifDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifDomain);
            return CreatedAtAction(nameof(AddWalkDifficulty), new {id= walkDifDTO.Id}, walkDifDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateWalkDif(Guid id,
            Models.DTO.UpdateWalkDifficultyReauest updateWalkDifficultyReauest)
        {
            //validate incoming request

            if(!ValidateUpdateWalkDif(updateWalkDifficultyReauest))
            {
                return BadRequest(ModelState);
            }

            var walkDifDomain = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultyReauest.Code,
            };

            walkDifDomain = await walkDifficultyRepository.UpdateWalkDifficultyAsync(id, walkDifDomain);
            if(walkDifDomain==null)
            {
                return NotFound();
            }
            var walkDifDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifDomain);
            return Ok(walkDifDTO);
        }
        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteWalkDifficulty(Guid id)
        {
            var walkDifDomain = await walkDifficultyRepository.DeleteWalkDifficultyAsync(id);
            if(walkDifDomain==null)
            {
                return NotFound();
            }
            var WalkDifDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifDomain);
            return Ok(WalkDifDTO);
        }

        #region Private methods

        private bool ValidateAddWalkDifficulty(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            if(addWalkDifficultyRequest==null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest), $"Please add walk difficulty data");
                return false;
            }
            if(string.IsNullOrWhiteSpace( addWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code),$" Please enter correct code");
            }
            if( ModelState.ErrorCount>0)
            {
                return false;
            }
            return true;
        }

        private bool ValidateUpdateWalkDif(Models.DTO.UpdateWalkDifficultyReauest updateWalkDifficultyReauest)
        {
            if(updateWalkDifficultyReauest==null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyReauest), $" Please enter walk difficulty data");
                return false;
            }
            if(string.IsNullOrWhiteSpace(updateWalkDifficultyReauest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyReauest.Code), $" invalid code");
            }
            if(ModelState.ErrorCount>0)
            {
                return false;
            }
            return true;
        }


        #endregion

    }
}
