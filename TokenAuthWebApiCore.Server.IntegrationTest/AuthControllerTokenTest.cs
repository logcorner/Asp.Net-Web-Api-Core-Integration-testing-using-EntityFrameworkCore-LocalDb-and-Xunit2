using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TokenAuthWebApiCore.Server.IntegrationTest.Setup;
using TokenAuthWebApiCore.Server.Models;
using Xunit;

namespace TokenAuthWebApiCore.Server.IntegrationTest
{
	[TestCaseOrderer("TokenAuthWebApiCore.Server.IntegrationTest.Setup.PriorityOrderer",
		"TokenAuthWebApiCore.Server.IntegrationTest")]
	public class AuthControllerTokenTest : IClassFixture<TestFixture<TestStartupLocalDb>>
	{
		private HttpClient Client { get; }

		public AuthControllerTokenTest(TestFixture<TestStartupLocalDb> fixture)
		{
			Client = fixture.HttpClient;
		}

		[Fact(DisplayName = "WhenNoRegisteredUser_SignUpForToken_WithValidModelState_Return_OK"), TestPriority(1)]
		public async Task WhenNoRegisteredUser_SignUpForToken_WithValidModelState_Return_OK()
		{
			// Arrange
			var obj = new RegisterViewModel
			{
				Email = "simpleuser@yopmail.com",
				Password = "WebApiCore1#",
				ConfirmPassword = "WebApiCore1#"
			};
			string stringData = JsonConvert.SerializeObject(obj);
			var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
			// Act
			var response = await Client.PostAsync($"/api/auth/register", contentData);

			// Assert
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}

		[Fact(DisplayName = "WhenRegisteredUser_SignIn_WithValidModelState_Return_ValidToken"), TestPriority(2)]
		public async Task WhenRegisteredUser_SignIn_WithValidModelState_Return_ValidToken()
		{
			// Arrange
			var obj = new LoginViewModel
			{
				Email = "simpleuser@yopmail.com",
				Password = "WebApiCore1#"
			};
			string stringData = JsonConvert.SerializeObject(obj);
			var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");
			// Act
			var response = await Client.PostAsync($"/api/auth/token", contentData);
			response.EnsureSuccessStatusCode();
			// Assert

			Assert.Equal(HttpStatusCode.OK, response.StatusCode);

			var jwToken = JsonConvert.DeserializeObject<JwToken>(
				await response.Content.ReadAsStringAsync());
			Assert.True(jwToken.Expiration > DateTime.UtcNow);
			Assert.True(jwToken.Token.Split('.').Length == 3);
		}
	}
}