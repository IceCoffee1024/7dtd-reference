using System;

namespace PrefabVolumes
{
	// Token: 0x0200191B RID: 6427
	public class PrefabTeleportVolume : PrefabVolumeAbs<PrefabTeleportVolume>
	{
		// Token: 0x170018F5 RID: 6389
		// (get) Token: 0x0600C5F1 RID: 50673 RVA: 0x00020119 File Offset: 0x0001E319
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Teleport;
			}
		}

		// Token: 0x170018F6 RID: 6390
		// (get) Token: 0x0600C5F2 RID: 50674 RVA: 0x0004AC0D File Offset: 0x00048E0D
		public override int SerializedSize
		{
			get
			{
				return 25;
			}
		}
	}
}
