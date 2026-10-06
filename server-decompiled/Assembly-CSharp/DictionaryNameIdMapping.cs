using System;

// Token: 0x02001403 RID: 5123
public class DictionaryNameIdMapping
{
	// Token: 0x0600A075 RID: 41077 RVA: 0x003C6BDC File Offset: 0x003C4DDC
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

	// Token: 0x0600A076 RID: 41078 RVA: 0x003C6C1B File Offset: 0x003C4E1B
	public void Clear()
	{
		this.nextId = 0;
		this.namesToIds.Clear();
	}

	// Token: 0x0600A077 RID: 41079 RVA: 0x003C6C30 File Offset: 0x003C4E30
	public int FindId(string _name)
	{
		int num;
		this.namesToIds.TryGetValue(_name, out num);
		return num;
	}

	// Token: 0x0400796B RID: 31083
	public const int cIDNone = 0;

	// Token: 0x0400796C RID: 31084
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextId;

	// Token: 0x0400796D RID: 31085
	[PublicizedFrom(EAccessModifier.Private)]
	public CaseInsensitiveStringDictionary<int> namesToIds = new CaseInsensitiveStringDictionary<int>();
}
