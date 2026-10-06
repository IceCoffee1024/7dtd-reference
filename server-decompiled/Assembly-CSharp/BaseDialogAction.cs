using System;

// Token: 0x020002E3 RID: 739
public abstract class BaseDialogAction
{
	// Token: 0x1700024D RID: 589
	// (get) Token: 0x06001541 RID: 5441 RVA: 0x000803B3 File Offset: 0x0007E5B3
	// (set) Token: 0x06001542 RID: 5442 RVA: 0x000803BB File Offset: 0x0007E5BB
	public string ID { get; set; }

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x06001543 RID: 5443 RVA: 0x000803C4 File Offset: 0x0007E5C4
	// (set) Token: 0x06001544 RID: 5444 RVA: 0x000803CC File Offset: 0x0007E5CC
	public string Value { get; set; }

	// Token: 0x1700024F RID: 591
	// (get) Token: 0x06001545 RID: 5445 RVA: 0x000803D5 File Offset: 0x0007E5D5
	// (set) Token: 0x06001546 RID: 5446 RVA: 0x000803DD File Offset: 0x0007E5DD
	public Dialog OwnerDialog { get; set; }

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x06001547 RID: 5447 RVA: 0x000803E6 File Offset: 0x0007E5E6
	// (set) Token: 0x06001548 RID: 5448 RVA: 0x000803EE File Offset: 0x0007E5EE
	public DialogResponse Owner { get; set; }

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x06001549 RID: 5449 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual BaseDialogAction.ActionTypes ActionType
	{
		get
		{
			return BaseDialogAction.ActionTypes.AddBuff;
		}
	}

	// Token: 0x0600154A RID: 5450 RVA: 0x000803F7 File Offset: 0x0007E5F7
	public BaseDialogAction()
	{
		this.ID = "";
		this.Value = "";
	}

	// Token: 0x0600154B RID: 5451 RVA: 0x00080415 File Offset: 0x0007E615
	[PublicizedFrom(EAccessModifier.Protected)]
	public void CopyValues(BaseDialogAction action)
	{
		action.ID = this.ID;
		action.Value = this.Value;
	}

	// Token: 0x0600154C RID: 5452 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupAction()
	{
	}

	// Token: 0x0600154D RID: 5453 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void PerformAction(EntityPlayer player)
	{
	}

	// Token: 0x0600154E RID: 5454 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual BaseDialogAction Clone()
	{
		return null;
	}

	// Token: 0x020002E4 RID: 740
	public enum ActionTypes
	{
		// Token: 0x04000E48 RID: 3656
		AddBuff,
		// Token: 0x04000E49 RID: 3657
		AddItem,
		// Token: 0x04000E4A RID: 3658
		AddQuest,
		// Token: 0x04000E4B RID: 3659
		CompleteQuest,
		// Token: 0x04000E4C RID: 3660
		Trader,
		// Token: 0x04000E4D RID: 3661
		Voice
	}
}
