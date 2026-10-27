using System;
using System.Collections.Generic;
using DynamicMusic.Legacy.ObjectModel;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001AF5 RID: 6901
	public class ThreatLevelStreamer
	{
		// Token: 0x17001A38 RID: 6712
		// (get) Token: 0x0600CFE6 RID: 53222 RVA: 0x004B3B40 File Offset: 0x004B1D40
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

		// Token: 0x17001A39 RID: 6713
		// (get) Token: 0x0600CFE7 RID: 53223 RVA: 0x004B3BA0 File Offset: 0x004B1DA0
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

		// Token: 0x0600CFE8 RID: 53224 RVA: 0x004B3C00 File Offset: 0x004B1E00
		public static ThreatLevelStreamer Create(ThreatLevelLegacyType _tl)
		{
			return new ThreatLevelStreamer(_tl);
		}

		// Token: 0x0600CFE9 RID: 53225 RVA: 0x004B3C08 File Offset: 0x004B1E08
		public static ThreatLevelStreamer Create(ThreatLevelLegacyType _tl, ThreatLevel _groupTL, ThreatLevelConfig _config)
		{
			return new ThreatLevelStreamer(_tl, _groupTL, _config);
		}

		// Token: 0x0600CFEA RID: 53226 RVA: 0x004B3C14 File Offset: 0x004B1E14
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

		// Token: 0x0600CFEB RID: 53227 RVA: 0x004B3CE4 File Offset: 0x004B1EE4
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

		// Token: 0x0600CFEC RID: 53228 RVA: 0x004B3D84 File Offset: 0x004B1F84
		public void Cleanup()
		{
			foreach (LayerStreamer layerStreamer in this.LayerStreamers.Values)
			{
				layerStreamer.Cleanup();
			}
		}

		// Token: 0x0600CFED RID: 53229 RVA: 0x004B3DDC File Offset: 0x004B1FDC
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

		// Token: 0x0600CFEE RID: 53230 RVA: 0x004B3E64 File Offset: 0x004B2064
		public void Pause()
		{
			this.IsPaused = true;
			foreach (LayerStreamer layerStreamer in this.LayerStreamers.Values)
			{
				layerStreamer.Pause();
			}
		}

		// Token: 0x0600CFEF RID: 53231 RVA: 0x004B3EC0 File Offset: 0x004B20C0
		public void UnPause()
		{
			this.IsPaused = false;
			foreach (LayerStreamer layerStreamer in this.LayerStreamers.Values)
			{
				layerStreamer.UnPause();
			}
		}

		// Token: 0x0600CFF0 RID: 53232 RVA: 0x004B3F1C File Offset: 0x004B211C
		public void Stop()
		{
			foreach (LayerStreamer layerStreamer in this.LayerStreamers.Values)
			{
				layerStreamer.Stop();
			}
		}

		// Token: 0x0600CFF1 RID: 53233 RVA: 0x004B3F74 File Offset: 0x004B2174
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

		// Token: 0x17001A3A RID: 6714
		// (get) Token: 0x0600CFF2 RID: 53234 RVA: 0x004B3FD4 File Offset: 0x004B21D4
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

		// Token: 0x04009D62 RID: 40290
		public ThreatLevelLegacyType threatLevel;

		// Token: 0x04009D63 RID: 40291
		public static int numCreated;

		// Token: 0x04009D64 RID: 40292
		public readonly int id;

		// Token: 0x04009D65 RID: 40293
		public bool IsPaused;

		// Token: 0x04009D66 RID: 40294
		public Dictionary<LayerType, LayerStreamer> LayerStreamers;
	}
}
