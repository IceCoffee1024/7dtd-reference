using System;

// Token: 0x020002E5 RID: 741
public abstract class BaseDialogAction
{
	// Token: 0x1700024D RID: 589
	// (get) Token: 0x06001545 RID: 5445 RVA: 0x00080583 File Offset: 0x0007E783
	// (set) Token: 0x06001546 RID: 5446 RVA: 0x0008058B File Offset: 0x0007E78B
	public string ID { get; set; }

	// Token: 0x1700024E RID: 590
	// (get) Token: 0x06001547 RID: 5447 RVA: 0x00080594 File Offset: 0x0007E794
	// (set) Token: 0x06001548 RID: 5448 RVA: 0x0008059C File Offset: 0x0007E79C
	public string Value { get; set; }

	// Token: 0x1700024F RID: 591
	// (get) Token: 0x06001549 RID: 5449 RVA: 0x000805A5 File Offset: 0x0007E7A5
	// (set) Token: 0x0600154A RID: 5450 RVA: 0x000805AD File Offset: 0x0007E7AD
	public Dialog OwnerDialog { get; set; }

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x0600154B RID: 5451 RVA: 0x000805B6 File Offset: 0x0007E7B6
	// (set) Token: 0x0600154C RID: 5452 RVA: 0x000805BE File Offset: 0x0007E7BE
	public DialogResponse Owner { get; set; }

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x0600154D RID: 5453 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual BaseDialogAction.ActionTypes ActionType
	{
		get
		{
			return BaseDialogAction.ActionTypes.AddBuff;
		}
	}

	// Token: 0x0600154E RID: 5454 RVA: 0x000805C7 File Offset: 0x0007E7C7
	public BaseDialogAction()
	{
		this.ID = "";
		this.Value = "";
	}

	// Token: 0x0600154F RID: 5455 RVA: 0x000805E5 File Offset: 0x0007E7E5
	[PublicizedFrom(EAccessModifier.Protected)]
	public void CopyValues(BaseDialogAction action)
	{
		action.ID = this.ID;
		action.Value = this.Value;
	}

	// Token: 0x06001550 RID: 5456 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupAction()
	{
	}

	// Token: 0x06001551 RID: 5457 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void PerformAction(EntityPlayer player)
	{
	}

	// Token: 0x06001552 RID: 5458 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual BaseDialogAction Clone()
	{
		return null;
	}

	// Token: 0x020002E6 RID: 742
	public enum ActionTypes
	{
		// Token: 0x04000E49 RID: 3657
		AddBuff,
		// Token: 0x04000E4A RID: 3658
		AddItem,
		// Token: 0x04000E4B RID: 3659
		AddQuest,
		// Token: 0x04000E4C RID: 3660
		CompleteQuest,
		// Token: 0x04000E4D RID: 3661
		Trader,
		// Token: 0x04000E4E RID: 3662
		Voice
	}
}
