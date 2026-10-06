using System;

// Token: 0x02001562 RID: 5474
public class vp_Activity<V> : vp_Activity
{
	// Token: 0x0600ABA8 RID: 43944 RVA: 0x004015E5 File Offset: 0x003FF7E5
	public vp_Activity(string name)
		: base(name)
	{
	}

	// Token: 0x0600ABA9 RID: 43945 RVA: 0x004015EE File Offset: 0x003FF7EE
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
