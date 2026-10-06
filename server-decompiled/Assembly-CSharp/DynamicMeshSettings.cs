using System;

// Token: 0x020003A8 RID: 936
public static class DynamicMeshSettings
{
	// Token: 0x17000340 RID: 832
	// (get) Token: 0x06001BBE RID: 7102 RVA: 0x000A4FCE File Offset: 0x000A31CE
	// (set) Token: 0x06001BBF RID: 7103 RVA: 0x000A4FD5 File Offset: 0x000A31D5
	public static bool UseImposterValues { get; set; } = true;

	// Token: 0x17000341 RID: 833
	// (get) Token: 0x06001BC0 RID: 7104 RVA: 0x000A4FDD File Offset: 0x000A31DD
	// (set) Token: 0x06001BC1 RID: 7105 RVA: 0x000A4FE4 File Offset: 0x000A31E4
	public static bool OnlyPlayerAreas { get; set; } = false;

	// Token: 0x17000342 RID: 834
	// (get) Token: 0x06001BC2 RID: 7106 RVA: 0x000A4FEC File Offset: 0x000A31EC
	// (set) Token: 0x06001BC3 RID: 7107 RVA: 0x000A4FF3 File Offset: 0x000A31F3
	public static int PlayerAreaChunkBuffer { get; set; } = 3;

	// Token: 0x17000343 RID: 835
	// (get) Token: 0x06001BC4 RID: 7108 RVA: 0x000A4FFB File Offset: 0x000A31FB
	// (set) Token: 0x06001BC5 RID: 7109 RVA: 0x000A5002 File Offset: 0x000A3202
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

	// Token: 0x17000344 RID: 836
	// (get) Token: 0x06001BC6 RID: 7110 RVA: 0x000A501E File Offset: 0x000A321E
	// (set) Token: 0x06001BC7 RID: 7111 RVA: 0x000A5025 File Offset: 0x000A3225
	public static bool NewWorldFullRegen { get; set; } = false;

	// Token: 0x06001BC8 RID: 7112 RVA: 0x000A5030 File Offset: 0x000A3230
	[PublicizedFrom(EAccessModifier.Private)]
	static DynamicMeshSettings()
	{
		GamePrefs.OnGamePrefChanged += DynamicMeshSettings.OnGamePrefChanged;
	}

	// Token: 0x06001BC9 RID: 7113 RVA: 0x000A5082 File Offset: 0x000A3282
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OnGamePrefChanged(EnumGamePrefs _pref)
	{
		if (_pref == EnumGamePrefs.DynamicMeshDistance)
		{
			DynamicMeshSettings.MaxViewDistance = GamePrefs.GetInt(_pref);
		}
	}

	// Token: 0x06001BCA RID: 7114 RVA: 0x000A5098 File Offset: 0x000A3298
	public static void LogSettings()
	{
		Log.Out("Dynamic Mesh Settings");
		Log.Out("Use Imposter Values: " + DynamicMeshSettings.UseImposterValues.ToString());
		Log.Out("Only Player Areas: " + DynamicMeshSettings.OnlyPlayerAreas.ToString());
		Log.Out("Player Area Buffer: " + DynamicMeshSettings.PlayerAreaChunkBuffer.ToString());
		Log.Out("Max View Distance: " + DynamicMeshSettings.MaxViewDistance.ToString());
		Log.Out("Regen all on new world: " + DynamicMeshSettings.NewWorldFullRegen.ToString());
	}

	// Token: 0x06001BCB RID: 7115 RVA: 0x000027FC File Offset: 0x000009FC
	public static void Validate()
	{
	}

	// Token: 0x040011E3 RID: 4579
	public static int MaxRegionMeshData = 1;

	// Token: 0x040011E4 RID: 4580
	public static int MaxRegionLoadMsPerFrame = 2;

	// Token: 0x040011E5 RID: 4581
	public static int MaxDyMeshData = 3;

	// Token: 0x040011E9 RID: 4585
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _maxViewDistance = 1000;
}
