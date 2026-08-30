using System;

namespace PrefabVolumes
{
	// Token: 0x020018BE RID: 6334
	public class PrefabTeleportVolume : PrefabVolumeAbs<PrefabTeleportVolume>
	{
		// Token: 0x1700180B RID: 6155
		// (get) Token: 0x0600C385 RID: 50053 RVA: 0x00020021 File Offset: 0x0001E221
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Teleport;
			}
		}

		// Token: 0x1700180C RID: 6156
		// (get) Token: 0x0600C386 RID: 50054 RVA: 0x0004AC4A File Offset: 0x00048E4A
		public override int SerializedSize
		{
			get
			{
				return 25;
			}
		}
	}
}
