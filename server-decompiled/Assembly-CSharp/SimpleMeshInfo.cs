using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000954 RID: 2388
public class SimpleMeshInfo
{
	// Token: 0x060045ED RID: 17901 RVA: 0x001BB6FE File Offset: 0x001B98FE
	public SimpleMeshInfo(string[] _meshNames, Mesh[] _meshes, float _offsetY, Material _mat, List<ImposterCanvas> _signs)
	{
		this.meshNames = _meshNames;
		this.meshes = _meshes;
		this.offsetY = _offsetY;
		this.mat = _mat;
		this.signs = _signs;
	}

	// Token: 0x04003828 RID: 14376
	public string[] meshNames;

	// Token: 0x04003829 RID: 14377
	public Mesh[] meshes;

	// Token: 0x0400382A RID: 14378
	public readonly float offsetY;

	// Token: 0x0400382B RID: 14379
	public readonly Material mat;

	// Token: 0x0400382C RID: 14380
	public List<ImposterCanvas> signs;
}
