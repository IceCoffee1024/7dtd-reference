using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000295 RID: 661
[Preserve]
public class ConsoleCmdSleeper : ConsoleCmdAbstract
{
	// Token: 0x0600134D RID: 4941 RVA: 0x00075F27 File Offset: 0x00074127
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "sleeper" };
	}

	// Token: 0x0600134E RID: 4942 RVA: 0x00075F37 File Offset: 0x00074137
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Drawn or list sleeper info";
	}

	// Token: 0x0600134F RID: 4943 RVA: 0x00075F3E File Offset: 0x0007413E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "draw - toggle drawing for current player prefab\nlist - list for current player prefab\nlistall - list all\nr - reset all";
	}

	// Token: 0x06001350 RID: 4944 RVA: 0x00075F48 File Offset: 0x00074148
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

	// Token: 0x06001351 RID: 4945 RVA: 0x00076010 File Offset: 0x00074210
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

	// Token: 0x06001352 RID: 4946 RVA: 0x000760E9 File Offset: 0x000742E9
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

	// Token: 0x06001353 RID: 4947 RVA: 0x000760F8 File Offset: 0x000742F8
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

	// Token: 0x06001354 RID: 4948 RVA: 0x0007612C File Offset: 0x0007432C
	[PublicizedFrom(EAccessModifier.Private)]
	public void Print(string _s, params object[] _values)
	{
		string text = string.Format(_s, _values);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(text);
	}

	// Token: 0x04000D5E RID: 3422
	[PublicizedFrom(EAccessModifier.Private)]
	public Coroutine drawVolumesCo;
}
