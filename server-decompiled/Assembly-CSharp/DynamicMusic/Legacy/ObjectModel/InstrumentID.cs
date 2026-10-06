using System;
using System.Collections.Generic;
using MusicUtils.Enums;
using UnityEngine;

namespace DynamicMusic.Legacy.ObjectModel
{
	// Token: 0x02001ACB RID: 6859
	public class InstrumentID : Dictionary<PlacementType, string>
	{
		// Token: 0x14000118 RID: 280
		// (add) Token: 0x0600CF0A RID: 53002 RVA: 0x004B0D08 File Offset: 0x004AEF08
		// (remove) Token: 0x0600CF0B RID: 53003 RVA: 0x004B0D40 File Offset: 0x004AEF40
		public event InstrumentID.LoadFinishedAction OnLoadFinished;

		// Token: 0x170019FE RID: 6654
		// (get) Token: 0x0600CF0C RID: 53004 RVA: 0x004B0D75 File Offset: 0x004AEF75
		// (set) Token: 0x0600CF0D RID: 53005 RVA: 0x004B0D7D File Offset: 0x004AEF7D
		public bool IsLoaded
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600CF0E RID: 53006 RVA: 0x004B0D86 File Offset: 0x004AEF86
		public InstrumentID()
			: base(3)
		{
			this.IsLoaded = false;
			this.ClipData = new EnumDictionary<PlacementType, float[]>(3);
			this.Clips = new EnumDictionary<PlacementType, AudioClip>(3);
		}

		// Token: 0x0600CF0F RID: 53007 RVA: 0x004B0DC4 File Offset: 0x004AEFC4
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

		// Token: 0x0600CF10 RID: 53008 RVA: 0x004B0DEE File Offset: 0x004AEFEE
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

		// Token: 0x0600CF11 RID: 53009 RVA: 0x004B0DFD File Offset: 0x004AEFFD
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnRequestFinished(AudioClip clip)
		{
			this.isClipLoaded = true;
		}

		// Token: 0x0600CF12 RID: 53010 RVA: 0x004B0E08 File Offset: 0x004AF008
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

		// Token: 0x04009C9F RID: 40095
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator<bool> thisEnumerator;

		// Token: 0x04009CA1 RID: 40097
		public static string BundlePath;

		// Token: 0x04009CA2 RID: 40098
		public string Name;

		// Token: 0x04009CA3 RID: 40099
		public string SourceName;

		// Token: 0x04009CA4 RID: 40100
		public float Volume = 1f;

		// Token: 0x04009CA5 RID: 40101
		public int Frames;

		// Token: 0x04009CA6 RID: 40102
		public int Samples;

		// Token: 0x04009CA7 RID: 40103
		public int Channels;

		// Token: 0x04009CA8 RID: 40104
		public int Frequency = 44100;

		// Token: 0x04009CA9 RID: 40105
		[PublicizedFrom(EAccessModifier.Private)]
		public EnumDictionary<PlacementType, AudioClip> Clips;

		// Token: 0x04009CAA RID: 40106
		public EnumDictionary<PlacementType, float[]> ClipData;

		// Token: 0x04009CAC RID: 40108
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasGrabbedClipProperties;

		// Token: 0x04009CAD RID: 40109
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isClipLoaded;

		// Token: 0x02001ACC RID: 6860
		// (Invoke) Token: 0x0600CF14 RID: 53012
		public delegate void LoadFinishedAction();
	}
}
