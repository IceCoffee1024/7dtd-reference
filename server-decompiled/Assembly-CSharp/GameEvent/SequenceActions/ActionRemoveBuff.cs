using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001979 RID: 6521
	[Preserve]
	public class ActionRemoveBuff : ActionBaseTargetAction
	{
		// Token: 0x0600C769 RID: 51049 RVA: 0x0049633C File Offset: 0x0049453C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.BuffList = this.buffName.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600C76A RID: 51050 RVA: 0x00496354 File Offset: 0x00494554
		public override BaseAction.ActionCompleteStates PerformTargetAction(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				for (int i = 0; i < this.BuffList.Length; i++)
				{
					if (entityAlive.Buffs.HasBuff(this.BuffList[i]))
					{
						entityAlive.Buffs.RemoveBuff(this.BuffList[i], -1, true);
					}
				}
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C76B RID: 51051 RVA: 0x004963AF File Offset: 0x004945AF
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionRemoveBuff.PropBuffName))
			{
				this.buffName = properties.Values[ActionRemoveBuff.PropBuffName];
			}
		}

		// Token: 0x0600C76C RID: 51052 RVA: 0x004963E0 File Offset: 0x004945E0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionRemoveBuff
			{
				buffName = this.buffName,
				BuffList = this.BuffList,
				targetGroup = this.targetGroup
			};
		}

		// Token: 0x0400971B RID: 38683
		[PublicizedFrom(EAccessModifier.Protected)]
		public string buffName = "";

		// Token: 0x0400971C RID: 38684
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] BuffList;

		// Token: 0x0400971D RID: 38685
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_name";
	}
}
