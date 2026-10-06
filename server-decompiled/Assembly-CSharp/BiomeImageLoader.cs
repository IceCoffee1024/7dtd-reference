using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

// Token: 0x02000C4F RID: 3151
public struct BiomeImageLoader
{
	// Token: 0x06005F7F RID: 24447 RVA: 0x0025EDFC File Offset: 0x0025CFFC
	[PublicizedFrom(EAccessModifier.Private)]
	public static uint BiomeValueFromARGB32(BiomeImageLoader.BiomePixel _argb)
	{
		uint num = (uint)((uint)_argb.c2 << 16);
		uint num2 = (uint)((uint)_argb.c3 << 8);
		uint c = (uint)_argb.c4;
		return num | num2 | c;
	}

	// Token: 0x06005F80 RID: 24448 RVA: 0x0025EE28 File Offset: 0x0025D028
	[PublicizedFrom(EAccessModifier.Private)]
	public static uint BiomeValueFromRGBA32(BiomeImageLoader.BiomePixel _rgba)
	{
		uint num = (uint)((uint)_rgba.c1 << 16);
		uint num2 = (uint)((uint)_rgba.c2 << 8);
		uint c = (uint)_rgba.c3;
		return num | num2 | c;
	}

	// Token: 0x06005F81 RID: 24449 RVA: 0x0025EE54 File Offset: 0x0025D054
	public BiomeImageLoader(Texture2D _biomesTex, Dictionary<uint, BiomeDefinition> _biomeDefinitions)
	{
		if (_biomesTex.format == TextureFormat.ARGB32)
		{
			this.toBiomeValue = BiomeImageLoader.fromARGB32;
		}
		else
		{
			if (_biomesTex.format != TextureFormat.RGBA32)
			{
				throw new Exception(string.Format("Unsupported biome texture format: {0}", _biomesTex.format));
			}
			this.toBiomeValue = BiomeImageLoader.fromRGBA32;
		}
		this.biomesTex = _biomesTex;
		this.biomeDefinitions = _biomeDefinitions;
		int num = 16;
		this.biomeMap = new GridCompressedData<byte>(this.biomesTex.width, this.biomesTex.height, num, num);
		this.isError = false;
		this.lastBiomeValue = 0U;
		this.biomeId = byte.MaxValue;
	}

	// Token: 0x06005F82 RID: 24450 RVA: 0x0025EEF5 File Offset: 0x0025D0F5
	public IEnumerator Load()
	{
		this.isError = false;
		this.lastBiomeValue = 0U;
		this.biomeId = byte.MaxValue;
		MicroStopwatch msw = new MicroStopwatch(true);
		NativeArray<BiomeImageLoader.BiomePixel> biomePixs = this.biomesTex.GetPixelData<BiomeImageLoader.BiomePixel>(0);
		int blockSize = this.biomeMap.cellSizeX;
		int blockIndex = 0;
		int num5;
		for (int blockY = 0; blockY < this.biomeMap.heightCells; blockY = num5 + 1)
		{
			for (int blockX = 0; blockX < this.biomeMap.widthCells; blockX = num5 + 1)
			{
				int num = blockY * this.biomeMap.cellSizeY;
				int num2 = blockX * this.biomeMap.cellSizeX;
				int num3 = num2 + num * this.biomesTex.width;
				BiomeImageLoader.BiomePixel biomePixel = biomePixs[num2 + num * this.biomesTex.width];
				uint num4 = this.toBiomeValue(biomePixel);
				this.biomeMap.SetSameValue(blockIndex, this.GetBiomeId(num4));
				for (int i = 0; i < blockSize; i++)
				{
					for (int j = 0; j < blockSize; j++)
					{
						num3 = num2 + j + (num + i) * this.biomesTex.width;
						num4 = this.toBiomeValue(biomePixs[num3]);
						this.biomeMap.SetValue(blockIndex, j, i, this.GetBiomeId(num4));
					}
				}
				if (num3 % 8192 == 0 && msw.ElapsedMilliseconds > (long)Constants.cMaxLoadTimePerFrameMillis)
				{
					yield return null;
					msw.ResetAndRestart();
				}
				num5 = blockIndex;
				blockIndex = num5 + 1;
				num5 = blockX;
			}
			num5 = blockY;
		}
		biomePixs.Dispose();
		yield break;
	}

	// Token: 0x06005F83 RID: 24451 RVA: 0x0025EF0C File Offset: 0x0025D10C
	[PublicizedFrom(EAccessModifier.Private)]
	public byte GetBiomeId(uint iBiomeValue)
	{
		if (this.lastBiomeValue != iBiomeValue)
		{
			this.lastBiomeValue = iBiomeValue;
			BiomeDefinition biomeDefinition;
			if (this.biomeDefinitions.TryGetValue(iBiomeValue, out biomeDefinition))
			{
				this.biomeId = biomeDefinition.m_Id;
			}
		}
		return this.biomeId;
	}

	// Token: 0x06005F84 RID: 24452 RVA: 0x0025EF4C File Offset: 0x0025D14C
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 BiomeIdToColor32(uint iBiomeValue)
	{
		byte b = (byte)((iBiomeValue >> 16) & 255U);
		byte b2 = (byte)((iBiomeValue >> 8) & 255U);
		byte b3 = (byte)(iBiomeValue & 255U);
		return new Color32(b, b2, b3, 0);
	}

	// Token: 0x04004B11 RID: 19217
	[PublicizedFrom(EAccessModifier.Private)]
	public static BiomeImageLoader.PixelToBiomeValue fromARGB32 = new BiomeImageLoader.PixelToBiomeValue(BiomeImageLoader.BiomeValueFromARGB32);

	// Token: 0x04004B12 RID: 19218
	[PublicizedFrom(EAccessModifier.Private)]
	public static BiomeImageLoader.PixelToBiomeValue fromRGBA32 = new BiomeImageLoader.PixelToBiomeValue(BiomeImageLoader.BiomeValueFromRGBA32);

	// Token: 0x04004B13 RID: 19219
	[PublicizedFrom(EAccessModifier.Private)]
	public BiomeImageLoader.PixelToBiomeValue toBiomeValue;

	// Token: 0x04004B14 RID: 19220
	[PublicizedFrom(EAccessModifier.Private)]
	public Texture2D biomesTex;

	// Token: 0x04004B15 RID: 19221
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<uint, BiomeDefinition> biomeDefinitions;

	// Token: 0x04004B16 RID: 19222
	public GridCompressedData<byte> biomeMap;

	// Token: 0x04004B17 RID: 19223
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isError;

	// Token: 0x04004B18 RID: 19224
	[PublicizedFrom(EAccessModifier.Private)]
	public uint lastBiomeValue;

	// Token: 0x04004B19 RID: 19225
	[PublicizedFrom(EAccessModifier.Private)]
	public byte biomeId;

	// Token: 0x02000C50 RID: 3152
	[PublicizedFrom(EAccessModifier.Private)]
	public struct BiomePixel
	{
		// Token: 0x04004B1A RID: 19226
		public byte c1;

		// Token: 0x04004B1B RID: 19227
		public byte c2;

		// Token: 0x04004B1C RID: 19228
		public byte c3;

		// Token: 0x04004B1D RID: 19229
		public byte c4;
	}

	// Token: 0x02000C51 RID: 3153
	// (Invoke) Token: 0x06005F87 RID: 24455
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate uint PixelToBiomeValue(BiomeImageLoader.BiomePixel pix);
}
