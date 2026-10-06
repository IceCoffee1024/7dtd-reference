using System;
using System.Collections.ObjectModel;
using UnityEngine.Scripting;

// Token: 0x02000788 RID: 1928
[Preserve]
public class PlayerSlotsAuthorizer : AuthorizerAbs
{
	// Token: 0x170005F4 RID: 1524
	// (get) Token: 0x060039A6 RID: 14758 RVA: 0x0017837C File Offset: 0x0017657C
	public override int Order
	{
		get
		{
			return 80;
		}
	}

	// Token: 0x170005F5 RID: 1525
	// (get) Token: 0x060039A7 RID: 14759 RVA: 0x00178380 File Offset: 0x00176580
	public override string AuthorizerName
	{
		get
		{
			return "PlayerSlots";
		}
	}

	// Token: 0x170005F6 RID: 1526
	// (get) Token: 0x060039A8 RID: 14760 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060039A9 RID: 14761 RVA: 0x00178388 File Offset: 0x00176588
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		AdminTools adminTools = GameManager.Instance.adminTools;
		int num = 0;
		int num2 = 0;
		int @int = GamePrefs.GetInt(EnumGamePrefs.ServerMaxPlayerCount);
		int int2 = GamePrefs.GetInt(EnumGamePrefs.ServerReservedSlots);
		int int3 = GamePrefs.GetInt(EnumGamePrefs.ServerReservedSlotsPermission);
		ReadOnlyCollection<ClientInfo> list = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.List;
		for (int i = 0; i < list.Count; i++)
		{
			ClientInfo clientInfo = list[i];
			if (clientInfo != _clientInfo)
			{
				if (((adminTools != null) ? adminTools.Users.GetUserPermissionLevel(clientInfo) : 1000) <= int3)
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
		}
		if (!GameManager.IsDedicatedServer)
		{
			num++;
		}
		bool flag = ((adminTools != null) ? adminTools.Users.GetUserPermissionLevel(_clientInfo) : 1000) <= int3;
		bool flag2;
		if (flag)
		{
			flag2 = num2 + num < @int;
		}
		else
		{
			flag2 = num2 + num < @int && num2 < @int - int2;
		}
		if (!flag2)
		{
			int int4 = GamePrefs.GetInt(EnumGamePrefs.ServerAdminSlots);
			if (int4 > 0 && num2 + num < @int + int4)
			{
				flag2 = ((adminTools != null) ? adminTools.Users.GetUserPermissionLevel(_clientInfo) : 1000) <= GamePrefs.GetInt(EnumGamePrefs.ServerAdminSlotsPermission);
			}
		}
		if (flag2)
		{
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
		}
		string text;
		if (!flag && num2 + num < @int)
		{
			EAuthorizerSyncResult eauthorizerSyncResult = EAuthorizerSyncResult.SyncDeny;
			GameUtils.EKickReason ekickReason = GameUtils.EKickReason.PlayerLimitExceededNonVIP;
			int num3 = 0;
			text = (@int - int2).ToString();
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(eauthorizerSyncResult, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(ekickReason, num3, default(DateTime), text)));
		}
		EAuthorizerSyncResult eauthorizerSyncResult2 = EAuthorizerSyncResult.SyncDeny;
		GameUtils.EKickReason ekickReason2 = GameUtils.EKickReason.PlayerLimitExceeded;
		int num4 = 0;
		text = @int.ToString();
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(eauthorizerSyncResult2, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(ekickReason2, num4, default(DateTime), text)));
	}
}
