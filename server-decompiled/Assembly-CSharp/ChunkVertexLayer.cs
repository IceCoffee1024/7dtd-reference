using System;
using UnityEngine;

// Token: 0x02000B5E RID: 2910
public class ChunkVertexLayer : IMemoryPoolableObject
{
	// Token: 0x06005787 RID: 22407 RVA: 0x0021C8FC File Offset: 0x0021AAFC
	public ChunkVertexLayer()
	{
		this.wPow = 4;
		this.hPow = 4;
		int num = 1 << this.wPow;
		int num2 = 1 << this.hPow;
		this.m_Vertices = new Vector3[num * num2];
		this.yPos = new float[num * num2];
		this.valid = new bool[num * num2];
	}

	// Token: 0x06005788 RID: 22408 RVA: 0x0021C960 File Offset: 0x0021AB60
	public void Reset()
	{
		for (int i = 0; i < this.valid.Length; i++)
		{
			this.m_Vertices[i] = Vector3.zero;
			this.yPos[i] = 0f;
			this.valid[i] = false;
		}
	}

	// Token: 0x06005789 RID: 22409 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x0600578A RID: 22410 RVA: 0x0021C9A8 File Offset: 0x0021ABA8
	public bool getAt(int _x, int _y, out Vector3 _vec)
	{
		int num = _x + (_y << this.wPow);
		return this.getAt(num, out _vec);
	}

	// Token: 0x0600578B RID: 22411 RVA: 0x0021C9CB File Offset: 0x0021ABCB
	public bool getAt(int _offs, out Vector3 _vec)
	{
		_vec = this.m_Vertices[_offs];
		return this.valid[_offs];
	}

	// Token: 0x0600578C RID: 22412 RVA: 0x0021C9E8 File Offset: 0x0021ABE8
	public void setAt(int _x, int _y, Vector3 _v)
	{
		int num = _x + (_y << this.wPow);
		this.m_Vertices[num] = _v;
		this.valid[num] = true;
	}

	// Token: 0x0600578D RID: 22413 RVA: 0x0021CA1C File Offset: 0x0021AC1C
	public bool getYPosAt(int _x, int _y, out float _ypos)
	{
		int num = _x + (_y << this.wPow);
		return this.getYPosAt(num, out _ypos);
	}

	// Token: 0x0600578E RID: 22414 RVA: 0x0021CA3F File Offset: 0x0021AC3F
	public bool getYPosAt(int _offs, out float _ypos)
	{
		_ypos = this.yPos[_offs];
		return this.valid[_offs];
	}

	// Token: 0x0600578F RID: 22415 RVA: 0x0021CA54 File Offset: 0x0021AC54
	public void setYPosAt(int _x, int _y, float _ypos)
	{
		int num = _x + (_y << this.wPow);
		this.yPos[num] = _ypos;
		this.valid[num] = true;
	}

	// Token: 0x06005790 RID: 22416 RVA: 0x0021CA81 File Offset: 0x0021AC81
	public void setInvalid(int _offs)
	{
		this.valid[_offs] = false;
	}

	// Token: 0x06005791 RID: 22417 RVA: 0x0021CA8C File Offset: 0x0021AC8C
	public int GetUsedMem()
	{
		return this.m_Vertices.Length * 13 + 8;
	}

	// Token: 0x04004413 RID: 17427
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3[] m_Vertices;

	// Token: 0x04004414 RID: 17428
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] yPos;

	// Token: 0x04004415 RID: 17429
	[PublicizedFrom(EAccessModifier.Private)]
	public bool[] valid;

	// Token: 0x04004416 RID: 17430
	[PublicizedFrom(EAccessModifier.Private)]
	public int wPow;

	// Token: 0x04004417 RID: 17431
	[PublicizedFrom(EAccessModifier.Private)]
	public int hPow;

	// Token: 0x04004418 RID: 17432
	public static int InstanceCount;
}
