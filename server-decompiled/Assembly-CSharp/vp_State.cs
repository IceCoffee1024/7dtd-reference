using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200155D RID: 5469
[Preserve]
[Serializable]
public class vp_State
{
	// Token: 0x0600AB79 RID: 43897 RVA: 0x00400A1F File Offset: 0x003FEC1F
	public vp_State(string typeName, string name = "Untitled", string path = null, TextAsset asset = null)
	{
		this.TypeName = typeName;
		this.Name = name;
		this.TextAsset = asset;
	}

	// Token: 0x1700141D RID: 5149
	// (get) Token: 0x0600AB7A RID: 43898 RVA: 0x00400A3D File Offset: 0x003FEC3D
	// (set) Token: 0x0600AB7B RID: 43899 RVA: 0x00400A45 File Offset: 0x003FEC45
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

	// Token: 0x1700141E RID: 5150
	// (get) Token: 0x0600AB7C RID: 43900 RVA: 0x00400A78 File Offset: 0x003FEC78
	public bool Blocked
	{
		get
		{
			return this.CurrentlyBlockedBy.Count > 0;
		}
	}

	// Token: 0x1700141F RID: 5151
	// (get) Token: 0x0600AB7D RID: 43901 RVA: 0x00400A88 File Offset: 0x003FEC88
	public int BlockCount
	{
		get
		{
			return this.CurrentlyBlockedBy.Count;
		}
	}

	// Token: 0x17001420 RID: 5152
	// (get) Token: 0x0600AB7E RID: 43902 RVA: 0x00400A95 File Offset: 0x003FEC95
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

	// Token: 0x0600AB7F RID: 43903 RVA: 0x00400AB0 File Offset: 0x003FECB0
	public void AddBlocker(vp_State blocker)
	{
		if (!this.CurrentlyBlockedBy.Contains(blocker))
		{
			this.CurrentlyBlockedBy.Add(blocker);
		}
	}

	// Token: 0x0600AB80 RID: 43904 RVA: 0x00400ACC File Offset: 0x003FECCC
	public void RemoveBlocker(vp_State blocker)
	{
		if (this.CurrentlyBlockedBy.Contains(blocker))
		{
			this.CurrentlyBlockedBy.Remove(blocker);
		}
	}

	// Token: 0x04007F62 RID: 32610
	public vp_StateManager StateManager;

	// Token: 0x04007F63 RID: 32611
	public string TypeName;

	// Token: 0x04007F64 RID: 32612
	public string Name;

	// Token: 0x04007F65 RID: 32613
	public TextAsset TextAsset;

	// Token: 0x04007F66 RID: 32614
	public vp_ComponentPreset Preset;

	// Token: 0x04007F67 RID: 32615
	public List<int> StatesToBlock;

	// Token: 0x04007F68 RID: 32616
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Enabled;

	// Token: 0x04007F69 RID: 32617
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<vp_State> m_CurrentlyBlockedBy;
}
