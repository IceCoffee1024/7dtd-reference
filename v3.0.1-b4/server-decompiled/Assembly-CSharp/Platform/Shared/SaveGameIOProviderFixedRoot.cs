using System;
using System.IO;

namespace Platform.Shared
{
	// Token: 0x02001C88 RID: 7304
	public class SaveGameIOProviderFixedRoot : SaveGameIOProvider
	{
		// Token: 0x0600D846 RID: 55366 RVA: 0x004DB794 File Offset: 0x004D9994
		public SaveGameIOProviderFixedRoot(string rootPath)
		{
			this.m_rootPath = GameIO.GetNormalizedPath(rootPath);
			Directory.CreateDirectory(this.m_rootPath);
		}

		// Token: 0x0600D847 RID: 55367 RVA: 0x004DB7B4 File Offset: 0x004D99B4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string GetPath(SaveDataManagedPath path)
		{
			if (this.m_rootPath != null)
			{
				return GameIO.GetNormalizedPath(Path.Combine(this.m_rootPath, path.PathRelativeToRoot));
			}
			return path.GetOriginalPath();
		}

		// Token: 0x0400A3EF RID: 41967
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string m_rootPath;
	}
}
