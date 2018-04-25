using MediatR;
using MemberManagement.Api.Attributes;
using MemberManagement.Api.Features.Members;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MemberManagement.Api.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Member")]
    public class MemberController : Controller
    {
        private readonly IMediator _mediator;

        public MemberController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _mediator.Send(new Get.GetListQuery()));
        }

        [HttpGet("{id}", Name = "GetMember")]
        public async Task<IActionResult> GetById(int id)
        {
            var member = await _mediator.Send(new Get.GetByIdQuery { Id = id });

            if (member != null)
            {
                return Ok(member);
            }

            return NotFound();
        }

        [HttpGet("IsUserNameUnique/{userName}")]
        public async Task<IActionResult> IsUserNameUnique(string userName)
        {
            var member = await _mediator.Send(new Get.UserNameUniqueQuery { UserName = userName });
            return Ok(member);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] Add.Command command)
        {
            var keyId = await _mediator.Send(command);

            return CreatedAtRoute("GetMember", new { id = keyId }, keyId);
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Update(int id, [FromBody]Update.Command command)
        {
            if (command == null || command.Id != id)
            {
                return BadRequest();
            }

            await _mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new Delete.Command { Id = id };

            await _mediator.Send(command);

            return NoContent();
        }
    }
}