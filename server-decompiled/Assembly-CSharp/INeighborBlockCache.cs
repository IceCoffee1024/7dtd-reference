using System;

// Token: 0x02000B11 RID: 2833
public interface INeighborBlockCache
{
	// Token: 0x0600557A RID: 21882
	void Init(int _bX, int _bZ);

	// Token: 0x0600557B RID: 21883
	void Clear();

	// Token: 0x0600557C RID: 21884
	IChunk GetChunk(int x, int z);

	// Token: 0x0600557D RID: 21885
	IChunk GetNeighborChunk(int x, int z);

	// Token: 0x0600557E RID: 21886
	BlockValue Get(int absy);

	// Token: 0x0600557F RID: 21887
	BlockValue Get(int relx, int absy, int relz);

	// Token: 0x06005580 RID: 21888
	byte GetStab(int relx, int absy, int relz);

	// Token: 0x06005581 RID: 21889
	bool IsWater(int relx, int absy, int relz);

	// Token: 0x06005582 RID: 21890
	bool IsAir(int relx, int absy, int relz);
}
