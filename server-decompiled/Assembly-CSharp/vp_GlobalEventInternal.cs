using System;
using System.Collections;

// Token: 0x02001586 RID: 5510
[PublicizedFrom(EAccessModifier.Internal)]
public static class vp_GlobalEventInternal
{
	// Token: 0x0600AC59 RID: 44121 RVA: 0x00405265 File Offset: 0x00403465
	public static vp_GlobalEventInternal.UnregisterException ShowUnregisterException(string name)
	{
		return new vp_GlobalEventInternal.UnregisterException(string.Format("Attempting to Unregister the event {0} but vp_GlobalEvent has not registered this event.", name));
	}

	// Token: 0x0600AC5A RID: 44122 RVA: 0x00405277 File Offset: 0x00403477
	public static vp_GlobalEventInternal.SendException ShowSendException(string name)
	{
		return new vp_GlobalEventInternal.SendException(string.Format("Attempting to Send the event {0} but vp_GlobalEvent has not registered this event.", name));
	}

	// Token: 0x04007FD3 RID: 32723
	public static Hashtable Callbacks = new Hashtable();

	// Token: 0x02001587 RID: 5511
	public class UnregisterException : Exception
	{
		// Token: 0x0600AC5C RID: 44124 RVA: 0x003C81C7 File Offset: 0x003C63C7
		public UnregisterException(string msg)
			: base(msg)
		{
		}
	}

	// Token: 0x02001588 RID: 5512
	public class SendException : Exception
	{
		// Token: 0x0600AC5D RID: 44125 RVA: 0x003C81C7 File Offset: 0x003C63C7
		public SendException(string msg)
			: base(msg)
		{
		}
	}
}
