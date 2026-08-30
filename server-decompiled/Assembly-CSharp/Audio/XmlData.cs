using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
	// Token: 0x02001B10 RID: 6928
	public class XmlData
	{
		// Token: 0x0600D01D RID: 53277 RVA: 0x004BC14C File Offset: 0x004BA34C
		public XmlData()
		{
			this.soundGroupName = "Invalid";
			this.maxVoices = 1;
			this.maxVoicesPerEntity = 5;
			this.audioClipMap = new List<ClipSourceMap>();
			this.noiseData = new NoiseData();
			this.localCrouchVolumeScale = 0.5f;
			this.crouchNoiseScale = 0.5f;
			this.noiseScale = 1f;
			this.maxRepeatRate = 0.1f;
			this.prioritizeNewNodes = false;
			this.stopOldestNode = false;
			this.voicesPlaying = 0;
			this.lastRecordedPlayTime = Time.time;
			this.lastClipLenght = 0f;
			this.maxVolume = 1f;
			this.sequence = false;
			this.runningVolumeScale = 1f;
			this.lowestPitch = 1f;
			this.highestPitch = 1f;
			this.distantFadeStart = -1f;
			this.distantFadeEnd = -1f;
			this.channel = XmlData.Channel.Environment;
			this.priority = 99;
		}

		// Token: 0x0600D01E RID: 53278 RVA: 0x004BC250 File Offset: 0x004BA450
		public bool CanAddANewVoice()
		{
			if (this.maxRepeatRate > 0f)
			{
				float time = Time.time;
				float num = time - this.lastRecordedPlayTime;
				if (num < this.maxRepeatRate)
				{
					return false;
				}
				float num2 = this.maxRepeatRate;
				if (this.prioritizeNewNodes && this.lastClipLenght > 0f)
				{
					num2 = this.lastClipLenght;
				}
				this.voicesPlaying = Utils.FastClamp(this.voicesPlaying - (int)(num / num2), 0, 999);
				if (this.voicesPlaying >= this.maxVoices)
				{
					if (this.prioritizeNewNodes)
					{
						this.stopOldestNode = true;
						return true;
					}
					return false;
				}
				else
				{
					this.voicesPlaying++;
					this.lastRecordedPlayTime = time;
				}
			}
			return true;
		}

		// Token: 0x0600D01F RID: 53279 RVA: 0x004BC2FD File Offset: 0x004BA4FD
		public List<ClipSourceMap> GetClipList()
		{
			if (Manager.Instance.bUseAltSounds && this.altAudioClipMap != null)
			{
				return this.altAudioClipMap;
			}
			if (this.hasProfanity && GamePrefs.GetBool(EnumGamePrefs.OptionsFilterProfanity))
			{
				return this.cleanClipMap;
			}
			return this.audioClipMap;
		}

		// Token: 0x0600D020 RID: 53280 RVA: 0x004BC33C File Offset: 0x004BA53C
		public ClipSourceMap GetRandomClip()
		{
			List<ClipSourceMap> clipList = this.GetClipList();
			int num = 0;
			int count = clipList.Count;
			if (count > 1)
			{
				if (count == 2)
				{
					num = this.randomLastIndex ^ 1;
				}
				else
				{
					num = Manager.random.RandomRange(count - 1);
					if (num >= this.randomLastIndex)
					{
						num++;
					}
				}
				this.randomLastIndex = num;
			}
			else if (count == 0)
			{
				Log.Warning("No Clips in Audio ClipSourceMap " + this.soundGroupName + ", " + ((this.hasProfanity && GamePrefs.GetBool(EnumGamePrefs.OptionsFilterProfanity)) ? "using 'no profanity' map:" : ""));
				return null;
			}
			return clipList[num];
		}

		// Token: 0x0600D021 RID: 53281 RVA: 0x004BC3D6 File Offset: 0x004BA5D6
		public void AddAltClipSourceMap(ClipSourceMap csm)
		{
			if (this.altAudioClipMap == null)
			{
				this.altAudioClipMap = new List<ClipSourceMap>();
			}
			this.altAudioClipMap.Add(csm);
		}

		// Token: 0x04009E5D RID: 40541
		public string soundGroupName;

		// Token: 0x04009E5E RID: 40542
		public int maxVoices;

		// Token: 0x04009E5F RID: 40543
		public List<ClipSourceMap> audioClipMap;

		// Token: 0x04009E60 RID: 40544
		public List<ClipSourceMap> altAudioClipMap;

		// Token: 0x04009E61 RID: 40545
		public List<ClipSourceMap> cleanClipMap;

		// Token: 0x04009E62 RID: 40546
		public NoiseData noiseData;

		// Token: 0x04009E63 RID: 40547
		public float localCrouchVolumeScale;

		// Token: 0x04009E64 RID: 40548
		public float runningVolumeScale;

		// Token: 0x04009E65 RID: 40549
		public float crouchNoiseScale;

		// Token: 0x04009E66 RID: 40550
		public float noiseScale;

		// Token: 0x04009E67 RID: 40551
		public float maxRepeatRate;

		// Token: 0x04009E68 RID: 40552
		public bool prioritizeNewNodes;

		// Token: 0x04009E69 RID: 40553
		public bool stopOldestNode;

		// Token: 0x04009E6A RID: 40554
		public int voicesPlaying;

		// Token: 0x04009E6B RID: 40555
		public float lastRecordedPlayTime;

		// Token: 0x04009E6C RID: 40556
		public float lastClipLenght;

		// Token: 0x04009E6D RID: 40557
		public bool playImmediate;

		// Token: 0x04009E6E RID: 40558
		public bool sequence;

		// Token: 0x04009E6F RID: 40559
		public float maxVolume;

		// Token: 0x04009E70 RID: 40560
		public float lowestPitch;

		// Token: 0x04009E71 RID: 40561
		public float highestPitch;

		// Token: 0x04009E72 RID: 40562
		public float distantFadeStart;

		// Token: 0x04009E73 RID: 40563
		public float distantFadeEnd;

		// Token: 0x04009E74 RID: 40564
		public int maxVoicesPerEntity;

		// Token: 0x04009E75 RID: 40565
		public bool hasProfanity;

		// Token: 0x04009E76 RID: 40566
		public XmlData.Channel channel;

		// Token: 0x04009E77 RID: 40567
		public int priority;

		// Token: 0x04009E78 RID: 40568
		public bool vibratesController = true;

		// Token: 0x04009E79 RID: 40569
		public float vibrationStrengthMultiplier = 1f;

		// Token: 0x04009E7A RID: 40570
		[PublicizedFrom(EAccessModifier.Private)]
		public int randomLastIndex;

		// Token: 0x02001B11 RID: 6929
		public enum Channel
		{
			// Token: 0x04009E7C RID: 40572
			Mouth,
			// Token: 0x04009E7D RID: 40573
			Environment
		}
	}
}
