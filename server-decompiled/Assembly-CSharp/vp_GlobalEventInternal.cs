using System;
using System.Collections;

// Token: 0x02001552 RID: 5458
[PublicizedFrom(EAccessModifier.Internal)]
public static class vp_GlobalEventInternal
{
	// Token: 0x0600AB4E RID: 43854 RVA: 0x003FE00D File Offset: 0x003FC20D
	public static vp_GlobalEventInternal.UnregisterException ShowUnregisterException(string name)
	{
		return new vp_GlobalEventInternal.UnregisterException(string.Format("Attempting to Unregister the event {0} but vp_GlobalEvent has not registered this event.", name));
	}

	// Token: 0x0600AB4F RID: 43855 RVA: 0x003FE01F File Offset: 0x003FC21F
	public static vp_GlobalEventInternal.SendException ShowSendException(string name)
	{
		return new vp_GlobalEventInternal.SendException(string.Format("Attempting to Send the event {0} but vp_GlobalEvent has not registered this event.", name));
	}

	// Token: 0x04007F0F RID: 32527
	public static Hashtable Callbacks = new Hashtable();

	// Token: 0x02001553 RID: 5459
	public class UnregisterException : Exception
	{
		// Token: 0x0600AB51 RID: 43857 RVA: 0x003C1227 File Offset: 0x003BF427
		public UnregisterException(string msg)
			: base(msg)
		{
		}
	}

	// Token: 0x02001554 RID: 5460
	public class SendException : Exception
	{
		// Token: 0x0600AB52 RID: 43858 RVA: 0x003C1227 File Offset: 0x003BF427
		public SendException(string msg)
			: base(msg)
		{
		}
	}
}
