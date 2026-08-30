using System;
using System.Collections.Generic;
using DynamicMusic.Legacy.ObjectModel;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001A96 RID: 6806
	public class LayerStreamer
	{
		// Token: 0x1700194B RID: 6475
		// (get) Token: 0x0600CD62 RID: 52578 RVA: 0x004AB65D File Offset: 0x004A985D
		// (set) Token: 0x0600CD63 RID: 52579 RVA: 0x004AB665 File Offset: 0x004A9865
		public bool HasReachedLastHyperbar
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700194C RID: 6476
		// (get) Token: 0x0600CD64 RID: 52580 RVA: 0x004AB66E File Offset: 0x004A986E
		public bool IsPlaying
		{
			get
			{
				return this.Src && this.Src.isPlaying;
			}
		}

		// Token: 0x0600CD65 RID: 52581 RVA: 0x004AB68A File Offset: 0x004A988A
		public static LayerStreamer Create(Layer _layer, LayerConfig _layerConfig, ThreatLevelStreamer _parent = null)
		{
			return new LayerStreamer(_layer, _layerConfig, _parent);
		}

		// Token: 0x0600CD66 RID: 52582 RVA: 0x004AB694 File Offset: 0x004A9894
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

		// Token: 0x0600CD67 RID: 52583 RVA: 0x004AB6F8 File Offset: 0x004A98F8
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnClipSetLoad()
		{
			this.Src = global::UnityEngine.Object.Instantiate<AudioSource>(Resources.Load<AudioSource>(this.instrumentID.SourceName));
			this.Src.volume = this.instrumentID.Volume;
			this.Src.clip = AudioClip.Create(this.instrumentID.Name, this.instrumentID.Frames, this.instrumentID.Channels, this.instrumentID.Frequency, true, new AudioClip.PCMReaderCallback(this.FillStream));
			this.InitFinished = true;
		}

		// Token: 0x0600CD68 RID: 52584 RVA: 0x004AB788 File Offset: 0x004A9988
		public void Cleanup()
		{
			if (this.Src)
			{
				global::UnityEngine.Object.Destroy(this.Src.gameObject);
			}
			this.instrumentID.OnLoadFinished -= this.OnClipSetLoad;
			this.instrumentID.Unload();
		}

		// Token: 0x0600CD69 RID: 52585 RVA: 0x004AB7D4 File Offset: 0x004A99D4
		public void Play(double _time)
		{
			this.Src.PlayScheduled(_time);
			this.Src.loop = true;
		}

		// Token: 0x0600CD6A RID: 52586 RVA: 0x004AB7EE File Offset: 0x004A99EE
		public void Pause()
		{
			if (this.Src && this.Src.isPlaying)
			{
				this.Src.Pause();
			}
		}

		// Token: 0x0600CD6B RID: 52587 RVA: 0x004AB815 File Offset: 0x004A9A15
		public void UnPause()
		{
			if (this.Src)
			{
				this.Src.UnPause();
			}
		}

		// Token: 0x0600CD6C RID: 52588 RVA: 0x004AB82F File Offset: 0x004A9A2F
		public void Stop()
		{
			if (this.Src && this.Src.isPlaying)
			{
				this.Src.Stop();
			}
		}

		// Token: 0x0600CD6D RID: 52589 RVA: 0x004AB858 File Offset: 0x004A9A58
		public void Tick()
		{
			if (this.HasReachedLastHyperbar && this.Src && this.Src.loop)
			{
				this.Src.loop = false;
				Log.Out(string.Format("Set loop to false on {0}", this.instrumentID.Name));
			}
		}

		// Token: 0x0600CD6E RID: 52590 RVA: 0x004AB8B0 File Offset: 0x004A9AB0
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

		// Token: 0x04009BA6 RID: 39846
		public bool InitFinished;

		// Token: 0x04009BA8 RID: 39848
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int parentId;

		// Token: 0x04009BA9 RID: 39849
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly InstrumentID instrumentID;

		// Token: 0x04009BAA RID: 39850
		[PublicizedFrom(EAccessModifier.Private)]
		public LayerConfig LayerConfig;

		// Token: 0x04009BAB RID: 39851
		[PublicizedFrom(EAccessModifier.Private)]
		public AudioSource Src;

		// Token: 0x04009BAC RID: 39852
		[PublicizedFrom(EAccessModifier.Private)]
		public int hyperbar;

		// Token: 0x04009BAD RID: 39853
		[PublicizedFrom(EAccessModifier.Private)]
		public int cursor;

		// Token: 0x04009BAE RID: 39854
		[PublicizedFrom(EAccessModifier.Private)]
		public float[] currentClipData;

		// Token: 0x04009BAF RID: 39855
		[PublicizedFrom(EAccessModifier.Private)]
		public bool InFillStream;
	}
}
