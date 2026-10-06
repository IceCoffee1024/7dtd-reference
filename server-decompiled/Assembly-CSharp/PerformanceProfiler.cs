using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SystemInformation;
using Unity.Profiling;
using UnityEngine;

// Token: 0x02001533 RID: 5427
public static class PerformanceProfiler
{
	// Token: 0x14000108 RID: 264
	// (add) Token: 0x0600AA56 RID: 43606 RVA: 0x003F9458 File Offset: 0x003F7658
	// (remove) Token: 0x0600AA57 RID: 43607 RVA: 0x003F948C File Offset: 0x003F768C
	public static event Action<int, int, PerformanceProfiler.PerformanceMetrics> OnRunComplete;

	// Token: 0x14000109 RID: 265
	// (add) Token: 0x0600AA58 RID: 43608 RVA: 0x003F94C0 File Offset: 0x003F76C0
	// (remove) Token: 0x0600AA59 RID: 43609 RVA: 0x003F94F4 File Offset: 0x003F76F4
	public static event Action<PerformanceProfiler.AggregateMetrics> OnSessionComplete;

	// Token: 0x0600AA5A RID: 43610 RVA: 0x003F9528 File Offset: 0x003F7728
	public static void BeginSession(string directory, int runCount, string prefix = null)
	{
		if (PerformanceProfiler.isInSession)
		{
			Debug.LogWarning("[PerformanceProfiler] Session already in progress.");
			return;
		}
		if (runCount < 1)
		{
			Debug.LogWarning("[PerformanceProfiler] runCount must be >= 1.");
			return;
		}
		PerformanceProfiler.isInSession = true;
		PerformanceProfiler.sessionRunTarget = runCount;
		PerformanceProfiler.sessionRunsCompleted = 0;
		PerformanceProfiler.sessionRunMetrics.Clear();
		PerformanceProfiler.sessionDirectory = directory ?? DateTime.Now.ToString("yyyyMMdd_HHmmss");
		PerformanceProfiler.sessionPrefix = prefix;
		Debug.Log(string.Format("[PerformanceProfiler] Session '{0}' begun — {1} run(s) planned.", PerformanceProfiler.sessionDirectory, runCount));
	}

	// Token: 0x0600AA5B RID: 43611 RVA: 0x003F95AE File Offset: 0x003F77AE
	public static void AbortSession()
	{
		if (!PerformanceProfiler.isInSession)
		{
			return;
		}
		if (PerformanceProfiler.isCapturing)
		{
			PerformanceProfiler.StopCapture(false);
		}
		PerformanceProfiler.isInSession = false;
		PerformanceProfiler.sessionPrefix = null;
		PerformanceProfiler.sessionRunMetrics.Clear();
		Debug.Log("[PerformanceProfiler] Session aborted.");
	}

	// Token: 0x0600AA5C RID: 43612 RVA: 0x003F95E8 File Offset: 0x003F77E8
	public static void StartCapture(string directory = null, string prefix = null, int targetFps = 30)
	{
		if (PerformanceProfiler.isCapturing)
		{
			Debug.LogWarning("[PerformanceProfiler] Capture already in progress.");
			return;
		}
		PerformanceProfiler.frameTimeData.Clear();
		PerformanceProfiler.frameTimeData.Capacity = 5000;
		PerformanceProfiler.frameExtraData.Clear();
		PerformanceProfiler.frameExtraData.Capacity = 5000;
		PerformanceProfiler.lastRunSpikeEvents.Clear();
		PerformanceProfiler.lastRunSpikeEvents.Capacity = 1000;
		PerformanceProfiler.profilePrefix = prefix;
		PerformanceProfiler.targetFramerate = targetFps;
		PerformanceProfiler.captureStartTime = DateTime.Now;
		PerformanceProfiler.captureDirectory = (PerformanceProfiler.isInSession ? PerformanceProfiler.sessionDirectory : (directory ?? DateTime.Now.ToString("yyyyMMdd_HHmmss")));
		if (!PerformanceProfiler.TryCreateProfilerRecorder())
		{
			Debug.LogWarning("[PerformanceProfiler] ProfilerRecorder not available — using Time.deltaTime.");
		}
		PerformanceProfiler.memoryProfiler = ProfilerCaptureUtils.CreateMemoryProfiler();
		PerformanceProfiler.memorySnapshotStart = null;
		PerformanceProfiler.isCapturing = true;
		GameObject gameObject = new GameObject("PerformanceProfilerCapture");
		gameObject.hideFlags = HideFlags.HideAndDontSave;
		PerformanceProfiler.captureComponent = gameObject.AddComponent<PerformanceProfiler.FrameTimeCapture>();
		global::UnityEngine.Object.DontDestroyOnLoad(gameObject);
		string text = ((prefix != null) ? (" [" + prefix + "]") : "");
		Debug.Log(string.Format("[PerformanceProfiler] Capture started{0}. Target: {1} FPS.", text, PerformanceProfiler.targetFramerate));
	}

	// Token: 0x0600AA5D RID: 43613 RVA: 0x003F9710 File Offset: 0x003F7910
	public static PerformanceProfiler.PerformanceMetrics StopCapture(bool saveOutput = true)
	{
		if (!PerformanceProfiler.isCapturing)
		{
			Debug.LogWarning("[PerformanceProfiler] No capture in progress.");
			return default(PerformanceProfiler.PerformanceMetrics);
		}
		PerformanceProfiler.isCapturing = false;
		if (PerformanceProfiler.captureComponent != null)
		{
			global::UnityEngine.Object.Destroy(PerformanceProfiler.captureComponent.gameObject);
			PerformanceProfiler.captureComponent = null;
		}
		if (PerformanceProfiler.frameTimeRecorder.Valid)
		{
			PerformanceProfiler.frameTimeRecorder.Dispose();
		}
		float num = (float)(DateTime.Now - PerformanceProfiler.captureStartTime).TotalSeconds;
		if (PerformanceProfiler.frameTimeData.Count == 0)
		{
			Debug.LogError("[PerformanceProfiler] No frame data captured.");
			return default(PerformanceProfiler.PerformanceMetrics);
		}
		Debug.Log(string.Format("[PerformanceProfiler] Capture stopped — {0} frames over {1:F2}s.", PerformanceProfiler.frameTimeData.Count, num));
		PerformanceProfiler.PerformanceMetrics performanceMetrics = PerformanceProfiler.CalculateMetrics(num);
		if (saveOutput)
		{
			string text = PerformanceProfiler.BuildBasePath(PerformanceProfiler.captureDirectory);
			PerformanceProfiler.SaveRawData(Path.Combine(text, "perf_raw_" + PerformanceProfiler.profilePrefix + ".csv"));
			PerformanceProfiler.SaveAnalysis(Path.Combine(text, "perf_analysis_" + PerformanceProfiler.profilePrefix + ".csv"), performanceMetrics);
			PerformanceProfiler.SaveSpikeEvents(Path.Combine(text, "perf_spikes_" + PerformanceProfiler.profilePrefix + ".csv"), PerformanceProfiler.lastRunSpikeEvents);
			PerformanceProfiler.SaveMemorySnapshot(Path.Combine(text, "perf_memory_" + PerformanceProfiler.profilePrefix + ".csv"));
			Debug.Log("Saved performance output to " + text + ".");
		}
		if (PerformanceProfiler.memoryProfiler != null)
		{
			PerformanceProfiler.memoryProfiler.Cleanup();
			PerformanceProfiler.memoryProfiler = null;
		}
		PerformanceProfiler.LogSummary(performanceMetrics);
		if (PerformanceProfiler.isInSession)
		{
			PerformanceProfiler.sessionRunMetrics.Add(performanceMetrics);
			int num2 = PerformanceProfiler.sessionRunsCompleted++;
			Action<int, int, PerformanceProfiler.PerformanceMetrics> onRunComplete = PerformanceProfiler.OnRunComplete;
			if (onRunComplete != null)
			{
				onRunComplete(num2, PerformanceProfiler.sessionRunTarget, performanceMetrics);
			}
			Debug.Log(string.Format("[PerformanceProfiler] Session run {0}/{1} complete.", PerformanceProfiler.sessionRunsCompleted, PerformanceProfiler.sessionRunTarget));
			if (PerformanceProfiler.sessionRunsCompleted >= PerformanceProfiler.sessionRunTarget)
			{
				PerformanceProfiler.FinalizeSession();
			}
		}
		else
		{
			Action<int, int, PerformanceProfiler.PerformanceMetrics> onRunComplete2 = PerformanceProfiler.OnRunComplete;
			if (onRunComplete2 != null)
			{
				onRunComplete2(0, 1, performanceMetrics);
			}
		}
		return performanceMetrics;
	}

