using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001961 RID: 6497
	[Preserve]
	public class ActionAddRandomBuff : ActionBaseTargetAction
	{
		// Token: 0x0600C7ED RID: 51181 RVA: 0x004951BC File Offset: 0x004933BC
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

		// Token: 0x0600C7EE RID: 51182 RVA: 0x00495286 File Offset: 0x00493486
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddRandomBuff.PropBuffName, ref this.addsBuff);
			properties.ParseString(ActionAddRandomBuff.PropRemovesBuff, ref this.removesBuff);
		}

		// Token: 0x0600C7EF RID: 51183 RVA: 0x004952B1 File Offset: 0x004934B1
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

		// Token: 0x040096B6 RID: 38582
		[PublicizedFrom(EAccessModifier.Protected)]
		public string addsBuff = "";

		// Token: 0x040096B7 RID: 38583
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] buffNames;

		// Token: 0x040096B8 RID: 38584
		[PublicizedFrom(EAccessModifier.Protected)]
		public string removesBuff = "";

		// Token: 0x040096B9 RID: 38585
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] removesBuffs;

		// Token: 0x040096BA RID: 38586
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_names";

		// Token: 0x040096BB RID: 38587
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemovesBuff = "removes_buff";
	}
}
