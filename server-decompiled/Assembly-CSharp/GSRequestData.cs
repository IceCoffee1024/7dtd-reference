using System;
using System.Collections.Generic;

// Token: 0x0200120C RID: 4620
public class GSRequestData
{
	// Token: 0x06009368 RID: 37736 RVA: 0x000200DA File Offset: 0x0001E2DA
	public GSRequestData GetGSData(string _key)
	{
		return null;
	}

	// Token: 0x06009369 RID: 37737 RVA: 0x0037BB70 File Offset: 0x00379D70
	public int? GetInt(string _key)
	{
		return null;
	}

	// Token: 0x0600936A RID: 37738 RVA: 0x0037BB88 File Offset: 0x00379D88
	public float? GetFloat(string _key)
	{
		return null;
	}

	// Token: 0x0600936B RID: 37739 RVA: 0x000027FC File Offset: 0x000009FC
	public void Add(string _key, object _value)
	{
	}

	// Token: 0x0600936C RID: 37740 RVA: 0x000027FC File Offset: 0x000009FC
	public void AddNumber(string _key, double _value)
	{
	}

	// Token: 0x0600936D RID: 37741 RVA: 0x000027FC File Offset: 0x000009FC
	public void AddNumber(string _key, int _value)
	{
	}

	// Token: 0x0600936E RID: 37742 RVA: 0x000027FC File Offset: 0x000009FC
	public void AddString(string _key, string _value)
	{
	}

	// Token: 0x0600936F RID: 37743 RVA: 0x000027FC File Offset: 0x000009FC
	public void AddBoolean(string _key, bool _value)
	{
	}

	// Token: 0x06009370 RID: 37744 RVA: 0x000027FC File Offset: 0x000009FC
	public void AddObject(string _key, GSRequestData _value)
	{
	}

	// Token: 0x04006E33 RID: 28211
	public IEnumerable<KeyValuePair<string, object>> BaseData;

	// Token: 0x04006E34 RID: 28212
	public string JSON;
}
