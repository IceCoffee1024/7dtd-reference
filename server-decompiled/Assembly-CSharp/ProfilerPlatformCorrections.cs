using System;
using System.Text;
using Unity.Profiling;

// Token: 0x0200154E RID: 5454
public static class ProfilerPlatformCorrections
{
	// Token: 0x0600AAE1 RID: 43745 RVA: 0x003FC4A1 File Offset: 0x003FA6A1
	public static IMetric Graphics(string header, string usedOrReserved)
	{
		return new ProfilerRecorderMetric
		{
			Header = header,
			recorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default)
		};
	}

	// Token: 0x0600AAE2 RID: 43746 RVA: 0x003FC4D2 File Offset: 0x003FA6D2
	public static IMetric Native(string header, string usedOrReserved)
	{
		return new ProfilerPlatformCorrections.NativeDefault(header, usedOrReserved);
	}

	// Token: 0x0600AAE3 RID: 43747 RVA: 0x003FC4DB File Offset: 0x003FA6DB
	public static IMetric TotalTracked(string header, string usedOrReserved)
	{
		return new ProfilerRecorderMetric
		{
			Header = header,
			recorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default)
		};
	}

	// Token: 0x0200154F RID: 5455
	public class NativeDefault : IMetric
	{
		// Token: 0x1700140B RID: 5131
		// (get) Token: 0x0600AAE4 RID: 43748 RVA: 0x003FC50C File Offset: 0x003FA70C
		// (set) Token: 0x0600AAE5 RID: 43749 RVA: 0x003FC514 File Offset: 0x003FA714
		public string Header { get; set; }

		// Token: 0x0600AAE6 RID: 43750 RVA: 0x003FC520 File Offset: 0x003FA720
		public NativeDefault(string header, string usedOrReserved)
		{
			this.Header = header;
			this.graphics = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default);
			this.total = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default);
			this.managed = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default);
		}

		// Token: 0x0600AAE7 RID: 43751 RVA: 0x003FC5A4 File Offset: 0x003FA7A4
		public void AppendLastValue(StringBuilder builder)
		{
			double num = this.total.LastValueAsDouble - this.graphics.LastValueAsDouble - this.managed.LastValueAsDouble;
			builder.AppendFormat("{0:F2}", num * 9.5367431640625E-07);
		}

		// Token: 0x0600AAE8 RID: 43752 RVA: 0x003FC5F1 File Offset: 0x003FA7F1
		public void Cleanup()
		{
			this.graphics.Dispose();
			this.total.Dispose();
			this.managed.Dispose();
		}

		// Token: 0x04007EDA RID: 32474
		[PublicizedFrom(EAccessModifier.Private)]
		public ProfilerRecorder graphics;

		// Token: 0x04007EDB RID: 32475
		[PublicizedFrom(EAccessModifier.Private)]
		public ProfilerRecorder total;

		// Token: 0x04007EDC RID: 32476
		[PublicizedFrom(EAccessModifier.Private)]
		public ProfilerRecorder managed;
	}

	// Token: 0x02001550 RID: 5456
	public class TotalTrackedPS5 : IMetric
	{
		// Token: 0x1700140C RID: 5132
		// (get) Token: 0x0600AAE9 RID: 43753 RVA: 0x003FC614 File Offset: 0x003FA814
		// (set) Token: 0x0600AAEA RID: 43754 RVA: 0x003FC61C File Offset: 0x003FA81C
		public string Header { get; set; }

		// Token: 0x0600AAEB RID: 43755 RVA: 0x003FC628 File Offset: 0x003FA828
		public TotalTrackedPS5(string header, string usedOrReserved)
		{
			this.Header = header;
			this.graphics = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default);
			this.total = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default);
		}

		// Token: 0x0600AAEC RID: 43756 RVA: 0x003FC688 File Offset: 0x003FA888
		public void AppendLastValue(StringBuilder builder)
		{
			double num = this.total.LastValueAsDouble - this.graphics.LastValueAsDouble;
			builder.AppendFormat("{0:F2}", num * 9.5367431640625E-07);
		}

		// Token: 0x0600AAED RID: 43757 RVA: 0x003FC6C9 File Offset: 0x003FA8C9
		public void Cleanup()
		{
			this.graphics.Dispose();
			this.total.Dispose();
		}

		// Token: 0x04007EDE RID: 32478
		[PublicizedFrom(EAccessModifier.Private)]
		public ProfilerRecorder graphics;

		// Token: 0x04007EDF RID: 32479
		[PublicizedFrom(EAccessModifier.Private)]
		public ProfilerRecorder total;
	}
}
