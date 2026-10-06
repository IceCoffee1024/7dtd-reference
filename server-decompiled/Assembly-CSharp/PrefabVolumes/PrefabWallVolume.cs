using System;

namespace PrefabVolumes
{
	// Token: 0x020018EC RID: 6380
	public class PrefabWallVolume : PrefabVolumeAbs<PrefabWallVolume>
	{
		// Token: 0x170018B1 RID: 6321
		// (get) Token: 0x0600C4E3 RID: 50403 RVA: 0x000470B6 File Offset: 0x000452B6
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Wall;
			}
		}

		// Token: 0x170018B2 RID: 6322
		// (get) Token: 0x0600C4E4 RID: 50404 RVA: 0x0004AC4D File Offset: 0x00048E4D
		public override int SerializedSize
		{
			get
			{
				return 25;
			}
		}
	}
}
