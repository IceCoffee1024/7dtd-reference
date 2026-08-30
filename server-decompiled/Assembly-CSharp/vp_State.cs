using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02001520 RID: 5408
[Preserve]
[Serializable]
public class vp_State
{
	// Token: 0x0600A9C9 RID: 43465 RVA: 0x003F688B File Offset: 0x003F4A8B
	public vp_State(string typeName, string name = "Untitled", string path = null, TextAsset asset = null)
	{
		this.TypeName = typeName;
		this.Name = name;
		this.TextAsset = asset;
	}

	// Token: 0x1700139F RID: 5023
	// (get) Token: 0x0600A9CA RID: 43466 RVA: 0x003F68A9 File Offset: 0x003F4AA9
	// (set) Token: 0x0600A9CB RID: 43467 RVA: 0x003F68B1 File Offset: 0x003F4AB1
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

	// Token: 0x170013A0 RID: 5024
	// (get) Token: 0x0600A9CC RID: 43468 RVA: 0x003F68E4 File Offset: 0x003F4AE4
	public bool Blocked
	{
		get
		{
			return this.CurrentlyBlockedBy.Count > 0;
		}
	}

	// Token: 0x170013A1 RID: 5025
	// (get) Token: 0x0600A9CD RID: 43469 RVA: 0x003F68F4 File Offset: 0x003F4AF4
	public int BlockCount
	{
		get
		{
			return this.CurrentlyBlockedBy.Count;
		}
	}

	// Token: 0x170013A2 RID: 5026
	// (get) Token: 0x0600A9CE RID: 43470 RVA: 0x003F6901 File Offset: 0x003F4B01
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

	// Token: 0x0600A9CF RID: 43471 RVA: 0x003F691C File Offset: 0x003F4B1C
	public void AddBlocker(vp_State blocker)
	{
		if (!this.CurrentlyBlockedBy.Contains(blocker))
		{
			this.CurrentlyBlockedBy.Add(blocker);
		}
	}

	// Token: 0x0600A9D0 RID: 43472 RVA: 0x003F6938 File Offset: 0x003F4B38
	public void RemoveBlocker(vp_State blocker)
	{
		if (this.CurrentlyBlockedBy.Contains(blocker))
		{
			this.CurrentlyBlockedBy.Remove(blocker);
		}
	}

	// Token: 0x04007E06 RID: 32262
	public vp_StateManager StateManager;

	// Token: 0x04007E07 RID: 32263
	public string TypeName;

	// Token: 0x04007E08 RID: 32264
	public string Name;

	// Token: 0x04007E09 RID: 32265
	public TextAsset TextAsset;

	// Token: 0x04007E0A RID: 32266
	public vp_ComponentPreset Preset;

	// Token: 0x04007E0B RID: 32267
	public List<int> StatesToBlock;

	// Token: 0x04007E0C RID: 32268
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool m_Enabled;

	// Token: 0x04007E0D RID: 32269
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<vp_State> m_CurrentlyBlockedBy;
}
