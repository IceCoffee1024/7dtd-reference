using System;

// Token: 0x020003A5 RID: 933
public static class DynamicMeshSettings
{
	// Token: 0x17000348 RID: 840
	// (get) Token: 0x06001BC5 RID: 7109 RVA: 0x000A4D8E File Offset: 0x000A2F8E
	// (set) Token: 0x06001BC6 RID: 7110 RVA: 0x000A4D95 File Offset: 0x000A2F95
	public static bool UseImposterValues { get; set; } = true;

	// Token: 0x17000349 RID: 841
	// (get) Token: 0x06001BC7 RID: 7111 RVA: 0x000A4D9D File Offset: 0x000A2F9D
	// (set) Token: 0x06001BC8 RID: 7112 RVA: 0x000A4DA4 File Offset: 0x000A2FA4
	public static bool OnlyPlayerAreas { get; set; } = false;

	// Token: 0x1700034A RID: 842
	// (get) Token: 0x06001BC9 RID: 7113 RVA: 0x000A4DAC File Offset: 0x000A2FAC
	// (set) Token: 0x06001BCA RID: 7114 RVA: 0x000A4DB3 File Offset: 0x000A2FB3
	public static int PlayerAreaChunkBuffer { get; set; } = 3;

	// Token: 0x1700034B RID: 843
	// (get) Token: 0x06001BCB RID: 7115 RVA: 0x000A4DBB File Offset: 0x000A2FBB
	// (set) Token: 0x06001BCC RID: 7116 RVA: 0x000A4DC2 File Offset: 0x000A2FC2
	public static int MaxViewDistance
	{
		get
		{
			return DynamicMeshSettings._maxViewDistance;
		}
		set
		{
			DynamicMeshSettings._maxViewDistance = Math.Min(3000, value);
			PrefabLODManager.lodPoiDistance = DynamicMeshSettings._maxViewDistance;
		}
	}

	// Token: 0x1700034C RID: 844
	// (get) Token: 0x06001BCD RID: 7117 RVA: 0x000A4DDE File Offset: 0x000A2FDE
	// (set) Token: 0x06001BCE RID: 7118 RVA: 0x000A4DE5 File Offset: 0x000A2FE5
	public static bool NewWorldFullRegen { get; set; } = false;

	// Token: 0x06001BCF RID: 7119 RVA: 0x000A4DF0 File Offset: 0x000A2FF0
	[PublicizedFrom(EAccessModifier.Private)]
	static DynamicMeshSettings()
	{
		GamePrefs.OnGamePrefChanged += DynamicMeshSettings.OnGamePrefChanged;
	}

	// Token: 0x06001BD0 RID: 7120 RVA: 0x000A4E42 File Offset: 0x000A3042
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OnGamePrefChanged(EnumGamePrefs _pref)
	{
		if (_pref == EnumGamePrefs.DynamicMeshDistance)
		{
			DynamicMeshSettings.MaxViewDistance = GamePrefs.GetInt(_pref);
		}
	}

	// Token: 0x06001BD1 RID: 7121 RVA: 0x000A4E58 File Offset: 0x000A3058
	public static void LogSettings()
	{
		Log.Out("Dynamic Mesh Settings");
		Log.Out("Use Imposter Values: " + DynamicMeshSettings.UseImposterValues.ToString());
		Log.Out("Only Player Areas: " + DynamicMeshSettings.OnlyPlayerAreas.ToString());
		Log.Out("Player Area Buffer: " + DynamicMeshSettings.PlayerAreaChunkBuffer.ToString());
		Log.Out("Max View Distance: " + DynamicMeshSettings.MaxViewDistance.ToString());
		Log.Out("Regen all on new world: " + DynamicMeshSettings.NewWorldFullRegen.ToString());
	}

	// Token: 0x06001BD2 RID: 7122 RVA: 0x000027FC File Offset: 0x000009FC
	public static void Validate()
	{
	}

	// Token: 0x040011EA RID: 4586
	public static int MaxRegionMeshData = 1;

	// Token: 0x040011EB RID: 4587
	public static int MaxRegionLoadMsPerFrame = 2;

	// Token: 0x040011EC RID: 4588
	public static int MaxDyMeshData = 3;

	// Token: 0x040011F0 RID: 4592
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _maxViewDistance = 1000;
}
