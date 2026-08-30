using System;

// Token: 0x02000BCE RID: 3022
public class TileAreaSimple<T> : ITileArea<T> where T : class
{
	// Token: 0x06005B18 RID: 23320 RVA: 0x00230367 File Offset: 0x0022E567
	public TileAreaSimple(T[,] _data = null)
	{
		this.data = _data;
	}

	// Token: 0x17000983 RID: 2435
	public T this[uint _key]
	{
		get
		{
			return this.data[0, 0];
		}
	}

	// Token: 0x17000984 RID: 2436
	public T this[int _tileX, int _tileZ]
	{
		get
		{
			return this.data[0, 0];
		}
	}

	// Token: 0x17000985 RID: 2437
	// (get) Token: 0x06005B1B RID: 23323 RVA: 0x00230388 File Offset: 0x0022E588
	public TileAreaConfig Config
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x040046B8 RID: 18104
	[PublicizedFrom(EAccessModifier.Private)]
	public T[,] data;
}
