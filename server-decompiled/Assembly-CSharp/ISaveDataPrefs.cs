using System;

// Token: 0x02000A09 RID: 2569
public interface ISaveDataPrefs
{
	// Token: 0x06004C45 RID: 19525 RVA: 0x001D9C2F File Offset: 0x001D7E2F
	float GetFloat(string key)
	{
		return this.GetFloat(key, 0f);
	}

	// Token: 0x06004C46 RID: 19526
	float GetFloat(string key, float defaultValue);

	// Token: 0x06004C47 RID: 19527
	void SetFloat(string key, float value);

	// Token: 0x06004C48 RID: 19528 RVA: 0x001D9C3D File Offset: 0x001D7E3D
	int GetInt(string key)
	{
		return this.GetInt(key, 0);
	}

	// Token: 0x06004C49 RID: 19529
	int GetInt(string key, int defaultValue);

	// Token: 0x06004C4A RID: 19530
	void SetInt(string key, int value);

	// Token: 0x06004C4B RID: 19531 RVA: 0x001D9C47 File Offset: 0x001D7E47
	string GetString(string key)
	{
		return this.GetString(key, "");
	}

	// Token: 0x06004C4C RID: 19532
	string GetString(string key, string defaultValue);

	// Token: 0x06004C4D RID: 19533
	void SetString(string key, string value);

	// Token: 0x06004C4E RID: 19534
	bool HasKey(string key);

	// Token: 0x06004C4F RID: 19535
	void DeleteKey(string key);

	// Token: 0x06004C50 RID: 19536
	void DeleteAll();

	// Token: 0x06004C51 RID: 19537
	void Save();

	// Token: 0x06004C52 RID: 19538
	void Load();

	// Token: 0x17000848 RID: 2120
	// (get) Token: 0x06004C53 RID: 19539
	bool CanLoad { get; }
}
