using System;
using UnityEngine;

// Token: 0x0200158F RID: 5519
public static class vp_TargetEventReturn<T, R>
{
	// Token: 0x0600AC7B RID: 44155 RVA: 0x00404CAC File Offset: 0x00402EAC
	public static void Register(object target, string eventName, Func<T, R> callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 5);
	}

	// Token: 0x0600AC7C RID: 44156 RVA: 0x004048F7 File Offset: 0x00402AF7
	public static void Unregister(object target, string eventName, Func<T, R> callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600AC7D RID: 44157 RVA: 0x00404901 File Offset: 0x00402B01
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600AC7E RID: 44158 RVA: 0x00404CB8 File Offset: 0x00402EB8
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

	// Token: 0x0600AC7F RID: 44159 RVA: 0x00404D18 File Offset: 0x00402F18
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
