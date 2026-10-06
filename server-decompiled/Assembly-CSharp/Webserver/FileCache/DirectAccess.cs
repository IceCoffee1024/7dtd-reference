using System;
using System.IO;

namespace Webserver.FileCache
{
	// Token: 0x02001B35 RID: 6965
	public class DirectAccess : AbstractCache
	{
		// Token: 0x0600D154 RID: 53588 RVA: 0x004BFBD0 File Offset: 0x004BDDD0
		public override byte[] GetFileContent(string _filename)
		{
			try
			{
				return File.Exists(_filename) ? File.ReadAllBytes(_filename) : null;
			}
			catch (Exception ex)
			{
				Log.Out(string.Format("Error in DirectAccess.GetFileContent: {0}", ex));
			}
			return null;
		}

		// Token: 0x0600D155 RID: 53589 RVA: 0x004B3838 File Offset: 0x004B1A38
		public override ValueTuple<int, int> Invalidate()
		{
			return new ValueTuple<int, int>(0, 0);
		}
	}
}
