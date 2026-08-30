using System;

// Token: 0x020013C8 RID: 5064
public class DictionaryNameIdMapping
{
	// Token: 0x06009ECD RID: 40653 RVA: 0x003BCCFC File Offset: 0x003BAEFC
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

	// Token: 0x06009ECE RID: 40654 RVA: 0x003BCD3B File Offset: 0x003BAF3B
	public void Clear()
	{
		this.nextId = 0;
		this.namesToIds.Clear();
	}

	// Token: 0x06009ECF RID: 40655 RVA: 0x003BCD50 File Offset: 0x003BAF50
	public int FindId(string _name)
	{
		int num;
		this.namesToIds.TryGetValue(_name, out num);
		return num;
	}

	// Token: 0x04007817 RID: 30743
	public const int cIDNone = 0;

	// Token: 0x04007818 RID: 30744
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextId;

	// Token: 0x04007819 RID: 30745
	[PublicizedFrom(EAccessModifier.Private)]
	public CaseInsensitiveStringDictionary<int> namesToIds = new CaseInsensitiveStringDictionary<int>();
}
