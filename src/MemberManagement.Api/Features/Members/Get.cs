using AutoMapper;
using MediatR;
using MemberManagement.AppCore.Entities;
using MemberManagement.AppCore.Interfaces;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace MemberManagement.Api.Features.Members
{
    public class Get
    {
        public class Model : MemberModel
        {
            public int Id { get; set; }
        }

        //***********************************
        // Get a filtered list of Members
        //***********************************

        public class GetListQuery : IRequest<List<Model>> { }

        public class GetListQueryHandler : IRequestHandler<GetListQuery, List<Model>>
        {
            private readonly IMapper _mapper;
            private readonly IMemberRepository _memberRepository;

            public GetListQueryHandler(IMapper mapper, IMemberRepository memberRepository)
            {
                _mapper = mapper;
                _memberRepository = memberRepository;
            }

            public async Task<List<Model>> Handle(GetListQuery request, CancellationToken cancellationToken)
            {
                return _mapper.Map<List<Model>>(await _memberRepository.ListAllAsync());
            }
        }

        //***********************************
        // Get a Member by ID
        //***********************************

        public class GetByIdQuery : IRequest<Model>
        {
            public int Id { get; set; }
        }

        public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, Model>
        {
            private readonly IMapper _mapper;
            private readonly IMemberRepository _memberRepository;

            public GetByIdQueryHandler(IMapper mapper, IMemberRepository memberRepository)
            {
                _mapper = mapper;
                _memberRepository = memberRepository;
            }

            public async Task<Model> Handle(GetByIdQuery request, CancellationToken cancellationToken)
            {
                return _mapper.Map<Model>(await _memberRepository.GetByIdAsync(request.Id));
            }
        }

        //***************************************
        // Check to see if a User Name is unique
        //***************************************

        public class UserNameUniqueQuery : IRequest<bool>
        {
            public string UserName { get; set; }
        }

        public class UserNameUniqueQueryHandler : IRequestHandler<UserNameUniqueQuery, bool>
        {
            private readonly IMemberRepository _memberRepository;

            public UserNameUniqueQueryHandler(IMemberRepository memberRepository)
            {
                _memberRepository = memberRepository;
            }

            public async Task<bool> Handle(UserNameUniqueQuery request, CancellationToken cancellationToken)
            {
                return await _memberRepository.IsUserNameUniqueAsync(request.UserName);
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Member, Model>();
            }
        }
    }
}
