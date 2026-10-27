using System;
using UnityEngine;

// Token: 0x0200159D RID: 5533
public static class vp_TargetEvent<T>
{
	// Token: 0x0600ACBE RID: 44222 RVA: 0x00406DC0 File Offset: 0x00404FC0
	public static void Register(object target, string eventName, Action<T> callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 1);
	}

	// Token: 0x0600ACBF RID: 44223 RVA: 0x00406CFB File Offset: 0x00404EFB
	public static void Unregister(object target, string eventName, Action<T> callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600ACC0 RID: 44224 RVA: 0x00406D05 File Offset: 0x00404F05
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600ACC1 RID: 44225 RVA: 0x00406DCC File Offset: 0x00404FCC
	public static void Send(object target, string eventName, T arg, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, false, 1, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				((Action<T>)callback)(arg);
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

	// Token: 0x0600ACC2 RID: 44226 RVA: 0x00406E20 File Offset: 0x00405020
	public static void SendUpwards(Component target, string eventName, T arg, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, true, 1, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				((Action<T>)callback)(arg);
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
