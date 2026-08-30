using System;
using System.Collections.Generic;
using DynamicMusic.Legacy.ObjectModel;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001A98 RID: 6808
	public class ThreatLevelStreamer
	{
		// Token: 0x1700194E RID: 6478
		// (get) Token: 0x0600CD7A RID: 52602 RVA: 0x004ABB78 File Offset: 0x004A9D78
		public bool InitFinished
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				using (Dictionary<LayerType, LayerStreamer>.ValueCollection.Enumerator enumerator = this.LayerStreamers.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.InitFinished)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x1700194F RID: 6479
		// (get) Token: 0x0600CD7B RID: 52603 RVA: 0x004ABBD8 File Offset: 0x004A9DD8
		public bool HasReachedLastHyperbar
		{
			get
			{
				using (Dictionary<LayerType, LayerStreamer>.ValueCollection.Enumerator enumerator = this.LayerStreamers.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.HasReachedLastHyperbar)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x0600CD7C RID: 52604 RVA: 0x004ABC38 File Offset: 0x004A9E38
		public static ThreatLevelStreamer Create(ThreatLevelLegacyType _tl)
		{
			return new ThreatLevelStreamer(_tl);
		}

		// Token: 0x0600CD7D RID: 52605 RVA: 0x004ABC40 File Offset: 0x004A9E40
		public static ThreatLevelStreamer Create(ThreatLevelLegacyType _tl, ThreatLevel _groupTL, ThreatLevelConfig _config)
		{
			return new ThreatLevelStreamer(_tl, _groupTL, _config);
		}

		// Token: 0x0600CD7E RID: 52606 RVA: 0x004ABC4C File Offset: 0x004A9E4C
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreatLevelStreamer(ThreatLevelLegacyType _tl)
		{
			this.id = ThreatLevelStreamer.numCreated++;
			this.threatLevel = _tl;
			MusicGroup musicGroup = MusicGroup.AllGroups[0];
			Dictionary<LayerType, LayerConfig> dictionary = ConfigSet.AllConfigSets[musicGroup.ConfigIDs[0]][_tl];
			ThreatLevel threatLevel = musicGroup[_tl];
			this.LayerStreamers = new EnumDictionary<LayerType, LayerStreamer>();
			foreach (KeyValuePair<LayerType, LayerConfig> keyValuePair in dictionary)
			{
				this.LayerStreamers.Add(keyValuePair.Key, LayerStreamer.Create(threatLevel[keyValuePair.Key], keyValuePair.Value, this));
			}
		}

		// Token: 0x0600CD7F RID: 52607 RVA: 0x004ABD1C File Offset: 0x004A9F1C
		[PublicizedFrom(EAccessModifier.Private)]
		public ThreatLevelStreamer(ThreatLevelLegacyType _tl, ThreatLevel _groupTL, ThreatLevelConfig _config)
		{
			this.id = ThreatLevelStreamer.numCreated++;
			this.LayerStreamers = new EnumDictionary<LayerType, LayerStreamer>(_config.Count);
			foreach (KeyValuePair<LayerType, LayerConfig> keyValuePair in _config)
			{
				this.LayerStreamers.Add(keyValuePair.Key, LayerStreamer.Create(_groupTL[keyValuePair.Key], keyValuePair.Value, this));
			}
		}

		// Token: 0x0600CD80 RID: 52608 RVA: 0x004ABDBC File Offset: 0x004A9FBC
		public void Cleanup()
		{
			foreach (LayerStreamer layerStreamer in this.LayerStreamers.Values)
			{
				layerStreamer.Cleanup();
			}
		}

		// Token: 0x0600CD81 RID: 52609 RVA: 0x004ABE14 File Offset: 0x004AA014
		public void Play()
		{
			if (this.InitFinished)
			{
				double num = AudioSettings.dspTime + 0.25;
				Log.Out(string.Format("Calling Play on {0}", this.id));
				foreach (LayerStreamer layerStreamer in this.LayerStreamers.Values)
				{
					layerStreamer.Play(num);
				}
			}
		}

		// Token: 0x0600CD82 RID: 52610 RVA: 0x004ABE9C File Offset: 0x004AA09C
		public void Pause()
		{
			this.IsPaused = true;
			foreach (LayerStreamer layerStreamer in this.LayerStreamers.Values)
			{
				layerStreamer.Pause();
			}
		}

		// Token: 0x0600CD83 RID: 52611 RVA: 0x004ABEF8 File Offset: 0x004AA0F8
		public void UnPause()
		{
			this.IsPaused = false;
			foreach (LayerStreamer layerStreamer in this.LayerStreamers.Values)
			{
				layerStreamer.UnPause();
			}
		}

		// Token: 0x0600CD84 RID: 52612 RVA: 0x004ABF54 File Offset: 0x004AA154
		public void Stop()
		{
			foreach (LayerStreamer layerStreamer in this.LayerStreamers.Values)
			{
				layerStreamer.Stop();
			}
		}

		// Token: 0x0600CD85 RID: 52613 RVA: 0x004ABFAC File Offset: 0x004AA1AC
		public void Tick()
		{
			if (!this.IsPlaying)
			{
				return;
			}
			foreach (LayerStreamer layerStreamer in this.LayerStreamers.Values)
			{
				layerStreamer.Tick();
			}
		}

		// Token: 0x17001950 RID: 6480
		// (get) Token: 0x0600CD86 RID: 52614 RVA: 0x004AC00C File Offset: 0x004AA20C
		public bool IsPlaying
		{
			get
			{
				using (Dictionary<LayerType, LayerStreamer>.ValueCollection.Enumerator enumerator = this.LayerStreamers.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.IsPlaying)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x04009BB3 RID: 39859
		public ThreatLevelLegacyType threatLevel;

		// Token: 0x04009BB4 RID: 39860
		public static int numCreated;

		// Token: 0x04009BB5 RID: 39861
		public readonly int id;

		// Token: 0x04009BB6 RID: 39862
		public bool IsPaused;

		// Token: 0x04009BB7 RID: 39863
		public Dictionary<LayerType, LayerStreamer> LayerStreamers;
	}
}
