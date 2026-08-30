using System;

namespace Platform.XBL.Save.MasterFileTable
{
	// Token: 0x02001C3C RID: 7228
	public interface IMigratable
	{
		// Token: 0x17001A37 RID: 6711
		// (get) Token: 0x0600D59E RID: 54686
		ushort Version { get; }

		// Token: 0x0600D59F RID: 54687
		void Write(PooledBinaryWriter writer);

		// Token: 0x0600D5A0 RID: 54688
		void Read(PooledBinaryReader reader);
	}
}
