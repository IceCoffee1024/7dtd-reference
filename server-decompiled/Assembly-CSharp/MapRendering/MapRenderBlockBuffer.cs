using System;
using System.IO;
using Unity.Collections;
using UnityEngine;

namespace MapRendering
{
	// Token: 0x02001AAD RID: 6829
	public class MapRenderBlockBuffer
	{
		// Token: 0x0600CE04 RID: 52740 RVA: 0x004ADBA4 File Offset: 0x004ABDA4
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

		// Token: 0x17001967 RID: 6503
		// (get) Token: 0x0600CE05 RID: 52741 RVA: 0x004ADCB5 File Offset: 0x004ABEB5
		public TextureFormat FormatSelf
		{
			get
			{
				return this.blockMap.format;
			}
		}

		// Token: 0x0600CE06 RID: 52742 RVA: 0x004ADCC2 File Offset: 0x004ABEC2
		public void ResetBlock()
		{
			this.currentBlockMapFolder = string.Empty;
			this.currentBlockMapPos = new Vector2i(int.MinValue, int.MinValue);
			this.cache.ResetTile(this.zoomLevel);
		}

		// Token: 0x0600CE07 RID: 52743 RVA: 0x004ADCF8 File Offset: 0x004ABEF8
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

		// Token: 0x0600CE08 RID: 52744 RVA: 0x004ADD30 File Offset: 0x004ABF30
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

		// Token: 0x0600CE09 RID: 52745 RVA: 0x004ADDF4 File Offset: 0x004ABFF4
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

		// Token: 0x0600CE0A RID: 52746 RVA: 0x004ADE94 File Offset: 0x004AC094
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

		// Token: 0x0600CE0B RID: 52747 RVA: 0x004ADF2C File Offset: 0x004AC12C
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

		// Token: 0x0600CE0C RID: 52748 RVA: 0x004AE014 File Offset: 0x004AC214
		public NativeArray<int> GetHalfScaledNative()
		{
			if (this.zoomBuffer.format != this.blockMap.format || this.zoomBuffer.height != Constants.MapBlockSize / 2 || this.zoomBuffer.width != Constants.MapBlockSize / 2)
			{
				this.zoomBuffer.Reinitialize(Constants.MapBlockSize / 2, Constants.MapBlockSize / 2, this.blockMap.format, false);
			}
			MapRenderBlockBuffer.ScaleNative(this.blockMap, this.zoomBuffer);
			return this.zoomBuffer.GetRawTextureData<int>();
		}

		// Token: 0x0600CE0D RID: 52749 RVA: 0x004AE0A4 File Offset: 0x004AC2A4
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

		// Token: 0x0600CE0E RID: 52750 RVA: 0x004AE13C File Offset: 0x004AC33C
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

		// Token: 0x0600CE0F RID: 52751 RVA: 0x004AE210 File Offset: 0x004AC410
		[PublicizedFrom(EAccessModifier.Private)]
		public void saveTextureToFile()
		{
			byte[] array = this.blockMap.EncodeToPNG();
			this.cache.SaveTile(this.zoomLevel, array);
		}

		// Token: 0x04009C11 RID: 39953
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Texture2D blockMap = new Texture2D(Constants.MapBlockSize, Constants.MapBlockSize, Constants.DefaultTextureFormat, false);

		// Token: 0x04009C12 RID: 39954
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MapTileCache cache;

		// Token: 0x04009C13 RID: 39955
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly NativeArray<int> emptyImageData;

		// Token: 0x04009C14 RID: 39956
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Texture2D zoomBuffer = new Texture2D(Constants.MapBlockSize / 2, Constants.MapBlockSize / 2, Constants.DefaultTextureFormat, false);

		// Token: 0x04009C15 RID: 39957
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int zoomLevel;

		// Token: 0x04009C16 RID: 39958
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string folderBase;

		// Token: 0x04009C17 RID: 39959
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector2i currentBlockMapPos = new Vector2i(int.MinValue, int.MinValue);

		// Token: 0x04009C18 RID: 39960
		[PublicizedFrom(EAccessModifier.Private)]
		public string currentBlockMapFolder = string.Empty;
	}
}
