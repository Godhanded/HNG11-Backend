using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Security.Cryptography;
using UserOrgs.Dto;

namespace Auth.Spec
{
    [TestClass]
    public class UserOrgTest
    {
        private HttpClient _httpClient;

        public UserOrgTest()
        {
            var app = new WebApplicationFactory<Program>();
            _httpClient = app.CreateDefaultClient();
        }
        [TestMethod]
        public async Task RegisterRoute_RegistersUserAndCreatesOrganisationSuccessfully()
        {
            

            UserRegisterDto details = new()
            {
                firstName = "foo",
                lastName = "bar",
                email = $"foo@gmail.com",
                password = "foobar"
            };
            var response = await _httpClient.PostAsJsonAsync("auth/register",details);

            var content = await response.Content.ReadFromJsonAsync<SuccessResponse>();

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(response.StatusCode,HttpStatusCode.Created);

        }
    }

    //public class SuccessResponse { 


    //    string message;
    //    UserDataDto user;
    //    string status = "success";
    //        }
}