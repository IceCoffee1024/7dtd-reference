using System;
using UnityEngine;

// Token: 0x02001591 RID: 5521
public static class vp_TargetEventReturn<T, U, V, R>
{
	// Token: 0x0600AC85 RID: 44165 RVA: 0x00404E44 File Offset: 0x00403044
	public static void Register(object target, string eventName, Func<T, U, V, R> callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 7);
	}

	// Token: 0x0600AC86 RID: 44166 RVA: 0x004048F7 File Offset: 0x00402AF7
	public static void Unregister(object target, string eventName, Func<T, U, V, R> callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600AC87 RID: 44167 RVA: 0x00404901 File Offset: 0x00402B01
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600AC88 RID: 44168 RVA: 0x00404E50 File Offset: 0x00403050
	public static R Send(object target, string eventName, T arg1, U arg2, V arg3, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		R r;
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, false, 7, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				r = ((Func<T, U, V, R>)callback)(arg1, arg2, arg3);
			}
			catch
			{
				eventName += "_";
				continue;
			}
			return r;
		}
		vp_TargetEventHandler.OnNoReceiver(eventName, options);
		r = default(R);
		return r;
	}

	// Token: 0x0600AC89 RID: 44169 RVA: 0x00404EB4 File Offset: 0x004030B4
	public static R SendUpwards(Component target, string eventName, T arg1, U arg2, V arg3, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		R r;
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, true, 7, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				r = ((Func<T, U, V, R>)callback)(arg1, arg2, arg3);
			}
			catch
			{
				eventName += "_";
				continue;
			}
			return r;
		}
		vp_TargetEventHandler.OnNoReceiver(eventName, options);
		r = default(R);
		return r;
	}
}
