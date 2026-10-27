using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BDC RID: 3036
public struct TerrainSubMesh
{
	// Token: 0x06005B20 RID: 23328 RVA: 0x00233D17 File Offset: 0x00231F17
	public TerrainSubMesh(List<TerrainSubMesh> _others, int _minSize = 0)
	{
		this.others = _others;
		this.textureIds = new ArrayDynamicFast<int>(TerrainSubMesh.vertexColors.Length);
		this.triangles = new ArrayListMP<int>(MemoryPools.poolInt, _minSize);
		this.needToAdd = new ArrayListMP<int>(MemoryPools.poolInt, 0);
	}

	// Token: 0x06005B21 RID: 23329 RVA: 0x00233D54 File Offset: 0x00231F54
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

	// Token: 0x06005B22 RID: 23330 RVA: 0x00233D94 File Offset: 0x00231F94
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

	// Token: 0x06005B23 RID: 23331 RVA: 0x00233E2C File Offset: 0x0023202C
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

	// Token: 0x06005B24 RID: 23332 RVA: 0x00233EA8 File Offset: 0x002320A8
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

	// Token: 0x06005B25 RID: 23333 RVA: 0x00233F28 File Offset: 0x00232128
	public Color GetColorForTextureId(int _texId)
	{
		int num = this.textureIds.Contains(_texId);
		if (num != -1)
		{
			return TerrainSubMesh.vertexColors[num];
		}
		return TerrainSubMesh.vertexColors[0];
	}

	// Token: 0x06005B26 RID: 23334 RVA: 0x00233F5D File Offset: 0x0023215D
	public int GetTextureIdCount()
	{
		return this.textureIds.Count;
	}

	// Token: 0x06005B27 RID: 23335 RVA: 0x00233F6C File Offset: 0x0023216C
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

	// Token: 0x040046F1 RID: 18161
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color[] vertexColors = new Color[]
	{
		new Color(0f, 0f, 0f, 0f),
		new Color(1f, 0f, 0f, 0f),
		new Color(0f, 1f, 0f, 0f)
	};

	// Token: 0x040046F2 RID: 18162
	public ArrayDynamicFast<int> textureIds;

	// Token: 0x040046F3 RID: 18163
	public ArrayListMP<int> triangles;

	// Token: 0x040046F4 RID: 18164
	[PublicizedFrom(EAccessModifier.Private)]
	public ArrayListMP<int> needToAdd;

	// Token: 0x040046F5 RID: 18165
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TerrainSubMesh> others;
}
