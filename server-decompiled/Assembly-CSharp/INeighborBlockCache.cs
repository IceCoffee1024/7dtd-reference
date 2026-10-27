using System;

// Token: 0x02000B2E RID: 2862
public interface INeighborBlockCache
{
	// Token: 0x060055B8 RID: 21944
	void Init(int _bX, int _bZ);

	// Token: 0x060055B9 RID: 21945
	void Clear();

	// Token: 0x060055BA RID: 21946
	IChunk GetChunk(int x, int z);

	// Token: 0x060055BB RID: 21947
	IChunk GetNeighborChunk(int x, int z);

	// Token: 0x060055BC RID: 21948
	BlockValue Get(int absy);

	// Token: 0x060055BD RID: 21949
	BlockValue Get(int relx, int absy, int relz);

	// Token: 0x060055BE RID: 21950
	byte GetStab(int relx, int absy, int relz);

	// Token: 0x060055BF RID: 21951
	bool IsWater(int relx, int absy, int relz);

	// Token: 0x060055C0 RID: 21952
	bool IsAir(int relx, int absy, int relz);
}
