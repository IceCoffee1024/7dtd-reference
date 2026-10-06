using System;
using UnityEngine.Scripting;

// Token: 0x0200078A RID: 1930
[Preserve]
public class LegacyModAuthorizer : AuthorizerAbs
{
	// Token: 0x170005FA RID: 1530
	// (get) Token: 0x060039B0 RID: 14768 RVA: 0x00178598 File Offset: 0x00176798
	public override int Order
	{
		get
		{
			return 150;
		}
	}

	// Token: 0x170005FB RID: 1531
	// (get) Token: 0x060039B1 RID: 14769 RVA: 0x0017859F File Offset: 0x0017679F
	public override string AuthorizerName
	{
		get
		{
			return "LegacyModAuthorizations";
		}
	}

	// Token: 0x170005FC RID: 1532
	// (get) Token: 0x060039B2 RID: 14770 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060039B3 RID: 14771 RVA: 0x001785A8 File Offset: 0x001767A8
	public override ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?> Authorize(ClientInfo _clientInfo)
	{
		ModEvents.SPlayerLoginData splayerLoginData = new ModEvents.SPlayerLoginData(_clientInfo, _clientInfo.compatibilityVersion);
		ValueTuple<ModEvents.EModEventResult, Mod> valueTuple = ModEvents.PlayerLogin.Invoke(ref splayerLoginData);
		ModEvents.EModEventResult item = valueTuple.Item1;
		Mod item2 = valueTuple.Item2;
		if (item == ModEvents.EModEventResult.StopHandlersAndVanilla)
		{
			Log.Out("Denying login from mod: " + item2.Name);
			EAuthorizerSyncResult eauthorizerSyncResult = EAuthorizerSyncResult.SyncDeny;
			GameUtils.EKickReason ekickReason = GameUtils.EKickReason.ModDecision;
			int num = 0;
			string customMessage = splayerLoginData.CustomMessage;
			return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(eauthorizerSyncResult, new GameUtils.KickPlayerData?(new GameUtils.KickPlayerData(ekickReason, num, default(DateTime), customMessage)));
		}
		return new ValueTuple<EAuthorizerSyncResult, GameUtils.KickPlayerData?>(EAuthorizerSyncResult.SyncAllow, null);
	}
}
