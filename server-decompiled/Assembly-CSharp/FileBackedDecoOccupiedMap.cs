using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C41 RID: 3137
public class FileBackedDecoOccupiedMap : IDisposable
{
	// Token: 0x06005F05 RID: 24325 RVA: 0x00253B74 File Offset: 0x00251D74
	public FileBackedDecoOccupiedMap(int _worldWidth, int _worldHeight)
	{
		this.width = _worldWidth;
		this.height = _worldHeight;
		this.heightHalf = this.height / 2;
		this.occupiedMap = new FileBackedArray<EnumDecoOccupied>(this.width * this.height);
		this.cacheLength = this.width * 128;
	}

	// Token: 0x06005F06 RID: 24326 RVA: 0x00253BCD File Offset: 0x00251DCD
	[PublicizedFrom(EAccessModifier.Private)]
	public int GetDecoChunkRowCacheStart(int offset)
	{
		return offset / this.cacheLength * this.cacheLength;
	}

	// Token: 0x06005F07 RID: 24327 RVA: 0x00253BE0 File Offset: 0x00251DE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void Cache(int offset)
	{
		if (offset >= this.cacheEnd || offset < this.cacheStart)
		{
			this.cacheStart = this.GetDecoChunkRowCacheStart(offset);
			this.cacheEnd = this.cacheStart + this.cacheLength;
			IBackedArrayHandle backedArrayHandle = this.cacheHandle;
			if (backedArrayHandle != null)
			{
				backedArrayHandle.Dispose();
			}
			this.cacheHandle = this.occupiedMap.GetReadOnlyMemory(this.cacheStart, this.cacheLength, out this.cache);
		}
	}

	// Token: 0x06005F08 RID: 24328 RVA: 0x00253C54 File Offset: 0x00251E54
	public unsafe EnumDecoOccupied Get(int _offs)
	{
		this.Cache(_offs);
		return (EnumDecoOccupied)(*this.cache.Span[_offs - this.cacheStart]);
	}

	// Token: 0x06005F09 RID: 24329 RVA: 0x00253C84 File Offset: 0x00251E84
	public void CopyDecoChunkRow(int row, EnumDecoOccupied[] from)
	{
		int num = this.heightHalf / 128;
		int num2 = (row + num) * 128 * this.width;
		Span<EnumDecoOccupied> span2;
		using (this.occupiedMap.GetSpan(num2, this.cacheLength, out span2))
		{
			from.AsSpan<EnumDecoOccupied>(num2, this.cacheLength).CopyTo(span2);
		}
	}

	// Token: 0x06005F0A RID: 24330 RVA: 0x00253CF8 File Offset: 0x00251EF8
	public void Dispose()
	{
		IBackedArrayHandle backedArrayHandle = this.cacheHandle;
		if (backedArrayHandle != null)
		{
			backedArrayHandle.Dispose();
		}
		this.cacheHandle = null;
		FileBackedArray<EnumDecoOccupied> fileBackedArray = this.occupiedMap;
		if (fileBackedArray != null)
		{
			fileBackedArray.Dispose();
		}
		this.occupiedMap = null;
	}

	// Token: 0x06005F0B RID: 24331 RVA: 0x00253D2C File Offset: 0x00251F2C
	public void SaveAsTexture(string path, bool includeFlatAreas = false, List<FlatArea> flatAreas = null)
	{
		Color32[] array = new Color32[this.occupiedMap.Length];
		for (int i = 0; i < this.occupiedMap.Length; i++)
		{
			Color color = Color.black;
			switch (this.Get(i))
			{
			case EnumDecoOccupied.SmallSlope:
				color = Color.blue;
				break;
			case EnumDecoOccupied.Stop_BigDeco:
				color = Color.gray;
				break;
			case EnumDecoOccupied.Perimeter:
				color = Color.red;
				break;
			case EnumDecoOccupied.Stop_AnyDeco:
				color = Color.cyan;
				break;
			case EnumDecoOccupied.Deco:
				color = Color.green;
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

	// Token: 0x04004A10 RID: 18960
	[PublicizedFrom(EAccessModifier.Private)]
	public FileBackedArray<EnumDecoOccupied> occupiedMap;

	// Token: 0x04004A11 RID: 18961
	[PublicizedFrom(EAccessModifier.Private)]
	public int width;

	// Token: 0x04004A12 RID: 18962
	[PublicizedFrom(EAccessModifier.Private)]
	public int height;

	// Token: 0x04004A13 RID: 18963
	[PublicizedFrom(EAccessModifier.Private)]
	public int heightHalf;

	// Token: 0x04004A14 RID: 18964
	[PublicizedFrom(EAccessModifier.Private)]
	public int cacheLength;

	// Token: 0x04004A15 RID: 18965
	[PublicizedFrom(EAccessModifier.Private)]
	public IBackedArrayHandle cacheHandle;

	// Token: 0x04004A16 RID: 18966
	[PublicizedFrom(EAccessModifier.Private)]
	public ReadOnlyMemory<EnumDecoOccupied> cache;

	// Token: 0x04004A17 RID: 18967
	[PublicizedFrom(EAccessModifier.Private)]
	public int cacheStart;

	// Token: 0x04004A18 RID: 18968
	[PublicizedFrom(EAccessModifier.Private)]
	public int cacheEnd;
}
