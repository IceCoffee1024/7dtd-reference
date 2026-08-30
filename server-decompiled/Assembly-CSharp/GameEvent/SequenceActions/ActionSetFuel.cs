using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001990 RID: 6544
	[Preserve]
	public class ActionSetFuel : ActionBaseTargetAction
	{
		// Token: 0x0600C7E4 RID: 51172 RVA: 0x00498C3C File Offset: 0x00496E3C
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

		// Token: 0x0600C7E5 RID: 51173 RVA: 0x00498CC4 File Offset: 0x00496EC4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<ActionSetFuel.FuelSettingTypes>(ActionSetFuel.PropFuelSettingType, ref this.SettingType);
		}

		// Token: 0x0600C7E6 RID: 51174 RVA: 0x00498CDE File Offset: 0x00496EDE
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSetFuel
			{
				targetGroup = this.targetGroup,
				SettingType = this.SettingType
			};
		}

		// Token: 0x04009790 RID: 38800
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionSetFuel.FuelSettingTypes SettingType;

		// Token: 0x04009791 RID: 38801
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropFuelSettingType = "setting_type";

		// Token: 0x02001991 RID: 6545
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum FuelSettingTypes
		{
			// Token: 0x04009793 RID: 38803
			Remove,
			// Token: 0x04009794 RID: 38804
			Fill
		}
	}
}
