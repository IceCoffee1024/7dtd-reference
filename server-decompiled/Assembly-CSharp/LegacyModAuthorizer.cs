using System;
using UnityEngine.Scripting;

// Token: 0x02000788 RID: 1928
[Preserve]
public class LegacyModAuthorizer : AuthorizerAbs
{
	// Token: 0x170005D5 RID: 1493
	// (get) Token: 0x0600396E RID: 14702 RVA: 0x00177DE0 File Offset: 0x00175FE0
	public override int Order
	{
		get
		{
			return 150;
		}
	}

	// Token: 0x170005D6 RID: 1494
	// (get) Token: 0x0600396F RID: 14703 RVA: 0x00177DE7 File Offset: 0x00175FE7
	public override string AuthorizerName
	{
		get
		{
			return "LegacyModAuthorizations";
		}
	}

	// Token: 0x170005D7 RID: 1495
	// (get) Token: 0x06003970 RID: 14704 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06003971 RID: 14705 RVA: 0x00177DF0 File Offset: 0x00175FF0
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
