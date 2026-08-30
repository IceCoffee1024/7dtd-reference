using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002B1 RID: 689
[Preserve]
public class ConsoleCmdTrees : ConsoleCmdAbstract
{
	// Token: 0x060013E9 RID: 5097 RVA: 0x000793A5 File Offset: 0x000775A5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "trees" };
	}

	// Token: 0x060013EA RID: 5098 RVA: 0x000793B5 File Offset: 0x000775B5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Switches trees on/off";
	}

	// Token: 0x060013EB RID: 5099 RVA: 0x000793BC File Offset: 0x000775BC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "trees - toggles\ntrees <value> - (off, on)";
	}

	// Token: 0x060013EC RID: 5100 RVA: 0x000793C4 File Offset: 0x000775C4
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 0)
		{
			DecoManager.Instance.IsHidden = !DecoManager.Instance.IsHidden;
		}
		else if (_params[0] == "on")
		{
			DecoManager.Instance.IsHidden = false;
		}
		else if (_params[0] == "off")
		{
			DecoManager.Instance.IsHidden = true;
		}
		else
		{
			int num;
			if (int.TryParse(_params[0], out num))
			{
				DecoManager.Instance.SetChunkDistance(num);
				DecoManager.Instance.OnWorldUnloaded();
				IChunkProvider chunkProvider = GameManager.Instance.World.ChunkCache.ChunkProvider;
				ThreadManager.RunCoroutineSync(DecoManager.Instance.OnWorldLoaded(chunkProvider.GetWorldSize().x, chunkProvider.GetWorldSize().y, GameManager.Instance.World, chunkProvider));
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Concat(new string[]
				{
					"Setting to deco chunk distance ",
					num.ToString(),
					" =",
					(128 * num).ToString(),
					"m"
				}));
				return;
			}
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Unknown parameter");
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Trees set to " + ((!DecoManager.Instance.IsHidden) ? "on" : "off"));
	}
}
