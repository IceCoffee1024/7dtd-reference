using System;
using System.Collections.Generic;
using RaycastPathing;
using UnityEngine.Scripting;

// Token: 0x020008F1 RID: 2289
[Preserve]
public class RaycastPathManager
{
	// Token: 0x17000708 RID: 1800
	// (get) Token: 0x0600419D RID: 16797 RVA: 0x0019D7F3 File Offset: 0x0019B9F3
	public static RaycastPathManager Instance
	{
		get
		{
			return RaycastPathManager.instance;
		}
	}

	// Token: 0x0600419E RID: 16798 RVA: 0x0019D7FA File Offset: 0x0019B9FA
	public static void Init()
	{
		RaycastPathManager.instance = new RaycastPathManager();
		RaycastPathManager.instance._Init();
	}

	// Token: 0x0600419F RID: 16799 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void _Init()
	{
	}

	// Token: 0x060041A0 RID: 16800 RVA: 0x0019D810 File Offset: 0x0019BA10
	public void Add(RaycastPath path)
	{
		if (!this.paths.Contains(path))
		{
			this.paths.Add(path);
		}
	}

	// Token: 0x060041A1 RID: 16801 RVA: 0x0019D82C File Offset: 0x0019BA2C
	public void Remove(RaycastPath path)
	{
		if (this.paths.Contains(path))
		{
			this.paths.Remove(path);
		}
	}

	// Token: 0x060041A2 RID: 16802 RVA: 0x0019D849 File Offset: 0x0019BA49
	public void Update()
	{
		if (RaycastPathManager.DebugModeEnabled)
		{
			this._DebugDraw();
		}
	}

	// Token: 0x060041A3 RID: 16803 RVA: 0x0019D858 File Offset: 0x0019BA58
	[PublicizedFrom(EAccessModifier.Private)]
	public void _DebugDraw()
	{
		for (int i = 0; i < this.paths.Count; i++)
		{
			this.paths[i].DebugDraw();
		}
	}

	// Token: 0x060041A4 RID: 16804 RVA: 0x0019D88C File Offset: 0x0019BA8C
	public static implicit operator bool(RaycastPathManager exists)
	{
		return exists != null;
	}

	// Token: 0x04003597 RID: 13719
	public static bool DebugModeEnabled;

	// Token: 0x04003598 RID: 13720
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<RaycastPath> paths = new List<RaycastPath>();

	// Token: 0x04003599 RID: 13721
	[PublicizedFrom(EAccessModifier.Private)]
	public static RaycastPathManager instance;
}
