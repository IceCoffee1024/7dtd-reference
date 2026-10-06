using System;

// Token: 0x020003C7 RID: 967
public struct DMSUpdateConditions
{
	// Token: 0x17000373 RID: 883
	// (set) Token: 0x06001CF9 RID: 7417 RVA: 0x000AED5B File Offset: 0x000ACF5B
	public bool DoesPlayerExist
	{
		set
		{
			this.SetBoolHolder(value, 128);
		}
	}

	// Token: 0x17000374 RID: 884
	// (set) Token: 0x06001CFA RID: 7418 RVA: 0x000AED69 File Offset: 0x000ACF69
	public bool IsGameUnPaused
	{
		set
		{
			this.SetBoolHolder(value, 64);
		}
	}

	// Token: 0x17000375 RID: 885
	// (set) Token: 0x06001CFB RID: 7419 RVA: 0x000AED74 File Offset: 0x000ACF74
	public bool IsDMSInitialized
	{
		set
		{
			this.SetBoolHolder(value, 32);
		}
	}

	// Token: 0x17000376 RID: 886
	// (set) Token: 0x06001CFC RID: 7420 RVA: 0x000AED7F File Offset: 0x000ACF7F
	public bool IsDMSEnabled
	{
		set
		{
			this.SetBoolHolder(value, 16);
		}
	}

	// Token: 0x17000377 RID: 887
	// (get) Token: 0x06001CFD RID: 7421 RVA: 0x000AED8A File Offset: 0x000ACF8A
	public bool CanUpdate
	{
		get
		{
			return this.BoolHolder == 240;
		}
	}

	// Token: 0x06001CFE RID: 7422 RVA: 0x000AED99 File Offset: 0x000ACF99
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

	// Token: 0x040012E4 RID: 4836
	[PublicizedFrom(EAccessModifier.Private)]
	public byte BoolHolder;
}
