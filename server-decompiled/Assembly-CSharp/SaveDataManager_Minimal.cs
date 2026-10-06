using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x02000A33 RID: 2611
public class SaveDataManager_Minimal : SaveDataManagerBase
{
	// Token: 0x17000889 RID: 2185
	// (get) Token: 0x06004E57 RID: 20055 RVA: 0x001E0455 File Offset: 0x001DE655
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

	// Token: 0x06004E58 RID: 20056 RVA: 0x001E0470 File Offset: 0x001DE670
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

	// Token: 0x06004E59 RID: 20057 RVA: 0x001E053C File Offset: 0x001DE73C
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

	// Token: 0x04003CBF RID: 15551
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly bool concurrencyCheckEnabled = true;

	// Token: 0x04003CC0 RID: 15552
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<SaveDataManagedPath, int> openCountMap = new Dictionary<SaveDataManagedPath, int>();

	// Token: 0x04003CC1 RID: 15553
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Stream, SaveDataManagedPath> streamToPathMap = new Dictionary<Stream, SaveDataManagedPath>();

	// Token: 0x04003CC2 RID: 15554
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly object lockObj = new object();

	// Token: 0x04003CC3 RID: 15555
	[PublicizedFrom(EAccessModifier.Private)]
	public static SaveDataManager_Minimal instance;
}
