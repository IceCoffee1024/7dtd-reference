using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001933 RID: 6451
	[Preserve]
	public class ChallengeObjectiveKill : BaseChallengeObjective
	{
		// Token: 0x17001918 RID: 6424
		// (get) Token: 0x0600C733 RID: 50995 RVA: 0x0008148A File Offset: 0x0007F68A
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Kill;
			}
		}

		// Token: 0x17001919 RID: 6425
		// (get) Token: 0x0600C734 RID: 50996 RVA: 0x00491594 File Offset: 0x0048F794
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

		// Token: 0x0600C735 RID: 50997 RVA: 0x00491614 File Offset: 0x0048F814
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

		// Token: 0x0600C736 RID: 50998 RVA: 0x00491674 File Offset: 0x0048F874
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

		// Token: 0x0600C737 RID: 50999 RVA: 0x00491703 File Offset: 0x0048F903
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.EntityKill -= this.Current_EntityKill;
		}

		// Token: 0x0600C738 RID: 51000 RVA: 0x0049171C File Offset: 0x0048F91C
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

		// Token: 0x0600C739 RID: 51001 RVA: 0x004917A5 File Offset: 0x0048F9A5
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			e.ParseAttribute("entity_names", ref this.entityIDs);
			e.ParseAttribute("is_enemy", ref this.isEnemy);
		}

		// Token: 0x0600C73A RID: 51002 RVA: 0x004917DC File Offset: 0x0048F9DC
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

		// Token: 0x04009620 RID: 38432
		[PublicizedFrom(EAccessModifier.Private)]
		public string entityIDs = "";

		// Token: 0x04009621 RID: 38433
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] entityNames;

		// Token: 0x04009622 RID: 38434
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isEnemy = true;
	}
}
