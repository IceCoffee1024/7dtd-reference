using System;

// Token: 0x02000AFF RID: 2815
public interface INeighborBlockCache
{
	// Token: 0x060054F2 RID: 21746
	void Init(int _bX, int _bZ);

	// Token: 0x060054F3 RID: 21747
	void Clear();

	// Token: 0x060054F4 RID: 21748
	IChunk GetChunk(int x, int z);

	// Token: 0x060054F5 RID: 21749
	IChunk GetNeighborChunk(int x, int z);

	// Token: 0x060054F6 RID: 21750
	BlockValue Get(int absy);

	// Token: 0x060054F7 RID: 21751
	BlockValue Get(int relx, int absy, int relz);

	// Token: 0x060054F8 RID: 21752
	byte GetStab(int relx, int absy, int relz);

	// Token: 0x060054F9 RID: 21753
	bool IsWater(int relx, int absy, int relz);

	// Token: 0x060054FA RID: 21754
	bool IsAir(int relx, int absy, int relz);
}
