using System;
using System.Text;
using Unity.Profiling;

// Token: 0x02001560 RID: 5472
public static class ProfilerPlatformCorrections
{
	// Token: 0x0600AB39 RID: 43833 RVA: 0x003FE8A5 File Offset: 0x003FCAA5
	public static IMetric Graphics(string header, string usedOrReserved)
	{
		return new ProfilerRecorderMetric
		{
			Header = header,
			recorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default)
		};
	}

	// Token: 0x0600AB3A RID: 43834 RVA: 0x003FE8D6 File Offset: 0x003FCAD6
	public static IMetric Native(string header, string usedOrReserved)
	{
		return new ProfilerPlatformCorrections.NativeDefault(header, usedOrReserved);
	}

	// Token: 0x0600AB3B RID: 43835 RVA: 0x003FE8DF File Offset: 0x003FCADF
	public static IMetric TotalTracked(string header, string usedOrReserved)
	{
		return new ProfilerRecorderMetric
		{
			Header = header,
			recorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default)
		};
	}

	// Token: 0x02001561 RID: 5473
	public class NativeDefault : IMetric
	{
		// Token: 0x17001417 RID: 5143
		// (get) Token: 0x0600AB3C RID: 43836 RVA: 0x003FE910 File Offset: 0x003FCB10
		// (set) Token: 0x0600AB3D RID: 43837 RVA: 0x003FE918 File Offset: 0x003FCB18
		public string Header { get; set; }

		// Token: 0x0600AB3E RID: 43838 RVA: 0x003FE924 File Offset: 0x003FCB24
		public NativeDefault(string header, string usedOrReserved)
		{
			this.Header = header;
			this.graphics = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default);
			this.total = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default);
			this.managed = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default);
		}

		// Token: 0x0600AB3F RID: 43839 RVA: 0x003FE9A8 File Offset: 0x003FCBA8
		public void AppendLastValue(StringBuilder builder)
		{
			double num = this.total.LastValueAsDouble - this.graphics.LastValueAsDouble - this.managed.LastValueAsDouble;
			builder.AppendFormat("{0:F2}", num * 9.5367431640625E-07);
		}

		// Token: 0x0600AB40 RID: 43840 RVA: 0x003FE9F5 File Offset: 0x003FCBF5
		public void Cleanup()
		{
			this.graphics.Dispose();
			this.total.Dispose();
			this.managed.Dispose();
		}

		// Token: 0x04007F1B RID: 32539
		[PublicizedFrom(EAccessModifier.Private)]
		public ProfilerRecorder graphics;

		// Token: 0x04007F1C RID: 32540
		[PublicizedFrom(EAccessModifier.Private)]
		public ProfilerRecorder total;

		// Token: 0x04007F1D RID: 32541
		[PublicizedFrom(EAccessModifier.Private)]
		public ProfilerRecorder managed;
	}

	// Token: 0x02001562 RID: 5474
	public class TotalTrackedPS5 : IMetric
	{
		// Token: 0x17001418 RID: 5144
		// (get) Token: 0x0600AB41 RID: 43841 RVA: 0x003FEA18 File Offset: 0x003FCC18
		// (set) Token: 0x0600AB42 RID: 43842 RVA: 0x003FEA20 File Offset: 0x003FCC20
		public string Header { get; set; }

		// Token: 0x0600AB43 RID: 43843 RVA: 0x003FEA2C File Offset: 0x003FCC2C
		public TotalTrackedPS5(string header, string usedOrReserved)
		{
			this.Header = header;
			this.graphics = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default);
			this.total = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default);
		}

		// Token: 0x0600AB44 RID: 43844 RVA: 0x003FEA8C File Offset: 0x003FCC8C
		public void AppendLastValue(StringBuilder builder)
		{
			double num = this.total.LastValueAsDouble - this.graphics.LastValueAsDouble;
			builder.AppendFormat("{0:F2}", num * 9.5367431640625E-07);
		}

		// Token: 0x0600AB45 RID: 43845 RVA: 0x003FEACD File Offset: 0x003FCCCD
		public void Cleanup()
		{
			this.graphics.Dispose();
			this.total.Dispose();
		}

		// Token: 0x04007F1F RID: 32543
		[PublicizedFrom(EAccessModifier.Private)]
		public ProfilerRecorder graphics;

		// Token: 0x04007F20 RID: 32544
		[PublicizedFrom(EAccessModifier.Private)]
		public ProfilerRecorder total;
	}
}
