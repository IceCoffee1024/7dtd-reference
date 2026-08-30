using System;

// Token: 0x02000BBC RID: 3004
public class TileAreaSimple<T> : ITileArea<T> where T : class
{
	// Token: 0x06005A90 RID: 23184 RVA: 0x0022D5FF File Offset: 0x0022B7FF
	public TileAreaSimple(T[,] _data = null)
	{
		this.data = _data;
	}

	// Token: 0x17000971 RID: 2417
	public T this[uint _key]
	{
		get
		{
			return this.data[0, 0];
		}
	}

	// Token: 0x17000972 RID: 2418
	public T this[int _tileX, int _tileZ]
	{
		get
		{
			return this.data[0, 0];
		}
	}

	// Token: 0x17000973 RID: 2419
	// (get) Token: 0x06005A93 RID: 23187 RVA: 0x0022D620 File Offset: 0x0022B820
	public TileAreaConfig Config
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x0400461C RID: 17948
	[PublicizedFrom(EAccessModifier.Private)]
	public T[,] data;
}
