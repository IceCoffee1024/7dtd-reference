using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Scripting;

namespace Platform.Steam
{
	// Token: 0x02001C5B RID: 7259
	[Preserve]
	public class SteamGroupsAuthorizer : AuthorizerAbs
	{
		// Token: 0x17001A62 RID: 6754
		// (get) Token: 0x0600D6D6 RID: 54998 RVA: 0x004D6331 File Offset: 0x004D4531
		public override int Order
		{
			get
			{
				return 470;
			}
		}

		// Token: 0x17001A63 RID: 6755
		// (get) Token: 0x0600D6D7 RID: 54999 RVA: 0x004D6338 File Offset: 0x004D4538
		public override string AuthorizerName
		{
			get
			{
				return "SteamGroups";
			}
		}

		// Token: 0x17001A64 RID: 6756
		// (get) Token: 0x0600D6D8 RID: 55000 RVA: 0x004D633F File Offset: 0x004D453F
		public override string StateLocalizationKey
		{
			get
			{
				return "authstate_steamgroups";
			}
		}

		// Token: 0x17001A65 RID: 6757
		// (get) Token: 0x0600D6D9 RID: 55001 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override EPlatformIdentifier PlatformRestriction
		{
			get
			{
				return EPlatformIdentifier.Steam;
			}
		}

		// Token: 0x17001A66 RID: 6758
		// (get) Token: 0x0600D6DA RID: 55002 RVA: 0x004D6346 File Offset: 0x004D4546
		public override bool AuthorizerActive
		{
			get
			{
				return GameManager.Instance.adminTools != null && PlatformManager.InstanceForPlatformIdentifier(EPlatformIdentifier.Steam) != null;
			}
		}

		// Token: 0x0600D6DB RID: 55003 RVA: 0x004D635F File Offset: 0x004D455F
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

		// Token: 0x0600D6DC RID: 55004 RVA: 0x004D6388 File Offset: 0x004D4588
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

		// Token: 0x0600D6DD RID: 55005 RVA: 0x004D64CC File Offset: 0x004D46CC
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
