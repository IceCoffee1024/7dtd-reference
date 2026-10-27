using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000280 RID: 640
[Preserve]
public class ConsoleCmdSaveChunkAgeMap : ConsoleCmdAbstract
{
	// Token: 0x060012C2 RID: 4802 RVA: 0x00074FE5 File Offset: 0x000731E5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "agemap" };
	}

	// Token: 0x060012C3 RID: 4803 RVA: 0x00074FF5 File Offset: 0x000731F5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Output debug map for chunk age/protection/save status.";
	}

	// Token: 0x060012C4 RID: 4804 RVA: 0x00074FFC File Offset: 0x000731FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "\"agemap\" or \"agemap [x]\", where [x] is a float value specifying the maximum age to normalise results to in in-game days. Defaults to EnumGamePrefs.MaxChunkAge when not specified.\n\tOutputs a TGA texture representing a map of all chunks with chunk age, protection status and save status data split across each colour channel:\n\tR [scalar]: Effective chunk age proportionate to maximum age, taking POI-based grouping rules into account. More red = older, closer to expiry.\n\tG [scalar]: Raw chunk age proportionate to maximum age, ignoring POI-based grouping. Can be compared with the red channel to asses the impacts of grouping.\n\tB [scalar]: Protection level. More blue = more protected.\n\tA [binary]: Saved status. Opaque = saved, transparent = not saved.";
	}

	// Token: 0x060012C5 RID: 4805 RVA: 0x00075004 File Offset: 0x00073204
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		ChunkProviderGenerateWorld chunkProviderGenerateWorld = GameManager.Instance.World.ChunkCache.ChunkProvider as ChunkProviderGenerateWorld;
		if (chunkProviderGenerateWorld == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Failed to create chunk age map: ChunkProviderGenerateWorld could not be found for current world instance.");
			return;
		}
		float num;
		if (_params.Count == 0)
		{
			num = (float)GamePrefs.GetInt(EnumGamePrefs.MaxChunkAge);
		}
		else if (_params.Count != 1 || !float.TryParse(_params[0], out num) || num < 1E-45f)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(this.GetHelp());
			return;
		}
		try
		{
			chunkProviderGenerateWorld.SaveChunkAgeDebugTexture(num);
		}
		catch (Exception ex)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Failed to create chunk age map with exception: " + ex.Message);
		}
	}
}
