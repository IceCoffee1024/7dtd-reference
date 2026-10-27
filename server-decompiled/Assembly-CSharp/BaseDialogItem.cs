using System;

// Token: 0x020002EF RID: 751
public class BaseDialogItem
{
	// Token: 0x17000259 RID: 601
	// (get) Token: 0x06001572 RID: 5490 RVA: 0x00080F85 File Offset: 0x0007F185
	// (set) Token: 0x06001573 RID: 5491 RVA: 0x00080F8D File Offset: 0x0007F18D
	public virtual string HeaderName
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x06001574 RID: 5492 RVA: 0x00080F96 File Offset: 0x0007F196
	// (set) Token: 0x06001575 RID: 5493 RVA: 0x00080F9E File Offset: 0x0007F19E
	public Dialog OwnerDialog { get; set; }

	// Token: 0x04000E74 RID: 3700
	public string ID;
}
