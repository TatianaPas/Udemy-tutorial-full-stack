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

        public WalksController(IWalksRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
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

    }
}
