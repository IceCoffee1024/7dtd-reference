using System;

// Token: 0x020002ED RID: 749
public class BaseDialogItem
{
	// Token: 0x17000259 RID: 601
	// (get) Token: 0x0600156E RID: 5486 RVA: 0x00080D61 File Offset: 0x0007EF61
	// (set) Token: 0x0600156F RID: 5487 RVA: 0x00080D69 File Offset: 0x0007EF69
	public virtual string HeaderName
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x06001570 RID: 5488 RVA: 0x00080D72 File Offset: 0x0007EF72
	// (set) Token: 0x06001571 RID: 5489 RVA: 0x00080D7A File Offset: 0x0007EF7A
	public Dialog OwnerDialog { get; set; }

	// Token: 0x04000E64 RID: 3684
	public string ID;
}
