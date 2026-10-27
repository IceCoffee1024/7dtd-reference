using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200062E RID: 1582
[Preserve]
public class LootEntryRequirementBiome : BaseLootEntryRequirement
{
	// Token: 0x06003361 RID: 13153 RVA: 0x0014D9AC File Offset: 0x0014BBAC
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

	// Token: 0x06003362 RID: 13154 RVA: 0x0014D9F6 File Offset: 0x0014BBF6
	public override bool CheckRequirement(EntityPlayer player)
	{
		return this.biomes.ContainsCaseInsensitive(player.biomeStandingOn.m_sBiomeName);
	}

	// Token: 0x040028EC RID: 10476
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] biomes;
}
