using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001990 RID: 6544
	[Preserve]
	public class RequirementInBiome : BaseRequirement
	{
		// Token: 0x0600C97E RID: 51582 RVA: 0x00499164 File Offset: 0x00497364
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

		// Token: 0x0600C97F RID: 51583 RVA: 0x004991A2 File Offset: 0x004973A2
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementInBiome.PropBiome, ref this.biomes);
			if (!string.IsNullOrEmpty(this.biomes))
			{
				this.biomeList = this.biomes.Split(',', StringSplitOptions.None);
			}
		}

		// Token: 0x0600C980 RID: 51584 RVA: 0x004991DD File Offset: 0x004973DD
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementInBiome
			{
				Invert = this.Invert,
				biomeList = this.biomeList
			};
		}

		// Token: 0x0400979D RID: 38813
		[PublicizedFrom(EAccessModifier.Private)]
		public string biomes;

		// Token: 0x0400979E RID: 38814
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] biomeList;

		// Token: 0x0400979F RID: 38815
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBiome = "biomes";
	}
}
