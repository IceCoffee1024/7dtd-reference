using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200155E RID: 5470
[Preserve]
public class vp_StateManager
{
	// Token: 0x0600AB81 RID: 43905 RVA: 0x00400AEC File Offset: 0x003FECEC
	public vp_StateManager(vp_Component component, List<vp_State> states)
	{
		this.m_States = states;
		this.m_Component = component;
		this.m_Component.RefreshDefaultState();
		this.m_StateIds = new Dictionary<string, int>(StringComparer.CurrentCulture);
		foreach (vp_State vp_State in this.m_States)
		{
			vp_State.StateManager = this;
			if (!this.m_StateIds.ContainsKey(vp_State.Name))
			{
				this.m_StateIds.Add(vp_State.Name, this.m_States.IndexOf(vp_State));
			}
			else
			{
				string[] array = new string[7];
				array[0] = "Warning: ";
				int num = 1;
				Type type = this.m_Component.GetType();
				array[num] = ((type != null) ? type.ToString() : null);
				array[2] = " on '";
				array[3] = this.m_Component.name;
				array[4] = "' has more than one state named: '";
				array[5] = vp_State.Name;
				array[6] = "'. Only the topmost one will be used.";
				Debug.LogWarning(string.Concat(array));
				this.m_States[this.m_DefaultId].StatesToBlock.Add(this.m_States.IndexOf(vp_State));
			}
			if (vp_State.Preset == null)
			{
				vp_State.Preset = new vp_ComponentPreset();
			}
			if (vp_State.TextAsset != null)
			{
				vp_State.Preset.LoadFromTextAsset(vp_State.TextAsset);
			}
		}
		this.m_DefaultId = this.m_States.Count - 1;
	}

	// Token: 0x0600AB82 RID: 43906 RVA: 0x00400C84 File Offset: 0x003FEE84
	public void ImposeBlockingList(vp_State blocker)
	{
		if (blocker == null)
		{
			return;
		}
		if (blocker.StatesToBlock == null)
		{
			return;
		}
		if (this.m_States == null)
		{
			return;
		}
		foreach (int num in blocker.StatesToBlock)
		{
			this.m_States[num].AddBlocker(blocker);
		}
	}

	// Token: 0x0600AB83 RID: 43907 RVA: 0x00400CF8 File Offset: 0x003FEEF8
	public void RelaxBlockingList(vp_State blocker)
	{
		if (blocker == null)
		{
			return;
		}
		if (blocker.StatesToBlock == null)
		{
			return;
		}
		if (this.m_States == null)
		{
			return;
		}
		foreach (int num in blocker.StatesToBlock)
		{
			this.m_States[num].RemoveBlocker(blocker);
		}
	}

	// Token: 0x0600AB84 RID: 43908 RVA: 0x00400D6C File Offset: 0x003FEF6C
	public void SetState(string state, bool setEnabled = true)
	{
		if (!vp_StateManager.AppPlaying())
		{
			return;
		}
		if (!this.m_StateIds.TryGetValue(state, out this.m_TargetId))
		{
			return;
		}
		if (this.m_TargetId == this.m_DefaultId && !setEnabled)
		{
			Debug.LogWarning(vp_StateManager.m_DefaultStateNoDisableMessage);
			return;
		}
		this.m_States[this.m_TargetId].Enabled = setEnabled;
		this.CombineStates();
		this.m_Component.Refresh();
	}

	// Token: 0x0600AB85 RID: 43909 RVA: 0x00400DDC File Offset: 0x003FEFDC
	public void Reset()
	{
		if (!vp_StateManager.AppPlaying())
		{
			return;
		}
		foreach (vp_State vp_State in this.m_States)
		{
			vp_State.Enabled = false;
		}
		this.m_States[this.m_DefaultId].Enabled = true;
		this.m_TargetId = this.m_DefaultId;
		this.CombineStates();
	}

	// Token: 0x0600AB86 RID: 43910 RVA: 0x00400E60 File Offset: 0x003FF060
	public void CombineStates()
	{
		for (int i = this.m_States.Count - 1; i > -1; i--)
		{
			if ((i == this.m_DefaultId || (this.m_States[i].Enabled && !this.m_States[i].Blocked && !(this.m_States[i].TextAsset == null))) && this.m_States[i].Preset != null && !(this.m_States[i].Preset.ComponentType == null))
			{
				vp_ComponentPreset.Apply(this.m_Component, this.m_States[i].Preset);
			}
		}
	}

	// Token: 0x0600AB87 RID: 43911 RVA: 0x00400F21 File Offset: 0x003FF121
	public bool IsEnabled(string state)
	{
		return vp_StateManager.AppPlaying() && this.m_StateIds.TryGetValue(state, out this.m_TargetId) && this.m_States[this.m_TargetId].Enabled;
	}

	// Token: 0x0600AB88 RID: 43912 RVA: 0x00020119 File Offset: 0x0001E319
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool AppPlaying()
	{
		return true;
	}

	// Token: 0x04007F6A RID: 32618
	[PublicizedFrom(EAccessModifier.Private)]
	public vp_Component m_Component;

	// Token: 0x04007F6B RID: 32619
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<vp_State> m_States;

	// Token: 0x04007F6C RID: 32620
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, int> m_StateIds;

	// Token: 0x04007F6D RID: 32621
	[PublicizedFrom(EAccessModifier.Private)]
	public static string m_AppNotPlayingMessage = "Error: StateManager can only be accessed while application is playing.";

	// Token: 0x04007F6E RID: 32622
	[PublicizedFrom(EAccessModifier.Private)]
	public static string m_DefaultStateNoDisableMessage = "Warning: The 'Default' state cannot be disabled.";

	// Token: 0x04007F6F RID: 32623
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_DefaultId;

	// Token: 0x04007F70 RID: 32624
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_TargetId;
}
