using System;

// Token: 0x02000B41 RID: 2881
public class ChunkProviderParameter
{
	// Token: 0x060056CF RID: 22223 RVA: 0x002170E4 File Offset: 0x002152E4
	public ChunkProviderParameter(int _id, string _name, float _val, float _minVal, float _maxVal)
	{
		this.id = _id;
		this.name = _name;
		this.val = _val;
		this.minVal = _minVal;
		this.maxVal = _maxVal;
	}

	// Token: 0x04004366 RID: 17254
	public int id;

	// Token: 0x04004367 RID: 17255
	public string name;

	// Token: 0x04004368 RID: 17256
	public float val;

	// Token: 0x04004369 RID: 17257
	public float minVal;

	// Token: 0x0400436A RID: 17258
	public float maxVal;
}
