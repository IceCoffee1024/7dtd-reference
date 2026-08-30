using System;
using System.Collections.Generic;
using DynamicMusic.Legacy.ObjectModel;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001A71 RID: 6769
	public class ThreatLevelStreamer
	{
		// Token: 0x170018DC RID: 6364
		// (get) Token: 0x0600CBF1 RID: 52209 RVA: 0x004A5784 File Offset: 0x004A3984
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

		// Token: 0x170018DD RID: 6365
		// (get) Token: 0x0600CBF2 RID: 52210 RVA: 0x004A57E4 File Offset: 0x004A39E4
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

		// Token: 0x0600CBF3 RID: 52211 RVA: 0x004A5844 File Offset: 0x004A3A44
		public static ThreatLevelStreamer Create(ThreatLevelLegacyType _tl)
		{
			return new ThreatLevelStreamer(_tl);
		}

		// Token: 0x0600CBF4 RID: 52212 RVA: 0x004A584C File Offset: 0x004A3A4C
		public static ThreatLevelStreamer Create(ThreatLevelLegacyType _tl, ThreatLevel _groupTL, ThreatLevelConfig _config)
		{
			return new ThreatLevelStreamer(_tl, _groupTL, _config);
		}

		// Token: 0x0600CBF5 RID: 52213 RVA: 0x004A5858 File Offset: 0x004A3A58
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

		// Token: 0x0600CBF6 RID: 52214 RVA: 0x004A5928 File Offset: 0x004A3B28
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

		// Token: 0x0600CBF7 RID: 52215 RVA: 0x004A59C8 File Offset: 0x004A3BC8
		public void Cleanup()
		{
			foreach (LayerStreamer layerStreamer in this.LayerStreamers.Values)
			{
				layerStreamer.Cleanup();
			}
		}

		// Token: 0x0600CBF8 RID: 52216 RVA: 0x004A5A20 File Offset: 0x004A3C20
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

		// Token: 0x0600CBF9 RID: 52217 RVA: 0x004A5AA8 File Offset: 0x004A3CA8
		public void Pause()
		{
			this.IsPaused = true;
			foreach (LayerStreamer layerStreamer in this.LayerStreamers.Values)
			{
				layerStreamer.Pause();
			}
		}

		// Token: 0x0600CBFA RID: 52218 RVA: 0x004A5B04 File Offset: 0x004A3D04
		public void UnPause()
		{
			this.IsPaused = false;
			foreach (LayerStreamer layerStreamer in this.LayerStreamers.Values)
			{
				layerStreamer.UnPause();
			}
		}

		// Token: 0x0600CBFB RID: 52219 RVA: 0x004A5B60 File Offset: 0x004A3D60
		public void Stop()
		{
			foreach (LayerStreamer layerStreamer in this.LayerStreamers.Values)
			{
				layerStreamer.Stop();
			}
		}

		// Token: 0x0600CBFC RID: 52220 RVA: 0x004A5BB8 File Offset: 0x004A3DB8
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

		// Token: 0x170018DE RID: 6366
		// (get) Token: 0x0600CBFD RID: 52221 RVA: 0x004A5C18 File Offset: 0x004A3E18
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

		// Token: 0x04009A82 RID: 39554
		public ThreatLevelLegacyType threatLevel;

		// Token: 0x04009A83 RID: 39555
		public static int numCreated;

		// Token: 0x04009A84 RID: 39556
		public readonly int id;

		// Token: 0x04009A85 RID: 39557
		public bool IsPaused;

		// Token: 0x04009A86 RID: 39558
		public Dictionary<LayerType, LayerStreamer> LayerStreamers;
	}
}
