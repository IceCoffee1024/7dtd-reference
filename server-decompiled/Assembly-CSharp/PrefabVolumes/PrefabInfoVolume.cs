using System;

namespace PrefabVolumes
{
	// Token: 0x020018BF RID: 6335
	public class PrefabInfoVolume : PrefabVolumeAbs<PrefabInfoVolume>
	{
		// Token: 0x1700180D RID: 6157
		// (get) Token: 0x0600C388 RID: 50056 RVA: 0x0002F168 File Offset: 0x0002D368
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Info;
			}
		}

		// Token: 0x1700180E RID: 6158
		// (get) Token: 0x0600C389 RID: 50057 RVA: 0x0004AC4A File Offset: 0x00048E4A
		public override int SerializedSize
		{
			get
			{
				return 25;
			}
		}
	}
}
