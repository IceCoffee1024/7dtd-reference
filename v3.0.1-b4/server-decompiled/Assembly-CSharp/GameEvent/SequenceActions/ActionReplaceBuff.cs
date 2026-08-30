using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001982 RID: 6530
	[Preserve]
	public class ActionReplaceBuff : ActionBaseTargetAction
	{
		// Token: 0x0600C795 RID: 51093 RVA: 0x00496E70 File Offset: 0x00495070
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null && entityAlive.Buffs.HasBuff(this.replaceBuff))
			{
				entityAlive.Buffs.RemoveBuff(this.replaceBuff, -1, true);
				entityAlive.Buffs.AddBuff(this.replaceWithBuff, -1, true, false, -1f);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C796 RID: 51094 RVA: 0x00496ECE File Offset: 0x004950CE
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionReplaceBuff.PropReplaceBuffName, ref this.replaceBuff);
			properties.ParseString(ActionReplaceBuff.PropReplaceWithBuffName, ref this.replaceWithBuff);
		}

		// Token: 0x0600C797 RID: 51095 RVA: 0x00496EF9 File Offset: 0x004950F9
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionReplaceBuff
			{
				replaceBuff = this.replaceBuff,
				replaceWithBuff = this.replaceWithBuff,
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x04009733 RID: 38707
		[PublicizedFrom(EAccessModifier.Protected)]
		public string replaceBuff = "";

		// Token: 0x04009734 RID: 38708
		[PublicizedFrom(EAccessModifier.Protected)]
		public string replaceWithBuff = "";

		// Token: 0x04009735 RID: 38709
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropReplaceBuffName = "replace_buff";

		// Token: 0x04009736 RID: 38710
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropReplaceWithBuffName = "replace_with_buff";
	}
}
