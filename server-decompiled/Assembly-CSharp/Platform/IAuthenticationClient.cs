using System;

namespace Platform
{
	// Token: 0x02001B8D RID: 7053
	public interface IAuthenticationClient
	{
		// Token: 0x0600D347 RID: 54087
		void Init(IPlatform _owner);

		// Token: 0x0600D348 RID: 54088
		string GetAuthTicket();

		// Token: 0x0600D349 RID: 54089
		void AuthenticateServer(ClientAuthenticateServerContext _context);

		// Token: 0x0600D34A RID: 54090
		void Destroy();
	}
}
