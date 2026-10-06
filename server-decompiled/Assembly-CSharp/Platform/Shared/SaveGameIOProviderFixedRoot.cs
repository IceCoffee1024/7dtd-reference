using System;
using System.IO;

namespace Platform.Shared
{
	// Token: 0x02001CE1 RID: 7393
	public class SaveGameIOProviderFixedRoot : SaveGameIOProvider
	{
		// Token: 0x0600DB56 RID: 56150 RVA: 0x004E68B8 File Offset: 0x004E4AB8
		public SaveGameIOProviderFixedRoot(string rootPath)
		{
			this.m_rootPath = GameIO.GetNormalizedPath(rootPath);
			Directory.CreateDirectory(this.m_rootPath);
		}

		// Token: 0x0600DB57 RID: 56151 RVA: 0x004E68D8 File Offset: 0x004E4AD8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string GetPath(SaveDataManagedPath path)
		{
			if (this.m_rootPath != null)
			{
				return GameIO.GetNormalizedPath(Path.Combine(this.m_rootPath, path.PathRelativeToRoot));
			}
			return path.GetOriginalPath();
		}

		// Token: 0x0400A5F7 RID: 42487
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string m_rootPath;
	}
}
