using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020002B9 RID: 697
[Preserve]
public class ConsoleCmdVisitPois : ConsoleCmdAbstract
{
	// Token: 0x17000238 RID: 568
	// (get) Token: 0x06001425 RID: 5157 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001426 RID: 5158 RVA: 0x0007A0FF File Offset: 0x000782FF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "vpois", "visitpois" };
	}

	// Token: 0x06001427 RID: 5159 RVA: 0x00031E8B File Offset: 0x0003008B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "";
	}

	// Token: 0x06001428 RID: 5160 RVA: 0x0007A117 File Offset: 0x00078317
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "<s[tart] [pois per auto-pause]|p[ause]|r[eset]>";
	}

	// Token: 0x06001429 RID: 5161 RVA: 0x0007A11E File Offset: 0x0007831E
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (!this.ExecuteInternal(_params, _senderInfo))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(this.GetHelp());
		}
	}

	// Token: 0x0600142A RID: 5162 RVA: 0x0007A13C File Offset: 0x0007833C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ExecuteInternal(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (!GameManager.Instance.World.GetPrimaryPlayer())
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No local player! (Are you in-game?)");
			return true;
		}
		if (_params.Count == 0)
		{
			return false;
		}
		string text = _params[0].ToLowerInvariant();
		if (!(text == "s") && !(text == "start"))
		{
			if (text == "p" || text == "pause")
			{
				this.MacroPause();
				return true;
			}
			if (!(text == "r") && !(text == "reset"))
			{
				return false;
			}
			this.MacroReset();
			return true;
		}
		else
		{
			if (_params.Count > 2)
			{
				return false;
			}
			int num = 0;
			if (_params.Count > 1 && !int.TryParse(_params[1], out num))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Failed to parse as int " + _params[1]);
				return false;
			}
			this.MacroStart(num);
			return true;
		}
	}

	// Token: 0x0600142B RID: 5163 RVA: 0x0007A234 File Offset: 0x00078434
	[PublicizedFrom(EAccessModifier.Private)]
	public void MacroStart(int _poisPerAutoPause)
	{
		if (this.m_coroutine != null)
		{
			return;
		}
		Coroutine coroutine = ThreadManager.StartCoroutine(this.CoroutineVisit(_poisPerAutoPause));
		if (this.m_isRunning)
		{
			this.m_coroutine = coroutine;
		}
	}

	// Token: 0x0600142C RID: 5164 RVA: 0x0007A266 File Offset: 0x00078466
	[PublicizedFrom(EAccessModifier.Private)]
	public void MacroPause()
	{
		if (this.m_isRunning)
		{
			this.m_isRunning = false;
			return;
		}
		this.m_isRunning = true;
	}

	// Token: 0x0600142D RID: 5165 RVA: 0x0007A27F File Offset: 0x0007847F
	[PublicizedFrom(EAccessModifier.Private)]
	public void MacroReset()
	{
		if (this.m_coroutine == null)
		{
			return;
		}
		this.m_isRunning = false;
		ThreadManager.StopCoroutine(this.m_coroutine);
		this.m_coroutine = null;
	}

	// Token: 0x0600142E RID: 5166 RVA: 0x0007A2A3 File Offset: 0x000784A3
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator CoroutineVisit(int _poisPerAutoPause)
	{
		World world = GameManager.Instance.World;
		if (world == null)
		{
			yield break;
		}
		EntityPlayerLocal player;
		if (!ProfilerGameUtils.TryGetFlyingPlayer(out player))
		{
			yield break;
		}
		this.m_isRunning = true;
		int poisVisited = 0;
		List<PrefabInstance> list = new List<PrefabInstance>();
		GameManager.Instance.GetDynamicPrefabDecorator().GetWorldPrefabs(list);
		foreach (PrefabInstance prefabInstance in list)
		{
			while (!this.m_isRunning)
			{
				yield return new WaitForSeconds(1f);
			}
			Vector3 size = prefabInstance.GetAABB().size;
			if (size.x > ConsoleCmdVisitPois.MinPoiSize.x && size.y > ConsoleCmdVisitPois.MinPoiSize.y && size.z > ConsoleCmdVisitPois.MinPoiSize.z)
			{
				if (world != GameManager.Instance.World || player != world.GetPrimaryPlayer())
				{
					this.MacroReset();
					yield break;
				}
				Bounds aabb = prefabInstance.GetAABB();
				Vector3 center = aabb.center;
				Log.Out(string.Format("Visit Pois: {0} ({1}, {2}, {3}) {4}", new object[] { prefabInstance.name, center.x, center.y, center.z, aabb }));
				player.SetPosition(center, true);
				yield return ProfilerGameUtils.WaitForChunksAroundObserverToLoad(player.ChunkObserver, ChunkConditions.Displayed);
				yield return null;
				yield return null;
				yield return null;
				if ((_poisPerAutoPause > 0 && poisVisited % _poisPerAutoPause == 0) || poisVisited == 0)
				{
					Log.Out(string.Format("Visit Pois: #{0} (PAUSED)", poisVisited));
					this.MacroPause();
				}
				int num = poisVisited;
				poisVisited = num + 1;
				prefabInstance = null;
			}
		}
		List<PrefabInstance>.Enumerator enumerator = default(List<PrefabInstance>.Enumerator);
		this.MacroReset();
		yield break;
		yield break;
	}

	// Token: 0x04000D8E RID: 3470
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_isRunning;

	// Token: 0x04000D8F RID: 3471
	[PublicizedFrom(EAccessModifier.Private)]
	public Coroutine m_coroutine;

	// Token: 0x04000D90 RID: 3472
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Vector3 MinPoiSize = new Vector3i(5, 5, 5);
}
