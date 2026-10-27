using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A32 RID: 6706
	[Preserve]
	public class ActionUpdateBossGroup : BaseAction
	{
		// Token: 0x0600CC56 RID: 52310 RVA: 0x004A9021 File Offset: 0x004A7221
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			GameEventManager.Current.UpdateBossGroupType(base.Owner.CurrentBossGroupID, this.bossGroupType);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CC57 RID: 52311 RVA: 0x004A903F File Offset: 0x004A723F
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<BossGroup.BossGroupTypes>(ActionUpdateBossGroup.PropGroupType, ref this.bossGroupType);
		}

		// Token: 0x0600CC58 RID: 52312 RVA: 0x004A9059 File Offset: 0x004A7259
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionUpdateBossGroup
			{
				bossGroupType = this.bossGroupType
			};
		}

		// Token: 0x04009AE9 RID: 39657
		[PublicizedFrom(EAccessModifier.Protected)]
		public BossGroup.BossGroupTypes bossGroupType;

		// Token: 0x04009AEA RID: 39658
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupType = "group_type";
	}
}
