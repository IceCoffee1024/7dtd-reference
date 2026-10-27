using System;

// Token: 0x02000593 RID: 1427
public class ToolbeltConfig
{
	// Token: 0x1700051E RID: 1310
	// (get) Token: 0x06002EE2 RID: 12002 RVA: 0x001290D3 File Offset: 0x001272D3
	// (set) Token: 0x06002EE3 RID: 12003 RVA: 0x001290DA File Offset: 0x001272DA
	public static ToolbeltConfig GameConfig { get; set; } = new ToolbeltConfig();

	// Token: 0x1700051F RID: 1311
	// (get) Token: 0x06002EE4 RID: 12004 RVA: 0x001290E2 File Offset: 0x001272E2
	// (set) Token: 0x06002EE5 RID: 12005 RVA: 0x001290E9 File Offset: 0x001272E9
	public static ToolbeltConfig EditorConfig { get; set; } = new ToolbeltConfig();

	// Token: 0x17000520 RID: 1312
	// (get) Token: 0x06002EE6 RID: 12006 RVA: 0x001290F1 File Offset: 0x001272F1
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
	// (get) Token: 0x06002EE7 RID: 12007 RVA: 0x00129111 File Offset: 0x00127311
	// (set) Token: 0x06002EE8 RID: 12008 RVA: 0x00129119 File Offset: 0x00127319
	public int SlotsPerBar { get; set; } = 10;

	// Token: 0x17000522 RID: 1314
	// (get) Token: 0x06002EE9 RID: 12009 RVA: 0x00129122 File Offset: 0x00127322
	// (set) Token: 0x06002EEA RID: 12010 RVA: 0x0012912A File Offset: 0x0012732A
	public int BarsPerPage { get; set; } = 1;

	// Token: 0x17000523 RID: 1315
	// (get) Token: 0x06002EEB RID: 12011 RVA: 0x00129133 File Offset: 0x00127333
	// (set) Token: 0x06002EEC RID: 12012 RVA: 0x0012913B File Offset: 0x0012733B
	public int PageCount { get; set; } = 1;

	// Token: 0x17000524 RID: 1316
	// (get) Token: 0x06002EED RID: 12013 RVA: 0x00129144 File Offset: 0x00127344
	public int SlotsPerPage
	{
		get
		{
			return this.BarsPerPage * this.SlotsPerBar;
		}
	}

	// Token: 0x17000525 RID: 1317
	// (get) Token: 0x06002EEE RID: 12014 RVA: 0x00129153 File Offset: 0x00127353
	public int TotalSlots
	{
		get
		{
			return this.SlotsPerBar * this.BarsPerPage * this.PageCount;
		}
	}
}
