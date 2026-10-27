using System;

// Token: 0x02000BEB RID: 3051
public class TileAreaSimple<T> : ITileArea<T> where T : class
{
	// Token: 0x06005B56 RID: 23382 RVA: 0x00234BBF File Offset: 0x00232DBF
	public TileAreaSimple(T[,] _data = null)
	{
		this.data = _data;
	}

	// Token: 0x170009AB RID: 2475
	public T this[uint _key]
	{
		get
		{
			return this.data[0, 0];
		}
	}

	// Token: 0x170009AC RID: 2476
	public T this[int _tileX, int _tileZ]
	{
		get
		{
			return this.data[0, 0];
		}
	}

	// Token: 0x170009AD RID: 2477
	// (get) Token: 0x06005B59 RID: 23385 RVA: 0x00234BE0 File Offset: 0x00232DE0
	public TileAreaConfig Config
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	// Token: 0x0400471C RID: 18204
	[PublicizedFrom(EAccessModifier.Private)]
	public T[,] data;
}
