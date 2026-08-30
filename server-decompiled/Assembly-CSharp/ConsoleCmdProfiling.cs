using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Profiling;
using UnityEngine.Scripting;

// Token: 0x0200026E RID: 622
[Preserve]
public class ConsoleCmdProfiling : ConsoleCmdAbstract
{
	// Token: 0x170001EB RID: 491
	// (get) Token: 0x0600127B RID: 4731 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600127C RID: 4732 RVA: 0x000739CE File Offset: 0x00071BCE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "profiling" };
	}

	// Token: 0x170001EC RID: 492
	// (get) Token: 0x0600127D RID: 4733 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600127E RID: 4734 RVA: 0x000739E0 File Offset: 0x00071BE0
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

	// Token: 0x0600127F RID: 4735 RVA: 0x00073B5E File Offset: 0x00071D5E
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

	// Token: 0x06001280 RID: 4736 RVA: 0x00073B74 File Offset: 0x00071D74
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

	// Token: 0x06001281 RID: 4737 RVA: 0x00073B9A File Offset: 0x00071D9A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Enable Unity profiling for 300 frames";
	}

	// Token: 0x04000D37 RID: 3383
	[PublicizedFrom(EAccessModifier.Private)]
	public ConsoleCmdProfileNetwork cmdNetwork;

	// Token: 0x04000D38 RID: 3384
	[PublicizedFrom(EAccessModifier.Private)]
	public bool profileNetwork;

	// Token: 0x04000D39 RID: 3385
	[PublicizedFrom(EAccessModifier.Private)]
	public const int FramesDefault = 300;

	// Token: 0x04000D3A RID: 3386
	[PublicizedFrom(EAccessModifier.Private)]
	public const int FramesMin = 10;

	// Token: 0x04000D3B RID: 3387
	[PublicizedFrom(EAccessModifier.Private)]
	public const int FramesMax = 3000;
}
