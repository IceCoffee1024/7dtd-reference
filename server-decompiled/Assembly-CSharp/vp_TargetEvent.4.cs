using System;
using UnityEngine;

// Token: 0x0200156B RID: 5483
public static class vp_TargetEvent<T, U, V>
{
	// Token: 0x0600ABBD RID: 43965 RVA: 0x003FFCD8 File Offset: 0x003FDED8
	public static void Register(object target, string eventName, Action<T, U, V> callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 3);
	}

	// Token: 0x0600ABBE RID: 43966 RVA: 0x003FFAA3 File Offset: 0x003FDCA3
	public static void Unregister(object target, string eventName, Action<T, U, V> callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600ABBF RID: 43967 RVA: 0x003FFAAD File Offset: 0x003FDCAD
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600ABC0 RID: 43968 RVA: 0x003FFCE4 File Offset: 0x003FDEE4
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

	// Token: 0x0600ABC1 RID: 43969 RVA: 0x003FFD3C File Offset: 0x003FDF3C
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
