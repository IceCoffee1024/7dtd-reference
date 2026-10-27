using System;

// Token: 0x020003AA RID: 938
public static class DynamicMeshSettings
{
	// Token: 0x17000340 RID: 832
	// (get) Token: 0x06001BC2 RID: 7106 RVA: 0x000A51DE File Offset: 0x000A33DE
	// (set) Token: 0x06001BC3 RID: 7107 RVA: 0x000A51E5 File Offset: 0x000A33E5
	public static bool UseImposterValues { get; set; } = true;

	// Token: 0x17000341 RID: 833
	// (get) Token: 0x06001BC4 RID: 7108 RVA: 0x000A51ED File Offset: 0x000A33ED
	// (set) Token: 0x06001BC5 RID: 7109 RVA: 0x000A51F4 File Offset: 0x000A33F4
	public static bool OnlyPlayerAreas { get; set; } = false;

	// Token: 0x17000342 RID: 834
	// (get) Token: 0x06001BC6 RID: 7110 RVA: 0x000A51FC File Offset: 0x000A33FC
	// (set) Token: 0x06001BC7 RID: 7111 RVA: 0x000A5203 File Offset: 0x000A3403
	public static int PlayerAreaChunkBuffer { get; set; } = 3;

	// Token: 0x17000343 RID: 835
	// (get) Token: 0x06001BC8 RID: 7112 RVA: 0x000A520B File Offset: 0x000A340B
	// (set) Token: 0x06001BC9 RID: 7113 RVA: 0x000A5212 File Offset: 0x000A3412
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
	// (get) Token: 0x06001BCA RID: 7114 RVA: 0x000A522E File Offset: 0x000A342E
	// (set) Token: 0x06001BCB RID: 7115 RVA: 0x000A5235 File Offset: 0x000A3435
	public static bool NewWorldFullRegen { get; set; } = false;

	// Token: 0x06001BCC RID: 7116 RVA: 0x000A5240 File Offset: 0x000A3440
	[PublicizedFrom(EAccessModifier.Private)]
	static DynamicMeshSettings()
	{
		GamePrefs.OnGamePrefChanged += DynamicMeshSettings.OnGamePrefChanged;
	}

	// Token: 0x06001BCD RID: 7117 RVA: 0x000A5292 File Offset: 0x000A3492
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OnGamePrefChanged(EnumGamePrefs _pref)
	{
		if (_pref == EnumGamePrefs.DynamicMeshDistance)
		{
			DynamicMeshSettings.MaxViewDistance = GamePrefs.GetInt(_pref);
		}
	}

	// Token: 0x06001BCE RID: 7118 RVA: 0x000A52A8 File Offset: 0x000A34A8
	public static void LogSettings()
	{
		Log.Out("Dynamic Mesh Settings");
		Log.Out("Use Imposter Values: " + DynamicMeshSettings.UseImposterValues.ToString());
		Log.Out("Only Player Areas: " + DynamicMeshSettings.OnlyPlayerAreas.ToString());
		Log.Out("Player Area Buffer: " + DynamicMeshSettings.PlayerAreaChunkBuffer.ToString());
		Log.Out("Max View Distance: " + DynamicMeshSettings.MaxViewDistance.ToString());
		Log.Out("Regen all on new world: " + DynamicMeshSettings.NewWorldFullRegen.ToString());
	}

	// Token: 0x06001BCF RID: 7119 RVA: 0x000027FC File Offset: 0x000009FC
	public static void Validate()
	{
	}

	// Token: 0x040011F3 RID: 4595
	public static int MaxRegionMeshData = 1;

	// Token: 0x040011F4 RID: 4596
	public static int MaxRegionLoadMsPerFrame = 2;

	// Token: 0x040011F5 RID: 4597
	public static int MaxDyMeshData = 3;

	// Token: 0x040011F9 RID: 4601
	[PublicizedFrom(EAccessModifier.Private)]
	public static int _maxViewDistance = 1000;
}
