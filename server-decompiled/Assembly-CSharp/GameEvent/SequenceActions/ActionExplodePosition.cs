using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019AA RID: 6570
	[Preserve]
	public class ActionExplodePosition : BaseAction
	{
		// Token: 0x0600C9CB RID: 51659 RVA: 0x0049CCD8 File Offset: 0x0049AED8
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

		// Token: 0x0600C9CC RID: 51660 RVA: 0x0049CDE0 File Offset: 0x0049AFE0
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

		// Token: 0x0600C9CD RID: 51661 RVA: 0x0049CE7C File Offset: 0x0049B07C
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

		// Token: 0x04009822 RID: 38946
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blastPowerText;

		// Token: 0x04009823 RID: 38947
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockDamageText;

		// Token: 0x04009824 RID: 38948
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockRadiusText;

		// Token: 0x04009825 RID: 38949
		[PublicizedFrom(EAccessModifier.Protected)]
		public string entityDamageText;

		// Token: 0x04009826 RID: 38950
		[PublicizedFrom(EAccessModifier.Protected)]
		public string entityRadiusText;

		// Token: 0x04009827 RID: 38951
		[PublicizedFrom(EAccessModifier.Protected)]
		public int particleIndex = 13;

		// Token: 0x04009828 RID: 38952
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool ignoreHeatMap = true;

		// Token: 0x04009829 RID: 38953
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockTags = "";

		// Token: 0x0400982A RID: 38954
		public static string PropBlastPower = "blast_power";

		// Token: 0x0400982B RID: 38955
		public static string PropBlockDamage = "block_damage";

		// Token: 0x0400982C RID: 38956
		public static string PropBlockRadius = "block_radius";

		// Token: 0x0400982D RID: 38957
		public static string PropBlockTags = "block_tags";

		// Token: 0x0400982E RID: 38958
		public static string PropEntityDamage = "entity_damage";

		// Token: 0x0400982F RID: 38959
		public static string PropEntityRadius = "entity_radius";

		// Token: 0x04009830 RID: 38960
		public static string PropParticleIndex = "particle_index";

		// Token: 0x04009831 RID: 38961
		public static string PropIgnoreHeatMap = "ignore_heatmap";
	}
}
