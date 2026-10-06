using System;

namespace Platform.XBL.Save.MasterFileTable
{
	// Token: 0x02001C95 RID: 7317
	public interface IMigratable
	{
		// Token: 0x17001B4F RID: 6991
		// (get) Token: 0x0600D8A7 RID: 55463
		ushort Version { get; }

		// Token: 0x0600D8A8 RID: 55464
		void Write(PooledBinaryWriter writer);

		// Token: 0x0600D8A9 RID: 55465
		void Read(PooledBinaryReader reader);
	}
}
