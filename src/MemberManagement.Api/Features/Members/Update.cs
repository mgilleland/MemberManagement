using AutoMapper;
using MediatR;
using MemberManagement.Api.Infrastructure;
using MemberManagement.AppCore.Entities;
using MemberManagement.AppCore.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace MemberManagement.Api.Features.Members
{
    public class Update
    {
        public class Command : MemberModel, IRequest
        {
            public int Id { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly IMapper _mapper;
            private readonly IMemberRepository _memberRepository;

            public CommandHandler(IMapper mapper, IMemberRepository memberRepository)
            {
                _mapper = mapper;
                _memberRepository = memberRepository;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var member = await _memberRepository.GetByIdAsync(request.Id);

                if (member == null)
                {
                    throw new NotFoundException($"No Member found with ID: {request.Id}");
                }

                _mapper.Map(request, member);
                await _memberRepository.UpdateAsync(member);
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Command, Member>();
            }
        }
    }
}
