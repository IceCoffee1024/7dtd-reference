using System;

// Token: 0x02000BDC RID: 3036
public struct TileAreaConfig
{
	// Token: 0x06005B22 RID: 23330 RVA: 0x0023340C File Offset: 0x0023160C
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

	// Token: 0x040046F0 RID: 18160
	public Vector2i tileStart;

	// Token: 0x040046F1 RID: 18161
	public Vector2i tileEnd;

	// Token: 0x040046F2 RID: 18162
	public int tileSizeInWorldUnits;

	// Token: 0x040046F3 RID: 18163
	public bool bWrapAroundX;

	// Token: 0x040046F4 RID: 18164
	public bool bWrapAroundZ;
}
