using System;
using System.Collections.Generic;

// Token: 0x02001439 RID: 5177
public class HashSetList<T>
{
	// Token: 0x0600A254 RID: 41556 RVA: 0x003CEC1B File Offset: 0x003CCE1B
	public void Add(T _value)
	{
		if (this.hashSet.Add(_value))
		{
			this.list.Add(_value);
		}
	}

	// Token: 0x0600A255 RID: 41557 RVA: 0x003CEC37 File Offset: 0x003CCE37
	public void Remove(T _value)
	{
		if (this.hashSet.Remove(_value))
		{
			this.list.Remove(_value);
		}
	}

	// Token: 0x0600A256 RID: 41558 RVA: 0x003CEC54 File Offset: 0x003CCE54
	public void Clear()
	{
		this.list.Clear();
		this.hashSet.Clear();
	}

	// Token: 0x04007A20 RID: 31264
	public HashSet<T> hashSet = new HashSet<T>();

	// Token: 0x04007A21 RID: 31265
	public List<T> list = new List<T>();
}
