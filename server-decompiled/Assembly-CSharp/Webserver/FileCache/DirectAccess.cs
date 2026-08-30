using System;
using System.IO;

namespace Webserver.FileCache
{
	// Token: 0x02001B09 RID: 6921
	public class DirectAccess : AbstractCache
	{
		// Token: 0x0600CFFC RID: 53244 RVA: 0x004BB704 File Offset: 0x004B9904
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

		// Token: 0x0600CFFD RID: 53245 RVA: 0x004AF36C File Offset: 0x004AD56C
		public override ValueTuple<int, int> Invalidate()
		{
			return new ValueTuple<int, int>(0, 0);
		}
	}
}
