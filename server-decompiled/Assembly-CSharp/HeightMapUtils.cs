using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020013FF RID: 5119
public static class HeightMapUtils
{
	// Token: 0x0600A0AF RID: 41135 RVA: 0x003C4D8C File Offset: 0x003C2F8C
	public static bool TryGetHeightDataFromResources(string levelName, out float[,] heightData)
	{
		Texture2D texture2D = Resources.Load("Data/Worlds/" + levelName + "/dtm", typeof(Texture2D)) as Texture2D;
		if (texture2D == null)
		{
			heightData = null;
			return false;
		}
		Resources.UnloadAsset(texture2D);
		heightData = HeightMapUtils.ConvertDTMToHeightData(texture2D, false);
		return true;
	}

	// Token: 0x0600A0B0 RID: 41136 RVA: 0x003C4DDC File Offset: 0x003C2FDC
	public static float[,] GetHeightDataFromWorld(string worldPath, bool loadPNG = true)
	{
		string text = worldPath + "/dtm";
		Texture2D texture2D;
		if (loadPNG && SdFile.Exists(text + ".png"))
		{
			texture2D = TextureUtils.LoadTexture(text + ".png", FilterMode.Point, false, false, null);
		}
		else
		{
			if (!SdFile.Exists(text + ".tga"))
			{
				throw new FileNotFoundException();
			}
			texture2D = TextureUtils.LoadTexture(text + ".tga", FilterMode.Point, false, false, null);
		}
		float[,] array = HeightMapUtils.ConvertDTMToHeightData(texture2D, false);
		global::UnityEngine.Object.Destroy(texture2D);
		return array;
	}

	// Token: 0x0600A0B1 RID: 41137 RVA: 0x003C4E60 File Offset: 0x003C3060
	public static float[,] GetHeightDataFromImageFile(string filePath)
	{
		if (!SdFile.Exists(filePath))
		{
			throw new FileNotFoundException();
		}
		Texture2D texture2D = TextureUtils.LoadTexture(filePath, FilterMode.Point, false, false, null);
		float[,] array = HeightMapUtils.ConvertDTMToHeightData(texture2D, false);
		global::UnityEngine.Object.Destroy(texture2D);
		return array;
	}

	// Token: 0x0600A0B2 RID: 41138 RVA: 0x003C4E94 File Offset: 0x003C3094
	public static float[,] ConvertDTMToHeightData(Color[] dtmPixs, int dtmSize, bool _bFlip = false)
	{
		float[,] array = new float[dtmSize, dtmSize];
		if (!_bFlip)
		{
			for (int i = 0; i < dtmSize; i++)
			{
				for (int j = 0; j < dtmSize; j++)
				{
					array[i, j] = dtmPixs[i + j * dtmSize].r * 255f;
				}
			}
		}
		else
		{
			for (int k = 0; k < dtmSize; k++)
			{
				for (int l = 0; l < dtmSize; l++)
				{
					array[k, dtmSize - l - 1] = dtmPixs[k + l * dtmSize].r * 255f;
				}
			}
		}
		return array;
	}

	// Token: 0x0600A0B3 RID: 41139 RVA: 0x003C4F28 File Offset: 0x003C3128
	public static float[,] ConvertDTMToHeightData(Texture2D dtm, bool _bFlip = false)
	{
		Color[] pixels = dtm.GetPixels();
		float[,] array = new float[dtm.width, dtm.height];
		if (!_bFlip)
		{
			for (int i = 0; i < dtm.width; i++)
			{
				for (int j = 0; j < dtm.height; j++)
				{
					array[i, j] = pixels[i + j * dtm.width].r * 255f;
				}
			}
		}
		else
		{
			int height = dtm.height;
			int width = dtm.width;
			for (int k = 0; k < width; k++)
			{
				for (int l = 0; l < height; l++)
				{
					array[k, height - l - 1] = pixels[k + l * dtm.width].r * 255f;
				}
			}
		}
		return array;
	}

