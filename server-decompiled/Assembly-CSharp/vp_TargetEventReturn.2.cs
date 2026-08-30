using System;
using UnityEngine;

// Token: 0x0200156D RID: 5485
public static class vp_TargetEventReturn<T, R>
{
	// Token: 0x0600ABC8 RID: 43976 RVA: 0x003FFE58 File Offset: 0x003FE058
	public static void Register(object target, string eventName, Func<T, R> callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 5);
	}

	// Token: 0x0600ABC9 RID: 43977 RVA: 0x003FFAA3 File Offset: 0x003FDCA3
	public static void Unregister(object target, string eventName, Func<T, R> callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600ABCA RID: 43978 RVA: 0x003FFAAD File Offset: 0x003FDCAD
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600ABCB RID: 43979 RVA: 0x003FFE64 File Offset: 0x003FE064
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

	// Token: 0x0600ABCC RID: 43980 RVA: 0x003FFEC4 File Offset: 0x003FE0C4
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
