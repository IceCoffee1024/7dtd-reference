using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001A70 RID: 6768
	public class StreamerMaster
	{
		// Token: 0x170018DB RID: 6363
		// (get) Token: 0x0600CBE6 RID: 52198 RVA: 0x004A559B File Offset: 0x004A379B
		public bool IsReplacementNecessary
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return StreamerMaster.currentStreamer == null || (StreamerMaster.currentStreamer.HasReachedLastHyperbar && !StreamerMaster.currentStreamer.IsPlaying);
			}
		}

		// Token: 0x0600CBE7 RID: 52199 RVA: 0x004A55C1 File Offset: 0x004A37C1
		public static StreamerMaster Create()
		{
			return new StreamerMaster();
		}

		// Token: 0x0600CBE8 RID: 52200 RVA: 0x004A55C8 File Offset: 0x004A37C8
		public static void Init(DynamicMusicManager _dynamicMusicManager)
		{
			_dynamicMusicManager.StreamerMaster = StreamerMaster.Create();
			_dynamicMusicManager.StreamerMaster.dynamicMusicManager = _dynamicMusicManager;
			StreamerMaster.Streamers = new EnumDictionary<ThreatLevelLegacyType, Queue<ThreatLevelStreamer>>();
		}

		// Token: 0x0600CBE9 RID: 52201 RVA: 0x004A55EC File Offset: 0x004A37EC
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

		// Token: 0x0600CBEA RID: 52202 RVA: 0x004A5660 File Offset: 0x004A3860
		public void Play()
		{
			if (StreamerMaster.currentStreamer != null)
			{
				StreamerMaster.currentStreamer.Play();
			}
		}

		// Token: 0x0600CBEB RID: 52203 RVA: 0x004A5673 File Offset: 0x004A3873
		public void Pause()
		{
			if (StreamerMaster.currentStreamer != null)
			{
				StreamerMaster.currentStreamer.Pause();
			}
		}

		// Token: 0x0600CBEC RID: 52204 RVA: 0x004A5686 File Offset: 0x004A3886
		public void UnPause()
		{
			if (StreamerMaster.currentStreamer != null)
			{
				StreamerMaster.currentStreamer.UnPause();
			}
		}

		// Token: 0x0600CBED RID: 52205 RVA: 0x004A5699 File Offset: 0x004A3899
		public void Stop()
		{
			if (StreamerMaster.currentStreamer != null && (StreamerMaster.currentStreamer.IsPlaying || StreamerMaster.currentStreamer.IsPaused))
			{
				StreamerMaster.currentStreamer.Stop();
				this.ReplaceCurrentStreamer();
			}
		}

		// Token: 0x0600CBEE RID: 52206 RVA: 0x004A56CA File Offset: 0x004A38CA
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

		// Token: 0x0600CBEF RID: 52207 RVA: 0x004A56FC File Offset: 0x004A38FC
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

		// Token: 0x04009A7F RID: 39551
		[PublicizedFrom(EAccessModifier.Private)]
		public static EnumDictionary<ThreatLevelLegacyType, Queue<ThreatLevelStreamer>> Streamers;

		// Token: 0x04009A80 RID: 39552
		[PublicizedFrom(EAccessModifier.Private)]
		public DynamicMusicManager dynamicMusicManager;

		// Token: 0x04009A81 RID: 39553
		public static ThreatLevelStreamer currentStreamer;
	}
}
