using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000A18 RID: 2584
public class SaveDataManager_Minimal : SaveDataManagerBase
{
	// Token: 0x17000857 RID: 2135
	// (get) Token: 0x06004E02 RID: 19970 RVA: 0x001DC70D File Offset: 0x001DA90D
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

	// Token: 0x06004E03 RID: 19971 RVA: 0x001DC728 File Offset: 0x001DA928
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

	// Token: 0x06004E04 RID: 19972 RVA: 0x001DC7F4 File Offset: 0x001DA9F4
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

	// Token: 0x04003C21 RID: 15393
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly bool concurrencyCheckEnabled = true;

	// Token: 0x04003C22 RID: 15394
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<SaveDataManagedPath, int> openCountMap = new Dictionary<SaveDataManagedPath, int>();

	// Token: 0x04003C23 RID: 15395
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Stream, SaveDataManagedPath> streamToPathMap = new Dictionary<Stream, SaveDataManagedPath>();

	// Token: 0x04003C24 RID: 15396
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object lockObj = new object();

	// Token: 0x04003C25 RID: 15397
	[PublicizedFrom(EAccessModifier.Private)]
	public static SaveDataManager_Minimal instance;
}
