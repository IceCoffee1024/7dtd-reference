using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001931 RID: 6449
	[Preserve]
	public class ActionAddBuff : ActionBaseTargetAction
	{
		// Token: 0x0600C636 RID: 50742 RVA: 0x0048D928 File Offset: 0x0048BB28
		public override bool CanPerform(Entity target)
		{
			if (!this.checkAlreadyExists)
			{
				return true;
			}
			EntityAlive entityAlive = target as EntityAlive;
			return entityAlive != null && !entityAlive.Buffs.HasBuff(this.buffName) && (!(this.altVisionBuffName != "") || !entityAlive.Buffs.HasBuff(this.altVisionBuffName));
		}

		// Token: 0x0600C637 RID: 50743 RVA: 0x0048D988 File Offset: 0x0048BB88
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

		// Token: 0x0600C638 RID: 50744 RVA: 0x0048DAAC File Offset: 0x0048BCAC
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

		// Token: 0x0600C639 RID: 50745 RVA: 0x0048DB2C File Offset: 0x0048BD2C
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

		// Token: 0x0400953E RID: 38206
		[PublicizedFrom(EAccessModifier.Protected)]
		public string buffName = "";

		// Token: 0x0400953F RID: 38207
		[PublicizedFrom(EAccessModifier.Protected)]
		public string removesBuff = "";

		// Token: 0x04009540 RID: 38208
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] removesBuffs;

		// Token: 0x04009541 RID: 38209
		[PublicizedFrom(EAccessModifier.Protected)]
		public string altVisionBuffName = "";

		// Token: 0x04009542 RID: 38210
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool checkAlreadyExists = true;

		// Token: 0x04009543 RID: 38211
		[PublicizedFrom(EAccessModifier.Protected)]
		public string sequenceLink = "";

		// Token: 0x04009544 RID: 38212
		[PublicizedFrom(EAccessModifier.Protected)]
		public float duration = -1f;

		// Token: 0x04009545 RID: 38213
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_name";

		// Token: 0x04009546 RID: 38214
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemovesBuff = "removes_buff";

		// Token: 0x04009547 RID: 38215
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAltVisionBuffName = "alt_vision_buff_name";

		// Token: 0x04009548 RID: 38216
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCheckAlreadyExists = "check_already_exists";

		// Token: 0x04009549 RID: 38217
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSequenceLink = "sequence_link";

		// Token: 0x0400954A RID: 38218
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDuration = "duration";
	}
}
