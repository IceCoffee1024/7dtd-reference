using System;
using UnityEngine;

// Token: 0x020001B9 RID: 441
public abstract class BlockShapeRotatedAbstract : BlockShape
{
	// Token: 0x06000D2E RID: 3374 RVA: 0x0005664C File Offset: 0x0005484C
	public BlockShapeRotatedAbstract()
	{
		this.IsRotatable = true;
	}

	// Token: 0x06000D2F RID: 3375 RVA: 0x00056675 File Offset: 0x00054875
	public override void Init(Block _block)
	{
		base.Init(_block);
		this.createVertices();
		this.createBoundingBoxes();
		this.createAABBVertices();
	}

	// Token: 0x06000D30 RID: 3376 RVA: 0x00056690 File Offset: 0x00054890
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void createBoundingBoxes()
	{
		if (this.vertices == null)
		{
			return;
		}
		for (int i = 0; i < this.vertices.Length; i++)
		{
			if (i == 0)
			{
				this.boundsArr[0] = new Bounds(this.vertices[0], Vector3.zero);
			}
			else
			{
				this.boundsArr[0].Encapsulate(this.vertices[i]);
			}
		}
	}

	// Token: 0x06000D31 RID: 3377 RVA: 0x00056700 File Offset: 0x00054900
	[PublicizedFrom(EAccessModifier.Private)]
	public void createAABBVertices()
	{
		if (this.vertices == null)
		{
			return;
		}
		this.aabbVertices = new Vector3[this.boundsArr.Length * 2];
		for (int i = 0; i < this.boundsArr.Length; i++)
		{
			this.aabbVertices[i * 2] = new Vector3(this.boundsArr[i].min.x, this.boundsArr[i].min.y, this.boundsArr[i].min.z);
			this.aabbVertices[i * 2 + 1] = new Vector3(this.boundsArr[i].max.x, this.boundsArr[i].max.y, this.boundsArr[i].max.z);
		}
	}

	// Token: 0x06000D32 RID: 3378
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract void createVertices();

	// Token: 0x06000D33 RID: 3379 RVA: 0x000567F0 File Offset: 0x000549F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3[] rotateVertices(Vector3[] _vertices, Vector3 _drawPos, BlockValue _blockValue)
	{
		Quaternion rotation = this.GetRotation(_blockValue);
		Vector3[] array = MemoryPools.poolVector3.Alloc(_vertices.Length);
		Vector3 vector = BlockShapeRotatedAbstract.vecInternalOffset;
		Vector3 vector2 = _drawPos - BlockShapeRotatedAbstract.vecInternalOffset + this.GetRotationOffset(_blockValue);
		for (int i = 0; i < _vertices.Length; i++)
		{
			array[i] = rotation * (_vertices[i] + vector) + vector2;
		}
		return array;
	}

	// Token: 0x06000D34 RID: 3380 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int getFacesDrawnFullBitfield(BlockValue _blockValue)
	{
		return 0;
	}

	// Token: 0x06000D35 RID: 3381 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool isRenderFace(BlockValue _blockValue, BlockFace _face, BlockValue _adjBlockValue)
	{
		return false;
	}

	// Token: 0x06000D36 RID: 3382 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsRenderDecoration()
	{
		return true;
	}

	// Token: 0x06000D37 RID: 3383 RVA: 0x00056868 File Offset: 0x00054A68
	public override Bounds[] GetBounds(BlockValue _blockValue)
	{
		int rotation = (int)_blockValue.rotation;
		if (rotation < this.cachedBounds.Length)
		{
			Bounds[] array = this.cachedBounds[rotation];
			if (array != null)
			{
				return array;
			}
		}
		if (this.aabbVertices == null)
		{
			this.createAABBVertices();
		}
		Vector3[] array2 = (_blockValue.Block.RotateVerticesOnCollisionCheck(_blockValue) ? this.rotateVertices(this.aabbVertices, Vector3.zero, _blockValue) : this.aabbVertices);
		this.maxAABB_Y[rotation] = 0f;
		for (int i = 0; i < this.boundsArr.Length; i++)
		{
			this.boundsArr[i] = new Bounds(array2[i * 2], Vector3.zero);
			this.boundsArr[i].Encapsulate(array2[i * 2 + 1]);
			this.maxAABB_Y[rotation] = Utils.FastMax(this.maxAABB_Y[rotation], this.boundsArr[i].max.y);
		}
		for (int j = 0; j < this.boundsArr.Length; j++)
		{
			Vector3 size = this.boundsArr[j].size;
			for (int k = 0; k < 3; k++)
			{
				size[k] = Math.Max(Math.Max(this.minBounds[k], 0.05f) - size[k], 0f);
			}
			this.boundsArr[j].Expand(size);
		}
		if (rotation < this.cachedBounds.Length)
		{
			Bounds[] array3 = new Bounds[this.boundsArr.Length];
			this.boundsArr.CopyTo(array3, 0);
			this.cachedBounds[rotation] = array3;
		}
		return this.boundsArr;
	}

	// Token: 0x06000D38 RID: 3384 RVA: 0x00056A10 File Offset: 0x00054C10
	public override BlockValue RotateY(bool _bLeft, BlockValue _blockValue, int _rotCount)
	{
		for (int i = 0; i < _rotCount; i++)
		{
			byte b = _blockValue.rotation;
			if (b <= 3)
			{
				if (_bLeft)
				{
					b = ((b > 0) ? (b - 1) : 3);
				}
				else
				{
					b = ((b < 3) ? (b + 1) : 0);
				}
			}
			else if (b <= 7)
			{
				if (_bLeft)
				{
					b = ((b > 4) ? (b - 1) : 7);
				}
				else
				{
					b = ((b < 7) ? (b + 1) : 4);
				}
			}
			else if (b <= 11)
			{
				if (_bLeft)
				{
					b = ((b > 8) ? (b - 1) : 11);
				}
				else
				{
					b = ((b < 11) ? (b + 1) : 8);
				}
			}
			else if (b <= 15)
			{
				if (_bLeft)
				{
					b = ((b > 12) ? (b - 1) : 15);
				}
				else
				{
					b = ((b < 15) ? (b + 1) : 12);
				}
			}
			_blockValue.rotation = b;
		}
		return _blockValue;
	}

	// Token: 0x06000D39 RID: 3385 RVA: 0x00056AD2 File Offset: 0x00054CD2
	public override float GetStepHeight(BlockValue _blockValue, BlockFace crossingFace)
	{
		this.GetBounds(_blockValue);
		return this.maxAABB_Y[(int)_blockValue.rotation];
	}

	// Token: 0x04000BAA RID: 2986
	[PublicizedFrom(EAccessModifier.Protected)]
	public Vector3[] vertices;

	// Token: 0x04000BAB RID: 2987
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Vector3 vecInternalOffset = new Vector3(-0.5f, -0.5f, -0.5f);

	// Token: 0x04000BAC RID: 2988
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3[] aabbVertices;

	// Token: 0x04000BAD RID: 2989
	[PublicizedFrom(EAccessModifier.Private)]
	public Bounds[][] cachedBounds = new Bounds[32][];

	// Token: 0x04000BAE RID: 2990
	[PublicizedFrom(EAccessModifier.Protected)]
	public float[] maxAABB_Y = new float[32];
}
