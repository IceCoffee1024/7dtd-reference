using System;

// Token: 0x02000441 RID: 1089
public readonly struct AIFocus<T> where T : struct, IFocusTarget
{
	// Token: 0x06002144 RID: 8516 RVA: 0x000C8A47 File Offset: 0x000C6C47
	public AIFocus(bool thisIsDumb)
	{
		this.FocusTargets = new T[4];
	}

	// Token: 0x06002145 RID: 8517 RVA: 0x000C8A55 File Offset: 0x000C6C55
	public void SetFocus(FocusPriority priority, T newTarget)
	{
		this.FocusTargets[(int)priority] = newTarget;
	}

	// Token: 0x06002146 RID: 8518 RVA: 0x000C8A64 File Offset: 0x000C6C64
	public void ClearFocus(FocusPriority priority)
	{
		this.FocusTargets[(int)priority] = new T();
	}

	// Token: 0x040016E5 RID: 5861
	public readonly T[] FocusTargets;
}
