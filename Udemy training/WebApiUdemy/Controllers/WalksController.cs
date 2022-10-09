using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApiUdemy.Models.DTO;
using WebApiUdemy.Repositories;

namespace WebApiUdemy.Controllers
{
    [ApiController]
    [Route("Walks")]
    public class WalksController : Controller
    {
        private readonly IWalksRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalksRepository walkRepository, IMapper mapper,
            IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            //Fetch data from database-domain walks
            var walks = await walkRepository.GetAllAsync();
            // convert data to DTO
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walks);
            //return response
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            var walk= await walkRepository.GetAsync(id);
            if(walk==null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
            return Ok(walkDTO);
        }

        [HttpPost]
        [Route("Walks")]
        [ActionName("AddWalkAsync")]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {

            //validtae incoming request

            if(!(await ValidateAddWalkAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }


            //convert DTO to Domain object

            var walkDomain = new Models.Domain.Walk
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkdifficultyId = addWalkRequest.WalkdifficultyId,                
            };

            //pass Domain object to Repository to persist this
            await walkRepository.AddWalkAsync(walkDomain);

            //convert Domain object back to DTO
            //can use map instead

            var WalkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkdifficultyId = walkDomain.WalkdifficultyId,
            };

            //send DTO response back to client
            return CreatedAtAction(nameof(AddWalkAsync), new {id= WalkDTO .Id}, WalkDTO);
        }




        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] UpdateWalkRequest updateWalkRequest)
        {
            ///validate incoming request
            ///
            if(!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }

            //Convert DTO to Domain model
            var walk = new Models.Domain.Walk
            {
                Name = updateWalkRequest.Name,
                Length = updateWalkRequest.Length,
                RegionId = updateWalkRequest.RegionId,
                WalkdifficultyId = updateWalkRequest.WalkdifficultyId,
            };
            //Update region using repository
            walk = await walkRepository.UpdateAsync(id, walk);

            //if null, notFound

            if (walk == null)
            {
                return NotFound();
            }


            //Convert Domain back to DTO

            var walkDTO = new Models.DTO.Walk
            {
                
                Name = walk.Name,
                Length = walk.Length,
                RegionId = walk.RegionId,
                WalkdifficultyId = walk.WalkdifficultyId,
            };

            //Retorn OK response
            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            var walkDomain= await walkRepository.DeleteAsync(id);
            if(walkDomain==null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
            return Ok(walkDTO);
        }


        #region Private methods
        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if(addWalkRequest==null)
            {
                ModelState.AddModelError(nameof(addWalkRequest), $"Please add walks data");
                return false;
            }
            if(string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)}Can not be empty");
            }
            if (addWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length), $"{nameof(addWalkRequest.Length)} can not be less than 0");
            }

            var region = await regionRepository.GetAsync(addWalkRequest.RegionId);
                if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} does not exist");
            }

            var walkDifficulty =await walkDifficultyRepository.GetAsync(addWalkRequest.WalkdifficultyId);
            if(walkDifficulty==null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkdifficultyId), $"{nameof(addWalkRequest.WalkdifficultyId)} does not exist");
            }
            if(ModelState.ErrorCount>0)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> ValidateUpdateWalkAsync(UpdateWalkRequest updateWalkRequest)
        {
            if (updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest), $"Please add walks data");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{nameof(updateWalkRequest.Name)} can not be empty");
            }
            if (updateWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.Length), $"{nameof(updateWalkRequest.Length)} can not be less than 0");
            }

            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId), $"{nameof(updateWalkRequest.RegionId)} does not exist");
            }

            var walkDifficulty = await walkDifficultyRepository.GetAsync(updateWalkRequest.WalkdifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkdifficultyId), $"{nameof(updateWalkRequest.WalkdifficultyId)} does not exist");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }


        #endregion
    }
}
