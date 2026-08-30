using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic.Legacy.ObjectModel
{
	// Token: 0x02001A9B RID: 6811
	public class MusicGroup : EnumDictionary<ThreatLevelLegacyType, ThreatLevel>
	{
		// Token: 0x0600CDA6 RID: 52646 RVA: 0x004AC670 File Offset: 0x004AA870
		public MusicGroup(int _sampleRate, byte _hbLength)
		{
			this.SampleRate = _sampleRate;
			this.HBLength = _hbLength;
			this.ConfigIDs = new List<int>();
		}

		// Token: 0x0600CDA7 RID: 52647 RVA: 0x004AC691 File Offset: 0x004AA891
		public static void InitStatic()
		{
			MusicGroup.AllGroups = new List<MusicGroup>();
		}

		// Token: 0x0600CDA8 RID: 52648 RVA: 0x004AC69D File Offset: 0x004AA89D
		public static void Cleanup()
		{
			if (MusicGroup.AllGroups != null)
			{
				MusicGroup.AllGroups.Clear();
			}
		}

		// Token: 0x04009BCE RID: 39886
		public static List<MusicGroup> AllGroups;

		// Token: 0x04009BCF RID: 39887
		public List<int> ConfigIDs;

		// Token: 0x04009BD0 RID: 39888
		public readonly int SampleRate;

		// Token: 0x04009BD1 RID: 39889
		public readonly byte HBLength;
	}
}
