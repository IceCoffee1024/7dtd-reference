using System;
using System.Collections.Generic;
using RaycastPathing;
using UnityEngine.Scripting;

// Token: 0x020008D6 RID: 2262
[Preserve]
public class RaycastPathManager
{
	// Token: 0x170006D7 RID: 1751
	// (get) Token: 0x0600417F RID: 16767 RVA: 0x0019A55F File Offset: 0x0019875F
	public static RaycastPathManager Instance
	{
		get
		{
			return RaycastPathManager.instance;
		}
	}

	// Token: 0x06004180 RID: 16768 RVA: 0x0019A566 File Offset: 0x00198766
	public static void Init()
	{
		RaycastPathManager.instance = new RaycastPathManager();
		RaycastPathManager.instance._Init();
	}

	// Token: 0x06004181 RID: 16769 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Private)]
	public void _Init()
	{
	}

	// Token: 0x06004182 RID: 16770 RVA: 0x0019A57C File Offset: 0x0019877C
	public void Add(RaycastPath path)
	{
		if (!this.paths.Contains(path))
		{
			this.paths.Add(path);
		}
	}

	// Token: 0x06004183 RID: 16771 RVA: 0x0019A598 File Offset: 0x00198798
	public void Remove(RaycastPath path)
	{
		if (this.paths.Contains(path))
		{
			this.paths.Remove(path);
		}
	}

	// Token: 0x06004184 RID: 16772 RVA: 0x0019A5B5 File Offset: 0x001987B5
	public void Update()
	{
		if (RaycastPathManager.DebugModeEnabled)
		{
			this._DebugDraw();
		}
	}

	// Token: 0x06004185 RID: 16773 RVA: 0x0019A5C4 File Offset: 0x001987C4
	[PublicizedFrom(EAccessModifier.Private)]
	public void _DebugDraw()
	{
		for (int i = 0; i < this.paths.Count; i++)
		{
			this.paths[i].DebugDraw();
		}
	}

	// Token: 0x06004186 RID: 16774 RVA: 0x0019A5F8 File Offset: 0x001987F8
	public static implicit operator bool(RaycastPathManager exists)
	{
		return exists != null;
	}

	// Token: 0x04003502 RID: 13570
	public static bool DebugModeEnabled;

	// Token: 0x04003503 RID: 13571
	[PublicizedFrom(EAccessModifier.Protected)]
	public List<RaycastPath> paths = new List<RaycastPath>();

	// Token: 0x04003504 RID: 13572
	[PublicizedFrom(EAccessModifier.Private)]
	public static RaycastPathManager instance;
}
