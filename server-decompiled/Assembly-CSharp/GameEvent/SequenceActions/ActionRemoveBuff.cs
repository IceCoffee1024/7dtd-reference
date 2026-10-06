using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019CB RID: 6603
	[Preserve]
	public class ActionRemoveBuff : ActionBaseTargetAction
	{
		// Token: 0x0600CA48 RID: 51784 RVA: 0x004A0B44 File Offset: 0x0049ED44
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.BuffList = this.buffName.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600CA49 RID: 51785 RVA: 0x004A0B5C File Offset: 0x0049ED5C
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

		// Token: 0x0600CA4A RID: 51786 RVA: 0x004A0BB7 File Offset: 0x0049EDB7
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionRemoveBuff.PropBuffName))
			{
				this.buffName = properties.Values[ActionRemoveBuff.PropBuffName];
			}
		}

		// Token: 0x0600CA4B RID: 51787 RVA: 0x004A0BE8 File Offset: 0x0049EDE8
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

		// Token: 0x0400990A RID: 39178
		[PublicizedFrom(EAccessModifier.Protected)]
		public string buffName = "";

		// Token: 0x0400990B RID: 39179
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] BuffList;

		// Token: 0x0400990C RID: 39180
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_name";
	}
}
