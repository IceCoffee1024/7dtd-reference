using System;

// Token: 0x02000449 RID: 1097
public readonly struct AIFocus<T> where T : struct, IFocusTarget
{
	// Token: 0x06002155 RID: 8533 RVA: 0x000C96B3 File Offset: 0x000C78B3
	public AIFocus(bool thisIsDumb)
	{
		this.FocusTargets = new T[4];
	}

	// Token: 0x06002156 RID: 8534 RVA: 0x000C96C1 File Offset: 0x000C78C1
	public void SetFocus(FocusPriority priority, T newTarget)
	{
		this.FocusTargets[(int)priority] = newTarget;
	}

	// Token: 0x06002157 RID: 8535 RVA: 0x000C96D0 File Offset: 0x000C78D0
	public void ClearFocus(FocusPriority priority)
	{
		this.FocusTargets[(int)priority] = new T();
	}

	// Token: 0x04001701 RID: 5889
	public readonly T[] FocusTargets;
}
