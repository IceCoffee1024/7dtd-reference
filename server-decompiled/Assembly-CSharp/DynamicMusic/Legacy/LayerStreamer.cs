using System;
using System.Collections.Generic;
using DynamicMusic.Legacy.ObjectModel;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic.Legacy
{
	// Token: 0x02001A6F RID: 6767
	public class LayerStreamer
	{
		// Token: 0x170018D9 RID: 6361
		// (get) Token: 0x0600CBD9 RID: 52185 RVA: 0x004A5269 File Offset: 0x004A3469
		// (set) Token: 0x0600CBDA RID: 52186 RVA: 0x004A5271 File Offset: 0x004A3471
		public bool HasReachedLastHyperbar
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170018DA RID: 6362
		// (get) Token: 0x0600CBDB RID: 52187 RVA: 0x004A527A File Offset: 0x004A347A
		public bool IsPlaying
		{
			get
			{
				return this.Src && this.Src.isPlaying;
			}
		}

		// Token: 0x0600CBDC RID: 52188 RVA: 0x004A5296 File Offset: 0x004A3496
		public static LayerStreamer Create(Layer _layer, LayerConfig _layerConfig, ThreatLevelStreamer _parent = null)
		{
			return new LayerStreamer(_layer, _layerConfig, _parent);
		}

		// Token: 0x0600CBDD RID: 52189 RVA: 0x004A52A0 File Offset: 0x004A34A0
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

		// Token: 0x0600CBDE RID: 52190 RVA: 0x004A5304 File Offset: 0x004A3504
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnClipSetLoad()
		{
			this.Src = global::UnityEngine.Object.Instantiate<AudioSource>(Resources.Load<AudioSource>(this.instrumentID.SourceName));
			this.Src.volume = this.instrumentID.Volume;
			this.Src.clip = AudioClip.Create(this.instrumentID.Name, this.instrumentID.Frames, this.instrumentID.Channels, this.instrumentID.Frequency, true, new AudioClip.PCMReaderCallback(this.FillStream));
			this.InitFinished = true;
		}

		// Token: 0x0600CBDF RID: 52191 RVA: 0x004A5394 File Offset: 0x004A3594
		public void Cleanup()
		{
			if (this.Src)
			{
				global::UnityEngine.Object.Destroy(this.Src.gameObject);
			}
			this.instrumentID.OnLoadFinished -= this.OnClipSetLoad;
			this.instrumentID.Unload();
		}

		// Token: 0x0600CBE0 RID: 52192 RVA: 0x004A53E0 File Offset: 0x004A35E0
		public void Play(double _time)
		{
			this.Src.PlayScheduled(_time);
			this.Src.loop = true;
		}

		// Token: 0x0600CBE1 RID: 52193 RVA: 0x004A53FA File Offset: 0x004A35FA
		public void Pause()
		{
			if (this.Src && this.Src.isPlaying)
			{
				this.Src.Pause();
			}
		}

		// Token: 0x0600CBE2 RID: 52194 RVA: 0x004A5421 File Offset: 0x004A3621
		public void UnPause()
		{
			if (this.Src)
			{
				this.Src.UnPause();
			}
		}

		// Token: 0x0600CBE3 RID: 52195 RVA: 0x004A543B File Offset: 0x004A363B
		public void Stop()
		{
			if (this.Src && this.Src.isPlaying)
			{
				this.Src.Stop();
			}
		}

		// Token: 0x0600CBE4 RID: 52196 RVA: 0x004A5464 File Offset: 0x004A3664
		public void Tick()
		{
			if (this.HasReachedLastHyperbar && this.Src && this.Src.loop)
			{
				this.Src.loop = false;
				Log.Out(string.Format("Set loop to false on {0}", this.instrumentID.Name));
			}
		}

		// Token: 0x0600CBE5 RID: 52197 RVA: 0x004A54BC File Offset: 0x004A36BC
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

		// Token: 0x04009A75 RID: 39541
		public bool InitFinished;

		// Token: 0x04009A77 RID: 39543
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int parentId;

		// Token: 0x04009A78 RID: 39544
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly InstrumentID instrumentID;

		// Token: 0x04009A79 RID: 39545
		[PublicizedFrom(EAccessModifier.Private)]
		public LayerConfig LayerConfig;

		// Token: 0x04009A7A RID: 39546
		[PublicizedFrom(EAccessModifier.Private)]
		public AudioSource Src;

		// Token: 0x04009A7B RID: 39547
		[PublicizedFrom(EAccessModifier.Private)]
		public int hyperbar;

		// Token: 0x04009A7C RID: 39548
		[PublicizedFrom(EAccessModifier.Private)]
		public int cursor;

		// Token: 0x04009A7D RID: 39549
		[PublicizedFrom(EAccessModifier.Private)]
		public float[] currentClipData;

		// Token: 0x04009A7E RID: 39550
		[PublicizedFrom(EAccessModifier.Private)]
		public bool InFillStream;
	}
}
