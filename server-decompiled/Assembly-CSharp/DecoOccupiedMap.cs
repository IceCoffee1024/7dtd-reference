using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000C1C RID: 3100
public class DecoOccupiedMap
{
	// Token: 0x06005EA0 RID: 24224 RVA: 0x0024E794 File Offset: 0x0024C994
	public DecoOccupiedMap(int _worldWidth, int _worldHeight)
	{
		this.width = _worldWidth;
		this.height = _worldHeight;
		this.widthHalf = this.width / 2;
		this.heightHalf = this.height / 2;
		this.occupiedMap = new EnumDecoOccupied[this.width * this.height];
	}

	// Token: 0x06005EA1 RID: 24225 RVA: 0x0024E7E9 File Offset: 0x0024C9E9
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public EnumDecoOccupied Get(int _offs)
	{
		return this.occupiedMap[_offs];
	}

	// Token: 0x06005EA2 RID: 24226 RVA: 0x0024E7F4 File Offset: 0x0024C9F4
	public EnumDecoOccupied Get(int _x, int _z)
	{
		int num = DecoManager.CheckPosition(this.width, this.height, _x, _z);
		if (num >= 0)
		{
			return this.occupiedMap[num];
		}
		return EnumDecoOccupied.NoneAllowed;
	}

	// Token: 0x06005EA3 RID: 24227 RVA: 0x0024E823 File Offset: 0x0024CA23
	public void Set(int _offs, EnumDecoOccupied _v)
	{
		this.occupiedMap[_offs] = _v;
	}

	// Token: 0x06005EA4 RID: 24228 RVA: 0x0024E830 File Offset: 0x0024CA30
	public void Set(int _x, int _z, EnumDecoOccupied _v)
	{
		int num = DecoManager.CheckPosition(this.width, this.height, _x, _z);
		if (num >= 0)
		{
			this.occupiedMap[num] = _v;
		}
	}

	// Token: 0x06005EA5 RID: 24229 RVA: 0x0024E860 File Offset: 0x0024CA60
	public bool CheckArea(int _x, int _z, EnumDecoOccupied _v, int _rectSizeX, int _rectSizeZ)
	{
		int num = DecoManager.CheckPosition(this.width, this.height, _x, _z);
		if (num < 0)
		{
			return true;
		}
		for (int i = 0; i < _rectSizeZ; i++)
		{
			for (int j = 0; j < _rectSizeX; j++)
			{
				if (num >= this.occupiedMap.Length)
				{
					return true;
				}
				if (this.occupiedMap[num] >= _v)
				{
					return true;
				}
				num++;
			}
			num += this.width - _rectSizeX;
		}
		return false;
	}

	// Token: 0x06005EA6 RID: 24230 RVA: 0x0024E8CC File Offset: 0x0024CACC
	public void SetArea(int _x, int _z, EnumDecoOccupied _v, int _rectSizeX, int _rectSizeZ)
	{
		int num = _x + this.widthHalf + (_z + this.heightHalf) * this.width;
		for (int i = 0; i < _rectSizeZ; i++)
		{
			for (int j = 0; j < _rectSizeX; j++)
			{
				if (num < 0 || num >= this.occupiedMap.Length)
				{
					num++;
				}
				else
				{
					if (this.occupiedMap[num] < _v)
					{
						this.occupiedMap[num] = _v;
					}
					num++;
				}
			}
			num += this.width - _rectSizeX;
		}
	}

	// Token: 0x06005EA7 RID: 24231 RVA: 0x0024E945 File Offset: 0x0024CB45
	public EnumDecoOccupied[] GetData()
	{
		return this.occupiedMap;
	}

	// Token: 0x06005EA8 RID: 24232 RVA: 0x0024E950 File Offset: 0x0024CB50
	public void SaveAsTexture(string path, bool includeFlatAreas = false, List<FlatArea> flatAreas = null)
	{
		Color32[] array = new Color32[this.occupiedMap.Length];
		for (int i = 0; i < this.occupiedMap.Length; i++)
		{
			Color color = Color.black;
			switch (this.occupiedMap[i])
			{
			case EnumDecoOccupied.SmallSlope:
				color = Color.blue;
				break;
			case EnumDecoOccupied.Stop_BigDeco:
				color = Color.gray;
				break;
			case EnumDecoOccupied.Perimeter:
				if (!includeFlatAreas)
				{
					color = Color.red;
				}
				break;
			case EnumDecoOccupied.Stop_AnyDeco:
				color = Color.cyan;
				break;
			case EnumDecoOccupied.Deco:
				if (!includeFlatAreas)
				{
					color = Color.green;
				}
				break;
			case EnumDecoOccupied.POI:
				color = Color.magenta;
				break;
			case EnumDecoOccupied.BigSlope:
				color = Color.yellow;
				break;
			case EnumDecoOccupied.NoneAllowed:
				color = Color.white;
				break;
			}
			array[i] = color;
		}
		if (includeFlatAreas)
		{
			if (flatAreas == null || flatAreas.Count == 0)
			{
				FlatAreaManager flatAreaManager = GameManager.Instance.World.FlatAreaManager;
				flatAreas = ((flatAreaManager != null) ? flatAreaManager.GetAllFlatAreas() : null);
			}
			if (flatAreas != null)
			{
				foreach (FlatArea flatArea in flatAreas)
				{
					foreach (Vector2i vector2i in flatArea.GetPositions())
					{
						Color red;
						if (vector2i.x == flatArea.position.x && vector2i.y == flatArea.position.z)
						{
							red = Color.red;
						}
						else if (flatArea.size == 16)
						{
							red = new Color(0.75f, 0.75f, 0.75f, 1f);
						}
						else
						{
							red = new Color(0.5f, 0.5f, 0.5f, 1f);
						}
						int num = DecoManager.CheckPosition(this.width, this.height, vector2i.x, vector2i.y);
						array[num] = red;
					}
				}
			}
		}
		Texture2D texture2D = new Texture2D(this.width, this.height);
		texture2D.SetPixels32(array);
		texture2D.Apply();
		TextureUtils.SaveTexture(texture2D, path);
		Log.Out("Saved deco texture to {0}", new object[] { path });
		global::UnityEngine.Object.Destroy(texture2D);
	}

	// Token: 0x04004991 RID: 18833
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumDecoOccupied[] occupiedMap;

	// Token: 0x04004992 RID: 18834
	[PublicizedFrom(EAccessModifier.Private)]
	public int width;

	// Token: 0x04004993 RID: 18835
	[PublicizedFrom(EAccessModifier.Private)]
	public int height;

	// Token: 0x04004994 RID: 18836
	[PublicizedFrom(EAccessModifier.Private)]
	public int widthHalf;

	// Token: 0x04004995 RID: 18837
	[PublicizedFrom(EAccessModifier.Private)]
	public int heightHalf;
}
