using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Challenges
{
	// Token: 0x0200194A RID: 6474
	public class ChallengeCategory
	{
		// Token: 0x0600C737 RID: 50999 RVA: 0x00490793 File Offset: 0x0048E993
		public ChallengeCategory(string name)
		{
			this.Name = name;
		}

		// Token: 0x17001932 RID: 6450
		// (get) Token: 0x0600C738 RID: 51000 RVA: 0x004907B8 File Offset: 0x0048E9B8
		public bool IsActive
		{
			get
			{
				if (this.showType == ChallengeCategory.ShowTypes.BiomeProgression)
				{
					return World.BiomeProgressionEnabled;
				}
				return ChallengeJournal.AllowChallenges;
			}
		}

		// Token: 0x0600C739 RID: 51001 RVA: 0x004907D0 File Offset: 0x0048E9D0
		public bool CanShow(EntityPlayer player)
		{
			if (this.showType == ChallengeCategory.ShowTypes.Twitch)
			{
				return player.TwitchEnabled;
			}
			if (this.showType == ChallengeCategory.ShowTypes.CVar)
			{
				return player.Buffs.GetCustomVar(this.showValue) > 0f;
			}
			if (this.showType == ChallengeCategory.ShowTypes.BiomeProgression)
			{
				return World.BiomeProgressionEnabled;
			}
			return ChallengeJournal.AllowChallenges;
		}

		// Token: 0x0600C73A RID: 51002 RVA: 0x00490824 File Offset: 0x0048EA24
		public void ParseElement(XElement e)
		{
			if (e.HasAttribute("title_key"))
			{
				this.Title = Localization.Get(e.GetAttribute("title_key"), false, null);
			}
			else if (e.HasAttribute("title"))
			{
				this.Title = e.GetAttribute("title");
			}
			else
			{
				this.Title = this.Name;
			}
			if (e.HasAttribute("icon"))
			{
				this.Icon = e.GetAttribute("icon");
			}
			if (e.HasAttribute("show_type"))
			{
				this.showType = (ChallengeCategory.ShowTypes)Enum.Parse(typeof(ChallengeCategory.ShowTypes), e.GetAttribute("show_type"), true);
			}
			if (e.HasAttribute("show_value"))
			{
				this.showValue = e.GetAttribute("show_value");
			}
			if (e.HasAttribute("display_key"))
			{
				this.DisplayKey = e.GetAttribute("display_key");
			}
		}

		// Token: 0x04009661 RID: 38497
		public static Dictionary<string, ChallengeCategory> s_ChallengeCategories = new CaseInsensitiveStringDictionary<ChallengeCategory>();

		// Token: 0x04009662 RID: 38498
		public string Name;

		// Token: 0x04009663 RID: 38499
		public string Icon;

		// Token: 0x04009664 RID: 38500
		public string Title;

		// Token: 0x04009665 RID: 38501
		public string DisplayKey = "";

		// Token: 0x04009666 RID: 38502
		[PublicizedFrom(EAccessModifier.Private)]
		public ChallengeCategory.ShowTypes showType;

		// Token: 0x04009667 RID: 38503
		[PublicizedFrom(EAccessModifier.Private)]
		public string showValue = "";

		// Token: 0x0200194B RID: 6475
		public enum ShowTypes
		{
			// Token: 0x04009669 RID: 38505
			Normal,
			// Token: 0x0400966A RID: 38506
			CVar,
			// Token: 0x0400966B RID: 38507
			Twitch,
			// Token: 0x0400966C RID: 38508
			BiomeProgression
		}
	}
}
