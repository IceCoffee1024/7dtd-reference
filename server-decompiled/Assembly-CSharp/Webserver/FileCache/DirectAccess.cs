using System;
using System.IO;

namespace Webserver.FileCache
{
	// Token: 0x02001B66 RID: 7014
	public class DirectAccess : AbstractCache
	{
		// Token: 0x0600D268 RID: 53864 RVA: 0x004C36F8 File Offset: 0x004C18F8
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

		// Token: 0x0600D269 RID: 53865 RVA: 0x004B7334 File Offset: 0x004B5534
		public override ValueTuple<int, int> Invalidate()
		{
			return new ValueTuple<int, int>(0, 0);
		}
	}
}
