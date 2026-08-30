using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200197D RID: 6525
	[Preserve]
	public class ActionRemoveFuel : ActionBaseTargetAction
	{
		// Token: 0x0600C782 RID: 51074 RVA: 0x004966A4 File Offset: 0x004948A4
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

		// Token: 0x0600C783 RID: 51075 RVA: 0x004966EC File Offset: 0x004948EC
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
		}

		// Token: 0x0600C784 RID: 51076 RVA: 0x004966F5 File Offset: 0x004948F5
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
