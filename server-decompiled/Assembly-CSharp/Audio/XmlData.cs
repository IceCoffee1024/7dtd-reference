using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
	// Token: 0x02001B6D RID: 7021
	public class XmlData
	{
		// Token: 0x0600D289 RID: 53897 RVA: 0x004C4140 File Offset: 0x004C2340
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

		// Token: 0x0600D28A RID: 53898 RVA: 0x004C4244 File Offset: 0x004C2444
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

		// Token: 0x0600D28B RID: 53899 RVA: 0x004C42F1 File Offset: 0x004C24F1
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

		// Token: 0x0600D28C RID: 53900 RVA: 0x004C4330 File Offset: 0x004C2530
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

		// Token: 0x0600D28D RID: 53901 RVA: 0x004C43CA File Offset: 0x004C25CA
		public void AddAltClipSourceMap(ClipSourceMap csm)
		{
			if (this.altAudioClipMap == null)
			{
				this.altAudioClipMap = new List<ClipSourceMap>();
			}
			this.altAudioClipMap.Add(csm);
		}

		// Token: 0x0400A00C RID: 40972
		public string soundGroupName;

		// Token: 0x0400A00D RID: 40973
		public int maxVoices;

		// Token: 0x0400A00E RID: 40974
		public List<ClipSourceMap> audioClipMap;

		// Token: 0x0400A00F RID: 40975
		public List<ClipSourceMap> altAudioClipMap;

		// Token: 0x0400A010 RID: 40976
		public List<ClipSourceMap> cleanClipMap;

		// Token: 0x0400A011 RID: 40977
		public NoiseData noiseData;

		// Token: 0x0400A012 RID: 40978
		public float localCrouchVolumeScale;

		// Token: 0x0400A013 RID: 40979
		public float runningVolumeScale;

		// Token: 0x0400A014 RID: 40980
		public float crouchNoiseScale;

		// Token: 0x0400A015 RID: 40981
		public float noiseScale;

		// Token: 0x0400A016 RID: 40982
		public float maxRepeatRate;

		// Token: 0x0400A017 RID: 40983
		public bool prioritizeNewNodes;

		// Token: 0x0400A018 RID: 40984
		public bool stopOldestNode;

		// Token: 0x0400A019 RID: 40985
		public int voicesPlaying;

		// Token: 0x0400A01A RID: 40986
		public float lastRecordedPlayTime;

		// Token: 0x0400A01B RID: 40987
		public float lastClipLenght;

		// Token: 0x0400A01C RID: 40988
		public bool playImmediate;

		// Token: 0x0400A01D RID: 40989
		public bool sequence;

		// Token: 0x0400A01E RID: 40990
		public float maxVolume;

		// Token: 0x0400A01F RID: 40991
		public float lowestPitch;

		// Token: 0x0400A020 RID: 40992
		public float highestPitch;

		// Token: 0x0400A021 RID: 40993
		public float distantFadeStart;

		// Token: 0x0400A022 RID: 40994
		public float distantFadeEnd;

		// Token: 0x0400A023 RID: 40995
		public int maxVoicesPerEntity;

		// Token: 0x0400A024 RID: 40996
		public bool hasProfanity;

		// Token: 0x0400A025 RID: 40997
		public XmlData.Channel channel;

		// Token: 0x0400A026 RID: 40998
		public int priority;

		// Token: 0x0400A027 RID: 40999
		public bool vibratesController = true;

		// Token: 0x0400A028 RID: 41000
		public float vibrationStrengthMultiplier = 1f;

		// Token: 0x0400A029 RID: 41001
		[PublicizedFrom(EAccessModifier.Private)]
		public int randomLastIndex;

		// Token: 0x02001B6E RID: 7022
		public enum Channel
		{
			// Token: 0x0400A02B RID: 41003
			Mouth,
			// Token: 0x0400A02C RID: 41004
			Environment
		}
	}
}
