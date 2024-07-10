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
        private string _email= "foo@gmail.com";
        private string _password="foobar";

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
                email = _email,
                password = _password
            };
            var response = await _httpClient.PostAsJsonAsync("auth/register",details);

            var content = await response.Content.ReadFromJsonAsync<SuccessResponse<AuthResponsedto>>();

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(response.StatusCode,HttpStatusCode.Created);
            Assert.IsNotNull(content.data.accessToken);
            Assert.AreEqual(content.data.user.email, _email);

        }

        public async Task LoginRoute_LogsInUserSuccessfully()
        {
            UserLoginDto request = new(_email, _password);

            var response= await _httpClient.PostAsJsonAsync("auth/login",request);

            var content = await response.Content.ReadFromJsonAsync<SuccessResponse<AuthResponsedto>>();

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsInstanceOfType(content?.data?.user,typeof(UserDataDto));
            Assert.IsNotNull(content.data.accessToken);
        }
    }

    //public class SuccessResponse { 


    //    string message;
    //    UserDataDto user;
    //    string status = "success";
    //        }
}