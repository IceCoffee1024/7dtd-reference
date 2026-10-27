using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019FC RID: 6652
	[Preserve]
	public class ActionRemoveBuff : ActionBaseTargetAction
	{
		// Token: 0x0600CB5C RID: 52060 RVA: 0x004A4644 File Offset: 0x004A2844
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
			this.BuffList = this.buffName.Split(',', StringSplitOptions.None);
		}

		// Token: 0x0600CB5D RID: 52061 RVA: 0x004A465C File Offset: 0x004A285C
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

		// Token: 0x0600CB5E RID: 52062 RVA: 0x004A46B7 File Offset: 0x004A28B7
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(ActionRemoveBuff.PropBuffName))
			{
				this.buffName = properties.Values[ActionRemoveBuff.PropBuffName];
			}
		}

		// Token: 0x0600CB5F RID: 52063 RVA: 0x004A46E8 File Offset: 0x004A28E8
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

		// Token: 0x040099F3 RID: 39411
		[PublicizedFrom(EAccessModifier.Protected)]
		public string buffName = "";

		// Token: 0x040099F4 RID: 39412
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] BuffList;

		// Token: 0x040099F5 RID: 39413
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBuffName = "buff_name";
	}
}
