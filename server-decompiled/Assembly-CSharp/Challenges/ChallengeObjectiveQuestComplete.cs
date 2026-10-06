using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001937 RID: 6455
	[Preserve]
	public class ChallengeObjectiveQuestComplete : BaseChallengeObjective
	{
		// Token: 0x17001920 RID: 6432
		// (get) Token: 0x0600C756 RID: 51030 RVA: 0x0026F794 File Offset: 0x0026D994
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.QuestComplete;
			}
		}

		// Token: 0x17001921 RID: 6433
		// (get) Token: 0x0600C757 RID: 51031 RVA: 0x00491DFC File Offset: 0x0048FFFC
		public override string DescriptionText
		{
			get
			{
				if (this.questText == "")
				{
					this.questText = Localization.Get("challengeTargetAnyQuest", false, null);
				}
				return this.questText + " " + Localization.Get("challengeObjectiveQuestCompleted", false, null) + ":";
			}
		}

		// Token: 0x0600C758 RID: 51032 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C759 RID: 51033 RVA: 0x00491E4E File Offset: 0x0049004E
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.QuestComplete += this.Current_QuestComplete;
		}

		// Token: 0x0600C75A RID: 51034 RVA: 0x00491E66 File Offset: 0x00490066
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.QuestComplete -= this.Current_QuestComplete;
		}

		// Token: 0x0600C75B RID: 51035 RVA: 0x00491E80 File Offset: 0x00490080
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_QuestComplete(FastTags<TagGroup.Global> questTags, QuestClass questClass)
		{
			if (this.questTag.IsEmpty || questTags.Test_AnySet(this.questTag))
			{
				if (this.CheckBaseRequirements())
				{
					return;
				}
				int num = base.Current;
				base.Current = num + 1;
				if (base.Current >= this.MaxCount)
				{
					base.Current = this.MaxCount;
					this.CheckObjectiveComplete(true);
				}
			}
		}

		// Token: 0x0600C75C RID: 51036 RVA: 0x00491EE4 File Offset: 0x004900E4
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("quest_tag"))
			{
				this.questTagText = e.GetAttribute("quest_tag");
				this.questTag = FastTags<TagGroup.Global>.Parse(this.questTagText);
			}
			else
			{
				this.questTag = FastTags<TagGroup.Global>.none;
			}
			if (e.HasAttribute("quest_text_key"))
			{
				this.questText = Localization.Get(e.GetAttribute("quest_text_key"), false, null);
			}
			if (e.HasAttribute("tier"))
			{
				this.tier = StringParsers.ParseSInt32(e.GetAttribute("tier"), 0, -1, NumberStyles.Integer);
			}
		}

		// Token: 0x0600C75D RID: 51037 RVA: 0x00491F9D File Offset: 0x0049019D
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveQuestComplete
			{
				questTag = this.questTag,
				questText = this.questText,
				tier = this.tier
			};
		}

		// Token: 0x0400962C RID: 38444
		[PublicizedFrom(EAccessModifier.Private)]
		public string questTagText;

		// Token: 0x0400962D RID: 38445
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> questTag = FastTags<TagGroup.Global>.none;

		// Token: 0x0400962E RID: 38446
		[PublicizedFrom(EAccessModifier.Private)]
		public int tier;

		// Token: 0x0400962F RID: 38447
		[PublicizedFrom(EAccessModifier.Private)]
		public string questText = "";
	}
}
