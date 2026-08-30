using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200197E RID: 6526
	[Preserve]
	public class ActionExplodePosition : BaseAction
	{
		// Token: 0x0600C873 RID: 51315 RVA: 0x00498814 File Offset: 0x00496A14
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

		// Token: 0x0600C874 RID: 51316 RVA: 0x0049891C File Offset: 0x00496B1C
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

		// Token: 0x0600C875 RID: 51317 RVA: 0x004989B8 File Offset: 0x00496BB8
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

		// Token: 0x0400975C RID: 38748
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blastPowerText;

		// Token: 0x0400975D RID: 38749
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockDamageText;

		// Token: 0x0400975E RID: 38750
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockRadiusText;

		// Token: 0x0400975F RID: 38751
		[PublicizedFrom(EAccessModifier.Protected)]
		public string entityDamageText;

		// Token: 0x04009760 RID: 38752
		[PublicizedFrom(EAccessModifier.Protected)]
		public string entityRadiusText;

		// Token: 0x04009761 RID: 38753
		[PublicizedFrom(EAccessModifier.Protected)]
		public int particleIndex = 13;

		// Token: 0x04009762 RID: 38754
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool ignoreHeatMap = true;

		// Token: 0x04009763 RID: 38755
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockTags = "";

		// Token: 0x04009764 RID: 38756
		public static string PropBlastPower = "blast_power";

		// Token: 0x04009765 RID: 38757
		public static string PropBlockDamage = "block_damage";

		// Token: 0x04009766 RID: 38758
		public static string PropBlockRadius = "block_radius";

		// Token: 0x04009767 RID: 38759
		public static string PropBlockTags = "block_tags";

		// Token: 0x04009768 RID: 38760
		public static string PropEntityDamage = "entity_damage";

		// Token: 0x04009769 RID: 38761
		public static string PropEntityRadius = "entity_radius";

		// Token: 0x0400976A RID: 38762
		public static string PropParticleIndex = "particle_index";

		// Token: 0x0400976B RID: 38763
		public static string PropIgnoreHeatMap = "ignore_heatmap";
	}
}
