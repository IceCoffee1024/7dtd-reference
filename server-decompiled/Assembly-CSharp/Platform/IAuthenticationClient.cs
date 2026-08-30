using System;

namespace Platform
{
	// Token: 0x02001B5B RID: 7003
	public interface IAuthenticationClient
	{
		// Token: 0x0600D1D0 RID: 53712
		void Init(IPlatform _owner);

		// Token: 0x0600D1D1 RID: 53713
		string GetAuthTicket();

		// Token: 0x0600D1D2 RID: 53714
		void AuthenticateServer(ClientAuthenticateServerContext _context);

		// Token: 0x0600D1D3 RID: 53715
		void Destroy();
	}
}
