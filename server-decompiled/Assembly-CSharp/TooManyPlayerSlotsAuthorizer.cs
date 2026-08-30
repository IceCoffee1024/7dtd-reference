using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200077D RID: 1917
[Preserve]
public class TooManyPlayerSlotsAuthorizer : AuthorizerAbs
{
	// Token: 0x170005C8 RID: 1480
	// (get) Token: 0x06003911 RID: 14609 RVA: 0x00175AEC File Offset: 0x00173CEC
	public override int Order
	{
		get
		{
			return 81;
		}
	}

	// Token: 0x170005C9 RID: 1481
	// (get) Token: 0x06003912 RID: 14610 RVA: 0x00175948 File Offset: 0x00173B48
	public override string AuthorizerName
	{
		get
		{
			return "PlayerSlots";
		}
	}

	// Token: 0x170005CA RID: 1482
	// (get) Token: 0x06003913 RID: 14611 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06003914 RID: 14612 RVA: 0x00175AF0 File Offset: 0x00173CF0
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
