using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B4 RID: 6580
	[Preserve]
	public class ActionAddBuff : ActionBaseTargetAction
	{
		// Token: 0x0600CA29 RID: 51753 RVA: 0x0049BB54 File Offset: 0x00499D54
		public override bool CanPerform(Entity target)
		{
			if (!this.checkAlreadyExists)
			{
				return true;
			}
			EntityAlive entityAlive = target as EntityAlive;
			return entityAlive != null && !entityAlive.Buffs.HasBuff(this.buffName) && (!(this.altVisionBuffName != "") || !entityAlive.Buffs.HasBuff(this.altVisionBuffName));
		}

		// Token: 0x0600CA2A RID: 51754 RVA: 0x0049BBB4 File Offset: 0x00499DB4
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

		// Token: 0x0600CA2B RID: 51755 RVA: 0x0049BCD8 File Offset: 0x00499ED8
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

		// Token: 0x0600CA2C RID: 51756 RVA: 0x0049BD58 File Offset: 0x00499F58
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

		// Token: 0x04009816 RID: 38934
		[PublicizedFrom(EAccessModifier.Protected)]
		public string buffName = "";

		// Token: 0x04009817 RID: 38935
		[PublicizedFrom(EAccessModifier.Protected)]
		public string removesBuff = "";

		// Token: 0x04009818 RID: 38936
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] removesBuffs;

		// Token: 0x04009819 RID: 38937
		[PublicizedFrom(EAccessModifier.Protected)]
		public string altVisionBuffName = "";

		// Token: 0x0400981A RID: 38938
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool checkAlreadyExists = true;

		// Token: 0x0400981B RID: 38939
		[PublicizedFrom(EAccessModifier.Protected)]
		public string sequenceLink = "";

		// Token: 0x0400981C RID: 38940
		[PublicizedFrom(EAccessModifier.Protected)]
		public float duration = -1f;

		// Token: 0x0400981D RID: 38941
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_name";

		// Token: 0x0400981E RID: 38942
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemovesBuff = "removes_buff";

		// Token: 0x0400981F RID: 38943
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAltVisionBuffName = "alt_vision_buff_name";

		// Token: 0x04009820 RID: 38944
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCheckAlreadyExists = "check_already_exists";

		// Token: 0x04009821 RID: 38945
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSequenceLink = "sequence_link";

		// Token: 0x04009822 RID: 38946
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDuration = "duration";
	}
}
