using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000789 RID: 1929
[Preserve]
public class TooManyPlayerSlotsAuthorizer : AuthorizerAbs
{
	// Token: 0x170005F7 RID: 1527
	// (get) Token: 0x060039AB RID: 14763 RVA: 0x00178524 File Offset: 0x00176724
	public override int Order
	{
		get
		{
			return 81;
		}
	}

	// Token: 0x170005F8 RID: 1528
	// (get) Token: 0x060039AC RID: 14764 RVA: 0x00178380 File Offset: 0x00176580
	public override string AuthorizerName
	{
		get
		{
			return "PlayerSlots";
		}
	}

	// Token: 0x170005F9 RID: 1529
	// (get) Token: 0x060039AD RID: 14765 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060039AE RID: 14766 RVA: 0x00178528 File Offset: 0x00176728
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
