using System;
using System.Collections.Generic;
using RaycastPathing;
using UnityEngine.Scripting;

// Token: 0x020008E2 RID: 2274
[Preserve]
public class RaycastPathManager
{
	// Token: 0x170006E3 RID: 1763
	// (get) Token: 0x060041E4 RID: 16868 RVA: 0x0019C637 File Offset: 0x0019A837
	public static RaycastPathManager Instance
	{
		get
		{
			return RaycastPathManager.instance;
		}
	}

	// Token: 0x060041E5 RID: 16869 RVA: 0x0019C63E File Offset: 0x0019A83E
	public static void Init()
	{
		RaycastPathManager.instance = new RaycastPathManager();
		RaycastPathManager.instance._Init();
	}

	// Token: 0x060041E6 RID: 16870 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void _Init()
	{
	}

	// Token: 0x060041E7 RID: 16871 RVA: 0x0019C654 File Offset: 0x0019A854
	public void Add(RaycastPath path)
	{
		if (!this.paths.Contains(path))
		{
			this.paths.Add(path);
		}
	}

	// Token: 0x060041E8 RID: 16872 RVA: 0x0019C670 File Offset: 0x0019A870
	public void Remove(RaycastPath path)
	{
		if (this.paths.Contains(path))
		{
			this.paths.Remove(path);
		}
	}

	// Token: 0x060041E9 RID: 16873 RVA: 0x0019C68D File Offset: 0x0019A88D
	public void Update()
	{
		if (RaycastPathManager.DebugModeEnabled)
		{
			this._DebugDraw();
		}
	}

	// Token: 0x060041EA RID: 16874 RVA: 0x0019C69C File Offset: 0x0019A89C
	[PublicizedFrom(EAccessModifier.Private)]
	public void _DebugDraw()
	{
		for (int i = 0; i < this.paths.Count; i++)
		{
			this.paths[i].DebugDraw();
		}
	}

	// Token: 0x060041EB RID: 16875 RVA: 0x0019C6D0 File Offset: 0x0019A8D0
	public static implicit operator bool(RaycastPathManager exists)
	{
		return exists != null;
	}

	// Token: 0x04003583 RID: 13699
	public static bool DebugModeEnabled;

	// Token: 0x04003584 RID: 13700
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<RaycastPath> paths = new List<RaycastPath>();

	// Token: 0x04003585 RID: 13701
	[PublicizedFrom(EAccessModifier.Private)]
	public static RaycastPathManager instance;
}
