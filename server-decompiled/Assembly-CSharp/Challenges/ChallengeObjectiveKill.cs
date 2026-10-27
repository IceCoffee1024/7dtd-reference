using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001964 RID: 6500
	[Preserve]
	public class ChallengeObjectiveKill : BaseChallengeObjective
	{
		// Token: 0x17001960 RID: 6496
		// (get) Token: 0x0600C847 RID: 51271 RVA: 0x000816AE File Offset: 0x0007F8AE
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Kill;
			}
		}

		// Token: 0x17001961 RID: 6497
		// (get) Token: 0x0600C848 RID: 51272 RVA: 0x00494F88 File Offset: 0x00493188
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

		// Token: 0x0600C849 RID: 51273 RVA: 0x00495008 File Offset: 0x00493208
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

		// Token: 0x0600C84A RID: 51274 RVA: 0x00495068 File Offset: 0x00493268
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

		// Token: 0x0600C84B RID: 51275 RVA: 0x004950F7 File Offset: 0x004932F7
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.EntityKill -= this.Current_EntityKill;
		}

		// Token: 0x0600C84C RID: 51276 RVA: 0x00495110 File Offset: 0x00493310
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

		// Token: 0x0600C84D RID: 51277 RVA: 0x00495199 File Offset: 0x00493399
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			e.ParseAttribute("entity_names", ref this.entityIDs);
			e.ParseAttribute("is_enemy", ref this.isEnemy);
		}

		// Token: 0x0600C84E RID: 51278 RVA: 0x004951D0 File Offset: 0x004933D0
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

		// Token: 0x04009709 RID: 38665
		[PublicizedFrom(EAccessModifier.Private)]
		public string entityIDs = "";

		// Token: 0x0400970A RID: 38666
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] entityNames;

		// Token: 0x0400970B RID: 38667
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isEnemy = true;
	}
}
