using System;

// Token: 0x02000B4C RID: 2892
public class ChunkProviderParameter
{
	// Token: 0x060056F4 RID: 22260 RVA: 0x00218640 File Offset: 0x00216840
	public ChunkProviderParameter(int _id, string _name, float _val, float _minVal, float _maxVal)
	{
		this.id = _id;
		this.name = _name;
		this.val = _val;
		this.minVal = _minVal;
		this.maxVal = _maxVal;
	}

	// Token: 0x0400438B RID: 17291
	public int id;

	// Token: 0x0400438C RID: 17292
	public string name;

	// Token: 0x0400438D RID: 17293
	public float val;

	// Token: 0x0400438E RID: 17294
	public float minVal;

	// Token: 0x0400438F RID: 17295
	public float maxVal;
}
