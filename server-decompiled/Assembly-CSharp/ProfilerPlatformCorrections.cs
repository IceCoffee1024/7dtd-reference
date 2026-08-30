using System;
using System.Text;
using Unity.Profiling;

// Token: 0x02001511 RID: 5393
public static class ProfilerPlatformCorrections
{
	// Token: 0x0600A931 RID: 43313 RVA: 0x003F230D File Offset: 0x003F050D
	public static IMetric Graphics(string header, string usedOrReserved)
	{
		return new ProfilerRecorderMetric
		{
			Header = header,
			recorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default)
		};
	}

	// Token: 0x0600A932 RID: 43314 RVA: 0x003F233E File Offset: 0x003F053E
	public static IMetric Native(string header, string usedOrReserved)
	{
		return new ProfilerPlatformCorrections.NativeDefault(header, usedOrReserved);
	}

	// Token: 0x0600A933 RID: 43315 RVA: 0x003F2347 File Offset: 0x003F0547
	public static IMetric TotalTracked(string header, string usedOrReserved)
	{
		return new ProfilerRecorderMetric
		{
			Header = header,
			recorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default)
		};
	}

	// Token: 0x02001512 RID: 5394
	public class NativeDefault : IMetric
	{
		// Token: 0x1700138D RID: 5005
		// (get) Token: 0x0600A934 RID: 43316 RVA: 0x003F2378 File Offset: 0x003F0578
		// (set) Token: 0x0600A935 RID: 43317 RVA: 0x003F2380 File Offset: 0x003F0580
		public string Header { get; set; }

		// Token: 0x0600A936 RID: 43318 RVA: 0x003F238C File Offset: 0x003F058C
		public NativeDefault(string header, string usedOrReserved)
		{
			this.Header = header;
			this.graphics = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default);
			this.total = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default);
			this.managed = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default);
		}

		// Token: 0x0600A937 RID: 43319 RVA: 0x003F2410 File Offset: 0x003F0610
		public void AppendLastValue(StringBuilder builder)
		{
			double num = this.total.LastValueAsDouble - this.graphics.LastValueAsDouble - this.managed.LastValueAsDouble;
			builder.AppendFormat("{0:F2}", num * 9.5367431640625E-07);
		}

		// Token: 0x0600A938 RID: 43320 RVA: 0x003F245D File Offset: 0x003F065D
		public void Cleanup()
		{
			this.graphics.Dispose();
			this.total.Dispose();
			this.managed.Dispose();
		}

		// Token: 0x04007D7E RID: 32126
		[PublicizedFrom(EAccessModifier.Private)]
		public ProfilerRecorder graphics;

		// Token: 0x04007D7F RID: 32127
		[PublicizedFrom(EAccessModifier.Private)]
		public ProfilerRecorder total;

		// Token: 0x04007D80 RID: 32128
		[PublicizedFrom(EAccessModifier.Private)]
		public ProfilerRecorder managed;
	}

	// Token: 0x02001513 RID: 5395
	public class TotalTrackedPS5 : IMetric
	{
		// Token: 0x1700138E RID: 5006
		// (get) Token: 0x0600A939 RID: 43321 RVA: 0x003F2480 File Offset: 0x003F0680
		// (set) Token: 0x0600A93A RID: 43322 RVA: 0x003F2488 File Offset: 0x003F0688
		public string Header { get; set; }

		// Token: 0x0600A93B RID: 43323 RVA: 0x003F2494 File Offset: 0x003F0694
		public TotalTrackedPS5(string header, string usedOrReserved)
		{
			this.Header = header;
			this.graphics = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default);
			this.total = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default);
		}

		// Token: 0x0600A93C RID: 43324 RVA: 0x003F24F4 File Offset: 0x003F06F4
		public void AppendLastValue(StringBuilder builder)
		{
			double num = this.total.LastValueAsDouble - this.graphics.LastValueAsDouble;
			builder.AppendFormat("{0:F2}", num * 9.5367431640625E-07);
		}

		// Token: 0x0600A93D RID: 43325 RVA: 0x003F2535 File Offset: 0x003F0735
		public void Cleanup()
		{
			this.graphics.Dispose();
			this.total.Dispose();
		}

		// Token: 0x04007D82 RID: 32130
		[PublicizedFrom(EAccessModifier.Private)]
		public ProfilerRecorder graphics;

		// Token: 0x04007D83 RID: 32131
		[PublicizedFrom(EAccessModifier.Private)]
		public ProfilerRecorder total;
	}
}
