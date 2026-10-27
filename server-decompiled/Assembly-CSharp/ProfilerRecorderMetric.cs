using System;
using System.Text;
using Unity.Profiling;

// Token: 0x0200155A RID: 5466
public class ProfilerRecorderMetric : IMetric
{
	// Token: 0x17001415 RID: 5141
	// (get) Token: 0x0600AB1A RID: 43802 RVA: 0x003FE450 File Offset: 0x003FC650
	// (set) Token: 0x0600AB1B RID: 43803 RVA: 0x003FE458 File Offset: 0x003FC658
	public string Header { get; set; }

	// Token: 0x0600AB1C RID: 43804 RVA: 0x003FE461 File Offset: 0x003FC661
	public void AppendLastValue(StringBuilder builder)
	{
		ProfilerUtils.AppendLastValue(this.recorder, builder);
	}

	// Token: 0x0600AB1D RID: 43805 RVA: 0x003FE46F File Offset: 0x003FC66F
	public void Cleanup()
	{
		this.recorder.Dispose();
	}

	// Token: 0x04007F0E RID: 32526
	public ProfilerRecorder recorder;
}
