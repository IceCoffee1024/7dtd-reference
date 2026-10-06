using System;

// Token: 0x02000B23 RID: 2851
public interface INeighborBlockCache
{
	// Token: 0x06005593 RID: 21907
	void Init(int _bX, int _bZ);

	// Token: 0x06005594 RID: 21908
	void Clear();

	// Token: 0x06005595 RID: 21909
	IChunk GetChunk(int x, int z);

	// Token: 0x06005596 RID: 21910
	IChunk GetNeighborChunk(int x, int z);

	// Token: 0x06005597 RID: 21911
	BlockValue Get(int absy);

	// Token: 0x06005598 RID: 21912
	BlockValue Get(int relx, int absy, int relz);

	// Token: 0x06005599 RID: 21913
	byte GetStab(int relx, int absy, int relz);

	// Token: 0x0600559A RID: 21914
	bool IsWater(int relx, int absy, int relz);

	// Token: 0x0600559B RID: 21915
	bool IsAir(int relx, int absy, int relz);
}
