using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019A8 RID: 6568
	[Preserve]
	public class ActionReplaceBuff : ActionBaseTargetAction
	{
		// Token: 0x0600C91C RID: 51484 RVA: 0x0049D1B4 File Offset: 0x0049B3B4
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

		// Token: 0x0600C91D RID: 51485 RVA: 0x0049D212 File Offset: 0x0049B412
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionReplaceBuff.PropReplaceBuffName, ref this.replaceBuff);
			properties.ParseString(ActionReplaceBuff.PropReplaceWithBuffName, ref this.replaceWithBuff);
		}

		// Token: 0x0600C91E RID: 51486 RVA: 0x0049D23D File Offset: 0x0049B43D
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

		// Token: 0x0400985C RID: 39004
		[PublicizedFrom(EAccessModifier.Protected)]
		public string replaceBuff = "";

		// Token: 0x0400985D RID: 39005
		[PublicizedFrom(EAccessModifier.Protected)]
		public string replaceWithBuff = "";

		// Token: 0x0400985E RID: 39006
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropReplaceBuffName = "replace_buff";

		// Token: 0x0400985F RID: 39007
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropReplaceWithBuffName = "replace_with_buff";
	}
}
