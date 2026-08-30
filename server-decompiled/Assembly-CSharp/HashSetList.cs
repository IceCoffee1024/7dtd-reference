using System;
using System.Collections.Generic;

// Token: 0x02001419 RID: 5145
public class HashSetList<T>
{
	// Token: 0x0600A1A9 RID: 41385 RVA: 0x003CA07F File Offset: 0x003C827F
	public void Add(T _value)
	{
		if (this.hashSet.Add(_value))
		{
			this.list.Add(_value);
		}
	}

	// Token: 0x0600A1AA RID: 41386 RVA: 0x003CA09B File Offset: 0x003C829B
	public void Remove(T _value)
	{
		if (this.hashSet.Remove(_value))
		{
			this.list.Remove(_value);
		}
	}

	// Token: 0x0600A1AB RID: 41387 RVA: 0x003CA0B8 File Offset: 0x003C82B8
	public void Clear()
	{
		this.list.Clear();
		this.hashSet.Clear();
	}

	// Token: 0x040079A5 RID: 31141
	public HashSet<T> hashSet = new HashSet<T>();

	// Token: 0x040079A6 RID: 31142
	public List<T> list = new List<T>();
}
