using System;
using System.Collections;

// Token: 0x02001537 RID: 5431
[PublicizedFrom(EAccessModifier.Internal)]
public static class vp_GlobalEventInternal
{
	// Token: 0x0600AA51 RID: 43601 RVA: 0x003F8CCD File Offset: 0x003F6ECD
	public static vp_GlobalEventInternal.UnregisterException ShowUnregisterException(string name)
	{
		return new vp_GlobalEventInternal.UnregisterException(string.Format("Attempting to Unregister the event {0} but vp_GlobalEvent has not registered this event.", name));
	}

	// Token: 0x0600AA52 RID: 43602 RVA: 0x003F8CDF File Offset: 0x003F6EDF
	public static vp_GlobalEventInternal.SendException ShowSendException(string name)
	{
		return new vp_GlobalEventInternal.SendException(string.Format("Attempting to Send the event {0} but vp_GlobalEvent has not registered this event.", name));
	}

	// Token: 0x04007E36 RID: 32310
	public static Hashtable Callbacks = new Hashtable();

	// Token: 0x02001538 RID: 5432
	public class UnregisterException : Exception
	{
		// Token: 0x0600AA54 RID: 43604 RVA: 0x003BBEE3 File Offset: 0x003BA0E3
		public UnregisterException(string msg)
			: base(msg)
		{
		}
	}

	// Token: 0x02001539 RID: 5433
	public class SendException : Exception
	{
		// Token: 0x0600AA55 RID: 43605 RVA: 0x003BBEE3 File Offset: 0x003BA0E3
		public SendException(string msg)
			: base(msg)
		{
		}
	}
}
