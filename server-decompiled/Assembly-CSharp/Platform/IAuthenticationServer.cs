using System;

namespace Platform
{
	// Token: 0x02001B5F RID: 7007
	public interface IAuthenticationServer
	{
		// Token: 0x0600D1E3 RID: 53731
		void Init(IPlatform _owner);

		// Token: 0x0600D1E4 RID: 53732
		EBeginUserAuthenticationResult AuthenticateUser(ClientInfo _cInfo);

		// Token: 0x0600D1E5 RID: 53733
		void RemoveUser(ClientInfo _cInfo);

		// Token: 0x0600D1E6 RID: 53734
		void StartServer(AuthenticationSuccessfulCallbackDelegate _authSuccessfulDelegate, KickPlayerDelegate _kickPlayerDelegate);

		// Token: 0x0600D1E7 RID: 53735
		void StartServerSteamGroups(SteamGroupStatusResponse _groupStatusResponseDelegate);

		// Token: 0x0600D1E8 RID: 53736
		void StopServer();

		// Token: 0x0600D1E9 RID: 53737
		bool RequestUserInGroupStatus(ClientInfo _cInfo, string _steamIdGroup);
	}
}
