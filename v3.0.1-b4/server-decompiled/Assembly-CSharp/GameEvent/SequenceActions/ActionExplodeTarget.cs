using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001959 RID: 6489
	[Preserve]
	public class ActionExplodeTarget : ActionBaseTargetAction
	{
		// Token: 0x0600C6F1 RID: 50929 RVA: 0x00492758 File Offset: 0x00490958
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

		// Token: 0x0600C6F2 RID: 50930 RVA: 0x00492854 File Offset: 0x00490A54
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

		// Token: 0x0600C6F3 RID: 50931 RVA: 0x004928F0 File Offset: 0x00490AF0
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

		// Token: 0x04009643 RID: 38467
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blastPowerText;

		// Token: 0x04009644 RID: 38468
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockDamageText;

		// Token: 0x04009645 RID: 38469
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockRadiusText;

		// Token: 0x04009646 RID: 38470
		[PublicizedFrom(EAccessModifier.Protected)]
		public string entityDamageText;

		// Token: 0x04009647 RID: 38471
		[PublicizedFrom(EAccessModifier.Protected)]
		public string entityRadiusText;

		// Token: 0x04009648 RID: 38472
		[PublicizedFrom(EAccessModifier.Protected)]
		public int particleIndex = 13;

		// Token: 0x04009649 RID: 38473
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool ignoreHeatMap = true;

		// Token: 0x0400964A RID: 38474
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockTags = "";

		// Token: 0x0400964B RID: 38475
		public static string PropBlastPower = "blast_power";

		// Token: 0x0400964C RID: 38476
		public static string PropBlockDamage = "block_damage";

		// Token: 0x0400964D RID: 38477
		public static string PropBlockRadius = "block_radius";

		// Token: 0x0400964E RID: 38478
		public static string PropBlockTags = "block_tags";

		// Token: 0x0400964F RID: 38479
		public static string PropEntityDamage = "entity_damage";

		// Token: 0x04009650 RID: 38480
		public static string PropEntityRadius = "entity_radius";

		// Token: 0x04009651 RID: 38481
		public static string PropParticleIndex = "particle_index";

		// Token: 0x04009652 RID: 38482
		public static string PropIgnoreHeatMap = "ignore_heatmap";
	}
}
