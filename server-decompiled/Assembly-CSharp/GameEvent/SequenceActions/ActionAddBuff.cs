using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001983 RID: 6531
	[Preserve]
	public class ActionAddBuff : ActionBaseTargetAction
	{
		// Token: 0x0600C915 RID: 51477 RVA: 0x00498160 File Offset: 0x00496360
		public override bool CanPerform(Entity target)
		{
			if (!this.checkAlreadyExists)
			{
				return true;
			}
			EntityAlive entityAlive = target as EntityAlive;
			return entityAlive != null && !entityAlive.Buffs.HasBuff(this.buffName) && (!(this.altVisionBuffName != "") || !entityAlive.Buffs.HasBuff(this.altVisionBuffName));
		}

		// Token: 0x0600C916 RID: 51478 RVA: 0x004981C0 File Offset: 0x004963C0
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

		// Token: 0x0600C917 RID: 51479 RVA: 0x004982E4 File Offset: 0x004964E4
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

		// Token: 0x0600C918 RID: 51480 RVA: 0x00498364 File Offset: 0x00496564
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

		// Token: 0x0400972D RID: 38701
		[PublicizedFrom(EAccessModifier.Protected)]
		public string buffName = "";

		// Token: 0x0400972E RID: 38702
		[PublicizedFrom(EAccessModifier.Protected)]
		public string removesBuff = "";

		// Token: 0x0400972F RID: 38703
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] removesBuffs;

		// Token: 0x04009730 RID: 38704
		[PublicizedFrom(EAccessModifier.Protected)]
		public string altVisionBuffName = "";

		// Token: 0x04009731 RID: 38705
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool checkAlreadyExists = true;

		// Token: 0x04009732 RID: 38706
		[PublicizedFrom(EAccessModifier.Protected)]
		public string sequenceLink = "";

		// Token: 0x04009733 RID: 38707
		[PublicizedFrom(EAccessModifier.Protected)]
		public float duration = -1f;

		// Token: 0x04009734 RID: 38708
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_name";

		// Token: 0x04009735 RID: 38709
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemovesBuff = "removes_buff";

		// Token: 0x04009736 RID: 38710
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropAltVisionBuffName = "alt_vision_buff_name";

		// Token: 0x04009737 RID: 38711
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCheckAlreadyExists = "check_already_exists";

		// Token: 0x04009738 RID: 38712
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSequenceLink = "sequence_link";

		// Token: 0x04009739 RID: 38713
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropDuration = "duration";
	}
}
