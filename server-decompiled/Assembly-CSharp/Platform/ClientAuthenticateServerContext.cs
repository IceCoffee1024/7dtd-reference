using System;

namespace Platform
{
	// Token: 0x02001BBF RID: 7103
	public sealed class ClientAuthenticateServerContext
	{
		// Token: 0x0600D45F RID: 54367 RVA: 0x004CEC94 File Offset: 0x004CCE94
		public ClientAuthenticateServerContext(GameServerInfo _gameServerInfo, PlatformUserIdentifierAbs _platformUserId, PlatformUserIdentifierAbs _crossplatformUserId, ClientAuthenticateServerSuccessDelegate _success, ClientAuthenticateServerDisconnectDelegate _disconnect)
		{
			this.GameServerInfo = _gameServerInfo;
			this.PlatformUserId = _platformUserId;
			this.CrossplatformUserId = _crossplatformUserId;
			this.success = _success;
			this.disconnect = _disconnect;
		}

		// Token: 0x17001AC5 RID: 6853
		// (get) Token: 0x0600D460 RID: 54368 RVA: 0x004CECC1 File Offset: 0x004CCEC1
		public GameServerInfo GameServerInfo { get; }

		// Token: 0x17001AC6 RID: 6854
		// (get) Token: 0x0600D461 RID: 54369 RVA: 0x004CECC9 File Offset: 0x004CCEC9
		public PlatformUserIdentifierAbs PlatformUserId { get; }

		// Token: 0x17001AC7 RID: 6855
		// (get) Token: 0x0600D462 RID: 54370 RVA: 0x004CECD1 File Offset: 0x004CCED1
		public PlatformUserIdentifierAbs CrossplatformUserId { get; }

		// Token: 0x0600D463 RID: 54371 RVA: 0x004CECD9 File Offset: 0x004CCED9
		public void Success()
		{
			ClientAuthenticateServerSuccessDelegate clientAuthenticateServerSuccessDelegate = this.success;
			if (clientAuthenticateServerSuccessDelegate == null)
			{
				return;
			}
			clientAuthenticateServerSuccessDelegate();
		}

		// Token: 0x0600D464 RID: 54372 RVA: 0x004CECEC File Offset: 0x004CCEEC
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

		// Token: 0x0600D465 RID: 54373 RVA: 0x004CED23 File Offset: 0x004CCF23
		public void DisconnectNoCrossplay(EPlayGroup otherPlayGroup)
		{
			ClientAuthenticateServerDisconnectDelegate clientAuthenticateServerDisconnectDelegate = this.disconnect;
			if (clientAuthenticateServerDisconnectDelegate == null)
			{
				return;
			}
			clientAuthenticateServerDisconnectDelegate(string.Format(Localization.Get("auth_noCrossplayBetween", false, null), EPlayGroupExtensions.Current, otherPlayGroup));
		}

		// Token: 0x0400A237 RID: 41527
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ClientAuthenticateServerSuccessDelegate success;

		// Token: 0x0400A238 RID: 41528
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ClientAuthenticateServerDisconnectDelegate disconnect;
	}
}
