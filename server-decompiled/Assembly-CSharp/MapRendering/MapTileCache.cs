using System;
using System.IO;
using UnityEngine;
using Webserver.FileCache;

namespace MapRendering
{
	// Token: 0x02001AB3 RID: 6835
	public class MapTileCache : AbstractCache
	{
		// Token: 0x0600CE32 RID: 52786 RVA: 0x004AF030 File Offset: 0x004AD230
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

		// Token: 0x0600CE33 RID: 52787 RVA: 0x004AF09C File Offset: 0x004AD29C
		public void SetZoomCount(int _count)
		{
			this.cache = new MapTileCache.CurrentZoomFile[_count];
			for (int i = 0; i < this.cache.Length; i++)
			{
				this.cache[i] = new MapTileCache.CurrentZoomFile();
			}
		}

		// Token: 0x0600CE34 RID: 52788 RVA: 0x004AF0D8 File Offset: 0x004AD2D8
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

		// Token: 0x0600CE35 RID: 52789 RVA: 0x004AF18C File Offset: 0x004AD38C
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

		// Token: 0x0600CE36 RID: 52790 RVA: 0x004AF240 File Offset: 0x004AD440
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

		// Token: 0x0600CE37 RID: 52791 RVA: 0x004AF2B8 File Offset: 0x004AD4B8
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

		// Token: 0x0600CE38 RID: 52792 RVA: 0x004AF36C File Offset: 0x004AD56C
		public override ValueTuple<int, int> Invalidate()
		{
			return new ValueTuple<int, int>(0, 0);
		}

		// Token: 0x0600CE39 RID: 52793 RVA: 0x004AF378 File Offset: 0x004AD578
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

		// Token: 0x04009C2F RID: 39983
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] transparentTile;

		// Token: 0x04009C30 RID: 39984
		[PublicizedFrom(EAccessModifier.Private)]
		public MapTileCache.CurrentZoomFile[] cache;

		// Token: 0x02001AB4 RID: 6836
		[PublicizedFrom(EAccessModifier.Private)]
		public class CurrentZoomFile
		{
			// Token: 0x04009C31 RID: 39985
			public string filename;

			// Token: 0x04009C32 RID: 39986
			public byte[] pngData;
		}
	}
}
