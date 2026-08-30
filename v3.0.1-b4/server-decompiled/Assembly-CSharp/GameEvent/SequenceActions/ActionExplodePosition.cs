using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001958 RID: 6488
	[Preserve]
	public class ActionExplodePosition : BaseAction
	{
		// Token: 0x0600C6EC RID: 50924 RVA: 0x004924CC File Offset: 0x004906CC
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

		// Token: 0x0600C6ED RID: 50925 RVA: 0x004925D4 File Offset: 0x004907D4
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

		// Token: 0x0600C6EE RID: 50926 RVA: 0x00492670 File Offset: 0x00490870
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

		// Token: 0x04009633 RID: 38451
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blastPowerText;

		// Token: 0x04009634 RID: 38452
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockDamageText;

		// Token: 0x04009635 RID: 38453
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockRadiusText;

		// Token: 0x04009636 RID: 38454
		[PublicizedFrom(EAccessModifier.Protected)]
		public string entityDamageText;

		// Token: 0x04009637 RID: 38455
		[PublicizedFrom(EAccessModifier.Protected)]
		public string entityRadiusText;

		// Token: 0x04009638 RID: 38456
		[PublicizedFrom(EAccessModifier.Protected)]
		public int particleIndex = 13;

		// Token: 0x04009639 RID: 38457
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool ignoreHeatMap = true;

		// Token: 0x0400963A RID: 38458
		[PublicizedFrom(EAccessModifier.Protected)]
		public string blockTags = "";

		// Token: 0x0400963B RID: 38459
		public static string PropBlastPower = "blast_power";

		// Token: 0x0400963C RID: 38460
		public static string PropBlockDamage = "block_damage";

		// Token: 0x0400963D RID: 38461
		public static string PropBlockRadius = "block_radius";

		// Token: 0x0400963E RID: 38462
		public static string PropBlockTags = "block_tags";

		// Token: 0x0400963F RID: 38463
		public static string PropEntityDamage = "entity_damage";

		// Token: 0x04009640 RID: 38464
		public static string PropEntityRadius = "entity_radius";

		// Token: 0x04009641 RID: 38465
		public static string PropParticleIndex = "particle_index";

		// Token: 0x04009642 RID: 38466
		public static string PropIgnoreHeatMap = "ignore_heatmap";
	}
}
