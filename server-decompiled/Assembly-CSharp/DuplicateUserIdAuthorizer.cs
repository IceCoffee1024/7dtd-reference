using System;
using System.Collections.ObjectModel;
using UnityEngine.Scripting;

// Token: 0x0200077A RID: 1914
[Preserve]
public class DuplicateUserIdAuthorizer : AuthorizerAbs
{
	// Token: 0x170005BF RID: 1471
	// (get) Token: 0x06003902 RID: 14594 RVA: 0x00175819 File Offset: 0x00173A19
	public override int Order
	{
		get
		{
			return 60;
		}
	}

	// Token: 0x170005C0 RID: 1472
	// (get) Token: 0x06003903 RID: 14595 RVA: 0x0017581D File Offset: 0x00173A1D
	public override string AuthorizerName
	{
		get
		{
			return "DuplicateUserId";
		}
	}

	// Token: 0x170005C1 RID: 1473
	// (get) Token: 0x06003904 RID: 14596 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06003905 RID: 14597 RVA: 0x00175824 File Offset: 0x00173A24
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		ReadOnlyCollection<ClientInfo> list = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.List;
		for (int i = 0; i < list.Count; i++)
		{
			ClientInfo clientInfo = list[i];
			if (clientInfo != _clientInfo)
			{
				if (!_clientInfo.PlatformId.Equals(clientInfo.PlatformId))
				{
					PlatformUserIdentifierAbs crossplatformId = _clientInfo.CrossplatformId;
					if (crossplatformId == null || !crossplatformId.Equals(clientInfo.CrossplatformId))
					{
						goto IL_0089;
					}
				}
				GameUtils.KickPlayerForClientInfo(clientInfo, new GameUtils.KickPlayerData(GameUtils.EKickReason.DuplicatePlayerID, 0, default(DateTime), ""));
				return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncDeny, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(GameUtils.EKickReason.DuplicatePlayerID, 0, default(DateTime), "")));
			}
			IL_0089:;
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
	}
}
