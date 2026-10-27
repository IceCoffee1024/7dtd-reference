using System;

namespace Platform.XBL.Save.MasterFileTable
{
	// Token: 0x02001CC6 RID: 7366
	public interface IMigratable
	{
		// Token: 0x17001B97 RID: 7063
		// (get) Token: 0x0600D9BB RID: 55739
		ushort Version { get; }

		// Token: 0x0600D9BC RID: 55740
		void Write(PooledBinaryWriter writer);

		// Token: 0x0600D9BD RID: 55741
		void Read(PooledBinaryReader reader);
	}
}
