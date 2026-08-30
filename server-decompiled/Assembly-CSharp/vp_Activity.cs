using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200153D RID: 5437
[Preserve]
public class vp_Activity : vp_Event
{
	// Token: 0x0600AAD7 RID: 43735 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void Empty()
	{
	}

	// Token: 0x0600AAD8 RID: 43736 RVA: 0x00020021 File Offset: 0x0001E221
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool AlwaysOK()
	{
		return true;
	}

	// Token: 0x0600AAD9 RID: 43737 RVA: 0x003FC11A File Offset: 0x003FA31A
	public vp_Activity(string name)
		: base(name)
	{
		this.InitFields();
	}

	// Token: 0x170013DC RID: 5084
	// (get) Token: 0x0600AADA RID: 43738 RVA: 0x003FC13F File Offset: 0x003FA33F
	// (set) Token: 0x0600AADB RID: 43739 RVA: 0x003FC147 File Offset: 0x003FA347
	public float MinPause
	{
		get
		{
			return this.m_MinPause;
		}
		set
		{
			this.m_MinPause = Mathf.Max(0f, value);
		}
	}

	// Token: 0x170013DD RID: 5085
	// (get) Token: 0x0600AADC RID: 43740 RVA: 0x003FC15A File Offset: 0x003FA35A
	// (set) Token: 0x0600AADD RID: 43741 RVA: 0x003FC164 File Offset: 0x003FA364
	public float MinDuration
	{
		get
		{
			return this.m_MinDuration;
		}
		set
		{
			this.m_MinDuration = Mathf.Max(0.001f, value);
			if (this.m_MaxDuration == -1f)
			{
				return;
			}
			if (this.m_MinDuration > this.m_MaxDuration)
			{
				this.m_MinDuration = this.m_MaxDuration;
				Debug.LogWarning("Warning: (vp_Activity) Tried to set MinDuration longer than MaxDuration for '" + base.EventName + "'. Capping at MaxDuration.");
			}
		}
	}

	// Token: 0x170013DE RID: 5086
	// (get) Token: 0x0600AADE RID: 43742 RVA: 0x003FC1C4 File Offset: 0x003FA3C4
	// (set) Token: 0x0600AADF RID: 43743 RVA: 0x003FC1CC File Offset: 0x003FA3CC
	public float AutoDuration
	{
		get
		{
			return this.m_MaxDuration;
		}
		set
		{
			if (value == -1f)
			{
				this.m_MaxDuration = value;
				return;
			}
			this.m_MaxDuration = Mathf.Max(0.001f, value);
			if (this.m_MaxDuration < this.m_MinDuration)
			{
				this.m_MaxDuration = this.m_MinDuration;
				Debug.LogWarning("Warning: (vp_Activity) Tried to set MaxDuration shorter than MinDuration for '" + base.EventName + "'. Capping at MinDuration.");
			}
		}
	}

