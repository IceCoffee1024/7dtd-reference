using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019E2 RID: 6626
	[Preserve]
	public class ActionSetFuel : ActionBaseTargetAction
	{
		// Token: 0x0600CAC3 RID: 51907 RVA: 0x004A3484 File Offset: 0x004A1684
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

		// Token: 0x0600CAC4 RID: 51908 RVA: 0x004A350C File Offset: 0x004A170C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<ActionSetFuel.FuelSettingTypes>(ActionSetFuel.PropFuelSettingType, ref this.SettingType);
		}

		// Token: 0x0600CAC5 RID: 51909 RVA: 0x004A3526 File Offset: 0x004A1726
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSetFuel
			{
				targetGroup = this.targetGroup,
				SettingType = this.SettingType
			};
		}

		// Token: 0x0400997F RID: 39295
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionSetFuel.FuelSettingTypes SettingType;

		// Token: 0x04009980 RID: 39296
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropFuelSettingType = "setting_type";

		// Token: 0x020019E3 RID: 6627
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum FuelSettingTypes
		{
			// Token: 0x04009982 RID: 39298
			Remove,
			// Token: 0x04009983 RID: 39299
			Fill
		}
	}
}
