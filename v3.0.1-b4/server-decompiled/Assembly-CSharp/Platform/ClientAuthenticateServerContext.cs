using System;

namespace Platform
{
	// Token: 0x02001B35 RID: 6965
	public sealed class ClientAuthenticateServerContext
	{
		// Token: 0x0600D04B RID: 53323 RVA: 0x004C0290 File Offset: 0x004BE490
		public ClientAuthenticateServerContext(GameServerInfo _gameServerInfo, PlatformUserIdentifierAbs _platformUserId, PlatformUserIdentifierAbs _crossplatformUserId, ClientAuthenticateServerSuccessDelegate _success, ClientAuthenticateServerDisconnectDelegate _disconnect)
		{
			this.GameServerInfo = _gameServerInfo;
			this.PlatformUserId = _platformUserId;
			this.CrossplatformUserId = _crossplatformUserId;
			this.success = _success;
			this.disconnect = _disconnect;
		}

		// Token: 0x17001965 RID: 6501
		// (get) Token: 0x0600D04C RID: 53324 RVA: 0x004C02BD File Offset: 0x004BE4BD
		public GameServerInfo GameServerInfo { get; }

		// Token: 0x17001966 RID: 6502
		// (get) Token: 0x0600D04D RID: 53325 RVA: 0x004C02C5 File Offset: 0x004BE4C5
		public PlatformUserIdentifierAbs PlatformUserId { get; }

		// Token: 0x17001967 RID: 6503
		// (get) Token: 0x0600D04E RID: 53326 RVA: 0x004C02CD File Offset: 0x004BE4CD
		public PlatformUserIdentifierAbs CrossplatformUserId { get; }

		// Token: 0x0600D04F RID: 53327 RVA: 0x004C02D5 File Offset: 0x004BE4D5
		public void Success()
		{
			ClientAuthenticateServerSuccessDelegate clientAuthenticateServerSuccessDelegate = this.success;
			if (clientAuthenticateServerSuccessDelegate == null)
			{
				return;
			}
			clientAuthenticateServerSuccessDelegate();
		}

		// Token: 0x0600D050 RID: 53328 RVA: 0x004C02E8 File Offset: 0x004BE4E8
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

		// Token: 0x0600D051 RID: 53329 RVA: 0x004C031F File Offset: 0x004BE51F
		public void DisconnectNoCrossplay(EPlayGroup otherPlayGroup)
		{
			ClientAuthenticateServerDisconnectDelegate clientAuthenticateServerDisconnectDelegate = this.disconnect;
			if (clientAuthenticateServerDisconnectDelegate == null)
			{
				return;
			}
			clientAuthenticateServerDisconnectDelegate(string.Format(Localization.Get("auth_noCrossplayBetween", false, null), EPlayGroupExtensions.Current, otherPlayGroup));
		}

		// Token: 0x04009F48 RID: 40776
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ClientAuthenticateServerSuccessDelegate success;

		// Token: 0x04009F49 RID: 40777
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly ClientAuthenticateServerDisconnectDelegate disconnect;
	}
}
