using System;

// Token: 0x02001525 RID: 5413
public class vp_Activity<V> : vp_Activity
{
	// Token: 0x0600A9F8 RID: 43512 RVA: 0x003F7451 File Offset: 0x003F5651
	public vp_Activity(string name)
		: base(name)
	{
	}

	// Token: 0x0600A9F9 RID: 43513 RVA: 0x003F745A File Offset: 0x003F565A
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
