using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000C2E RID: 3118
public class DecoOccupiedMap
{
	// Token: 0x06005EB9 RID: 24249 RVA: 0x00251A90 File Offset: 0x0024FC90
	public DecoOccupiedMap(int _worldWidth, int _worldHeight)
	{
		this.width = _worldWidth;
		this.height = _worldHeight;
		this.widthHalf = this.width / 2;
		this.heightHalf = this.height / 2;
		this.occupiedMap = new EnumDecoOccupied[this.width * this.height];
	}

	// Token: 0x06005EBA RID: 24250 RVA: 0x00251AE5 File Offset: 0x0024FCE5
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public EnumDecoOccupied Get(int _offs)
	{
		return this.occupiedMap[_offs];
	}

	// Token: 0x06005EBB RID: 24251 RVA: 0x00251AF0 File Offset: 0x0024FCF0
	public EnumDecoOccupied Get(int _x, int _z)
	{
		int num = DecoManager.CheckPosition(this.width, this.height, _x, _z);
		if (num >= 0)
		{
			return this.occupiedMap[num];
		}
		return EnumDecoOccupied.NoneAllowed;
	}

	// Token: 0x06005EBC RID: 24252 RVA: 0x00251B1F File Offset: 0x0024FD1F
	public void Set(int _offs, EnumDecoOccupied _v)
	{
		this.occupiedMap[_offs] = _v;
	}

	// Token: 0x06005EBD RID: 24253 RVA: 0x00251B2C File Offset: 0x0024FD2C
	public void Set(int _x, int _z, EnumDecoOccupied _v)
	{
		int num = DecoManager.CheckPosition(this.width, this.height, _x, _z);
		if (num >= 0)
		{
			this.occupiedMap[num] = _v;
		}
	}

	// Token: 0x06005EBE RID: 24254 RVA: 0x00251B5C File Offset: 0x0024FD5C
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

	// Token: 0x06005EBF RID: 24255 RVA: 0x00251BC8 File Offset: 0x0024FDC8
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

	// Token: 0x06005EC0 RID: 24256 RVA: 0x00251C41 File Offset: 0x0024FE41
	public EnumDecoOccupied[] GetData()
	{
		return this.occupiedMap;
	}

	// Token: 0x06005EC1 RID: 24257 RVA: 0x00251C4C File Offset: 0x0024FE4C
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

	// Token: 0x040049D0 RID: 18896
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumDecoOccupied[] occupiedMap;

	// Token: 0x040049D1 RID: 18897
	[PublicizedFrom(EAccessModifier.Private)]
	public int width;

	// Token: 0x040049D2 RID: 18898
	[PublicizedFrom(EAccessModifier.Private)]
	public int height;

	// Token: 0x040049D3 RID: 18899
	[PublicizedFrom(EAccessModifier.Private)]
	public int widthHalf;

	// Token: 0x040049D4 RID: 18900
	[PublicizedFrom(EAccessModifier.Private)]
	public int heightHalf;
}
