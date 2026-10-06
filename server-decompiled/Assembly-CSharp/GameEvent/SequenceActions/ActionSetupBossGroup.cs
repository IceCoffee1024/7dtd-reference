using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019EB RID: 6635
	[Preserve]
	public class ActionSetupBossGroup : BaseAction
	{
		// Token: 0x0600CAE6 RID: 51942 RVA: 0x004A3BD8 File Offset: 0x004A1DD8
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

		// Token: 0x0600CAE7 RID: 51943 RVA: 0x004A3C8C File Offset: 0x004A1E8C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionSetupBossGroup.PropMinionGroupName, ref this.minionGroupName);
			properties.ParseString(ActionSetupBossGroup.PropBossGroupName, ref this.bossGroupName);
			properties.ParseString(ActionSetupBossGroup.PropBossIcon1, ref this.bossIcon1);
			properties.ParseEnum<BossGroup.BossGroupTypes>(ActionSetupBossGroup.PropGroupType, ref this.bossGroupType);
		}

		// Token: 0x0600CAE8 RID: 51944 RVA: 0x004A3CE4 File Offset: 0x004A1EE4
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

		// Token: 0x040099A0 RID: 39328
		[PublicizedFrom(EAccessModifier.Protected)]
		public string minionGroupName = "";

		// Token: 0x040099A1 RID: 39329
		[PublicizedFrom(EAccessModifier.Protected)]
		public string bossGroupName = "";

		// Token: 0x040099A2 RID: 39330
		[PublicizedFrom(EAccessModifier.Protected)]
		public string bossIcon1 = "";

		// Token: 0x040099A3 RID: 39331
		[PublicizedFrom(EAccessModifier.Protected)]
		public BossGroup.BossGroupTypes bossGroupType;

		// Token: 0x040099A4 RID: 39332
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinionGroupName = "minion_group_name";

		// Token: 0x040099A5 RID: 39333
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBossGroupName = "boss_group_name";

		// Token: 0x040099A6 RID: 39334
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBossIcon1 = "boss_icon1";

		// Token: 0x040099A7 RID: 39335
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupType = "group_type";
	}
}
