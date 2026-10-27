using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001AF4 RID: 6900
	public class StreamerMaster
	{
		// Token: 0x17001A37 RID: 6711
		// (get) Token: 0x0600CFDB RID: 53211 RVA: 0x004B3957 File Offset: 0x004B1B57
		public bool IsReplacementNecessary
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return StreamerMaster.currentStreamer == null || (StreamerMaster.currentStreamer.HasReachedLastHyperbar && !StreamerMaster.currentStreamer.IsPlaying);
			}
		}

		// Token: 0x0600CFDC RID: 53212 RVA: 0x004B397D File Offset: 0x004B1B7D
		public static StreamerMaster Create()
		{
			return new StreamerMaster();
		}

		// Token: 0x0600CFDD RID: 53213 RVA: 0x004B3984 File Offset: 0x004B1B84
		public static void Init(DynamicMusicManager _dynamicMusicManager)
		{
			_dynamicMusicManager.StreamerMaster = StreamerMaster.Create();
			_dynamicMusicManager.StreamerMaster.dynamicMusicManager = _dynamicMusicManager;
			StreamerMaster.Streamers = new EnumDictionary<ThreatLevelLegacyType, Queue<ThreatLevelStreamer>>();
		}

		// Token: 0x0600CFDE RID: 53214 RVA: 0x004B39A8 File Offset: 0x004B1BA8
		public void Tick()
		{
			LayerReserve.Tick();
			if (StreamerMaster.currentStreamer != null)
			{
				StreamerMaster.currentStreamer.Tick();
			}
			if (this.IsReplacementNecessary)
			{
				Log.Out("Getting new currentStreamer!");
				this.ReplaceCurrentStreamer();
			}
			if (!this.dynamicMusicManager.IsInDeadWindow)
			{
				if (this.dynamicMusicManager.FrequencyManager.CanScheduleTrack && this.dynamicMusicManager.IsPlayAllowed)
				{
					this.Play();
					return;
				}
			}
			else
			{
				this.Stop();
			}
		}

		// Token: 0x0600CFDF RID: 53215 RVA: 0x004B3A1C File Offset: 0x004B1C1C
		public void Play()
		{
			if (StreamerMaster.currentStreamer != null)
			{
				StreamerMaster.currentStreamer.Play();
			}
		}

		// Token: 0x0600CFE0 RID: 53216 RVA: 0x004B3A2F File Offset: 0x004B1C2F
		public void Pause()
		{
			if (StreamerMaster.currentStreamer != null)
			{
				StreamerMaster.currentStreamer.Pause();
			}
		}

		// Token: 0x0600CFE1 RID: 53217 RVA: 0x004B3A42 File Offset: 0x004B1C42
		public void UnPause()
		{
			if (StreamerMaster.currentStreamer != null)
			{
				StreamerMaster.currentStreamer.UnPause();
			}
		}

		// Token: 0x0600CFE2 RID: 53218 RVA: 0x004B3A55 File Offset: 0x004B1C55
		public void Stop()
		{
			if (StreamerMaster.currentStreamer != null && (StreamerMaster.currentStreamer.IsPlaying || StreamerMaster.currentStreamer.IsPaused))
			{
				StreamerMaster.currentStreamer.Stop();
				this.ReplaceCurrentStreamer();
			}
		}

		// Token: 0x0600CFE3 RID: 53219 RVA: 0x004B3A86 File Offset: 0x004B1C86
		public void Cleanup()
		{
			if (StreamerMaster.currentStreamer != null)
			{
				StreamerMaster.currentStreamer.Cleanup();
				StreamerMaster.currentStreamer = null;
			}
			if (StreamerMaster.Streamers != null)
			{
				StreamerMaster.Streamers.Clear();
				StreamerMaster.Streamers = null;
			}
		}

		// Token: 0x0600CFE4 RID: 53220 RVA: 0x004B3AB8 File Offset: 0x004B1CB8
		public void ReplaceCurrentStreamer()
		{
			if (StreamerMaster.currentStreamer != null)
			{
				StreamerMaster.currentStreamer.Cleanup();
			}
			ThreatLevelStreamer threatLevelStreamer = ThreatLevelStreamer.Create(ThreatLevelLegacyType.Exploration);
			Queue<ThreatLevelStreamer> queue;
			if (!StreamerMaster.Streamers.TryGetValue(ThreatLevelLegacyType.Exploration, out queue))
			{
				StreamerMaster.Streamers.Add(ThreatLevelLegacyType.Exploration, queue = new Queue<ThreatLevelStreamer>());
				queue.Enqueue(ThreatLevelStreamer.Create(ThreatLevelLegacyType.Exploration));
				StreamerMaster.currentStreamer = threatLevelStreamer;
				return;
			}
			if (queue.Count > 0)
			{
				StreamerMaster.currentStreamer = queue.Dequeue();
				queue.Enqueue(threatLevelStreamer);
				return;
			}
			StreamerMaster.currentStreamer = threatLevelStreamer;
			queue.Enqueue(ThreatLevelStreamer.Create(ThreatLevelLegacyType.Exploration));
		}

		// Token: 0x04009D5F RID: 40287
		[PublicizedFrom(EAccessModifier.Private)]
		public static EnumDictionary<ThreatLevelLegacyType, Queue<ThreatLevelStreamer>> Streamers;

		// Token: 0x04009D60 RID: 40288
		[PublicizedFrom(EAccessModifier.Private)]
		public DynamicMusicManager dynamicMusicManager;

		// Token: 0x04009D61 RID: 40289
		public static ThreatLevelStreamer currentStreamer;
	}
}
