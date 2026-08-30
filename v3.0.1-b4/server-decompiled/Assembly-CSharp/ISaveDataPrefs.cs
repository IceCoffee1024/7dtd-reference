using System;

// Token: 0x020009EE RID: 2542
public interface ISaveDataPrefs
{
	// Token: 0x06004BF0 RID: 19440 RVA: 0x001D6197 File Offset: 0x001D4397
	float GetFloat(string key)
	{
		return this.GetFloat(key, 0f);
	}

	// Token: 0x06004BF1 RID: 19441
	float GetFloat(string key, float defaultValue);

	// Token: 0x06004BF2 RID: 19442
	void SetFloat(string key, float value);

	// Token: 0x06004BF3 RID: 19443 RVA: 0x001D61A5 File Offset: 0x001D43A5
	int GetInt(string key)
	{
		return this.GetInt(key, 0);
	}

	// Token: 0x06004BF4 RID: 19444
	int GetInt(string key, int defaultValue);

	// Token: 0x06004BF5 RID: 19445
	void SetInt(string key, int value);

	// Token: 0x06004BF6 RID: 19446 RVA: 0x001D61AF File Offset: 0x001D43AF
	string GetString(string key)
	{
		return this.GetString(key, "");
	}

	// Token: 0x06004BF7 RID: 19447
	string GetString(string key, string defaultValue);

	// Token: 0x06004BF8 RID: 19448
	void SetString(string key, string value);

	// Token: 0x06004BF9 RID: 19449
	bool HasKey(string key);

	// Token: 0x06004BFA RID: 19450
	void DeleteKey(string key);

	// Token: 0x06004BFB RID: 19451
	void DeleteAll();

	// Token: 0x06004BFC RID: 19452
	void Save();

	// Token: 0x06004BFD RID: 19453
	void Load();

	// Token: 0x17000816 RID: 2070
	// (get) Token: 0x06004BFE RID: 19454
	bool CanLoad { get; }
}
