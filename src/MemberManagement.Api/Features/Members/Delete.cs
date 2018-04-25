using MediatR;
using MemberManagement.Api.Infrastructure;
using MemberManagement.AppCore.Entities;
using MemberManagement.AppCore.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MemberManagement.Api.Features.Members
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly IMemberRepository _memberRepository;

            public CommandHandler(IMemberRepository memberRepository)
            {
                _memberRepository = memberRepository;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var member = await _memberRepository.GetByIdAsync(request.Id);

                if (member == null)
                {
                    throw new NotFoundException($"No Member found with ID: {request.Id}");
                }

                await _memberRepository.DeleteAsync(member);
            }
        }
    }
}
