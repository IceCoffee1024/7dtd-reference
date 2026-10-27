using System;
using UnityEngine;

// Token: 0x020015A3 RID: 5539
public static class vp_TargetEventReturn<T, U, V, R>
{
	// Token: 0x0600ACDD RID: 44253 RVA: 0x00407248 File Offset: 0x00405448
	public static void Register(object target, string eventName, Func<T, U, V, R> callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 7);
	}

	// Token: 0x0600ACDE RID: 44254 RVA: 0x00406CFB File Offset: 0x00404EFB
	public static void Unregister(object target, string eventName, Func<T, U, V, R> callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600ACDF RID: 44255 RVA: 0x00406D05 File Offset: 0x00404F05
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600ACE0 RID: 44256 RVA: 0x00407254 File Offset: 0x00405454
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

	// Token: 0x0600ACE1 RID: 44257 RVA: 0x004072B8 File Offset: 0x004054B8
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