	// Token: 0x0600AA5E RID: 43614 RVA: 0x003F9928 File Offset: 0x003F7B28
	public static IReadOnlyList<PerformanceProfiler.SpikeEvent> GetLastRunSpikeEvents()
	{
		return PerformanceProfiler.lastRunSpikeEvents;
	}

	// Token: 0x0600AA5F RID: 43615 RVA: 0x003F992F File Offset: 0x003F7B2F
	public static bool IsCapturing()
	{
		return PerformanceProfiler.isCapturing;
	}

	// Token: 0x0600AA60 RID: 43616 RVA: 0x003F9936 File Offset: 0x003F7B36
	public static int GetCurrentFrameCount()
	{
		return PerformanceProfiler.frameTimeData.Count;
	}

	// Token: 0x0600AA61 RID: 43617 RVA: 0x003F9942 File Offset: 0x003F7B42
	public static float GetLastFrameTime()
	{
		return PerformanceProfiler.lastFrameTime;
	}

	// Token: 0x0600AA62 RID: 43618 RVA: 0x003F994C File Offset: 0x003F7B4C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void FinalizeSession()
	{
		PerformanceProfiler.AggregateMetrics aggregateMetrics = PerformanceProfiler.CalculateAggregateMetrics(PerformanceProfiler.sessionRunMetrics);
		string text = PerformanceProfiler.BuildBasePath(PerformanceProfiler.sessionDirectory);
		string text2 = (string.IsNullOrEmpty(PerformanceProfiler.sessionPrefix) ? "perf_aggregate.csv" : ("perf_aggregate_" + PerformanceProfiler.sessionPrefix + ".csv"));
		PerformanceProfiler.SaveAggregateAnalysis(Path.Combine(text, text2), aggregateMetrics);
		PerformanceProfiler.LogAggregateSummary(aggregateMetrics);
		Action<PerformanceProfiler.AggregateMetrics> onSessionComplete = PerformanceProfiler.OnSessionComplete;
		if (onSessionComplete != null)
		{
			onSessionComplete(aggregateMetrics);
		}
		PerformanceProfiler.isInSession = false;
		PerformanceProfiler.sessionPrefix = null;
		PerformanceProfiler.sessionRunMetrics.Clear();
		Debug.Log("[PerformanceProfiler] Session '" + PerformanceProfiler.sessionDirectory + "' complete.");
	}

	// Token: 0x0600AA63 RID: 43619 RVA: 0x003F99E8 File Offset: 0x003F7BE8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void CaptureFrameTime()
	{
		if (!PerformanceProfiler.isCapturing)
		{
			return;
		}
		float num = 0f;
		if (PerformanceProfiler.frameTimeRecorder.Valid)
		{
			num = (float)PerformanceProfiler.frameTimeRecorder.LastValue / 1000000f;
		}
		if (num <= 0f)
		{
			num = Time.deltaTime * 1000f;
		}
		if (num <= 0f || num >= 1000f)
		{
			return;
		}
		long currentTextureMemory = (long)Texture.currentTextureMemory;
		World world = GameManager.Instance.World;
		PerformanceProfiler.frameTimeData.Add(num);
		PerformanceProfiler.frameExtraData.Add(new PerformanceProfiler.FrameInfo
		{
			systemTextureMemory = currentTextureMemory,
			systemTextureMemoryDelta = currentTextureMemory - PerformanceProfiler.lastSystemTextureMemory,
			texStreamCurrentMB = Texture.currentTextureMemory / 1048576f,
			texStreamTargetMB = Texture.targetTextureMemory / 1048576f,
			texStreamDesiredMB = Texture.desiredTextureMemory / 1048576f,
			texStreamBudgetMB = (QualitySettings.streamingMipmapsActive ? QualitySettings.streamingMipmapsMemoryBudget : (-1f)),
			gcHeapMemoryMB = (float)GC.GetTotalMemory(false) / 1048576f,
			rssMemoryMB = (float)GetRSS.GetCurrentRSS() / 1024f / 1024f,
			chunkCount = Chunk.InstanceCount,
			displayedChunkObjects = ((world != null) ? world.m_ChunkManager.GetDisplayedChunkGameObjectsCount() : 0),
			enemyCount = GameStats.GetInt(EnumGameStats.EnemyCount),
			entityCount = ((world != null) ? world.Entities.Count : 0),
			entityInstanceCount = Entity.InstanceCount,
			itemCount = EntityItem.ItemInstanceCount,
			timeSinceLevelLoadMin = Time.timeSinceLevelLoad / 60f
		});
		PerformanceProfiler.lastSystemTextureMemory = currentTextureMemory;
		PerformanceProfiler.lastFrameTime = num;
	}

	// Token: 0x0600AA64 RID: 43620 RVA: 0x003F9B90 File Offset: 0x003F7D90
	[PublicizedFrom(EAccessModifier.Private)]
	public static float[] CalculateRollingBaseline()
	{
		int count = PerformanceProfiler.frameTimeData.Count;
		float[] array = new float[count];
		List<float> list = new List<float>(61);
		for (int i = 0; i < count; i++)
		{
			int num = Math.Max(0, i - 30);
			int num2 = Math.Min(count - 1, i + 30);
			list.Clear();
			for (int j = num; j <= num2; j++)
			{
				if (j != i)
				{
					list.Add(PerformanceProfiler.frameTimeData[j]);
				}
			}
			list.Sort();
			array[i] = ((list.Count == 0) ? PerformanceProfiler.frameTimeData[i] : list[list.Count / 2]);
		}
		return array;
	}

