using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200192C RID: 6444
	[Preserve]
	public class ChallengeObjectiveEnterBiome : BaseChallengeObjective
	{
		// Token: 0x17001909 RID: 6409
		// (get) Token: 0x0600C6DC RID: 50908 RVA: 0x00081AB0 File Offset: 0x0007FCB0
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.EnterBiome;
			}
		}

		// Token: 0x1700190A RID: 6410
		// (get) Token: 0x0600C6DD RID: 50909 RVA: 0x0048FB0D File Offset: 0x0048DD0D
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveEnter", false, null) + " " + Localization.Get("biome_" + this.biome, false, null) + ":";
			}
		}

		// Token: 0x0600C6DE RID: 50910 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C6DF RID: 50911 RVA: 0x0048FB41 File Offset: 0x0048DD41
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.BiomeEnter += this.Current_BiomeEnter;
		}

		// Token: 0x0600C6E0 RID: 50912 RVA: 0x0048FB59 File Offset: 0x0048DD59
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.BiomeEnter -= this.Current_BiomeEnter;
		}

		// Token: 0x0600C6E1 RID: 50913 RVA: 0x0048FB74 File Offset: 0x0048DD74
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_BiomeEnter(BiomeDefinition biomeDef)
		{
			if (biomeDef != null && biomeDef.m_sBiomeName == this.biome)
			{
				int num = base.Current;
				base.Current = num + 1;
				if (base.Current >= this.MaxCount)
				{
					base.Current = this.MaxCount;
					this.CheckObjectiveComplete(true);
				}
			}
		}

		// Token: 0x0600C6E2 RID: 50914 RVA: 0x0048FBC9 File Offset: 0x0048DDC9
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("biome"))
			{
				this.biome = e.GetAttribute("biome");
			}
		}

		// Token: 0x0600C6E3 RID: 50915 RVA: 0x0048FBFA File Offset: 0x0048DDFA
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveEnterBiome
			{
				biome = this.biome
			};
		}

		// Token: 0x04009602 RID: 38402
		[PublicizedFrom(EAccessModifier.Private)]
		public string biome;
	}
}
