using System;

namespace Platform
{
	// Token: 0x02001BC2 RID: 7106
	public interface IAuthenticationServer
	{
		// Token: 0x0600D46E RID: 54382
		void Init(IPlatform _owner);

		// Token: 0x0600D46F RID: 54383
		EBeginUserAuthenticationResult AuthenticateUser(ClientInfo _cInfo);

		// Token: 0x0600D470 RID: 54384
		void RemoveUser(ClientInfo _cInfo);

		// Token: 0x0600D471 RID: 54385
		void StartServer(AuthenticationSuccessfulCallbackDelegate _authSuccessfulDelegate, KickPlayerDelegate _kickPlayerDelegate);

		// Token: 0x0600D472 RID: 54386
		void StartServerSteamGroups(SteamGroupStatusResponse _groupStatusResponseDelegate);

		// Token: 0x0600D473 RID: 54387
		void StopServer();

		// Token: 0x0600D474 RID: 54388
		bool RequestUserInGroupStatus(ClientInfo _cInfo, string _steamIdGroup);
	}
}
