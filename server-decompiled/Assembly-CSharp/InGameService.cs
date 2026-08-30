using System;

// Token: 0x02001008 RID: 4104
public class InGameService
{
	// Token: 0x17000EFD RID: 3837
	// (get) Token: 0x06008197 RID: 33175 RVA: 0x00314023 File Offset: 0x00312223
	// (set) Token: 0x06008198 RID: 33176 RVA: 0x0031402B File Offset: 0x0031222B
	public InGameService.InGameServiceTypes ServiceType { get; set; }

	// Token: 0x17000EFE RID: 3838
	// (get) Token: 0x06008199 RID: 33177 RVA: 0x00314034 File Offset: 0x00312234
	// (set) Token: 0x0600819A RID: 33178 RVA: 0x0031403C File Offset: 0x0031223C
	public string Name { get; set; }

	// Token: 0x17000EFF RID: 3839
	// (get) Token: 0x0600819B RID: 33179 RVA: 0x00314045 File Offset: 0x00312245
	// (set) Token: 0x0600819C RID: 33180 RVA: 0x0031404D File Offset: 0x0031224D
	public string Description { get; set; }

	// Token: 0x17000F00 RID: 3840
	// (get) Token: 0x0600819D RID: 33181 RVA: 0x00314056 File Offset: 0x00312256
	// (set) Token: 0x0600819E RID: 33182 RVA: 0x0031405E File Offset: 0x0031225E
	public string Icon { get; set; }

	// Token: 0x17000F01 RID: 3841
	// (get) Token: 0x0600819F RID: 33183 RVA: 0x00314067 File Offset: 0x00312267
	// (set) Token: 0x060081A0 RID: 33184 RVA: 0x0031406F File Offset: 0x0031226F
	public int Price { get; set; }

	// Token: 0x04005FE9 RID: 24553
	public Action<bool> VisibleChangedHandler;

	// Token: 0x02001009 RID: 4105
	public enum InGameServiceTypes
	{
		// Token: 0x04005FEB RID: 24555
		VendingRent
	}
}
