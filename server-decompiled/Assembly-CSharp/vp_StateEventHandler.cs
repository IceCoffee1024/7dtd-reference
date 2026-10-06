using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001585 RID: 5509
[Preserve]
public abstract class vp_StateEventHandler : vp_EventHandler
{
	// Token: 0x0600AC49 RID: 44105 RVA: 0x00403CEC File Offset: 0x00401EEC
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

	// Token: 0x0600AC4A RID: 44106 RVA: 0x00403D50 File Offset: 0x00401F50
	[Preserve]
	[PublicizedFrom(EAccessModifier.Protected)]
	public void BindStateToActivity(vp_Activity a)
	{
		this.BindStateToActivityOnStart(a);
		this.BindStateToActivityOnStop(a);
	}

	// Token: 0x0600AC4B RID: 44107 RVA: 0x00403D60 File Offset: 0x00401F60
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

	// Token: 0x0600AC4C RID: 44108 RVA: 0x00403DB4 File Offset: 0x00401FB4
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

	// Token: 0x0600AC4D RID: 44109 RVA: 0x00403E08 File Offset: 0x00402008
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

	// Token: 0x0600AC4E RID: 44110 RVA: 0x00403ED0 File Offset: 0x004020D0
	[Preserve]
	public void ResetActivityStates()
	{
		foreach (vp_Component vp_Component in this.m_StateTargets)
		{
			vp_Component.ResetState();
		}
	}

	// Token: 0x0600AC4F RID: 44111 RVA: 0x00403F20 File Offset: 0x00402120
	[Preserve]
	public void SetState(string state, bool setActive = true, bool recursive = true, bool includeDisabled = false)
	{
		foreach (vp_Component vp_Component in this.m_StateTargets)
		{
			vp_Component.SetState(state, setActive, recursive, includeDisabled);
		}
	}

	// Token: 0x0600AC50 RID: 44112 RVA: 0x00403F78 File Offset: 0x00402178
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

	// Token: 0x0600AC51 RID: 44113 RVA: 0x00403FDC File Offset: 0x004021DC
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_StateEventHandler()
	{
	}

	// Token: 0x04007F9E RID: 32670
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<vp_Component> m_StateTargets = new List<vp_Component>();
}
