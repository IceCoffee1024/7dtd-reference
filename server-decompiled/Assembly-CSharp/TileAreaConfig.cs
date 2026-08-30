using System;

// Token: 0x02000BCA RID: 3018
public struct TileAreaConfig
{
	// Token: 0x06005B09 RID: 23305 RVA: 0x00230110 File Offset: 0x0022E310
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

	// Token: 0x040046B1 RID: 18097
	public Vector2i tileStart;

	// Token: 0x040046B2 RID: 18098
	public Vector2i tileEnd;

	// Token: 0x040046B3 RID: 18099
	public int tileSizeInWorldUnits;

	// Token: 0x040046B4 RID: 18100
	public bool bWrapAroundX;

	// Token: 0x040046B5 RID: 18101
	public bool bWrapAroundZ;
}
