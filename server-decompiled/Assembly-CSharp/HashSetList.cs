using System;
using System.Collections.Generic;

// Token: 0x0200144B RID: 5195
public class HashSetList<T>
{
	// Token: 0x0600A2AC RID: 41644 RVA: 0x003D101F File Offset: 0x003CF21F
	public void Add(T _value)
	{
		if (this.hashSet.Add(_value))
		{
			this.list.Add(_value);
		}
	}

	// Token: 0x0600A2AD RID: 41645 RVA: 0x003D103B File Offset: 0x003CF23B
	public void Remove(T _value)
	{
		if (this.hashSet.Remove(_value))
		{
			this.list.Remove(_value);
		}
	}

	// Token: 0x0600A2AE RID: 41646 RVA: 0x003D1058 File Offset: 0x003CF258
	public void Clear()
	{
		this.list.Clear();
		this.hashSet.Clear();
	}

	// Token: 0x04007A61 RID: 31329
	public HashSet<T> hashSet = new HashSet<T>();

	// Token: 0x04007A62 RID: 31330
	public List<T> list = new List<T>();
}
