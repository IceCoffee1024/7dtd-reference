using System;

// Token: 0x020002DC RID: 732
public abstract class BaseDialogAction
{
	// Token: 0x17000254 RID: 596
	// (get) Token: 0x06001531 RID: 5425 RVA: 0x0007FA43 File Offset: 0x0007DC43
	// (set) Token: 0x06001532 RID: 5426 RVA: 0x0007FA4B File Offset: 0x0007DC4B
	public string ID { get; set; }

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x06001533 RID: 5427 RVA: 0x0007FA54 File Offset: 0x0007DC54
	// (set) Token: 0x06001534 RID: 5428 RVA: 0x0007FA5C File Offset: 0x0007DC5C
	public string Value { get; set; }

	// Token: 0x17000256 RID: 598
	// (get) Token: 0x06001535 RID: 5429 RVA: 0x0007FA65 File Offset: 0x0007DC65
	// (set) Token: 0x06001536 RID: 5430 RVA: 0x0007FA6D File Offset: 0x0007DC6D
	public Dialog OwnerDialog { get; set; }

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06001537 RID: 5431 RVA: 0x0007FA76 File Offset: 0x0007DC76
	// (set) Token: 0x06001538 RID: 5432 RVA: 0x0007FA7E File Offset: 0x0007DC7E
	public DialogResponse Owner { get; set; }

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x06001539 RID: 5433 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual BaseDialogAction.ActionTypes ActionType
	{
		get
		{
			return BaseDialogAction.ActionTypes.AddBuff;
		}
	}

	// Token: 0x0600153A RID: 5434 RVA: 0x0007FA87 File Offset: 0x0007DC87
	public BaseDialogAction()
	{
		this.ID = "";
		this.Value = "";
	}

	// Token: 0x0600153B RID: 5435 RVA: 0x0007FAA5 File Offset: 0x0007DCA5
	[PublicizedFrom(EAccessModifier.Protected)]
	public void CopyValues(BaseDialogAction action)
	{
		action.ID = this.ID;
		action.Value = this.Value;
	}

	// Token: 0x0600153C RID: 5436 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupAction()
	{
	}

	// Token: 0x0600153D RID: 5437 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void PerformAction(EntityPlayer player)
	{
	}

	// Token: 0x0600153E RID: 5438 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual BaseDialogAction Clone()
	{
		return null;
	}

	// Token: 0x020002DD RID: 733
	public enum ActionTypes
	{
		// Token: 0x04000E3E RID: 3646
		AddBuff,
		// Token: 0x04000E3F RID: 3647
		AddItem,
		// Token: 0x04000E40 RID: 3648
		AddQuest,
		// Token: 0x04000E41 RID: 3649
		CompleteQuest,
		// Token: 0x04000E42 RID: 3650
		Trader,
		// Token: 0x04000E43 RID: 3651
		Voice
	}
}
