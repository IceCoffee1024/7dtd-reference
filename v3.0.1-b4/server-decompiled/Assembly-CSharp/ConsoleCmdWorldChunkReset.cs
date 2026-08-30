using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000271 RID: 625
[Preserve]
public class ConsoleCmdWorldChunkReset : ConsoleCmdAbstract
{
	// Token: 0x0600128E RID: 4750 RVA: 0x00073D17 File Offset: 0x00071F17
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "worldchunkreset", "wcr" };
	}

	// Token: 0x0600128F RID: 4751 RVA: 0x00073D2F File Offset: 0x00071F2F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Resets all unprotected chunks across the world.";
	}

	// Token: 0x06001290 RID: 4752 RVA: 0x00073D36 File Offset: 0x00071F36
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage: wcr [-g|-gmode <n>]\n\nExamples: \n'wcr' - reset all unprotected chunks using the default grouping.\n'wcr -g 2' - reset all unprotected chunks, grouping mode 2.\n\nGrouping modes (-g|-gmode, default 3): \n'1' - NoGrouping: Chunks are reset based on their own protection flags only.\n'2' - SeparatePOIs: Each POI gets the combined protection flags from all chunks overlapping the POI.\n'3' - GroupedPOIs: Like SeparatePOIs, but POIs with overlapping chunks also merge into groups. Standard reset; should not cause POI discontinuities.\n\nNotes: \n - All chunk protection statuses are respected. To bypass protections, see 'rr'.\n - Use with caution! This operation permanently deletes all saved data for affected chunks.";
	}

	// Token: 0x06001291 RID: 4753 RVA: 0x00073D40 File Offset: 0x00071F40
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		EnumResetUnprotectedChunksGroupingMode enumResetUnprotectedChunksGroupingMode;
		if (!ChunkResetCommandHelpers.TryParseGroupingMode(_params, out enumResetUnprotectedChunksGroupingMode))
		{
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Running world chunk reset: -g {0} ({1}).", (int)enumResetUnprotectedChunksGroupingMode, enumResetUnprotectedChunksGroupingMode));
		ChunkResetCommandHelpers.ExecuteReset(ChunkProtectionLevel.All, enumResetUnprotectedChunksGroupingMode, null, null, "World chunk reset");
	}
}
