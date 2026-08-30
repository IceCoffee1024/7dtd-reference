using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200061C RID: 1564
[Preserve]
public class LootEntryRequirementBiome : BaseLootEntryRequirement
{
	// Token: 0x060032BD RID: 12989 RVA: 0x0014A32C File Offset: 0x0014852C
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

	// Token: 0x060032BE RID: 12990 RVA: 0x0014A376 File Offset: 0x00148576
	public override bool CheckRequirement(EntityPlayer player)
	{
		return this.biomes.ContainsCaseInsensitive(player.biomeStandingOn.m_sBiomeName);
	}

	// Token: 0x04002865 RID: 10341
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] biomes;
}
