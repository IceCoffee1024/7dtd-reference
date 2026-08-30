using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019BF RID: 6591
	[Preserve]
	public class ActionSetupBossGroup : BaseAction
	{
		// Token: 0x0600C98E RID: 51598 RVA: 0x0049F6EC File Offset: 0x0049D8EC
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

		// Token: 0x0600C98F RID: 51599 RVA: 0x0049F7A0 File Offset: 0x0049D9A0
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionSetupBossGroup.PropMinionGroupName, ref this.minionGroupName);
			properties.ParseString(ActionSetupBossGroup.PropBossGroupName, ref this.bossGroupName);
			properties.ParseString(ActionSetupBossGroup.PropBossIcon1, ref this.bossIcon1);
			properties.ParseEnum<BossGroup.BossGroupTypes>(ActionSetupBossGroup.PropGroupType, ref this.bossGroupType);
		}

		// Token: 0x0600C990 RID: 51600 RVA: 0x0049F7F8 File Offset: 0x0049D9F8
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

		// Token: 0x040098DA RID: 39130
		[PublicizedFrom(EAccessModifier.Protected)]
		public string minionGroupName = "";

		// Token: 0x040098DB RID: 39131
		[PublicizedFrom(EAccessModifier.Protected)]
		public string bossGroupName = "";

		// Token: 0x040098DC RID: 39132
		[PublicizedFrom(EAccessModifier.Protected)]
		public string bossIcon1 = "";

		// Token: 0x040098DD RID: 39133
		[PublicizedFrom(EAccessModifier.Protected)]
		public BossGroup.BossGroupTypes bossGroupType;

		// Token: 0x040098DE RID: 39134
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinionGroupName = "minion_group_name";

		// Token: 0x040098DF RID: 39135
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBossGroupName = "boss_group_name";

		// Token: 0x040098E0 RID: 39136
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBossIcon1 = "boss_icon1";

		// Token: 0x040098E1 RID: 39137
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupType = "group_type";
	}
}
