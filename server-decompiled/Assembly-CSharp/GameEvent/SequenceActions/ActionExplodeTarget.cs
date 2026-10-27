using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019DC RID: 6620
	[Preserve]
	public class ActionExplodeTarget : ActionBaseTargetAction
	{
		// Token: 0x0600CAE4 RID: 51940 RVA: 0x004A0A64 File Offset: 0x0049EC64
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

		// Token: 0x0600CAE5 RID: 51941 RVA: 0x004A0B60 File Offset: 0x0049ED60
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

		// Token: 0x0600CAE6 RID: 51942 RVA: 0x004A0BFC File Offset: 0x0049EDFC
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

		// Token: 0x0400991B RID: 39195
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blastPowerText;

		// Token: 0x0400991C RID: 39196
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockDamageText;

		// Token: 0x0400991D RID: 39197
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockRadiusText;

		// Token: 0x0400991E RID: 39198
		[PublicizedFrom(EAccessModifier.Protected)]
		public string entityDamageText;

		// Token: 0x0400991F RID: 39199
		[PublicizedFrom(EAccessModifier.Protected)]
		public string entityRadiusText;

		// Token: 0x04009920 RID: 39200
		[PublicizedFrom(EAccessModifier.Protected)]
		public int particleIndex = 13;

		// Token: 0x04009921 RID: 39201
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool ignoreHeatMap = true;

		// Token: 0x04009922 RID: 39202
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockTags = "";

		// Token: 0x04009923 RID: 39203
		public static string PropBlastPower = "blast_power";

		// Token: 0x04009924 RID: 39204
		public static string PropBlockDamage = "block_damage";

		// Token: 0x04009925 RID: 39205
		public static string PropBlockRadius = "block_radius";

		// Token: 0x04009926 RID: 39206
		public static string PropBlockTags = "block_tags";

		// Token: 0x04009927 RID: 39207
		public static string PropEntityDamage = "entity_damage";

		// Token: 0x04009928 RID: 39208
		public static string PropEntityRadius = "entity_radius";

		// Token: 0x04009929 RID: 39209
		public static string PropParticleIndex = "particle_index";

		// Token: 0x0400992A RID: 39210
		public static string PropIgnoreHeatMap = "ignore_heatmap";
	}
}
