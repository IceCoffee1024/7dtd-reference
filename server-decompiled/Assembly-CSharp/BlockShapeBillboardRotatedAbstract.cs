using System;

// Token: 0x020001A5 RID: 421
public abstract class BlockShapeBillboardRotatedAbstract : BlockShapeRotatedAbstract
{
	// Token: 0x06000CBA RID: 3258 RVA: 0x0004FE37 File Offset: 0x0004E037
	public BlockShapeBillboardRotatedAbstract()
	{
		this.IsSolidCube = false;
		this.IsSolidSpace = false;
		this.IsRotatable = true;
		this.LightOpacity = 0;
	}

	// Token: 0x06000CBB RID: 3259 RVA: 0x0004FE5B File Offset: 0x0004E05B
	public override void Init(Block _block)
	{
		base.Init(_block);
		_block.IsDecoration = true;
	}

	// Token: 0x06000CBC RID: 3260 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsRenderDecoration()
	{
		return true;
	}

	// Token: 0x06000CBD RID: 3261 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool isRenderFace(BlockValue _blockValue, BlockFace _face, BlockValue _adjBlockValue)
	{
		return false;
	}

	// Token: 0x06000CBE RID: 3262 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int getFacesDrawnFullBitfield(BlockValue _blockValue)
	{
		return 0;
	}

	// Token: 0x06000CBF RID: 3263 RVA: 0x0004FE6C File Offset: 0x0004E06C
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
