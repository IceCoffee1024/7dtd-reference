using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001957 RID: 6487
	[Preserve]
	public class ActionAddBuff : ActionBaseTargetAction
	{
		// Token: 0x0600C7BD RID: 51133 RVA: 0x00493C74 File Offset: 0x00491E74
		public override bool CanPerform(Entity target)
		{
			if (!this.checkAlreadyExists)
			{
				return true;
			}
			EntityAlive entityAlive = target as EntityAlive;
			return entityAlive != null && !entityAlive.Buffs.HasBuff(this.buffName) && (!(this.altVisionBuffName != "") || !entityAlive.Buffs.HasBuff(this.altVisionBuffName));
		}

		// Token: 0x0600C7BE RID: 51134 RVA: 0x00493CD4 File Offset: 0x00491ED4
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			if (this.removesBuffs == null)
			{
				this.removesBuffs = this.removesBuff.Split(',', StringSplitOptions.None);
			}
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				bool flag = false;
				for (int i = 0; i < this.removesBuffs.Length; i++)
				{
					if (entityAlive.Buffs.HasBuff(this.removesBuffs[i]))
					{
						entityAlive.Buffs.RemoveBuff(this.removesBuffs[i], -1, true);
						flag = true;
					}
				}
				if (!flag)
				{
					if (this.altVisionBuffName != "" && entityAlive is EntityPlayer && (entityAlive as EntityPlayer).TwitchVisionDisabled)
					{
						entityAlive.Buffs.AddBuff(this.altVisionBuffName, -1, true, false, -1f);
						return BaseAction.ActionCompleteStates.Complete;
					}
					entityAlive.Buffs.AddBuff(this.buffName, -1, true, false, this.duration);
					if (this.sequenceLink != "" && entityAlive.Buffs.GetBuff(this.buffName) != null)
					{
						GameEventManager.Current.RegisterLink(entityAlive as EntityPlayer, base.Owner, this.sequenceLink);
					}
				}
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C7BF RID: 51135 RVA: 0x00493DF8 File Offset: 0x00491FF8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddBuff.PropBuffName, ref this.buffName);
			properties.ParseString(ActionAddBuff.PropRemovesBuff, ref this.removesBuff);
			properties.ParseString(ActionAddBuff.PropAltVisionBuffName, ref this.altVisionBuffName);
			properties.ParseBool(ActionAddBuff.PropCheckAlreadyExists, ref this.checkAlreadyExists);
			properties.ParseString(ActionAddBuff.PropSequenceLink, ref this.sequenceLink);
			this.Properties.ParseFloat(ActionAddBuff.PropDuration, ref this.duration);
		}

		// Token: 0x0600C7C0 RID: 51136 RVA: 0x00493E78 File Offset: 0x00492078
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddBuff
			{
				buffName = this.buffName,
				removesBuff = this.removesBuff,
				targetGroup = this.targetGroup,
				altVisionBuffName = this.altVisionBuffName,
				checkAlreadyExists = this.checkAlreadyExists,
				sequenceLink = this.sequenceLink,
				duration = this.duration
			};
		}

		// Token: 0x04009667 RID: 38503
		[PublicizedFrom(EAccessModifier.Protected)]
		public string buffName = "";

		// Token: 0x04009668 RID: 38504
		[PublicizedFrom(EAccessModifier.Protected)]
		public string removesBuff = "";

		// Token: 0x04009669 RID: 38505
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] removesBuffs;

		// Token: 0x0400966A RID: 38506
		[PublicizedFrom(EAccessModifier.Protected)]
		public string altVisionBuffName = "";

		// Token: 0x0400966B RID: 38507
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool checkAlreadyExists = true;

		// Token: 0x0400966C RID: 38508
		[PublicizedFrom(EAccessModifier.Protected)]
		public string sequenceLink = "";

		// Token: 0x0400966D RID: 38509
		[PublicizedFrom(EAccessModifier.Protected)]
		public float duration = -1f;

		// Token: 0x0400966E RID: 38510
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_name";

		// Token: 0x0400966F RID: 38511
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemovesBuff = "removes_buff";

		// Token: 0x04009670 RID: 38512
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAltVisionBuffName = "alt_vision_buff_name";

		// Token: 0x04009671 RID: 38513
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCheckAlreadyExists = "check_already_exists";

		// Token: 0x04009672 RID: 38514
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSequenceLink = "sequence_link";

		// Token: 0x04009673 RID: 38515
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDuration = "duration";
	}
}
