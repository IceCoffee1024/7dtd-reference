using System;
using UnityEngine;

// Token: 0x0200159E RID: 5534
public static class vp_TargetEvent<T, U>
{
	// Token: 0x0600ACC3 RID: 44227 RVA: 0x00406E74 File Offset: 0x00405074
	public static void Register(object target, string eventName, Action<T, U> callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 2);
	}

	// Token: 0x0600ACC4 RID: 44228 RVA: 0x00406CFB File Offset: 0x00404EFB
	public static void Unregister(object target, string eventName, Action<T, U> callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600ACC5 RID: 44229 RVA: 0x00406D05 File Offset: 0x00404F05
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600ACC6 RID: 44230 RVA: 0x00406E80 File Offset: 0x00405080
	public static void Send(object target, string eventName, T arg1, U arg2, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, false, 2, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				((Action<T, U>)callback)(arg1, arg2);
			}
			catch
			{
				eventName += "_";
				continue;
			}
			return;
		}
		vp_TargetEventHandler.OnNoReceiver(eventName, options);
	}

	// Token: 0x0600ACC7 RID: 44231 RVA: 0x00406ED8 File Offset: 0x004050D8
	public static void SendUpwards(Component target, string eventName, T arg1, U arg2, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, true, 2, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				((Action<T, U>)callback)(arg1, arg2);
			}
			catch
			{
				eventName += "_";
				continue;
			}
			return;
		}
		vp_TargetEventHandler.OnNoReceiver(eventName, options);
	}
}
