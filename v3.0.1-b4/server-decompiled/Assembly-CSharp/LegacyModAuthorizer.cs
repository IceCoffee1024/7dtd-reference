using System;
using UnityEngine.Scripting;

// Token: 0x0200077E RID: 1918
[Preserve]
public class LegacyModAuthorizer : AuthorizerAbs
{
	// Token: 0x170005CB RID: 1483
	// (get) Token: 0x06003916 RID: 14614 RVA: 0x00175B60 File Offset: 0x00173D60
	public override int Order
	{
		get
		{
			return 150;
		}
	}

	// Token: 0x170005CC RID: 1484
	// (get) Token: 0x06003917 RID: 14615 RVA: 0x00175B67 File Offset: 0x00173D67
	public override string AuthorizerName
	{
		get
		{
			return "LegacyModAuthorizations";
		}
	}

	// Token: 0x170005CD RID: 1485
	// (get) Token: 0x06003918 RID: 14616 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public override string StateLocalizationKey
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06003919 RID: 14617 RVA: 0x00175B70 File Offset: 0x00173D70
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
