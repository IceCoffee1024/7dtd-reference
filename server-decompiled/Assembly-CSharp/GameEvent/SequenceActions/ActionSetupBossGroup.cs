using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A1C RID: 6684
	[Preserve]
	public class ActionSetupBossGroup : BaseAction
	{
		// Token: 0x0600CBFA RID: 52218 RVA: 0x004A76D8 File Offset: 0x004A58D8
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

		// Token: 0x0600CBFB RID: 52219 RVA: 0x004A778C File Offset: 0x004A598C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionSetupBossGroup.PropMinionGroupName, ref this.minionGroupName);
			properties.ParseString(ActionSetupBossGroup.PropBossGroupName, ref this.bossGroupName);
			properties.ParseString(ActionSetupBossGroup.PropBossIcon1, ref this.bossIcon1);
			properties.ParseEnum<BossGroup.BossGroupTypes>(ActionSetupBossGroup.PropGroupType, ref this.bossGroupType);
		}

		// Token: 0x0600CBFC RID: 52220 RVA: 0x004A77E4 File Offset: 0x004A59E4
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

		// Token: 0x04009A89 RID: 39561
		[PublicizedFrom(EAccessModifier.Protected)]
		public string minionGroupName = "";

		// Token: 0x04009A8A RID: 39562
		[PublicizedFrom(EAccessModifier.Protected)]
		public string bossGroupName = "";

		// Token: 0x04009A8B RID: 39563
		[PublicizedFrom(EAccessModifier.Protected)]
		public string bossIcon1 = "";

		// Token: 0x04009A8C RID: 39564
		[PublicizedFrom(EAccessModifier.Protected)]
		public BossGroup.BossGroupTypes bossGroupType;

		// Token: 0x04009A8D RID: 39565
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropMinionGroupName = "minion_group_name";

		// Token: 0x04009A8E RID: 39566
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBossGroupName = "boss_group_name";

		// Token: 0x04009A8F RID: 39567
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBossIcon1 = "boss_icon1";

		// Token: 0x04009A90 RID: 39568
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropGroupType = "group_type";
	}
}
