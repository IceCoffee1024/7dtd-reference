using System;

namespace PrefabVolumes
{
	// Token: 0x02001899 RID: 6297
	public class PrefabInfoVolume : PrefabVolumeAbs<PrefabInfoVolume>
	{
		// Token: 0x1700179B RID: 6043
		// (get) Token: 0x0600C201 RID: 49665 RVA: 0x0002EEAC File Offset: 0x0002D0AC
		public override PrefabVolumeAbs.EVolumeType VolumeType
		{
			get
			{
				return PrefabVolumeAbs.EVolumeType.Info;
			}
		}

		// Token: 0x1700179C RID: 6044
		// (get) Token: 0x0600C202 RID: 49666 RVA: 0x0004A832 File Offset: 0x00048A32
		public override int SerializedSize
		{
			get
			{
				return 25;
			}
		}
	}
}
