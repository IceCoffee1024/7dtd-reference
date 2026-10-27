using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200195D RID: 6493
	[Preserve]
	public class ChallengeObjectiveEnterBiome : BaseChallengeObjective
	{
		// Token: 0x17001951 RID: 6481
		// (get) Token: 0x0600C7F0 RID: 51184 RVA: 0x00081CD4 File Offset: 0x0007FED4
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.EnterBiome;
			}
		}

		// Token: 0x17001952 RID: 6482
		// (get) Token: 0x0600C7F1 RID: 51185 RVA: 0x00493501 File Offset: 0x00491701
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveEnter", false, null) + " " + Localization.Get("biome_" + this.biome, false, null) + ":";
			}
		}

		// Token: 0x0600C7F2 RID: 51186 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C7F3 RID: 51187 RVA: 0x00493535 File Offset: 0x00491735
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.BiomeEnter += this.Current_BiomeEnter;
		}

		// Token: 0x0600C7F4 RID: 51188 RVA: 0x0049354D File Offset: 0x0049174D
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.BiomeEnter -= this.Current_BiomeEnter;
		}

		// Token: 0x0600C7F5 RID: 51189 RVA: 0x00493568 File Offset: 0x00491768
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

		// Token: 0x0600C7F6 RID: 51190 RVA: 0x004935BD File Offset: 0x004917BD
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("biome"))
			{
				this.biome = e.GetAttribute("biome");
			}
		}

		// Token: 0x0600C7F7 RID: 51191 RVA: 0x004935EE File Offset: 0x004917EE
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveEnterBiome
			{
				biome = this.biome
			};
		}

		// Token: 0x040096EB RID: 38635
		[PublicizedFrom(EAccessModifier.Private)]
		public string biome;
	}
}
