using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BBF RID: 3007
public struct TerrainSubMesh
{
	// Token: 0x06005AE2 RID: 23266 RVA: 0x0022F4BF File Offset: 0x0022D6BF
	public TerrainSubMesh(List<TerrainSubMesh> _others, int _minSize = 0)
	{
		this.others = _others;
		this.textureIds = new ArrayDynamicFast<int>(TerrainSubMesh.vertexColors.Length);
		this.triangles = new ArrayListMP<int>(MemoryPools.poolInt, _minSize);
		this.needToAdd = new ArrayListMP<int>(MemoryPools.poolInt, 0);
	}

	// Token: 0x06005AE3 RID: 23267 RVA: 0x0022F4FC File Offset: 0x0022D6FC
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

	// Token: 0x06005AE4 RID: 23268 RVA: 0x0022F53C File Offset: 0x0022D73C
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

	// Token: 0x06005AE5 RID: 23269 RVA: 0x0022F5D4 File Offset: 0x0022D7D4
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

	// Token: 0x06005AE6 RID: 23270 RVA: 0x0022F650 File Offset: 0x0022D850
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

	// Token: 0x06005AE7 RID: 23271 RVA: 0x0022F6D0 File Offset: 0x0022D8D0
	public Color GetColorForTextureId(int _texId)
	{
		int num = this.textureIds.Contains(_texId);
		if (num != -1)
		{
			return TerrainSubMesh.vertexColors[num];
		}
		return TerrainSubMesh.vertexColors[0];
	}

	// Token: 0x06005AE8 RID: 23272 RVA: 0x0022F705 File Offset: 0x0022D905
	public int GetTextureIdCount()
	{
		return this.textureIds.Count;
	}

	// Token: 0x06005AE9 RID: 23273 RVA: 0x0022F714 File Offset: 0x0022D914
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

	// Token: 0x0400468D RID: 18061
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color[] vertexColors = new Color[]
	{
		new Color(0f, 0f, 0f, 0f),
		new Color(1f, 0f, 0f, 0f),
		new Color(0f, 1f, 0f, 0f)
	};

	// Token: 0x0400468E RID: 18062
	public ArrayDynamicFast<int> textureIds;

	// Token: 0x0400468F RID: 18063
	public ArrayListMP<int> triangles;

	// Token: 0x04004690 RID: 18064
	[PublicizedFrom(EAccessModifier.Private)]
	public ArrayListMP<int> needToAdd;

	// Token: 0x04004691 RID: 18065
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TerrainSubMesh> others;
}
