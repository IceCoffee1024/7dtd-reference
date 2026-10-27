using System;
using UnityEngine.Scripting;

// Token: 0x020002EC RID: 748
[Preserve]
public class DialogActionTrader : BaseDialogAction
{
	// Token: 0x17000256 RID: 598
	// (get) Token: 0x06001561 RID: 5473 RVA: 0x000809D4 File Offset: 0x0007EBD4
	public override BaseDialogAction.ActionTypes ActionType
	{
		get
		{
			return BaseDialogAction.ActionTypes.Trader;
		}
	}

	// Token: 0x06001562 RID: 5474 RVA: 0x000809D8 File Offset: 0x0007EBD8
	public override void PerformAction(EntityPlayer player)
	{
		LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(player as EntityPlayerLocal);
		EntityNPC respondent = uiforPlayer.xui.Dialog.Respondent;
		if (respondent != null)
		{
			EntityTrader entityTrader = respondent as EntityTrader;
			if (entityTrader != null)
			{
				if (base.ID.EqualsCaseInsensitive("restock"))
				{
					entityTrader.TraderData.lastInventoryUpdate = 0UL;
				}
				else if (base.ID.EqualsCaseInsensitive("trade"))
				{
					entityTrader.SetNextTraderWindow(EntityTrader.TraderWindowState.Trade);
					uiforPlayer.windowManager.CloseAllOpenModalWindows(null, false);
				}
				else if (base.ID.EqualsCaseInsensitive("reset_quests"))
				{
					entityTrader.ClearActiveQuests(player.entityId);
				}
				else if (base.ID.EqualsCaseInsensitive("drone_commands_unstuck"))
				{
					if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
					{
						DroneManager.Instance.RecoverDroneForPlayer(player);
					}
					else
					{
						SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageDroneRecoveryRequest>().Setup(player.entityId, base.ID), false);
					}
				}
			}
			EntityDrone entityDrone = respondent as EntityDrone;
			if (entityDrone != null)
			{
				if (base.ID.EqualsCaseInsensitive("drone_storage"))
				{
					entityDrone.OpenStorageFromDialog(player);
					return;
				}
				if (base.ID.EqualsCaseInsensitive("drone_command_follow") || base.ID.EqualsCaseInsensitive("drone_command_stay"))
				{
					entityDrone.ToggleOrderState();
					return;
				}
				if (base.ID.EqualsCaseInsensitive("drone_dont_heal_allies") || base.ID.EqualsCaseInsensitive("drone_heal_allies"))
				{
					entityDrone.ToggleHealAllies();
					return;
				}
				if (base.ID.EqualsCaseInsensitive("drone_attack_mode_passive") || base.ID.EqualsCaseInsensitive("drone_attack_mode_aggressive"))
				{
					entityDrone.ToggleAttackMode();
					return;
				}
				if (base.ID.EqualsCaseInsensitive("drone_light_on") || base.ID.EqualsCaseInsensitive("drone_light_off"))
				{
					entityDrone.ToggleLightAction();
					return;
				}
				if (base.ID.EqualsCaseInsensitive("drone_command_heal"))
				{
					entityDrone.HealRequest();
					return;
				}
				if (base.ID.EqualsCaseInsensitive("trader_response_nevermind"))
				{
					entityDrone.ProcessDialog(base.ID);
				}
			}
		}
	}

	// Token: 0x04000E56 RID: 3670
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cTraderRestock = "restock";

	// Token: 0x04000E57 RID: 3671
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cTraderTrade = "trade";

	// Token: 0x04000E58 RID: 3672
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cTraderResetQuests = "reset_quests";

	// Token: 0x04000E59 RID: 3673
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cTraderResponseNevermind = "trader_response_nevermind";

	// Token: 0x04000E5A RID: 3674
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cDroneCommandUnstuck = "drone_commands_unstuck";

	// Token: 0x04000E5B RID: 3675
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cDroneStorage = "drone_storage";

	// Token: 0x04000E5C RID: 3676
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cDroneCommandFollow = "drone_command_follow";

	// Token: 0x04000E5D RID: 3677
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cDroneCommandStay = "drone_command_stay";

	// Token: 0x04000E5E RID: 3678
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cDroneDontHealAllies = "drone_dont_heal_allies";

	// Token: 0x04000E5F RID: 3679
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cDroneHealAllies = "drone_heal_allies";

	// Token: 0x04000E60 RID: 3680
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cDroneAttackModePassive = "drone_attack_mode_passive";

	// Token: 0x04000E61 RID: 3681
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cDroneAttackModeAggressive = "drone_attack_mode_aggressive";

	// Token: 0x04000E62 RID: 3682
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cDroneLightOn = "drone_light_on";

	// Token: 0x04000E63 RID: 3683
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cDroneLightOff = "drone_light_off";

	// Token: 0x04000E64 RID: 3684
	[PublicizedFrom(EAccessModifier.Private)]
	public const string cDroneCommandHeal = "drone_command_heal";

	// Token: 0x04000E65 RID: 3685
	[PublicizedFrom(EAccessModifier.Private)]
	public string name = "";
}
