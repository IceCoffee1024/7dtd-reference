using System;

namespace Platform
{
	// Token: 0x02001B34 RID: 6964
	public interface IAuthenticationClient
	{
		// Token: 0x0600D047 RID: 53319
		void Init(IPlatform _owner);

		// Token: 0x0600D048 RID: 53320
		string GetAuthTicket();

		// Token: 0x0600D049 RID: 53321
		void AuthenticateServer(ClientAuthenticateServerContext _context);

		// Token: 0x0600D04A RID: 53322
		void Destroy();
	}
}
