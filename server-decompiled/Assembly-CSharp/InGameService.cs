using System;

// Token: 0x02001040 RID: 4160
public class InGameService
{
	// Token: 0x17000F68 RID: 3944
	// (get) Token: 0x060082EA RID: 33514 RVA: 0x0031D593 File Offset: 0x0031B793
	// (set) Token: 0x060082EB RID: 33515 RVA: 0x0031D59B File Offset: 0x0031B79B
	public InGameService.InGameServiceTypes ServiceType { get; set; }

	// Token: 0x17000F69 RID: 3945
	// (get) Token: 0x060082EC RID: 33516 RVA: 0x0031D5A4 File Offset: 0x0031B7A4
	// (set) Token: 0x060082ED RID: 33517 RVA: 0x0031D5AC File Offset: 0x0031B7AC
	public string Name { get; set; }

	// Token: 0x17000F6A RID: 3946
	// (get) Token: 0x060082EE RID: 33518 RVA: 0x0031D5B5 File Offset: 0x0031B7B5
	// (set) Token: 0x060082EF RID: 33519 RVA: 0x0031D5BD File Offset: 0x0031B7BD
	public string Description { get; set; }

	// Token: 0x17000F6B RID: 3947
	// (get) Token: 0x060082F0 RID: 33520 RVA: 0x0031D5C6 File Offset: 0x0031B7C6
	// (set) Token: 0x060082F1 RID: 33521 RVA: 0x0031D5CE File Offset: 0x0031B7CE
	public string Icon { get; set; }

	// Token: 0x17000F6C RID: 3948
	// (get) Token: 0x060082F2 RID: 33522 RVA: 0x0031D5D7 File Offset: 0x0031B7D7
	// (set) Token: 0x060082F3 RID: 33523 RVA: 0x0031D5DF File Offset: 0x0031B7DF
	public int Price { get; set; }

	// Token: 0x04006117 RID: 24855
	public Action<bool> VisibleChangedHandler;

	// Token: 0x02001041 RID: 4161
	public enum InGameServiceTypes
	{
		// Token: 0x04006119 RID: 24857
		VendingRent
	}
}