	// Token: 0x0600AA65 RID: 43621 RVA: 0x003F9C38 File Offset: 0x003F7E38
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<PerformanceProfiler.SpikeEvent> ExtractSpikeEvents(float[] baseline)
	{
		int count = PerformanceProfiler.frameTimeData.Count;
		bool[] array = new bool[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = baseline[i] > 0f && PerformanceProfiler.frameTimeData[i] > baseline[i] * 2f;
		}
		List<ValueTuple<int, int>> list = new List<ValueTuple<int, int>>();
		int j = 0;
		while (j < count)
		{
			if (!array[j])
			{
				j++;
			}
			else
			{
				int num = j;
				while (j < count && array[j])
				{
					j++;
				}
				list.Add(new ValueTuple<int, int>(num, j - 1));
			}
		}
		for (int k = list.Count - 1; k > 0; k--)
		{
			if (list[k].Item1 - list[k - 1].Item2 - 1 < 3)
			{
				list[k - 1] = new ValueTuple<int, int>(list[k - 1].Item1, list[k].Item2);
				list.RemoveAt(k);
			}
		}
		List<PerformanceProfiler.SpikeEvent> list2 = new List<PerformanceProfiler.SpikeEvent>(list.Count);
		foreach (ValueTuple<int, int> valueTuple in list)
		{
			int item = valueTuple.Item1;
			int item2 = valueTuple.Item2;
			float num2 = 0f;
			float num3 = 0f;
			for (int l = item; l <= item2; l++)
			{
				num2 += PerformanceProfiler.frameTimeData[l];
				if (PerformanceProfiler.frameTimeData[l] > num3)
				{
					num3 = PerformanceProfiler.frameTimeData[l];
				}
			}
			float num4 = 0f;
			int num5 = Math.Max(1, item - 1);
			int num6 = Math.Min(count - 1, item2 + 1);
			for (int m = num5; m <= num6; m++)
			{
				float num7 = PerformanceProfiler.frameExtraData[m - 1].gcHeapMemoryMB - PerformanceProfiler.frameExtraData[m].gcHeapMemoryMB;
				if (num7 > num4)
				{
					num4 = num7;
				}
			}
			list2.Add(new PerformanceProfiler.SpikeEvent
			{
				startFrame = item,
				durationFrames = item2 - item + 1,
				totalDurationMs = num2,
				peakFrameTimeMs = num3,
				baselineFrameTimeMs = baseline[item],
				peakMultiple = ((baseline[item] > 0f) ? (num3 / baseline[item]) : 0f),
				isGcAttributed = (num4 >= 0.5f),
				gcHeapDropMB = ((num4 >= 0.5f) ? num4 : 0f)
			});
		}
		return list2;
	}

	// Token: 0x0600AA66 RID: 43622 RVA: 0x003F9EEC File Offset: 0x003F80EC
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<int> DetectGcCollections()
	{
		List<int> list = new List<int>();
		for (int i = 1; i < PerformanceProfiler.frameExtraData.Count; i++)
		{
			if (PerformanceProfiler.frameExtraData[i - 1].gcHeapMemoryMB - PerformanceProfiler.frameExtraData[i].gcHeapMemoryMB >= 0.5f)
			{
				list.Add(i);
			}
		}
		return list;
	}

	// Token: 0x0600AA67 RID: 43623 RVA: 0x003F9F48 File Offset: 0x003F8148
	[PublicizedFrom(EAccessModifier.Private)]
	public static PerformanceProfiler.PerformanceMetrics CalculateMetrics(float captureDuration)
	{
		PerformanceProfiler.<>c__DisplayClass49_0 CS$<>8__locals1 = new PerformanceProfiler.<>c__DisplayClass49_0();
		CS$<>8__locals1.m = default(PerformanceProfiler.PerformanceMetrics);
		if (PerformanceProfiler.frameTimeData.Count == 0)
		{
			return CS$<>8__locals1.m;
		}
		List<float> list = new List<float>(PerformanceProfiler.frameTimeData);
		list.Sort();
		CS$<>8__locals1.m.totalFramesCaptured = PerformanceProfiler.frameTimeData.Count;
		CS$<>8__locals1.m.captureDurationSeconds = captureDuration;
		CS$<>8__locals1.m.averageFrameTime = PerformanceProfiler.frameTimeData.Average();
		CS$<>8__locals1.m.averageFPS = 1000f / CS$<>8__locals1.m.averageFrameTime;
		CS$<>8__locals1.m.minFrameTime = list[0];
		CS$<>8__locals1.m.maxFrameTime = list[list.Count - 1];
		CS$<>8__locals1.m.fivePercentLow = PerformanceProfiler.GetPercentile(list, 95f);
		CS$<>8__locals1.m.onePercentLow = PerformanceProfiler.GetPercentile(list, 99f);
		CS$<>8__locals1.m.pointOnePercentLow = PerformanceProfiler.GetPercentile(list, 99.9f);
		float num = PerformanceProfiler.frameTimeData.Sum<float>((float f) => (f - CS$<>8__locals1.m.averageFrameTime) * (f - CS$<>8__locals1.m.averageFrameTime));
		CS$<>8__locals1.m.standardDeviation = Mathf.Sqrt(num / (float)CS$<>8__locals1.m.totalFramesCaptured);
		CS$<>8__locals1.m.coefficientOfVariation = CS$<>8__locals1.m.standardDeviation / CS$<>8__locals1.m.averageFrameTime;
		float num2 = 1000f / (float)PerformanceProfiler.targetFramerate;
		float num3 = num2 * 1.2f;
		float num4 = num2 * 1.5f;
		int num5 = 0;
		foreach (float num6 in PerformanceProfiler.frameTimeData)
		{
			if (num6 > num3)
			{
				PerformanceProfiler.<>c__DisplayClass49_0 CS$<>8__locals2 = CS$<>8__locals1;
				CS$<>8__locals2.m.framePacingViolations20 = CS$<>8__locals2.m.framePacingViolations20 + 1;
				num5++;
			}
			else
			{
				num5 = 0;
			}
			if (num6 > num4)
			{
				PerformanceProfiler.<>c__DisplayClass49_0 CS$<>8__locals3 = CS$<>8__locals1;
				CS$<>8__locals3.m.framePacingViolations50 = CS$<>8__locals3.m.framePacingViolations50 + 1;
			}
			CS$<>8__locals1.m.maxConsecutiveBadFrames = Mathf.Max(CS$<>8__locals1.m.maxConsecutiveBadFrames, num5);
		}
		CS$<>8__locals1.m.framePacingViolationPercent20 = (float)CS$<>8__locals1.m.framePacingViolations20 / (float)CS$<>8__locals1.m.totalFramesCaptured * 100f;
		CS$<>8__locals1.m.framePacingViolationPercent50 = (float)CS$<>8__locals1.m.framePacingViolations50 / (float)CS$<>8__locals1.m.totalFramesCaptured * 100f;
		List<PerformanceProfiler.SpikeEvent> list2 = PerformanceProfiler.ExtractSpikeEvents(PerformanceProfiler.CalculateRollingBaseline());
		PerformanceProfiler.lastRunSpikeEvents = list2;
		CS$<>8__locals1.m.spikeEventCount = list2.Count;
		CS$<>8__locals1.m.gcAttributedSpikeCount = list2.Count<PerformanceProfiler.SpikeEvent>((PerformanceProfiler.SpikeEvent s) => s.isGcAttributed);
		CS$<>8__locals1.m.nonGcAttributedSpikeCount = list2.Count<PerformanceProfiler.SpikeEvent>((PerformanceProfiler.SpikeEvent s) => !s.isGcAttributed);
		CS$<>8__locals1.m.gcAttributedSpikePercent = ((list2.Count > 0) ? ((float)CS$<>8__locals1.m.gcAttributedSpikeCount / (float)list2.Count * 100f) : 0f);
		if (list2.Count > 0)
		{
			CS$<>8__locals1.m.meanSpikeDurationMs = list2.Average<PerformanceProfiler.SpikeEvent>((PerformanceProfiler.SpikeEvent s) => s.totalDurationMs);
			CS$<>8__locals1.m.maxSpikeDurationMs = list2.Max<PerformanceProfiler.SpikeEvent>((PerformanceProfiler.SpikeEvent s) => s.totalDurationMs);
			List<PerformanceProfiler.SpikeEvent> list3 = list2.Where<PerformanceProfiler.SpikeEvent>((PerformanceProfiler.SpikeEvent s) => s.isGcAttributed).ToList<PerformanceProfiler.SpikeEvent>();
			List<PerformanceProfiler.SpikeEvent> list4 = list2.Where<PerformanceProfiler.SpikeEvent>((PerformanceProfiler.SpikeEvent s) => !s.isGcAttributed).ToList<PerformanceProfiler.SpikeEvent>();
			PerformanceProfiler.<>c__DisplayClass49_0 CS$<>8__locals4 = CS$<>8__locals1;
			float num7;
			if (list3.Count <= 0)
			{
				num7 = 0f;
			}
			else
			{
				num7 = list3.Average<PerformanceProfiler.SpikeEvent>((PerformanceProfiler.SpikeEvent s) => s.totalDurationMs);
			}
			CS$<>8__locals4.m.meanGcSpikeMs = num7;
			PerformanceProfiler.<>c__DisplayClass49_0 CS$<>8__locals5 = CS$<>8__locals1;
			float num8;
			if (list4.Count <= 0)
			{
				num8 = 0f;
			}
			else
			{
				num8 = list4.Average<PerformanceProfiler.SpikeEvent>((PerformanceProfiler.SpikeEvent s) => s.totalDurationMs);
			}
			CS$<>8__locals5.m.meanNonGcSpikeMs = num8;
		}
		if (list2.Count >= 2)
		{
			float num9 = 0f;
			for (int i = 1; i < list2.Count; i++)
			{
				num9 += (float)(list2[i].startFrame - list2[i - 1].startFrame);
			}
			float num10 = num9 / (float)(list2.Count - 1);
			CS$<>8__locals1.m.meanTimeBetweenSpikesSec = num10 * (captureDuration / (float)CS$<>8__locals1.m.totalFramesCaptured);
		}
		List<int> list5 = PerformanceProfiler.DetectGcCollections();
		CS$<>8__locals1.m.estimatedGcCollectionCount = list5.Count;
		if (list5.Count >= 2)
		{
			float num11 = 0f;
			for (int j = 1; j < list5.Count; j++)
			{
				num11 += (float)(list5[j] - list5[j - 1]);
			}
			float num12 = num11 / (float)(list5.Count - 1);
			CS$<>8__locals1.m.meanGcIntervalSec = num12 * (captureDuration / (float)CS$<>8__locals1.m.totalFramesCaptured);
		}
		return CS$<>8__locals1.m;
	}

