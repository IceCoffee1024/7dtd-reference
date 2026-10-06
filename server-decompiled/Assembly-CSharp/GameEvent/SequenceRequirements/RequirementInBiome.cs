using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200195F RID: 6495
	[Preserve]
	public class RequirementInBiome : BaseRequirement
	{
		// Token: 0x0600C86A RID: 51306 RVA: 0x00495770 File Offset: 0x00493970
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

		// Token: 0x0600C86B RID: 51307 RVA: 0x004957AE File Offset: 0x004939AE
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementInBiome.PropBiome, ref this.biomes);
			if (!string.IsNullOrEmpty(this.biomes))
			{
				this.biomeList = this.biomes.Split(',', StringSplitOptions.None);
			}
		}

		// Token: 0x0600C86C RID: 51308 RVA: 0x004957E9 File Offset: 0x004939E9
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementInBiome
			{
				Invert = this.Invert,
				biomeList = this.biomeList
			};
		}

		// Token: 0x040096B4 RID: 38580
		[PublicizedFrom(EAccessModifier.Private)]
		public string biomes;

		// Token: 0x040096B5 RID: 38581
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] biomeList;

		// Token: 0x040096B6 RID: 38582
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBiome = "biomes";
	}
}
