using System;

// Token: 0x02000584 RID: 1412
public interface ILockContext
{
	// Token: 0x06002E3C RID: 11836
	void Write(PooledBinaryWriter _bw);

	// Token: 0x06002E3D RID: 11837
	void Read(PooledBinaryReader _br);
}
