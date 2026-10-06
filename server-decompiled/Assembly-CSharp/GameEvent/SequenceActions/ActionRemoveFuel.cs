using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019CF RID: 6607
	[Preserve]
	public class ActionRemoveFuel : ActionBaseTargetAction
	{
		// Token: 0x0600CA61 RID: 51809 RVA: 0x004A0EAC File Offset: 0x0049F0AC
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityVehicle entityVehicle = target as EntityVehicle;
			if (entityVehicle != null && entityVehicle.vehicle.GetMaxFuelLevel() > 0f)
			{
				entityVehicle.vehicle.SetFuelLevel(0f);
				entityVehicle.StopUIInteraction();
				return BaseAction.ActionCompleteStates.Complete;
			}
			return BaseAction.ActionCompleteStates.InComplete;
		}

		// Token: 0x0600CA62 RID: 51810 RVA: 0x004A0EF4 File Offset: 0x0049F0F4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
		}

		// Token: 0x0600CA63 RID: 51811 RVA: 0x004A0EFD File Offset: 0x0049F0FD
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRemoveFuel
			{
				targetGroup = this.targetGroup
			};
		}
	}
}
