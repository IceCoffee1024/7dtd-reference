using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A13 RID: 6675
	[Preserve]
	public class ActionSetFuel : ActionBaseTargetAction
	{
		// Token: 0x0600CBD7 RID: 52183 RVA: 0x004A6F84 File Offset: 0x004A5184
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

		// Token: 0x0600CBD8 RID: 52184 RVA: 0x004A700C File Offset: 0x004A520C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<ActionSetFuel.FuelSettingTypes>(ActionSetFuel.PropFuelSettingType, ref this.SettingType);
		}

		// Token: 0x0600CBD9 RID: 52185 RVA: 0x004A7026 File Offset: 0x004A5226
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSetFuel
			{
				targetGroup = this.targetGroup,
				SettingType = this.SettingType
			};
		}

		// Token: 0x04009A68 RID: 39528
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionSetFuel.FuelSettingTypes SettingType;

		// Token: 0x04009A69 RID: 39529
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropFuelSettingType = "setting_type";

		// Token: 0x02001A14 RID: 6676
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum FuelSettingTypes
		{
			// Token: 0x04009A6B RID: 39531
			Remove,
			// Token: 0x04009A6C RID: 39532
			Fill
		}
	}
}
