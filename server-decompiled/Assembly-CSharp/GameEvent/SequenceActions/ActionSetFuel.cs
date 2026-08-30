using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B6 RID: 6582
	[Preserve]
	public class ActionSetFuel : ActionBaseTargetAction
	{
		// Token: 0x0600C96B RID: 51563 RVA: 0x0049EF94 File Offset: 0x0049D194
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityVehicle entityVehicle = target as EntityVehicle;
			if (entityVehicle != null)
			{
				ActionSetFuel.FuelSettingTypes settingType = this.SettingType;
				if (settingType != ActionSetFuel.FuelSettingTypes.Remove)
				{
					if (settingType == ActionSetFuel.FuelSettingTypes.Fill)
					{
						if (entityVehicle.vehicle.GetMaxFuelLevel() > 0f)
						{
							entityVehicle.vehicle.SetFuelLevel(entityVehicle.vehicle.GetMaxFuelLevel());
							entityVehicle.StopUIInteraction();
							return BaseAction.ActionCompleteStates.Complete;
						}
					}
				}
				else if (entityVehicle.vehicle.GetMaxFuelLevel() > 0f)
				{
					entityVehicle.vehicle.SetFuelLevel(0f);
					entityVehicle.StopUIInteraction();
					return BaseAction.ActionCompleteStates.Complete;
				}
			}
			return BaseAction.ActionCompleteStates.InComplete;
		}

		// Token: 0x0600C96C RID: 51564 RVA: 0x0049F01C File Offset: 0x0049D21C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<ActionSetFuel.FuelSettingTypes>(ActionSetFuel.PropFuelSettingType, ref this.SettingType);
		}

		// Token: 0x0600C96D RID: 51565 RVA: 0x0049F036 File Offset: 0x0049D236
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSetFuel
			{
				targetGroup = this.targetGroup,
				SettingType = this.SettingType
			};
		}

		// Token: 0x040098B9 RID: 39097
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionSetFuel.FuelSettingTypes SettingType;

		// Token: 0x040098BA RID: 39098
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropFuelSettingType = "setting_type";

		// Token: 0x020019B7 RID: 6583
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum FuelSettingTypes
		{
			// Token: 0x040098BC RID: 39100
			Remove,
			// Token: 0x040098BD RID: 39101
			Fill
		}
	}
}
