using System;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001C5A RID: 7258
	public class AuthenticationServer : IAuthenticationServer
	{
		// Token: 0x0600D6CB RID: 54987 RVA: 0x004D5F8B File Offset: 0x004D418B
		public void Init(IPlatform _owner)
		{
			this.owner = _owner;
			this.owner.Api.ClientApiInitialized += delegate
			{
				Log.Out("[Steamworks.NET] Registering auth callbacks");
				if (this.m_validateAuthTicketResponse == null)
				{
					this.m_validateAuthTicketResponse = Callback<ValidateAuthTicketResponse_t>.CreateGameServer(new Callback<ValidateAuthTicketResponse_t>.DispatchDelegate(this.ValidateAuthTicketResponse));
				}
				if (this.m_gsClientGroupStatus == null)
				{
					this.m_gsClientGroupStatus = Callback<GSClientGroupStatus_t>.CreateGameServer(new Callback<GSClientGroupStatus_t>.DispatchDelegate(this.GsClientGroupStatus));
				}
			};
		}

		// Token: 0x0600D6CC RID: 54988 RVA: 0x004D5FB0 File Offset: 0x004D41B0
		public EBeginUserAuthenticationResult AuthenticateUser(ClientInfo _cInfo)
		{
			Log.Out("[Steamworks.NET] Auth.AuthenticateUser()");
			UserIdentifierSteam userIdentifierSteam = (UserIdentifierSteam)_cInfo.PlatformId;
			CSteamID csteamID = new CSteamID(userIdentifierSteam.SteamId);
			byte[] ticket = userIdentifierSteam.Ticket;
			if (ticket == null || ticket.Length == 0)
			{
				return EBeginUserAuthenticationResult.InvalidTicket;
			}
			EBeginAuthSessionResult ebeginAuthSessionResult = SteamGameServer.BeginAuthSession(ticket, ticket.Length, csteamID);
			string[] array = new string[8];
			array[0] = "[Steamworks.NET] Authenticating player: ";
			array[1] = _cInfo.playerName;
			array[2] = " SteamId: ";
			int num = 3;
			CSteamID csteamID2 = csteamID;
			array[num] = csteamID2.ToString();
			array[4] = " TicketLen: ";
			array[5] = ticket.Length.ToString();
			array[6] = " Result: ";
			array[7] = ebeginAuthSessionResult.ToStringCached<EBeginAuthSessionResult>();
			Log.Out(string.Concat(array));
			if (ebeginAuthSessionResult == EBeginAuthSessionResult.k_EBeginAuthSessionResultOK)
			{
				return EBeginUserAuthenticationResult.Ok;
			}
			SteamGameServer.EndAuthSession(csteamID);
			return (EBeginUserAuthenticationResult)ebeginAuthSessionResult;
		}

		// Token: 0x0600D6CD RID: 54989 RVA: 0x004D606B File Offset: 0x004D426B
		public void RemoveUser(ClientInfo _cInfo)
		{
			if (this.owner.ServerListAnnouncer.GameServerInitialized)
			{
				SteamGameServer.EndAuthSession(new CSteamID(((UserIdentifierSteam)_cInfo.PlatformId).SteamId));
			}
		}

		// Token: 0x0600D6CE RID: 54990 RVA: 0x004D609C File Offset: 0x004D429C
		[PublicizedFrom(EAccessModifier.Private)]
		public void ValidateAuthTicketResponse(ValidateAuthTicketResponse_t _resp)
		{
			CSteamID steamID = _resp.m_SteamID;
			CSteamID ownerSteamID = _resp.m_OwnerSteamID;
			PlatformUserIdentifierAbs platformUserIdentifierAbs = new UserIdentifierSteam(steamID);
			ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForUserId(platformUserIdentifierAbs);
			if (clientInfo == null)
			{
				Log.Warning(string.Format("[Steamworks.NET] Authentication callback failed: User not found. ID: {0}", steamID));
				return;
			}
			string playerName = clientInfo.playerName;
			((UserIdentifierSteam)clientInfo.PlatformId).OwnerId = new UserIdentifierSteam(ownerSteamID);
			Log.Out(string.Format("[Steamworks.NET] Authentication callback. ID: {0}, owner: {1}, result: {2}", steamID, ownerSteamID, _resp.m_eAuthSessionResponse.ToStringCached<EAuthSessionResponse>()));
			if (_resp.m_eAuthSessionResponse == EAuthSessionResponse.k_EAuthSessionResponseOK)
			{
				this.authSuccessfulDelegate(clientInfo);
				return;
			}
			Log.Out(string.Format("[Steamworks.NET] Kick player for invalid login: {0} {1}", steamID, playerName));
			KickPlayerDelegate kickPlayerDelegate = this.kickPlayerDelegate;
			if (kickPlayerDelegate == null)
			{
				return;
			}
			kickPlayerDelegate(clientInfo, new GameUtils.KickPlayerData(GameUtils.EKickReason.PlatformAuthenticationFailed, (int)_resp.m_eAuthSessionResponse, default(DateTime), ""));
		}

		// Token: 0x0600D6CF RID: 54991 RVA: 0x004D6185 File Offset: 0x004D4385
		public void StartServer(AuthenticationSuccessfulCallbackDelegate _authSuccessfulDelegate, KickPlayerDelegate _kickPlayerDelegate)
		{
			this.authSuccessfulDelegate = _authSuccessfulDelegate;
			this.kickPlayerDelegate = _kickPlayerDelegate;
		}

		// Token: 0x0600D6D0 RID: 54992 RVA: 0x004D6195 File Offset: 0x004D4395
		public void StartServerSteamGroups(SteamGroupStatusResponse _groupStatusResponseDelegate)
		{
			this.groupStatusResponseDelegate = _groupStatusResponseDelegate;
		}

		// Token: 0x0600D6D1 RID: 54993 RVA: 0x004D619E File Offset: 0x004D439E
		public void StopServer()
		{
			this.authSuccessfulDelegate = null;
			this.kickPlayerDelegate = null;
		}

		// Token: 0x0600D6D2 RID: 54994 RVA: 0x004D61B0 File Offset: 0x004D43B0
		public bool RequestUserInGroupStatus(ClientInfo _cInfo, string _steamIdGroup)
		{
			UserIdentifierSteam userIdentifierSteam = (UserIdentifierSteam)_cInfo.PlatformId;
			CSteamID csteamID = new CSteamID(userIdentifierSteam.SteamId);
			ulong num;
			if (!ulong.TryParse(_steamIdGroup, out num))
			{
				Log.Warning("Invalid Steam group ID '" + _steamIdGroup + "' (value out of range) in serveradmin.xml, ignoring");
				return false;
			}
			CSteamID csteamID2 = new CSteamID(num);
			EAccountType eaccountType = csteamID2.GetEAccountType();
			if (eaccountType != EAccountType.k_EAccountTypeClan)
			{
				if (eaccountType == EAccountType.k_EAccountTypeIndividual)
				{
					Log.Warning("Invalid Steam group ID '" + _steamIdGroup + "' (SteamID is for a user, not for a group) in serveradmin.xml, ignoring");
				}
				else
				{
					Log.Warning(string.Concat(new string[]
					{
						"Invalid Steam group ID '",
						_steamIdGroup,
						"' (SteamID not valid for a Steam group but for ",
						eaccountType.ToStringCached<EAccountType>(),
						") in serveradmin.xml, ignoring"
					}));
				}
				return false;
			}
			bool flag = SteamGameServer.RequestUserGroupStatus(csteamID, csteamID2);
			if (!flag)
			{
				Log.Warning(string.Format("Failed requesting Steam group membership for user {0} and group {1}", csteamID, csteamID2));
			}
			return flag;
		}

		// Token: 0x0600D6D3 RID: 54995 RVA: 0x004D6288 File Offset: 0x004D4488
		[PublicizedFrom(EAccessModifier.Private)]
		public void GsClientGroupStatus(GSClientGroupStatus_t _response)
		{
			CSteamID steamIDUser = _response.m_SteamIDUser;
			ulong steamID = _response.m_SteamIDGroup.m_SteamID;
			PlatformUserIdentifierAbs platformUserIdentifierAbs = new UserIdentifierSteam(steamIDUser);
			ClientInfo clientInfo = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForUserId(platformUserIdentifierAbs);
			if (clientInfo != null)
			{
				this.groupStatusResponseDelegate(clientInfo, steamID, _response.m_bMember, _response.m_bOfficer);
			}
		}

		// Token: 0x0400A329 RID: 41769
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A32A RID: 41770
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<ValidateAuthTicketResponse_t> m_validateAuthTicketResponse;

		// Token: 0x0400A32B RID: 41771
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<GSClientGroupStatus_t> m_gsClientGroupStatus;

		// Token: 0x0400A32C RID: 41772
		[PublicizedFrom(EAccessModifier.Private)]
		public AuthenticationSuccessfulCallbackDelegate authSuccessfulDelegate;

		// Token: 0x0400A32D RID: 41773
		[PublicizedFrom(EAccessModifier.Private)]
		public KickPlayerDelegate kickPlayerDelegate;

		// Token: 0x0400A32E RID: 41774
		[PublicizedFrom(EAccessModifier.Private)]
		public SteamGroupStatusResponse groupStatusResponseDelegate;
	}
}