	// Token: 0x0600A0B4 RID: 41140 RVA: 0x003C4FF8 File Offset: 0x003C31F8
	public static float[,] ConvertDTMToTerrainStampData(Texture2D dtm)
	{
		Color[] pixels = dtm.GetPixels();
		float[,] array = new float[dtm.width, dtm.height];
		for (int i = 0; i < dtm.width; i++)
		{
			for (int j = 0; j < dtm.height; j++)
			{
				array[i, j] = pixels[i + j * dtm.width].r;
			}
		}
		return array;
	}

	// Token: 0x0600A0B5 RID: 41141 RVA: 0x003C5060 File Offset: 0x003C3260
	public static float[,] ConvertDTMToHeightData(Color32[] dtmPixs, int w, int h, bool _bFlip = false)
	{
		float[,] array = new float[w, h];
		if (!_bFlip)
		{
			for (int i = 0; i < w; i++)
			{
				for (int j = 0; j < h; j++)
				{
					array[i, j] = (float)dtmPixs[i + j * w].r;
					if (dtmPixs[i + j * w].r != dtmPixs[i + j * w].g)
					{
						array[i, j] += (float)dtmPixs[i + j * w].g / 255f;
					}
				}
			}
		}
		else
		{
			for (int k = 0; k < w; k++)
			{
				for (int l = 0; l < h; l++)
				{
					array[k, h - l - 1] = (float)dtmPixs[k + l * w].r;
					if (dtmPixs[k + l * w].r != dtmPixs[k + l * w].g)
					{
						array[k, h - l - 1] += (float)dtmPixs[k + l * w].g / 255f;
					}
				}
			}
		}
		return array;
	}

	// Token: 0x0600A0B6 RID: 41142 RVA: 0x003C5184 File Offset: 0x003C3384
	public static float[,] LoadRAWToHeightData(string _filePath)
	{
		ushort[] array = HeightMapUtils.LoadHeightMapRAW(_filePath);
		int num = (int)Mathf.Sqrt((float)array.Length);
		int num2 = num;
		float[,] array2 = new float[num, num2];
		for (int i = num - 1; i >= 0; i--)
		{
			for (int j = num2 - 1; j >= 0; j--)
			{
				ushort num3 = array[i + j * num];
				array2[i, j] = (float)num3 / 65280f * 255f;
			}
		}
		return array2;
	}

	// Token: 0x0600A0B7 RID: 41143 RVA: 0x003C51F8 File Offset: 0x003C33F8
	public static ushort[] LoadHeightMapRAW(string _filePath)
	{
		ushort[] array3;
		using (BufferedStream bufferedStream = new BufferedStream(SdFile.OpenRead(_filePath)))
		{
			int num = (int)Mathf.Sqrt((float)bufferedStream.Length);
			byte[] array = new byte[8192];
			ushort[] array2 = new ushort[bufferedStream.Length / 2L];
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			while ((long)num4 < bufferedStream.Length)
			{
				int num5 = bufferedStream.Read(array, 0, array.Length);
				num4 += num5;
				int i = 0;
				int num6 = num2 + num3 * num;
				while (i < num5)
				{
					byte b = array[i];
					byte b2 = array[i + 1];
					array2[num6++] = (ushort)((int)b2 * 256 + (int)b);
					num2++;
					if (num2 >= num)
					{
						num2 = 0;
						num3++;
						num6 = num2 + num3 * num;
					}
					i += 2;
				}
			}
			array3 = array2;
		}
		return array3;
	}

