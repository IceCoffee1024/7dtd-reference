using System;

// Token: 0x02000B1D RID: 2845
public class ChunkProviderParameter
{
	// Token: 0x0600562D RID: 22061 RVA: 0x0021113C File Offset: 0x0020F33C
	public ChunkProviderParameter(int _id, string _name, float _val, float _minVal, float _maxVal)
	{
		this.id = _id;
		this.name = _name;
		this.val = _val;
		this.minVal = _minVal;
		this.maxVal = _maxVal;
	}

	// Token: 0x0400428D RID: 17037
	public int id;

	// Token: 0x0400428E RID: 17038
	public string name;

	// Token: 0x0400428F RID: 17039
	public float val;

	// Token: 0x04004290 RID: 17040
	public float minVal;

	// Token: 0x04004291 RID: 17041
	public float maxVal;
}
