using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000975 RID: 2421
public class SimpleMeshInfo
{
	// Token: 0x06004637 RID: 17975 RVA: 0x001BF47E File Offset: 0x001BD67E
	public SimpleMeshInfo(string[] _meshNames, Mesh[] _meshes, float _offsetY, Material _mat, List<ImposterCanvas> _signs)
	{
		this.meshNames = _meshNames;
		this.meshes = _meshes;
		this.offsetY = _offsetY;
		this.mat = _mat;
		this.signs = _signs;
	}

	// Token: 0x040038D5 RID: 14549
	public string[] meshNames;

	// Token: 0x040038D6 RID: 14550
	public Mesh[] meshes;

	// Token: 0x040038D7 RID: 14551
	public readonly float offsetY;

	// Token: 0x040038D8 RID: 14552
	public readonly Material mat;

	// Token: 0x040038D9 RID: 14553
	public List<ImposterCanvas> signs;
}
