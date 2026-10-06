using System;
using UnityEngine;

// Token: 0x0200158D RID: 5517
public static class vp_TargetEvent<T, U, V>
{
	// Token: 0x0600AC70 RID: 44144 RVA: 0x00404B2C File Offset: 0x00402D2C
	public static void Register(object target, string eventName, Action<T, U, V> callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 3);
	}

	// Token: 0x0600AC71 RID: 44145 RVA: 0x004048F7 File Offset: 0x00402AF7
	public static void Unregister(object target, string eventName, Action<T, U, V> callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600AC72 RID: 44146 RVA: 0x00404901 File Offset: 0x00402B01
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600AC73 RID: 44147 RVA: 0x00404B38 File Offset: 0x00402D38
	public static void Send(object target, string eventName, T arg1, U arg2, V arg3, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, false, 3, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				((Action<T, U, V>)callback)(arg1, arg2, arg3);
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

	// Token: 0x0600AC74 RID: 44148 RVA: 0x00404B90 File Offset: 0x00402D90
	public static void SendUpwards(Component target, string eventName, T arg1, U arg2, V arg3, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, true, 3, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				((Action<T, U, V>)callback)(arg1, arg2, arg3);
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
