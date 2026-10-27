using System;
using System.Collections.Generic;
using System.Text;

// Token: 0x02001484 RID: 5252
public class ObjectDuplicateChecker<T> where T : class, IEquatable<T>
{
	// Token: 0x0600A450 RID: 42064 RVA: 0x003D7E18 File Offset: 0x003D6018
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

	// Token: 0x0600A451 RID: 42065 RVA: 0x003D7ED4 File Offset: 0x003D60D4
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

	// Token: 0x04007B32 RID: 31538
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<ObjectDuplicateChecker<T>.ObjectInfo> Objects = new List<ObjectDuplicateChecker<T>.ObjectInfo>();

	// Token: 0x02001485 RID: 5253
	[PublicizedFrom(EAccessModifier.Private)]
	public class ObjectInfo
	{
		// Token: 0x04007B33 RID: 31539
		public T Value;

		// Token: 0x04007B34 RID: 31540
		public int DupeCount;

		// Token: 0x04007B35 RID: 31541
		public int SharedCount;
	}
}
