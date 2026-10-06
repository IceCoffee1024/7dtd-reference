using System;

// Token: 0x02001030 RID: 4144
public class InGameService
{
	// Token: 0x17000F5D RID: 3933
	// (get) Token: 0x06008298 RID: 33432 RVA: 0x0031B88F File Offset: 0x00319A8F
	// (set) Token: 0x06008299 RID: 33433 RVA: 0x0031B897 File Offset: 0x00319A97
	public InGameService.InGameServiceTypes ServiceType { get; set; }

	// Token: 0x17000F5E RID: 3934
	// (get) Token: 0x0600829A RID: 33434 RVA: 0x0031B8A0 File Offset: 0x00319AA0
	// (set) Token: 0x0600829B RID: 33435 RVA: 0x0031B8A8 File Offset: 0x00319AA8
	public string Name { get; set; }

	// Token: 0x17000F5F RID: 3935
	// (get) Token: 0x0600829C RID: 33436 RVA: 0x0031B8B1 File Offset: 0x00319AB1
	// (set) Token: 0x0600829D RID: 33437 RVA: 0x0031B8B9 File Offset: 0x00319AB9
	public string Description { get; set; }

	// Token: 0x17000F60 RID: 3936
	// (get) Token: 0x0600829E RID: 33438 RVA: 0x0031B8C2 File Offset: 0x00319AC2
	// (set) Token: 0x0600829F RID: 33439 RVA: 0x0031B8CA File Offset: 0x00319ACA
	public string Icon { get; set; }

	// Token: 0x17000F61 RID: 3937
	// (get) Token: 0x060082A0 RID: 33440 RVA: 0x0031B8D3 File Offset: 0x00319AD3
	// (set) Token: 0x060082A1 RID: 33441 RVA: 0x0031B8DB File Offset: 0x00319ADB
	public int Price { get; set; }

	// Token: 0x040060D9 RID: 24793
	public Action<bool> VisibleChangedHandler;

	// Token: 0x02001031 RID: 4145
	public enum InGameServiceTypes
	{
		// Token: 0x040060DB RID: 24795
		VendingRent
	}
}
