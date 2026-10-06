using System;

// Token: 0x02000BDE RID: 3038
public interface ITileArea<T> where T : class
{
	// Token: 0x1700099F RID: 2463
	// (get) Token: 0x06005B26 RID: 23334
	TileAreaConfig Config { get; }

	// Token: 0x170009A0 RID: 2464
	T this[int _tileX, int _tileZ] { get; }

	// Token: 0x170009A1 RID: 2465
	T this[uint _key] { get; }

	// Token: 0x06005B29 RID: 23337 RVA: 0x000027FC File Offset: 0x000009FC
	void Cleanup()
	{
	}
}
