using System;
using UnityEngine;

// Token: 0x0200158A RID: 5514
public static class vp_TargetEvent
{
	// Token: 0x0600AC60 RID: 44128 RVA: 0x004048EC File Offset: 0x00402AEC
	public static void Register(object target, string eventName, Action callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 0);
	}

	// Token: 0x0600AC61 RID: 44129 RVA: 0x004048F7 File Offset: 0x00402AF7
	public static void Unregister(object target, string eventName, Action callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600AC62 RID: 44130 RVA: 0x00404901 File Offset: 0x00402B01
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600AC63 RID: 44131 RVA: 0x0040490B File Offset: 0x00402B0B
	public static void Unregister(Component component)
	{
		vp_TargetEventHandler.Unregister(component);
	}

	// Token: 0x0600AC64 RID: 44132 RVA: 0x00404914 File Offset: 0x00402B14
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

	// Token: 0x0600AC65 RID: 44133 RVA: 0x00404968 File Offset: 0x00402B68
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
