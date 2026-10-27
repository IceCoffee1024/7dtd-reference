using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019DB RID: 6619
	[Preserve]
	public class ActionExplodePosition : BaseAction
	{
		// Token: 0x0600CADF RID: 51935 RVA: 0x004A07D8 File Offset: 0x0049E9D8
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			if (base.Owner.TargetPosition.y == 0f)
			{
				return BaseAction.ActionCompleteStates.InCompleteRefund;
			}
			Vector3 targetPosition = base.Owner.TargetPosition;
			EntityAlive entityAlive = base.Owner.Target as EntityAlive;
			ExplosionData explosionData = new ExplosionData
			{
				BlastPower = GameEventManager.GetIntValue(entityAlive, this.blastPowerText, 75),
				BlockDamage = GameEventManager.GetFloatValue(entityAlive, this.blockDamageText, 1f),
				BlockRadius = GameEventManager.GetFloatValue(entityAlive, this.blockRadiusText, 4f),
				BlockTags = this.blockTags,
				EntityDamage = GameEventManager.GetFloatValue(entityAlive, this.entityDamageText, 5000f),
				EntityRadius = GameEventManager.GetIntValue(entityAlive, this.entityRadiusText, 3),
				ParticleIndex = this.particleIndex,
				IgnoreHeatMap = this.ignoreHeatMap
			};
			GameManager.Instance.ExplosionServer(targetPosition, World.worldToBlockPos(targetPosition), Quaternion.identity, explosionData, -1, 0.1f, false, null);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CAE0 RID: 51936 RVA: 0x004A08E0 File Offset: 0x0049EAE0
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionExplodePosition.PropBlastPower, ref this.blastPowerText);
			properties.ParseString(ActionExplodePosition.PropBlockDamage, ref this.blockDamageText);
			properties.ParseString(ActionExplodePosition.PropBlockRadius, ref this.blockRadiusText);
			properties.ParseString(ActionExplodePosition.PropEntityDamage, ref this.entityDamageText);
			properties.ParseString(ActionExplodePosition.PropEntityRadius, ref this.entityRadiusText);
			properties.ParseString(ActionExplodePosition.PropBlockTags, ref this.blockTags);
			properties.ParseInt(ActionExplodePosition.PropParticleIndex, ref this.particleIndex);
			properties.ParseBool(ActionExplodePosition.PropIgnoreHeatMap, ref this.ignoreHeatMap);
		}

		// Token: 0x0600CAE1 RID: 51937 RVA: 0x004A097C File Offset: 0x0049EB7C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionExplodePosition
			{
				blastPowerText = this.blastPowerText,
				blockDamageText = this.blockDamageText,
				blockRadiusText = this.blockRadiusText,
				entityDamageText = this.entityDamageText,
				entityRadiusText = this.entityRadiusText,
				particleIndex = this.particleIndex,
				blockTags = this.blockTags
			};
		}

		// Token: 0x0400990B RID: 39179
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blastPowerText;

		// Token: 0x0400990C RID: 39180
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockDamageText;

		// Token: 0x0400990D RID: 39181
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockRadiusText;

		// Token: 0x0400990E RID: 39182
		[PublicizedFrom(EAccessModifier.Protected)]
		public string entityDamageText;

		// Token: 0x0400990F RID: 39183
		[PublicizedFrom(EAccessModifier.Protected)]
		public string entityRadiusText;

		// Token: 0x04009910 RID: 39184
		[PublicizedFrom(EAccessModifier.Protected)]
		public int particleIndex = 13;

		// Token: 0x04009911 RID: 39185
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool ignoreHeatMap = true;

		// Token: 0x04009912 RID: 39186
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockTags = "";

		// Token: 0x04009913 RID: 39187
		public static string PropBlastPower = "blast_power";

		// Token: 0x04009914 RID: 39188
		public static string PropBlockDamage = "block_damage";

		// Token: 0x04009915 RID: 39189
		public static string PropBlockRadius = "block_radius";

		// Token: 0x04009916 RID: 39190
		public static string PropBlockTags = "block_tags";

		// Token: 0x04009917 RID: 39191
		public static string PropEntityDamage = "entity_damage";

		// Token: 0x04009918 RID: 39192
		public static string PropEntityRadius = "entity_radius";

		// Token: 0x04009919 RID: 39193
		public static string PropParticleIndex = "particle_index";

		// Token: 0x0400991A RID: 39194
		public static string PropIgnoreHeatMap = "ignore_heatmap";
	}
}
