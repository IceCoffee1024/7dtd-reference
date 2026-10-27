using System;
using UnityEngine;

// Token: 0x020015A0 RID: 5536
public static class vp_TargetEventReturn<R>
{
	// Token: 0x0600ACCD RID: 44237 RVA: 0x00406FEC File Offset: 0x004051EC
	public static void Register(object target, string eventName, Func<R> callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 4);
	}

	// Token: 0x0600ACCE RID: 44238 RVA: 0x00406CFB File Offset: 0x00404EFB
	public static void Unregister(object target, string eventName, Func<R> callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600ACCF RID: 44239 RVA: 0x00406D05 File Offset: 0x00404F05
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600ACD0 RID: 44240 RVA: 0x00406D0F File Offset: 0x00404F0F
	public static void Unregister(Component component)
	{
		vp_TargetEventHandler.Unregister(component);
	}

	// Token: 0x0600ACD1 RID: 44241 RVA: 0x00406FF8 File Offset: 0x004051F8
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

	// Token: 0x0600ACD2 RID: 44242 RVA: 0x00407054 File Offset: 0x00405254
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
