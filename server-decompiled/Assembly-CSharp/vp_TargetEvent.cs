using System;
using UnityEngine;

// Token: 0x02001568 RID: 5480
public static class vp_TargetEvent
{
	// Token: 0x0600ABAD RID: 43949 RVA: 0x003FFA98 File Offset: 0x003FDC98
	public static void Register(object target, string eventName, Action callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 0);
	}

	// Token: 0x0600ABAE RID: 43950 RVA: 0x003FFAA3 File Offset: 0x003FDCA3
	public static void Unregister(object target, string eventName, Action callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600ABAF RID: 43951 RVA: 0x003FFAAD File Offset: 0x003FDCAD
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600ABB0 RID: 43952 RVA: 0x003FFAB7 File Offset: 0x003FDCB7
	public static void Unregister(Component component)
	{
		vp_TargetEventHandler.Unregister(component);
	}

	// Token: 0x0600ABB1 RID: 43953 RVA: 0x003FFAC0 File Offset: 0x003FDCC0
	public static void Send(object target, string eventName, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, false, 0, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				((Action)callback)();
			}
			catch
			{
				eventName += "_";
				continue;
			}
			return;
		}
		vp_TargetEventHandler.OnNoReceiver(eventName, options);
	}

	// Token: 0x0600ABB2 RID: 43954 RVA: 0x003FFB14 File Offset: 0x003FDD14
	public static void SendUpwards(Component target, string eventName, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, true, 0, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				((Action)callback)();
			}
			catch
			{
				eventName += "_";
				continue;
			}
			return;
		}
		vp_TargetEventHandler.OnNoReceiver(eventName, options);
	}
}
