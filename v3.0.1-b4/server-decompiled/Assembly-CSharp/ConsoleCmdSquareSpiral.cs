using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200029E RID: 670
[Preserve]
public class ConsoleCmdSquareSpiral : ConsoleCmdAbstract
{
	// Token: 0x1700021A RID: 538
	// (get) Token: 0x06001384 RID: 4996 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001385 RID: 4997 RVA: 0x00077274 File Offset: 0x00075474
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "squarespiral", "sqs" };
	}

	// Token: 0x06001386 RID: 4998 RVA: 0x0007728C File Offset: 0x0007548C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Move the player chunk by chunk in a square spiral. Will start off paused and required un-pausing. Also gives god mode and flying at the start.";
	}

	// Token: 0x06001387 RID: 4999 RVA: 0x00077293 File Offset: 0x00075493
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "<s[tart] [chunks per auto-pause]|p[ause]|r[eset]|waitmode [minimal|meshes|displayed]>";
	}

	// Token: 0x06001388 RID: 5000 RVA: 0x0007729A File Offset: 0x0007549A
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (!this.ExecuteInternal(_params, _senderInfo))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(this.GetHelp());
		}
	}

	// Token: 0x06001389 RID: 5001 RVA: 0x000772B8 File Offset: 0x000754B8
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
		uint num = <PrivateImplementationDetails>.ComputeStringHash(text);
		if (num <= 1697318111U)
		{
			if (num != 367725747U)
			{
				if (num != 1695364032U)
				{
					if (num != 1697318111U)
					{
						return false;
					}
					if (!(text == "start"))
					{
						return false;
					}
				}
				else
				{
					if (!(text == "reset"))
					{
						return false;
					}
					goto IL_017A;
				}
			}
			else
			{
				if (!(text == "waitmode"))
				{
					return false;
				}
				if (_params.Count > 1)
				{
					string text2 = _params[1].ToLowerInvariant();
					if (text2 == "minimal")
					{
						this.waitMode = ConsoleCmdSquareSpiral.WaitMode.SingleChunkDecorated;
						this.chunkCondition = ChunkConditions.Decorated;
						return true;
					}
					if (text2 == "meshes")
					{
						this.waitMode = ConsoleCmdSquareSpiral.WaitMode.SurroundingMeshesCopied;
						this.chunkCondition = ChunkConditions.MeshesCopied;
						return true;
					}
					if (text2 == "displayed")
					{
						this.waitMode = ConsoleCmdSquareSpiral.WaitMode.SurroundingChunksDisplayed;
						this.chunkCondition = ChunkConditions.Displayed;
						return true;
					}
				}
				return false;
			}
		}
		else
		{
			if (num <= 4111221743U)
			{
				if (num != 1887753101U)
				{
					if (num != 4111221743U)
					{
						return false;
					}
					if (!(text == "p"))
					{
						return false;
					}
				}
				else if (!(text == "pause"))
				{
					return false;
				}
				this.MacroPause();
				return true;
			}
			if (num != 4127999362U)
			{
				if (num != 4144776981U)
				{
					return false;
				}
				if (!(text == "r"))
				{
					return false;
				}
				goto IL_017A;
			}
			else if (!(text == "s"))
			{
				return false;
			}
		}
		if (_params.Count > 2)
		{
			return false;
		}
		int num2 = 0;
		if (_params.Count > 1 && !int.TryParse(_params[1], out num2))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Failed to parse as int " + _params[1]);
			return false;
		}
		this.MacroStart(num2);
		return true;
		IL_017A:
		this.MacroReset();
		return true;
	}

	// Token: 0x0600138A RID: 5002 RVA: 0x000774C8 File Offset: 0x000756C8
	[PublicizedFrom(EAccessModifier.Private)]
	public void MacroStart(int _chunksPerAutoPause)
	{
		if (this.m_coroutine != null)
		{
			return;
		}
		Coroutine coroutine = ThreadManager.StartCoroutine(this.CoroutineSpiral(_chunksPerAutoPause));
		if (this.m_isRunning)
		{
			this.m_coroutine = coroutine;
		}
	}

	// Token: 0x0600138B RID: 5003 RVA: 0x000774FA File Offset: 0x000756FA
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

	// Token: 0x0600138C RID: 5004 RVA: 0x00077513 File Offset: 0x00075713
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

	// Token: 0x0600138D RID: 5005 RVA: 0x00077537 File Offset: 0x00075737
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator CoroutineSpiral(int _chunksPerAutoPause)
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
		IEnumerator<Vector2i> spiralSequence = this.SpiralSequence();
		float lastY = player.position.y;
		int i = 0;
		DateTime lastPrintTime = DateTime.MinValue;
		while (world == GameManager.Instance.World && !(player != world.GetPrimaryPlayer()))
		{
			if (this.m_isRunning)
			{
				spiralSequence.MoveNext();
				Vector2i chunkPos = spiralSequence.Current;
				int num = i;
				i = num + 1;
				DateTime now = DateTime.Now;
				if (now.Subtract(lastPrintTime).TotalSeconds > 10.0)
				{
					Log.Out("Square Spiral: ({0}, {1}) #{2}", new object[] { chunkPos.x, chunkPos.y, i });
					lastPrintTime = now;
				}
				Vector2i centreOfChunk = new Vector2i(chunkPos.x * 16 + 8, chunkPos.y * 16 + 8);
				Vector3 rotationEuler = Quaternion.FromToRotation(Vector3.forward, new Vector3((float)(-(float)centreOfChunk.x), 0f, (float)(-(float)centreOfChunk.y))).eulerAngles;
				Chunk chunk = (Chunk)world.GetChunkSync(chunkPos.x, chunkPos.y);
				if (chunk == null)
				{
					player.SetPosition(new Vector3((float)centreOfChunk.x, lastY, (float)centreOfChunk.y), true);
					player.SetRotation(rotationEuler);
					while (chunk == null)
					{
						yield return null;
						chunk = (Chunk)world.GetChunkSync(chunkPos.x, chunkPos.y);
					}
				}
				float num2 = (float)(world.GetHeight(centreOfChunk.x, centreOfChunk.y) + 10);
				player.SetPosition(new Vector3((float)centreOfChunk.x, num2, (float)centreOfChunk.y), true);
				player.SetRotation(rotationEuler);
				lastY = player.position.y;
				switch (this.waitMode)
				{
				case ConsoleCmdSquareSpiral.WaitMode.SingleChunkDecorated:
					yield return ProfilerGameUtils.WaitForSingleChunkToLoad(chunk, this.chunkCondition);
					break;
				case ConsoleCmdSquareSpiral.WaitMode.SurroundingMeshesCopied:
					yield return ProfilerGameUtils.WaitForChunksAroundObserverToLoad(player.ChunkObserver, this.chunkCondition);
					break;
				case ConsoleCmdSquareSpiral.WaitMode.SurroundingChunksDisplayed:
					yield return ProfilerGameUtils.WaitForChunksAroundObserverToLoad(player.ChunkObserver, this.chunkCondition);
					break;
				}
				yield return null;
				if ((_chunksPerAutoPause > 0 && i % _chunksPerAutoPause == 0) || i == 1)
				{
					Log.Out("Square Spiral: ({0}, {1}) #{2} (PAUSED)", new object[] { chunkPos.x, chunkPos.y, i });
					this.MacroPause();
				}
				rotationEuler = default(Vector3);
			}
			else
			{
				yield return new WaitForSeconds(1f);
			}
		}
		this.MacroReset();
		yield break;
		yield break;
	}

	// Token: 0x0600138E RID: 5006 RVA: 0x0007754D File Offset: 0x0007574D
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator<Vector2i> SpiralSequence()
	{
		int x = 0;
		int y = 0;
		int rPos = 1;
		int rNeg = -1;
		ConsoleCmdSquareSpiral.SpiralSequenceState state = ConsoleCmdSquareSpiral.SpiralSequenceState.Left;
		for (;;)
		{
			yield return new Vector2i(x, y);
			switch (state)
			{
			case ConsoleCmdSquareSpiral.SpiralSequenceState.Left:
				if (x == rNeg)
				{
					state = ConsoleCmdSquareSpiral.SpiralSequenceState.Down;
					int num = y;
					y = num - 1;
				}
				else
				{
					int num = x;
					x = num - 1;
				}
				break;
			case ConsoleCmdSquareSpiral.SpiralSequenceState.Down:
				if (y == rNeg)
				{
					state = ConsoleCmdSquareSpiral.SpiralSequenceState.Right;
					int num = x;
					x = num + 1;
				}
				else
				{
					int num = y;
					y = num - 1;
				}
				break;
			case ConsoleCmdSquareSpiral.SpiralSequenceState.Right:
				if (x == rPos)
				{
					state = ConsoleCmdSquareSpiral.SpiralSequenceState.Up;
					int num = y;
					y = num + 1;
				}
				else
				{
					int num = x;
					x = num + 1;
				}
				break;
			case ConsoleCmdSquareSpiral.SpiralSequenceState.Up:
				if (y == rPos)
				{
					int num = rPos;
					rPos = num + 1;
					num = rNeg;
					rNeg = num - 1;
					state = ConsoleCmdSquareSpiral.SpiralSequenceState.Left;
					num = x;
					x = num - 1;
				}
				else
				{
					int num = y;
					y = num + 1;
				}
				break;
			}
		}
		yield break;
	}

	// Token: 0x04000D63 RID: 3427
	[PublicizedFrom(EAccessModifier.Private)]
	public bool m_isRunning;

	// Token: 0x04000D64 RID: 3428
	[PublicizedFrom(EAccessModifier.Private)]
	public Coroutine m_coroutine;

	// Token: 0x04000D65 RID: 3429
	[PublicizedFrom(EAccessModifier.Private)]
	public ChunkConditions.Delegate chunkCondition = ChunkConditions.Decorated;

	// Token: 0x04000D66 RID: 3430
	[PublicizedFrom(EAccessModifier.Private)]
	public ConsoleCmdSquareSpiral.WaitMode waitMode;

	// Token: 0x0200029F RID: 671
	[PublicizedFrom(EAccessModifier.Private)]
	public enum WaitMode
	{
		// Token: 0x04000D68 RID: 3432
		SingleChunkDecorated,
		// Token: 0x04000D69 RID: 3433
		SurroundingMeshesCopied,
		// Token: 0x04000D6A RID: 3434
		SurroundingChunksDisplayed
	}

	// Token: 0x020002A0 RID: 672
	[PublicizedFrom(EAccessModifier.Private)]
	public enum SpiralSequenceState
	{
		// Token: 0x04000D6C RID: 3436
		Left,
		// Token: 0x04000D6D RID: 3437
		Down,
		// Token: 0x04000D6E RID: 3438
		Right,
		// Token: 0x04000D6F RID: 3439
		Up
	}
}
