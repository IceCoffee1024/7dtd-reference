using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic.Legacy.ObjectModel
{
	// Token: 0x02001A9F RID: 6815
	public class InstrumentID : Dictionary<PlacementType, string>
	{
		// Token: 0x14000119 RID: 281
		// (add) Token: 0x0600CDB2 RID: 52658 RVA: 0x004AC83C File Offset: 0x004AAA3C
		// (remove) Token: 0x0600CDB3 RID: 52659 RVA: 0x004AC874 File Offset: 0x004AAA74
		public event InstrumentID.LoadFinishedAction OnLoadFinished;

		// Token: 0x1700195C RID: 6492
		// (get) Token: 0x0600CDB4 RID: 52660 RVA: 0x004AC8A9 File Offset: 0x004AAAA9
		// (set) Token: 0x0600CDB5 RID: 52661 RVA: 0x004AC8B1 File Offset: 0x004AAAB1
		public bool IsLoaded
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CDB6 RID: 52662 RVA: 0x004AC8BA File Offset: 0x004AAABA
		public InstrumentID()
			: base(3)
		{
			this.IsLoaded = false;
			this.ClipData = new EnumDictionary<PlacementType, float[]>(3);
			this.Clips = new EnumDictionary<PlacementType, AudioClip>(3);
		}

		// Token: 0x0600CDB7 RID: 52663 RVA: 0x004AC8F8 File Offset: 0x004AAAF8
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

		// Token: 0x0600CDB8 RID: 52664 RVA: 0x004AC922 File Offset: 0x004AAB22
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

		// Token: 0x0600CDB9 RID: 52665 RVA: 0x004AC931 File Offset: 0x004AAB31
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnRequestFinished(AudioClip clip)
		{
			this.isClipLoaded = true;
		}

		// Token: 0x0600CDBA RID: 52666 RVA: 0x004AC93C File Offset: 0x004AAB3C
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

		// Token: 0x04009BD9 RID: 39897
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator<bool> thisEnumerator;

		// Token: 0x04009BDB RID: 39899
		public static string BundlePath;

		// Token: 0x04009BDC RID: 39900
		public string Name;

		// Token: 0x04009BDD RID: 39901
		public string SourceName;

		// Token: 0x04009BDE RID: 39902
		public float Volume = 1f;

		// Token: 0x04009BDF RID: 39903
		public int Frames;

		// Token: 0x04009BE0 RID: 39904
		public int Samples;

		// Token: 0x04009BE1 RID: 39905
		public int Channels;

		// Token: 0x04009BE2 RID: 39906
		public int Frequency = 44100;

		// Token: 0x04009BE3 RID: 39907
		[PublicizedFrom(EAccessModifier.Private)]
		public EnumDictionary<PlacementType, AudioClip> Clips;

		// Token: 0x04009BE4 RID: 39908
		public EnumDictionary<PlacementType, float[]> ClipData;

		// Token: 0x04009BE6 RID: 39910
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasGrabbedClipProperties;

		// Token: 0x04009BE7 RID: 39911
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isClipLoaded;

		// Token: 0x02001AA0 RID: 6816
		// (Invoke) Token: 0x0600CDBC RID: 52668
		public delegate void LoadFinishedAction();
	}
}
