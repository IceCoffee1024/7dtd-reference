using System;

// Token: 0x0200058C RID: 1420
public class ToolbeltConfig
{
	// Token: 0x1700051E RID: 1310
	// (get) Token: 0x06002ED4 RID: 11988 RVA: 0x001284DF File Offset: 0x001266DF
	// (set) Token: 0x06002ED5 RID: 11989 RVA: 0x001284E6 File Offset: 0x001266E6
	public static ToolbeltConfig GameConfig { get; set; } = new ToolbeltConfig();

	// Token: 0x1700051F RID: 1311
	// (get) Token: 0x06002ED6 RID: 11990 RVA: 0x001284EE File Offset: 0x001266EE
	// (set) Token: 0x06002ED7 RID: 11991 RVA: 0x001284F5 File Offset: 0x001266F5
	public static ToolbeltConfig EditorConfig { get; set; } = new ToolbeltConfig();

	// Token: 0x17000520 RID: 1312
	// (get) Token: 0x06002ED8 RID: 11992 RVA: 0x001284FD File Offset: 0x001266FD
	public static ToolbeltConfig CurrentConfig
	{
		get
		{
			if (PrefabEditModeManager.Instance == null || !PrefabEditModeManager.Instance.IsActive())
			{
				return ToolbeltConfig.GameConfig;
			}
			return ToolbeltConfig.EditorConfig;
		}
	}

	// Token: 0x17000521 RID: 1313
	// (get) Token: 0x06002ED9 RID: 11993 RVA: 0x0012851D File Offset: 0x0012671D
	// (set) Token: 0x06002EDA RID: 11994 RVA: 0x00128525 File Offset: 0x00126725
	public int SlotsPerBar { get; set; } = 10;

	// Token: 0x17000522 RID: 1314
	// (get) Token: 0x06002EDB RID: 11995 RVA: 0x0012852E File Offset: 0x0012672E
	// (set) Token: 0x06002EDC RID: 11996 RVA: 0x00128536 File Offset: 0x00126736
	public int BarsPerPage { get; set; } = 1;

	// Token: 0x17000523 RID: 1315
	// (get) Token: 0x06002EDD RID: 11997 RVA: 0x0012853F File Offset: 0x0012673F
	// (set) Token: 0x06002EDE RID: 11998 RVA: 0x00128547 File Offset: 0x00126747
	public int PageCount { get; set; } = 1;

	// Token: 0x17000524 RID: 1316
	// (get) Token: 0x06002EDF RID: 11999 RVA: 0x00128550 File Offset: 0x00126750
	public int SlotsPerPage
	{
		get
		{
			return this.BarsPerPage * this.SlotsPerBar;
		}
	}

	// Token: 0x17000525 RID: 1317
	// (get) Token: 0x06002EE0 RID: 12000 RVA: 0x0012855F File Offset: 0x0012675F
	public int TotalSlots
	{
		get
		{
			return this.SlotsPerBar * this.BarsPerPage * this.PageCount;
		}
	}
}
