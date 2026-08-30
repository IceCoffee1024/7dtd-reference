using System;

// Token: 0x02001021 RID: 4129
public class InGameService
{
	// Token: 0x17000F36 RID: 3894
	// (get) Token: 0x06008289 RID: 33417 RVA: 0x0031884B File Offset: 0x00316A4B
	// (set) Token: 0x0600828A RID: 33418 RVA: 0x00318853 File Offset: 0x00316A53
	public InGameService.InGameServiceTypes ServiceType { get; set; }

	// Token: 0x17000F37 RID: 3895
	// (get) Token: 0x0600828B RID: 33419 RVA: 0x0031885C File Offset: 0x00316A5C
	// (set) Token: 0x0600828C RID: 33420 RVA: 0x00318864 File Offset: 0x00316A64
	public string Name { get; set; }

	// Token: 0x17000F38 RID: 3896
	// (get) Token: 0x0600828D RID: 33421 RVA: 0x0031886D File Offset: 0x00316A6D
	// (set) Token: 0x0600828E RID: 33422 RVA: 0x00318875 File Offset: 0x00316A75
	public string Description { get; set; }

	// Token: 0x17000F39 RID: 3897
	// (get) Token: 0x0600828F RID: 33423 RVA: 0x0031887E File Offset: 0x00316A7E
	// (set) Token: 0x06008290 RID: 33424 RVA: 0x00318886 File Offset: 0x00316A86
	public string Icon { get; set; }

	// Token: 0x17000F3A RID: 3898
	// (get) Token: 0x06008291 RID: 33425 RVA: 0x0031888F File Offset: 0x00316A8F
	// (set) Token: 0x06008292 RID: 33426 RVA: 0x00318897 File Offset: 0x00316A97
	public int Price { get; set; }

	// Token: 0x040060B0 RID: 24752
	public Action<bool> VisibleChangedHandler;

	// Token: 0x02001022 RID: 4130
	public enum InGameServiceTypes
	{
		// Token: 0x040060B2 RID: 24754
		VendingRent
	}
}
