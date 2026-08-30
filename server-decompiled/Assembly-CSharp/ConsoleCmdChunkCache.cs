using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.Scripting;

// Token: 0x020001F6 RID: 502
[Preserve]
public class ConsoleCmdChunkCache : ConsoleCmdAbstract
{
	// Token: 0x17000150 RID: 336
	// (get) Token: 0x06000F6E RID: 3950 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000F6F RID: 3951 RVA: 0x00063EAB File Offset: 0x000620AB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "chunkcache", "cc" };
	}

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x06000F70 RID: 3952 RVA: 0x0006110A File Offset: 0x0005F30A
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x06000F71 RID: 3953 RVA: 0x00063EC4 File Offset: 0x000620C4
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		int num = 1;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int[] array = new int[1];
		ReaderWriterLockSlim syncRoot = GameManager.Instance.World.ChunkCache.GetSyncRoot();
		lock (syncRoot)
		{
			foreach (Chunk chunk in GameManager.Instance.World.ChunkCache.GetChunkArray())
			{
				int usedMem = chunk.GetUsedMem();
				int[] array2;
				chunk.GetTextureChannelMemory(out array2);
				for (int i = 0; i < array.Length; i++)
				{
					array[i] += array2[i];
					num5 += array2[i];
				}
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Concat(new string[]
				{
					num++.ToString(),
					". ",
					chunk.X.ToString(),
					", ",
					chunk.Z.ToString(),
					"  M=",
					(usedMem / 1024).ToString(),
					"k",
					chunk.IsDisplayed ? "D" : ""
				}));
				num2 += (chunk.IsDisplayed ? 1 : 0);
				num3 += usedMem;
				num4 += chunk.MeshLayerCount;
			}
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Chunks: " + GameManager.Instance.World.ChunkCache.Count().ToString());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Chunk Memory: " + (num3 / 1048576).ToString() + "MB");
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Texture Memory Total: {0:F2}MB", (float)num5 / 1048576f));
		for (int j = 0; j < array.Length; j++)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Texture Memory {0}: {1:F2}MB", j, (float)array[j] / 1048576f));
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Displayed: " + num2.ToString());
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("VML: " + num4.ToString());
	}

	// Token: 0x06000F72 RID: 3954 RVA: 0x0006417C File Offset: 0x0006237C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "shows all loaded chunks in cache";
	}
}
