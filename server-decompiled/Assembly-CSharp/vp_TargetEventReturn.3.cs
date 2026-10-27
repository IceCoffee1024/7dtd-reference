using System;
using UnityEngine;

// Token: 0x020015A2 RID: 5538
public static class vp_TargetEventReturn<T, U, R>
{
	// Token: 0x0600ACD8 RID: 44248 RVA: 0x0040717C File Offset: 0x0040537C
	public static void Register(object target, string eventName, Func<T, U, R> callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 6);
	}

	// Token: 0x0600ACD9 RID: 44249 RVA: 0x00406CFB File Offset: 0x00404EFB
	public static void Unregister(object target, string eventName, Func<T, U, R> callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600ACDA RID: 44250 RVA: 0x00406D05 File Offset: 0x00404F05
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600ACDB RID: 44251 RVA: 0x00407188 File Offset: 0x00405388
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

	// Token: 0x0600ACDC RID: 44252 RVA: 0x004071E8 File Offset: 0x004053E8
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
