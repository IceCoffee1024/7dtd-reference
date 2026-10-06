using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Scripting;

namespace Platform.Steam
{
	// Token: 0x02001CB4 RID: 7348
	[Preserve]
	public class SteamGroupsAuthorizer : AuthorizerAbs
	{
		// Token: 0x17001B7A RID: 7034
		// (get) Token: 0x0600D9DF RID: 55775 RVA: 0x004E1291 File Offset: 0x004DF491
		public override int Order
		{
			get
			{
				return 470;
			}
		}

		// Token: 0x17001B7B RID: 7035
		// (get) Token: 0x0600D9E0 RID: 55776 RVA: 0x004E1298 File Offset: 0x004DF498
		public override string AuthorizerName
		{
			get
			{
				return "SteamGroups";
			}
		}

		// Token: 0x17001B7C RID: 7036
		// (get) Token: 0x0600D9E1 RID: 55777 RVA: 0x004E129F File Offset: 0x004DF49F
		public override string StateLocalizationKey
		{
			get
			{
				return "authstate_steamgroups";
			}
		}

		// Token: 0x17001B7D RID: 7037
		// (get) Token: 0x0600D9E2 RID: 55778 RVA: 0x000470B6 File Offset: 0x000452B6
		public override EPlatformIdentifier PlatformRestriction
		{
			get
			{
				return EPlatformIdentifier.Steam;
			}
		}

		// Token: 0x17001B7E RID: 7038
		// (get) Token: 0x0600D9E3 RID: 55779 RVA: 0x004E12A6 File Offset: 0x004DF4A6
		public override bool AuthorizerActive
		{
			get
			{
				return GameManager.Instance.adminTools != null && PlatformManager.InstanceForPlatformIdentifier(EPlatformIdentifier.Steam) != null;
			}
		}

		// Token: 0x0600D9E4 RID: 55780 RVA: 0x004E12BF File Offset: 0x004DF4BF
		public override void ServerStart()
		{
			base.ServerStart();
			IAuthenticationServer authenticationServer = PlatformManager.NativePlatform.AuthenticationServer;
			if (authenticationServer == null)
			{
				return;
			}
			authenticationServer.StartServerSteamGroups(new SteamGroupStatusResponse(this.groupStatusCallback));
		}

		// Token: 0x0600D9E5 RID: 55781 RVA: 0x004E12E8 File Offset: 0x004DF4E8
		public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
		{
			Dictionary<string, AdminWhitelist.WhitelistGroup> groups = GameManager.Instance.adminTools.Whitelist.GetGroups();
			Dictionary<string, AdminUsers.GroupPermission> groups2 = GameManager.Instance.adminTools.Users.GetGroups();
			if (groups.Count == 0 && groups2.Count == 0)
			{
				return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
			}
			EPlatformIdentifier platformIdentifier = _clientInfo.PlatformId.PlatformIdentifier;
			IPlatform platform = PlatformManager.InstanceForPlatformIdentifier(platformIdentifier);
			if (platform == null)
			{
				EAuthorizerSyncResult eauthorizerSyncResult = EAuthorizerSyncResult.SyncDeny;
				GameUtils.EKickReason ekickReason = GameUtils.EKickReason.UnsupportedPlatform;
				int num = 0;
				string text = platformIdentifier.ToStringCached<EPlatformIdentifier>();
				return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(eauthorizerSyncResult, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(ekickReason, num, default(DateTime), text)));
			}
			HashSet<string> hashSet = new HashSet<string>(StringComparer.Ordinal);
			groups.CopyKeysTo<string, AdminWhitelist.WhitelistGroup>(hashSet);
			groups2.CopyKeysTo<string, AdminUsers.GroupPermission>(hashSet);
			_clientInfo.groupMembershipsWaiting = hashSet.Count;
			foreach (string text2 in hashSet)
			{
				if (!platform.AuthenticationServer.RequestUserInGroupStatus(_clientInfo, text2))
				{
					Interlocked.Decrement(ref _clientInfo.groupMembershipsWaiting);
				}
			}
			if (_clientInfo.groupMembershipsWaiting == 0)
			{
				return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
			}
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.WaitAsync, null);
		}

		// Token: 0x0600D9E6 RID: 55782 RVA: 0x004E142C File Offset: 0x004DF62C
		[PublicizedFrom(EAccessModifier.Private)]
		public void groupStatusCallback(ClientInfo _clientInfo, ulong _groupId, bool _member, bool _officer)
		{
			bool flag = Interlocked.Decrement(ref _clientInfo.groupMembershipsWaiting) != 0;
			if (_member)
			{
				_clientInfo.groupMemberships[_groupId.ToString()] = (_officer ? 2 : 1);
			}
			if (!flag)
			{
				this.authResponsesHandler.AuthorizationAccepted(this, _clientInfo);
			}
		}
	}
}
