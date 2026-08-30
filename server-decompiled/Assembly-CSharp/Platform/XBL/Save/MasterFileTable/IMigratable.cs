using System;

namespace Platform.XBL.Save.MasterFileTable
{
	// Token: 0x02001C63 RID: 7267
	public interface IMigratable
	{
		// Token: 0x17001AA9 RID: 6825
		// (get) Token: 0x0600D72E RID: 55086
		ushort Version { get; }

		// Token: 0x0600D72F RID: 55087
		void Write(PooledBinaryWriter writer);

		// Token: 0x0600D730 RID: 55088
		void Read(PooledBinaryReader reader);
	}
}
