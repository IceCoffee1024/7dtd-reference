using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001957 RID: 6487
	[Preserve]
	public class ActionEnemyToCrawler : ActionBaseTargetAction
	{
		// Token: 0x0600C6E9 RID: 50921 RVA: 0x00492434 File Offset: 0x00490634
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

		// Token: 0x0600C6EA RID: 50922 RVA: 0x004924B7 File Offset: 0x004906B7
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
