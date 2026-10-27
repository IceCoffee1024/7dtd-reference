using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019BE RID: 6590
	[Preserve]
	public class ActionAddRandomBuff : ActionBaseTargetAction
	{
		// Token: 0x0600CA59 RID: 51801 RVA: 0x0049D0A0 File Offset: 0x0049B2A0
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

		// Token: 0x0600CA5A RID: 51802 RVA: 0x0049D16A File Offset: 0x0049B36A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddRandomBuff.PropBuffName, ref this.addsBuff);
			properties.ParseString(ActionAddRandomBuff.PropRemovesBuff, ref this.removesBuff);
		}

		// Token: 0x0600CA5B RID: 51803 RVA: 0x0049D195 File Offset: 0x0049B395
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

		// Token: 0x04009865 RID: 39013
		[PublicizedFrom(EAccessModifier.Protected)]
		public string addsBuff = "";

		// Token: 0x04009866 RID: 39014
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] buffNames;

		// Token: 0x04009867 RID: 39015
		[PublicizedFrom(EAccessModifier.Protected)]
		public string removesBuff = "";

		// Token: 0x04009868 RID: 39016
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] removesBuffs;

		// Token: 0x04009869 RID: 39017
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_names";

		// Token: 0x0400986A RID: 39018
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemovesBuff = "removes_buff";
	}
}
