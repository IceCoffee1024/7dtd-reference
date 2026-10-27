using System;
using System.Collections.Generic;
using DynamicMusic.Legacy.ObjectModel;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001AF3 RID: 6899
	public class LayerStreamer
	{
		// Token: 0x17001A35 RID: 6709
		// (get) Token: 0x0600CFCE RID: 53198 RVA: 0x004B3625 File Offset: 0x004B1825
		// (set) Token: 0x0600CFCF RID: 53199 RVA: 0x004B362D File Offset: 0x004B182D
		public bool HasReachedLastHyperbar
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001A36 RID: 6710
		// (get) Token: 0x0600CFD0 RID: 53200 RVA: 0x004B3636 File Offset: 0x004B1836
		public bool IsPlaying
		{
			get
			{
				return this.Src && this.Src.isPlaying;
			}
		}

		// Token: 0x0600CFD1 RID: 53201 RVA: 0x004B3652 File Offset: 0x004B1852
		public static LayerStreamer Create(Layer _layer, LayerConfig _layerConfig, ThreatLevelStreamer _parent = null)
		{
			return new LayerStreamer(_layer, _layerConfig, _parent);
		}

		// Token: 0x0600CFD2 RID: 53202 RVA: 0x004B365C File Offset: 0x004B185C
		[PublicizedFrom(EAccessModifier.Private)]
		public LayerStreamer(Layer _layer, LayerConfig _layerConfig, ThreatLevelStreamer _parent)
		{
			this.parentId = _parent.id;
			this.HasReachedLastHyperbar = false;
			this.LayerConfig = _layerConfig;
			this.instrumentID = _layer.GetInstrumentID();
			if (this.instrumentID.IsLoaded)
			{
				this.OnClipSetLoad();
				return;
			}
			this.instrumentID.OnLoadFinished += this.OnClipSetLoad;
		}

		// Token: 0x0600CFD3 RID: 53203 RVA: 0x004B36C0 File Offset: 0x004B18C0
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnClipSetLoad()
		{
			this.Src = global::UnityEngine.Object.Instantiate<AudioSource>(Resources.Load<AudioSource>(this.instrumentID.SourceName));
			this.Src.volume = this.instrumentID.Volume;
			this.Src.clip = AudioClip.Create(this.instrumentID.Name, this.instrumentID.Frames, this.instrumentID.Channels, this.instrumentID.Frequency, true, new AudioClip.PCMReaderCallback(this.FillStream));
			this.InitFinished = true;
		}

		// Token: 0x0600CFD4 RID: 53204 RVA: 0x004B3750 File Offset: 0x004B1950
		public void Cleanup()
		{
			if (this.Src)
			{
				global::UnityEngine.Object.Destroy(this.Src.gameObject);
			}
			this.instrumentID.OnLoadFinished -= this.OnClipSetLoad;
			this.instrumentID.Unload();
		}

		// Token: 0x0600CFD5 RID: 53205 RVA: 0x004B379C File Offset: 0x004B199C
		public void Play(double _time)
		{
			this.Src.PlayScheduled(_time);
			this.Src.loop = true;
		}

		// Token: 0x0600CFD6 RID: 53206 RVA: 0x004B37B6 File Offset: 0x004B19B6
		public void Pause()
		{
			if (this.Src && this.Src.isPlaying)
			{
				this.Src.Pause();
			}
		}

		// Token: 0x0600CFD7 RID: 53207 RVA: 0x004B37DD File Offset: 0x004B19DD
		public void UnPause()
		{
			if (this.Src)
			{
				this.Src.UnPause();
			}
		}

		// Token: 0x0600CFD8 RID: 53208 RVA: 0x004B37F7 File Offset: 0x004B19F7
		public void Stop()
		{
			if (this.Src && this.Src.isPlaying)
			{
				this.Src.Stop();
			}
		}

		// Token: 0x0600CFD9 RID: 53209 RVA: 0x004B3820 File Offset: 0x004B1A20
		public void Tick()
		{
			if (this.HasReachedLastHyperbar && this.Src && this.Src.loop)
			{
				this.Src.loop = false;
				Log.Out(string.Format("Set loop to false on {0}", this.instrumentID.Name));
			}
		}

		// Token: 0x0600CFDA RID: 53210 RVA: 0x004B3878 File Offset: 0x004B1A78
		[PublicizedFrom(EAccessModifier.Private)]
		public void FillStream(float[] data)
		{
			if (!this.InFillStream)
			{
				this.InFillStream = true;
				int num = data.Length;
				for (int i = 0; i < num; i++)
				{
					if (this.cursor == 0)
					{
						Dictionary<byte, PlacementType> layerConfig = this.LayerConfig;
						int num2 = this.hyperbar;
						this.hyperbar = num2 + 1;
						PlacementType placementType;
						if (layerConfig.TryGetValue((byte)num2, out placementType))
						{
							this.currentClipData = this.instrumentID.ClipData[placementType];
							this.HasReachedLastHyperbar = placementType == PlacementType.End;
						}
						else
						{
							this.currentClipData = null;
						}
					}
					data[i] = ((this.currentClipData != null) ? this.currentClipData[this.cursor] : 0f);
					this.cursor++;
					this.cursor %= this.instrumentID.Samples;
				}
				this.InFillStream = false;
				return;
			}
			Log.Warning("FillStream was called while it was still running.");
		}

		// Token: 0x04009D55 RID: 40277
		public bool InitFinished;

		// Token: 0x04009D57 RID: 40279
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int parentId;

		// Token: 0x04009D58 RID: 40280
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly InstrumentID instrumentID;

		// Token: 0x04009D59 RID: 40281
		[PublicizedFrom(EAccessModifier.Private)]
		public LayerConfig LayerConfig;

		// Token: 0x04009D5A RID: 40282
		[PublicizedFrom(EAccessModifier.Private)]
		public AudioSource Src;

		// Token: 0x04009D5B RID: 40283
		[PublicizedFrom(EAccessModifier.Private)]
		public int hyperbar;

		// Token: 0x04009D5C RID: 40284
		[PublicizedFrom(EAccessModifier.Private)]
		public int cursor;

		// Token: 0x04009D5D RID: 40285
		[PublicizedFrom(EAccessModifier.Private)]
		public float[] currentClipData;

		// Token: 0x04009D5E RID: 40286
		[PublicizedFrom(EAccessModifier.Private)]
		public bool InFillStream;
	}
}
