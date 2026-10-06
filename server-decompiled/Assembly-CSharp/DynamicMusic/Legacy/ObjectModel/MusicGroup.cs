using System;
using System.Collections.Generic;
using MusicUtils.Enums;

namespace DynamicMusic.Legacy.ObjectModel
{
	// Token: 0x02001AC7 RID: 6855
	public class MusicGroup : EnumDictionary<ThreatLevelLegacyType, ThreatLevel>
	{
		// Token: 0x0600CEFE RID: 52990 RVA: 0x004B0B3C File Offset: 0x004AED3C
		public MusicGroup(int _sampleRate, byte _hbLength)
		{
			this.SampleRate = _sampleRate;
			this.HBLength = _hbLength;
			this.ConfigIDs = new List<int>();
		}

		// Token: 0x0600CEFF RID: 52991 RVA: 0x004B0B5D File Offset: 0x004AED5D
		public static void InitStatic()
		{
			MusicGroup.AllGroups = new List<MusicGroup>();
		}

		// Token: 0x0600CF00 RID: 52992 RVA: 0x004B0B69 File Offset: 0x004AED69
		public static void Cleanup()
		{
			if (MusicGroup.AllGroups != null)
			{
				MusicGroup.AllGroups.Clear();
			}
		}

		// Token: 0x04009C94 RID: 40084
		public static List<MusicGroup> AllGroups;

		// Token: 0x04009C95 RID: 40085
		public List<int> ConfigIDs;

		// Token: 0x04009C96 RID: 40086
		public readonly int SampleRate;

		// Token: 0x04009C97 RID: 40087
		public readonly byte HBLength;
	}
}
