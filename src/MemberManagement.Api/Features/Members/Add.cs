using AutoMapper;
using MediatR;
using MemberManagement.Api.Infrastructure;
using MemberManagement.AppCore.Entities;
using MemberManagement.AppCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace MemberManagement.Api.Features.Members
{
    public class Add
    {
        public class Command : MemberModel, IRequest<int> { }

        public class CommandHandler : IRequestHandler<Command, int>
        {
            private readonly IMapper _mapper;
            private readonly IMemberRepository _memberRepository;

            public CommandHandler(IMapper mapper, IMemberRepository memberRepository)
            {
                _mapper = mapper;
                _memberRepository = memberRepository;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                var member = _mapper.Map<Member>(request);

                try
                {
                    await _memberRepository.AddAsync(member);
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("duplicate key"))
                    {
                        throw new DuplicateKeyException(ex.InnerException.Message);
                    }

                    throw;
                }

                return member.Id;
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