	// Token: 0x0600AA68 RID: 43624 RVA: 0x003FA4B0 File Offset: 0x003F86B0
	[PublicizedFrom(EAccessModifier.Private)]
	public static PerformanceProfiler.AggregateMetrics CalculateAggregateMetrics(List<PerformanceProfiler.PerformanceMetrics> runs)
	{
		PerformanceProfiler.AggregateMetrics agg = default(PerformanceProfiler.AggregateMetrics);
		if (runs.Count == 0)
		{
			return agg;
		}
		agg.runCount = runs.Count;
		agg.meanAverageFrameTime = runs.Average<PerformanceProfiler.PerformanceMetrics>((PerformanceProfiler.PerformanceMetrics r) => r.averageFrameTime);
		agg.meanAverageFPS = runs.Average<PerformanceProfiler.PerformanceMetrics>((PerformanceProfiler.PerformanceMetrics r) => r.averageFPS);
		agg.meanFivePercentLow = runs.Average<PerformanceProfiler.PerformanceMetrics>((PerformanceProfiler.PerformanceMetrics r) => r.fivePercentLow);
		agg.meanOnePercentLow = runs.Average<PerformanceProfiler.PerformanceMetrics>((PerformanceProfiler.PerformanceMetrics r) => r.onePercentLow);
		agg.meanPointOnePercentLow = runs.Average<PerformanceProfiler.PerformanceMetrics>((PerformanceProfiler.PerformanceMetrics r) => r.pointOnePercentLow);
		agg.meanFramePacingViolationPercent20 = runs.Average<PerformanceProfiler.PerformanceMetrics>((PerformanceProfiler.PerformanceMetrics r) => r.framePacingViolationPercent20);
		agg.meanFramePacingViolationPercent50 = runs.Average<PerformanceProfiler.PerformanceMetrics>((PerformanceProfiler.PerformanceMetrics r) => r.framePacingViolationPercent50);
		agg.meanMaxConsecutiveBadFrames = (float)runs.Average<PerformanceProfiler.PerformanceMetrics>((PerformanceProfiler.PerformanceMetrics r) => r.maxConsecutiveBadFrames);
		agg.meanSpikeEventCount = (float)runs.Average<PerformanceProfiler.PerformanceMetrics>((PerformanceProfiler.PerformanceMetrics r) => r.spikeEventCount);
		agg.meanGcAttributedSpikePercent = runs.Average<PerformanceProfiler.PerformanceMetrics>((PerformanceProfiler.PerformanceMetrics r) => r.gcAttributedSpikePercent);
		agg.meanTimeBetweenSpikesSec = runs.Average<PerformanceProfiler.PerformanceMetrics>((PerformanceProfiler.PerformanceMetrics r) => r.meanTimeBetweenSpikesSec);
		agg.meanEstimatedGcCollectionCount = (float)runs.Average<PerformanceProfiler.PerformanceMetrics>((PerformanceProfiler.PerformanceMetrics r) => r.estimatedGcCollectionCount);
		agg.meanGcIntervalSec = runs.Average<PerformanceProfiler.PerformanceMetrics>((PerformanceProfiler.PerformanceMetrics r) => r.meanGcIntervalSec);
		agg.totalFramesCaptured = runs.Sum<PerformanceProfiler.PerformanceMetrics>((PerformanceProfiler.PerformanceMetrics r) => r.totalFramesCaptured);
		agg.totalCaptureDurationSeconds = runs.Sum<PerformanceProfiler.PerformanceMetrics>((PerformanceProfiler.PerformanceMetrics r) => r.captureDurationSeconds);
		agg.bestRunAverageFPS = runs.Max<PerformanceProfiler.PerformanceMetrics>((PerformanceProfiler.PerformanceMetrics r) => r.averageFPS);
		agg.worstRunAverageFPS = runs.Min<PerformanceProfiler.PerformanceMetrics>((PerformanceProfiler.PerformanceMetrics r) => r.averageFPS);
		float num = runs.Sum<PerformanceProfiler.PerformanceMetrics>((PerformanceProfiler.PerformanceMetrics r) => (r.averageFPS - agg.meanAverageFPS) * (r.averageFPS - agg.meanAverageFPS));
		float num2 = runs.Sum<PerformanceProfiler.PerformanceMetrics>((PerformanceProfiler.PerformanceMetrics r) => (r.onePercentLow - agg.meanOnePercentLow) * (r.onePercentLow - agg.meanOnePercentLow));
		agg.stdDevAverageFPS = Mathf.Sqrt(num / (float)runs.Count);
		agg.stdDevOnePercentLow = Mathf.Sqrt(num2 / (float)runs.Count);
		return agg;
	}

