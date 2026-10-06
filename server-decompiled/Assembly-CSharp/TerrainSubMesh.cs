using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BD1 RID: 3025
public struct TerrainSubMesh
{
	// Token: 0x06005AFB RID: 23291 RVA: 0x002327BB File Offset: 0x002309BB
	public TerrainSubMesh(List<TerrainSubMesh> _others, int _minSize = 0)
	{
		this.others = _others;
		this.textureIds = new ArrayDynamicFast<int>(TerrainSubMesh.vertexColors.Length);
		this.triangles = new ArrayListMP<int>(MemoryPools.poolInt, _minSize);
		this.needToAdd = new ArrayListMP<int>(MemoryPools.poolInt, 0);
	}

	// Token: 0x06005AFC RID: 23292 RVA: 0x002327F8 File Offset: 0x002309F8
	public bool Contains(IList<int> _texIds)
	{
		for (int i = 0; i < _texIds.Count; i++)
		{
			if (_texIds[i] != -1 && this.textureIds.Contains(_texIds[i]) == -1)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06005AFD RID: 23293 RVA: 0x00232838 File Offset: 0x00230A38
	public bool CanAdd(IList<int> _texIds)
	{
		this.needToAdd.Clear();
		for (int i = 0; i < _texIds.Count; i++)
		{
			if (_texIds[i] != -1 && this.textureIds.Contains(_texIds[i]) == -1)
			{
				this.needToAdd.Add(_texIds[i]);
			}
		}
		if (this.needToAdd.Count == 0)
		{
			return true;
		}
		if (this.needToAdd.Count <= TerrainSubMesh.vertexColors.Length - this.textureIds.Count)
		{
			this.Add(this.needToAdd);
			return true;
		}
		return false;
	}

	// Token: 0x06005AFE RID: 23294 RVA: 0x002328D0 File Offset: 0x00230AD0
	public void Add(int[] _texIds)
	{
		foreach (int num in _texIds)
		{
			if (num != -1)
			{
				int num2 = -1;
				int num3 = 0;
				while (num2 == -1 && num3 < this.others.Count)
				{
					num2 = this.others[num3].textureIds.Contains(num);
					if (num2 != -1 && this.textureIds.DataAvail[num2])
					{
						num2 = -1;
					}
					num3++;
				}
				this.textureIds.Add(num2, num);
			}
		}
	}

	// Token: 0x06005AFF RID: 23295 RVA: 0x0023294C File Offset: 0x00230B4C
	public void Add(ArrayListMP<int> _texIds)
	{
		for (int i = 0; i < _texIds.Count; i++)
		{
			int num = _texIds[i];
			if (num != -1)
			{
				int num2 = -1;
				int num3 = 0;
				while (num2 == -1 && num3 < this.others.Count)
				{
					num2 = this.others[num3].textureIds.Contains(num);
					if (num2 != -1 && this.textureIds.DataAvail[num2])
					{
						num2 = -1;
					}
					num3++;
				}
				this.textureIds.Add(num2, num);
			}
		}
	}

	// Token: 0x06005B00 RID: 23296 RVA: 0x002329CC File Offset: 0x00230BCC
	public Color GetColorForTextureId(int _texId)
	{
		int num = this.textureIds.Contains(_texId);
		if (num != -1)
		{
			return TerrainSubMesh.vertexColors[num];
		}
		return TerrainSubMesh.vertexColors[0];
	}

	// Token: 0x06005B01 RID: 23297 RVA: 0x00232A01 File Offset: 0x00230C01
	public int GetTextureIdCount()
	{
		return this.textureIds.Count;
	}

	// Token: 0x06005B02 RID: 23298 RVA: 0x00232A10 File Offset: 0x00230C10
	public int GetTextureId(int _idx)
	{
		for (int i = 0; i < this.textureIds.Size; i++)
		{
			if (this.textureIds.DataAvail[i] && _idx == 0)
			{
				return this.textureIds.Data[i];
			}
			_idx--;
		}
		return 0;
	}

	// Token: 0x040046CC RID: 18124
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color[] vertexColors = new Color[]
	{
		new Color(0f, 0f, 0f, 0f),
		new Color(1f, 0f, 0f, 0f),
		new Color(0f, 1f, 0f, 0f)
	};

	// Token: 0x040046CD RID: 18125
	public ArrayDynamicFast<int> textureIds;

	// Token: 0x040046CE RID: 18126
	public ArrayListMP<int> triangles;

	// Token: 0x040046CF RID: 18127
	[PublicizedFrom(EAccessModifier.Private)]
	public ArrayListMP<int> needToAdd;

	// Token: 0x040046D0 RID: 18128
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TerrainSubMesh> others;
}
