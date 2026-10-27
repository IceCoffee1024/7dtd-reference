using System;

// Token: 0x02000BE7 RID: 3047
public struct TileAreaConfig
{
	// Token: 0x06005B47 RID: 23367 RVA: 0x00234968 File Offset: 0x00232B68
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

	// Token: 0x04004715 RID: 18197
	public Vector2i tileStart;

	// Token: 0x04004716 RID: 18198
	public Vector2i tileEnd;

	// Token: 0x04004717 RID: 18199
	public int tileSizeInWorldUnits;

	// Token: 0x04004718 RID: 18200
	public bool bWrapAroundX;

	// Token: 0x04004719 RID: 18201
	public bool bWrapAroundZ;
}
