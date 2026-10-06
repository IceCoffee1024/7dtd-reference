using System;

// Token: 0x02000BE0 RID: 3040
public class TileAreaSimple<T> : ITileArea<T> where T : class
{
	// Token: 0x06005B31 RID: 23345 RVA: 0x00233663 File Offset: 0x00231863
	public TileAreaSimple(T[,] _data = null)
	{
		this.data = _data;
	}

	// Token: 0x170009A5 RID: 2469
	public T this[uint _key]
	{
		get
		{
			return this.data[0, 0];
		}
	}

	// Token: 0x170009A6 RID: 2470
	public T this[int _tileX, int _tileZ]
	{
		get
		{
			return this.data[0, 0];
		}
	}

	// Token: 0x170009A7 RID: 2471
	// (get) Token: 0x06005B34 RID: 23348 RVA: 0x00233684 File Offset: 0x00231884
	public TileAreaConfig Config
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x040046F7 RID: 18167
	[PublicizedFrom(EAccessModifier.Private)]
	public T[,] data;
}
