using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001571 RID: 5489
[Preserve]
public class vp_Activity : vp_Event
{
	// Token: 0x0600ABE2 RID: 44002 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void Empty()
	{
	}

	// Token: 0x0600ABE3 RID: 44003 RVA: 0x00020119 File Offset: 0x0001E319
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool AlwaysOK()
	{
		return true;
	}

	// Token: 0x0600ABE4 RID: 44004 RVA: 0x00403372 File Offset: 0x00401572
	public vp_Activity(string name)
		: base(name)
	{
		this.InitFields();
	}

	// Token: 0x1700142D RID: 5165
	// (get) Token: 0x0600ABE5 RID: 44005 RVA: 0x00403397 File Offset: 0x00401597
	// (set) Token: 0x0600ABE6 RID: 44006 RVA: 0x0040339F File Offset: 0x0040159F
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

	// Token: 0x1700142E RID: 5166
	// (get) Token: 0x0600ABE7 RID: 44007 RVA: 0x004033B2 File Offset: 0x004015B2
	// (set) Token: 0x0600ABE8 RID: 44008 RVA: 0x004033BC File Offset: 0x004015BC
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

	// Token: 0x1700142F RID: 5167
	// (get) Token: 0x0600ABE9 RID: 44009 RVA: 0x0040341C File Offset: 0x0040161C
	// (set) Token: 0x0600ABEA RID: 44010 RVA: 0x00403424 File Offset: 0x00401624
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

	// Token: 0x17001430 RID: 5168
	// (get) Token: 0x0600ABEB RID: 44011 RVA: 0x00403488 File Offset: 0x00401688
	// (set) Token: 0x0600ABEC RID: 44012 RVA: 0x004034EC File Offset: 0x004016EC
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

	// Token: 0x0600ABED RID: 44013 RVA: 0x00403550 File Offset: 0x00401750
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

	// Token: 0x0600ABEE RID: 44014 RVA: 0x00403778 File Offset: 0x00401978
	public override void Register(object t, string m, int v)
	{
		base.AddExternalMethodToField(t, this.m_Fields[v], m, this.m_DelegateTypes[v]);
		base.Refresh();
	}

	// Token: 0x0600ABEF RID: 44015 RVA: 0x00403798 File Offset: 0x00401998
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

	// Token: 0x0600ABF0 RID: 44016 RVA: 0x00403808 File Offset: 0x00401A08
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

	// Token: 0x0600ABF1 RID: 44017 RVA: 0x00403880 File Offset: 0x00401A80
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

	// Token: 0x17001431 RID: 5169
	// (get) Token: 0x0600ABF3 RID: 44019 RVA: 0x00403989 File Offset: 0x00401B89
	// (set) Token: 0x0600ABF2 RID: 44018 RVA: 0x004038EC File Offset: 0x00401AEC
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

	// Token: 0x0600ABF4 RID: 44020 RVA: 0x00403991 File Offset: 0x00401B91
	public void Start(float forcedActiveDuration = 0f)
	{
		this.Active = true;
		if (forcedActiveDuration > 0f)
		{
			this.NextAllowedStopTime = Time.time + forcedActiveDuration;
		}
	}

	// Token: 0x0600ABF5 RID: 44021 RVA: 0x004039AF File Offset: 0x00401BAF
	public void Stop(float forcedPauseDuration = 0f)
	{
		this.Active = false;
		if (forcedPauseDuration > 0f)
		{
			this.NextAllowedStartTime = Time.time + forcedPauseDuration;
		}
	}

	// Token: 0x0600ABF6 RID: 44022 RVA: 0x004039CD File Offset: 0x00401BCD
	public void Disallow(float duration)
	{
		this.NextAllowedStartTime = Time.time + duration;
	}

	// Token: 0x04007FB2 RID: 32690
	public vp_Activity.Callback StartCallbacks;

	// Token: 0x04007FB3 RID: 32691
	public vp_Activity.Callback StopCallbacks;

	// Token: 0x04007FB4 RID: 32692
	public vp_Activity.Condition StartConditions;

	// Token: 0x04007FB5 RID: 32693
	public vp_Activity.Condition StopConditions;

	// Token: 0x04007FB6 RID: 32694
	public vp_Activity.Callback FailStartCallbacks;

	// Token: 0x04007FB7 RID: 32695
	public vp_Activity.Callback FailStopCallbacks;

	// Token: 0x04007FB8 RID: 32696
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Timer.Handle m_ForceStopTimer = new vp_Timer.Handle();

	// Token: 0x04007FB9 RID: 32697
	[PublicizedFrom(EAccessModifier.Protected)]
	public object m_Argument;

	// Token: 0x04007FBA RID: 32698
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool m_Active;

	// Token: 0x04007FBB RID: 32699
	public float NextAllowedStartTime;

	// Token: 0x04007FBC RID: 32700
	public float NextAllowedStopTime;

	// Token: 0x04007FBD RID: 32701
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_MinPause;

	// Token: 0x04007FBE RID: 32702
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_MinDuration;

	// Token: 0x04007FBF RID: 32703
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_MaxDuration = -1f;

	// Token: 0x02001572 RID: 5490
	// (Invoke) Token: 0x0600ABF9 RID: 44025
	public delegate void Callback();

	// Token: 0x02001573 RID: 5491
	// (Invoke) Token: 0x0600ABFD RID: 44029
	public delegate bool Condition();
}
