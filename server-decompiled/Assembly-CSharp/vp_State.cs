using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200156F RID: 5487
[Preserve]
[Serializable]
public class vp_State
{
	// Token: 0x0600ABD1 RID: 43985 RVA: 0x00402E23 File Offset: 0x00401023
	public vp_State(string typeName, string name = "Untitled", string path = null, TextAsset asset = null)
	{
		this.TypeName = typeName;
		this.Name = name;
		this.TextAsset = asset;
	}

	// Token: 0x17001429 RID: 5161
	// (get) Token: 0x0600ABD2 RID: 43986 RVA: 0x00402E41 File Offset: 0x00401041
	// (set) Token: 0x0600ABD3 RID: 43987 RVA: 0x00402E49 File Offset: 0x00401049
	public bool Enabled
	{
		get
		{
			return this.m_Enabled;
		}
		set
		{
			this.m_Enabled = value;
			if (this.StateManager == null)
			{
				return;
			}
			if (this.m_Enabled)
			{
				this.StateManager.ImposeBlockingList(this);
				return;
			}
			this.StateManager.RelaxBlockingList(this);
		}
	}

	// Token: 0x1700142A RID: 5162
	// (get) Token: 0x0600ABD4 RID: 43988 RVA: 0x00402E7C File Offset: 0x0040107C
	public bool Blocked
	{
		get
		{
			return this.CurrentlyBlockedBy.Count > 0;
		}
	}

	// Token: 0x1700142B RID: 5163
	// (get) Token: 0x0600ABD5 RID: 43989 RVA: 0x00402E8C File Offset: 0x0040108C
	public int BlockCount
	{
		get
		{
			return this.CurrentlyBlockedBy.Count;
		}
	}

	// Token: 0x1700142C RID: 5164
	// (get) Token: 0x0600ABD6 RID: 43990 RVA: 0x00402E99 File Offset: 0x00401099
	public List<vp_State> CurrentlyBlockedBy
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			if (this.m_CurrentlyBlockedBy == null)
			{
				this.m_CurrentlyBlockedBy = new List<vp_State>();
			}
			return this.m_CurrentlyBlockedBy;
		}
	}

	// Token: 0x0600ABD7 RID: 43991 RVA: 0x00402EB4 File Offset: 0x004010B4
	public void AddBlocker(vp_State blocker)
	{
		if (!this.CurrentlyBlockedBy.Contains(blocker))
		{
			this.CurrentlyBlockedBy.Add(blocker);
		}
	}

	// Token: 0x0600ABD8 RID: 43992 RVA: 0x00402ED0 File Offset: 0x004010D0
	public void RemoveBlocker(vp_State blocker)
	{
		if (this.CurrentlyBlockedBy.Contains(blocker))
		{
			this.CurrentlyBlockedBy.Remove(blocker);
		}
	}

	// Token: 0x04007FA3 RID: 32675
	public vp_StateManager StateManager;

	// Token: 0x04007FA4 RID: 32676
	public string TypeName;

	// Token: 0x04007FA5 RID: 32677
	public string Name;

	// Token: 0x04007FA6 RID: 32678
	public TextAsset TextAsset;

	// Token: 0x04007FA7 RID: 32679
	public vp_ComponentPreset Preset;

	// Token: 0x04007FA8 RID: 32680
	public List<int> StatesToBlock;

	// Token: 0x04007FA9 RID: 32681
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Enabled;

	// Token: 0x04007FAA RID: 32682
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<vp_State> m_CurrentlyBlockedBy;
}
