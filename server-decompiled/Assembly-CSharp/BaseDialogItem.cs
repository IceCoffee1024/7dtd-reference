using System;

// Token: 0x020002EA RID: 746
public class BaseDialogItem
{
	// Token: 0x17000261 RID: 609
	// (get) Token: 0x06001571 RID: 5489 RVA: 0x00080AAD File Offset: 0x0007ECAD
	// (set) Token: 0x06001572 RID: 5490 RVA: 0x00080AB5 File Offset: 0x0007ECB5
	public virtual string HeaderName
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000262 RID: 610
	// (get) Token: 0x06001573 RID: 5491 RVA: 0x00080ABE File Offset: 0x0007ECBE
	// (set) Token: 0x06001574 RID: 5492 RVA: 0x00080AC6 File Offset: 0x0007ECC6
	public Dialog OwnerDialog { get; set; }

	// Token: 0x04000E6B RID: 3691
	public string ID;
}
