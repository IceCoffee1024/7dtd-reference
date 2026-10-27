using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A00 RID: 6656
	[Preserve]
	public class ActionRemoveFuel : ActionBaseTargetAction
	{
		// Token: 0x0600CB75 RID: 52085 RVA: 0x004A49AC File Offset: 0x004A2BAC
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

		// Token: 0x0600CB76 RID: 52086 RVA: 0x004A49F4 File Offset: 0x004A2BF4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
		}

		// Token: 0x0600CB77 RID: 52087 RVA: 0x004A49FD File Offset: 0x004A2BFD
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
