using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200197F RID: 6527
	[Preserve]
	public class ActionExplodeTarget : ActionBaseTargetAction
	{
		// Token: 0x0600C878 RID: 51320 RVA: 0x00498AA0 File Offset: 0x00496CA0
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			EntityAlive entityAlive2 = base.Owner.Target as EntityAlive;
			if (entityAlive != null)
			{
				ExplosionData explosionData = new ExplosionData
				{
					BlastPower = GameEventManager.GetIntValue(entityAlive2, this.blastPowerText, 75),
					BlockDamage = GameEventManager.GetFloatValue(entityAlive2, this.blockDamageText, 1f),
					BlockRadius = GameEventManager.GetFloatValue(entityAlive2, this.blockRadiusText, 4f),
					BlockTags = this.blockTags,
					EntityDamage = GameEventManager.GetFloatValue(entityAlive2, this.entityDamageText, 5000f),
					EntityRadius = GameEventManager.GetIntValue(entityAlive2, this.entityRadiusText, 3),
					ParticleIndex = this.particleIndex,
					IgnoreHeatMap = this.ignoreHeatMap
				};
				GameManager.Instance.ExplosionServer(entityAlive.position, entityAlive.GetBlockPosition(), entityAlive.qrotation, explosionData, -1, 0.1f, false, null);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C879 RID: 51321 RVA: 0x00498B9C File Offset: 0x00496D9C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionExplodeTarget.PropBlastPower, ref this.blastPowerText);
			properties.ParseString(ActionExplodeTarget.PropBlockDamage, ref this.blockDamageText);
			properties.ParseString(ActionExplodeTarget.PropBlockRadius, ref this.blockRadiusText);
			properties.ParseString(ActionExplodeTarget.PropEntityDamage, ref this.entityDamageText);
			properties.ParseString(ActionExplodeTarget.PropEntityRadius, ref this.entityRadiusText);
			properties.ParseString(ActionExplodeTarget.PropBlockTags, ref this.blockTags);
			properties.ParseInt(ActionExplodeTarget.PropParticleIndex, ref this.particleIndex);
			properties.ParseBool(ActionExplodeTarget.PropIgnoreHeatMap, ref this.ignoreHeatMap);
		}

		// Token: 0x0600C87A RID: 51322 RVA: 0x00498C38 File Offset: 0x00496E38
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionExplodeTarget
			{
				targetGroup = this.targetGroup,
				blastPowerText = this.blastPowerText,
				blockDamageText = this.blockDamageText,
				blockRadiusText = this.blockRadiusText,
				entityDamageText = this.entityDamageText,
				entityRadiusText = this.entityRadiusText,
				particleIndex = this.particleIndex,
				blockTags = this.blockTags
			};
		}

		// Token: 0x0400976C RID: 38764
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blastPowerText;

		// Token: 0x0400976D RID: 38765
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockDamageText;

		// Token: 0x0400976E RID: 38766
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockRadiusText;

		// Token: 0x0400976F RID: 38767
		[PublicizedFrom(EAccessModifier.Protected)]
		public string entityDamageText;

		// Token: 0x04009770 RID: 38768
		[PublicizedFrom(EAccessModifier.Protected)]
		public string entityRadiusText;

		// Token: 0x04009771 RID: 38769
		[PublicizedFrom(EAccessModifier.Protected)]
		public int particleIndex = 13;

		// Token: 0x04009772 RID: 38770
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool ignoreHeatMap = true;

		// Token: 0x04009773 RID: 38771
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockTags = "";

		// Token: 0x04009774 RID: 38772
		public static string PropBlastPower = "blast_power";

		// Token: 0x04009775 RID: 38773
		public static string PropBlockDamage = "block_damage";

		// Token: 0x04009776 RID: 38774
		public static string PropBlockRadius = "block_radius";

		// Token: 0x04009777 RID: 38775
		public static string PropBlockTags = "block_tags";

		// Token: 0x04009778 RID: 38776
		public static string PropEntityDamage = "entity_damage";

		// Token: 0x04009779 RID: 38777
		public static string PropEntityRadius = "entity_radius";

		// Token: 0x0400977A RID: 38778
		public static string PropParticleIndex = "particle_index";

		// Token: 0x0400977B RID: 38779
		public static string PropIgnoreHeatMap = "ignore_heatmap";
	}
}
