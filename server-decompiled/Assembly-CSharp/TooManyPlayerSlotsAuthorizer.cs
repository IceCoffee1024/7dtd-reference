using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000787 RID: 1927
[Preserve]
public class TooManyPlayerSlotsAuthorizer : AuthorizerAbs
{
	// Token: 0x170005D2 RID: 1490
	// (get) Token: 0x06003969 RID: 14697 RVA: 0x00177D6C File Offset: 0x00175F6C
	public override int Order
	{
		get
		{
			return 81;
		}
	}

	// Token: 0x170005D3 RID: 1491
	// (get) Token: 0x0600396A RID: 14698 RVA: 0x00177BC8 File Offset: 0x00175DC8
	public override string AuthorizerName
	{
		get
		{
			return "PlayerSlots";
		}
	}

	// Token: 0x170005D4 RID: 1492
	// (get) Token: 0x0600396B RID: 14699 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x0600396C RID: 14700 RVA: 0x00177D70 File Offset: 0x00175F70
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		int @int = GamePrefs.GetInt(EnumGamePrefs.ServerMaxPlayerCount);
		bool flag = true;
		EPlayGroup eplayGroup = _clientInfo.device.ToPlayGroup();
		if (eplayGroup == EPlayGroup.XBS || eplayGroup == EPlayGroup.PS5)
		{
			flag = @int <= 8;
		}
		if (!flag)
		{
			EAuthorizerSyncResult eauthorizerSyncResult = EAuthorizerSyncResult.SyncDeny;
			GameUtils.EKickReason ekickReason = GameUtils.EKickReason.PlatformPlayerLimitExceeded;
			int num = 0;
			string text = 8.ToString();
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(eauthorizerSyncResult, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(ekickReason, num, default(DateTime), text)));
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
	}
}
