using System;

namespace TokenAuthWebApiCore.Server.IntegrationTest.Setup
{
	public class JwToken
	{
		public string Token { get; set; }
		public DateTime Expiration { get; set; }
	}
}