using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200096B RID: 2411
public class SimpleMeshInfo
{
	// Token: 0x06004614 RID: 17940 RVA: 0x001BE3FA File Offset: 0x001BC5FA
	public SimpleMeshInfo(string[] _meshNames, Mesh[] _meshes, float _offsetY, Material _mat, List<ImposterCanvas> _signs)
	{
		this.meshNames = _meshNames;
		this.meshes = _meshes;
		this.offsetY = _offsetY;
		this.mat = _mat;
		this.signs = _signs;
	}

	// Token: 0x040038B5 RID: 14517
	public string[] meshNames;

	// Token: 0x040038B6 RID: 14518
	public Mesh[] meshes;

	// Token: 0x040038B7 RID: 14519
	public readonly float offsetY;

	// Token: 0x040038B8 RID: 14520
	public readonly Material mat;

	// Token: 0x040038B9 RID: 14521
	public List<ImposterCanvas> signs;
}
