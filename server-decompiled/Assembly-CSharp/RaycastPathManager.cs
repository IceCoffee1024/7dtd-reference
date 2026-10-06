using System;
using System.Collections.Generic;
using RaycastPathing;
using UnityEngine.Scripting;

// Token: 0x020008E7 RID: 2279
[Preserve]
public class RaycastPathManager
{
	// Token: 0x17000702 RID: 1794
	// (get) Token: 0x0600417A RID: 16762 RVA: 0x0019C74B File Offset: 0x0019A94B
	public static RaycastPathManager Instance
	{
		get
		{
			return RaycastPathManager.instance;
		}
	}

	// Token: 0x0600417B RID: 16763 RVA: 0x0019C752 File Offset: 0x0019A952
	public static void Init()
	{
		RaycastPathManager.instance = new RaycastPathManager();
		RaycastPathManager.instance._Init();
	}

	// Token: 0x0600417C RID: 16764 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void _Init()
	{
	}

	// Token: 0x0600417D RID: 16765 RVA: 0x0019C768 File Offset: 0x0019A968
	public void Add(RaycastPath path)
	{
		if (!this.paths.Contains(path))
		{
			this.paths.Add(path);
		}
	}

	// Token: 0x0600417E RID: 16766 RVA: 0x0019C784 File Offset: 0x0019A984
	public void Remove(RaycastPath path)
	{
		if (this.paths.Contains(path))
		{
			this.paths.Remove(path);
		}
	}

	// Token: 0x0600417F RID: 16767 RVA: 0x0019C7A1 File Offset: 0x0019A9A1
	public void Update()
	{
		if (RaycastPathManager.DebugModeEnabled)
		{
			this._DebugDraw();
		}
	}

	// Token: 0x06004180 RID: 16768 RVA: 0x0019C7B0 File Offset: 0x0019A9B0
	[PublicizedFrom(EAccessModifier.Private)]
	public void _DebugDraw()
	{
		for (int i = 0; i < this.paths.Count; i++)
		{
			this.paths[i].DebugDraw();
		}
	}

	// Token: 0x06004181 RID: 16769 RVA: 0x0019C7E4 File Offset: 0x0019A9E4
	public static implicit operator bool(RaycastPathManager exists)
	{
		return exists != null;
	}

	// Token: 0x04003577 RID: 13687
	public static bool DebugModeEnabled;

	// Token: 0x04003578 RID: 13688
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<RaycastPath> paths = new List<RaycastPath>();

	// Token: 0x04003579 RID: 13689
	[PublicizedFrom(EAccessModifier.Private)]
	public static RaycastPathManager instance;
}
