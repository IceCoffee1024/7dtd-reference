using System;
using UnityEngine;

// Token: 0x02000B53 RID: 2899
public class ChunkVertexLayer : IMemoryPoolableObject
{
	// Token: 0x06005762 RID: 22370 RVA: 0x0021B3A0 File Offset: 0x002195A0
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

	// Token: 0x06005763 RID: 22371 RVA: 0x0021B404 File Offset: 0x00219604
	public void Reset()
	{
		for (int i = 0; i < this.valid.Length; i++)
		{
			this.m_Vertices[i] = Vector3.zero;
			this.yPos[i] = 0f;
			this.valid[i] = false;
		}
	}

	// Token: 0x06005764 RID: 22372 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x06005765 RID: 22373 RVA: 0x0021B44C File Offset: 0x0021964C
	public bool getAt(int _x, int _y, out Vector3 _vec)
	{
		int num = _x + (_y << this.wPow);
		return this.getAt(num, out _vec);
	}

	// Token: 0x06005766 RID: 22374 RVA: 0x0021B46F File Offset: 0x0021966F
	public bool getAt(int _offs, out Vector3 _vec)
	{
		_vec = this.m_Vertices[_offs];
		return this.valid[_offs];
	}

	// Token: 0x06005767 RID: 22375 RVA: 0x0021B48C File Offset: 0x0021968C
	public void setAt(int _x, int _y, Vector3 _v)
	{
		int num = _x + (_y << this.wPow);
		this.m_Vertices[num] = _v;
		this.valid[num] = true;
	}

	// Token: 0x06005768 RID: 22376 RVA: 0x0021B4C0 File Offset: 0x002196C0
	public bool getYPosAt(int _x, int _y, out float _ypos)
	{
		int num = _x + (_y << this.wPow);
		return this.getYPosAt(num, out _ypos);
	}

	// Token: 0x06005769 RID: 22377 RVA: 0x0021B4E3 File Offset: 0x002196E3
	public bool getYPosAt(int _offs, out float _ypos)
	{
		_ypos = this.yPos[_offs];
		return this.valid[_offs];
	}

	// Token: 0x0600576A RID: 22378 RVA: 0x0021B4F8 File Offset: 0x002196F8
	public void setYPosAt(int _x, int _y, float _ypos)
	{
		int num = _x + (_y << this.wPow);
		this.yPos[num] = _ypos;
		this.valid[num] = true;
	}

	// Token: 0x0600576B RID: 22379 RVA: 0x0021B525 File Offset: 0x00219725
	public void setInvalid(int _offs)
	{
		this.valid[_offs] = false;
	}

	// Token: 0x0600576C RID: 22380 RVA: 0x0021B530 File Offset: 0x00219730
	public int GetUsedMem()
	{
		return this.m_Vertices.Length * 13 + 8;
	}

	// Token: 0x040043EE RID: 17390
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3[] m_Vertices;

	// Token: 0x040043EF RID: 17391
	[PublicizedFrom(EAccessModifier.Private)]
	public float[] yPos;

	// Token: 0x040043F0 RID: 17392
	[PublicizedFrom(EAccessModifier.Private)]
	public bool[] valid;

	// Token: 0x040043F1 RID: 17393
	[PublicizedFrom(EAccessModifier.Private)]
	public int wPow;

	// Token: 0x040043F2 RID: 17394
	[PublicizedFrom(EAccessModifier.Private)]
	public int hPow;

	// Token: 0x040043F3 RID: 17395
	public static int InstanceCount;
}
