using System;
using UnityEngine;

// Token: 0x02000B2F RID: 2863
public class ChunkVertexLayer : IMemoryPoolableObject
{
	// Token: 0x060056C0 RID: 22208 RVA: 0x002153F8 File Offset: 0x002135F8
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

	// Token: 0x060056C1 RID: 22209 RVA: 0x0021545C File Offset: 0x0021365C
	public void Reset()
	{
		for (int i = 0; i < this.valid.Length; i++)
		{
			this.m_Vertices[i] = Vector3.zero;
			this.yPos[i] = 0f;
			this.valid[i] = false;
		}
	}

	// Token: 0x060056C2 RID: 22210 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x060056C3 RID: 22211 RVA: 0x002154A4 File Offset: 0x002136A4
	public bool getAt(int _x, int _y, out Vector3 _vec)
	{
		int num = _x + (_y << this.wPow);
		return this.getAt(num, out _vec);
	}

	// Token: 0x060056C4 RID: 22212 RVA: 0x002154C7 File Offset: 0x002136C7
	public bool getAt(int _offs, out Vector3 _vec)
	{
		_vec = this.m_Vertices[_offs];
		return this.valid[_offs];
	}

	// Token: 0x060056C5 RID: 22213 RVA: 0x002154E4 File Offset: 0x002136E4
	public void setAt(int _x, int _y, Vector3 _v)
	{
		int num = _x + (_y << this.wPow);
		this.m_Vertices[num] = _v;
		this.valid[num] = true;
	}

	// Token: 0x060056C6 RID: 22214 RVA: 0x00215518 File Offset: 0x00213718
	public bool getYPosAt(int _x, int _y, out float _ypos)
	{
		int num = _x + (_y << this.wPow);
		return this.getYPosAt(num, out _ypos);
	}

	// Token: 0x060056C7 RID: 22215 RVA: 0x0021553B File Offset: 0x0021373B
	public bool getYPosAt(int _offs, out float _ypos)
	{
		_ypos = this.yPos[_offs];
		return this.valid[_offs];
	}

	// Token: 0x060056C8 RID: 22216 RVA: 0x00215550 File Offset: 0x00213750
	public void setYPosAt(int _x, int _y, float _ypos)
	{
		int num = _x + (_y << this.wPow);
		this.yPos[num] = _ypos;
		this.valid[num] = true;
	}

	// Token: 0x060056C9 RID: 22217 RVA: 0x0021557D File Offset: 0x0021377D
	public void setInvalid(int _offs)
	{
		this.valid[_offs] = false;
	}

	// Token: 0x060056CA RID: 22218 RVA: 0x00215588 File Offset: 0x00213788
	public int GetUsedMem()
	{
		return this.m_Vertices.Length * 13 + 8;
	}

	// Token: 0x04004315 RID: 17173
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3[] m_Vertices;

	// Token: 0x04004316 RID: 17174
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] yPos;

	// Token: 0x04004317 RID: 17175
	[PublicizedFrom(EAccessModifier.Private)]
	public bool[] valid;

	// Token: 0x04004318 RID: 17176
	[PublicizedFrom(EAccessModifier.Private)]
	public int wPow;

	// Token: 0x04004319 RID: 17177
	[PublicizedFrom(EAccessModifier.Private)]
	public int hPow;

	// Token: 0x0400431A RID: 17178
	public static int InstanceCount;
}
