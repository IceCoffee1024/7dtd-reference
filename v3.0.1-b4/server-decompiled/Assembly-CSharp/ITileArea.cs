using System;

// Token: 0x02000BBA RID: 3002
public interface ITileArea<T> where T : class
{
	// Token: 0x1700096B RID: 2411
	// (get) Token: 0x06005A85 RID: 23173
	TileAreaConfig Config { get; }

	// Token: 0x1700096C RID: 2412
	T this[int _tileX, int _tileZ] { get; }

	// Token: 0x1700096D RID: 2413
	T this[uint _key] { get; }

	// Token: 0x06005A88 RID: 23176 RVA: 0x000027FC File Offset: 0x000009FC
	void Cleanup()
	{
	}
}
