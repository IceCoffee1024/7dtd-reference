using System;

// Token: 0x0200057F RID: 1407
public interface ILockContext
{
	// Token: 0x06002E0E RID: 11790
	void Write(PooledBinaryWriter _bw);

	// Token: 0x06002E0F RID: 11791
	void Read(PooledBinaryReader _br);
}
