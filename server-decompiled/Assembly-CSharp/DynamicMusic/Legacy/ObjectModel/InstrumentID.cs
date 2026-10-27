using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic.Legacy.ObjectModel
{
	// Token: 0x02001AFC RID: 6908
	public class InstrumentID : Dictionary<PlacementType, string>
	{
		// Token: 0x14000118 RID: 280
		// (add) Token: 0x0600D01E RID: 53278 RVA: 0x004B4804 File Offset: 0x004B2A04
		// (remove) Token: 0x0600D01F RID: 53279 RVA: 0x004B483C File Offset: 0x004B2A3C
		public event InstrumentID.LoadFinishedAction OnLoadFinished;

		// Token: 0x17001A46 RID: 6726
		// (get) Token: 0x0600D020 RID: 53280 RVA: 0x004B4871 File Offset: 0x004B2A71
		// (set) Token: 0x0600D021 RID: 53281 RVA: 0x004B4879 File Offset: 0x004B2A79
		public bool IsLoaded
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600D022 RID: 53282 RVA: 0x004B4882 File Offset: 0x004B2A82
		public InstrumentID()
			: base(3)
		{
			this.IsLoaded = false;
			this.ClipData = new EnumDictionary<PlacementType, float[]>(3);
			this.Clips = new EnumDictionary<PlacementType, AudioClip>(3);
		}

		// Token: 0x0600D023 RID: 53283 RVA: 0x004B48C0 File Offset: 0x004B2AC0
		public void Load()
		{
			if (!this.IsLoaded)
			{
				if (this.thisEnumerator == null)
				{
					this.thisEnumerator = this.LoadClip();
				}
				this.thisEnumerator.MoveNext();
			}
		}

		// Token: 0x0600D024 RID: 53284 RVA: 0x004B48EA File Offset: 0x004B2AEA
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator<bool> LoadClip()
		{
			if (!this.IsLoaded)
			{
				foreach (KeyValuePair<PlacementType, string> kvp in this)
				{
					LoadManager.AssetRequestTask<AudioClip> requestTask = LoadManager.LoadAsset<AudioClip>(kvp.Value, null, null, false, false, false);
					while (!requestTask.IsDone)
					{
						yield return false;
					}
					AudioClip clip = requestTask.Asset;
					if (clip != null)
					{
						if (clip.loadState == AudioDataLoadState.Unloaded)
						{
							clip.LoadAudioData();
							while (clip.loadState != AudioDataLoadState.Loaded)
							{
								yield return false;
								if (clip.loadState == AudioDataLoadState.Failed)
								{
									Log.Warning(string.Format("clip load failed in {0}", this.Name));
									break;
								}
							}
						}
						if (!this.hasGrabbedClipProperties)
						{
							this.Frames = clip.samples;
							this.Channels = clip.channels;
							this.Frequency = clip.frequency;
							this.Samples = this.Frames * this.Channels;
							this.hasGrabbedClipProperties = true;
						}
						else if (this.Frames != clip.samples || this.Channels != clip.channels || this.Frequency != clip.frequency)
						{
							Log.Warning(string.Format("Inconsistent clip properties for clips in {0}", this.Name));
						}
						int samplesPerPass = 44100;
						int samplesGrabbed = 0;
						int samplesToGrab = Utils.FastMin(samplesPerPass, this.Samples - samplesGrabbed);
						float[] sampleData = MemoryPools.poolFloat.Alloc(this.Samples);
						float[] buffer = MemoryPools.poolFloat.Alloc(samplesPerPass);
						while (samplesGrabbed < this.Samples)
						{
							if (this.Samples - samplesGrabbed < samplesPerPass)
							{
								buffer = new float[this.Samples - samplesGrabbed];
							}
							clip.GetData(buffer, samplesGrabbed / 2);
							buffer.CopyTo(sampleData, samplesGrabbed);
							samplesGrabbed += samplesToGrab;
							yield return false;
						}
						MemoryPools.poolFloat.Free(buffer);
						this.ClipData.Add(kvp.Key, sampleData);
						yield return false;
						clip.UnloadAudioData();
						this.isClipLoaded = false;
						sampleData = null;
						buffer = null;
					}
					else
					{
						Log.Warning(string.Format("Loaded resource {0} could not be boxed as an AudioClip", kvp.Value));
					}
					requestTask = null;
					clip = null;
					kvp = default(KeyValuePair<PlacementType, string>);
				}
				Dictionary<PlacementType, string>.Enumerator enumerator = default(Dictionary<PlacementType, string>.Enumerator);
			}
			this.IsLoaded = true;
			InstrumentID.LoadFinishedAction onLoadFinished = this.OnLoadFinished;
			if (onLoadFinished != null)
			{
				onLoadFinished();
			}
			yield return true;
			yield break;
			yield break;
		}

		// Token: 0x0600D025 RID: 53285 RVA: 0x004B48F9 File Offset: 0x004B2AF9
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnRequestFinished(AudioClip clip)
		{
			this.isClipLoaded = true;
		}

		// Token: 0x0600D026 RID: 53286 RVA: 0x004B4904 File Offset: 0x004B2B04
		public void Unload()
		{
			foreach (float[] array in this.ClipData.Values)
			{
				MemoryPools.poolFloat.Free(array);
			}
			this.ClipData.Clear();
			this.thisEnumerator = null;
			this.IsLoaded = (this.hasGrabbedClipProperties = false);
			this.Frames = (this.Samples = (this.Channels = 0));
			this.Frequency = 44100;
		}

		// Token: 0x04009D88 RID: 40328
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator<bool> thisEnumerator;

		// Token: 0x04009D8A RID: 40330
		public static string BundlePath;

		// Token: 0x04009D8B RID: 40331
		public string Name;

		// Token: 0x04009D8C RID: 40332
		public string SourceName;

		// Token: 0x04009D8D RID: 40333
		public float Volume = 1f;

		// Token: 0x04009D8E RID: 40334
		public int Frames;

		// Token: 0x04009D8F RID: 40335
		public int Samples;

		// Token: 0x04009D90 RID: 40336
		public int Channels;

		// Token: 0x04009D91 RID: 40337
		public int Frequency = 44100;

		// Token: 0x04009D92 RID: 40338
		[PublicizedFrom(EAccessModifier.Private)]
		public EnumDictionary<PlacementType, AudioClip> Clips;

		// Token: 0x04009D93 RID: 40339
		public EnumDictionary<PlacementType, float[]> ClipData;

		// Token: 0x04009D95 RID: 40341
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasGrabbedClipProperties;

		// Token: 0x04009D96 RID: 40342
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isClipLoaded;

		// Token: 0x02001AFD RID: 6909
		// (Invoke) Token: 0x0600D028 RID: 53288
		public delegate void LoadFinishedAction();
	}
}
