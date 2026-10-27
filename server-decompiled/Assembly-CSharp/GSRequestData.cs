using System;
using System.Collections.Generic;

// Token: 0x0200121C RID: 4636
public class GSRequestData
{
	// Token: 0x060093BE RID: 37822 RVA: 0x000200DA File Offset: 0x0001E2DA
	public GSRequestData GetGSData(string _key)
	{
		return null;
	}

	// Token: 0x060093BF RID: 37823 RVA: 0x0037DCE4 File Offset: 0x0037BEE4
	public int? GetInt(string _key)
	{
		return null;
	}

	// Token: 0x060093C0 RID: 37824 RVA: 0x0037DCFC File Offset: 0x0037BEFC
	public float? GetFloat(string _key)
	{
		return null;
	}

	// Token: 0x060093C1 RID: 37825 RVA: 0x000027FC File Offset: 0x000009FC
	public void Add(string _key, object _value)
	{
	}

	// Token: 0x060093C2 RID: 37826 RVA: 0x000027FC File Offset: 0x000009FC
	public void AddNumber(string _key, double _value)
	{
	}

	// Token: 0x060093C3 RID: 37827 RVA: 0x000027FC File Offset: 0x000009FC
	public void AddNumber(string _key, int _value)
	{
	}

	// Token: 0x060093C4 RID: 37828 RVA: 0x000027FC File Offset: 0x000009FC
	public void AddString(string _key, string _value)
	{
	}

	// Token: 0x060093C5 RID: 37829 RVA: 0x000027FC File Offset: 0x000009FC
	public void AddBoolean(string _key, bool _value)
	{
	}

	// Token: 0x060093C6 RID: 37830 RVA: 0x000027FC File Offset: 0x000009FC
	public void AddObject(string _key, GSRequestData _value)
	{
	}

	// Token: 0x04006E4B RID: 28235
	public IEnumerable<KeyValuePair<string, object>> BaseData;

	// Token: 0x04006E4C RID: 28236
	public string JSON;
}
