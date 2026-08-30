using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001522 RID: 5410
[Preserve]
public class vp_Activity : vp_Event
{
	// Token: 0x0600A9DA RID: 43482 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void Empty()
	{
	}

	// Token: 0x0600A9DB RID: 43483 RVA: 0x0001FF5D File Offset: 0x0001E15D
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool AlwaysOK()
	{
		return true;
	}

	// Token: 0x0600A9DC RID: 43484 RVA: 0x003F6DDA File Offset: 0x003F4FDA
	public vp_Activity(string name)
		: base(name)
	{
		this.InitFields();
	}

	// Token: 0x170013A3 RID: 5027
	// (get) Token: 0x0600A9DD RID: 43485 RVA: 0x003F6DFF File Offset: 0x003F4FFF
	// (set) Token: 0x0600A9DE RID: 43486 RVA: 0x003F6E07 File Offset: 0x003F5007
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

	// Token: 0x170013A4 RID: 5028
	// (get) Token: 0x0600A9DF RID: 43487 RVA: 0x003F6E1A File Offset: 0x003F501A
	// (set) Token: 0x0600A9E0 RID: 43488 RVA: 0x003F6E24 File Offset: 0x003F5024
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

	// Token: 0x170013A5 RID: 5029
	// (get) Token: 0x0600A9E1 RID: 43489 RVA: 0x003F6E84 File Offset: 0x003F5084
	// (set) Token: 0x0600A9E2 RID: 43490 RVA: 0x003F6E8C File Offset: 0x003F508C
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

	// Token: 0x170013A6 RID: 5030
	// (get) Token: 0x0600A9E3 RID: 43491 RVA: 0x003F6EF0 File Offset: 0x003F50F0
	// (set) Token: 0x0600A9E4 RID: 43492 RVA: 0x003F6F54 File Offset: 0x003F5154
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

	// Token: 0x0600A9E5 RID: 43493 RVA: 0x003F6FB8 File Offset: 0x003F51B8
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

	// Token: 0x0600A9E6 RID: 43494 RVA: 0x003F71E0 File Offset: 0x003F53E0
	public override void Register(object t, string m, int v)
	{
		base.AddExternalMethodToField(t, this.m_Fields[v], m, this.m_DelegateTypes[v]);
		base.Refresh();
	}

	// Token: 0x0600A9E7 RID: 43495 RVA: 0x003F7200 File Offset: 0x003F5400
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

	// Token: 0x0600A9E8 RID: 43496 RVA: 0x003F7270 File Offset: 0x003F5470
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

	// Token: 0x0600A9E9 RID: 43497 RVA: 0x003F72E8 File Offset: 0x003F54E8
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

	// Token: 0x170013A7 RID: 5031
	// (get) Token: 0x0600A9EB RID: 43499 RVA: 0x003F73F1 File Offset: 0x003F55F1
	// (set) Token: 0x0600A9EA RID: 43498 RVA: 0x003F7354 File Offset: 0x003F5554
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

	// Token: 0x0600A9EC RID: 43500 RVA: 0x003F73F9 File Offset: 0x003F55F9
	public void Start(float forcedActiveDuration = 0f)
	{
		this.Active = true;
		if (forcedActiveDuration > 0f)
		{
			this.NextAllowedStopTime = Time.time + forcedActiveDuration;
		}
	}

	// Token: 0x0600A9ED RID: 43501 RVA: 0x003F7417 File Offset: 0x003F5617
	public void Stop(float forcedPauseDuration = 0f)
	{
		this.Active = false;
		if (forcedPauseDuration > 0f)
		{
			this.NextAllowedStartTime = Time.time + forcedPauseDuration;
		}
	}

	// Token: 0x0600A9EE RID: 43502 RVA: 0x003F7435 File Offset: 0x003F5635
	public void Disallow(float duration)
	{
		this.NextAllowedStartTime = Time.time + duration;
	}

	// Token: 0x04007E15 RID: 32277
	public vp_Activity.Callback StartCallbacks;

	// Token: 0x04007E16 RID: 32278
	public vp_Activity.Callback StopCallbacks;

	// Token: 0x04007E17 RID: 32279
	public vp_Activity.Condition StartConditions;

	// Token: 0x04007E18 RID: 32280
	public vp_Activity.Condition StopConditions;

	// Token: 0x04007E19 RID: 32281
	public vp_Activity.Callback FailStartCallbacks;

	// Token: 0x04007E1A RID: 32282
	public vp_Activity.Callback FailStopCallbacks;

	// Token: 0x04007E1B RID: 32283
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Timer.Handle m_ForceStopTimer = new vp_Timer.Handle();

	// Token: 0x04007E1C RID: 32284
	[PublicizedFrom(EAccessModifier.Protected)]
	public object m_Argument;

	// Token: 0x04007E1D RID: 32285
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool m_Active;

	// Token: 0x04007E1E RID: 32286
	public float NextAllowedStartTime;

	// Token: 0x04007E1F RID: 32287
	public float NextAllowedStopTime;

	// Token: 0x04007E20 RID: 32288
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_MinPause;

	// Token: 0x04007E21 RID: 32289
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_MinDuration;

	// Token: 0x04007E22 RID: 32290
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_MaxDuration = -1f;

	// Token: 0x02001523 RID: 5411
	// (Invoke) Token: 0x0600A9F1 RID: 43505
	public delegate void Callback();

	// Token: 0x02001524 RID: 5412
	// (Invoke) Token: 0x0600A9F5 RID: 43509
	public delegate bool Condition();
}
