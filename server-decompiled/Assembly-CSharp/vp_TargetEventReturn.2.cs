using System;
using UnityEngine;

// Token: 0x02001552 RID: 5458
public static class vp_TargetEventReturn<T, R>
{
	// Token: 0x0600AACB RID: 43723 RVA: 0x003FAB18 File Offset: 0x003F8D18
	public static void Register(object target, string eventName, Func<T, R> callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 5);
	}

	// Token: 0x0600AACC RID: 43724 RVA: 0x003FA763 File Offset: 0x003F8963
	public static void Unregister(object target, string eventName, Func<T, R> callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600AACD RID: 43725 RVA: 0x003FA76D File Offset: 0x003F896D
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600AACE RID: 43726 RVA: 0x003FAB24 File Offset: 0x003F8D24
	public static R Send(object target, string eventName, T arg, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		R r;
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, false, 5, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				r = ((Func<T, R>)callback)(arg);
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

	// Token: 0x0600AACF RID: 43727 RVA: 0x003FAB84 File Offset: 0x003F8D84
	public static R SendUpwards(Component target, string eventName, T arg, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		R r;
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, true, 5, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				r = ((Func<T, R>)callback)(arg);
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
