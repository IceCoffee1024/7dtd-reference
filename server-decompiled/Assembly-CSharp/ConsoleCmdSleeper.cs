using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200029E RID: 670
[Preserve]
public class ConsoleCmdSleeper : ConsoleCmdAbstract
{
	// Token: 0x06001360 RID: 4960 RVA: 0x00076A07 File Offset: 0x00074C07
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "sleeper" };
	}

	// Token: 0x06001361 RID: 4961 RVA: 0x00076A17 File Offset: 0x00074C17
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Drawn or list sleeper info";
	}

	// Token: 0x06001362 RID: 4962 RVA: 0x00076A1E File Offset: 0x00074C1E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "draw - toggle drawing for current player prefab\nlist - list for current player prefab\nlistall - list all\nr - reset all";
	}

	// Token: 0x06001363 RID: 4963 RVA: 0x00076A28 File Offset: 0x00074C28
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(this.GetHelp());
			return;
		}
		string text = _params[0].ToLower();
		if (!(text == "draw"))
		{
			if (text == "listall")
			{
				this.LogInfo(false);
				return;
			}
			if (text == "list")
			{
				this.LogInfo(true);
				return;
			}
			if (!(text == "r"))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Command not recognized. <end/>");
				return;
			}
			this.Reset();
			return;
		}
		else
		{
			if (this.drawVolumesCo != null)
			{
				GameManager.Instance.StopCoroutine(this.drawVolumesCo);
				this.drawVolumesCo = null;
				return;
			}
			this.drawVolumesCo = GameManager.Instance.StartCoroutine(this.DrawVolumes());
			return;
		}
	}

	// Token: 0x06001364 RID: 4964 RVA: 0x00076AF0 File Offset: 0x00074CF0
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogInfo(bool onlyPlayer)
	{
		World world = GameManager.Instance.World;
		if (world == null)
		{
			return;
		}
		EntityPlayerLocal entityPlayerLocal = (onlyPlayer ? world.GetPrimaryPlayer() : null);
		List<ValueTuple<int, SleeperVolume>> list = new List<ValueTuple<int, SleeperVolume>>();
		world.GetAllSleeperVolumes(list);
		int count = list.Count;
		int num = 0;
		int i = 0;
		while (i < count)
		{
			ValueTuple<int, SleeperVolume> valueTuple = list[i];
			int item = valueTuple.Item1;
			SleeperVolume item2 = valueTuple.Item2;
			if (!entityPlayerLocal)
			{
				goto IL_0072;
			}
			if (item2.PrefabInstance == entityPlayerLocal.prefab)
			{
				item2.Draw(3f);
				goto IL_0072;
			}
			IL_009D:
			i++;
			continue;
			IL_0072:
			num++;
			this.Print("#{0} {1}", new object[]
			{
				item,
				item2.GetDescription()
			});
			goto IL_009D;
		}
		this.Print("Sleeper volumes {0} of {1}", new object[] { num, count });
	}

	// Token: 0x06001365 RID: 4965 RVA: 0x00076BC9 File Offset: 0x00074DC9
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator DrawVolumes()
	{
		int num;
		for (int i = 0; i < 99999; i = num)
		{
			World world = GameManager.Instance.World;
			if (world == null)
			{
				break;
			}
			EntityPlayerLocal primaryPlayer = world.GetPrimaryPlayer();
			if (!primaryPlayer || primaryPlayer.prefab == null)
			{
				break;
			}
			foreach (SleeperVolume sleeperVolume in primaryPlayer.prefab.sleeperVolumes)
			{
				sleeperVolume.DrawDebugLines(1f);
			}
			foreach (TriggerVolume triggerVolume in primaryPlayer.prefab.triggerVolumes)
			{
				triggerVolume.DrawDebugLines(1f);
			}
			yield return new WaitForSeconds(0.5f);
			num = i + 1;
		}
		this.drawVolumesCo = null;
		yield break;
	}

	// Token: 0x06001366 RID: 4966 RVA: 0x00076BD8 File Offset: 0x00074DD8
	[PublicizedFrom(EAccessModifier.Private)]
	public void Reset()
	{
		World world = GameManager.Instance.World;
		if (world == null)
		{
			return;
		}
		world.ResetSleeperVolumes();
		this.Print("Reset all sleeper volumes", Array.Empty<object>());
	}

	// Token: 0x06001367 RID: 4967 RVA: 0x00076C0C File Offset: 0x00074E0C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Print(string _s, params object[] _values)
	{
		string text = string.Format(_s, _values);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(text);
	}

	// Token: 0x04000D69 RID: 3433
	[PublicizedFrom(EAccessModifier.Private)]
	public Coroutine drawVolumesCo;
}
