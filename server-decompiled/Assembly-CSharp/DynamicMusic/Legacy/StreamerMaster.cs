using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001AC3 RID: 6851
	public class StreamerMaster
	{
		// Token: 0x170019EF RID: 6639
		// (get) Token: 0x0600CEC7 RID: 52935 RVA: 0x004AFE5B File Offset: 0x004AE05B
		public bool IsReplacementNecessary
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return StreamerMaster.currentStreamer == null || (StreamerMaster.currentStreamer.HasReachedLastHyperbar && !StreamerMaster.currentStreamer.IsPlaying);
			}
		}

		// Token: 0x0600CEC8 RID: 52936 RVA: 0x004AFE81 File Offset: 0x004AE081
		public static StreamerMaster Create()
		{
			return new StreamerMaster();
		}

		// Token: 0x0600CEC9 RID: 52937 RVA: 0x004AFE88 File Offset: 0x004AE088
		public static void Init(DynamicMusicManager _dynamicMusicManager)
		{
			_dynamicMusicManager.StreamerMaster = StreamerMaster.Create();
			_dynamicMusicManager.StreamerMaster.dynamicMusicManager = _dynamicMusicManager;
			StreamerMaster.Streamers = new EnumDictionary<ThreatLevelLegacyType, Queue<ThreatLevelStreamer>>();
		}

		// Token: 0x0600CECA RID: 52938 RVA: 0x004AFEAC File Offset: 0x004AE0AC
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

		// Token: 0x0600CECB RID: 52939 RVA: 0x004AFF20 File Offset: 0x004AE120
		public void Play()
		{
			if (StreamerMaster.currentStreamer != null)
			{
				StreamerMaster.currentStreamer.Play();
			}
		}

		// Token: 0x0600CECC RID: 52940 RVA: 0x004AFF33 File Offset: 0x004AE133
		public void Pause()
		{
			if (StreamerMaster.currentStreamer != null)
			{
				StreamerMaster.currentStreamer.Pause();
			}
		}

		// Token: 0x0600CECD RID: 52941 RVA: 0x004AFF46 File Offset: 0x004AE146
		public void UnPause()
		{
			if (StreamerMaster.currentStreamer != null)
			{
				StreamerMaster.currentStreamer.UnPause();
			}
		}

		// Token: 0x0600CECE RID: 52942 RVA: 0x004AFF59 File Offset: 0x004AE159
		public void Stop()
		{
			if (StreamerMaster.currentStreamer != null && (StreamerMaster.currentStreamer.IsPlaying || StreamerMaster.currentStreamer.IsPaused))
			{
				StreamerMaster.currentStreamer.Stop();
				this.ReplaceCurrentStreamer();
			}
		}

		// Token: 0x0600CECF RID: 52943 RVA: 0x004AFF8A File Offset: 0x004AE18A
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

		// Token: 0x0600CED0 RID: 52944 RVA: 0x004AFFBC File Offset: 0x004AE1BC
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

		// Token: 0x04009C76 RID: 40054
		[PublicizedFrom(EAccessModifier.Private)]
		public static EnumDictionary<ThreatLevelLegacyType, Queue<ThreatLevelStreamer>> Streamers;

		// Token: 0x04009C77 RID: 40055
		[PublicizedFrom(EAccessModifier.Private)]
		public DynamicMusicManager dynamicMusicManager;

		// Token: 0x04009C78 RID: 40056
		public static ThreatLevelStreamer currentStreamer;
	}
}
