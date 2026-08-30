using System;
using System.Collections.Generic;

// Token: 0x020011F4 RID: 4596
public class GSRequestData
{
	// Token: 0x060092DA RID: 37594 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public GSRequestData GetGSData(string _key)
	{
		return null;
	}

	// Token: 0x060092DB RID: 37595 RVA: 0x00377328 File Offset: 0x00375528
	public int? GetInt(string _key)
	{
		return null;
	}

	// Token: 0x060092DC RID: 37596 RVA: 0x00377340 File Offset: 0x00375540
	public float? GetFloat(string _key)
	{
		return null;
	}

	// Token: 0x060092DD RID: 37597 RVA: 0x000027FC File Offset: 0x000009FC
	public void Add(string _key, object _value)
	{
	}

	// Token: 0x060092DE RID: 37598 RVA: 0x000027FC File Offset: 0x000009FC
	public void AddNumber(string _key, double _value)
	{
	}

	// Token: 0x060092DF RID: 37599 RVA: 0x000027FC File Offset: 0x000009FC
	public void AddNumber(string _key, int _value)
	{
	}

	// Token: 0x060092E0 RID: 37600 RVA: 0x000027FC File Offset: 0x000009FC
	public void AddString(string _key, string _value)
	{
	}

	// Token: 0x060092E1 RID: 37601 RVA: 0x000027FC File Offset: 0x000009FC
	public void AddBoolean(string _key, bool _value)
	{
	}

	// Token: 0x060092E2 RID: 37602 RVA: 0x000027FC File Offset: 0x000009FC
	public void AddObject(string _key, GSRequestData _value)
	{
	}

	// Token: 0x04006DC9 RID: 28105
	public IEnumerable<KeyValuePair<string, object>> BaseData;

	// Token: 0x04006DCA RID: 28106
	public string JSON;
}
