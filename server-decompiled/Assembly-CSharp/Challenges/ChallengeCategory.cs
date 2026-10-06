using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Challenges
{
	// Token: 0x02001919 RID: 6425
	public class ChallengeCategory
	{
		// Token: 0x0600C623 RID: 50723 RVA: 0x0048CD9F File Offset: 0x0048AF9F
		public ChallengeCategory(string name)
		{
			this.Name = name;
		}

		// Token: 0x170018EA RID: 6378
		// (get) Token: 0x0600C624 RID: 50724 RVA: 0x0048CDC4 File Offset: 0x0048AFC4
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

		// Token: 0x0600C625 RID: 50725 RVA: 0x0048CDDC File Offset: 0x0048AFDC
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

		// Token: 0x0600C626 RID: 50726 RVA: 0x0048CE30 File Offset: 0x0048B030
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

		// Token: 0x04009578 RID: 38264
		public static Dictionary<string, ChallengeCategory> s_ChallengeCategories = new CaseInsensitiveStringDictionary<ChallengeCategory>();

		// Token: 0x04009579 RID: 38265
		public string Name;

		// Token: 0x0400957A RID: 38266
		public string Icon;

		// Token: 0x0400957B RID: 38267
		public string Title;

		// Token: 0x0400957C RID: 38268
		public string DisplayKey = "";

		// Token: 0x0400957D RID: 38269
		[PublicizedFrom(EAccessModifier.Private)]
		public ChallengeCategory.ShowTypes showType;

		// Token: 0x0400957E RID: 38270
		[PublicizedFrom(EAccessModifier.Private)]
		public string showValue = "";

		// Token: 0x0200191A RID: 6426
		public enum ShowTypes
		{
			// Token: 0x04009580 RID: 38272
			Normal,
			// Token: 0x04009581 RID: 38273
			CVar,
			// Token: 0x04009582 RID: 38274
			Twitch,
			// Token: 0x04009583 RID: 38275
			BiomeProgression
		}
	}
}
