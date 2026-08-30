using System;
using System.Collections.Generic;
using System.IO;

namespace Webserver.FileCache
{
	// Token: 0x02001AE4 RID: 6884
	public class SimpleCache : AbstractCache
	{
		// Token: 0x0600CE7B RID: 52859 RVA: 0x004B53C0 File Offset: 0x004B35C0
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

		// Token: 0x0600CE7C RID: 52860 RVA: 0x004B5458 File Offset: 0x004B3658
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

		// Token: 0x04009CC0 RID: 40128
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, byte[]> fileCache = new Dictionary<string, byte[]>();
	}
}
