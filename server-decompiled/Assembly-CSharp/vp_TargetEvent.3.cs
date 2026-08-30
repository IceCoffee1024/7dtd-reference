using System;
using UnityEngine;

// Token: 0x0200156A RID: 5482
public static class vp_TargetEvent<T, U>
{
	// Token: 0x0600ABB8 RID: 43960 RVA: 0x003FFC1C File Offset: 0x003FDE1C
	public static void Register(object target, string eventName, Action<T, U> callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 2);
	}

	// Token: 0x0600ABB9 RID: 43961 RVA: 0x003FFAA3 File Offset: 0x003FDCA3
	public static void Unregister(object target, string eventName, Action<T, U> callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600ABBA RID: 43962 RVA: 0x003FFAAD File Offset: 0x003FDCAD
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600ABBB RID: 43963 RVA: 0x003FFC28 File Offset: 0x003FDE28
	public static void Send(object target, string eventName, T arg1, U arg2, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, false, 2, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				((Action<T, U>)callback)(arg1, arg2);
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

	// Token: 0x0600ABBC RID: 43964 RVA: 0x003FFC80 File Offset: 0x003FDE80
	public static void SendUpwards(Component target, string eventName, T arg1, U arg2, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, true, 2, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				((Action<T, U>)callback)(arg1, arg2);
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
