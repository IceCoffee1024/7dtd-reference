using System;

// Token: 0x02001415 RID: 5141
public class DictionaryNameIdMapping
{
	// Token: 0x0600A0CD RID: 41165 RVA: 0x003C8FE0 File Offset: 0x003C71E0
	public int Add(string _name)
	{
		int num;
		this.namesToIds.TryGetValue(_name, out num);
		if (num == 0)
		{
			int num2 = this.nextId + 1;
			this.nextId = num2;
			num = num2;
			this.namesToIds[_name] = num;
		}
		return num;
	}

	// Token: 0x0600A0CE RID: 41166 RVA: 0x003C901F File Offset: 0x003C721F
	public void Clear()
	{
		this.nextId = 0;
		this.namesToIds.Clear();
	}

	// Token: 0x0600A0CF RID: 41167 RVA: 0x003C9034 File Offset: 0x003C7234
	public int FindId(string _name)
	{
		int num;
		this.namesToIds.TryGetValue(_name, out num);
		return num;
	}

	// Token: 0x040079AC RID: 31148
	public const int cIDNone = 0;

	// Token: 0x040079AD RID: 31149
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextId;

	// Token: 0x040079AE RID: 31150
	[PublicizedFrom(EAccessModifier.Private)]
	public CaseInsensitiveStringDictionary<int> namesToIds = new CaseInsensitiveStringDictionary<int>();
}
