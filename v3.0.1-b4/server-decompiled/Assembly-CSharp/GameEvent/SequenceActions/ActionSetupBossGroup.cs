using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001999 RID: 6553
	[Preserve]
	public class ActionSetupBossGroup : BaseAction
	{
		// Token: 0x0600C807 RID: 51207 RVA: 0x00499394 File Offset: 0x00497594
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			List<Entity> entityGroup = base.Owner.GetEntityGroup(this.bossGroupName);
			List<Entity> entityGroup2 = base.Owner.GetEntityGroup(this.minionGroupName);
			List<EntityAlive> list = new List<EntityAlive>();
			EntityAlive entityAlive = entityGroup[0] as EntityAlive;
			for (int i = 0; i < entityGroup2.Count; i++)
			{
				EntityAlive entityAlive2 = entityGroup2[i] as EntityAlive;
				if (entityAlive2 != null)
				{
					list.Add(entityAlive2);
				}
			}
			EntityPlayer entityPlayer = base.Owner.Target as EntityPlayer;
			if (entityPlayer != null)
			{
				base.Owner.CurrentBossGroupID = GameEventManager.Current.SetupBossGroup(entityPlayer, entityAlive, list, this.bossGroupType, this.bossIcon1);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C808 RID: 51208 RVA: 0x00499448 File Offset: 0x00497648
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionSetupBossGroup.PropMinionGroupName, ref this.minionGroupName);
			properties.ParseString(ActionSetupBossGroup.PropBossGroupName, ref this.bossGroupName);
			properties.ParseString(ActionSetupBossGroup.PropBossIcon1, ref this.bossIcon1);
			properties.ParseEnum<BossGroup.BossGroupTypes>(ActionSetupBossGroup.PropGroupType, ref this.bossGroupType);
		}

		// Token: 0x0600C809 RID: 51209 RVA: 0x004994A0 File Offset: 0x004976A0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSetupBossGroup
			{
				minionGroupName = this.minionGroupName,
				bossGroupName = this.bossGroupName,
				bossGroupType = this.bossGroupType,
				bossIcon1 = this.bossIcon1
			};
		}

		// Token: 0x040097B1 RID: 38833
		[PublicizedFrom(EAccessModifier.Protected)]
		public string minionGroupName = "";

		// Token: 0x040097B2 RID: 38834
		[PublicizedFrom(EAccessModifier.Protected)]
		public string bossGroupName = "";

		// Token: 0x040097B3 RID: 38835
		[PublicizedFrom(EAccessModifier.Protected)]
		public string bossIcon1 = "";

		// Token: 0x040097B4 RID: 38836
		[PublicizedFrom(EAccessModifier.Protected)]
		public BossGroup.BossGroupTypes bossGroupType;

		// Token: 0x040097B5 RID: 38837
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinionGroupName = "minion_group_name";

		// Token: 0x040097B6 RID: 38838
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBossGroupName = "boss_group_name";

		// Token: 0x040097B7 RID: 38839
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBossIcon1 = "boss_icon1";

		// Token: 0x040097B8 RID: 38840
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupType = "group_type";
	}
}
