using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic.Legacy.ObjectModel
{
	// Token: 0x02001A78 RID: 6776
	public class InstrumentID : Dictionary<PlacementType, string>
	{
		// Token: 0x14000118 RID: 280
		// (add) Token: 0x0600CC29 RID: 52265 RVA: 0x004A6448 File Offset: 0x004A4648
		// (remove) Token: 0x0600CC2A RID: 52266 RVA: 0x004A6480 File Offset: 0x004A4680
		public event InstrumentID.LoadFinishedAction OnLoadFinished;

		// Token: 0x170018EA RID: 6378
		// (get) Token: 0x0600CC2B RID: 52267 RVA: 0x004A64B5 File Offset: 0x004A46B5
		// (set) Token: 0x0600CC2C RID: 52268 RVA: 0x004A64BD File Offset: 0x004A46BD
		public bool IsLoaded
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CC2D RID: 52269 RVA: 0x004A64C6 File Offset: 0x004A46C6
		public InstrumentID()
			: base(3)
		{
			this.IsLoaded = false;
			this.ClipData = new EnumDictionary<PlacementType, float[]>(3);
			this.Clips = new EnumDictionary<PlacementType, AudioClip>(3);
		}

		// Token: 0x0600CC2E RID: 52270 RVA: 0x004A6504 File Offset: 0x004A4704
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

		// Token: 0x0600CC2F RID: 52271 RVA: 0x004A652E File Offset: 0x004A472E
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

		// Token: 0x0600CC30 RID: 52272 RVA: 0x004A653D File Offset: 0x004A473D
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnRequestFinished(AudioClip clip)
		{
			this.isClipLoaded = true;
		}

		// Token: 0x0600CC31 RID: 52273 RVA: 0x004A6548 File Offset: 0x004A4748
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

		// Token: 0x04009AA8 RID: 39592
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator<bool> thisEnumerator;

		// Token: 0x04009AAA RID: 39594
		public static string BundlePath;

		// Token: 0x04009AAB RID: 39595
		public string Name;

		// Token: 0x04009AAC RID: 39596
		public string SourceName;

		// Token: 0x04009AAD RID: 39597
		public float Volume = 1f;

		// Token: 0x04009AAE RID: 39598
		public int Frames;

		// Token: 0x04009AAF RID: 39599
		public int Samples;

		// Token: 0x04009AB0 RID: 39600
		public int Channels;

		// Token: 0x04009AB1 RID: 39601
		public int Frequency = 44100;

		// Token: 0x04009AB2 RID: 39602
		[PublicizedFrom(EAccessModifier.Private)]
		public EnumDictionary<PlacementType, AudioClip> Clips;

		// Token: 0x04009AB3 RID: 39603
		public EnumDictionary<PlacementType, float[]> ClipData;

		// Token: 0x04009AB5 RID: 39605
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasGrabbedClipProperties;

		// Token: 0x04009AB6 RID: 39606
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isClipLoaded;

		// Token: 0x02001A79 RID: 6777
		// (Invoke) Token: 0x0600CC33 RID: 52275
		public delegate void LoadFinishedAction();
	}
}
