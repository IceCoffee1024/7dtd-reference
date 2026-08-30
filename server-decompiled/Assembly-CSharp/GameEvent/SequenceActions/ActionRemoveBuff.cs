using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200199F RID: 6559
	[Preserve]
	public class ActionRemoveBuff : ActionBaseTargetAction
	{
		// Token: 0x0600C8F0 RID: 51440 RVA: 0x0049C680 File Offset: 0x0049A880
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.BuffList = this.buffName.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600C8F1 RID: 51441 RVA: 0x0049C698 File Offset: 0x0049A898
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

		// Token: 0x0600C8F2 RID: 51442 RVA: 0x0049C6F3 File Offset: 0x0049A8F3
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionRemoveBuff.PropBuffName))
			{
				this.buffName = properties.Values[ActionRemoveBuff.PropBuffName];
			}
		}

		// Token: 0x0600C8F3 RID: 51443 RVA: 0x0049C724 File Offset: 0x0049A924
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

		// Token: 0x04009844 RID: 38980
		[PublicizedFrom(EAccessModifier.Protected)]
		public string buffName = "";

		// Token: 0x04009845 RID: 38981
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] BuffList;

		// Token: 0x04009846 RID: 38982
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_name";
	}
}
