using System;
using UnityEngine;

// Token: 0x0200156C RID: 5484
public static class vp_TargetEventReturn<R>
{
	// Token: 0x0600ABC2 RID: 43970 RVA: 0x003FFD94 File Offset: 0x003FDF94
	public static void Register(object target, string eventName, Func<R> callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 4);
	}

	// Token: 0x0600ABC3 RID: 43971 RVA: 0x003FFAA3 File Offset: 0x003FDCA3
	public static void Unregister(object target, string eventName, Func<R> callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600ABC4 RID: 43972 RVA: 0x003FFAAD File Offset: 0x003FDCAD
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600ABC5 RID: 43973 RVA: 0x003FFAB7 File Offset: 0x003FDCB7
	public static void Unregister(Component component)
	{
		vp_TargetEventHandler.Unregister(component);
	}

	// Token: 0x0600ABC6 RID: 43974 RVA: 0x003FFDA0 File Offset: 0x003FDFA0
	public static R Send(object target, string eventName, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		R r;
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, false, 4, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				r = ((Func<R>)callback)();
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

	// Token: 0x0600ABC7 RID: 43975 RVA: 0x003FFDFC File Offset: 0x003FDFFC
	public static R SendUpwards(Component target, string eventName, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		R r;
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, true, 4, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				r = ((Func<R>)callback)();
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
