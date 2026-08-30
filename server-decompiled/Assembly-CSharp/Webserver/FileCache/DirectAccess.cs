using System;
using System.IO;

namespace Webserver.FileCache
{
	// Token: 0x02001AE2 RID: 6882
	public class DirectAccess : AbstractCache
	{
		// Token: 0x0600CE73 RID: 52851 RVA: 0x004B5310 File Offset: 0x004B3510
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

		// Token: 0x0600CE74 RID: 52852 RVA: 0x004A8F78 File Offset: 0x004A7178
		public override ValueTuple<int, int> Invalidate()
		{
			return new ValueTuple<int, int>(0, 0);
		}
	}
}
