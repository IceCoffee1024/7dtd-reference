using System;

namespace PrefabVolumes
{
	// Token: 0x020018EB RID: 6379
	public class PrefabInfoVolume : PrefabVolumeAbs<PrefabInfoVolume>
	{
		// Token: 0x170018AF RID: 6319
		// (get) Token: 0x0600C4E0 RID: 50400 RVA: 0x0002F260 File Offset: 0x0002D460
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Info;
			}
		}

		// Token: 0x170018B0 RID: 6320
		// (get) Token: 0x0600C4E1 RID: 50401 RVA: 0x0004AC4D File Offset: 0x00048E4D
		public override int SerializedSize
		{
			get
			{
				return 25;
			}
		}
	}
}