	// Token: 0x170013DF RID: 5087
	// (get) Token: 0x0600AAE0 RID: 43744 RVA: 0x003FC230 File Offset: 0x003FA430
	// (set) Token: 0x0600AAE1 RID: 43745 RVA: 0x003FC294 File Offset: 0x003FA494
	public object Argument
	{
		get
		{
			if (this.m_ArgumentType == null)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Error: (",
					(this != null) ? this.ToString() : null,
					") Tried to fetch argument from '",
					base.EventName,
					"' but this activity takes no parameters."
				}));
				return null;
			}
			return this.m_Argument;
		}
		set
		{
			if (this.m_ArgumentType == null)
			{
				Debug.LogError(string.Concat(new string[]
				{
					"Error: (",
					(this != null) ? this.ToString() : null,
					") Tried to set argument for '",
					base.EventName,
					"' but this activity takes no parameters."
				}));
				return;
			}
			this.m_Argument = value;
		}
	}

	// Token: 0x0600AAE2 RID: 43746 RVA: 0x003FC2F8 File Offset: 0x003FA4F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InitFields()
	{
		this.m_DelegateTypes = new Type[]
		{
			typeof(vp_Activity.Callback),
			typeof(vp_Activity.Callback),
			typeof(vp_Activity.Condition),
			typeof(vp_Activity.Condition),
			typeof(vp_Activity.Callback),
			typeof(vp_Activity.Callback)
		};
		this.m_Fields = new FieldInfo[]
		{
			base.GetType().GetField("StartCallbacks"),
			base.GetType().GetField("StopCallbacks"),
			base.GetType().GetField("StartConditions"),
			base.GetType().GetField("StopConditions"),
			base.GetType().GetField("FailStartCallbacks"),
			base.GetType().GetField("FailStopCallbacks")
		};
		base.StoreInvokerFieldNames();
		this.m_DefaultMethods = new MethodInfo[]
		{
			base.GetType().GetMethod("Empty"),
			base.GetType().GetMethod("Empty"),
			base.GetType().GetMethod("AlwaysOK"),
			base.GetType().GetMethod("AlwaysOK"),
			base.GetType().GetMethod("Empty"),
			base.GetType().GetMethod("Empty")
		};
		this.Prefixes = new Dictionary<string, int>
		{
			{ "OnStart_", 0 },
			{ "OnStop_", 1 },
			{ "CanStart_", 2 },
			{ "CanStop_", 3 },
			{ "OnFailStart_", 4 },
			{ "OnFailStop_", 5 }
		};
		this.StartCallbacks = new vp_Activity.Callback(vp_Activity.Empty);
		this.StopCallbacks = new vp_Activity.Callback(vp_Activity.Empty);
		this.StartConditions = new vp_Activity.Condition(vp_Activity.AlwaysOK);
		this.StopConditions = new vp_Activity.Condition(vp_Activity.AlwaysOK);
		this.FailStartCallbacks = new vp_Activity.Callback(vp_Activity.Empty);
		this.FailStopCallbacks = new vp_Activity.Callback(vp_Activity.Empty);
	}

	// Token: 0x0600AAE3 RID: 43747 RVA: 0x003FC520 File Offset: 0x003FA720
	public override void Register(object t, string m, int v)
	{
		base.AddExternalMethodToField(t, this.m_Fields[v], m, this.m_DelegateTypes[v]);
		base.Refresh();
	}

	// Token: 0x0600AAE4 RID: 43748 RVA: 0x003FC540 File Offset: 0x003FA740
	public override void Unregister(object t)
	{
		base.RemoveExternalMethodFromField(t, this.m_Fields[0]);
		base.RemoveExternalMethodFromField(t, this.m_Fields[1]);
		base.RemoveExternalMethodFromField(t, this.m_Fields[2]);
		base.RemoveExternalMethodFromField(t, this.m_Fields[3]);
		base.RemoveExternalMethodFromField(t, this.m_Fields[4]);
		base.RemoveExternalMethodFromField(t, this.m_Fields[5]);
		base.Refresh();
	}

	// Token: 0x0600AAE5 RID: 43749 RVA: 0x003FC5B0 File Offset: 0x003FA7B0
	public bool TryStart(bool startIfAllowed = true)
	{
		if (this.m_Active)
		{
			return false;
		}
		if (Time.time < this.NextAllowedStartTime)
		{
			this.m_Argument = null;
			return false;
		}
		Delegate[] invocationList = this.StartConditions.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			if (!((vp_Activity.Condition)invocationList[i])())
			{
				this.m_Argument = null;
				if (startIfAllowed)
				{
					this.FailStartCallbacks();
				}
				return false;
			}
		}
		if (startIfAllowed)
		{
			this.Active = true;
		}
		return true;
	}

	// Token: 0x0600AAE6 RID: 43750 RVA: 0x003FC628 File Offset: 0x003FA828
	public bool TryStop(bool stopIfAllowed = true)
	{
		if (!this.m_Active)
		{
			return false;
		}
		if (Time.time < this.NextAllowedStopTime)
		{
			return false;
		}
		Delegate[] invocationList = this.StopConditions.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			if (!((vp_Activity.Condition)invocationList[i])())
			{
				if (stopIfAllowed)
				{
					this.FailStopCallbacks();
				}
				return false;
			}
		}
		if (stopIfAllowed)
		{
			this.Active = false;
		}
		return true;
	}

	// Token: 0x170013E0 RID: 5088
	// (get) Token: 0x0600AAE8 RID: 43752 RVA: 0x003FC731 File Offset: 0x003FA931
	// (set) Token: 0x0600AAE7 RID: 43751 RVA: 0x003FC694 File Offset: 0x003FA894
	public bool Active
	{
		get
		{
			return this.m_Active;
		}
		set
		{
			if (value && !this.m_Active)
			{
				this.m_Active = true;
				this.StartCallbacks();
				this.NextAllowedStopTime = Time.time + this.m_MinDuration;
				if (this.m_MaxDuration > 0f)
				{
					vp_Timer.In(this.m_MaxDuration, delegate
					{
						this.Stop(0f);
					}, this.m_ForceStopTimer);
					return;
				}
			}
			else if (!value && this.m_Active)
			{
				this.m_Active = false;
				this.StopCallbacks();
				this.NextAllowedStartTime = Time.time + this.m_MinPause;
				this.m_Argument = null;
			}
		}
	}

	// Token: 0x0600AAE9 RID: 43753 RVA: 0x003FC739 File Offset: 0x003FA939
	public void Start(float forcedActiveDuration = 0f)
	{
		this.Active = true;
		if (forcedActiveDuration > 0f)
		{
			this.NextAllowedStopTime = Time.time + forcedActiveDuration;
		}
	}

	// Token: 0x0600AAEA RID: 43754 RVA: 0x003FC757 File Offset: 0x003FA957
	public void Stop(float forcedPauseDuration = 0f)
	{
		this.Active = false;
		if (forcedPauseDuration > 0f)
		{
			this.NextAllowedStartTime = Time.time + forcedPauseDuration;
		}
	}

	// Token: 0x0600AAEB RID: 43755 RVA: 0x003FC775 File Offset: 0x003FA975
	public void Disallow(float duration)
	{
		this.NextAllowedStartTime = Time.time + duration;
	}

	// Token: 0x04007EEE RID: 32494
	public vp_Activity.Callback StartCallbacks;

	// Token: 0x04007EEF RID: 32495
	public vp_Activity.Callback StopCallbacks;

	// Token: 0x04007EF0 RID: 32496
	public vp_Activity.Condition StartConditions;

	// Token: 0x04007EF1 RID: 32497
	public vp_Activity.Condition StopConditions;

	// Token: 0x04007EF2 RID: 32498
	public vp_Activity.Callback FailStartCallbacks;

	// Token: 0x04007EF3 RID: 32499
	public vp_Activity.Callback FailStopCallbacks;

	// Token: 0x04007EF4 RID: 32500
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Timer.Handle m_ForceStopTimer = new vp_Timer.Handle();

	// Token: 0x04007EF5 RID: 32501
	[PublicizedFrom(EAccessModifier.Protected)]
	public object m_Argument;

	// Token: 0x04007EF6 RID: 32502
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool m_Active;

	// Token: 0x04007EF7 RID: 32503
	public float NextAllowedStartTime;

	// Token: 0x04007EF8 RID: 32504
	public float NextAllowedStopTime;

	// Token: 0x04007EF9 RID: 32505
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_MinPause;

	// Token: 0x04007EFA RID: 32506
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_MinDuration;

	// Token: 0x04007EFB RID: 32507
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_MaxDuration = -1f;

	// Token: 0x0200153E RID: 5438
	// (Invoke) Token: 0x0600AAEE RID: 43758
	public delegate void Callback();

	// Token: 0x0200153F RID: 5439
	// (Invoke) Token: 0x0600AAF2 RID: 43762
	public delegate bool Condition();
}
