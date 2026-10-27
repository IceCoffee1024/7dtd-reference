using System;
using System.IO;

namespace Platform.Shared
{
	// Token: 0x02001D12 RID: 7442
	public class SaveGameIOProviderFixedRoot : SaveGameIOProvider
	{
		// Token: 0x0600DC6A RID: 56426 RVA: 0x004EA458 File Offset: 0x004E8658
		public SaveGameIOProviderFixedRoot(string rootPath)
		{
			this.m_rootPath = GameIO.GetNormalizedPath(rootPath);
			Directory.CreateDirectory(this.m_rootPath);
		}

		// Token: 0x0600DC6B RID: 56427 RVA: 0x004EA478 File Offset: 0x004E8678
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string GetPath(SaveDataManagedPath path)
		{
			if (this.m_rootPath != null)
			{
				return GameIO.GetNormalizedPath(Path.Combine(this.m_rootPath, path.PathRelativeToRoot));
			}
			return path.GetOriginalPath();
		}

		// Token: 0x0400A6E0 RID: 42720
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string m_rootPath;
	}
}
