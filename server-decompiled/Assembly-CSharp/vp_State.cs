using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200153B RID: 5435
[Preserve]
[Serializable]
public class vp_State
{
	// Token: 0x0600AAC6 RID: 43718 RVA: 0x003FBBCB File Offset: 0x003F9DCB
	public vp_State(string typeName, string name = "Untitled", string path = null, TextAsset asset = null)
	{
		this.TypeName = typeName;
		this.Name = name;
		this.TextAsset = asset;
	}

	// Token: 0x170013D8 RID: 5080
	// (get) Token: 0x0600AAC7 RID: 43719 RVA: 0x003FBBE9 File Offset: 0x003F9DE9
	// (set) Token: 0x0600AAC8 RID: 43720 RVA: 0x003FBBF1 File Offset: 0x003F9DF1
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

	// Token: 0x170013D9 RID: 5081
	// (get) Token: 0x0600AAC9 RID: 43721 RVA: 0x003FBC24 File Offset: 0x003F9E24
	public bool Blocked
	{
		get
		{
			return this.CurrentlyBlockedBy.Count > 0;
		}
	}

	// Token: 0x170013DA RID: 5082
	// (get) Token: 0x0600AACA RID: 43722 RVA: 0x003FBC34 File Offset: 0x003F9E34
	public int BlockCount
	{
		get
		{
			return this.CurrentlyBlockedBy.Count;
		}
	}

	// Token: 0x170013DB RID: 5083
	// (get) Token: 0x0600AACB RID: 43723 RVA: 0x003FBC41 File Offset: 0x003F9E41
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

	// Token: 0x0600AACC RID: 43724 RVA: 0x003FBC5C File Offset: 0x003F9E5C
	public void AddBlocker(vp_State blocker)
	{
		if (!this.CurrentlyBlockedBy.Contains(blocker))
		{
			this.CurrentlyBlockedBy.Add(blocker);
		}
	}

	// Token: 0x0600AACD RID: 43725 RVA: 0x003FBC78 File Offset: 0x003F9E78
	public void RemoveBlocker(vp_State blocker)
	{
		if (this.CurrentlyBlockedBy.Contains(blocker))
		{
			this.CurrentlyBlockedBy.Remove(blocker);
		}
	}

	// Token: 0x04007EDF RID: 32479
	public vp_StateManager StateManager;

	// Token: 0x04007EE0 RID: 32480
	public string TypeName;

	// Token: 0x04007EE1 RID: 32481
	public string Name;

	// Token: 0x04007EE2 RID: 32482
	public TextAsset TextAsset;

	// Token: 0x04007EE3 RID: 32483
	public vp_ComponentPreset Preset;

	// Token: 0x04007EE4 RID: 32484
	public List<int> StatesToBlock;

	// Token: 0x04007EE5 RID: 32485
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Enabled;

	// Token: 0x04007EE6 RID: 32486
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<vp_State> m_CurrentlyBlockedBy;
}
