using System;
using System.IO;
using UnityEngine;
using Webserver.FileCache;

namespace MapRendering
{
	// Token: 0x02001A8C RID: 6796
	public class MapTileCache : AbstractCache
	{
		// Token: 0x0600CCA9 RID: 52393 RVA: 0x004A8C3C File Offset: 0x004A6E3C
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

		// Token: 0x0600CCAA RID: 52394 RVA: 0x004A8CA8 File Offset: 0x004A6EA8
		public void SetZoomCount(int _count)
		{
			this.cache = new MapTileCache.CurrentZoomFile[_count];
			for (int i = 0; i < this.cache.Length; i++)
			{
				this.cache[i] = new MapTileCache.CurrentZoomFile();
			}
		}

		// Token: 0x0600CCAB RID: 52395 RVA: 0x004A8CE4 File Offset: 0x004A6EE4
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

		// Token: 0x0600CCAC RID: 52396 RVA: 0x004A8D98 File Offset: 0x004A6F98
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

		// Token: 0x0600CCAD RID: 52397 RVA: 0x004A8E4C File Offset: 0x004A704C
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

		// Token: 0x0600CCAE RID: 52398 RVA: 0x004A8EC4 File Offset: 0x004A70C4
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

		// Token: 0x0600CCAF RID: 52399 RVA: 0x004A8F78 File Offset: 0x004A7178
		public override ValueTuple<int, int> Invalidate()
		{
			return new ValueTuple<int, int>(0, 0);
		}

		// Token: 0x0600CCB0 RID: 52400 RVA: 0x004A8F84 File Offset: 0x004A7184
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

		// Token: 0x04009AFE RID: 39678
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] transparentTile;

		// Token: 0x04009AFF RID: 39679
		[PublicizedFrom(EAccessModifier.Private)]
		public MapTileCache.CurrentZoomFile[] cache;

		// Token: 0x02001A8D RID: 6797
		[PublicizedFrom(EAccessModifier.Private)]
		public class CurrentZoomFile
		{
			// Token: 0x04009B00 RID: 39680
			public string filename;

			// Token: 0x04009B01 RID: 39681
			public byte[] pngData;
		}
	}
}
