using System;

namespace Platform
{
	// Token: 0x02001B91 RID: 7057
	public interface IAuthenticationServer
	{
		// Token: 0x0600D35A RID: 54106
		void Init(IPlatform _owner);

		// Token: 0x0600D35B RID: 54107
		EBeginUserAuthenticationResult AuthenticateUser(ClientInfo _cInfo);

		// Token: 0x0600D35C RID: 54108
		void RemoveUser(ClientInfo _cInfo);

		// Token: 0x0600D35D RID: 54109
		void StartServer(AuthenticationSuccessfulCallbackDelegate _authSuccessfulDelegate, KickPlayerDelegate _kickPlayerDelegate);

		// Token: 0x0600D35E RID: 54110
		void StartServerSteamGroups(SteamGroupStatusResponse _groupStatusResponseDelegate);

		// Token: 0x0600D35F RID: 54111
		void StopServer();

		// Token: 0x0600D360 RID: 54112
		bool RequestUserInGroupStatus(ClientInfo _cInfo, string _steamIdGroup);
	}
}
