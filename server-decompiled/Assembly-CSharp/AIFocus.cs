using System;

// Token: 0x02000447 RID: 1095
public readonly struct AIFocus<T> where T : struct, IFocusTarget
{
	// Token: 0x06002150 RID: 8528 RVA: 0x000C9307 File Offset: 0x000C7507
	public AIFocus(bool thisIsDumb)
	{
		this.FocusTargets = new T[4];
	}

	// Token: 0x06002151 RID: 8529 RVA: 0x000C9315 File Offset: 0x000C7515
	public void SetFocus(FocusPriority priority, T newTarget)
	{
		this.FocusTargets[(int)priority] = newTarget;
	}

	// Token: 0x06002152 RID: 8530 RVA: 0x000C9324 File Offset: 0x000C7524
	public void ClearFocus(FocusPriority priority)
	{
		this.FocusTargets[(int)priority] = new T();
	}

	// Token: 0x040016F1 RID: 5873
	public readonly T[] FocusTargets;
}
