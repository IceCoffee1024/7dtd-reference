using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Challenges
{
	// Token: 0x020018C7 RID: 6343
	public class ChallengeCategory
	{
		// Token: 0x0600C344 RID: 49988 RVA: 0x0048256F File Offset: 0x0048076F
		public ChallengeCategory(string name)
		{
			this.Name = name;
		}

		// Token: 0x170017D6 RID: 6102
		// (get) Token: 0x0600C345 RID: 49989 RVA: 0x00482594 File Offset: 0x00480794
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

		// Token: 0x0600C346 RID: 49990 RVA: 0x004825AC File Offset: 0x004807AC
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

		// Token: 0x0600C347 RID: 49991 RVA: 0x00482600 File Offset: 0x00480800
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

		// Token: 0x04009389 RID: 37769
		public static Dictionary<string, ChallengeCategory> s_ChallengeCategories = new CaseInsensitiveStringDictionary<ChallengeCategory>();

		// Token: 0x0400938A RID: 37770
		public string Name;

		// Token: 0x0400938B RID: 37771
		public string Icon;

		// Token: 0x0400938C RID: 37772
		public string Title;

		// Token: 0x0400938D RID: 37773
		public string DisplayKey = "";

		// Token: 0x0400938E RID: 37774
		[PublicizedFrom(EAccessModifier.Private)]
		public ChallengeCategory.ShowTypes showType;

		// Token: 0x0400938F RID: 37775
		[PublicizedFrom(EAccessModifier.Private)]
		public string showValue = "";

		// Token: 0x020018C8 RID: 6344
		public enum ShowTypes
		{
			// Token: 0x04009391 RID: 37777
			Normal,
			// Token: 0x04009392 RID: 37778
			CVar,
			// Token: 0x04009393 RID: 37779
			Twitch,
			// Token: 0x04009394 RID: 37780
			BiomeProgression
		}
	}
}
