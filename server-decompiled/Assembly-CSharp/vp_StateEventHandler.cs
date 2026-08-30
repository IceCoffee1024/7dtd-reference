using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001548 RID: 5448
[Preserve]
public abstract class vp_StateEventHandler : vp_EventHandler
{
	// Token: 0x0600AA99 RID: 43673 RVA: 0x003F9B58 File Offset: 0x003F7D58
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

	// Token: 0x0600AA9A RID: 43674 RVA: 0x003F9BBC File Offset: 0x003F7DBC
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void BindStateToActivity(vp_Activity a)
	{
		this.BindStateToActivityOnStart(a);
		this.BindStateToActivityOnStop(a);
	}

	// Token: 0x0600AA9B RID: 43675 RVA: 0x003F9BCC File Offset: 0x003F7DCC
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

	// Token: 0x0600AA9C RID: 43676 RVA: 0x003F9C20 File Offset: 0x003F7E20
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

	// Token: 0x0600AA9D RID: 43677 RVA: 0x003F9C74 File Offset: 0x003F7E74
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

	// Token: 0x0600AA9E RID: 43678 RVA: 0x003F9D3C File Offset: 0x003F7F3C
	[Preserve]
	public void ResetActivityStates()
	{
		foreach (vp_Component vp_Component in this.m_StateTargets)
		{
			vp_Component.ResetState();
		}
	}

	// Token: 0x0600AA9F RID: 43679 RVA: 0x003F9D8C File Offset: 0x003F7F8C
	[Preserve]
	public void SetState(string state, bool setActive = true, bool recursive = true, bool includeDisabled = false)
	{
		foreach (vp_Component vp_Component in this.m_StateTargets)
		{
			vp_Component.SetState(state, setActive, recursive, includeDisabled);
		}
	}

	// Token: 0x0600AAA0 RID: 43680 RVA: 0x003F9DE4 File Offset: 0x003F7FE4
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

	// Token: 0x0600AAA1 RID: 43681 RVA: 0x003F9E48 File Offset: 0x003F8048
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_StateEventHandler()
	{
	}

	// Token: 0x04007E42 RID: 32322
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<vp_Component> m_StateTargets = new List<vp_Component>();
}
