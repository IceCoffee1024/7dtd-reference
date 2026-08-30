using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018E1 RID: 6369
	[Preserve]
	public class ChallengeObjectiveKill : BaseChallengeObjective
	{
		// Token: 0x17001804 RID: 6148
		// (get) Token: 0x0600C454 RID: 50260 RVA: 0x00080B22 File Offset: 0x0007ED22
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Kill;
			}
		}

		// Token: 0x17001805 RID: 6149
		// (get) Token: 0x0600C455 RID: 50261 RVA: 0x00486D58 File Offset: 0x00484F58
		public override string DescriptionText
		{
			get
			{
				if (this.Biome == "")
				{
					return Localization.Get("challengeObjectiveKill", false, null) + " " + Localization.Get(this.entityIDs, false, null) + ":";
				}
				return string.Format(Localization.Get("challengeObjectiveKillIn", false, null), Localization.Get(this.entityIDs, false, null), Localization.Get("biome_" + this.Biome, false, null));
			}
		}

		// Token: 0x0600C456 RID: 50262 RVA: 0x00486DD8 File Offset: 0x00484FD8
		public override void Init()
		{
			if (this.entityIDs != null)
			{
				string[] array = this.entityIDs.Split(',', StringSplitOptions.None);
				if (array.Length > 1)
				{
					this.entityIDs = array[0];
					this.entityNames = new string[array.Length - 1];
					for (int i = 1; i < array.Length; i++)
					{
						this.entityNames[i - 1] = array[i];
					}
				}
			}
		}

		// Token: 0x0600C457 RID: 50263 RVA: 0x00486E38 File Offset: 0x00485038
		public override void HandleAddHooks()
		{
			if (!EntityFactory.EnemySpawnMode && this.isEnemy)
			{
				base.Current = this.MaxCount;
				base.Complete = true;
				this.Owner.HandleComplete(false);
				if (this.Owner.ChallengeState == Challenge.ChallengeStates.Completed)
				{
					this.Owner.AutoCompleted = true;
					this.Owner.ChallengeState = Challenge.ChallengeStates.Redeemed;
					return;
				}
			}
			else
			{
				QuestEventManager.Current.EntityKill -= this.Current_EntityKill;
				QuestEventManager.Current.EntityKill += this.Current_EntityKill;
			}
		}

		// Token: 0x0600C458 RID: 50264 RVA: 0x00486EC6 File Offset: 0x004850C6
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.EntityKill -= this.Current_EntityKill;
		}

		// Token: 0x0600C459 RID: 50265 RVA: 0x00486EE0 File Offset: 0x004850E0
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_EntityKill(EntityAlive killedBy, EntityAlive killedEntity)
		{
			string entityClassName = killedEntity.EntityClass.entityClassName;
			bool flag = false;
			if (this.CheckBaseRequirements())
			{
				return;
			}
			if (this.entityIDs == null || entityClassName.EqualsCaseInsensitive(this.entityIDs))
			{
				flag = true;
			}
			if (!flag && this.entityNames != null)
			{
				for (int i = 0; i < this.entityNames.Length; i++)
				{
					if (this.entityNames[i].EqualsCaseInsensitive(entityClassName))
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				int num = base.Current;
				base.Current = num + 1;
				this.CheckObjectiveComplete(true);
			}
		}

		// Token: 0x0600C45A RID: 50266 RVA: 0x00486F69 File Offset: 0x00485169
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			e.ParseAttribute("entity_names", ref this.entityIDs);
			e.ParseAttribute("is_enemy", ref this.isEnemy);
		}

		// Token: 0x0600C45B RID: 50267 RVA: 0x00486FA0 File Offset: 0x004851A0
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveKill
			{
				entityIDs = this.entityIDs,
				entityNames = this.entityNames,
				isEnemy = this.isEnemy,
				Biome = this.Biome
			};
		}

		// Token: 0x04009431 RID: 37937
		[PublicizedFrom(EAccessModifier.Private)]
		public string entityIDs = "";

		// Token: 0x04009432 RID: 37938
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] entityNames;

		// Token: 0x04009433 RID: 37939
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isEnemy = true;
	}
}
