using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001907 RID: 6407
	[Preserve]
	public class ChallengeObjectiveKill : BaseChallengeObjective
	{
		// Token: 0x17001876 RID: 6262
		// (get) Token: 0x0600C5DB RID: 50651 RVA: 0x000811D6 File Offset: 0x0007F3D6
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Kill;
			}
		}

		// Token: 0x17001877 RID: 6263
		// (get) Token: 0x0600C5DC RID: 50652 RVA: 0x0048D0A4 File Offset: 0x0048B2A4
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

		// Token: 0x0600C5DD RID: 50653 RVA: 0x0048D124 File Offset: 0x0048B324
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

		// Token: 0x0600C5DE RID: 50654 RVA: 0x0048D184 File Offset: 0x0048B384
		public override void HandleAddHooks()
		{
			if (!EntityFactory.EnemySpawnMode && this.isEnemy)
			{
				base.Current = this.MaxCount;
				base.Complete = true;
				this.Owner.HandleComplete(false, true);
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

		// Token: 0x0600C5DF RID: 50655 RVA: 0x0048D213 File Offset: 0x0048B413
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.EntityKill -= this.Current_EntityKill;
		}

		// Token: 0x0600C5E0 RID: 50656 RVA: 0x0048D22C File Offset: 0x0048B42C
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

		// Token: 0x0600C5E1 RID: 50657 RVA: 0x0048D2B5 File Offset: 0x0048B4B5
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			e.ParseAttribute("entity_names", ref this.entityIDs);
			e.ParseAttribute("is_enemy", ref this.isEnemy);
		}

		// Token: 0x0600C5E2 RID: 50658 RVA: 0x0048D2EC File Offset: 0x0048B4EC
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

		// Token: 0x0400955A RID: 38234
		[PublicizedFrom(EAccessModifier.Private)]
		public string entityIDs = "";

		// Token: 0x0400955B RID: 38235
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] entityNames;

		// Token: 0x0400955C RID: 38236
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isEnemy = true;
	}
}
