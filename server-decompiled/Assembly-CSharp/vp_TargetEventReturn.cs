using System;
using UnityEngine;

// Token: 0x0200158E RID: 5518
public static class vp_TargetEventReturn<R>
{
	// Token: 0x0600AC75 RID: 44149 RVA: 0x00404BE8 File Offset: 0x00402DE8
	public static void Register(object target, string eventName, Func<R> callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 4);
	}

	// Token: 0x0600AC76 RID: 44150 RVA: 0x004048F7 File Offset: 0x00402AF7
	public static void Unregister(object target, string eventName, Func<R> callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600AC77 RID: 44151 RVA: 0x00404901 File Offset: 0x00402B01
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600AC78 RID: 44152 RVA: 0x0040490B File Offset: 0x00402B0B
	public static void Unregister(Component component)
	{
		vp_TargetEventHandler.Unregister(component);
	}

	// Token: 0x0600AC79 RID: 44153 RVA: 0x00404BF4 File Offset: 0x00402DF4
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

	// Token: 0x0600AC7A RID: 44154 RVA: 0x00404C50 File Offset: 0x00402E50
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
