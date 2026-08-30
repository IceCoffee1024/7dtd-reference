using System;
using UnityEngine;

// Token: 0x02001569 RID: 5481
public static class vp_TargetEvent<T>
{
	// Token: 0x0600ABB3 RID: 43955 RVA: 0x003FFB68 File Offset: 0x003FDD68
	public static void Register(object target, string eventName, Action<T> callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 1);
	}

	// Token: 0x0600ABB4 RID: 43956 RVA: 0x003FFAA3 File Offset: 0x003FDCA3
	public static void Unregister(object target, string eventName, Action<T> callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600ABB5 RID: 43957 RVA: 0x003FFAAD File Offset: 0x003FDCAD
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600ABB6 RID: 43958 RVA: 0x003FFB74 File Offset: 0x003FDD74
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

	// Token: 0x0600ABB7 RID: 43959 RVA: 0x003FFBC8 File Offset: 0x003FDDC8
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
