using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200155F RID: 5471
[Preserve]
public class vp_Activity : vp_Event
{
	// Token: 0x0600AB8A RID: 43914 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void Empty()
	{
	}

	// Token: 0x0600AB8B RID: 43915 RVA: 0x00020119 File Offset: 0x0001E319
	[PublicizedFrom(EAccessModifier.Protected)]
	public static bool AlwaysOK()
	{
		return true;
	}

	// Token: 0x0600AB8C RID: 43916 RVA: 0x00400F6E File Offset: 0x003FF16E
	public vp_Activity(string name)
		: base(name)
	{
		this.InitFields();
	}

	// Token: 0x17001421 RID: 5153
	// (get) Token: 0x0600AB8D RID: 43917 RVA: 0x00400F93 File Offset: 0x003FF193
	// (set) Token: 0x0600AB8E RID: 43918 RVA: 0x00400F9B File Offset: 0x003FF19B
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

	// Token: 0x17001422 RID: 5154
	// (get) Token: 0x0600AB8F RID: 43919 RVA: 0x00400FAE File Offset: 0x003FF1AE
	// (set) Token: 0x0600AB90 RID: 43920 RVA: 0x00400FB8 File Offset: 0x003FF1B8
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

	// Token: 0x17001423 RID: 5155
	// (get) Token: 0x0600AB91 RID: 43921 RVA: 0x00401018 File Offset: 0x003FF218
	// (set) Token: 0x0600AB92 RID: 43922 RVA: 0x00401020 File Offset: 0x003FF220
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

	// Token: 0x17001424 RID: 5156
	// (get) Token: 0x0600AB93 RID: 43923 RVA: 0x00401084 File Offset: 0x003FF284
	// (set) Token: 0x0600AB94 RID: 43924 RVA: 0x004010E8 File Offset: 0x003FF2E8
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

	// Token: 0x0600AB95 RID: 43925 RVA: 0x0040114C File Offset: 0x003FF34C
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

	// Token: 0x0600AB96 RID: 43926 RVA: 0x00401374 File Offset: 0x003FF574
	public override void Register(object t, string m, int v)
	{
		base.AddExternalMethodToField(t, this.m_Fields[v], m, this.m_DelegateTypes[v]);
		base.Refresh();
	}

	// Token: 0x0600AB97 RID: 43927 RVA: 0x00401394 File Offset: 0x003FF594
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

	// Token: 0x0600AB98 RID: 43928 RVA: 0x00401404 File Offset: 0x003FF604
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

	// Token: 0x0600AB99 RID: 43929 RVA: 0x0040147C File Offset: 0x003FF67C
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

	// Token: 0x17001425 RID: 5157
	// (get) Token: 0x0600AB9B RID: 43931 RVA: 0x00401585 File Offset: 0x003FF785
	// (set) Token: 0x0600AB9A RID: 43930 RVA: 0x004014E8 File Offset: 0x003FF6E8
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

	// Token: 0x0600AB9C RID: 43932 RVA: 0x0040158D File Offset: 0x003FF78D
	public void Start(float forcedActiveDuration = 0f)
	{
		this.Active = true;
		if (forcedActiveDuration > 0f)
		{
			this.NextAllowedStopTime = Time.time + forcedActiveDuration;
		}
	}

	// Token: 0x0600AB9D RID: 43933 RVA: 0x004015AB File Offset: 0x003FF7AB
	public void Stop(float forcedPauseDuration = 0f)
	{
		this.Active = false;
		if (forcedPauseDuration > 0f)
		{
			this.NextAllowedStartTime = Time.time + forcedPauseDuration;
		}
	}

	// Token: 0x0600AB9E RID: 43934 RVA: 0x004015C9 File Offset: 0x003FF7C9
	public void Disallow(float duration)
	{
		this.NextAllowedStartTime = Time.time + duration;
	}

	// Token: 0x04007F71 RID: 32625
	public vp_Activity.Callback StartCallbacks;

	// Token: 0x04007F72 RID: 32626
	public vp_Activity.Callback StopCallbacks;

	// Token: 0x04007F73 RID: 32627
	public vp_Activity.Condition StartConditions;

	// Token: 0x04007F74 RID: 32628
	public vp_Activity.Condition StopConditions;

	// Token: 0x04007F75 RID: 32629
	public vp_Activity.Callback FailStartCallbacks;

	// Token: 0x04007F76 RID: 32630
	public vp_Activity.Callback FailStopCallbacks;

	// Token: 0x04007F77 RID: 32631
	[PublicizedFrom(EAccessModifier.Protected)]
	public vp_Timer.Handle m_ForceStopTimer = new vp_Timer.Handle();

	// Token: 0x04007F78 RID: 32632
	[PublicizedFrom(EAccessModifier.Protected)]
	public object m_Argument;

	// Token: 0x04007F79 RID: 32633
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool m_Active;

	// Token: 0x04007F7A RID: 32634
	public float NextAllowedStartTime;

	// Token: 0x04007F7B RID: 32635
	public float NextAllowedStopTime;

	// Token: 0x04007F7C RID: 32636
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_MinPause;

	// Token: 0x04007F7D RID: 32637
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_MinDuration;

	// Token: 0x04007F7E RID: 32638
	[PublicizedFrom(EAccessModifier.Private)]
	public float m_MaxDuration = -1f;

	// Token: 0x02001560 RID: 5472
	// (Invoke) Token: 0x0600ABA1 RID: 43937
	public delegate void Callback();

	// Token: 0x02001561 RID: 5473
	// (Invoke) Token: 0x0600ABA5 RID: 43941
	public delegate bool Condition();
}
