using System;
using UnityEngine;

// Token: 0x0200158C RID: 5516
public static class vp_TargetEvent<T, U>
{
	// Token: 0x0600AC6B RID: 44139 RVA: 0x00404A70 File Offset: 0x00402C70
	public static void Register(object target, string eventName, Action<T, U> callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 2);
	}

	// Token: 0x0600AC6C RID: 44140 RVA: 0x004048F7 File Offset: 0x00402AF7
	public static void Unregister(object target, string eventName, Action<T, U> callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600AC6D RID: 44141 RVA: 0x00404901 File Offset: 0x00402B01
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600AC6E RID: 44142 RVA: 0x00404A7C File Offset: 0x00402C7C
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

	// Token: 0x0600AC6F RID: 44143 RVA: 0x00404AD4 File Offset: 0x00402CD4
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
