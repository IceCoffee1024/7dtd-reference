using System;

namespace Platform
{
	// Token: 0x02001B38 RID: 6968
	public interface IAuthenticationServer
	{
		// Token: 0x0600D05A RID: 53338
		void Init(IPlatform _owner);

		// Token: 0x0600D05B RID: 53339
		EBeginUserAuthenticationResult AuthenticateUser(ClientInfo _cInfo);

		// Token: 0x0600D05C RID: 53340
		void RemoveUser(ClientInfo _cInfo);

		// Token: 0x0600D05D RID: 53341
		void StartServer(AuthenticationSuccessfulCallbackDelegate _authSuccessfulDelegate, KickPlayerDelegate _kickPlayerDelegate);

		// Token: 0x0600D05E RID: 53342
		void StartServerSteamGroups(SteamGroupStatusResponse _groupStatusResponseDelegate);

		// Token: 0x0600D05F RID: 53343
		void StopServer();

		// Token: 0x0600D060 RID: 53344
		bool RequestUserInGroupStatus(ClientInfo _cInfo, string _steamIdGroup);
	}
}
