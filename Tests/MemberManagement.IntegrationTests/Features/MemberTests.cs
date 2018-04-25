using System;
using System.Linq;
using System.Threading;
using AutoMapper;
using MemberManagement.AppCore.Entities;
using MemberManagement.Data;
using MemberManagement.Api.Features.Members;
using MemberManagement.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MemberManagement.IntegrationTests.Features
{
    public class MemberTests
    {
        private readonly MemberManagementContext _context;
        private readonly MemberRepository _repository;

        public MemberTests()
        {
            var dbOptions = new DbContextOptionsBuilder<MemberManagementContext>()
                .UseInMemoryDatabase(databaseName: "TestMember")
                .Options;
            _context = new MemberManagementContext(dbOptions);
            _repository = new MemberRepository(_context);
        }

        [Fact]
        public async void GetListQuery_ReturnsAll()
        {
            ClearMembers();
            AddTestUser();
            Mapper.Initialize(cfg => cfg.CreateMap<Member, Get.Model>());

            var handler = new Get.GetListQueryHandler(Mapper.Instance, _repository);
            var result = await handler.Handle(new Get.GetListQuery(), CancellationToken.None);

            Assert.NotEmpty(result);

            var member = result.First();
            Assert.Equal("TestUserName", member.UserName);
            Assert.Equal("TestFirstName", member.FirstName);
            Assert.Equal("TestLastName", member.LastName);
            Assert.Equal("email@test.com", member.Email);
            Assert.Equal("1234567890", member.PhoneNumber);
            Assert.Equal(DateTime.Now.Date.AddDays(-1), member.DateOfBirth);
        }

        [Fact]
        public async void GetListQuery_ArchiveNotReturned()
        {
            ClearMembers();
            var id = AddTestUser();

            // Soft delete the user that was just added
            var deleteHandler = new Delete.CommandHandler(_repository);
            var command = new Delete.Command
            {
                Id = id
            };

            await deleteHandler.Handle(command, CancellationToken.None);

            // Setup GetListQuery
            Mapper.Initialize(cfg => cfg.CreateMap<Member, Get.Model>());

            var getHandler = new Get.GetListQueryHandler(Mapper.Instance, _repository);
            var result = await getHandler.Handle(new Get.GetListQuery(), CancellationToken.None);

            Assert.Empty(result);
        }

        [Fact]
        public async void GetByIdQuery_ReturnsOne()
        {
            ClearMembers();
            var id = AddTestUser();
            Mapper.Initialize(cfg => cfg.CreateMap<Member, Get.Model>());

            var handler = new Get.GetByIdQueryHandler(Mapper.Instance, _repository);
            var member = await handler.Handle(new Get.GetByIdQuery{Id = id}, CancellationToken.None);

            Assert.Equal("TestUserName", member.UserName);
            Assert.Equal("TestFirstName", member.FirstName);
            Assert.Equal("TestLastName", member.LastName);
            Assert.Equal("email@test.com", member.Email);
            Assert.Equal("1234567890", member.PhoneNumber);
            Assert.Equal(DateTime.Now.Date.AddDays(-1), member.DateOfBirth);
        }

        [Fact]
        public async void AddCommand_AddsOne()
        {
            ClearMembers();
            Mapper.Initialize(cfg => cfg.CreateMap<Add.Command, Member>());

            var handler = new Add.CommandHandler(Mapper.Instance, _repository);
            var command = new Add.Command
            {
                UserName = "TestUserName",
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Email = "email@test.com",
                PhoneNumber = "1234567890",
                DateOfBirth = DateTime.Now.Date.AddDays(-1)
            };
            var memberId = await handler.Handle(command, CancellationToken.None);

            var member = _context.Members.Find(memberId);
            Assert.Equal("TestUserName", member.UserName);
            Assert.Equal("TestFirstName", member.FirstName);
            Assert.Equal("TestLastName", member.LastName);
            Assert.Equal("email@test.com", member.Email);
            Assert.Equal("1234567890", member.PhoneNumber);
            Assert.Equal(DateTime.Now.Date.AddDays(-1), member.DateOfBirth);
            Assert.Equal(DateTime.Now.Date, member.Created.Date);
            Assert.Equal(DateTime.Now.Date, member.LastUpdated.Date);
            Assert.False(member.Archived);
        }

        [Fact]
        public async void UpdateCommand_UpdatesOne()
        {
            ClearMembers();
            var id = AddTestUser();

            Mapper.Initialize(cfg => cfg.CreateMap<Update.Command, Member>());

            var handler = new Update.CommandHandler(Mapper.Instance, _repository);
            var command = new Update.Command
            {
                Id = id,
                UserName = "TestUserName",
                FirstName = "UpdatedFirstName",
                LastName = "TestLastName",
                Email = "email@test.com",
                PhoneNumber = "1234567890",
                DateOfBirth = DateTime.Now.Date.AddDays(-1)
            };

            await handler.Handle(command, CancellationToken.None);

            var member = _context.Members.Find(id);
            Assert.Equal("TestUserName", member.UserName);
            Assert.Equal("UpdatedFirstName", member.FirstName);
            Assert.Equal("TestLastName", member.LastName);
            Assert.Equal("email@test.com", member.Email);
            Assert.Equal("1234567890", member.PhoneNumber);
            Assert.Equal(DateTime.Now.Date.AddDays(-1), member.DateOfBirth);
            Assert.Equal(DateTime.Now.Date, member.Created.Date);
            Assert.Equal(DateTime.Now.Date, member.LastUpdated.Date);
            Assert.False(member.Archived);
        }

        [Fact]
        public async void UpdateCommand_NotFound()
        {
            ClearMembers();

            Mapper.Initialize(cfg => cfg.CreateMap<Update.Command, Member>());

            var handler = new Update.CommandHandler(Mapper.Instance, _repository);
            var command = new Update.Command
            {
                Id = 99999,
                UserName = "TestUserName",
                FirstName = "UpdatedFirstName",
                LastName = "TestLastName",
                Email = "email@test.com",
                PhoneNumber = "1234567890",
                DateOfBirth = DateTime.Now.Date.AddDays(-1)
            };

            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async void DeleteCommand_DeletesOne()
        {
            ClearMembers();
            var id = AddTestUser();

            var handler = new Delete.CommandHandler(_repository);
            var command = new Delete.Command
            {
                Id = id
            };

            await handler.Handle(command, CancellationToken.None);

            var member = _context.Members.Find(id);
            Assert.True(member.Archived);
        }

        [Fact]
        public async void DeleteCommand_NotFound()
        {
            ClearMembers();

            var handler = new Delete.CommandHandler(_repository);
            var command = new Delete.Command
            {
                Id = 99999
            };

            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async void UserNameUniqueQuery_IsTrue()
        {
            ClearMembers();
            AddTestUser();

            var handler = new Get.UserNameUniqueQueryHandler(_repository);
            var command = new Get.UserNameUniqueQuery
            {
                UserName = "NewUserName"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.True(result);
        }

        [Fact]
        public async void UserNameUniqueQuery_IsFalse()
        {
            ClearMembers();
            AddTestUser();

            var handler = new Get.UserNameUniqueQueryHandler(_repository);
            var command = new Get.UserNameUniqueQuery
            {
                UserName = "TestUserName"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.False(result);
        }

        private void ClearMembers()
        {
            var all = _context.Members;
            _context.Members.RemoveRange(all);
            _context.SaveChanges();
        }

        private int AddTestUser()
        {
            var member = new Member()
            {
                UserName = "TestUserName",
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Email = "email@test.com",
                PhoneNumber = "1234567890",
                DateOfBirth = DateTime.Now.Date.AddDays(-1),
                Created = DateTime.Now,
                LastUpdated = DateTime.Now,
                Archived = false
            };

            _context.Members.Add(member);
            _context.SaveChanges();

            return member.Id;
        }
    }
}
