using System;
using System.IO;
using UnityEngine;
using Webserver.FileCache;

namespace MapRendering
{
	// Token: 0x02001ADF RID: 6879
	public class MapTileCache : AbstractCache
	{
		// Token: 0x0600CF8A RID: 53130 RVA: 0x004B34FC File Offset: 0x004B16FC
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

		// Token: 0x0600CF8B RID: 53131 RVA: 0x004B3568 File Offset: 0x004B1768
		public void SetZoomCount(int _count)
		{
			this.cache = new MapTileCache.CurrentZoomFile[_count];
			for (int i = 0; i < this.cache.Length; i++)
			{
				this.cache[i] = new MapTileCache.CurrentZoomFile();
			}
		}

		// Token: 0x0600CF8C RID: 53132 RVA: 0x004B35A4 File Offset: 0x004B17A4
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

		// Token: 0x0600CF8D RID: 53133 RVA: 0x004B3658 File Offset: 0x004B1858
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

		// Token: 0x0600CF8E RID: 53134 RVA: 0x004B370C File Offset: 0x004B190C
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

		// Token: 0x0600CF8F RID: 53135 RVA: 0x004B3784 File Offset: 0x004B1984
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

		// Token: 0x0600CF90 RID: 53136 RVA: 0x004B3838 File Offset: 0x004B1A38
		public override ValueTuple<int, int> Invalidate()
		{
			return new ValueTuple<int, int>(0, 0);
		}

		// Token: 0x0600CF91 RID: 53137 RVA: 0x004B3844 File Offset: 0x004B1A44
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

		// Token: 0x04009CF5 RID: 40181
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] transparentTile;

		// Token: 0x04009CF6 RID: 40182
		[PublicizedFrom(EAccessModifier.Private)]
		public MapTileCache.CurrentZoomFile[] cache;

		// Token: 0x02001AE0 RID: 6880
		[PublicizedFrom(EAccessModifier.Private)]
		public class CurrentZoomFile
		{
			// Token: 0x04009CF7 RID: 40183
			public string filename;

			// Token: 0x04009CF8 RID: 40184
			public byte[] pngData;
		}
	}
}
