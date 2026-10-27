using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000A3D RID: 2621
public class SaveDataManager_Minimal : SaveDataManagerBase
{
	// Token: 0x1700088F RID: 2191
	// (get) Token: 0x06004E7B RID: 20091 RVA: 0x001E153D File Offset: 0x001DF73D
	public static SaveDataManager_Minimal Instance
	{
		get
		{
			if (SaveDataManager_Minimal.instance == null)
			{
				SaveDataManager_Minimal.instance = new SaveDataManager_Minimal();
			}
			return SaveDataManager_Minimal.instance;
		}
	}

	// Token: 0x06004E7C RID: 20092 RVA: 0x001E1558 File Offset: 0x001DF758
	[PublicizedFrom(EAccessModifier.Protected)]
	public override Stream GetStream(SaveDataManagedPath path, FileMode mode, FileAccess access, FileShare share)
	{
		object obj = this.lockObj;
		Stream stream2;
		lock (obj)
		{
			Stream stream = new FileStream(path.GetOriginalPath(), mode, access, share);
			if (SaveDataManager_Minimal.concurrencyCheckEnabled)
			{
				this.streamToPathMap[stream] = path;
				if (!this.openCountMap.ContainsKey(path))
				{
					this.openCountMap[path] = 1;
				}
				else
				{
					Dictionary<SaveDataManagedPath, int> dictionary = this.openCountMap;
					dictionary[path]++;
					if (this.openCountMap[path] > 1)
					{
						Log.Error(string.Format("[SaveDataManager] Detected {0} concurrent operations on file: {1}", this.openCountMap[path], path));
					}
				}
			}
			stream2 = stream;
		}
		return stream2;
	}

	// Token: 0x06004E7D RID: 20093 RVA: 0x001E1624 File Offset: 0x001DF824
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void ReturnStream(Stream stream)
	{
		object obj = this.lockObj;
		lock (obj)
		{
			if (SaveDataManager_Minimal.concurrencyCheckEnabled)
			{
				SaveDataManagedPath saveDataManagedPath = this.streamToPathMap[stream];
				stream.Dispose();
				this.openCountMap[saveDataManagedPath] = this.openCountMap[saveDataManagedPath] - 1;
			}
			else
			{
				stream.Dispose();
			}
		}
	}

	// Token: 0x04003CE0 RID: 15584
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly bool concurrencyCheckEnabled = true;

	// Token: 0x04003CE1 RID: 15585
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<SaveDataManagedPath, int> openCountMap = new Dictionary<SaveDataManagedPath, int>();

	// Token: 0x04003CE2 RID: 15586
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Stream, SaveDataManagedPath> streamToPathMap = new Dictionary<Stream, SaveDataManagedPath>();

	// Token: 0x04003CE3 RID: 15587
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object lockObj = new object();

	// Token: 0x04003CE4 RID: 15588
	[PublicizedFrom(EAccessModifier.Private)]
	public static SaveDataManager_Minimal instance;
}
