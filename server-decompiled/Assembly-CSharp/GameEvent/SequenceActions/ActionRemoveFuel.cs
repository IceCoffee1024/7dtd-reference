using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019A3 RID: 6563
	[Preserve]
	public class ActionRemoveFuel : ActionBaseTargetAction
	{
		// Token: 0x0600C909 RID: 51465 RVA: 0x0049C9E8 File Offset: 0x0049ABE8
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

		// Token: 0x0600C90A RID: 51466 RVA: 0x0049CA30 File Offset: 0x0049AC30
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
		}

		// Token: 0x0600C90B RID: 51467 RVA: 0x0049CA39 File Offset: 0x0049AC39
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