	// Token: 0x0600A0B8 RID: 41144 RVA: 0x003C52E0 File Offset: 0x003C34E0
	public unsafe static IBackedArray<ushort> LoadHeightMapRAW(string _filePath, int w, int h, float _fac = 1f, int _clampHeight = 1)
	{
		_clampHeight *= 256;
		if (_clampHeight <= 0)
		{
			_clampHeight = int.MaxValue;
		}
		ushort num = (ushort)_clampHeight;
		IBackedArray<ushort> backedArray2;
		using (Stream stream = SdFile.OpenRead(_filePath))
		{
			int num2 = (int)stream.Length / 2 * 2;
			IBackedArray<ushort> backedArray = BackedArrays.Create<ushort>(w * h);
			int i = 0;
			while (i < num2)
			{
				int num3 = i / 2;
				int num4 = Math.Min(num2 - i, 1048576);
				int num5 = num4 / 2;
				Span<ushort> span2;
				using (backedArray.GetSpan(num3, num5, out span2))
				{
					Span<byte> span3 = MemoryMarshal.Cast<ushort, byte>(span2);
					int j;
					int num6;
					for (j = 0; j < num4; j += num6)
					{
						num6 = stream.Read(span3.Slice(j, num4 - j));
						if (num6 <= 0)
						{
							throw new IOException(string.Format("Unexpected end of stream. Expected {0} bytes but only read {1} bytes.", num2, i));
						}
					}
					int num7 = j / 2;
					for (int k = 0; k < num7; k++)
					{
						if ((int)(*span2[k]) > _clampHeight)
						{
							*span2[k] = num;
						}
					}
					i += j;
				}
			}
			backedArray2 = backedArray;
		}
		return backedArray2;
	}

	// Token: 0x0600A0B9 RID: 41145 RVA: 0x003C5424 File Offset: 0x003C3624
	public static float[,] LoadHeightMapRAWAsUnityHeightMap(string _filePath, int w, int h, float _fac = 1f)
	{
		float[,] array3;
		using (BufferedStream bufferedStream = new BufferedStream(SdFile.OpenRead(_filePath)))
		{
			byte[] array = new byte[8192];
			float[,] array2 = new float[h, w];
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			while ((long)num3 < bufferedStream.Length)
			{
				int num4 = bufferedStream.Read(array, 0, array.Length);
				num3 += num4;
				for (int i = 0; i < num4; i += 2)
				{
					byte b = array[i];
					byte b2 = array[i + 1];
					array2[num2, num] = (float)((int)b2 * 256 + (int)b) / 65535f;
					num++;
					if (num >= w)
					{
						num = 0;
						num2++;
					}
				}
			}
			array3 = array2;
		}
		return array3;
	}

	// Token: 0x0600A0BA RID: 41146 RVA: 0x003C54E4 File Offset: 0x003C36E4
	public static float[,] LoadHeightMapRAWAsStampData(string _filePath, float multiplier = 1f)
	{
		float[,] array3;
		using (BufferedStream bufferedStream = new BufferedStream(SdFile.OpenRead(_filePath)))
		{
			byte[] array = new byte[8192];
			int num = (int)Mathf.Sqrt((float)(bufferedStream.Length / 2L));
			float[,] array2 = new float[num, num];
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			while ((long)num4 < bufferedStream.Length)
			{
				int num5 = bufferedStream.Read(array, 0, array.Length);
				num4 += num5;
				for (int i = 0; i < num5; i += 2)
				{
					byte b = array[i];
					byte b2 = array[i + 1];
					array2[num3, num2] = (float)((int)b2 * 256 + (int)b) / 65535f * multiplier;
					num2++;
					if (num2 >= num)
					{
						num2 = 0;
						num3++;
					}
				}
			}
			array3 = array2;
		}
		return array3;
	}

	// Token: 0x0600A0BB RID: 41147 RVA: 0x003C55C0 File Offset: 0x003C37C0
	public static void SaveHeightMapRAW(string _filePath, int w, int h, float[] _data)
	{
		using (BufferedStream bufferedStream = new BufferedStream(SdFile.OpenWrite(_filePath)))
		{
			int num = w * 2;
			byte[] array = new byte[num];
			for (int i = 0; i < h; i++)
			{
				int num2 = i * w;
				for (int j = 0; j < w; j++)
				{
					uint num3 = (uint)(_data[num2 + j] * 257f);
					array[2 * j] = (byte)num3;
					array[2 * j + 1] = (byte)(num3 >> 8);
				}
				bufferedStream.Write(array, 0, num);
			}
		}
	}

