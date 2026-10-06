using System;
using System.Collections.Generic;
using DynamicMusic.Legacy.ObjectModel;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001AC2 RID: 6850
	public class LayerStreamer
	{
		// Token: 0x170019ED RID: 6637
		// (get) Token: 0x0600CEBA RID: 52922 RVA: 0x004AFB29 File Offset: 0x004ADD29
		// (set) Token: 0x0600CEBB RID: 52923 RVA: 0x004AFB31 File Offset: 0x004ADD31
		public bool HasReachedLastHyperbar
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170019EE RID: 6638
		// (get) Token: 0x0600CEBC RID: 52924 RVA: 0x004AFB3A File Offset: 0x004ADD3A
		public bool IsPlaying
		{
			get
			{
				return this.Src && this.Src.isPlaying;
			}
		}

		// Token: 0x0600CEBD RID: 52925 RVA: 0x004AFB56 File Offset: 0x004ADD56
		public static LayerStreamer Create(Layer _layer, LayerConfig _layerConfig, ThreatLevelStreamer _parent = null)
		{
			return new LayerStreamer(_layer, _layerConfig, _parent);
		}

		// Token: 0x0600CEBE RID: 52926 RVA: 0x004AFB60 File Offset: 0x004ADD60
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

		// Token: 0x0600CEBF RID: 52927 RVA: 0x004AFBC4 File Offset: 0x004ADDC4
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnClipSetLoad()
		{
			this.Src = global::UnityEngine.Object.Instantiate<AudioSource>(Resources.Load<AudioSource>(this.instrumentID.SourceName));
			this.Src.volume = this.instrumentID.Volume;
			this.Src.clip = AudioClip.Create(this.instrumentID.Name, this.instrumentID.Frames, this.instrumentID.Channels, this.instrumentID.Frequency, true, new AudioClip.PCMReaderCallback(this.FillStream));
			this.InitFinished = true;
		}

		// Token: 0x0600CEC0 RID: 52928 RVA: 0x004AFC54 File Offset: 0x004ADE54
		public void Cleanup()
		{
			if (this.Src)
			{
				global::UnityEngine.Object.Destroy(this.Src.gameObject);
			}
			this.instrumentID.OnLoadFinished -= this.OnClipSetLoad;
			this.instrumentID.Unload();
		}

		// Token: 0x0600CEC1 RID: 52929 RVA: 0x004AFCA0 File Offset: 0x004ADEA0
		public void Play(double _time)
		{
			this.Src.PlayScheduled(_time);
			this.Src.loop = true;
		}

		// Token: 0x0600CEC2 RID: 52930 RVA: 0x004AFCBA File Offset: 0x004ADEBA
		public void Pause()
		{
			if (this.Src && this.Src.isPlaying)
			{
				this.Src.Pause();
			}
		}

		// Token: 0x0600CEC3 RID: 52931 RVA: 0x004AFCE1 File Offset: 0x004ADEE1
		public void UnPause()
		{
			if (this.Src)
			{
				this.Src.UnPause();
			}
		}

		// Token: 0x0600CEC4 RID: 52932 RVA: 0x004AFCFB File Offset: 0x004ADEFB
		public void Stop()
		{
			if (this.Src && this.Src.isPlaying)
			{
				this.Src.Stop();
			}
		}

		// Token: 0x0600CEC5 RID: 52933 RVA: 0x004AFD24 File Offset: 0x004ADF24
		public void Tick()
		{
			if (this.HasReachedLastHyperbar && this.Src && this.Src.loop)
			{
				this.Src.loop = false;
				Log.Out(string.Format("Set loop to false on {0}", this.instrumentID.Name));
			}
		}

		// Token: 0x0600CEC6 RID: 52934 RVA: 0x004AFD7C File Offset: 0x004ADF7C
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

		// Token: 0x04009C6C RID: 40044
		public bool InitFinished;

		// Token: 0x04009C6E RID: 40046
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int parentId;

		// Token: 0x04009C6F RID: 40047
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly InstrumentID instrumentID;

		// Token: 0x04009C70 RID: 40048
		[PublicizedFrom(EAccessModifier.Private)]
		public LayerConfig LayerConfig;

		// Token: 0x04009C71 RID: 40049
		[PublicizedFrom(EAccessModifier.Private)]
		public AudioSource Src;

		// Token: 0x04009C72 RID: 40050
		[PublicizedFrom(EAccessModifier.Private)]
		public int hyperbar;

		// Token: 0x04009C73 RID: 40051
		[PublicizedFrom(EAccessModifier.Private)]
		public int cursor;

		// Token: 0x04009C74 RID: 40052
		[PublicizedFrom(EAccessModifier.Private)]
		public float[] currentClipData;

		// Token: 0x04009C75 RID: 40053
		[PublicizedFrom(EAccessModifier.Private)]
		public bool InFillStream;
	}
}
