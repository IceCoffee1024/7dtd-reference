using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000627 RID: 1575
[Preserve]
public class LootEntryRequirementBiome : BaseLootEntryRequirement
{
	// Token: 0x06003353 RID: 13139 RVA: 0x0014CDB8 File Offset: 0x0014AFB8
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

	// Token: 0x06003354 RID: 13140 RVA: 0x0014CE02 File Offset: 0x0014B002
	public override bool CheckRequirement(EntityPlayer player)
	{
		return this.biomes.ContainsCaseInsensitive(player.biomeStandingOn.m_sBiomeName);
	}

	// Token: 0x040028D1 RID: 10449
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] biomes;
}
