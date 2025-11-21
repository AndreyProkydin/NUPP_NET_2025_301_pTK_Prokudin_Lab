using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restouran.Infrastructure;
using Restouran.Infrastructure.Models;
using Restouran.REST.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restouran.REST.Controllers
{

    [ApiController]
    [Route("api/[controller]")] 
    public class TagsController : ControllerBase
    {
        private readonly ICrudServiceAsync<Tag> _tagService;

        public TagsController(ICrudServiceAsync<Tag> tagService)
        {
            _tagService = tagService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagModel>>> GetTags()
        {
            var tags = await _tagService.ReadAllAsync();

            var tagModels = tags.Select(t => new TagModel
            {
                Id = t.Id,
                Name = t.Name
            });

            return Ok(tagModels); 
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<TagModel>> GetTag(int id)
        {
            var tag = await _tagService.ReadAsync(id);

            if (tag == null)
            {
                return NotFound(); 
            }

            var tagModel = new TagModel
            {
                Id = tag.Id,
                Name = tag.Name
            };

            return Ok(tagModel);
        }

        [Authorize(Roles = "Administrator, Moderator, CommonUser")]
        [HttpPost]
        public async Task<ActionResult<TagModel>> PostTag(TagRequestModel requestModel)
        {
            var newTag = new Tag
            {
                Name = requestModel.Name
            };

            await _tagService.CreateAsync(newTag);
            var tagModel = new TagModel
            {
                Id = newTag.Id,
                Name = newTag.Name
            };

            return CreatedAtAction(nameof(GetTag), new { id = tagModel.Id }, tagModel);
        }

        [Authorize(Roles = "Administrator, Moderator")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTag(int id, TagRequestModel requestModel)
        {
            var tagToUpdate = await _tagService.ReadAsync(id);

            if (tagToUpdate == null)
            {
                return NotFound();
            }

            tagToUpdate.Name = requestModel.Name;

            await _tagService.UpdateAsync(tagToUpdate);

            return NoContent(); 
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            var tagToDelete = await _tagService.ReadAsync(id);

            if (tagToDelete == null)
            {
                return NotFound(); 
            }

            await _tagService.RemoveAsync(tagToDelete);

            return NoContent();
        }
    }
}
