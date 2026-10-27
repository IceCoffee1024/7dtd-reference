using System;

namespace Platform
{
	// Token: 0x02001BBE RID: 7102
	public interface IAuthenticationClient
	{
		// Token: 0x0600D45B RID: 54363
		void Init(IPlatform _owner);

		// Token: 0x0600D45C RID: 54364
		string GetAuthTicket();

		// Token: 0x0600D45D RID: 54365
		void AuthenticateServer(ClientAuthenticateServerContext _context);

		// Token: 0x0600D45E RID: 54366
		void Destroy();
	}
}
