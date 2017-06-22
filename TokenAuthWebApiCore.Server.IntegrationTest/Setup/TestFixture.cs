using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;

namespace TokenAuthWebApiCore.Server.IntegrationTest.Setup
{
	public class TestFixture<TStartup> : IDisposable where TStartup : class
	{
		private readonly TestServer _testServer;
		public HttpClient HttpClient { get; }

		public TestFixture()
		{
			var webHostBuilder = new WebHostBuilder().UseStartup<TStartup>();
			_testServer = new TestServer(webHostBuilder);

			HttpClient = _testServer.CreateClient();
			HttpClient.BaseAddress = new Uri("http://localhost:58834");
		}

		public void Dispose()
		{
			HttpClient.Dispose();
			_testServer.Dispose();
		}
	}
}