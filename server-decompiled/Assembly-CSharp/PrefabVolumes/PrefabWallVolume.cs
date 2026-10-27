using System;

namespace PrefabVolumes
{
	// Token: 0x0200191D RID: 6429
	public class PrefabWallVolume : PrefabVolumeAbs<PrefabWallVolume>
	{
		// Token: 0x170018F9 RID: 6393
		// (get) Token: 0x0600C5F7 RID: 50679 RVA: 0x00047076 File Offset: 0x00045276
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Wall;
			}
		}

		// Token: 0x170018FA RID: 6394
		// (get) Token: 0x0600C5F8 RID: 50680 RVA: 0x0004AC0D File Offset: 0x00048E0D
		public override int SerializedSize
		{
			get
			{
				return 25;
			}
		}
	}
}
