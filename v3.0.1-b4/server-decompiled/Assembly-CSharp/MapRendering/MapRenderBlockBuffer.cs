using System;
using System.IO;
using Unity.Collections;
using UnityEngine;

namespace MapRendering
{
	// Token: 0x02001A86 RID: 6790
	public class MapRenderBlockBuffer
	{
		// Token: 0x0600CC7B RID: 52347 RVA: 0x004A77B0 File Offset: 0x004A59B0
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

		// Token: 0x170018F5 RID: 6389
		// (get) Token: 0x0600CC7C RID: 52348 RVA: 0x004A78C1 File Offset: 0x004A5AC1
		public TextureFormat FormatSelf
		{
			get
			{
				return this.blockMap.format;
			}
		}

		// Token: 0x0600CC7D RID: 52349 RVA: 0x004A78CE File Offset: 0x004A5ACE
		public void ResetBlock()
		{
			this.currentBlockMapFolder = string.Empty;
			this.currentBlockMapPos = new Vector2i(int.MinValue, int.MinValue);
			this.cache.ResetTile(this.zoomLevel);
		}

		// Token: 0x0600CC7E RID: 52350 RVA: 0x004A7904 File Offset: 0x004A5B04
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

		// Token: 0x0600CC7F RID: 52351 RVA: 0x004A793C File Offset: 0x004A5B3C
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

		// Token: 0x0600CC80 RID: 52352 RVA: 0x004A7A00 File Offset: 0x004A5C00
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

		// Token: 0x0600CC81 RID: 52353 RVA: 0x004A7AA0 File Offset: 0x004A5CA0
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

		// Token: 0x0600CC82 RID: 52354 RVA: 0x004A7B38 File Offset: 0x004A5D38
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

		// Token: 0x0600CC83 RID: 52355 RVA: 0x004A7C20 File Offset: 0x004A5E20
		public NativeArray<int> GetHalfScaledNative()
		{
			if (this.zoomBuffer.format != this.blockMap.format || this.zoomBuffer.height != Constants.MapBlockSize / 2 || this.zoomBuffer.width != Constants.MapBlockSize / 2)
			{
				this.zoomBuffer.Reinitialize(Constants.MapBlockSize / 2, Constants.MapBlockSize / 2, this.blockMap.format, false);
			}
			MapRenderBlockBuffer.ScaleNative(this.blockMap, this.zoomBuffer);
			return this.zoomBuffer.GetRawTextureData<int>();
		}

		// Token: 0x0600CC84 RID: 52356 RVA: 0x004A7CB0 File Offset: 0x004A5EB0
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

		// Token: 0x0600CC85 RID: 52357 RVA: 0x004A7D48 File Offset: 0x004A5F48
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

		// Token: 0x0600CC86 RID: 52358 RVA: 0x004A7E1C File Offset: 0x004A601C
		[PublicizedFrom(EAccessModifier.Private)]
		public void saveTextureToFile()
		{
			byte[] array = this.blockMap.EncodeToPNG();
			this.cache.SaveTile(this.zoomLevel, array);
		}

		// Token: 0x04009AE0 RID: 39648
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Texture2D blockMap = new Texture2D(Constants.MapBlockSize, Constants.MapBlockSize, Constants.DefaultTextureFormat, false);

		// Token: 0x04009AE1 RID: 39649
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MapTileCache cache;

		// Token: 0x04009AE2 RID: 39650
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly NativeArray<int> emptyImageData;

		// Token: 0x04009AE3 RID: 39651
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Texture2D zoomBuffer = new Texture2D(Constants.MapBlockSize / 2, Constants.MapBlockSize / 2, Constants.DefaultTextureFormat, false);

		// Token: 0x04009AE4 RID: 39652
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int zoomLevel;

		// Token: 0x04009AE5 RID: 39653
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string folderBase;

		// Token: 0x04009AE6 RID: 39654
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector2i currentBlockMapPos = new Vector2i(int.MinValue, int.MinValue);

		// Token: 0x04009AE7 RID: 39655
		[PublicizedFrom(EAccessModifier.Private)]
		public string currentBlockMapFolder = string.Empty;
	}
}
