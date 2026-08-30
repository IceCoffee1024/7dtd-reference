using System;

// Token: 0x020013E3 RID: 5091
public class DictionaryNameIdMapping
{
	// Token: 0x06009FCA RID: 40906 RVA: 0x003C2040 File Offset: 0x003C0240
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

	// Token: 0x06009FCB RID: 40907 RVA: 0x003C207F File Offset: 0x003C027F
	public void Clear()
	{
		this.nextId = 0;
		this.namesToIds.Clear();
	}

	// Token: 0x06009FCC RID: 40908 RVA: 0x003C2094 File Offset: 0x003C0294
	public int FindId(string _name)
	{
		int num;
		this.namesToIds.TryGetValue(_name, out num);
		return num;
	}

	// Token: 0x040078F0 RID: 30960
	public const int cIDNone = 0;

	// Token: 0x040078F1 RID: 30961
	[PublicizedFrom(EAccessModifier.Private)]
	public int nextId;

	// Token: 0x040078F2 RID: 30962
	[PublicizedFrom(EAccessModifier.Private)]
	public CaseInsensitiveStringDictionary<int> namesToIds = new CaseInsensitiveStringDictionary<int>();
}
