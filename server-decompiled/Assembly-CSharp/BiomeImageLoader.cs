using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

// Token: 0x02000C5A RID: 3162
public struct BiomeImageLoader
{
	// Token: 0x06005FA4 RID: 24484 RVA: 0x00260324 File Offset: 0x0025E524
	[PublicizedFrom(EAccessModifier.Private)]
	public static uint BiomeValueFromARGB32(BiomeImageLoader.BiomePixel _argb)
	{
		uint num = (uint)((uint)_argb.c2 << 16);
		uint num2 = (uint)((uint)_argb.c3 << 8);
		uint c = (uint)_argb.c4;
		return num | num2 | c;
	}

	// Token: 0x06005FA5 RID: 24485 RVA: 0x00260350 File Offset: 0x0025E550
	[PublicizedFrom(EAccessModifier.Private)]
	public static uint BiomeValueFromRGBA32(BiomeImageLoader.BiomePixel _rgba)
	{
		uint num = (uint)((uint)_rgba.c1 << 16);
		uint num2 = (uint)((uint)_rgba.c2 << 8);
		uint c = (uint)_rgba.c3;
		return num | num2 | c;
	}

	// Token: 0x06005FA6 RID: 24486 RVA: 0x0026037C File Offset: 0x0025E57C
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

	// Token: 0x06005FA7 RID: 24487 RVA: 0x0026041D File Offset: 0x0025E61D
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

	// Token: 0x06005FA8 RID: 24488 RVA: 0x00260434 File Offset: 0x0025E634
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

	// Token: 0x06005FA9 RID: 24489 RVA: 0x00260474 File Offset: 0x0025E674
	[PublicizedFrom(EAccessModifier.Private)]
	public Color32 BiomeIdToColor32(uint iBiomeValue)
	{
		byte b = (byte)((iBiomeValue >> 16) & 255U);
		byte b2 = (byte)((iBiomeValue >> 8) & 255U);
		byte b3 = (byte)(iBiomeValue & 255U);
		return new Color32(b, b2, b3, 0);
	}

	// Token: 0x04004B36 RID: 19254
	[PublicizedFrom(EAccessModifier.Private)]
	public static BiomeImageLoader.PixelToBiomeValue fromARGB32 = new BiomeImageLoader.PixelToBiomeValue(BiomeImageLoader.BiomeValueFromARGB32);

	// Token: 0x04004B37 RID: 19255
	[PublicizedFrom(EAccessModifier.Private)]
	public static BiomeImageLoader.PixelToBiomeValue fromRGBA32 = new BiomeImageLoader.PixelToBiomeValue(BiomeImageLoader.BiomeValueFromRGBA32);

	// Token: 0x04004B38 RID: 19256
	[PublicizedFrom(EAccessModifier.Private)]
	public BiomeImageLoader.PixelToBiomeValue toBiomeValue;

	// Token: 0x04004B39 RID: 19257
	[PublicizedFrom(EAccessModifier.Private)]
	public Texture2D biomesTex;

	// Token: 0x04004B3A RID: 19258
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<uint, BiomeDefinition> biomeDefinitions;

	// Token: 0x04004B3B RID: 19259
	public GridCompressedData<byte> biomeMap;

	// Token: 0x04004B3C RID: 19260
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isError;

	// Token: 0x04004B3D RID: 19261
	[PublicizedFrom(EAccessModifier.Private)]
	public uint lastBiomeValue;

	// Token: 0x04004B3E RID: 19262
	[PublicizedFrom(EAccessModifier.Private)]
	public byte biomeId;

	// Token: 0x02000C5B RID: 3163
	[PublicizedFrom(EAccessModifier.Private)]
	public struct BiomePixel
	{
		// Token: 0x04004B3F RID: 19263
		public byte c1;

		// Token: 0x04004B40 RID: 19264
		public byte c2;

		// Token: 0x04004B41 RID: 19265
		public byte c3;

		// Token: 0x04004B42 RID: 19266
		public byte c4;
	}

	// Token: 0x02000C5C RID: 3164
	// (Invoke) Token: 0x06005FAC RID: 24492
	[PublicizedFrom(EAccessModifier.Private)]
	public delegate uint PixelToBiomeValue(BiomeImageLoader.BiomePixel pix);
}
