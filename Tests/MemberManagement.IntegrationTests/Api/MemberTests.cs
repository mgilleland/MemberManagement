using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MemberManagement.Api.Features.Members;
using Newtonsoft.Json;
using Xunit;

namespace MemberManagement.IntegrationTests.Api
{
    public class MemberTests : BaseWebTest
    {
        [Fact]
        public async Task GetAll_ReturnsList()
        {
            var response = await _client.GetAsync("/api/member");
            Assert.True(response.IsSuccessStatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<List<MemberModel>>(stringResponse);

            Assert.NotEmpty(model);
        }

        [Fact]
        public async Task GetById_ReturnsOne()
        {
            var response = await _client.GetAsync("/api/member/1");
            Assert.True(response.IsSuccessStatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            var member = JsonConvert.DeserializeObject<MemberModel>(stringResponse);

            Assert.Equal("LSkywalker", member.UserName);
            Assert.Equal("Luke", member.FirstName);
            Assert.Equal("Skywalker", member.LastName);
            Assert.Equal("luke@test.com", member.Email);
            Assert.Equal("8005551212", member.PhoneNumber);
            Assert.Equal(DateTime.Parse("01/01/77"), member.DateOfBirth);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/member/99999");

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_AddsOne()
        {
            var command = new Add.Command
            {
                UserName = "TestCreate",
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Email = "email@test.com",
                PhoneNumber = "1234567890",
                DateOfBirth = DateTime.Now.Date.AddDays(-1)
            };

            var response = await _client.PostAsJsonAsync("/api/member", command);

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest()
        {
            var command = new Add.Command
            {
                UserName = "",
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Email = "email",
                PhoneNumber = "xxx",
                DateOfBirth = DateTime.Now.Date.AddDays(+1)
            };

            var response = await _client.PostAsJsonAsync("/api/member", command);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_UpdatesOne()
        {
            var command = new Update.Command
            {
                Id = 1,
                UserName = "LSkywalker",
                FirstName = "Luke",
                LastName = "Skywalker",
                Email = "luke@jedi.com",
                PhoneNumber = "8005551212",
                DateOfBirth = DateTime.Parse("01/01/77")
            };

            var response = await _client.PutAsJsonAsync("/api/member/1", command);

            Assert.True(response.IsSuccessStatusCode);

            var getResponse = await _client.GetAsync("/api/member/1");
            Assert.True(getResponse.IsSuccessStatusCode);

            var stringResponse = await getResponse.Content.ReadAsStringAsync();
            var member = JsonConvert.DeserializeObject<MemberModel>(stringResponse);

            Assert.Equal("LSkywalker", member.UserName);
            Assert.Equal("Luke", member.FirstName);
            Assert.Equal("Skywalker", member.LastName);
            Assert.Equal("luke@jedi.com", member.Email);
            Assert.Equal("8005551212", member.PhoneNumber);
            Assert.Equal(DateTime.Parse("01/01/77"), member.DateOfBirth);

        }

        [Fact]
        public async Task Update_ReturnsBadRequest()
        {
            var command = new Update.Command
            {
                Id = 1,
                UserName = "",
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Email = "email",
                PhoneNumber = "xxx",
                DateOfBirth = DateTime.Now.Date.AddDays(+1)
            };

            var response = await _client.PutAsJsonAsync("/api/member/1", command);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_ReturnsNotFound()
        {
            var command = new Update.Command
            {
                Id = 99999,
                UserName = "LSkywalker",
                FirstName = "Luke",
                LastName = "Skywalker",
                Email = "luke@jedi.com",
                PhoneNumber = "8005551212",
                DateOfBirth = DateTime.Parse("01/01/77")
            };

            var response = await _client.PutAsJsonAsync("/api/member/99999", command);

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound()
        {
            var response = await _client.DeleteAsync("/api/member/99999");

            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_DeletesOne()
        {
            var response = await _client.DeleteAsync("/api/member/5");

            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var getResponse = await _client.GetAsync("/api/member/5");

            Assert.False(getResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        [Fact]
        public async Task UserNameUniqueQuery_ReturnsFalse()
        {
            var response = await _client.GetAsync("/api/member/IsUserNameUnique/LSkywalker");

            Assert.True(response.IsSuccessStatusCode);

            var stringResponse = await response.Content.ReadAsStringAsync();
            var isUnique = JsonConvert.DeserializeObject<bool>(stringResponse);

            Assert.False(isUnique);
        }
    }
}
