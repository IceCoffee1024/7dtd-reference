using System;

// Token: 0x02000BCC RID: 3020
public interface ITileArea<T> where T : class
{
	// Token: 0x1700097D RID: 2429
	// (get) Token: 0x06005B0D RID: 23309
	TileAreaConfig Config { get; }

	// Token: 0x1700097E RID: 2430
	T this[int _tileX, int _tileZ] { get; }

	// Token: 0x1700097F RID: 2431
	T this[uint _key] { get; }

	// Token: 0x06005B10 RID: 23312 RVA: 0x000027FC File Offset: 0x000009FC
	void Cleanup()
	{
	}
}
