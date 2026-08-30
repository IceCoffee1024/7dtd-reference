using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Challenges
{
	// Token: 0x020018ED RID: 6381
	public class ChallengeCategory
	{
		// Token: 0x0600C4CB RID: 50379 RVA: 0x004888AF File Offset: 0x00486AAF
		public ChallengeCategory(string name)
		{
			this.Name = name;
		}

		// Token: 0x17001848 RID: 6216
		// (get) Token: 0x0600C4CC RID: 50380 RVA: 0x004888D4 File Offset: 0x00486AD4
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

		// Token: 0x0600C4CD RID: 50381 RVA: 0x004888EC File Offset: 0x00486AEC
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

		// Token: 0x0600C4CE RID: 50382 RVA: 0x00488940 File Offset: 0x00486B40
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

		// Token: 0x040094B2 RID: 38066
		public static Dictionary<string, ChallengeCategory> s_ChallengeCategories = new CaseInsensitiveStringDictionary<ChallengeCategory>();

		// Token: 0x040094B3 RID: 38067
		public string Name;

		// Token: 0x040094B4 RID: 38068
		public string Icon;

		// Token: 0x040094B5 RID: 38069
		public string Title;

		// Token: 0x040094B6 RID: 38070
		public string DisplayKey = "";

		// Token: 0x040094B7 RID: 38071
		[PublicizedFrom(EAccessModifier.Private)]
		public ChallengeCategory.ShowTypes showType;

		// Token: 0x040094B8 RID: 38072
		[PublicizedFrom(EAccessModifier.Private)]
		public string showValue = "";

		// Token: 0x020018EE RID: 6382
		public enum ShowTypes
		{
			// Token: 0x040094BA RID: 38074
			Normal,
			// Token: 0x040094BB RID: 38075
			CVar,
			// Token: 0x040094BC RID: 38076
			Twitch,
			// Token: 0x040094BD RID: 38077
			BiomeProgression
		}
	}
}
