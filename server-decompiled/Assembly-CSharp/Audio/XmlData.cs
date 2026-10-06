using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
	// Token: 0x02001B3C RID: 6972
	public class XmlData
	{
		// Token: 0x0600D175 RID: 53621 RVA: 0x004C0618 File Offset: 0x004BE818
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

		// Token: 0x0600D176 RID: 53622 RVA: 0x004C071C File Offset: 0x004BE91C
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

		// Token: 0x0600D177 RID: 53623 RVA: 0x004C07C9 File Offset: 0x004BE9C9
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

		// Token: 0x0600D178 RID: 53624 RVA: 0x004C0808 File Offset: 0x004BEA08
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

		// Token: 0x0600D179 RID: 53625 RVA: 0x004C08A2 File Offset: 0x004BEAA2
		public void AddAltClipSourceMap(ClipSourceMap csm)
		{
			if (this.altAudioClipMap == null)
			{
				this.altAudioClipMap = new List<ClipSourceMap>();
			}
			this.altAudioClipMap.Add(csm);
		}

		// Token: 0x04009F23 RID: 40739
		public string soundGroupName;

		// Token: 0x04009F24 RID: 40740
		public int maxVoices;

		// Token: 0x04009F25 RID: 40741
		public List<ClipSourceMap> audioClipMap;

		// Token: 0x04009F26 RID: 40742
		public List<ClipSourceMap> altAudioClipMap;

		// Token: 0x04009F27 RID: 40743
		public List<ClipSourceMap> cleanClipMap;

		// Token: 0x04009F28 RID: 40744
		public NoiseData noiseData;

		// Token: 0x04009F29 RID: 40745
		public float localCrouchVolumeScale;

		// Token: 0x04009F2A RID: 40746
		public float runningVolumeScale;

		// Token: 0x04009F2B RID: 40747
		public float crouchNoiseScale;

		// Token: 0x04009F2C RID: 40748
		public float noiseScale;

		// Token: 0x04009F2D RID: 40749
		public float maxRepeatRate;

		// Token: 0x04009F2E RID: 40750
		public bool prioritizeNewNodes;

		// Token: 0x04009F2F RID: 40751
		public bool stopOldestNode;

		// Token: 0x04009F30 RID: 40752
		public int voicesPlaying;

		// Token: 0x04009F31 RID: 40753
		public float lastRecordedPlayTime;

		// Token: 0x04009F32 RID: 40754
		public float lastClipLenght;

		// Token: 0x04009F33 RID: 40755
		public bool playImmediate;

		// Token: 0x04009F34 RID: 40756
		public bool sequence;

		// Token: 0x04009F35 RID: 40757
		public float maxVolume;

		// Token: 0x04009F36 RID: 40758
		public float lowestPitch;

		// Token: 0x04009F37 RID: 40759
		public float highestPitch;

		// Token: 0x04009F38 RID: 40760
		public float distantFadeStart;

		// Token: 0x04009F39 RID: 40761
		public float distantFadeEnd;

		// Token: 0x04009F3A RID: 40762
		public int maxVoicesPerEntity;

		// Token: 0x04009F3B RID: 40763
		public bool hasProfanity;

		// Token: 0x04009F3C RID: 40764
		public XmlData.Channel channel;

		// Token: 0x04009F3D RID: 40765
		public int priority;

		// Token: 0x04009F3E RID: 40766
		public bool vibratesController = true;

		// Token: 0x04009F3F RID: 40767
		public float vibrationStrengthMultiplier = 1f;

		// Token: 0x04009F40 RID: 40768
		[PublicizedFrom(EAccessModifier.Private)]
		public int randomLastIndex;

		// Token: 0x02001B3D RID: 6973
		public enum Channel
		{
			// Token: 0x04009F42 RID: 40770
			Mouth,
			// Token: 0x04009F43 RID: 40771
			Environment
		}
	}
}
