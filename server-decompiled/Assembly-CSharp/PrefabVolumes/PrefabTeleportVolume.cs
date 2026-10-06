using System;

namespace PrefabVolumes
{
	// Token: 0x020018EA RID: 6378
	public class PrefabTeleportVolume : PrefabVolumeAbs<PrefabTeleportVolume>
	{
		// Token: 0x170018AD RID: 6317
		// (get) Token: 0x0600C4DD RID: 50397 RVA: 0x00020119 File Offset: 0x0001E319
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Teleport;
			}
		}

		// Token: 0x170018AE RID: 6318
		// (get) Token: 0x0600C4DE RID: 50398 RVA: 0x0004AC4D File Offset: 0x00048E4D
		public override int SerializedSize
		{
			get
			{
				return 25;
			}
		}
	}
}
