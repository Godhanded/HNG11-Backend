using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using UserOrgs.Dto;

namespace Auth.Spec
{
    [TestClass]
    public class UserOrgTest
    {
        private HttpClient _httpClient;
        private string _password = "foobar";

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
                email = "email1@gmail.com",
                password = _password
            };
            var response = await _httpClient.PostAsJsonAsync("auth/register", details);

            var content = await response.Content.ReadFromJsonAsync<SuccessResponse<AuthResponsedto>>();

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Assert.IsNotNull(content!.data!.accessToken);
            Assert.AreEqual(content.data.user.email, "email1@gmail.com");

        }

        [TestMethod]
        public async Task LoginRoute_LogsInUserSuccessfully()
        {
            UserRegisterDto details = new()
            {
                firstName = "foo",
                lastName = "bar",
                email = "email2@gmail.com",
                password = _password
            };
            await _httpClient.PostAsJsonAsync("auth/register", details);
            UserLoginDto request = new("email2@gmail.com", _password);

            var response = await _httpClient.PostAsJsonAsync("auth/login", request);

            var content = await response.Content.ReadFromJsonAsync<SuccessResponse<AuthResponsedto>>();

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.IsInstanceOfType(content?.data?.user, typeof(UserDataDto));
            Assert.IsNotNull(content.data.accessToken);
        }

        [TestMethod]
        public async Task GetUsersOrganisationsRoute_GetsUserDefaultOrganisationInList()
        {
            //Arrange
            UserRegisterDto details = new()
            {
                firstName = "foo",
                lastName = "bar",
                email = "emailorg@gmail.com",
                password = _password
            };
            var response = await _httpClient.PostAsJsonAsync("auth/register", details);
            var Authcontent = await response.Content.ReadFromJsonAsync<SuccessResponse<AuthResponsedto>>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Authcontent!.data!.accessToken);

            // Act
            var organisationresponse = await _httpClient.GetAsync("api/organisations");
            var orgContent = await organisationresponse.Content.ReadFromJsonAsync<SuccessResponse<Dictionary<string, IEnumerable<OrganisationDto>>>>();

            // Assert
            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual("foo's Organisation", orgContent!.data!["organisations"].First().name);
            Assert.IsNotNull(orgContent);
        }


    }

    //public class SuccessResponse { 


    //    string message;
    //    UserDataDto user;
    //    string status = "success";
    //        }
}