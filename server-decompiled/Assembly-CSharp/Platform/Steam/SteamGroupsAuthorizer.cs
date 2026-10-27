using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Scripting;

namespace Platform.Steam
{
	// Token: 0x02001CE5 RID: 7397
	[Preserve]
	public class SteamGroupsAuthorizer : AuthorizerAbs
	{
		// Token: 0x17001BC2 RID: 7106
		// (get) Token: 0x0600DAF3 RID: 56051 RVA: 0x004E4DE1 File Offset: 0x004E2FE1
		public override int Order
		{
			get
			{
				return 470;
			}
		}

		// Token: 0x17001BC3 RID: 7107
		// (get) Token: 0x0600DAF4 RID: 56052 RVA: 0x004E4DE8 File Offset: 0x004E2FE8
		public override string AuthorizerName
		{
			get
			{
				return "SteamGroups";
			}
		}

		// Token: 0x17001BC4 RID: 7108
		// (get) Token: 0x0600DAF5 RID: 56053 RVA: 0x004E4DEF File Offset: 0x004E2FEF
		public override string StateLocalizationKey
		{
			get
			{
				return "authstate_steamgroups";
			}
		}

		// Token: 0x17001BC5 RID: 7109
		// (get) Token: 0x0600DAF6 RID: 56054 RVA: 0x00047076 File Offset: 0x00045276
		public override EPlatformIdentifier PlatformRestriction
		{
			get
			{
				return EPlatformIdentifier.Steam;
			}
		}

		// Token: 0x17001BC6 RID: 7110
		// (get) Token: 0x0600DAF7 RID: 56055 RVA: 0x004E4DF6 File Offset: 0x004E2FF6
		public override bool AuthorizerActive
		{
			get
			{
				return GameManager.Instance.adminTools != null && PlatformManager.InstanceForPlatformIdentifier(EPlatformIdentifier.Steam) != null;
			}
		}

		// Token: 0x0600DAF8 RID: 56056 RVA: 0x004E4E0F File Offset: 0x004E300F
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

		// Token: 0x0600DAF9 RID: 56057 RVA: 0x004E4E38 File Offset: 0x004E3038
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

		// Token: 0x0600DAFA RID: 56058 RVA: 0x004E4F7C File Offset: 0x004E317C
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
