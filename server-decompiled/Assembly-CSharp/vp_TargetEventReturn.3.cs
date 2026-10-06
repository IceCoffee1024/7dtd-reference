using System;
using UnityEngine;

// Token: 0x02001590 RID: 5520
public static class vp_TargetEventReturn<T, U, R>
{
	// Token: 0x0600AC80 RID: 44160 RVA: 0x00404D78 File Offset: 0x00402F78
	public static void Register(object target, string eventName, Func<T, U, R> callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 6);
	}

	// Token: 0x0600AC81 RID: 44161 RVA: 0x004048F7 File Offset: 0x00402AF7
	public static void Unregister(object target, string eventName, Func<T, U, R> callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600AC82 RID: 44162 RVA: 0x00404901 File Offset: 0x00402B01
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600AC83 RID: 44163 RVA: 0x00404D84 File Offset: 0x00402F84
	public static R Send(object target, string eventName, T arg1, U arg2, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		R r;
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, false, 6, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				r = ((Func<T, U, R>)callback)(arg1, arg2);
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

	// Token: 0x0600AC84 RID: 44164 RVA: 0x00404DE4 File Offset: 0x00402FE4
	public static R SendUpwards(Component target, string eventName, T arg1, U arg2, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		R r;
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, true, 6, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				r = ((Func<T, U, R>)callback)(arg1, arg2);
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
