using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
	// Token: 0x02001AE9 RID: 6889
	public class XmlData
	{
		// Token: 0x0600CE94 RID: 52884 RVA: 0x004B5D58 File Offset: 0x004B3F58
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

		// Token: 0x0600CE95 RID: 52885 RVA: 0x004B5E5C File Offset: 0x004B405C
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

		// Token: 0x0600CE96 RID: 52886 RVA: 0x004B5F09 File Offset: 0x004B4109
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

		// Token: 0x0600CE97 RID: 52887 RVA: 0x004B5F48 File Offset: 0x004B4148
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

		// Token: 0x0600CE98 RID: 52888 RVA: 0x004B5FE2 File Offset: 0x004B41E2
		public void AddAltClipSourceMap(ClipSourceMap csm)
		{
			if (this.altAudioClipMap == null)
			{
				this.altAudioClipMap = new List<ClipSourceMap>();
			}
			this.altAudioClipMap.Add(csm);
		}

		// Token: 0x04009D2C RID: 40236
		public string soundGroupName;

		// Token: 0x04009D2D RID: 40237
		public int maxVoices;

		// Token: 0x04009D2E RID: 40238
		public List<ClipSourceMap> audioClipMap;

		// Token: 0x04009D2F RID: 40239
		public List<ClipSourceMap> altAudioClipMap;

		// Token: 0x04009D30 RID: 40240
		public List<ClipSourceMap> cleanClipMap;

		// Token: 0x04009D31 RID: 40241
		public NoiseData noiseData;

		// Token: 0x04009D32 RID: 40242
		public float localCrouchVolumeScale;

		// Token: 0x04009D33 RID: 40243
		public float runningVolumeScale;

		// Token: 0x04009D34 RID: 40244
		public float crouchNoiseScale;

		// Token: 0x04009D35 RID: 40245
		public float noiseScale;

		// Token: 0x04009D36 RID: 40246
		public float maxRepeatRate;

		// Token: 0x04009D37 RID: 40247
		public bool prioritizeNewNodes;

		// Token: 0x04009D38 RID: 40248
		public bool stopOldestNode;

		// Token: 0x04009D39 RID: 40249
		public int voicesPlaying;

		// Token: 0x04009D3A RID: 40250
		public float lastRecordedPlayTime;

		// Token: 0x04009D3B RID: 40251
		public float lastClipLenght;

		// Token: 0x04009D3C RID: 40252
		public bool playImmediate;

		// Token: 0x04009D3D RID: 40253
		public bool sequence;

		// Token: 0x04009D3E RID: 40254
		public float maxVolume;

		// Token: 0x04009D3F RID: 40255
		public float lowestPitch;

		// Token: 0x04009D40 RID: 40256
		public float highestPitch;

		// Token: 0x04009D41 RID: 40257
		public float distantFadeStart;

		// Token: 0x04009D42 RID: 40258
		public float distantFadeEnd;

		// Token: 0x04009D43 RID: 40259
		public int maxVoicesPerEntity;

		// Token: 0x04009D44 RID: 40260
		public bool hasProfanity;

		// Token: 0x04009D45 RID: 40261
		public XmlData.Channel channel;

		// Token: 0x04009D46 RID: 40262
		public int priority;

		// Token: 0x04009D47 RID: 40263
		public bool vibratesController = true;

		// Token: 0x04009D48 RID: 40264
		public float vibrationStrengthMultiplier = 1f;

		// Token: 0x04009D49 RID: 40265
		[PublicizedFrom(EAccessModifier.Private)]
		public int randomLastIndex;

		// Token: 0x02001AEA RID: 6890
		public enum Channel
		{
			// Token: 0x04009D4B RID: 40267
			Mouth,
			// Token: 0x04009D4C RID: 40268
			Environment
		}
	}
}
