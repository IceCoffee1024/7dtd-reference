using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000625 RID: 1573
[Preserve]
public class LootEntryRequirementBiome : BaseLootEntryRequirement
{
	// Token: 0x06003310 RID: 13072 RVA: 0x0014C448 File Offset: 0x0014A648
	public override void Init(XElement e)
	{
		base.Init(e);
		string text = "";
		if (e.ParseAttribute("biomes", ref text))
		{
			this.biomes = text.Split(',', StringSplitOptions.None);
			return;
		}
		this.biomes = new string[0];
	}

	// Token: 0x06003311 RID: 13073 RVA: 0x0014C492 File Offset: 0x0014A692
	public override bool CheckRequirement(EntityPlayer player)
	{
		return this.biomes.ContainsCaseInsensitive(player.biomeStandingOn.m_sBiomeName);
	}

	// Token: 0x040028D8 RID: 10456
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] biomes;
}
