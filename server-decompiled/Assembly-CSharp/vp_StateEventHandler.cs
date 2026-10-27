using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001597 RID: 5527
[Preserve]
public abstract class vp_StateEventHandler : vp_EventHandler
{
	// Token: 0x0600ACA1 RID: 44193 RVA: 0x004060F0 File Offset: 0x004042F0
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

	// Token: 0x0600ACA2 RID: 44194 RVA: 0x00406154 File Offset: 0x00404354
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void BindStateToActivity(vp_Activity a)
	{
		this.BindStateToActivityOnStart(a);
		this.BindStateToActivityOnStop(a);
	}

	// Token: 0x0600ACA3 RID: 44195 RVA: 0x00406164 File Offset: 0x00404364
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

	// Token: 0x0600ACA4 RID: 44196 RVA: 0x004061B8 File Offset: 0x004043B8
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

	// Token: 0x0600ACA5 RID: 44197 RVA: 0x0040620C File Offset: 0x0040440C
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

	// Token: 0x0600ACA6 RID: 44198 RVA: 0x004062D4 File Offset: 0x004044D4
	[Preserve]
	public void ResetActivityStates()
	{
		foreach (vp_Component vp_Component in this.m_StateTargets)
		{
			vp_Component.ResetState();
		}
	}

	// Token: 0x0600ACA7 RID: 44199 RVA: 0x00406324 File Offset: 0x00404524
	[Preserve]
	public void SetState(string state, bool setActive = true, bool recursive = true, bool includeDisabled = false)
	{
		foreach (vp_Component vp_Component in this.m_StateTargets)
		{
			vp_Component.SetState(state, setActive, recursive, includeDisabled);
		}
	}

	// Token: 0x0600ACA8 RID: 44200 RVA: 0x0040637C File Offset: 0x0040457C
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

	// Token: 0x0600ACA9 RID: 44201 RVA: 0x004063E0 File Offset: 0x004045E0
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_StateEventHandler()
	{
	}

	// Token: 0x04007FDF RID: 32735
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<vp_Component> m_StateTargets = new List<vp_Component>();
}
