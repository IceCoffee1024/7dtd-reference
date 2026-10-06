using System;
using System.Collections;

// Token: 0x02001574 RID: 5492
[PublicizedFrom(EAccessModifier.Internal)]
public static class vp_GlobalEventInternal
{
	// Token: 0x0600AC01 RID: 44033 RVA: 0x00402E61 File Offset: 0x00401061
	public static vp_GlobalEventInternal.UnregisterException ShowUnregisterException(string name)
	{
		return new vp_GlobalEventInternal.UnregisterException(string.Format("Attempting to Unregister the event {0} but vp_GlobalEvent has not registered this event.", name));
	}

	// Token: 0x0600AC02 RID: 44034 RVA: 0x00402E73 File Offset: 0x00401073
	public static vp_GlobalEventInternal.SendException ShowSendException(string name)
	{
		return new vp_GlobalEventInternal.SendException(string.Format("Attempting to Send the event {0} but vp_GlobalEvent has not registered this event.", name));
	}

	// Token: 0x04007F92 RID: 32658
	public static Hashtable Callbacks = new Hashtable();

	// Token: 0x02001575 RID: 5493
	public class UnregisterException : Exception
	{
		// Token: 0x0600AC04 RID: 44036 RVA: 0x003C5DC3 File Offset: 0x003C3FC3
		public UnregisterException(string msg)
			: base(msg)
		{
		}
	}

	// Token: 0x02001576 RID: 5494
	public class SendException : Exception
	{
		// Token: 0x0600AC05 RID: 44037 RVA: 0x003C5DC3 File Offset: 0x003C3FC3
		public SendException(string msg)
			: base(msg)
		{
		}
	}
}
