using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019AB RID: 6571
	[Preserve]
	public class ActionExplodeTarget : ActionBaseTargetAction
	{
		// Token: 0x0600C9D0 RID: 51664 RVA: 0x0049CF64 File Offset: 0x0049B164
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

		// Token: 0x0600C9D1 RID: 51665 RVA: 0x0049D060 File Offset: 0x0049B260
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

		// Token: 0x0600C9D2 RID: 51666 RVA: 0x0049D0FC File Offset: 0x0049B2FC
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

		// Token: 0x04009832 RID: 38962
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blastPowerText;

		// Token: 0x04009833 RID: 38963
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockDamageText;

		// Token: 0x04009834 RID: 38964
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockRadiusText;

		// Token: 0x04009835 RID: 38965
		[PublicizedFrom(EAccessModifier.Protected)]
		public string entityDamageText;

		// Token: 0x04009836 RID: 38966
		[PublicizedFrom(EAccessModifier.Protected)]
		public string entityRadiusText;

		// Token: 0x04009837 RID: 38967
		[PublicizedFrom(EAccessModifier.Protected)]
		public int particleIndex = 13;

		// Token: 0x04009838 RID: 38968
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool ignoreHeatMap = true;

		// Token: 0x04009839 RID: 38969
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockTags = "";

		// Token: 0x0400983A RID: 38970
		public static string PropBlastPower = "blast_power";

		// Token: 0x0400983B RID: 38971
		public static string PropBlockDamage = "block_damage";

		// Token: 0x0400983C RID: 38972
		public static string PropBlockRadius = "block_radius";

		// Token: 0x0400983D RID: 38973
		public static string PropBlockTags = "block_tags";

		// Token: 0x0400983E RID: 38974
		public static string PropEntityDamage = "entity_damage";

		// Token: 0x0400983F RID: 38975
		public static string PropEntityRadius = "entity_radius";

		// Token: 0x04009840 RID: 38976
		public static string PropParticleIndex = "particle_index";

		// Token: 0x04009841 RID: 38977
		public static string PropIgnoreHeatMap = "ignore_heatmap";
	}
}
