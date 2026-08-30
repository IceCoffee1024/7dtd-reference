using System;

namespace PrefabVolumes
{
	// Token: 0x0200189A RID: 6298
	public class PrefabWallVolume : PrefabVolumeAbs<PrefabWallVolume>
	{
		// Token: 0x1700179D RID: 6045
		// (get) Token: 0x0600C204 RID: 49668 RVA: 0x00046C7A File Offset: 0x00044E7A
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Wall;
			}
		}

		// Token: 0x1700179E RID: 6046
		// (get) Token: 0x0600C205 RID: 49669 RVA: 0x0004A832 File Offset: 0x00048A32
		public override int SerializedSize
		{
			get
			{
				return 25;
			}
		}
	}
}
