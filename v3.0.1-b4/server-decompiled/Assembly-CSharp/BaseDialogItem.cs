using System;

// Token: 0x020002E6 RID: 742
public class BaseDialogItem
{
	// Token: 0x17000260 RID: 608
	// (get) Token: 0x0600155E RID: 5470 RVA: 0x000803F9 File Offset: 0x0007E5F9
	// (set) Token: 0x0600155F RID: 5471 RVA: 0x00080401 File Offset: 0x0007E601
	public virtual string HeaderName
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000261 RID: 609
	// (get) Token: 0x06001560 RID: 5472 RVA: 0x0008040A File Offset: 0x0007E60A
	// (set) Token: 0x06001561 RID: 5473 RVA: 0x00080412 File Offset: 0x0007E612
	public Dialog OwnerDialog { get; set; }

	// Token: 0x04000E5A RID: 3674
	public string ID;
}
