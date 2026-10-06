using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A01 RID: 6657
	[Preserve]
	public class ActionUpdateBossGroup : BaseAction
	{
		// Token: 0x0600CB42 RID: 52034 RVA: 0x004A5525 File Offset: 0x004A3725
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			GameEventManager.Current.UpdateBossGroupType(base.Owner.CurrentBossGroupID, this.bossGroupType);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CB43 RID: 52035 RVA: 0x004A5543 File Offset: 0x004A3743
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<BossGroup.BossGroupTypes>(ActionUpdateBossGroup.PropGroupType, ref this.bossGroupType);
		}

		// Token: 0x0600CB44 RID: 52036 RVA: 0x004A555D File Offset: 0x004A375D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionUpdateBossGroup
			{
				bossGroupType = this.bossGroupType
			};
		}

		// Token: 0x04009A00 RID: 39424
		[PublicizedFrom(EAccessModifier.Protected)]
		public BossGroup.BossGroupTypes bossGroupType;

		// Token: 0x04009A01 RID: 39425
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupType = "group_type";
	}
}
