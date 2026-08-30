using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001A97 RID: 6807
	public class StreamerMaster
	{
		// Token: 0x1700194D RID: 6477
		// (get) Token: 0x0600CD6F RID: 52591 RVA: 0x004AB98F File Offset: 0x004A9B8F
		public bool IsReplacementNecessary
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return StreamerMaster.currentStreamer == null || (StreamerMaster.currentStreamer.HasReachedLastHyperbar && !StreamerMaster.currentStreamer.IsPlaying);
			}
		}

		// Token: 0x0600CD70 RID: 52592 RVA: 0x004AB9B5 File Offset: 0x004A9BB5
		public static StreamerMaster Create()
		{
			return new StreamerMaster();
		}

		// Token: 0x0600CD71 RID: 52593 RVA: 0x004AB9BC File Offset: 0x004A9BBC
		public static void Init(DynamicMusicManager _dynamicMusicManager)
		{
			_dynamicMusicManager.StreamerMaster = StreamerMaster.Create();
			_dynamicMusicManager.StreamerMaster.dynamicMusicManager = _dynamicMusicManager;
			StreamerMaster.Streamers = new EnumDictionary<ThreatLevelLegacyType, Queue<ThreatLevelStreamer>>();
		}

		// Token: 0x0600CD72 RID: 52594 RVA: 0x004AB9E0 File Offset: 0x004A9BE0
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

		// Token: 0x0600CD73 RID: 52595 RVA: 0x004ABA54 File Offset: 0x004A9C54
		public void Play()
		{
			if (StreamerMaster.currentStreamer != null)
			{
				StreamerMaster.currentStreamer.Play();
			}
		}

		// Token: 0x0600CD74 RID: 52596 RVA: 0x004ABA67 File Offset: 0x004A9C67
		public void Pause()
		{
			if (StreamerMaster.currentStreamer != null)
			{
				StreamerMaster.currentStreamer.Pause();
			}
		}

		// Token: 0x0600CD75 RID: 52597 RVA: 0x004ABA7A File Offset: 0x004A9C7A
		public void UnPause()
		{
			if (StreamerMaster.currentStreamer != null)
			{
				StreamerMaster.currentStreamer.UnPause();
			}
		}

		// Token: 0x0600CD76 RID: 52598 RVA: 0x004ABA8D File Offset: 0x004A9C8D
		public void Stop()
		{
			if (StreamerMaster.currentStreamer != null && (StreamerMaster.currentStreamer.IsPlaying || StreamerMaster.currentStreamer.IsPaused))
			{
				StreamerMaster.currentStreamer.Stop();
				this.ReplaceCurrentStreamer();
			}
		}

		// Token: 0x0600CD77 RID: 52599 RVA: 0x004ABABE File Offset: 0x004A9CBE
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

		// Token: 0x0600CD78 RID: 52600 RVA: 0x004ABAF0 File Offset: 0x004A9CF0
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

		// Token: 0x04009BB0 RID: 39856
		[PublicizedFrom(EAccessModifier.Private)]
		public static EnumDictionary<ThreatLevelLegacyType, Queue<ThreatLevelStreamer>> Streamers;

		// Token: 0x04009BB1 RID: 39857
		[PublicizedFrom(EAccessModifier.Private)]
		public DynamicMusicManager dynamicMusicManager;

		// Token: 0x04009BB2 RID: 39858
		public static ThreatLevelStreamer currentStreamer;
	}
}
