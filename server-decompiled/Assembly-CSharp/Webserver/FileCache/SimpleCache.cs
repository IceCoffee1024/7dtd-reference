using System;
using System.Collections.Generic;
using System.IO;

namespace Webserver.FileCache
{
	// Token: 0x02001B68 RID: 7016
	public class SimpleCache : AbstractCache
	{
		// Token: 0x0600D270 RID: 53872 RVA: 0x004C37A8 File Offset: 0x004C19A8
		public override byte[] GetFileContent(string _filename)
		{
			try
			{
				Dictionary<string, byte[]> dictionary = this.fileCache;
				lock (dictionary)
				{
					byte[] array;
					if (this.fileCache.TryGetValue(_filename, out array))
					{
						return array;
					}
					if (!File.Exists(_filename))
					{
						return null;
					}
					byte[] array2 = File.ReadAllBytes(_filename);
					this.fileCache.Add(_filename, array2);
					return array2;
				}
			}
			catch (Exception ex)
			{
				Log.Out(string.Format("Error in SimpleCache.GetFileContent: {0}", ex));
			}
			return null;
		}

		// Token: 0x0600D271 RID: 53873 RVA: 0x004C3840 File Offset: 0x004C1A40
		public override ValueTuple<int, int> Invalidate()
		{
			ValueTuple<int, int> valueTuple = new ValueTuple<int, int>(0, 0);
			Dictionary<string, byte[]> dictionary = this.fileCache;
			lock (dictionary)
			{
				valueTuple.Item1 = this.fileCache.Count;
				foreach (KeyValuePair<string, byte[]> keyValuePair in this.fileCache)
				{
					string text;
					byte[] array;
					keyValuePair.Deconstruct(out text, out array);
					byte[] array2 = array;
					valueTuple.Item2 += array2.Length;
				}
				this.fileCache.Clear();
			}
			return valueTuple;
		}

		// Token: 0x04009FA0 RID: 40864
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, byte[]> fileCache = new Dictionary<string, byte[]>();
	}
}
