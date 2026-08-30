using System;

namespace PrefabVolumes
{
	// Token: 0x020018C0 RID: 6336
	public class PrefabWallVolume : PrefabVolumeAbs<PrefabWallVolume>
	{
		// Token: 0x1700180F RID: 6159
		// (get) Token: 0x0600C38B RID: 50059 RVA: 0x00046EDA File Offset: 0x000450DA
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Wall;
			}
		}

		// Token: 0x17001810 RID: 6160
		// (get) Token: 0x0600C38C RID: 50060 RVA: 0x0004AC4A File Offset: 0x00048E4A
		public override int SerializedSize
		{
			get
			{
				return 25;
			}
		}
	}
}
