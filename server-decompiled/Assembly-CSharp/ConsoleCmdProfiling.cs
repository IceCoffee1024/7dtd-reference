using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Profiling;
using UnityEngine.Scripting;

// Token: 0x02000275 RID: 629
[Preserve]
public class ConsoleCmdProfiling : ConsoleCmdAbstract
{
	// Token: 0x170001E4 RID: 484
	// (get) Token: 0x0600128A RID: 4746 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600128B RID: 4747 RVA: 0x000742DE File Offset: 0x000724DE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "profiling" };
	}

	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x0600128C RID: 4748 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600128D RID: 4749 RVA: 0x000742F0 File Offset: 0x000724F0
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (this.cmdNetwork == null)
		{
			this.cmdNetwork = SingletonMonoBehaviour<SdtdConsole>.Instance.GetCommand("profilenetwork", true) as ConsoleCmdProfileNetwork;
		}
		if (_params.Count != 1 || !_params[0].EqualsCaseInsensitive("stop"))
		{
			if (!Profiler.enabled)
			{
				this.profileNetwork = true;
				int num = 300;
				if (_params.Count > 0 && !int.TryParse(_params[0], out num))
				{
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Not a valid integer for number of frames (\"{0}\")", num));
					return;
				}
				if (num < 10 || num > 3000)
				{
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Number of frames needs to be within {0} and {1}", 10, 3000));
					return;
				}
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Enabled profiling for {0} frames (typically 5 - 10 seconds)", num));
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Profiler mem: {0}", Profiler.maxUsedMemory));
				Profiler.logFile = string.Format("{0}/profiling_{1:yyyy-MM-dd_HH-mm-ss}_unity.log", GameIO.GetApplicationPath(), DateTime.Now);
				Profiler.enableBinaryLog = true;
				Profiler.enabled = true;
				ThreadManager.StartCoroutine(this.stopProfilingLater(num));
				if (this.profileNetwork)
				{
					ConsoleCmdProfileNetwork consoleCmdProfileNetwork = this.cmdNetwork;
					if (consoleCmdProfileNetwork == null)
					{
						return;
					}
					consoleCmdProfileNetwork.resetData();
				}
			}
			return;
		}
		if (!Profiler.enabled)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Profiling not running.");
			return;
		}
		this.stopProfiling();
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Profiling stopped.");
	}

	// Token: 0x0600128E RID: 4750 RVA: 0x0007446E File Offset: 0x0007266E
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator stopProfilingLater(int _frames)
	{
		int i = 0;
		while (i < _frames && Profiler.enabled)
		{
			yield return null;
			int num = i;
			i = num + 1;
		}
		if (Profiler.enabled)
		{
			this.stopProfiling();
			Log.Out("Profiling done");
		}
		yield break;
	}

	// Token: 0x0600128F RID: 4751 RVA: 0x00074484 File Offset: 0x00072684
	[PublicizedFrom(EAccessModifier.Private)]
	public void stopProfiling()
	{
		Profiler.enabled = false;
		Profiler.logFile = null;
		if (this.profileNetwork)
		{
			ConsoleCmdProfileNetwork consoleCmdProfileNetwork = this.cmdNetwork;
			if (consoleCmdProfileNetwork == null)
			{
				return;
			}
			consoleCmdProfileNetwork.doProfileNetwork();
		}
	}

	// Token: 0x06001290 RID: 4752 RVA: 0x000744AA File Offset: 0x000726AA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Enable Unity profiling for 300 frames";
	}

	// Token: 0x04000D41 RID: 3393
	[PublicizedFrom(EAccessModifier.Private)]
	public ConsoleCmdProfileNetwork cmdNetwork;

	// Token: 0x04000D42 RID: 3394
	[PublicizedFrom(EAccessModifier.Private)]
	public bool profileNetwork;

	// Token: 0x04000D43 RID: 3395
	[PublicizedFrom(EAccessModifier.Private)]
	public const int FramesDefault = 300;

	// Token: 0x04000D44 RID: 3396
	[PublicizedFrom(EAccessModifier.Private)]
	public const int FramesMin = 10;

	// Token: 0x04000D45 RID: 3397
	[PublicizedFrom(EAccessModifier.Private)]
	public const int FramesMax = 3000;
}
