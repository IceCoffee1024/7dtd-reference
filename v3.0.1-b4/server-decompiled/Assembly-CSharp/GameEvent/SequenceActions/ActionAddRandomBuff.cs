using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200193B RID: 6459
	[Preserve]
	public class ActionAddRandomBuff : ActionBaseTargetAction
	{
		// Token: 0x0600C666 RID: 50790 RVA: 0x0048EE70 File Offset: 0x0048D070
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			if (this.removesBuffs == null)
			{
				this.removesBuffs = this.removesBuff.Split(',', StringSplitOptions.None);
			}
			if (this.buffNames == null)
			{
				this.buffNames = this.addsBuff.Split(',', StringSplitOptions.None);
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
					string text = this.buffNames[target.rand.RandomRange(this.buffNames.Length)];
					entityAlive.Buffs.AddBuff(text, -1, true, false, -1f);
				}
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C667 RID: 50791 RVA: 0x0048EF3A File Offset: 0x0048D13A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddRandomBuff.PropBuffName, ref this.addsBuff);
			properties.ParseString(ActionAddRandomBuff.PropRemovesBuff, ref this.removesBuff);
		}

		// Token: 0x0600C668 RID: 50792 RVA: 0x0048EF65 File Offset: 0x0048D165
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionAddRandomBuff
			{
				addsBuff = this.addsBuff,
				removesBuff = this.removesBuff,
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x0400958D RID: 38285
		[PublicizedFrom(EAccessModifier.Protected)]
		public string addsBuff = "";

		// Token: 0x0400958E RID: 38286
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] buffNames;

		// Token: 0x0400958F RID: 38287
		[PublicizedFrom(EAccessModifier.Protected)]
		public string removesBuff = "";

		// Token: 0x04009590 RID: 38288
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] removesBuffs;

		// Token: 0x04009591 RID: 38289
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_names";

		// Token: 0x04009592 RID: 38290
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemovesBuff = "removes_buff";
	}
}
