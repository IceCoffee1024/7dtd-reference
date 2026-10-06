using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019A9 RID: 6569
	[Preserve]
	public class ActionEnemyToCrawler : ActionBaseTargetAction
	{
		// Token: 0x0600C9C8 RID: 51656 RVA: 0x0049CC40 File Offset: 0x0049AE40
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null && !(entityAlive is EntityPlayer) && entityAlive is EntityHuman)
			{
				DamageResponse damageResponse = DamageResponse.New(false);
				damageResponse.Source = new DamageSource(EnumDamageSource.External, EnumDamageTypes.Bashing);
				damageResponse.Source.DismemberChance = 100000f;
				damageResponse.Strength = 1;
				damageResponse.CrippleLegs = true;
				damageResponse.Dismember = true;
				damageResponse.TurnIntoCrawler = true;
				damageResponse.HitBodyPart = EnumBodyPartHit.UpperLegs;
				entityAlive.ProcessDamageResponse(damageResponse);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C9C9 RID: 51657 RVA: 0x0049CCC3 File Offset: 0x0049AEC3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionEnemyToCrawler
			{
				targetGroup = this.targetGroup
			};
		}
	}
}
