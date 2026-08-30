using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000965 RID: 2405
public class SimpleMeshInfo
{
	// Token: 0x06004673 RID: 18035 RVA: 0x001BDEF2 File Offset: 0x001BC0F2
	public SimpleMeshInfo(string[] _meshNames, Mesh[] _meshes, float _offsetY, Material _mat, List<ImposterCanvas> _signs)
	{
		this.meshNames = _meshNames;
		this.meshes = _meshes;
		this.offsetY = _offsetY;
		this.mat = _mat;
		this.signs = _signs;
	}

	// Token: 0x040038B9 RID: 14521
	public string[] meshNames;

	// Token: 0x040038BA RID: 14522
	public Mesh[] meshes;

	// Token: 0x040038BB RID: 14523
	public readonly float offsetY;

	// Token: 0x040038BC RID: 14524
	public readonly Material mat;

	// Token: 0x040038BD RID: 14525
	public List<ImposterCanvas> signs;
}
