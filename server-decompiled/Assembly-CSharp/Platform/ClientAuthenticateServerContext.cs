using System;

namespace Platform
{
	// Token: 0x02001B5C RID: 7004
	public sealed class ClientAuthenticateServerContext
	{
		// Token: 0x0600D1D4 RID: 53716 RVA: 0x004C6684 File Offset: 0x004C4884
		public ClientAuthenticateServerContext(GameServerInfo _gameServerInfo, PlatformUserIdentifierAbs _platformUserId, PlatformUserIdentifierAbs _crossplatformUserId, ClientAuthenticateServerSuccessDelegate _success, ClientAuthenticateServerDisconnectDelegate _disconnect)
		{
			this.GameServerInfo = _gameServerInfo;
			this.PlatformUserId = _platformUserId;
			this.CrossplatformUserId = _crossplatformUserId;
			this.success = _success;
			this.disconnect = _disconnect;
		}

		// Token: 0x170019D7 RID: 6615
		// (get) Token: 0x0600D1D5 RID: 53717 RVA: 0x004C66B1 File Offset: 0x004C48B1
		public GameServerInfo GameServerInfo { get; }

		// Token: 0x170019D8 RID: 6616
		// (get) Token: 0x0600D1D6 RID: 53718 RVA: 0x004C66B9 File Offset: 0x004C48B9
		public PlatformUserIdentifierAbs PlatformUserId { get; }

		// Token: 0x170019D9 RID: 6617
		// (get) Token: 0x0600D1D7 RID: 53719 RVA: 0x004C66C1 File Offset: 0x004C48C1
		public PlatformUserIdentifierAbs CrossplatformUserId { get; }

		// Token: 0x0600D1D8 RID: 53720 RVA: 0x004C66C9 File Offset: 0x004C48C9
		public void Success()
		{
			ClientAuthenticateServerSuccessDelegate clientAuthenticateServerSuccessDelegate = this.success;
			if (clientAuthenticateServerSuccessDelegate == null)
			{
				return;
			}
			clientAuthenticateServerSuccessDelegate();
		}

		// Token: 0x0600D1D9 RID: 53721 RVA: 0x004C66DC File Offset: 0x004C48DC
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

		// Token: 0x0600D1DA RID: 53722 RVA: 0x004C6713 File Offset: 0x004C4913
		public void DisconnectNoCrossplay(EPlayGroup otherPlayGroup)
		{
			ClientAuthenticateServerDisconnectDelegate clientAuthenticateServerDisconnectDelegate = this.disconnect;
			if (clientAuthenticateServerDisconnectDelegate == null)
			{
				return;
			}
			clientAuthenticateServerDisconnectDelegate(string.Format(Localization.Get("auth_noCrossplayBetween", false, null), EPlayGroupExtensions.Current, otherPlayGroup));
		}

		// Token: 0x0400A079 RID: 41081
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ClientAuthenticateServerSuccessDelegate success;

		// Token: 0x0400A07A RID: 41082
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ClientAuthenticateServerDisconnectDelegate disconnect;
	}
}
