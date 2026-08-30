using System;
using UnityEngine;

// Token: 0x0200154D RID: 5453
public static class vp_TargetEvent
{
	// Token: 0x0600AAB0 RID: 43696 RVA: 0x003FA758 File Offset: 0x003F8958
	public static void Register(object target, string eventName, Action callback)
	{
		vp_TargetEventHandler.Register(target, eventName, callback, 0);
	}

	// Token: 0x0600AAB1 RID: 43697 RVA: 0x003FA763 File Offset: 0x003F8963
	public static void Unregister(object target, string eventName, Action callback)
	{
		vp_TargetEventHandler.Unregister(target, eventName, callback);
	}

	// Token: 0x0600AAB2 RID: 43698 RVA: 0x003FA76D File Offset: 0x003F896D
	public static void Unregister(object target)
	{
		vp_TargetEventHandler.Unregister(target, null, null);
	}

	// Token: 0x0600AAB3 RID: 43699 RVA: 0x003FA777 File Offset: 0x003F8977
	public static void Unregister(Component component)
	{
		vp_TargetEventHandler.Unregister(component);
	}

	// Token: 0x0600AAB4 RID: 43700 RVA: 0x003FA780 File Offset: 0x003F8980
	public static void Send(object target, string eventName, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, false, 0, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				((Action)callback)();
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

	// Token: 0x0600AAB5 RID: 43701 RVA: 0x003FA7D4 File Offset: 0x003F89D4
	public static void SendUpwards(Component target, string eventName, vp_TargetEventOptions options = vp_TargetEventOptions.DontRequireReceiver)
	{
		for (;;)
		{
			Delegate callback = vp_TargetEventHandler.GetCallback(target, eventName, true, 0, options);
			if (callback == null)
			{
				break;
			}
			try
			{
				((Action)callback)();
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
