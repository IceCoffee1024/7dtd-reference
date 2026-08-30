using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001563 RID: 5475
[Preserve]
public abstract class vp_StateEventHandler : vp_EventHandler
{
	// Token: 0x0600AB96 RID: 43926 RVA: 0x003FEE98 File Offset: 0x003FD098
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		foreach (vp_Component vp_Component in base.transform.root.GetComponentsInChildren<vp_Component>(true))
		{
			if (vp_Component.Parent == null || vp_Component.Parent.GetComponent<vp_Component>() == null)
			{
				this.m_StateTargets.Add(vp_Component);
			}
		}
	}

	// Token: 0x0600AB97 RID: 43927 RVA: 0x003FEEFC File Offset: 0x003FD0FC
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void BindStateToActivity(vp_Activity a)
	{
		this.BindStateToActivityOnStart(a);
		this.BindStateToActivityOnStop(a);
	}

	// Token: 0x0600AB98 RID: 43928 RVA: 0x003FEF0C File Offset: 0x003FD10C
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void BindStateToActivityOnStart(vp_Activity a)
	{
		if (!this.ActivityInitialized(a))
		{
			return;
		}
		string s = a.EventName;
		a.StartCallbacks = (vp_Activity.Callback)Delegate.Combine(a.StartCallbacks, new vp_Activity.Callback(delegate
		{
			foreach (vp_Component vp_Component in this.m_StateTargets)
			{
				vp_Component.SetState(s, true, true, false);
			}
		}));
	}

	// Token: 0x0600AB99 RID: 43929 RVA: 0x003FEF60 File Offset: 0x003FD160
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void BindStateToActivityOnStop(vp_Activity a)
	{
		if (!this.ActivityInitialized(a))
		{
			return;
		}
		string s = a.EventName;
		a.StopCallbacks = (vp_Activity.Callback)Delegate.Combine(a.StopCallbacks, new vp_Activity.Callback(delegate
		{
			foreach (vp_Component vp_Component in this.m_StateTargets)
			{
				vp_Component.SetState(s, false, true, false);
			}
		}));
	}

	// Token: 0x0600AB9A RID: 43930 RVA: 0x003FEFB4 File Offset: 0x003FD1B4
	[Preserve]
	public void RefreshActivityStates()
	{
		foreach (vp_Event vp_Event in this.m_HandlerEvents.Values)
		{
			if (vp_Event is vp_Activity || vp_Event.GetType().BaseType == typeof(vp_Activity))
			{
				foreach (vp_Component vp_Component in this.m_StateTargets)
				{
					vp_Component.SetState(vp_Event.EventName, ((vp_Activity)vp_Event).Active, true, false);
				}
			}
		}
	}

	// Token: 0x0600AB9B RID: 43931 RVA: 0x003FF07C File Offset: 0x003FD27C
	[Preserve]
	public void ResetActivityStates()
	{
		foreach (vp_Component vp_Component in this.m_StateTargets)
		{
			vp_Component.ResetState();
		}
	}

	// Token: 0x0600AB9C RID: 43932 RVA: 0x003FF0CC File Offset: 0x003FD2CC
	[Preserve]
	public void SetState(string state, bool setActive = true, bool recursive = true, bool includeDisabled = false)
	{
		foreach (vp_Component vp_Component in this.m_StateTargets)
		{
			vp_Component.SetState(state, setActive, recursive, includeDisabled);
		}
	}

	// Token: 0x0600AB9D RID: 43933 RVA: 0x003FF124 File Offset: 0x003FD324
	[Preserve]
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ActivityInitialized(vp_Activity a)
	{
		if (a == null)
		{
			Debug.LogError("Error: (" + ((this != null) ? this.ToString() : null) + ") Activity is null.");
			return false;
		}
		if (string.IsNullOrEmpty(a.EventName))
		{
			Debug.LogError("Error: (" + ((this != null) ? this.ToString() : null) + ") Activity not initialized. Make sure the event handler has run its Awake call before binding layers.");
			return false;
		}
		return true;
	}

	// Token: 0x0600AB9E RID: 43934 RVA: 0x003FF188 File Offset: 0x003FD388
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_StateEventHandler()
	{
	}

	// Token: 0x04007F1B RID: 32539
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<vp_Component> m_StateTargets = new List<vp_Component>();
}
