using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001933 RID: 6451
	[Preserve]
	public class RequirementInBiome : BaseRequirement
	{
		// Token: 0x0600C712 RID: 50962 RVA: 0x00491284 File Offset: 0x0048F484
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

		// Token: 0x0600C713 RID: 50963 RVA: 0x004912C2 File Offset: 0x0048F4C2
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementInBiome.PropBiome, ref this.biomes);
			if (!string.IsNullOrEmpty(this.biomes))
			{
				this.biomeList = this.biomes.Split(',', StringSplitOptions.None);
			}
		}

		// Token: 0x0600C714 RID: 50964 RVA: 0x004912FD File Offset: 0x0048F4FD
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementInBiome
			{
				Invert = this.Invert,
				biomeList = this.biomeList
			};
		}

		// Token: 0x040095EE RID: 38382
		[PublicizedFrom(EAccessModifier.Private)]
		public string biomes;

		// Token: 0x040095EF RID: 38383
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] biomeList;

		// Token: 0x040095F0 RID: 38384
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBiome = "biomes";
	}
}
