using System;
using System.Collections.Generic;

// Token: 0x020011D8 RID: 4568
public class GSRequestData
{
	// Token: 0x060091D6 RID: 37334 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public GSRequestData GetGSData(string _key)
	{
		return null;
	}

	// Token: 0x060091D7 RID: 37335 RVA: 0x0037205C File Offset: 0x0037025C
	public int? GetInt(string _key)
	{
		return null;
	}

	// Token: 0x060091D8 RID: 37336 RVA: 0x00372074 File Offset: 0x00370274
	public float? GetFloat(string _key)
	{
		return null;
	}

	// Token: 0x060091D9 RID: 37337 RVA: 0x000027FC File Offset: 0x000009FC
	public void Add(string _key, object _value)
	{
	}

	// Token: 0x060091DA RID: 37338 RVA: 0x000027FC File Offset: 0x000009FC
	public void AddNumber(string _key, double _value)
	{
	}

	// Token: 0x060091DB RID: 37339 RVA: 0x000027FC File Offset: 0x000009FC
	public void AddNumber(string _key, int _value)
	{
	}

	// Token: 0x060091DC RID: 37340 RVA: 0x000027FC File Offset: 0x000009FC
	public void AddString(string _key, string _value)
	{
	}

	// Token: 0x060091DD RID: 37341 RVA: 0x000027FC File Offset: 0x000009FC
	public void AddBoolean(string _key, bool _value)
	{
	}

	// Token: 0x060091DE RID: 37342 RVA: 0x000027FC File Offset: 0x000009FC
	public void AddObject(string _key, GSRequestData _value)
	{
	}

	// Token: 0x04006CF0 RID: 27888
	public IEnumerable<KeyValuePair<string, object>> BaseData;

	// Token: 0x04006CF1 RID: 27889
	public string JSON;
}
