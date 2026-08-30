using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000A29 RID: 2601
public class SaveDataManager_Minimal : SaveDataManagerBase
{
	// Token: 0x17000869 RID: 2153
	// (get) Token: 0x06004E88 RID: 20104 RVA: 0x001DF1A5 File Offset: 0x001DD3A5
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

	// Token: 0x06004E89 RID: 20105 RVA: 0x001DF1C0 File Offset: 0x001DD3C0
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

	// Token: 0x06004E8A RID: 20106 RVA: 0x001DF28C File Offset: 0x001DD48C
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

	// Token: 0x04003CB2 RID: 15538
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly bool concurrencyCheckEnabled = true;

	// Token: 0x04003CB3 RID: 15539
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<SaveDataManagedPath, int> openCountMap = new Dictionary<SaveDataManagedPath, int>();

	// Token: 0x04003CB4 RID: 15540
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Stream, SaveDataManagedPath> streamToPathMap = new Dictionary<Stream, SaveDataManagedPath>();

	// Token: 0x04003CB5 RID: 15541
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object lockObj = new object();

	// Token: 0x04003CB6 RID: 15542
	[PublicizedFrom(EAccessModifier.Private)]
	public static SaveDataManager_Minimal instance;
}
