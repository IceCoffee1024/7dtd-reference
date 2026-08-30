using System;

// Token: 0x020001A3 RID: 419
public abstract class BlockShapeBillboardRotatedAbstract : BlockShapeRotatedAbstract
{
	// Token: 0x06000CAF RID: 3247 RVA: 0x0004FA1F File Offset: 0x0004DC1F
	public BlockShapeBillboardRotatedAbstract()
	{
		this.IsSolidCube = false;
		this.IsSolidSpace = false;
		this.IsRotatable = true;
		this.LightOpacity = 0;
	}

	// Token: 0x06000CB0 RID: 3248 RVA: 0x0004FA43 File Offset: 0x0004DC43
	public override void Init(Block _block)
	{
		base.Init(_block);
		_block.IsDecoration = true;
	}

	// Token: 0x06000CB1 RID: 3249 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsRenderDecoration()
	{
		return true;
	}

	// Token: 0x06000CB2 RID: 3250 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool isRenderFace(BlockValue _blockValue, BlockFace _face, BlockValue _adjBlockValue)
	{
		return false;
	}

	// Token: 0x06000CB3 RID: 3251 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int getFacesDrawnFullBitfield(BlockValue _blockValue)
	{
		return 0;
	}

	// Token: 0x06000CB4 RID: 3252 RVA: 0x0004FA54 File Offset: 0x0004DC54
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
			_blockValue.rotation = b;
		}
		return _blockValue;
	}
}
