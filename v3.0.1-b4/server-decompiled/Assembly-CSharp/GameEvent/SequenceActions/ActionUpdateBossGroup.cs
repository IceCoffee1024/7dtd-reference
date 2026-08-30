using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019AF RID: 6575
	[Preserve]
	public class ActionUpdateBossGroup : BaseAction
	{
		// Token: 0x0600C863 RID: 51299 RVA: 0x0049AD28 File Offset: 0x00498F28
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			GameEventManager.Current.UpdateBossGroupType(base.Owner.CurrentBossGroupID, this.bossGroupType);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C864 RID: 51300 RVA: 0x0049AD46 File Offset: 0x00498F46
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<BossGroup.BossGroupTypes>(ActionUpdateBossGroup.PropGroupType, ref this.bossGroupType);
		}

		// Token: 0x0600C865 RID: 51301 RVA: 0x0049AD60 File Offset: 0x00498F60
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionUpdateBossGroup
			{
				bossGroupType = this.bossGroupType
			};
		}

		// Token: 0x04009811 RID: 38929
		[PublicizedFrom(EAccessModifier.Protected)]
		public BossGroup.BossGroupTypes bossGroupType;

		// Token: 0x04009812 RID: 38930
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupType = "group_type";
	}
}
