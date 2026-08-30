using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x02001900 RID: 6400
	[Preserve]
	public class ChallengeObjectiveEnterBiome : BaseChallengeObjective
	{
		// Token: 0x17001867 RID: 6247
		// (get) Token: 0x0600C584 RID: 50564 RVA: 0x000817FC File Offset: 0x0007F9FC
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.EnterBiome;
			}
		}

		// Token: 0x17001868 RID: 6248
		// (get) Token: 0x0600C585 RID: 50565 RVA: 0x0048B61D File Offset: 0x0048981D
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveEnter", false, null) + " " + Localization.Get("biome_" + this.biome, false, null) + ":";
			}
		}

		// Token: 0x0600C586 RID: 50566 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C587 RID: 50567 RVA: 0x0048B651 File Offset: 0x00489851
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.BiomeEnter += this.Current_BiomeEnter;
		}

		// Token: 0x0600C588 RID: 50568 RVA: 0x0048B669 File Offset: 0x00489869
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.BiomeEnter -= this.Current_BiomeEnter;
		}

		// Token: 0x0600C589 RID: 50569 RVA: 0x0048B684 File Offset: 0x00489884
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

		// Token: 0x0600C58A RID: 50570 RVA: 0x0048B6D9 File Offset: 0x004898D9
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("biome"))
			{
				this.biome = e.GetAttribute("biome");
			}
		}

		// Token: 0x0600C58B RID: 50571 RVA: 0x0048B70A File Offset: 0x0048990A
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveEnterBiome
			{
				biome = this.biome
			};
		}

		// Token: 0x0400953C RID: 38204
		[PublicizedFrom(EAccessModifier.Private)]
		public string biome;
	}
}
