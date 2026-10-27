using System;

// Token: 0x020003C9 RID: 969
public struct DMSUpdateConditions
{
	// Token: 0x17000373 RID: 883
	// (set) Token: 0x06001CFD RID: 7421 RVA: 0x000AEF6B File Offset: 0x000AD16B
	public bool DoesPlayerExist
	{
		set
		{
			this.SetBoolHolder(value, 128);
		}
	}

	// Token: 0x17000374 RID: 884
	// (set) Token: 0x06001CFE RID: 7422 RVA: 0x000AEF79 File Offset: 0x000AD179
	public bool IsGameUnPaused
	{
		set
		{
			this.SetBoolHolder(value, 64);
		}
	}

	// Token: 0x17000375 RID: 885
	// (set) Token: 0x06001CFF RID: 7423 RVA: 0x000AEF84 File Offset: 0x000AD184
	public bool IsDMSInitialized
	{
		set
		{
			this.SetBoolHolder(value, 32);
		}
	}

	// Token: 0x17000376 RID: 886
	// (set) Token: 0x06001D00 RID: 7424 RVA: 0x000AEF8F File Offset: 0x000AD18F
	public bool IsDMSEnabled
	{
		set
		{
			this.SetBoolHolder(value, 16);
		}
	}

	// Token: 0x17000377 RID: 887
	// (get) Token: 0x06001D01 RID: 7425 RVA: 0x000AEF9A File Offset: 0x000AD19A
	public bool CanUpdate
	{
		get
		{
			return this.BoolHolder == 240;
		}
	}

	// Token: 0x06001D02 RID: 7426 RVA: 0x000AEFA9 File Offset: 0x000AD1A9
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

	// Token: 0x040012F4 RID: 4852
	[PublicizedFrom(EAccessModifier.Private)]
	public byte BoolHolder;
}
