using System;
using System.Text;
using Unity.Profiling;

// Token: 0x02001548 RID: 5448
public class ProfilerRecorderMetric : IMetric
{
	// Token: 0x17001409 RID: 5129
	// (get) Token: 0x0600AAC2 RID: 43714 RVA: 0x003FC04C File Offset: 0x003FA24C
	// (set) Token: 0x0600AAC3 RID: 43715 RVA: 0x003FC054 File Offset: 0x003FA254
	public string Header { get; set; }

	// Token: 0x0600AAC4 RID: 43716 RVA: 0x003FC05D File Offset: 0x003FA25D
	public void AppendLastValue(StringBuilder builder)
	{
		ProfilerUtils.AppendLastValue(this.recorder, builder);
	}

	// Token: 0x0600AAC5 RID: 43717 RVA: 0x003FC06B File Offset: 0x003FA26B
	public void Cleanup()
	{
		this.recorder.Dispose();
	}

	// Token: 0x04007ECD RID: 32461
	public ProfilerRecorder recorder;
}
