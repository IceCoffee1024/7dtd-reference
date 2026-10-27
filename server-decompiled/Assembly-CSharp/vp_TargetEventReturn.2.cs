using System;
using UnityEngine;

// Token: 0x020015A1 RID: 5537
public static class vp_TargetEventReturn<T, R>
{
	// Token: 0x0600ACD3 RID: 44243 RVA: 0x004070B0 File Offset: 0x004052B0
	public static void Register(object target, string eventName, Func<T, R> callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 5);
	}

	// Token: 0x0600ACD4 RID: 44244 RVA: 0x00406CFB File Offset: 0x00404EFB
	public static void Unregister(object target, string eventName, Func<T, R> callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600ACD5 RID: 44245 RVA: 0x00406D05 File Offset: 0x00404F05
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600ACD6 RID: 44246 RVA: 0x004070BC File Offset: 0x004052BC
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

	// Token: 0x0600ACD7 RID: 44247 RVA: 0x0040711C File Offset: 0x0040531C
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
