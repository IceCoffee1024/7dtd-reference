using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200198D RID: 6541
	[Preserve]
	public class ActionAddRandomBuff : ActionBaseTargetAction
	{
		// Token: 0x0600C945 RID: 51525 RVA: 0x004996AC File Offset: 0x004978AC
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

		// Token: 0x0600C946 RID: 51526 RVA: 0x00499776 File Offset: 0x00497976
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionAddRandomBuff.PropBuffName, ref this.addsBuff);
			properties.ParseString(ActionAddRandomBuff.PropRemovesBuff, ref this.removesBuff);
		}

		// Token: 0x0600C947 RID: 51527 RVA: 0x004997A1 File Offset: 0x004979A1
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

		// Token: 0x0400977C RID: 38780
		[PublicizedFrom(EAccessModifier.Protected)]
		public string addsBuff = "";

		// Token: 0x0400977D RID: 38781
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] buffNames;

		// Token: 0x0400977E RID: 38782
		[PublicizedFrom(EAccessModifier.Protected)]
		public string removesBuff = "";

		// Token: 0x0400977F RID: 38783
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] removesBuffs;

		// Token: 0x04009780 RID: 38784
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_names";

		// Token: 0x04009781 RID: 38785
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropRemovesBuff = "removes_buff";
	}
}
