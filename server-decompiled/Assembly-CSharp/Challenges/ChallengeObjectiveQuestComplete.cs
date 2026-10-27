using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001968 RID: 6504
	[Preserve]
	public class ChallengeObjectiveQuestComplete : BaseChallengeObjective
	{
		// Token: 0x17001968 RID: 6504
		// (get) Token: 0x0600C86A RID: 51306 RVA: 0x00270CBC File Offset: 0x0026EEBC
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.QuestComplete;
			}
		}

		// Token: 0x17001969 RID: 6505
		// (get) Token: 0x0600C86B RID: 51307 RVA: 0x004957F0 File Offset: 0x004939F0
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

		// Token: 0x0600C86C RID: 51308 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C86D RID: 51309 RVA: 0x00495842 File Offset: 0x00493A42
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.QuestComplete += this.Current_QuestComplete;
		}

		// Token: 0x0600C86E RID: 51310 RVA: 0x0049585A File Offset: 0x00493A5A
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.QuestComplete -= this.Current_QuestComplete;
		}

		// Token: 0x0600C86F RID: 51311 RVA: 0x00495874 File Offset: 0x00493A74
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

		// Token: 0x0600C870 RID: 51312 RVA: 0x004958D8 File Offset: 0x00493AD8
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

		// Token: 0x0600C871 RID: 51313 RVA: 0x00495991 File Offset: 0x00493B91
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveQuestComplete
			{
				questTag = this.questTag,
				questText = this.questText,
				tier = this.tier
			};
		}

		// Token: 0x04009715 RID: 38677
		[PublicizedFrom(EAccessModifier.Private)]
		public string questTagText;

		// Token: 0x04009716 RID: 38678
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> questTag = FastTags<TagGroup.Global>.none;

		// Token: 0x04009717 RID: 38679
		[PublicizedFrom(EAccessModifier.Private)]
		public int tier;

		// Token: 0x04009718 RID: 38680
		[PublicizedFrom(EAccessModifier.Private)]
		public string questText = "";
	}
}
