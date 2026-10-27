using System;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000791 RID: 1937
[Preserve]
public class TooManyPlayerSlotsAuthorizer : AuthorizerAbs
{
	// Token: 0x170005FB RID: 1531
	// (get) Token: 0x060039C2 RID: 14786 RVA: 0x001794BC File Offset: 0x001776BC
	public override int Order
	{
		get
		{
			return 81;
		}
	}

	// Token: 0x170005FC RID: 1532
	// (get) Token: 0x060039C3 RID: 14787 RVA: 0x00179318 File Offset: 0x00177518
	public override string AuthorizerName
	{
		get
		{
			return "PlayerSlots";
		}
	}

	// Token: 0x170005FD RID: 1533
	// (get) Token: 0x060039C4 RID: 14788 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060039C5 RID: 14789 RVA: 0x001794C0 File Offset: 0x001776C0
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
