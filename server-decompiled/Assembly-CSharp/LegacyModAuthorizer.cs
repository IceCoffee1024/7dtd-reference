using System;
using UnityEngine.Scripting;

// Token: 0x02000792 RID: 1938
[Preserve]
public class LegacyModAuthorizer : AuthorizerAbs
{
	// Token: 0x170005FE RID: 1534
	// (get) Token: 0x060039C7 RID: 14791 RVA: 0x00179530 File Offset: 0x00177730
	public override int Order
	{
		get
		{
			return 150;
		}
	}

	// Token: 0x170005FF RID: 1535
	// (get) Token: 0x060039C8 RID: 14792 RVA: 0x00179537 File Offset: 0x00177737
	public override string AuthorizerName
	{
		get
		{
			return "LegacyModAuthorizations";
		}
	}

	// Token: 0x17000600 RID: 1536
	// (get) Token: 0x060039C9 RID: 14793 RVA: 0x000200DA File Offset: 0x0001E2DA
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x060039CA RID: 14794 RVA: 0x00179540 File Offset: 0x00177740
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
