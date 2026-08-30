using System;
using UnityEngine;

// Token: 0x02001554 RID: 5460
public static class vp_TargetEventReturn<T, U, V, R>
{
	// Token: 0x0600AAD5 RID: 43733 RVA: 0x003FACB0 File Offset: 0x003F8EB0
	public static void Register(object target, string eventName, Func<T, U, V, R> callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 7);
	}

	// Token: 0x0600AAD6 RID: 43734 RVA: 0x003FA763 File Offset: 0x003F8963
	public static void Unregister(object target, string eventName, Func<T, U, V, R> callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600AAD7 RID: 43735 RVA: 0x003FA76D File Offset: 0x003F896D
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600AAD8 RID: 43736 RVA: 0x003FACBC File Offset: 0x003F8EBC
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

	// Token: 0x0600AAD9 RID: 43737 RVA: 0x003FAD20 File Offset: 0x003F8F20
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
