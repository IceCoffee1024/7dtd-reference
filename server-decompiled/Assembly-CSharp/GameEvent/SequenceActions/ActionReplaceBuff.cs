using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A05 RID: 6661
	[Preserve]
	public class ActionReplaceBuff : ActionBaseTargetAction
	{
		// Token: 0x0600CB88 RID: 52104 RVA: 0x004A51A4 File Offset: 0x004A33A4
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

		// Token: 0x0600CB89 RID: 52105 RVA: 0x004A5202 File Offset: 0x004A3402
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionReplaceBuff.PropReplaceBuffName, ref this.replaceBuff);
			properties.ParseString(ActionReplaceBuff.PropReplaceWithBuffName, ref this.replaceWithBuff);
		}

		// Token: 0x0600CB8A RID: 52106 RVA: 0x004A522D File Offset: 0x004A342D
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

		// Token: 0x04009A0B RID: 39435
		[PublicizedFrom(EAccessModifier.Protected)]
		public string replaceBuff = "";

		// Token: 0x04009A0C RID: 39436
		[PublicizedFrom(EAccessModifier.Protected)]
		public string replaceWithBuff = "";

		// Token: 0x04009A0D RID: 39437
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropReplaceBuffName = "replace_buff";

		// Token: 0x04009A0E RID: 39438
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropReplaceWithBuffName = "replace_with_buff";
	}
}
