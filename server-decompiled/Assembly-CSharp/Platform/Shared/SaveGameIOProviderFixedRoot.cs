using System;
using System.IO;

namespace Platform.Shared
{
	// Token: 0x02001CAF RID: 7343
	public class SaveGameIOProviderFixedRoot : SaveGameIOProvider
	{
		// Token: 0x0600D9DC RID: 55772 RVA: 0x004E1D70 File Offset: 0x004DFF70
		public SaveGameIOProviderFixedRoot(string rootPath)
		{
			this.m_rootPath = GameIO.GetNormalizedPath(rootPath);
			Directory.CreateDirectory(this.m_rootPath);
		}

		// Token: 0x0600D9DD RID: 55773 RVA: 0x004E1D90 File Offset: 0x004DFF90
		[PublicizedFrom(EAccessModifier.Protected)]
		public override string GetPath(SaveDataManagedPath path)
		{
			if (this.m_rootPath != null)
			{
				return GameIO.GetNormalizedPath(Path.Combine(this.m_rootPath, path.PathRelativeToRoot));
			}
			return path.GetOriginalPath();
		}

		// Token: 0x0400A522 RID: 42274
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string m_rootPath;
	}
}
