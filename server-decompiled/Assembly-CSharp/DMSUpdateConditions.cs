using System;

// Token: 0x020003C4 RID: 964
public struct DMSUpdateConditions
{
	// Token: 0x1700037B RID: 891
	// (set) Token: 0x06001D01 RID: 7425 RVA: 0x000AEB4F File Offset: 0x000ACD4F
	public bool DoesPlayerExist
	{
		set
		{
			this.SetBoolHolder(value, 128);
		}
	}

	// Token: 0x1700037C RID: 892
	// (set) Token: 0x06001D02 RID: 7426 RVA: 0x000AEB5D File Offset: 0x000ACD5D
	public bool IsGameUnPaused
	{
		set
		{
			this.SetBoolHolder(value, 64);
		}
	}

	// Token: 0x1700037D RID: 893
	// (set) Token: 0x06001D03 RID: 7427 RVA: 0x000AEB68 File Offset: 0x000ACD68
	public bool IsDMSInitialized
	{
		set
		{
			this.SetBoolHolder(value, 32);
		}
	}

	// Token: 0x1700037E RID: 894
	// (set) Token: 0x06001D04 RID: 7428 RVA: 0x000AEB73 File Offset: 0x000ACD73
	public bool IsDMSEnabled
	{
		set
		{
			this.SetBoolHolder(value, 16);
		}
	}

	// Token: 0x1700037F RID: 895
	// (get) Token: 0x06001D05 RID: 7429 RVA: 0x000AEB7E File Offset: 0x000ACD7E
	public bool CanUpdate
	{
		get
		{
			return this.BoolHolder == 240;
		}
	}

	// Token: 0x06001D06 RID: 7430 RVA: 0x000AEB8D File Offset: 0x000ACD8D
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetBoolHolder(bool _value, byte _place)
	{
		if (_value)
		{
			this.BoolHolder |= _place;
			return;
		}
		this.BoolHolder &= ~_place;
	}

	// Token: 0x040012EB RID: 4843
	[PublicizedFrom(EAccessModifier.Private)]
	public byte BoolHolder;
}
