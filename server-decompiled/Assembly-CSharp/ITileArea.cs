using System;

// Token: 0x02000BE9 RID: 3049
public interface ITileArea<T> where T : class
{
	// Token: 0x170009A5 RID: 2469
	// (get) Token: 0x06005B4B RID: 23371
	TileAreaConfig Config { get; }

	// Token: 0x170009A6 RID: 2470
	T this[int _tileX, int _tileZ] { get; }

	// Token: 0x170009A7 RID: 2471
	T this[uint _key] { get; }

	// Token: 0x06005B4E RID: 23374 RVA: 0x000027FC File Offset: 0x000009FC
	void Cleanup()
	{
	}
}
