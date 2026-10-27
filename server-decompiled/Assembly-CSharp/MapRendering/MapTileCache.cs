using System;
using System.IO;
using UnityEngine;
using Webserver.FileCache;

namespace MapRendering
{
	// Token: 0x02001B10 RID: 6928
	public class MapTileCache : AbstractCache
	{
		// Token: 0x0600D09E RID: 53406 RVA: 0x004B6FF8 File Offset: 0x004B51F8
		public MapTileCache(int _tileSize)
		{
			Texture2D texture2D = new Texture2D(_tileSize, _tileSize);
			Color color = new Color(0f, 0f, 0f, 0f);
			for (int i = 0; i < _tileSize; i++)
			{
				for (int j = 0; j < _tileSize; j++)
				{
					texture2D.SetPixel(i, j, color);
				}
			}
			this.transparentTile = texture2D.EncodeToPNG();
			global::UnityEngine.Object.Destroy(texture2D);
		}

		// Token: 0x0600D09F RID: 53407 RVA: 0x004B7064 File Offset: 0x004B5264
		public void SetZoomCount(int _count)
		{
			this.cache = new MapTileCache.CurrentZoomFile[_count];
			for (int i = 0; i < this.cache.Length; i++)
			{
				this.cache[i] = new MapTileCache.CurrentZoomFile();
			}
		}

		// Token: 0x0600D0A0 RID: 53408 RVA: 0x004B70A0 File Offset: 0x004B52A0
		public byte[] LoadTile(int _zoomlevel, string _filename)
		{
			try
			{
				MapTileCache.CurrentZoomFile[] array = this.cache;
				lock (array)
				{
					MapTileCache.CurrentZoomFile currentZoomFile = this.cache[_zoomlevel];
					if (currentZoomFile.filename != null && currentZoomFile.filename.Equals(_filename))
					{
						return currentZoomFile.pngData;
					}
					currentZoomFile.filename = _filename;
					if (!File.Exists(_filename))
					{
						currentZoomFile.pngData = null;
						return null;
					}
					currentZoomFile.pngData = MapTileCache.ReadAllBytes(_filename);
					return currentZoomFile.pngData;
				}
			}
			catch (Exception ex)
			{
				Log.Warning(string.Format("Error in MapTileCache.LoadTile: {0}", ex));
			}
			return null;
		}

		// Token: 0x0600D0A1 RID: 53409 RVA: 0x004B7154 File Offset: 0x004B5354
		public void SaveTile(int _zoomlevel, byte[] _contentPng)
		{
			try
			{
				MapTileCache.CurrentZoomFile[] array = this.cache;
				lock (array)
				{
					MapTileCache.CurrentZoomFile currentZoomFile = this.cache[_zoomlevel];
					string filename = currentZoomFile.filename;
					if (!string.IsNullOrEmpty(filename))
					{
						currentZoomFile.pngData = _contentPng;
						using (Stream stream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4096))
						{
							stream.Write(_contentPng, 0, _contentPng.Length);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.Warning(string.Format("Error in MapTileCache.SaveTile: {0}", ex));
			}
		}

		// Token: 0x0600D0A2 RID: 53410 RVA: 0x004B7208 File Offset: 0x004B5408
		public void ResetTile(int _zoomlevel)
		{
			try
			{
				MapTileCache.CurrentZoomFile[] array = this.cache;
				lock (array)
				{
					this.cache[_zoomlevel].filename = null;
					this.cache[_zoomlevel].pngData = null;
				}
			}
			catch (Exception ex)
			{
				Log.Warning(string.Format("Error in MapTileCache.ResetTile: {0}", ex));
			}
		}

		// Token: 0x0600D0A3 RID: 53411 RVA: 0x004B7280 File Offset: 0x004B5480
		public override byte[] GetFileContent(string _filename)
		{
			try
			{
				MapTileCache.CurrentZoomFile[] array = this.cache;
				lock (array)
				{
					foreach (MapTileCache.CurrentZoomFile currentZoomFile in this.cache)
					{
						if (currentZoomFile.filename != null && currentZoomFile.filename.Equals(_filename))
						{
							return currentZoomFile.pngData;
						}
					}
					return (!File.Exists(_filename)) ? this.transparentTile : MapTileCache.ReadAllBytes(_filename);
				}
			}
			catch (Exception ex)
			{
				Log.Warning(string.Format("Error in MapTileCache.GetFileContent: {0}", ex));
			}
			return null;
		}

		// Token: 0x0600D0A4 RID: 53412 RVA: 0x004B7334 File Offset: 0x004B5534
		public override ValueTuple<int, int> Invalidate()
		{
			return new ValueTuple<int, int>(0, 0);
		}

		// Token: 0x0600D0A5 RID: 53413 RVA: 0x004B7340 File Offset: 0x004B5540
		[PublicizedFrom(EAccessModifier.Private)]
		public static byte[] ReadAllBytes(string _path)
		{
			byte[] array2;
			using (FileStream fileStream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096))
			{
				int num = 0;
				int i = (int)fileStream.Length;
				byte[] array = new byte[i];
				while (i > 0)
				{
					int num2 = fileStream.Read(array, num, i);
					if (num2 == 0)
					{
						throw new IOException("Unexpected end of stream");
					}
					num += num2;
					i -= num2;
				}
				array2 = array;
			}
			return array2;
		}

		// Token: 0x04009DDE RID: 40414
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] transparentTile;

		// Token: 0x04009DDF RID: 40415
		[PublicizedFrom(EAccessModifier.Private)]
		public MapTileCache.CurrentZoomFile[] cache;

		// Token: 0x02001B11 RID: 6929
		[PublicizedFrom(EAccessModifier.Private)]
		public class CurrentZoomFile
		{
			// Token: 0x04009DE0 RID: 40416
			public string filename;

			// Token: 0x04009DE1 RID: 40417
			public byte[] pngData;
		}
	}
}
