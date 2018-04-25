using MemberManagement.AppCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberManagement.Data
{
    public class MemberManagementContextSeed
    {
        public static async Task SeedAsync(MemberManagementContext context, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                if (!context.Members.Any())
                {
                    context.Members.AddRange(GetPreconfiguredMembers());

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    await SeedAsync(context, retryForAvailability);
                }
            }
        }

        private static IEnumerable<Member> GetPreconfiguredMembers()
        {
            return new List<Member>()
            {
                new Member() { UserName = "LSkywalker", FirstName = "Luke", LastName = "Skywalker", Email = "luke@test.com",
                               PhoneNumber = "8005551212", DateOfBirth = DateTime.Parse("01/01/77"), Created = DateTime.Now,
                               LastUpdated = DateTime.Now, Archived = false},
                new Member() { UserName = "LOrgana", FirstName = "Leia", LastName = "Organa", Email = "leia@test.com",
                               PhoneNumber = "8005551212", DateOfBirth = DateTime.Parse("01/02/77"), Created = DateTime.Now,
                               LastUpdated = DateTime.Now, Archived = false},
                new Member() { UserName = "HSolo", FirstName = "Han", LastName = "Solo", Email = "han@test.com",
                               PhoneNumber = "8005551212", DateOfBirth = DateTime.Parse("01/03/77"), Created = DateTime.Now,
                               LastUpdated = DateTime.Now, Archived = false},
                new Member() { UserName = "OKenobi", FirstName = "Obi Wan", LastName = "Kenobi", Email = "obiwan@test.com",
                               PhoneNumber = "8005551212", DateOfBirth = DateTime.Parse("01/04/77"), Created = DateTime.Now,
                               LastUpdated = DateTime.Now, Archived = false},
                new Member() { UserName = "DVader", FirstName = "Darth", LastName = "Vader", Email = "vader@test.com",
                               PhoneNumber = "8005551212", DateOfBirth = DateTime.Parse("01/05/77"), Created = DateTime.Now,
                               LastUpdated = DateTime.Now, Archived = false},
                new Member() { UserName = "LCalrissian", FirstName = "Lando", LastName = "Calrissian", Email = "lando@test.com",
                               PhoneNumber = "8005551212", DateOfBirth = DateTime.Parse("01/06/80"), Created = DateTime.Now,
                               LastUpdated = DateTime.Now, Archived = false},
                new Member() { UserName = "Yoda", FirstName = "Yoda", LastName = "GuessYouMust", Email = "yoda@test.com",
                               PhoneNumber = "8005551212", DateOfBirth = DateTime.Parse("01/07/80"), Created = DateTime.Now,
                               LastUpdated = DateTime.Now, Archived = false},
                new Member() { UserName = "Rey", FirstName = "Rey", LastName = "IfOnly", Email = "rey@test.com",
                               PhoneNumber = "8005551212", DateOfBirth = DateTime.Parse("01/08/15"), Created = DateTime.Now,
                               LastUpdated = DateTime.Now, Archived = false},
                new Member() { UserName = "KRen", FirstName = "Kylo", LastName = "Ren", Email = "ben@test.com",
                               PhoneNumber = "8005551212", DateOfBirth = DateTime.Parse("01/09/15"), Created = DateTime.Now,
                               LastUpdated = DateTime.Now, Archived = false},
                new Member() { UserName = "Finn", FirstName = "Finn", LastName = "FN-2187", Email = "finn@test.com",
                               PhoneNumber = "8005551212", DateOfBirth = DateTime.Parse("01/10/15"), Created = DateTime.Now,
                               LastUpdated = DateTime.Now, Archived = false},
                new Member() { UserName = "PDameron", FirstName = "Poe", LastName = "Dameron", Email = "poe@test.com",
                               PhoneNumber = "8005551212", DateOfBirth = DateTime.Parse("01/11/15"), Created = DateTime.Now,
                               LastUpdated = DateTime.Now, Archived = false},
                new Member() { UserName = "MKanata", FirstName = "Maz", LastName = "Kanata", Email = "maz@test.com",
                               PhoneNumber = "8005551212", DateOfBirth = DateTime.Parse("01/12/15"), Created = DateTime.Now,
                               LastUpdated = DateTime.Now, Archived = false}
            };
        }
    }
}
