using System;
using System.Collections.Generic;
using DynamicMusic.Legacy.ObjectModel;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001AC4 RID: 6852
	public class ThreatLevelStreamer
	{
		// Token: 0x170019F0 RID: 6640
		// (get) Token: 0x0600CED2 RID: 52946 RVA: 0x004B0044 File Offset: 0x004AE244
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

		// Token: 0x170019F1 RID: 6641
		// (get) Token: 0x0600CED3 RID: 52947 RVA: 0x004B00A4 File Offset: 0x004AE2A4
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

		// Token: 0x0600CED4 RID: 52948 RVA: 0x004B0104 File Offset: 0x004AE304
		public static ThreatLevelStreamer Create(ThreatLevelLegacyType _tl)
		{
			return new ThreatLevelStreamer(_tl);
		}

		// Token: 0x0600CED5 RID: 52949 RVA: 0x004B010C File Offset: 0x004AE30C
		public static ThreatLevelStreamer Create(ThreatLevelLegacyType _tl, ThreatLevel _groupTL, ThreatLevelConfig _config)
		{
			return new ThreatLevelStreamer(_tl, _groupTL, _config);
		}

		// Token: 0x0600CED6 RID: 52950 RVA: 0x004B0118 File Offset: 0x004AE318
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

		// Token: 0x0600CED7 RID: 52951 RVA: 0x004B01E8 File Offset: 0x004AE3E8
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

		// Token: 0x0600CED8 RID: 52952 RVA: 0x004B0288 File Offset: 0x004AE488
		public void Cleanup()
		{
			foreach (LayerStreamer layerStreamer in this.LayerStreamers.Values)
			{
				layerStreamer.Cleanup();
			}
		}

		// Token: 0x0600CED9 RID: 52953 RVA: 0x004B02E0 File Offset: 0x004AE4E0
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

		// Token: 0x0600CEDA RID: 52954 RVA: 0x004B0368 File Offset: 0x004AE568
		public void Pause()
		{
			this.IsPaused = true;
			foreach (LayerStreamer layerStreamer in this.LayerStreamers.Values)
			{
				layerStreamer.Pause();
			}
		}

		// Token: 0x0600CEDB RID: 52955 RVA: 0x004B03C4 File Offset: 0x004AE5C4
		public void UnPause()
		{
			this.IsPaused = false;
			foreach (LayerStreamer layerStreamer in this.LayerStreamers.Values)
			{
				layerStreamer.UnPause();
			}
		}

		// Token: 0x0600CEDC RID: 52956 RVA: 0x004B0420 File Offset: 0x004AE620
		public void Stop()
		{
			foreach (LayerStreamer layerStreamer in this.LayerStreamers.Values)
			{
				layerStreamer.Stop();
			}
		}

		// Token: 0x0600CEDD RID: 52957 RVA: 0x004B0478 File Offset: 0x004AE678
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

		// Token: 0x170019F2 RID: 6642
		// (get) Token: 0x0600CEDE RID: 52958 RVA: 0x004B04D8 File Offset: 0x004AE6D8
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

		// Token: 0x04009C79 RID: 40057
		public ThreatLevelLegacyType threatLevel;

		// Token: 0x04009C7A RID: 40058
		public static int numCreated;

		// Token: 0x04009C7B RID: 40059
		public readonly int id;

		// Token: 0x04009C7C RID: 40060
		public bool IsPaused;

		// Token: 0x04009C7D RID: 40061
		public Dictionary<LayerType, LayerStreamer> LayerStreamers;
	}
}
