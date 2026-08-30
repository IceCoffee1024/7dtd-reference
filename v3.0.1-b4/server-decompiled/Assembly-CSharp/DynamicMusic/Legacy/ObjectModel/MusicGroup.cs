using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic.Legacy.ObjectModel
{
	// Token: 0x02001A74 RID: 6772
	public class MusicGroup : EnumDictionary<ThreatLevelLegacyType, ThreatLevel>
	{
		// Token: 0x0600CC1D RID: 52253 RVA: 0x004A627C File Offset: 0x004A447C
		public MusicGroup(int _sampleRate, byte _hbLength)
		{
			this.SampleRate = _sampleRate;
			this.HBLength = _hbLength;
			this.ConfigIDs = new List<int>();
		}

		// Token: 0x0600CC1E RID: 52254 RVA: 0x004A629D File Offset: 0x004A449D
		public static void InitStatic()
		{
			MusicGroup.AllGroups = new List<MusicGroup>();
		}

		// Token: 0x0600CC1F RID: 52255 RVA: 0x004A62A9 File Offset: 0x004A44A9
		public static void Cleanup()
		{
			if (MusicGroup.AllGroups != null)
			{
				MusicGroup.AllGroups.Clear();
			}
		}

		// Token: 0x04009A9D RID: 39581
		public static List<MusicGroup> AllGroups;

		// Token: 0x04009A9E RID: 39582
		public List<int> ConfigIDs;

		// Token: 0x04009A9F RID: 39583
		public readonly int SampleRate;

		// Token: 0x04009AA0 RID: 39584
		public readonly byte HBLength;
	}
}
