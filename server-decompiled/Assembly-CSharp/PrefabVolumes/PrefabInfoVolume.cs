using System;

namespace PrefabVolumes
{
	// Token: 0x0200191C RID: 6428
	public class PrefabInfoVolume : PrefabVolumeAbs<PrefabInfoVolume>
	{
		// Token: 0x170018F7 RID: 6391
		// (get) Token: 0x0600C5F4 RID: 50676 RVA: 0x0002F260 File Offset: 0x0002D460
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Info;
			}
		}

		// Token: 0x170018F8 RID: 6392
		// (get) Token: 0x0600C5F5 RID: 50677 RVA: 0x0004AC0D File Offset: 0x00048E0D
		public override int SerializedSize
		{
			get
			{
				return 25;
			}
		}
	}
}