	// Token: 0x0600AA69 RID: 43625 RVA: 0x003FA880 File Offset: 0x003F8A80
	[PublicizedFrom(EAccessModifier.Private)]
	public static float GetPercentile(List<float> sorted, float percentile)
	{
		if (sorted.Count == 0)
		{
			return 0f;
		}
		if (sorted.Count == 1)
		{
			return sorted[0];
		}
		float num = percentile / 100f * (float)(sorted.Count - 1);
		int num2 = (int)num;
		float num3 = num - (float)num2;
		if (num2 + 1 >= sorted.Count)
		{
			return sorted[num2];
		}
		return sorted[num2] * (1f - num3) + sorted[num2 + 1] * num3;
	}

	// Token: 0x0600AA6A RID: 43626 RVA: 0x003FA8F4 File Offset: 0x003F8AF4
	[PublicizedFrom(EAccessModifier.Private)]
	public static string BuildBasePath(string directory)
	{
		string text = GameIO.GetPostTerminationAccessiblePath();
		text = text + "PerformanceCaptures/" + directory + "/";
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		return text;
	}

	// Token: 0x0600AA6B RID: 43627 RVA: 0x003FA92C File Offset: 0x003F8B2C
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool TryCreateProfilerRecorder()
	{
		foreach (string text in new string[] { "Main Thread", "CPU Frame Time", "Frame Time", "CPU Usage" })
		{
			try
			{
				PerformanceProfiler.frameTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Internal, text, 15, ProfilerRecorderOptions.Default);
				if (PerformanceProfiler.frameTimeRecorder.Valid)
				{
					Debug.Log("[PerformanceProfiler] Using recorder: " + text);
					return true;
				}
				PerformanceProfiler.frameTimeRecorder.Dispose();
			}
			catch
			{
			}
		}
		return false;
	}

	// Token: 0x0600AA6C RID: 43628 RVA: 0x003FA9C8 File Offset: 0x003F8BC8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SaveRawData(string filepath)
	{
		try
		{
			using (StreamWriter streamWriter = new StreamWriter(filepath))
			{
				streamWriter.WriteLine("Frame,FrameTime(ms),SystemTextureMem,SystemTextureMemDelta,TexStreamCurrent(MB),TexStreamTarget(MB),TexStreamDesired(MB),TexStreamBudget(MB),GCHeap(MB),RSS(MB),Chunks,DisplayedCGOs,Enemies,Entities,EntityInstances,Items,TimeSinceLevelLoad(min)");
				for (int i = 0; i < PerformanceProfiler.frameTimeData.Count; i++)
				{
					PerformanceProfiler.FrameInfo frameInfo = PerformanceProfiler.frameExtraData[i];
					streamWriter.WriteLine(string.Concat(new string[]
					{
						string.Format("{0},{1:F3},", i + 1, PerformanceProfiler.frameTimeData[i]),
						string.Format("{0},{1},", frameInfo.systemTextureMemory, frameInfo.systemTextureMemoryDelta),
						string.Format("{0:F2},{1:F2},{2:F2},{3:F2},", new object[] { frameInfo.texStreamCurrentMB, frameInfo.texStreamTargetMB, frameInfo.texStreamDesiredMB, frameInfo.texStreamBudgetMB }),
						string.Format("{0:F2},{1:F2},", frameInfo.gcHeapMemoryMB, frameInfo.rssMemoryMB),
						string.Format("{0},{1},", frameInfo.chunkCount, frameInfo.displayedChunkObjects),
						string.Format("{0},{1},{2},{3},", new object[] { frameInfo.enemyCount, frameInfo.entityCount, frameInfo.entityInstanceCount, frameInfo.itemCount }),
						string.Format("{0:F4}", frameInfo.timeSinceLevelLoadMin)
					}));
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("[PerformanceProfiler] Failed to save raw data: " + ex.Message);
		}
	}

	// Token: 0x0600AA6D RID: 43629 RVA: 0x003FABB8 File Offset: 0x003F8DB8
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SaveSpikeEvents(string filepath, List<PerformanceProfiler.SpikeEvent> spikes)
	{
		try
		{
			using (StreamWriter streamWriter = new StreamWriter(filepath))
			{
				streamWriter.WriteLine("SpikeIndex,StartFrame,DurationFrames,TotalDurationMs,PeakFrameTimeMs,BaselineFrameTimeMs,PeakMultiple,IsGcAttributed,GcHeapDropMB");
				for (int i = 0; i < spikes.Count; i++)
				{
					PerformanceProfiler.SpikeEvent spikeEvent = spikes[i];
					streamWriter.WriteLine(string.Format("{0},{1},{2},{3:F3},", new object[]
					{
						i + 1,
						spikeEvent.startFrame,
						spikeEvent.durationFrames,
						spikeEvent.totalDurationMs
					}) + string.Format("{0:F3},{1:F3},{2:F2},", spikeEvent.peakFrameTimeMs, spikeEvent.baselineFrameTimeMs, spikeEvent.peakMultiple) + string.Format("{0},{1:F3}", spikeEvent.isGcAttributed, spikeEvent.gcHeapDropMB));
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("[PerformanceProfiler] Failed to save spike data: " + ex.Message);
		}
	}

	// Token: 0x0600AA6E RID: 43630 RVA: 0x003FACD4 File Offset: 0x003F8ED4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SaveMemorySnapshot(string filepath)
	{
		if (PerformanceProfiler.memoryProfiler == null)
		{
			return;
		}
		try
		{
			string lastValueCsv = PerformanceProfiler.memoryProfiler.GetLastValueCsv();
			using (StreamWriter streamWriter = new StreamWriter(filepath))
			{
				streamWriter.WriteLine("Snapshot," + PerformanceProfiler.memoryProfiler.GetCsvHeader());
				if (PerformanceProfiler.memorySnapshotStart != null)
				{
					streamWriter.WriteLine("Before," + PerformanceProfiler.memorySnapshotStart);
				}
				streamWriter.WriteLine("After," + lastValueCsv);
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("[PerformanceProfiler] Failed to save memory snapshot: " + ex.Message);
		}
	}

	// Token: 0x0600AA6F RID: 43631 RVA: 0x003FAD88 File Offset: 0x003F8F88
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SaveAnalysis(string filepath, PerformanceProfiler.PerformanceMetrics m)
	{
		try
		{
			using (StreamWriter streamWriter = new StreamWriter(filepath))
			{
				streamWriter.WriteLine("Metric,Value");
				streamWriter.WriteLine(string.Format("AverageFrameTime(ms),{0:F3}", m.averageFrameTime));
				streamWriter.WriteLine(string.Format("AverageFPS,{0:F2}", m.averageFPS));
				streamWriter.WriteLine(string.Format("MinFrameTime(ms),{0:F3}", m.minFrameTime));
				streamWriter.WriteLine(string.Format("MaxFrameTime(ms),{0:F3}", m.maxFrameTime));
				streamWriter.WriteLine(string.Format("5%Low(ms),{0:F3}", m.fivePercentLow));
				streamWriter.WriteLine(string.Format("1%Low(ms),{0:F3}", m.onePercentLow));
				streamWriter.WriteLine(string.Format("0.1%Low(ms),{0:F3}", m.pointOnePercentLow));
				streamWriter.WriteLine(string.Format("StandardDeviation(ms),{0:F3}", m.standardDeviation));
				streamWriter.WriteLine(string.Format("CoefficientOfVariation,{0:F4}", m.coefficientOfVariation));
				streamWriter.WriteLine(string.Format("FramePacingViolations(>20%),{0}", m.framePacingViolations20));
				streamWriter.WriteLine(string.Format("FramePacingViolations(>50%),{0}", m.framePacingViolations50));
				streamWriter.WriteLine(string.Format("FramePacingViolationPercent(>20%),{0:F2}", m.framePacingViolationPercent20));
				streamWriter.WriteLine(string.Format("FramePacingViolationPercent(>50%),{0:F2}", m.framePacingViolationPercent50));
				streamWriter.WriteLine(string.Format("MaxConsecutiveBadFrames,{0}", m.maxConsecutiveBadFrames));
				streamWriter.WriteLine(string.Format("SpikeEventCount,{0}", m.spikeEventCount));
				streamWriter.WriteLine(string.Format("GcAttributedSpikeCount,{0}", m.gcAttributedSpikeCount));
				streamWriter.WriteLine(string.Format("NonGcAttributedSpikeCount,{0}", m.nonGcAttributedSpikeCount));
				streamWriter.WriteLine(string.Format("GcAttributedSpikePercent,{0:F1}", m.gcAttributedSpikePercent));
				streamWriter.WriteLine(string.Format("MeanSpikeDurationMs,{0:F3}", m.meanSpikeDurationMs));
				streamWriter.WriteLine(string.Format("MaxSpikeDurationMs,{0:F3}", m.maxSpikeDurationMs));
				streamWriter.WriteLine(string.Format("MeanTimeBetweenSpikesSec,{0:F2}", m.meanTimeBetweenSpikesSec));
				streamWriter.WriteLine(string.Format("MeanGcSpikeMs,{0:F3}", m.meanGcSpikeMs));
				streamWriter.WriteLine(string.Format("MeanNonGcSpikeMs,{0:F3}", m.meanNonGcSpikeMs));
				streamWriter.WriteLine(string.Format("EstimatedGcCollectionCount,{0}", m.estimatedGcCollectionCount));
				streamWriter.WriteLine(string.Format("MeanGcIntervalSec,{0:F2}", m.meanGcIntervalSec));
				streamWriter.WriteLine(string.Format("TargetFramerate,{0}", PerformanceProfiler.targetFramerate));
				streamWriter.WriteLine(string.Format("TotalFramesCaptured,{0}", m.totalFramesCaptured));
				streamWriter.WriteLine(string.Format("CaptureDuration(s),{0:F2}", m.captureDurationSeconds));
				streamWriter.WriteLine(string.Format("[Config]SpikeBaselineHalfWindow,{0}", 30));
				streamWriter.WriteLine(string.Format("[Config]SpikeThresholdMultiplier,{0}", 2f));
				streamWriter.WriteLine(string.Format("[Config]SpikeMergeGapFrames,{0}", 3));
				streamWriter.WriteLine(string.Format("[Config]GcAttributionHeapDropMB,{0}", 0.5f));
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("[PerformanceProfiler] Failed to save analysis: " + ex.Message);
		}
	}

	// Token: 0x0600AA70 RID: 43632 RVA: 0x003FB158 File Offset: 0x003F9358
	[PublicizedFrom(EAccessModifier.Private)]
	public static void SaveAggregateAnalysis(string filepath, PerformanceProfiler.AggregateMetrics agg)
	{
		try
		{
			using (StreamWriter streamWriter = new StreamWriter(filepath))
			{
				streamWriter.WriteLine("Metric,Value");
				streamWriter.WriteLine(string.Format("RunCount,{0}", agg.runCount));
				streamWriter.WriteLine(string.Format("MeanAverageFrameTime(ms),{0:F3}", agg.meanAverageFrameTime));
				streamWriter.WriteLine(string.Format("MeanAverageFPS,{0:F2}", agg.meanAverageFPS));
				streamWriter.WriteLine(string.Format("StdDevAverageFPS,{0:F3}", agg.stdDevAverageFPS));
				streamWriter.WriteLine(string.Format("MeanFivePercentLow(ms),{0:F3}", agg.meanFivePercentLow));
				streamWriter.WriteLine(string.Format("MeanOnePercentLow(ms),{0:F3}", agg.meanOnePercentLow));
				streamWriter.WriteLine(string.Format("StdDevOnePercentLow(ms),{0:F3}", agg.stdDevOnePercentLow));
				streamWriter.WriteLine(string.Format("MeanPointOnePercentLow(ms),{0:F3}", agg.meanPointOnePercentLow));
				streamWriter.WriteLine(string.Format("BestRunAverageFPS,{0:F2}", agg.bestRunAverageFPS));
				streamWriter.WriteLine(string.Format("WorstRunAverageFPS,{0:F2}", agg.worstRunAverageFPS));
				streamWriter.WriteLine(string.Format("MeanFramePacingViolationPercent(>20%),{0:F2}", agg.meanFramePacingViolationPercent20));
				streamWriter.WriteLine(string.Format("MeanFramePacingViolationPercent(>50%),{0:F2}", agg.meanFramePacingViolationPercent50));
				streamWriter.WriteLine(string.Format("MeanMaxConsecutiveBadFrames,{0:F1}", agg.meanMaxConsecutiveBadFrames));
				streamWriter.WriteLine(string.Format("MeanSpikeEventCount,{0:F1}", agg.meanSpikeEventCount));
				streamWriter.WriteLine(string.Format("MeanGcAttributedSpikePercent,{0:F1}", agg.meanGcAttributedSpikePercent));
				streamWriter.WriteLine(string.Format("MeanTimeBetweenSpikesSec,{0:F2}", agg.meanTimeBetweenSpikesSec));
				streamWriter.WriteLine(string.Format("MeanEstimatedGcCollectionCount,{0:F1}", agg.meanEstimatedGcCollectionCount));
				streamWriter.WriteLine(string.Format("MeanGcIntervalSec,{0:F2}", agg.meanGcIntervalSec));
				streamWriter.WriteLine(string.Format("TotalFramesCaptured,{0}", agg.totalFramesCaptured));
				streamWriter.WriteLine(string.Format("TotalCaptureDuration(s),{0:F2}", agg.totalCaptureDurationSeconds));
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("[PerformanceProfiler] Failed to save aggregate: " + ex.Message);
		}
	}

	// Token: 0x0600AA71 RID: 43633 RVA: 0x003FB3F0 File Offset: 0x003F95F0
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogSummary(PerformanceProfiler.PerformanceMetrics m)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("=== Performance Run Summary ===");
		stringBuilder.AppendLine(string.Format("  Average    : {0:F2}ms  ({1:F1} FPS)", m.averageFrameTime, m.averageFPS));
		stringBuilder.AppendLine(string.Format("  5% Low     : {0:F2}ms  ({1:F1} FPS)", m.fivePercentLow, 1000f / m.fivePercentLow));
		stringBuilder.AppendLine(string.Format("  1% Low     : {0:F2}ms  ({1:F1} FPS)", m.onePercentLow, 1000f / m.onePercentLow));
		stringBuilder.AppendLine(string.Format("  0.1% Low   : {0:F2}ms  ({1:F1} FPS)", m.pointOnePercentLow, 1000f / m.pointOnePercentLow));
		stringBuilder.AppendLine(string.Format("  Range      : {0:F2}ms – {1:F2}ms", m.minFrameTime, m.maxFrameTime));
		stringBuilder.AppendLine(string.Format("  Std Dev    : {0:F2}ms  (CV: {1:F3})", m.standardDeviation, m.coefficientOfVariation));
		stringBuilder.AppendLine(string.Format("  Pacing     : {0:F1}% frames >20% over target, max {1} consecutive", m.framePacingViolationPercent20, m.maxConsecutiveBadFrames));
		stringBuilder.AppendLine(string.Format("  Spikes     : {0} events  |  GC-attributed: {1} ({2:F0}%)  |  other: {3}", new object[] { m.spikeEventCount, m.gcAttributedSpikeCount, m.gcAttributedSpikePercent, m.nonGcAttributedSpikeCount }));
		if (m.spikeEventCount > 0)
		{
			stringBuilder.AppendLine(string.Format("  Spike dur  : mean {0:F1}ms  max {1:F1}ms", m.meanSpikeDurationMs, m.maxSpikeDurationMs) + ((m.spikeEventCount >= 2) ? string.Format("  every {0:F1}s", m.meanTimeBetweenSpikesSec) : ""));
			if (m.gcAttributedSpikeCount > 0)
			{
				stringBuilder.AppendLine(string.Format("  GC spikes  : mean {0:F1}ms  interval {1:F1}s  ({2} est. collections)", m.meanGcSpikeMs, m.meanGcIntervalSec, m.estimatedGcCollectionCount));
			}
			if (m.nonGcAttributedSpikeCount > 0)
			{
				stringBuilder.AppendLine(string.Format("  Other spike: mean {0:F1}ms", m.meanNonGcSpikeMs));
			}
		}
		Debug.Log(stringBuilder.ToString());
	}

	// Token: 0x0600AA72 RID: 43634 RVA: 0x003FB64C File Offset: 0x003F984C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void LogAggregateSummary(PerformanceProfiler.AggregateMetrics agg)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(string.Format("=== Session Aggregate Summary ({0} runs) ===", agg.runCount));
		stringBuilder.AppendLine(string.Format("  Mean FPS          : {0:F1}  (±{1:F2})", agg.meanAverageFPS, agg.stdDevAverageFPS));
		stringBuilder.AppendLine(string.Format("  Mean 1% Low       : {0:F2}ms  (±{1:F2})", agg.meanOnePercentLow, agg.stdDevOnePercentLow));
		stringBuilder.AppendLine(string.Format("  Best / Worst      : {0:F1} FPS / {1:F1} FPS", agg.bestRunAverageFPS, agg.worstRunAverageFPS));
		stringBuilder.AppendLine(string.Format("  Mean spikes/run   : {0:F1}  |  GC-attributed: {1:F0}%", agg.meanSpikeEventCount, agg.meanGcAttributedSpikePercent));
		stringBuilder.AppendLine(string.Format("  Mean spike freq   : every {0:F1}s", agg.meanTimeBetweenSpikesSec));
		stringBuilder.AppendLine(string.Format("  Mean GC interval  : {0:F1}s  ({1:F1} collections/run)", agg.meanGcIntervalSec, agg.meanEstimatedGcCollectionCount));
		stringBuilder.AppendLine(string.Format("  Mean pacing       : {0:F1}% frames >20% over target", agg.meanFramePacingViolationPercent20));
		stringBuilder.AppendLine(string.Format("  Total frames      : {0}  over {1:F1}s", agg.totalFramesCaptured, agg.totalCaptureDurationSeconds));
		Debug.Log(stringBuilder.ToString());
	}

	// Token: 0x04007E2F RID: 32303
	[PublicizedFrom(EAccessModifier.Private)]
	public const int SpikeBaselineHalfWindow = 30;

	// Token: 0x04007E30 RID: 32304
	[PublicizedFrom(EAccessModifier.Private)]
	public const float SpikeThresholdMultiplier = 2f;

	// Token: 0x04007E31 RID: 32305
	[PublicizedFrom(EAccessModifier.Private)]
	public const int SpikeMergeGapFrames = 3;

	// Token: 0x04007E32 RID: 32306
	[PublicizedFrom(EAccessModifier.Private)]
	public const float GcAttributionHeapDropMB = 0.5f;

	// Token: 0x04007E33 RID: 32307
	[PublicizedFrom(EAccessModifier.Private)]
	public const int GcAttributionWindowFrames = 1;

	// Token: 0x04007E36 RID: 32310
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool isCapturing = false;

	// Token: 0x04007E37 RID: 32311
	[PublicizedFrom(EAccessModifier.Private)]
	public static string profilePrefix = null;

	// Token: 0x04007E38 RID: 32312
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<float> frameTimeData = new List<float>();

	// Token: 0x04007E39 RID: 32313
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<PerformanceProfiler.FrameInfo> frameExtraData = new List<PerformanceProfiler.FrameInfo>();

	// Token: 0x04007E3A RID: 32314
	[PublicizedFrom(EAccessModifier.Private)]
	public static ProfilerRecorder frameTimeRecorder;

	// Token: 0x04007E3B RID: 32315
	[PublicizedFrom(EAccessModifier.Private)]
	public static DateTime captureStartTime;

	// Token: 0x04007E3C RID: 32316
	[PublicizedFrom(EAccessModifier.Private)]
	public static int targetFramerate = 30;

	// Token: 0x04007E3D RID: 32317
	[PublicizedFrom(EAccessModifier.Private)]
	public static float lastFrameTime = 0f;

	// Token: 0x04007E3E RID: 32318
	[PublicizedFrom(EAccessModifier.Private)]
	public static PerformanceProfiler.FrameTimeCapture captureComponent;

	// Token: 0x04007E3F RID: 32319
	[PublicizedFrom(EAccessModifier.Private)]
	public static string captureDirectory;

	// Token: 0x04007E40 RID: 32320
	[PublicizedFrom(EAccessModifier.Private)]
	public static ProfilingMetricCapture memoryProfiler;

	// Token: 0x04007E41 RID: 32321
	[PublicizedFrom(EAccessModifier.Private)]
	public static string memorySnapshotStart;

	// Token: 0x04007E42 RID: 32322
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<PerformanceProfiler.SpikeEvent> lastRunSpikeEvents = new List<PerformanceProfiler.SpikeEvent>();

	// Token: 0x04007E43 RID: 32323
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool isInSession = false;

	// Token: 0x04007E44 RID: 32324
	[PublicizedFrom(EAccessModifier.Private)]
	public static int sessionRunTarget = 0;

	// Token: 0x04007E45 RID: 32325
	[PublicizedFrom(EAccessModifier.Private)]
	public static int sessionRunsCompleted = 0;

	// Token: 0x04007E46 RID: 32326
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<PerformanceProfiler.PerformanceMetrics> sessionRunMetrics = new List<PerformanceProfiler.PerformanceMetrics>();

	// Token: 0x04007E47 RID: 32327
	[PublicizedFrom(EAccessModifier.Private)]
	public static string sessionDirectory = null;

	// Token: 0x04007E48 RID: 32328
	[PublicizedFrom(EAccessModifier.Private)]
	public static string sessionPrefix = null;

	// Token: 0x04007E49 RID: 32329
	[PublicizedFrom(EAccessModifier.Private)]
	public static long lastSystemTextureMemory = 0L;

	// Token: 0x02001534 RID: 5428
	public struct FrameInfo
	{
		// Token: 0x04007E4A RID: 32330
		public long systemTextureMemory;

		// Token: 0x04007E4B RID: 32331
		public long systemTextureMemoryDelta;

		// Token: 0x04007E4C RID: 32332
		public float texStreamCurrentMB;

		// Token: 0x04007E4D RID: 32333
		public float texStreamTargetMB;

		// Token: 0x04007E4E RID: 32334
		public float texStreamDesiredMB;

		// Token: 0x04007E4F RID: 32335
		public float texStreamBudgetMB;

		// Token: 0x04007E50 RID: 32336
		public float gcHeapMemoryMB;

		// Token: 0x04007E51 RID: 32337
		public float rssMemoryMB;

		// Token: 0x04007E52 RID: 32338
		public int chunkCount;

		// Token: 0x04007E53 RID: 32339
		public int displayedChunkObjects;

		// Token: 0x04007E54 RID: 32340
		public int enemyCount;

		// Token: 0x04007E55 RID: 32341
		public int entityCount;

		// Token: 0x04007E56 RID: 32342
		public int entityInstanceCount;

		// Token: 0x04007E57 RID: 32343
		public int itemCount;

		// Token: 0x04007E58 RID: 32344
		public float timeSinceLevelLoadMin;
	}

	// Token: 0x02001535 RID: 5429
	public struct SpikeEvent
	{
		// Token: 0x04007E59 RID: 32345
		public int startFrame;

		// Token: 0x04007E5A RID: 32346
		public int durationFrames;

		// Token: 0x04007E5B RID: 32347
		public float totalDurationMs;

		// Token: 0x04007E5C RID: 32348
		public float peakFrameTimeMs;

		// Token: 0x04007E5D RID: 32349
		public float baselineFrameTimeMs;

		// Token: 0x04007E5E RID: 32350
		public float peakMultiple;

		// Token: 0x04007E5F RID: 32351
		public bool isGcAttributed;

		// Token: 0x04007E60 RID: 32352
		public float gcHeapDropMB;
	}

	// Token: 0x02001536 RID: 5430
	public struct PerformanceMetrics
	{
		// Token: 0x04007E61 RID: 32353
		public float averageFrameTime;

		// Token: 0x04007E62 RID: 32354
		public float averageFPS;

		// Token: 0x04007E63 RID: 32355
		public float onePercentLow;

		// Token: 0x04007E64 RID: 32356
		public float pointOnePercentLow;

		// Token: 0x04007E65 RID: 32357
		public float fivePercentLow;

		// Token: 0x04007E66 RID: 32358
		public float maxFrameTime;

		// Token: 0x04007E67 RID: 32359
		public float minFrameTime;

		// Token: 0x04007E68 RID: 32360
		public float standardDeviation;

		// Token: 0x04007E69 RID: 32361
		public float coefficientOfVariation;

		// Token: 0x04007E6A RID: 32362
		public int framePacingViolations20;

		// Token: 0x04007E6B RID: 32363
		public int framePacingViolations50;

		// Token: 0x04007E6C RID: 32364
		public float framePacingViolationPercent20;

		// Token: 0x04007E6D RID: 32365
		public float framePacingViolationPercent50;

		// Token: 0x04007E6E RID: 32366
		public int maxConsecutiveBadFrames;

		// Token: 0x04007E6F RID: 32367
		public int spikeEventCount;

		// Token: 0x04007E70 RID: 32368
		public int gcAttributedSpikeCount;

		// Token: 0x04007E71 RID: 32369
		public int nonGcAttributedSpikeCount;

		// Token: 0x04007E72 RID: 32370
		public float gcAttributedSpikePercent;

		// Token: 0x04007E73 RID: 32371
		public float meanSpikeDurationMs;

		// Token: 0x04007E74 RID: 32372
		public float maxSpikeDurationMs;

		// Token: 0x04007E75 RID: 32373
		public float meanTimeBetweenSpikesSec;

		// Token: 0x04007E76 RID: 32374
		public float meanGcSpikeMs;

		// Token: 0x04007E77 RID: 32375
		public float meanNonGcSpikeMs;

		// Token: 0x04007E78 RID: 32376
		public int estimatedGcCollectionCount;

		// Token: 0x04007E79 RID: 32377
		public float meanGcIntervalSec;

		// Token: 0x04007E7A RID: 32378
		public int totalFramesCaptured;

		// Token: 0x04007E7B RID: 32379
		public float captureDurationSeconds;
	}

	// Token: 0x02001537 RID: 5431
	public struct AggregateMetrics
	{
		// Token: 0x04007E7C RID: 32380
		public int runCount;

		// Token: 0x04007E7D RID: 32381
		public float meanAverageFrameTime;

		// Token: 0x04007E7E RID: 32382
		public float meanAverageFPS;

		// Token: 0x04007E7F RID: 32383
		public float stdDevAverageFPS;

		// Token: 0x04007E80 RID: 32384
		public float meanOnePercentLow;

		// Token: 0x04007E81 RID: 32385
		public float stdDevOnePercentLow;

		// Token: 0x04007E82 RID: 32386
		public float meanPointOnePercentLow;

		// Token: 0x04007E83 RID: 32387
		public float meanFivePercentLow;

		// Token: 0x04007E84 RID: 32388
		public float meanFramePacingViolationPercent20;

		// Token: 0x04007E85 RID: 32389
		public float meanFramePacingViolationPercent50;

		// Token: 0x04007E86 RID: 32390
		public float meanMaxConsecutiveBadFrames;

		// Token: 0x04007E87 RID: 32391
		public float meanSpikeEventCount;

		// Token: 0x04007E88 RID: 32392
		public float meanGcAttributedSpikePercent;

		// Token: 0x04007E89 RID: 32393
		public float meanTimeBetweenSpikesSec;

		// Token: 0x04007E8A RID: 32394
		public float meanEstimatedGcCollectionCount;

		// Token: 0x04007E8B RID: 32395
		public float meanGcIntervalSec;

		// Token: 0x04007E8C RID: 32396
		public float bestRunAverageFPS;

		// Token: 0x04007E8D RID: 32397
		public float worstRunAverageFPS;

		// Token: 0x04007E8E RID: 32398
		public int totalFramesCaptured;

		// Token: 0x04007E8F RID: 32399
		public float totalCaptureDurationSeconds;
	}

	// Token: 0x02001538 RID: 5432
	[PublicizedFrom(EAccessModifier.Private)]
	public class FrameTimeCapture : MonoBehaviour
	{
		// Token: 0x0600AA74 RID: 43636 RVA: 0x003FB820 File Offset: 0x003F9A20
		[PublicizedFrom(EAccessModifier.Private)]
		public void LateUpdate()
		{
			if (!PerformanceProfiler.isCapturing)
			{
				global::UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			if (PerformanceProfiler.memorySnapshotStart == null)
			{
				this.lateUpdatesSeen++;
				if (this.lateUpdatesSeen == 2 && PerformanceProfiler.memoryProfiler != null)
				{
					PerformanceProfiler.memorySnapshotStart = PerformanceProfiler.memoryProfiler.GetLastValueCsv();
				}
			}
			PerformanceProfiler.CaptureFrameTime();
		}

		// Token: 0x04007E90 RID: 32400
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public int lateUpdatesSeen;
	}
}