	// Token: 0x0600A0BC RID: 41148 RVA: 0x003C5650 File Offset: 0x003C3850
	public static void SaveHeightMapRAW(string _filePath, int w, int h, IBackedArray<ushort> _data)
	{
		using (Stream stream = SdFile.OpenWrite(_filePath))
		{
			int num = w * h;
			int i = 0;
			while (i < num)
			{
				int num2 = Math.Min(524288, num - i);
				ReadOnlySpan<ushort> readOnlySpan2;
				using (_data.GetReadOnlySpan(i, num2, out readOnlySpan2))
				{
					ReadOnlySpan<byte> readOnlySpan3 = MemoryMarshal.Cast<ushort, byte>(readOnlySpan2);
					stream.Write(readOnlySpan3);
					i += num2;
				}
			}
		}
	}

	// Token: 0x0600A0BD RID: 41149 RVA: 0x003C56D8 File Offset: 0x003C38D8
	public static void SaveHeightMapRAW(string _filePath, int w, int h, float[,] _data)
	{
		using (BufferedStream bufferedStream = new BufferedStream(SdFile.OpenWrite(_filePath)))
		{
			int num = w * 2;
			byte[] array = new byte[num];
			for (int i = 0; i < h; i++)
			{
				for (int j = 0; j < w; j++)
				{
					uint num2 = (uint)(_data[j, i] * 257f);
					array[2 * j] = (byte)num2;
					array[2 * j + 1] = (byte)(num2 >> 8);
				}
				bufferedStream.Write(array, 0, num);
			}
		}
	}

	// Token: 0x0600A0BE RID: 41150 RVA: 0x003C5768 File Offset: 0x003C3968
	public static void SaveHeightMapRAW(Stream _stream, float[] _data, float _offset)
	{
		for (int i = 0; i < _data.Length; i++)
		{
			int num = (int)((_data[i] + _offset) * 257f);
			num = Utils.FastClamp(num, 0, 65535);
			_stream.WriteByte((byte)num);
			_stream.WriteByte((byte)(num >> 8));
		}
	}

	// Token: 0x0600A0BF RID: 41151 RVA: 0x003C57B0 File Offset: 0x003C39B0
	public static float[,] SmoothTerrain(int Passes, float[,] HeightData)
	{
		int length = HeightData.GetLength(0);
		int length2 = HeightData.GetLength(1);
		float[,] array = new float[length, length2];
		while (Passes > 0)
		{
			Passes--;
			for (int i = 0; i < length; i++)
			{
				for (int j = 0; j < length2; j++)
				{
					int num = 0;
					float num2 = 0f;
					if (i - 1 > 0)
					{
						num2 += HeightData[i - 1, j];
						num++;
						if (j - 1 > 0)
						{
							num2 += HeightData[i - 1, j - 1];
							num++;
						}
						if (j + 1 < length2)
						{
							num2 += HeightData[i - 1, j + 1];
							num++;
						}
					}
					if (i + 1 < length)
					{
						num2 += HeightData[i + 1, j];
						num++;
						if (j - 1 > 0)
						{
							num2 += HeightData[i + 1, j - 1];
							num++;
						}
						if (j + 1 < length2)
						{
							num2 += HeightData[i + 1, j + 1];
							num++;
						}
					}
					if (j - 1 > 0)
					{
						num2 += HeightData[i, j - 1];
						num++;
					}
					if (j + 1 < length2)
					{
						num2 += HeightData[i, j + 1];
						num++;
					}
					array[i, j] = (HeightData[i, j] + num2 / (float)num) * 0.5f;
				}
			}
			float[,] array2 = array;
			array = HeightData;
			HeightData = array2;
		}
		return HeightData;
	}

