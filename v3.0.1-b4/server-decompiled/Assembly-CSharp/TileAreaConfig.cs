using System;

// Token: 0x02000BB8 RID: 3000
public struct TileAreaConfig
{
	// Token: 0x06005A81 RID: 23169 RVA: 0x0022D3A8 File Offset: 0x0022B5A8
	public void checkCoordinates(ref int _tileX, ref int _tileZ)
	{
		Vector2i vector2i = this.tileEnd - this.tileStart + new Vector2i(1, 1);
		if (_tileX < this.tileStart.x)
		{
			if (this.bWrapAroundX)
			{
				_tileX += vector2i.x;
			}
			else
			{
				_tileX = this.tileStart.x;
			}
		}
		else if (_tileX > this.tileEnd.x)
		{
			if (this.bWrapAroundX)
			{
				_tileX -= vector2i.x;
			}
			else
			{
				_tileX = this.tileEnd.x;
			}
		}
		if (_tileZ >= this.tileStart.y)
		{
			if (_tileZ > this.tileEnd.y)
			{
				if (this.bWrapAroundZ)
				{
					_tileZ -= vector2i.y;
					return;
				}
				_tileZ = this.tileEnd.y;
			}
			return;
		}
		if (this.bWrapAroundZ)
		{
			_tileZ += vector2i.y;
			return;
		}
		_tileZ = this.tileStart.y;
	}

	// Token: 0x04004615 RID: 17941
	public Vector2i tileStart;

	// Token: 0x04004616 RID: 17942
	public Vector2i tileEnd;

	// Token: 0x04004617 RID: 17943
	public int tileSizeInWorldUnits;

	// Token: 0x04004618 RID: 17944
	public bool bWrapAroundX;

	// Token: 0x04004619 RID: 17945
	public bool bWrapAroundZ;
}
