using System;

// Token: 0x02000A13 RID: 2579
public interface ISaveDataPrefs
{
	// Token: 0x06004C69 RID: 19561 RVA: 0x001DAD17 File Offset: 0x001D8F17
	float GetFloat(string key)
	{
		return this.GetFloat(key, 0f);
	}

	// Token: 0x06004C6A RID: 19562
	float GetFloat(string key, float defaultValue);

	// Token: 0x06004C6B RID: 19563
	void SetFloat(string key, float value);

	// Token: 0x06004C6C RID: 19564 RVA: 0x001DAD25 File Offset: 0x001D8F25
	int GetInt(string key)
	{
		return this.GetInt(key, 0);
	}

	// Token: 0x06004C6D RID: 19565
	int GetInt(string key, int defaultValue);

	// Token: 0x06004C6E RID: 19566
	void SetInt(string key, int value);

	// Token: 0x06004C6F RID: 19567 RVA: 0x001DAD2F File Offset: 0x001D8F2F
	string GetString(string key)
	{
		return this.GetString(key, "");
	}

	// Token: 0x06004C70 RID: 19568
	string GetString(string key, string defaultValue);

	// Token: 0x06004C71 RID: 19569
	void SetString(string key, string value);

	// Token: 0x06004C72 RID: 19570
	bool HasKey(string key);

	// Token: 0x06004C73 RID: 19571
	void DeleteKey(string key);

	// Token: 0x06004C74 RID: 19572
	void DeleteAll();

	// Token: 0x06004C75 RID: 19573
	void Save();

	// Token: 0x06004C76 RID: 19574
	void Load();

	// Token: 0x1700084E RID: 2126
	// (get) Token: 0x06004C77 RID: 19575
	bool CanLoad { get; }
}
