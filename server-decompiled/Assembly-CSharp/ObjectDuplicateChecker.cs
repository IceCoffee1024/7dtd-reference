using System;
using System.Collections.Generic;
using System.Text;

// Token: 0x02001472 RID: 5234
public class ObjectDuplicateChecker<T> where T : class, IEquatable<T>
{
	// Token: 0x0600A3F8 RID: 41976 RVA: 0x003D5A14 File Offset: 0x003D3C14
	public void AddValue(T value)
	{
		int i = 0;
		while (i < this.Objects.Count)
		{
			if (this.Objects[i].Value == value)
			{
				if (this.Objects[i].Value == value)
				{
					this.Objects[i].SharedCount++;
					return;
				}
				this.Objects[i].DupeCount++;
				return;
			}
			else
			{
				i++;
			}
		}
		this.Objects.Add(new ObjectDuplicateChecker<T>.ObjectInfo
		{
			Value = value,
			DupeCount = 0,
			SharedCount = 1
		});
	}

	// Token: 0x0600A3F9 RID: 41977 RVA: 0x003D5AD0 File Offset: 0x003D3CD0
	public string GenerateReport()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		StringBuilder stringBuilder = new StringBuilder();
		foreach (ObjectDuplicateChecker<T>.ObjectInfo objectInfo in this.Objects)
		{
			int sizeAuto = MemoryTracker.GetSizeAuto(objectInfo.Value);
			int num4 = sizeAuto + sizeAuto * objectInfo.DupeCount;
			num += num4;
			num2 += sizeAuto;
			num3 += sizeAuto * objectInfo.SharedCount;
		}
		stringBuilder.AppendLine(string.Format("    Size(in memory): {0:F2}kb, Size(est unduped): {1:F2}kb, Size(saved by sharing): {2:F2}kb", (double)num / 1024.0, (double)num2 / 1024.0, (double)num3 / 1024.0));
		return stringBuilder.ToString();
	}

	// Token: 0x04007AF1 RID: 31473
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ObjectDuplicateChecker<T>.ObjectInfo> Objects = new List<ObjectDuplicateChecker<T>.ObjectInfo>();

	// Token: 0x02001473 RID: 5235
	[PublicizedFrom(EAccessModifier.Private)]
	public class ObjectInfo
	{
		// Token: 0x04007AF2 RID: 31474
		public T Value;

		// Token: 0x04007AF3 RID: 31475
		public int DupeCount;

		// Token: 0x04007AF4 RID: 31476
		public int SharedCount;
	}
}
