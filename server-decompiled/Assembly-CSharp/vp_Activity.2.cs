using System;

// Token: 0x02001540 RID: 5440
public class vp_Activity<V> : vp_Activity
{
	// Token: 0x0600AAF5 RID: 43765 RVA: 0x003FC791 File Offset: 0x003FA991
	public vp_Activity(string name)
		: base(name)
	{
	}

	// Token: 0x0600AAF6 RID: 43766 RVA: 0x003FC79A File Offset: 0x003FA99A
	public bool TryStart<T>(T argument)
	{
		if (this.m_Active)
		{
			return false;
		}
		this.m_Argument = argument;
		return base.TryStart(true);
	}
}
