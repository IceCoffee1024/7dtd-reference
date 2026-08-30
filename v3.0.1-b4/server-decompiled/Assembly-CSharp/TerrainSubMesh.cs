using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BAD RID: 2989
public struct TerrainSubMesh
{
	// Token: 0x06005A5A RID: 23130 RVA: 0x0022C757 File Offset: 0x0022A957
	public TerrainSubMesh(List<TerrainSubMesh> _others, int _minSize = 0)
	{
		this.others = _others;
		this.textureIds = new ArrayDynamicFast<int>(TerrainSubMesh.vertexColors.Length);
		this.triangles = new ArrayListMP<int>(MemoryPools.poolInt, _minSize);
		this.needToAdd = new ArrayListMP<int>(MemoryPools.poolInt, 0);
	}

	// Token: 0x06005A5B RID: 23131 RVA: 0x0022C794 File Offset: 0x0022A994
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

	// Token: 0x06005A5C RID: 23132 RVA: 0x0022C7D4 File Offset: 0x0022A9D4
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

	// Token: 0x06005A5D RID: 23133 RVA: 0x0022C86C File Offset: 0x0022AA6C
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

	// Token: 0x06005A5E RID: 23134 RVA: 0x0022C8E8 File Offset: 0x0022AAE8
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

	// Token: 0x06005A5F RID: 23135 RVA: 0x0022C968 File Offset: 0x0022AB68
	public Color GetColorForTextureId(int _texId)
	{
		int num = this.textureIds.Contains(_texId);
		if (num != -1)
		{
			return TerrainSubMesh.vertexColors[num];
		}
		return TerrainSubMesh.vertexColors[0];
	}

	// Token: 0x06005A60 RID: 23136 RVA: 0x0022C99D File Offset: 0x0022AB9D
	public int GetTextureIdCount()
	{
		return this.textureIds.Count;
	}

	// Token: 0x06005A61 RID: 23137 RVA: 0x0022C9AC File Offset: 0x0022ABAC
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

	// Token: 0x040045F1 RID: 17905
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Color[] vertexColors = new Color[]
	{
		new Color(0f, 0f, 0f, 0f),
		new Color(1f, 0f, 0f, 0f),
		new Color(0f, 1f, 0f, 0f)
	};

	// Token: 0x040045F2 RID: 17906
	public ArrayDynamicFast<int> textureIds;

	// Token: 0x040045F3 RID: 17907
	public ArrayListMP<int> triangles;

	// Token: 0x040045F4 RID: 17908
	[PublicizedFrom(EAccessModifier.Private)]
	public ArrayListMP<int> needToAdd;

	// Token: 0x040045F5 RID: 17909
	[PublicizedFrom(EAccessModifier.Private)]
	public List<TerrainSubMesh> others;
}
