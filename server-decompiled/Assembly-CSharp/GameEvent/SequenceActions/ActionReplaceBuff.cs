using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D4 RID: 6612
	[Preserve]
	public class ActionReplaceBuff : ActionBaseTargetAction
	{
		// Token: 0x0600CA74 RID: 51828 RVA: 0x004A16A4 File Offset: 0x0049F8A4
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

		// Token: 0x0600CA75 RID: 51829 RVA: 0x004A1702 File Offset: 0x0049F902
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionReplaceBuff.PropReplaceBuffName, ref this.replaceBuff);
			properties.ParseString(ActionReplaceBuff.PropReplaceWithBuffName, ref this.replaceWithBuff);
		}

		// Token: 0x0600CA76 RID: 51830 RVA: 0x004A172D File Offset: 0x0049F92D
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

		// Token: 0x04009922 RID: 39202
		[PublicizedFrom(EAccessModifier.Protected)]
		public string replaceBuff = "";

		// Token: 0x04009923 RID: 39203
		[PublicizedFrom(EAccessModifier.Protected)]
		public string replaceWithBuff = "";

		// Token: 0x04009924 RID: 39204
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropReplaceBuffName = "replace_buff";

		// Token: 0x04009925 RID: 39205
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropReplaceWithBuffName = "replace_with_buff";
	}
}
