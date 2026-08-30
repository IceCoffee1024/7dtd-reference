using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D5 RID: 6613
	[Preserve]
	public class ActionUpdateBossGroup : BaseAction
	{
		// Token: 0x0600C9EA RID: 51690 RVA: 0x004A1080 File Offset: 0x0049F280
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			GameEventManager.Current.UpdateBossGroupType(base.Owner.CurrentBossGroupID, this.bossGroupType);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C9EB RID: 51691 RVA: 0x004A109E File Offset: 0x0049F29E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<BossGroup.BossGroupTypes>(ActionUpdateBossGroup.PropGroupType, ref this.bossGroupType);
		}

		// Token: 0x0600C9EC RID: 51692 RVA: 0x004A10B8 File Offset: 0x0049F2B8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionUpdateBossGroup
			{
				bossGroupType = this.bossGroupType
			};
		}

		// Token: 0x0400993A RID: 39226
		[PublicizedFrom(EAccessModifier.Protected)]
		public BossGroup.BossGroupTypes bossGroupType;

		// Token: 0x0400993B RID: 39227
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupType = "group_type";
	}
}
