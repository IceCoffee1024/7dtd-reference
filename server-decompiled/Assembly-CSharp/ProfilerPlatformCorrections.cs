using System;
using System.Text;
using Unity.Profiling;

// Token: 0x0200152C RID: 5420
public static class ProfilerPlatformCorrections
{
	// Token: 0x0600AA2E RID: 43566 RVA: 0x003F764D File Offset: 0x003F584D
	public static IMetric Graphics(string header, string usedOrReserved)
	{
		return new ProfilerRecorderMetric
		{
			Header = header,
			recorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default)
		};
	}

	// Token: 0x0600AA2F RID: 43567 RVA: 0x003F767E File Offset: 0x003F587E
	public static IMetric Native(string header, string usedOrReserved)
	{
		return new ProfilerPlatformCorrections.NativeDefault(header, usedOrReserved);
	}

	// Token: 0x0600AA30 RID: 43568 RVA: 0x003F7687 File Offset: 0x003F5887
	public static IMetric TotalTracked(string header, string usedOrReserved)
	{
		return new ProfilerRecorderMetric
		{
			Header = header,
			recorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default)
		};
	}

	// Token: 0x0200152D RID: 5421
	public class NativeDefault : IMetric
	{
		// Token: 0x170013C6 RID: 5062
		// (get) Token: 0x0600AA31 RID: 43569 RVA: 0x003F76B8 File Offset: 0x003F58B8
		// (set) Token: 0x0600AA32 RID: 43570 RVA: 0x003F76C0 File Offset: 0x003F58C0
		public string Header { get; set; }

		// Token: 0x0600AA33 RID: 43571 RVA: 0x003F76CC File Offset: 0x003F58CC
		public NativeDefault(string header, string usedOrReserved)
		{
			this.Header = header;
			this.graphics = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default);
			this.total = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default);
			this.managed = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default);
		}

		// Token: 0x0600AA34 RID: 43572 RVA: 0x003F7750 File Offset: 0x003F5950
		public void AppendLastValue(StringBuilder builder)
		{
			double num = this.total.LastValueAsDouble - this.graphics.LastValueAsDouble - this.managed.LastValueAsDouble;
			builder.AppendFormat("{0:F2}", num * 9.5367431640625E-07);
		}

		// Token: 0x0600AA35 RID: 43573 RVA: 0x003F779D File Offset: 0x003F599D
		public void Cleanup()
		{
			this.graphics.Dispose();
			this.total.Dispose();
			this.managed.Dispose();
		}

		// Token: 0x04007E57 RID: 32343
		[PublicizedFrom(EAccessModifier.Private)]
		public ProfilerRecorder graphics;

		// Token: 0x04007E58 RID: 32344
		[PublicizedFrom(EAccessModifier.Private)]
		public ProfilerRecorder total;

		// Token: 0x04007E59 RID: 32345
		[PublicizedFrom(EAccessModifier.Private)]
		public ProfilerRecorder managed;
	}

	// Token: 0x0200152E RID: 5422
	public class TotalTrackedPS5 : IMetric
	{
		// Token: 0x170013C7 RID: 5063
		// (get) Token: 0x0600AA36 RID: 43574 RVA: 0x003F77C0 File Offset: 0x003F59C0
		// (set) Token: 0x0600AA37 RID: 43575 RVA: 0x003F77C8 File Offset: 0x003F59C8
		public string Header { get; set; }

		// Token: 0x0600AA38 RID: 43576 RVA: 0x003F77D4 File Offset: 0x003F59D4
		public TotalTrackedPS5(string header, string usedOrReserved)
		{
			this.Header = header;
			this.graphics = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default);
			this.total = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total " + usedOrReserved + " Memory", 1, ProfilerRecorderOptions.Default);
		}

		// Token: 0x0600AA39 RID: 43577 RVA: 0x003F7834 File Offset: 0x003F5A34
		public void AppendLastValue(StringBuilder builder)
		{
			double num = this.total.LastValueAsDouble - this.graphics.LastValueAsDouble;
			builder.AppendFormat("{0:F2}", num * 9.5367431640625E-07);
		}

		// Token: 0x0600AA3A RID: 43578 RVA: 0x003F7875 File Offset: 0x003F5A75
		public void Cleanup()
		{
			this.graphics.Dispose();
			this.total.Dispose();
		}

		// Token: 0x04007E5B RID: 32347
		[PublicizedFrom(EAccessModifier.Private)]
		public ProfilerRecorder graphics;

		// Token: 0x04007E5C RID: 32348
		[PublicizedFrom(EAccessModifier.Private)]
		public ProfilerRecorder total;
	}
}
