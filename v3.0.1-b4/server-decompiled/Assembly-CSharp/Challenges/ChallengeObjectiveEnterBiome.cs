using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018DA RID: 6362
	[Preserve]
	public class ChallengeObjectiveEnterBiome : BaseChallengeObjective
	{
		// Token: 0x170017F5 RID: 6133
		// (get) Token: 0x0600C3FD RID: 50173 RVA: 0x00081148 File Offset: 0x0007F348
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.EnterBiome;
			}
		}

		// Token: 0x170017F6 RID: 6134
		// (get) Token: 0x0600C3FE RID: 50174 RVA: 0x004852D5 File Offset: 0x004834D5
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("challengeObjectiveEnter", false, null) + " " + Localization.Get("biome_" + this.biome, false, null) + ":";
			}
		}

		// Token: 0x0600C3FF RID: 50175 RVA: 0x000027FC File Offset: 0x000009FC
		public override void Init()
		{
		}

		// Token: 0x0600C400 RID: 50176 RVA: 0x00485309 File Offset: 0x00483509
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.BiomeEnter += this.Current_BiomeEnter;
		}

		// Token: 0x0600C401 RID: 50177 RVA: 0x00485321 File Offset: 0x00483521
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.BiomeEnter -= this.Current_BiomeEnter;
		}

		// Token: 0x0600C402 RID: 50178 RVA: 0x0048533C File Offset: 0x0048353C
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

		// Token: 0x0600C403 RID: 50179 RVA: 0x00485391 File Offset: 0x00483591
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("biome"))
			{
				this.biome = e.GetAttribute("biome");
			}
		}

		// Token: 0x0600C404 RID: 50180 RVA: 0x004853C2 File Offset: 0x004835C2
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveEnterBiome
			{
				biome = this.biome
			};
		}

		// Token: 0x04009413 RID: 37907
		[PublicizedFrom(EAccessModifier.Private)]
		public string biome;
	}
}
