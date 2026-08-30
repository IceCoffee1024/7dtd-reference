using System;
using UnityEngine;

// Token: 0x0200154E RID: 5454
public static class vp_TargetEvent<T>
{
	// Token: 0x0600AAB6 RID: 43702 RVA: 0x003FA828 File Offset: 0x003F8A28
	public static void Register(object target, string eventName, Action<T> callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 1);
	}

	// Token: 0x0600AAB7 RID: 43703 RVA: 0x003FA763 File Offset: 0x003F8963
	public static void Unregister(object target, string eventName, Action<T> callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600AAB8 RID: 43704 RVA: 0x003FA76D File Offset: 0x003F896D
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600AAB9 RID: 43705 RVA: 0x003FA834 File Offset: 0x003F8A34
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

	// Token: 0x0600AABA RID: 43706 RVA: 0x003FA888 File Offset: 0x003F8A88
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