	// Token: 0x0600A0C0 RID: 41152 RVA: 0x003C592C File Offset: 0x003C3B2C
	public static float[,][,] ConvertAndSliceUnityHeightmap(ushort[] _rawHeightMap, int _heightMapWidth, int _heightMapHeight, int _sliceAtPix, int _resStep)
	{
		int num = _heightMapWidth / _sliceAtPix;
		int num2 = _heightMapHeight / _sliceAtPix;
		float[,][,] array = new float[num, num2][,];
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				float[,] array2 = new float[_sliceAtPix / _resStep + 1, _sliceAtPix / _resStep + 1];
				for (int k = _sliceAtPix; k >= 0; k -= _resStep)
				{
					for (int l = _sliceAtPix; l >= 0; l -= _resStep)
					{
						int num3 = j * _sliceAtPix + k;
						int num4 = i * _sliceAtPix + l;
						if (num3 >= _heightMapWidth)
						{
							num3 = _heightMapWidth - 1;
						}
						if (num4 >= _heightMapHeight)
						{
							num4 = _heightMapHeight - 1;
						}
						ushort num5 = _rawHeightMap[num3 + num4 * _heightMapWidth];
						array2[l / _resStep, k / _resStep] = (float)num5 / 65280f;
					}
				}
				array[j, i] = array2;
			}
		}
		return array;
	}

	// Token: 0x0600A0C1 RID: 41153 RVA: 0x003C59FC File Offset: 0x003C3BFC
	public static float[,][,] ConvertAndSliceUnityHeightmapQuartered(IBackedArray<ushort> _rawHeightMap, int _heightMapWidth, int _heightMapHeight, int _sliceAtPix)
	{
		int num = _heightMapWidth / _sliceAtPix;
		int num2 = _heightMapHeight / _sliceAtPix;
		float[,][,] array = new float[num, num2][,];
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				array[j, i] = HeightMapUtils.ConvertUnityHeightmapSliceQuartered(_rawHeightMap, _heightMapWidth, _heightMapHeight, j, i, _sliceAtPix);
			}
		}
		return array;
	}

	// Token: 0x0600A0C2 RID: 41154 RVA: 0x003C5A4C File Offset: 0x003C3C4C
	public unsafe static float[,] ConvertUnityHeightmapSliceQuartered(IBackedArray<ushort> _rawHeightMap, int _heightMapWidth, int _heightMapHeight, int _sliceX, int _sliceZ, int _sliceAtPix)
	{
		float[,] array = new float[_sliceAtPix / 2 + 1, _sliceAtPix / 2 + 1];
		float[,] array2;
		float* ptr;
		if ((array2 = array) == null || array2.Length == 0)
		{
			ptr = null;
		}
		else
		{
			ptr = &array2[0, 0];
		}
		Span<float> span = new Span<float>((void*)ptr, array.GetLength(0) * array.GetLength(1));
		HeightMapUtils.ConvertUnityHeightmapSliceQuartered(_rawHeightMap, _heightMapWidth, _heightMapHeight, _sliceX, _sliceZ, _sliceAtPix, span);
		array2 = null;
		return array;
	}

	// Token: 0x0600A0C3 RID: 41155 RVA: 0x003C5AB0 File Offset: 0x003C3CB0
	public static TileFile<float> ConvertAndSliceUnityHeightmapQuarteredToFile(IBackedArray<ushort> _rawHeightMap, int _heightMapWidth, int _heightMapHeight, int _sliceAtPix)
	{
		int num = _heightMapWidth / _sliceAtPix;
		int num2 = _heightMapHeight / _sliceAtPix;
		int num3 = _sliceAtPix / 2 + 1;
		FileBackedArray<float> fileBackedArray = new FileBackedArray<float>(num * num2 * num3 * num3);
		int num4 = num3 * num3;
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				int num5 = i * num4 * num + j * num4;
				Span<float> span2;
				using (fileBackedArray.GetSpan(num5, num4, out span2))
				{
					HeightMapUtils.ConvertUnityHeightmapSliceQuartered(_rawHeightMap, _heightMapWidth, _heightMapHeight, j, i, _sliceAtPix, span2);
				}
			}
		}
		return new TileFile<float>(fileBackedArray, num3, num, num2);
	}

	// Token: 0x0600A0C4 RID: 41156 RVA: 0x003C5B50 File Offset: 0x003C3D50
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe static void ConvertUnityHeightmapSliceQuartered(IBackedArray<ushort> _rawHeightMap, int _heightMapWidth, int _heightMapHeight, int _sliceX, int _sliceZ, int _sliceAtPix, Span<float> tempHeightMapData)
	{
		int num = _heightMapWidth - 1;
		int num2 = _heightMapHeight - 1;
		int num3 = Math.Max((_sliceZ * _sliceAtPix - 1) * _heightMapWidth, 0);
		int num4 = Math.Min(((_sliceZ + 1) * _sliceAtPix + 2) * _heightMapWidth, _rawHeightMap.Length) - num3;
		ReadOnlySpan<ushort> readOnlySpan2;
		using (_rawHeightMap.GetReadOnlySpan(num3, num4, out readOnlySpan2))
		{
			int num5 = _sliceAtPix / 2 + 1;
			for (int i = 0; i <= _sliceAtPix; i += 2)
			{
				int num6 = _sliceZ * _sliceAtPix + i;
				if (num6 >= num2)
				{
					num6 = _heightMapHeight - 2;
				}
				else if (num6 < 1)
				{
					num6 = 1;
				}
				int num7 = num6 * _heightMapWidth;
				for (int j = 0; j <= _sliceAtPix; j += 2)
				{
					int num8 = _sliceX * _sliceAtPix + j;
					if (num8 >= num)
					{
						num8 = _heightMapWidth - 2;
					}
					else if (num8 < 1)
					{
						num8 = 1;
					}
					int num9 = num8 + num7 - num3;
					uint num10 = (uint)(*readOnlySpan2[num9 - _heightMapWidth]);
					uint num11 = (uint)(*readOnlySpan2[num9 - 1]);
					uint num12 = (uint)(*readOnlySpan2[num9]);
					uint num13 = (uint)(*readOnlySpan2[num9 + 1]);
					uint num14 = (uint)(*readOnlySpan2[num9 + _heightMapWidth]);
					ushort num15 = (ushort)((num11 + num12 + num13 + num10 + num14) / 5U);
					int num16 = i / 2;
					int num17 = j / 2;
					*tempHeightMapData[num16 * num5 + num17] = (float)num15 / 65280f;
				}
			}
		}
	}

	// Token: 0x0600A0C5 RID: 41157 RVA: 0x003C5CB0 File Offset: 0x003C3EB0
	public static Color32[,][] ConvertAndSliceSplatmap(Color32[] _splat, int _splatMapWidth, int _splatMapHeight, int _overlapBorderPix, int _sliceAtWidth, int _resStep)
	{
		int num = _splatMapWidth / _sliceAtWidth;
		int num2 = _splatMapHeight / _sliceAtWidth;
		Color32[,][] array = new Color32[num, num2][];
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				int num3 = _sliceAtWidth / _resStep + 2 * _overlapBorderPix;
				int num4 = _sliceAtWidth / _resStep + 2 * _overlapBorderPix;
				Color32[] array2 = new Color32[num3 * num4];
				for (int k = -_resStep * _overlapBorderPix; k < _sliceAtWidth + _overlapBorderPix; k += _resStep)
				{
					for (int l = -_resStep * _overlapBorderPix; l < _sliceAtWidth + _overlapBorderPix; l += _resStep)
					{
						int num5 = j * _sliceAtWidth + k;
						int num6 = i * _sliceAtWidth + l;
						if (num5 < 0)
						{
							num5 += _splatMapHeight;
						}
						else if (num5 >= _splatMapWidth)
						{
							num5 -= _splatMapWidth;
						}
						if (num6 < 0)
						{
							num6 += _splatMapHeight;
						}
						else if (num6 >= _splatMapHeight)
						{
							num6 -= _splatMapHeight;
						}
						Color32 color = _splat[num5 + num6 * _splatMapWidth];
						array2[k / _resStep + _overlapBorderPix + (l / _resStep + _overlapBorderPix) * num3] = color;
					}
				}
				array[j, i] = array2;
			}
		}
		return array;
	}

	// Token: 0x040078CE RID: 30926
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cGameHeightToU16Scale = 257f;
}
