using System;

namespace Platform
{
	// Token: 0x02001B8E RID: 7054
	public sealed class ClientAuthenticateServerContext
	{
		// Token: 0x0600D34B RID: 54091 RVA: 0x004CB16C File Offset: 0x004C936C
		public ClientAuthenticateServerContext(GameServerInfo _gameServerInfo, PlatformUserIdentifierAbs _platformUserId, PlatformUserIdentifierAbs _crossplatformUserId, ClientAuthenticateServerSuccessDelegate _success, ClientAuthenticateServerDisconnectDelegate _disconnect)
		{
			this.GameServerInfo = _gameServerInfo;
			this.PlatformUserId = _platformUserId;
			this.CrossplatformUserId = _crossplatformUserId;
			this.success = _success;
			this.disconnect = _disconnect;
		}

		// Token: 0x17001A7D RID: 6781
		// (get) Token: 0x0600D34C RID: 54092 RVA: 0x004CB199 File Offset: 0x004C9399
		public GameServerInfo GameServerInfo { get; }

		// Token: 0x17001A7E RID: 6782
		// (get) Token: 0x0600D34D RID: 54093 RVA: 0x004CB1A1 File Offset: 0x004C93A1
		public PlatformUserIdentifierAbs PlatformUserId { get; }

		// Token: 0x17001A7F RID: 6783
		// (get) Token: 0x0600D34E RID: 54094 RVA: 0x004CB1A9 File Offset: 0x004C93A9
		public PlatformUserIdentifierAbs CrossplatformUserId { get; }

		// Token: 0x0600D34F RID: 54095 RVA: 0x004CB1B1 File Offset: 0x004C93B1
		public void Success()
		{
			ClientAuthenticateServerSuccessDelegate clientAuthenticateServerSuccessDelegate = this.success;
			if (clientAuthenticateServerSuccessDelegate == null)
			{
				return;
			}
			clientAuthenticateServerSuccessDelegate();
		}

		// Token: 0x0600D350 RID: 54096 RVA: 0x004CB1C4 File Offset: 0x004C93C4
		public void DisconnectNoCrossplay()
		{
			string text = PermissionsManager.GetPermissionDenyReason(EUserPerms.Crossplay, PermissionsManager.PermissionSources.All) ?? Localization.Get("auth_noCrossplay", false, null);
			ClientAuthenticateServerDisconnectDelegate clientAuthenticateServerDisconnectDelegate = this.disconnect;
			if (clientAuthenticateServerDisconnectDelegate == null)
			{
				return;
			}
			clientAuthenticateServerDisconnectDelegate(text);
		}

		// Token: 0x0600D351 RID: 54097 RVA: 0x004CB1FB File Offset: 0x004C93FB
		public void DisconnectNoCrossplay(EPlayGroup otherPlayGroup)
		{
			ClientAuthenticateServerDisconnectDelegate clientAuthenticateServerDisconnectDelegate = this.disconnect;
			if (clientAuthenticateServerDisconnectDelegate == null)
			{
				return;
			}
			clientAuthenticateServerDisconnectDelegate(string.Format(Localization.Get("auth_noCrossplayBetween", false, null), EPlayGroupExtensions.Current, otherPlayGroup));
		}

		// Token: 0x0400A14E RID: 41294
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ClientAuthenticateServerSuccessDelegate success;

		// Token: 0x0400A14F RID: 41295
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ClientAuthenticateServerDisconnectDelegate disconnect;
	}
}
