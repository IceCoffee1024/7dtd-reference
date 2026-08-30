using System;

// Token: 0x020009FF RID: 2559
public interface ISaveDataPrefs
{
	// Token: 0x06004C76 RID: 19574 RVA: 0x001D897F File Offset: 0x001D6B7F
	float GetFloat(string key)
	{
		return this.GetFloat(key, 0f);
	}

	// Token: 0x06004C77 RID: 19575
	float GetFloat(string key, float defaultValue);

	// Token: 0x06004C78 RID: 19576
	void SetFloat(string key, float value);

	// Token: 0x06004C79 RID: 19577 RVA: 0x001D898D File Offset: 0x001D6B8D
	int GetInt(string key)
	{
		return this.GetInt(key, 0);
	}

	// Token: 0x06004C7A RID: 19578
	int GetInt(string key, int defaultValue);

	// Token: 0x06004C7B RID: 19579
	void SetInt(string key, int value);

	// Token: 0x06004C7C RID: 19580 RVA: 0x001D8997 File Offset: 0x001D6B97
	string GetString(string key)
	{
		return this.GetString(key, "");
	}

	// Token: 0x06004C7D RID: 19581
	string GetString(string key, string defaultValue);

	// Token: 0x06004C7E RID: 19582
	void SetString(string key, string value);

	// Token: 0x06004C7F RID: 19583
	bool HasKey(string key);

	// Token: 0x06004C80 RID: 19584
	void DeleteKey(string key);

	// Token: 0x06004C81 RID: 19585
	void DeleteAll();

	// Token: 0x06004C82 RID: 19586
	void Save();

	// Token: 0x06004C83 RID: 19587
	void Load();

	// Token: 0x17000828 RID: 2088
	// (get) Token: 0x06004C84 RID: 19588
	bool CanLoad { get; }
}
