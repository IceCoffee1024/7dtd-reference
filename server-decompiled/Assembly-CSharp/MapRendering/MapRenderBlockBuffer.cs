using System;
using System.IO;
using Unity.Collections;
using UnityEngine;

namespace MapRendering
{
	// Token: 0x02001B0A RID: 6922
	public class MapRenderBlockBuffer
	{
		// Token: 0x0600D070 RID: 53360 RVA: 0x004B5B6C File Offset: 0x004B3D6C
		public MapRenderBlockBuffer(int _level, MapTileCache _cache)
		{
			this.zoomLevel = _level;
			this.cache = _cache;
			this.folderBase = string.Format("{0}/{1}/", Constants.MapDirectory, this.zoomLevel);
			Color color = new Color(0f, 0f, 0f, 0f);
			for (int i = 0; i < Constants.MapBlockSize; i++)
			{
				for (int j = 0; j < Constants.MapBlockSize; j++)
				{
					this.blockMap.SetPixel(i, j, color);
				}
			}
			NativeArray<int> rawTextureData = this.blockMap.GetRawTextureData<int>();
			this.emptyImageData = new NativeArray<int>(rawTextureData.Length, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
			rawTextureData.CopyTo(this.emptyImageData);
		}

		// Token: 0x17001A51 RID: 6737
		// (get) Token: 0x0600D071 RID: 53361 RVA: 0x004B5C7D File Offset: 0x004B3E7D
		public TextureFormat FormatSelf
		{
			get
			{
				return this.blockMap.format;
			}
		}

		// Token: 0x0600D072 RID: 53362 RVA: 0x004B5C8A File Offset: 0x004B3E8A
		public void ResetBlock()
		{
			this.currentBlockMapFolder = string.Empty;
			this.currentBlockMapPos = new Vector2i(int.MinValue, int.MinValue);
			this.cache.ResetTile(this.zoomLevel);
		}

		// Token: 0x0600D073 RID: 53363 RVA: 0x004B5CC0 File Offset: 0x004B3EC0
		public void SaveBlock()
		{
			try
			{
				this.saveTextureToFile();
			}
			catch (Exception ex)
			{
				Log.Warning(string.Format("Exception in MapRenderBlockBuffer.SaveBlock(): {0}", ex));
			}
		}

		// Token: 0x0600D074 RID: 53364 RVA: 0x004B5CF8 File Offset: 0x004B3EF8
		public bool LoadBlock(Vector2i _block)
		{
			Texture2D texture2D = this.blockMap;
			lock (texture2D)
			{
				if (this.currentBlockMapPos != _block)
				{
					string text;
					if (this.currentBlockMapPos.x != _block.x)
					{
						text = string.Format("{0}{1}/", this.folderBase, _block.x);
						Directory.CreateDirectory(text);
					}
					else
					{
						text = this.currentBlockMapFolder;
					}
					string text2 = string.Format("{0}{1}.png", text, _block.y);
					this.SaveBlock();
					this.loadTextureFromFile(text2);
					this.currentBlockMapFolder = text;
					this.currentBlockMapPos = _block;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D075 RID: 53365 RVA: 0x004B5DBC File Offset: 0x004B3FBC
		public void SetPart(Vector2i _offset, int _partSize, Color32[] _pixels)
		{
			if (_offset.x + _partSize > Constants.MapBlockSize || _offset.y + _partSize > Constants.MapBlockSize)
			{
				Log.Error(string.Format("MapBlockBuffer[{0}].SetPart ({1}, {2}, {3}) has blockMap.size ({4}/{5})", new object[]
				{
					this.zoomLevel,
					_offset,
					_partSize,
					_pixels.Length,
					Constants.MapBlockSize,
					Constants.MapBlockSize
				}));
				return;
			}
			this.blockMap.SetPixels32(_offset.x, _offset.y, _partSize, _partSize, _pixels);
		}

		// Token: 0x0600D076 RID: 53366 RVA: 0x004B5E5C File Offset: 0x004B405C
		public Color32[] GetHalfScaled()
		{
			this.zoomBuffer.Reinitialize(Constants.MapBlockSize, Constants.MapBlockSize);
			if (this.blockMap.format == this.zoomBuffer.format)
			{
				NativeArray<byte> rawTextureData = this.blockMap.GetRawTextureData<byte>();
				NativeArray<byte> rawTextureData2 = this.zoomBuffer.GetRawTextureData<byte>();
				rawTextureData.CopyTo(rawTextureData2);
			}
			else
			{
				this.zoomBuffer.SetPixels32(this.blockMap.GetPixels32());
			}
			TextureScale.Point(this.zoomBuffer, Constants.MapBlockSize / 2, Constants.MapBlockSize / 2);
			return this.zoomBuffer.GetPixels32();
		}

		// Token: 0x0600D077 RID: 53367 RVA: 0x004B5EF4 File Offset: 0x004B40F4
		public void SetPartNative(Vector2i _offset, int _partSize, NativeArray<int> _pixels)
		{
			if (_offset.x + _partSize > Constants.MapBlockSize || _offset.y + _partSize > Constants.MapBlockSize)
			{
				Log.Error(string.Format("MapBlockBuffer[{0}].SetPart ({1}, {2}, {3}) has blockMap.size ({4}/{5})", new object[]
				{
					this.zoomLevel,
					_offset,
					_partSize,
					_pixels.Length,
					Constants.MapBlockSize,
					Constants.MapBlockSize
				}));
				return;
			}
			NativeArray<int> rawTextureData = this.blockMap.GetRawTextureData<int>();
			for (int i = 0; i < _partSize; i++)
			{
				int num = _partSize * i;
				int num2 = this.blockMap.width * (_offset.y + i) + _offset.x;
				for (int j = 0; j < _partSize; j++)
				{
					rawTextureData[num2 + j] = _pixels[num + j];
				}
			}
		}

		// Token: 0x0600D078 RID: 53368 RVA: 0x004B5FDC File Offset: 0x004B41DC
		public NativeArray<int> GetHalfScaledNative()
		{
			if (this.zoomBuffer.format != this.blockMap.format || this.zoomBuffer.height != Constants.MapBlockSize / 2 || this.zoomBuffer.width != Constants.MapBlockSize / 2)
			{
				this.zoomBuffer.Reinitialize(Constants.MapBlockSize / 2, Constants.MapBlockSize / 2, this.blockMap.format, false);
			}
			MapRenderBlockBuffer.ScaleNative(this.blockMap, this.zoomBuffer);
			return this.zoomBuffer.GetRawTextureData<int>();
		}

		// Token: 0x0600D079 RID: 53369 RVA: 0x004B606C File Offset: 0x004B426C
		[PublicizedFrom(EAccessModifier.Private)]
		public static void ScaleNative(Texture2D _sourceTex, Texture2D _targetTex)
		{
			NativeArray<int> rawTextureData = _sourceTex.GetRawTextureData<int>();
			NativeArray<int> rawTextureData2 = _targetTex.GetRawTextureData<int>();
			int width = _sourceTex.width;
			float height = (float)_sourceTex.height;
			int width2 = _targetTex.width;
			int height2 = _targetTex.height;
			float num = (float)width / (float)width2;
			float num2 = height / (float)height2;
			for (int i = 0; i < height2; i++)
			{
				int num3 = (int)(num2 * (float)i) * width;
				int num4 = i * width2;
				for (int j = 0; j < width2; j++)
				{
					rawTextureData2[num4 + j] = rawTextureData[(int)((float)num3 + num * (float)j)];
				}
			}
		}

		// Token: 0x0600D07A RID: 53370 RVA: 0x004B6104 File Offset: 0x004B4304
		[PublicizedFrom(EAccessModifier.Private)]
		public void loadTextureFromFile(string _fileName)
		{
			byte[] array = this.cache.LoadTile(this.zoomLevel, _fileName);
			if (array != null && this.blockMap.LoadImage(array) && this.blockMap.height == Constants.MapBlockSize && this.blockMap.width == Constants.MapBlockSize)
			{
				return;
			}
			if (array != null)
			{
				Log.Error("Map image tile " + _fileName + " has been corrupted, recreating tile");
			}
			if (this.blockMap.format != Constants.DefaultTextureFormat || this.blockMap.height != Constants.MapBlockSize || this.blockMap.width != Constants.MapBlockSize)
			{
				this.blockMap.Reinitialize(Constants.MapBlockSize, Constants.MapBlockSize, Constants.DefaultTextureFormat, false);
			}
			this.blockMap.LoadRawTextureData<int>(this.emptyImageData);
		}

		// Token: 0x0600D07B RID: 53371 RVA: 0x004B61D8 File Offset: 0x004B43D8
		[PublicizedFrom(EAccessModifier.Private)]
		public void saveTextureToFile()
		{
			byte[] array = this.blockMap.EncodeToPNG();
			this.cache.SaveTile(this.zoomLevel, array);
		}

		// Token: 0x04009DC0 RID: 40384
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Texture2D blockMap = new Texture2D(Constants.MapBlockSize, Constants.MapBlockSize, Constants.DefaultTextureFormat, false);

		// Token: 0x04009DC1 RID: 40385
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MapTileCache cache;

		// Token: 0x04009DC2 RID: 40386
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly NativeArray<int> emptyImageData;

		// Token: 0x04009DC3 RID: 40387
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Texture2D zoomBuffer = new Texture2D(Constants.MapBlockSize / 2, Constants.MapBlockSize / 2, Constants.DefaultTextureFormat, false);

		// Token: 0x04009DC4 RID: 40388
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int zoomLevel;

		// Token: 0x04009DC5 RID: 40389
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string folderBase;

		// Token: 0x04009DC6 RID: 40390
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector2i currentBlockMapPos = new Vector2i(int.MinValue, int.MinValue);

		// Token: 0x04009DC7 RID: 40391
		[PublicizedFrom(EAccessModifier.Private)]
		public string currentBlockMapFolder = string.Empty;
	}
}
