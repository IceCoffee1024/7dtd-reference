using System;
using System.Collections.ObjectModel;
using UnityEngine.Scripting;

// Token: 0x02000786 RID: 1926
[Preserve]
public class DuplicateUserIdAuthorizer : AuthorizerAbs
{
	// Token: 0x170005EE RID: 1518
	// (get) Token: 0x0600399C RID: 14748 RVA: 0x00178251 File Offset: 0x00176451
	public override int Order
	{
		get
		{
			return 60;
		}
	}

	// Token: 0x170005EF RID: 1519
	// (get) Token: 0x0600399D RID: 14749 RVA: 0x00178255 File Offset: 0x00176455
	public override string AuthorizerName
	{
		get
		{
			return "DuplicateUserId";
		}
	}

	// Token: 0x170005F0 RID: 1520
	// (get) Token: 0x0600399E RID: 14750 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x0600399F RID: 14751 RVA: 0x0017825C File Offset: 0x0017645C
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
