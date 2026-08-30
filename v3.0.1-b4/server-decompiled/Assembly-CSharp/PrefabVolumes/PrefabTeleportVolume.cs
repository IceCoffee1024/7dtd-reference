using System;

namespace PrefabVolumes
{
	// Token: 0x02001898 RID: 6296
	public class PrefabTeleportVolume : PrefabVolumeAbs<PrefabTeleportVolume>
	{
		// Token: 0x17001799 RID: 6041
		// (get) Token: 0x0600C1FE RID: 49662 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Teleport;
			}
		}

		// Token: 0x1700179A RID: 6042
		// (get) Token: 0x0600C1FF RID: 49663 RVA: 0x0004A832 File Offset: 0x00048A32
		public override int SerializedSize
		{
			get
			{
				return 25;
			}
		}
	}
}
