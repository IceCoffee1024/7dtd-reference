using System;
using System.IO;
using Unity.Collections;
using UnityEngine;

namespace MapRendering
{
	// Token: 0x02001AD9 RID: 6873
	public class MapRenderBlockBuffer
	{
		// Token: 0x0600CF5C RID: 53084 RVA: 0x004B2070 File Offset: 0x004B0270
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

		// Token: 0x17001A09 RID: 6665
		// (get) Token: 0x0600CF5D RID: 53085 RVA: 0x004B2181 File Offset: 0x004B0381
		public TextureFormat FormatSelf
		{
			get
			{
				return this.blockMap.format;
			}
		}

		// Token: 0x0600CF5E RID: 53086 RVA: 0x004B218E File Offset: 0x004B038E
		public void ResetBlock()
		{
			this.currentBlockMapFolder = string.Empty;
			this.currentBlockMapPos = new Vector2i(int.MinValue, int.MinValue);
			this.cache.ResetTile(this.zoomLevel);
		}

		// Token: 0x0600CF5F RID: 53087 RVA: 0x004B21C4 File Offset: 0x004B03C4
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

		// Token: 0x0600CF60 RID: 53088 RVA: 0x004B21FC File Offset: 0x004B03FC
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

		// Token: 0x0600CF61 RID: 53089 RVA: 0x004B22C0 File Offset: 0x004B04C0
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

		// Token: 0x0600CF62 RID: 53090 RVA: 0x004B2360 File Offset: 0x004B0560
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

		// Token: 0x0600CF63 RID: 53091 RVA: 0x004B23F8 File Offset: 0x004B05F8
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

		// Token: 0x0600CF64 RID: 53092 RVA: 0x004B24E0 File Offset: 0x004B06E0
		public NativeArray<int> GetHalfScaledNative()
		{
			if (this.zoomBuffer.format != this.blockMap.format || this.zoomBuffer.height != Constants.MapBlockSize / 2 || this.zoomBuffer.width != Constants.MapBlockSize / 2)
			{
				this.zoomBuffer.Reinitialize(Constants.MapBlockSize / 2, Constants.MapBlockSize / 2, this.blockMap.format, false);
			}
			MapRenderBlockBuffer.ScaleNative(this.blockMap, this.zoomBuffer);
			return this.zoomBuffer.GetRawTextureData<int>();
		}

		// Token: 0x0600CF65 RID: 53093 RVA: 0x004B2570 File Offset: 0x004B0770
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

		// Token: 0x0600CF66 RID: 53094 RVA: 0x004B2608 File Offset: 0x004B0808
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

		// Token: 0x0600CF67 RID: 53095 RVA: 0x004B26DC File Offset: 0x004B08DC
		[PublicizedFrom(EAccessModifier.Private)]
		public void saveTextureToFile()
		{
			byte[] array = this.blockMap.EncodeToPNG();
			this.cache.SaveTile(this.zoomLevel, array);
		}

		// Token: 0x04009CD7 RID: 40151
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Texture2D blockMap = new Texture2D(Constants.MapBlockSize, Constants.MapBlockSize, Constants.DefaultTextureFormat, false);

		// Token: 0x04009CD8 RID: 40152
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MapTileCache cache;

		// Token: 0x04009CD9 RID: 40153
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly NativeArray<int> emptyImageData;

		// Token: 0x04009CDA RID: 40154
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Texture2D zoomBuffer = new Texture2D(Constants.MapBlockSize / 2, Constants.MapBlockSize / 2, Constants.DefaultTextureFormat, false);

		// Token: 0x04009CDB RID: 40155
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int zoomLevel;

		// Token: 0x04009CDC RID: 40156
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string folderBase;

		// Token: 0x04009CDD RID: 40157
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector2i currentBlockMapPos = new Vector2i(int.MinValue, int.MinValue);

		// Token: 0x04009CDE RID: 40158
		[PublicizedFrom(EAccessModifier.Private)]
		public string currentBlockMapFolder = string.Empty;
	}
}
