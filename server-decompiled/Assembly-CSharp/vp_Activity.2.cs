using System;

// Token: 0x02001574 RID: 5492
public class vp_Activity<V> : vp_Activity
{
	// Token: 0x0600AC00 RID: 44032 RVA: 0x004039E9 File Offset: 0x00401BE9
	public vp_Activity(string name)
		: base(name)
	{
	}

	// Token: 0x0600AC01 RID: 44033 RVA: 0x004039F2 File Offset: 0x00401BF2
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
