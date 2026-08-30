using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Scripting;

namespace Platform.Steam
{
	// Token: 0x02001C82 RID: 7298
	[Preserve]
	public class SteamGroupsAuthorizer : AuthorizerAbs
	{
		// Token: 0x17001AD4 RID: 6868
		// (get) Token: 0x0600D866 RID: 55398 RVA: 0x004DC755 File Offset: 0x004DA955
		public override int Order
		{
			get
			{
				return 470;
			}
		}

		// Token: 0x17001AD5 RID: 6869
		// (get) Token: 0x0600D867 RID: 55399 RVA: 0x004DC75C File Offset: 0x004DA95C
		public override string AuthorizerName
		{
			get
			{
				return "SteamGroups";
			}
		}

		// Token: 0x17001AD6 RID: 6870
		// (get) Token: 0x0600D868 RID: 55400 RVA: 0x004DC763 File Offset: 0x004DA963
		public override string StateLocalizationKey
		{
			get
			{
				return "authstate_steamgroups";
			}
		}

		// Token: 0x17001AD7 RID: 6871
		// (get) Token: 0x0600D869 RID: 55401 RVA: 0x00046EDA File Offset: 0x000450DA
		public override EPlatformIdentifier PlatformRestriction
		{
			get
			{
				return EPlatformIdentifier.Steam;
			}
		}

		// Token: 0x17001AD8 RID: 6872
		// (get) Token: 0x0600D86A RID: 55402 RVA: 0x004DC76A File Offset: 0x004DA96A
		public override bool AuthorizerActive
		{
			get
			{
				return GameManager.Instance.adminTools != null && PlatformManager.InstanceForPlatformIdentifier(EPlatformIdentifier.Steam) != null;
			}
		}

		// Token: 0x0600D86B RID: 55403 RVA: 0x004DC783 File Offset: 0x004DA983
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

		// Token: 0x0600D86C RID: 55404 RVA: 0x004DC7AC File Offset: 0x004DA9AC
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

		// Token: 0x0600D86D RID: 55405 RVA: 0x004DC8F0 File Offset: 0x004DAAF0
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
