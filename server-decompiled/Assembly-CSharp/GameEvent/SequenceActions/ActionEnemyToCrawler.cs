using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019DA RID: 6618
	[Preserve]
	public class ActionEnemyToCrawler : ActionBaseTargetAction
	{
		// Token: 0x0600CADC RID: 51932 RVA: 0x004A0740 File Offset: 0x0049E940
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

		// Token: 0x0600CADD RID: 51933 RVA: 0x004A07C3 File Offset: 0x0049E9C3
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
