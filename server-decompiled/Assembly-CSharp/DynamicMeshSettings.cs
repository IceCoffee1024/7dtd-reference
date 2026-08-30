using System;

// Token: 0x020003A1 RID: 929
public static class DynamicMeshSettings
{
	// Token: 0x17000344 RID: 836
	// (get) Token: 0x06001BA8 RID: 7080 RVA: 0x000A464E File Offset: 0x000A284E
	// (set) Token: 0x06001BA9 RID: 7081 RVA: 0x000A4655 File Offset: 0x000A2855
	public static bool UseImposterValues { get; set; } = true;

	// Token: 0x17000345 RID: 837
	// (get) Token: 0x06001BAA RID: 7082 RVA: 0x000A465D File Offset: 0x000A285D
	// (set) Token: 0x06001BAB RID: 7083 RVA: 0x000A4664 File Offset: 0x000A2864
	public static bool OnlyPlayerAreas { get; set; } = false;

	// Token: 0x17000346 RID: 838
	// (get) Token: 0x06001BAC RID: 7084 RVA: 0x000A466C File Offset: 0x000A286C
	// (set) Token: 0x06001BAD RID: 7085 RVA: 0x000A4673 File Offset: 0x000A2873
	public static int PlayerAreaChunkBuffer { get; set; } = 3;

	// Token: 0x17000347 RID: 839
	// (get) Token: 0x06001BAE RID: 7086 RVA: 0x000A467B File Offset: 0x000A287B
	// (set) Token: 0x06001BAF RID: 7087 RVA: 0x000A4682 File Offset: 0x000A2882
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

	// Token: 0x17000348 RID: 840
	// (get) Token: 0x06001BB0 RID: 7088 RVA: 0x000A469E File Offset: 0x000A289E
	// (set) Token: 0x06001BB1 RID: 7089 RVA: 0x000A46A5 File Offset: 0x000A28A5
	public static bool NewWorldFullRegen { get; set; } = false;

	// Token: 0x06001BB2 RID: 7090 RVA: 0x000A46B0 File Offset: 0x000A28B0
	[PublicizedFrom(EAccessModifier.Private)]
	static DynamicMeshSettings()
	{
		GamePrefs.OnGamePrefChanged += DynamicMeshSettings.OnGamePrefChanged;
	}

	// Token: 0x06001BB3 RID: 7091 RVA: 0x000A4702 File Offset: 0x000A2902
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OnGamePrefChanged(EnumGamePrefs _pref)
	{
		if (_pref == EnumGamePrefs.DynamicMeshDistance)
		{
			DynamicMeshSettings.MaxViewDistance = GamePrefs.GetInt(_pref);
		}
	}

	// Token: 0x06001BB4 RID: 7092 RVA: 0x000A4718 File Offset: 0x000A2918
	public static void LogSettings()
	{
		Log.Out("Dynamic Mesh Settings");
		Log.Out("Use Imposter Values: " + DynamicMeshSettings.UseImposterValues.ToString());
		Log.Out("Only Player Areas: " + DynamicMeshSettings.OnlyPlayerAreas.ToString());
		Log.Out("Player Area Buffer: " + DynamicMeshSettings.PlayerAreaChunkBuffer.ToString());
		Log.Out("Max View Distance: " + DynamicMeshSettings.MaxViewDistance.ToString());
		Log.Out("Regen all on new world: " + DynamicMeshSettings.NewWorldFullRegen.ToString());
	}

	// Token: 0x06001BB5 RID: 7093 RVA: 0x000027FC File Offset: 0x000009FC
	public static void Validate()
	{
	}

	// Token: 0x040011D5 RID: 4565
	public static int MaxRegionMeshData = 1;

	// Token: 0x040011D6 RID: 4566
	public static int MaxRegionLoadMsPerFrame = 2;

	// Token: 0x040011D7 RID: 4567
	public static int MaxDyMeshData = 3;

	// Token: 0x040011DB RID: 4571
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _maxViewDistance = 1000;
}
