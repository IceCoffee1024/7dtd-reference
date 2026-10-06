using System;
using UnityEngine;

// Token: 0x0200158B RID: 5515
public static class vp_TargetEvent<T>
{
	// Token: 0x0600AC66 RID: 44134 RVA: 0x004049BC File Offset: 0x00402BBC
	public static void Register(object target, string eventName, Action<T> callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 1);
	}

	// Token: 0x0600AC67 RID: 44135 RVA: 0x004048F7 File Offset: 0x00402AF7
	public static void Unregister(object target, string eventName, Action<T> callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600AC68 RID: 44136 RVA: 0x00404901 File Offset: 0x00402B01
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600AC69 RID: 44137 RVA: 0x004049C8 File Offset: 0x00402BC8
	public static void Send(object target, string eventName, T arg, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, false, 1, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				((Action<T>)callback)(arg);
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

	// Token: 0x0600AC6A RID: 44138 RVA: 0x00404A1C File Offset: 0x00402C1C
	public static void SendUpwards(Component target, string eventName, T arg, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, true, 1, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				((Action<T>)callback)(arg);
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
