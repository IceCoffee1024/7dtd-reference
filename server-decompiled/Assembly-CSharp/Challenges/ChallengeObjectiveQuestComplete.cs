using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018E5 RID: 6373
	[Preserve]
	public class ChallengeObjectiveQuestComplete : BaseChallengeObjective
	{
		// Token: 0x1700180C RID: 6156
		// (get) Token: 0x0600C477 RID: 50295 RVA: 0x0017DEA0 File Offset: 0x0017C0A0
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.QuestComplete;
			}
		}

		// Token: 0x1700180D RID: 6157
		// (get) Token: 0x0600C478 RID: 50296 RVA: 0x004875C4 File Offset: 0x004857C4
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

		// Token: 0x0600C479 RID: 50297 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C47A RID: 50298 RVA: 0x00487616 File Offset: 0x00485816
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.QuestComplete += this.Current_QuestComplete;
		}

		// Token: 0x0600C47B RID: 50299 RVA: 0x0048762E File Offset: 0x0048582E
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.QuestComplete -= this.Current_QuestComplete;
		}

		// Token: 0x0600C47C RID: 50300 RVA: 0x00487648 File Offset: 0x00485848
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

		// Token: 0x0600C47D RID: 50301 RVA: 0x004876AC File Offset: 0x004858AC
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

		// Token: 0x0600C47E RID: 50302 RVA: 0x00487765 File Offset: 0x00485965
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveQuestComplete
			{
				questTag = this.questTag,
				questText = this.questText,
				tier = this.tier
			};
		}

		// Token: 0x0400943D RID: 37949
		[PublicizedFrom(EAccessModifier.Private)]
		public string questTagText;

		// Token: 0x0400943E RID: 37950
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> questTag = FastTags<TagGroup.Global>.none;

		// Token: 0x0400943F RID: 37951
		[PublicizedFrom(EAccessModifier.Private)]
		public int tier;

		// Token: 0x04009440 RID: 37952
		[PublicizedFrom(EAccessModifier.Private)]
		public string questText = "";
	}
}
