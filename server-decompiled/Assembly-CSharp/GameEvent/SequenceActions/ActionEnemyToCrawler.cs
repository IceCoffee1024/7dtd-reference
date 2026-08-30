using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200197D RID: 6525
	[Preserve]
	public class ActionEnemyToCrawler : ActionBaseTargetAction
	{
		// Token: 0x0600C870 RID: 51312 RVA: 0x0049877C File Offset: 0x0049697C
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

		// Token: 0x0600C871 RID: 51313 RVA: 0x004987FF File Offset: 0x004969FF
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
