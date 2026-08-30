using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C12 RID: 3090
public class FileBackedDecoOccupiedMap : IDisposable
{
	// Token: 0x06005E3D RID: 24125 RVA: 0x0024C4A0 File Offset: 0x0024A6A0
	public FileBackedDecoOccupiedMap(int _worldWidth, int _worldHeight)
	{
		this.width = _worldWidth;
		this.height = _worldHeight;
		this.heightHalf = this.height / 2;
		this.occupiedMap = new FileBackedArray<EnumDecoOccupied>(this.width * this.height);
		this.cacheLength = this.width * 128;
	}

	// Token: 0x06005E3E RID: 24126 RVA: 0x0024C4F9 File Offset: 0x0024A6F9
	[PublicizedFrom(EAccessModifier.Private)]
	public int GetDecoChunkRowCacheStart(int offset)
	{
		return offset / this.cacheLength * this.cacheLength;
	}

	// Token: 0x06005E3F RID: 24127 RVA: 0x0024C50C File Offset: 0x0024A70C
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

	// Token: 0x06005E40 RID: 24128 RVA: 0x0024C580 File Offset: 0x0024A780
	public unsafe EnumDecoOccupied Get(int _offs)
	{
		this.Cache(_offs);
		return (EnumDecoOccupied)(*this.cache.Span[_offs - this.cacheStart]);
	}

	// Token: 0x06005E41 RID: 24129 RVA: 0x0024C5B0 File Offset: 0x0024A7B0
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

	// Token: 0x06005E42 RID: 24130 RVA: 0x0024C624 File Offset: 0x0024A824
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

	// Token: 0x06005E43 RID: 24131 RVA: 0x0024C658 File Offset: 0x0024A858
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

	// Token: 0x0400490F RID: 18703
	[PublicizedFrom(EAccessModifier.Private)]
	public FileBackedArray<EnumDecoOccupied> occupiedMap;

	// Token: 0x04004910 RID: 18704
	[PublicizedFrom(EAccessModifier.Private)]
	public int width;

	// Token: 0x04004911 RID: 18705
	[PublicizedFrom(EAccessModifier.Private)]
	public int height;

	// Token: 0x04004912 RID: 18706
	[PublicizedFrom(EAccessModifier.Private)]
	public int heightHalf;

	// Token: 0x04004913 RID: 18707
	[PublicizedFrom(EAccessModifier.Private)]
	public int cacheLength;

	// Token: 0x04004914 RID: 18708
	[PublicizedFrom(EAccessModifier.Private)]
	public IBackedArrayHandle cacheHandle;

	// Token: 0x04004915 RID: 18709
	[PublicizedFrom(EAccessModifier.Private)]
	public ReadOnlyMemory<EnumDecoOccupied> cache;

	// Token: 0x04004916 RID: 18710
	[PublicizedFrom(EAccessModifier.Private)]
	public int cacheStart;

	// Token: 0x04004917 RID: 18711
	[PublicizedFrom(EAccessModifier.Private)]
	public int cacheEnd;
}
