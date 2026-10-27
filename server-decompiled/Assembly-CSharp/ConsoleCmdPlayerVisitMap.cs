using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Platform;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000266 RID: 614
[Preserve]
public class ConsoleCmdPlayerVisitMap : ConsoleCmdAbstract
{
	// Token: 0x170001D2 RID: 466
	// (get) Token: 0x06001216 RID: 4630 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001217 RID: 4631 RVA: 0x000713F3 File Offset: 0x0006F5F3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "playervisitmap", "pvm" };
	}

	// Token: 0x06001218 RID: 4632 RVA: 0x0007140B File Offset: 0x0006F60B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Teleports the player through a rectangular area with optional memory logging";
	}

	// Token: 0x06001219 RID: 4633 RVA: 0x00071412 File Offset: 0x0006F612
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "<x1> <z1> <x2> <z2> : start teleporting through the area defined by the coorindates, should be such that x1 < x2 and z1 < z2\nstop : stop moving\nheight <int> : set the height off the ground to move the player to\nstepradius <int> : set distance between teleports in chunks, default is half the player's view dimension\nlogfile <prefix> : sets the file for the memory log to a temporary file with this prefix\nstepsperlog <int> : count of teleports between logs\ndumplog : dump the current log file to the game log";
	}

	// Token: 0x0600121A RID: 4634 RVA: 0x0007141C File Offset: 0x0006F61C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (!GameManager.Instance.World.GetPrimaryPlayer())
		{
			Log.Out("No local player! (Are you in-game?)");
			return;
		}
		if (_params.Count == 0)
		{
			return;
		}
		if (_params.Count == 4)
		{
			if (!int.TryParse(_params[0], out this.x1))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("The given x1 coordinate is not a valid integer");
				return;
			}
			if (!int.TryParse(_params[1], out this.z1))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("The given z1 coordinate is not a valid integer");
				return;
			}
			if (!int.TryParse(_params[2], out this.x2))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("The given x2 coordinate is not a valid integer");
				return;
			}
			if (!int.TryParse(_params[3], out this.z2))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("The given z2 coordinate is not a valid integer");
				return;
			}
			this.StartTraversing();
			return;
		}
		else
		{
			string text = _params[0].ToLowerInvariant();
			if (text == "stop")
			{
				this.Stop();
				return;
			}
			if (text == "height")
			{
				int num;
				if (_params.Count > 1 && int.TryParse(_params[1], out num))
				{
					this.height = Math.Max(num, 0);
				}
				Log.Out("Height above ground: {0}", new object[] { this.height });
				return;
			}
			if (!(text == "stepradius"))
			{
				if (text == "logfile")
				{
					string text2 = string.Empty;
					if (_params.Count > 1)
					{
						text2 = _params[1];
					}
					this.logFilePath = PlatformManager.NativePlatform.Utils.GetTempFileName(text2, ".log");
					Log.Out(string.Format("Setting logging for player visit map. Path: {0}, Steps per log: {1}", this.logFilePath, this.stepsPerLog));
					return;
				}
				if (text == "stepsperlog")
				{
					int num2;
					if (_params.Count > 1 && int.TryParse(_params[1], out num2))
					{
						if (num2 > 0)
						{
							this.stepsPerLog = num2;
							Log.Out(string.Format("Setting logging for player visit map. Path: {0}, Steps per log: {1}", this.logFilePath, this.stepsPerLog));
							return;
						}
						SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Steps per log must be at least 1");
					}
					return;
				}
				if (!(text == "dumplog"))
				{
					return;
				}
				if (this.logFilePath == null)
				{
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No log file set");
					return;
				}
				Log.Out(this.logFilePath + "\n" + File.ReadAllText(this.logFilePath));
				return;
			}
			else
			{
				int num3;
				if (_params.Count > 1 && int.TryParse(_params[1], out num3))
				{
					this.radius = num3;
				}
				if (this.radius >= 0)
				{
					Log.Out("Step Radius (Chunks): {0}", new object[] { this.radius });
					return;
				}
				Log.Out("Step Radius (Chunks): player view distance");
				return;
			}
		}
	}

	// Token: 0x0600121B RID: 4635 RVA: 0x000716DC File Offset: 0x0006F8DC
	[PublicizedFrom(EAccessModifier.Private)]
	public void StartTraversing()
	{
		if (this.traverseCoroutine != null)
		{
			return;
		}
		Coroutine coroutine = ThreadManager.StartCoroutine(this.CoroutineTraverse());
		if (this.isRunning)
		{
			this.traverseCoroutine = coroutine;
		}
	}

	// Token: 0x0600121C RID: 4636 RVA: 0x0007170D File Offset: 0x0006F90D
	[PublicizedFrom(EAccessModifier.Private)]
	public void Stop()
	{
		if (this.traverseCoroutine == null)
		{
			return;
		}
		this.isRunning = false;
		ThreadManager.StopCoroutine(this.traverseCoroutine);
		this.traverseCoroutine = null;
	}

	// Token: 0x0600121D RID: 4637 RVA: 0x00071731 File Offset: 0x0006F931
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator CoroutineTraverse()
	{
		EntityPlayerLocal player;
		if (!ProfilerGameUtils.TryGetFlyingPlayer(out player))
		{
			Log.Error("Could not get player, cancelling");
			yield break;
		}
		this.isRunning = true;
		int radiusChunks = this.radius;
		if (radiusChunks < 0)
		{
			radiusChunks = Math.Max(player.ChunkObserver.viewDim - 2, 0);
		}
		Vector3i chunkPos = new Vector3i(World.toChunkXZ((this.x1 <= this.x2) ? this.x1 : this.x2), 0, World.toChunkXZ((this.z1 <= this.z2) ? this.z1 : this.z2));
		Vector3i chunkPos2 = new Vector3i(World.toChunkXZ((this.x1 <= this.x2) ? this.x2 : this.x1), 0, World.toChunkXZ((this.z1 <= this.z2) ? this.z2 : this.z1));
		int num = chunkPos2.x - chunkPos.x + 1;
		int num2 = chunkPos2.z - chunkPos.z + 1;
		float time = Time.time;
		int curChunkX = Math.Min(chunkPos.x + radiusChunks, chunkPos2.x);
		int curChunkZ = Math.Min(chunkPos.z + radiusChunks, chunkPos2.z);
		ProfilingMetricCapture memoryMetrics = ProfilerCaptureUtils.CreateMemoryProfiler();
		if (this.logFilePath != null)
		{
			using (FileStream fileStream = File.Open(this.logFilePath, FileMode.OpenOrCreate))
			{
				using (StreamWriter streamWriter = new StreamWriter(fileStream))
				{
					streamWriter.Write("WorldX,WorldY,WorldZ,");
					streamWriter.Write(memoryMetrics.GetCsvHeader());
					streamWriter.WriteLine();
				}
			}
		}
		Log.Out("Running Player Visit Map. Block Rect: ({0},{1}), ({2},{3}), Step Radius: {4}, Chunk Rect: ({5},{6}), ({7},{8})", new object[] { this.x1, this.z1, this.x2, this.z2, radiusChunks, chunkPos.x, chunkPos.z, chunkPos2.x, chunkPos2.z });
		yield return null;
		int stepCount = 0;
		while (curChunkX - radiusChunks <= chunkPos2.x && curChunkZ - radiusChunks <= chunkPos2.z)
		{
			if (player.world != GameManager.Instance.World || player != GameManager.Instance.World.GetPrimaryPlayer())
			{
				this.Stop();
				yield break;
			}
			if (!this.isRunning)
			{
				yield break;
			}
			Vector3i blockPos = this.chunkPosToBlockPos(curChunkX, curChunkZ);
			player.SetPosition(blockPos, true);
			yield return new WaitForSeconds(0.5f);
			yield return ProfilerGameUtils.WaitForChunksAroundObserverToLoad(player.ChunkObserver, ChunkConditions.Displayed);
			curChunkX += radiusChunks * 2 + 1;
			if (curChunkX - radiusChunks > chunkPos2.x)
			{
				curChunkX = Math.Min(chunkPos.x + radiusChunks, chunkPos2.x);
				curChunkZ += radiusChunks * 2 + 1;
			}
			if (this.logFilePath != null && stepCount % this.stepsPerLog == 0)
			{
				using (FileStream fileStream2 = File.Open(this.logFilePath, FileMode.Append))
				{
					using (StreamWriter streamWriter2 = new StreamWriter(fileStream2))
					{
						streamWriter2.Write("{0},{1},{2},", blockPos.x, blockPos.y, blockPos.z);
						streamWriter2.Write(memoryMetrics.GetLastValueCsv());
						streamWriter2.WriteLine();
					}
				}
			}
			int num3 = stepCount;
			stepCount = num3 + 1;
		}
		this.Stop();
		yield break;
	}

	// Token: 0x0600121E RID: 4638 RVA: 0x00071740 File Offset: 0x0006F940
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i chunkPosToBlockPos(int _x, int _z)
	{
		int num = (_x << 4) + 8;
		int num2 = (_z << 4) + 8;
		return new Vector3i((float)num, GameManager.Instance.World.GetHeightAt((float)num, (float)num2) + (float)this.height, (float)num2);
	}

	// Token: 0x04000D06 RID: 3334
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isRunning;

	// Token: 0x04000D07 RID: 3335
	[PublicizedFrom(EAccessModifier.Private)]
	public Coroutine traverseCoroutine;

	// Token: 0x04000D08 RID: 3336
	[PublicizedFrom(EAccessModifier.Private)]
	public int x1;

	// Token: 0x04000D09 RID: 3337
	[PublicizedFrom(EAccessModifier.Private)]
	public int z1;

	// Token: 0x04000D0A RID: 3338
	[PublicizedFrom(EAccessModifier.Private)]
	public int x2;

	// Token: 0x04000D0B RID: 3339
	[PublicizedFrom(EAccessModifier.Private)]
	public int z2;

	// Token: 0x04000D0C RID: 3340
	[PublicizedFrom(EAccessModifier.Private)]
	public int height = 10;

	// Token: 0x04000D0D RID: 3341
	[PublicizedFrom(EAccessModifier.Private)]
	public int radius = -1;

	// Token: 0x04000D0E RID: 3342
	[PublicizedFrom(EAccessModifier.Private)]
	public string logFilePath;

	// Token: 0x04000D0F RID: 3343
	[PublicizedFrom(EAccessModifier.Private)]
	public int stepsPerLog = 1;
}
