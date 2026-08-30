using System;
using Steamworks;

namespace Platform.Steam
{
	// Token: 0x02001C81 RID: 7297
	public class AuthenticationServer : IAuthenticationServer
	{
		// Token: 0x0600D85B RID: 55387 RVA: 0x004DC3AF File Offset: 0x004DA5AF
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

		// Token: 0x0600D85C RID: 55388 RVA: 0x004DC3D4 File Offset: 0x004DA5D4
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

		// Token: 0x0600D85D RID: 55389 RVA: 0x004DC48F File Offset: 0x004DA68F
		public void RemoveUser(ClientInfo _cInfo)
		{
			if (this.owner.ServerListAnnouncer.GameServerInitialized)
			{
				SteamGameServer.EndAuthSession(new CSteamID(((UserIdentifierSteam)_cInfo.PlatformId).SteamId));
			}
		}

		// Token: 0x0600D85E RID: 55390 RVA: 0x004DC4C0 File Offset: 0x004DA6C0
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

		// Token: 0x0600D85F RID: 55391 RVA: 0x004DC5A9 File Offset: 0x004DA7A9
		public void StartServer(AuthenticationSuccessfulCallbackDelegate _authSuccessfulDelegate, KickPlayerDelegate _kickPlayerDelegate)
		{
			this.authSuccessfulDelegate = _authSuccessfulDelegate;
			this.kickPlayerDelegate = _kickPlayerDelegate;
		}

		// Token: 0x0600D860 RID: 55392 RVA: 0x004DC5B9 File Offset: 0x004DA7B9
		public void StartServerSteamGroups(SteamGroupStatusResponse _groupStatusResponseDelegate)
		{
			this.groupStatusResponseDelegate = _groupStatusResponseDelegate;
		}

		// Token: 0x0600D861 RID: 55393 RVA: 0x004DC5C2 File Offset: 0x004DA7C2
		public void StopServer()
		{
			this.authSuccessfulDelegate = null;
			this.kickPlayerDelegate = null;
		}

		// Token: 0x0600D862 RID: 55394 RVA: 0x004DC5D4 File Offset: 0x004DA7D4
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

		// Token: 0x0600D863 RID: 55395 RVA: 0x004DC6AC File Offset: 0x004DA8AC
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

		// Token: 0x0400A45A RID: 42074
		[PublicizedFrom(EAccessModifier.Private)]
		public IPlatform owner;

		// Token: 0x0400A45B RID: 42075
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<ValidateAuthTicketResponse_t> m_validateAuthTicketResponse;

		// Token: 0x0400A45C RID: 42076
		[PublicizedFrom(EAccessModifier.Private)]
		public Callback<GSClientGroupStatus_t> m_gsClientGroupStatus;

		// Token: 0x0400A45D RID: 42077
		[PublicizedFrom(EAccessModifier.Private)]
		public AuthenticationSuccessfulCallbackDelegate authSuccessfulDelegate;

		// Token: 0x0400A45E RID: 42078
		[PublicizedFrom(EAccessModifier.Private)]
		public KickPlayerDelegate kickPlayerDelegate;

		// Token: 0x0400A45F RID: 42079
		[PublicizedFrom(EAccessModifier.Private)]
		public SteamGroupStatusResponse groupStatusResponseDelegate;
	}
}
