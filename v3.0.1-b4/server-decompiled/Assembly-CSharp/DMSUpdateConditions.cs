using System;

// Token: 0x020003C0 RID: 960
public struct DMSUpdateConditions
{
	// Token: 0x17000377 RID: 887
	// (set) Token: 0x06001CE4 RID: 7396 RVA: 0x000AE40F File Offset: 0x000AC60F
	public bool DoesPlayerExist
	{
		set
		{
			this.SetBoolHolder(value, 128);
		}
	}

	// Token: 0x17000378 RID: 888
	// (set) Token: 0x06001CE5 RID: 7397 RVA: 0x000AE41D File Offset: 0x000AC61D
	public bool IsGameUnPaused
	{
		set
		{
			this.SetBoolHolder(value, 64);
		}
	}

	// Token: 0x17000379 RID: 889
	// (set) Token: 0x06001CE6 RID: 7398 RVA: 0x000AE428 File Offset: 0x000AC628
	public bool IsDMSInitialized
	{
		set
		{
			this.SetBoolHolder(value, 32);
		}
	}

	// Token: 0x1700037A RID: 890
	// (set) Token: 0x06001CE7 RID: 7399 RVA: 0x000AE433 File Offset: 0x000AC633
	public bool IsDMSEnabled
	{
		set
		{
			this.SetBoolHolder(value, 16);
		}
	}

	// Token: 0x1700037B RID: 891
	// (get) Token: 0x06001CE8 RID: 7400 RVA: 0x000AE43E File Offset: 0x000AC63E
	public bool CanUpdate
	{
		get
		{
			return this.BoolHolder == 240;
		}
	}

	// Token: 0x06001CE9 RID: 7401 RVA: 0x000AE44D File Offset: 0x000AC64D
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

	// Token: 0x040012D6 RID: 4822
	[PublicizedFrom(EAccessModifier.Private)]
	public byte BoolHolder;
}
