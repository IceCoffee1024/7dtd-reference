using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200190B RID: 6411
	[Preserve]
	public class ChallengeObjectiveQuestComplete : BaseChallengeObjective
	{
		// Token: 0x1700187E RID: 6270
		// (get) Token: 0x0600C5FE RID: 50686 RVA: 0x00180278 File Offset: 0x0017E478
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.QuestComplete;
			}
		}

		// Token: 0x1700187F RID: 6271
		// (get) Token: 0x0600C5FF RID: 50687 RVA: 0x0048D910 File Offset: 0x0048BB10
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

		// Token: 0x0600C600 RID: 50688 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C601 RID: 50689 RVA: 0x0048D962 File Offset: 0x0048BB62
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.QuestComplete += this.Current_QuestComplete;
		}

		// Token: 0x0600C602 RID: 50690 RVA: 0x0048D97A File Offset: 0x0048BB7A
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.QuestComplete -= this.Current_QuestComplete;
		}

		// Token: 0x0600C603 RID: 50691 RVA: 0x0048D994 File Offset: 0x0048BB94
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

		// Token: 0x0600C604 RID: 50692 RVA: 0x0048D9F8 File Offset: 0x0048BBF8
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

		// Token: 0x0600C605 RID: 50693 RVA: 0x0048DAB1 File Offset: 0x0048BCB1
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveQuestComplete
			{
				questTag = this.questTag,
				questText = this.questText,
				tier = this.tier
			};
		}

		// Token: 0x04009566 RID: 38246
		[PublicizedFrom(EAccessModifier.Private)]
		public string questTagText;

		// Token: 0x04009567 RID: 38247
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> questTag = FastTags<TagGroup.Global>.none;

		// Token: 0x04009568 RID: 38248
		[PublicizedFrom(EAccessModifier.Private)]
		public int tier;

		// Token: 0x04009569 RID: 38249
		[PublicizedFrom(EAccessModifier.Private)]
		public string questText = "";
	}
}
