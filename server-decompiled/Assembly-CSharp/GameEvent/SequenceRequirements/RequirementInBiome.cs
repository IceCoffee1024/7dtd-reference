using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200190D RID: 6413
	[Preserve]
	public class RequirementInBiome : BaseRequirement
	{
		// Token: 0x0600C58B RID: 50571 RVA: 0x0048AF38 File Offset: 0x00489138
		public override bool CanPerform(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive == null)
			{
				return false;
			}
			bool flag = this.biomeList.ContainsCaseInsensitive(entityAlive.biomeStandingOn.m_sBiomeName);
			if (!this.Invert)
			{
				return flag;
			}
			return !flag;
		}

		// Token: 0x0600C58C RID: 50572 RVA: 0x0048AF76 File Offset: 0x00489176
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementInBiome.PropBiome, ref this.biomes);
			if (!string.IsNullOrEmpty(this.biomes))
			{
				this.biomeList = this.biomes.Split(',', StringSplitOptions.None);
			}
		}

		// Token: 0x0600C58D RID: 50573 RVA: 0x0048AFB1 File Offset: 0x004891B1
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementInBiome
			{
				Invert = this.Invert,
				biomeList = this.biomeList
			};
		}

		// Token: 0x040094C5 RID: 38085
		[PublicizedFrom(EAccessModifier.Private)]
		public string biomes;

		// Token: 0x040094C6 RID: 38086
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] biomeList;

		// Token: 0x040094C7 RID: 38087
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBiome = "biomes";
	}
}
