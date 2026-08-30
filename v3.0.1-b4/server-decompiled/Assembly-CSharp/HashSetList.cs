using System;
using System.Collections.Generic;

// Token: 0x020013FE RID: 5118
public class HashSetList<T>
{
	// Token: 0x0600A0AC RID: 41132 RVA: 0x003C4D3B File Offset: 0x003C2F3B
	public void Add(T _value)
	{
		if (this.hashSet.Add(_value))
		{
			this.list.Add(_value);
		}
	}

	// Token: 0x0600A0AD RID: 41133 RVA: 0x003C4D57 File Offset: 0x003C2F57
	public void Remove(T _value)
	{
		if (this.hashSet.Remove(_value))
		{
			this.list.Remove(_value);
		}
	}

	// Token: 0x0600A0AE RID: 41134 RVA: 0x003C4D74 File Offset: 0x003C2F74
	public void Clear()
	{
		this.list.Clear();
		this.hashSet.Clear();
	}

	// Token: 0x040078CC RID: 30924
	public HashSet<T> hashSet = new HashSet<T>();

	// Token: 0x040078CD RID: 30925
	public List<T> list = new List<T>();
}
