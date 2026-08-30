using System;
using System.Text;
using Unity.Profiling;

// Token: 0x02001526 RID: 5414
public class ProfilerRecorderMetric : IMetric
{
	// Token: 0x170013C4 RID: 5060
	// (get) Token: 0x0600AA0F RID: 43535 RVA: 0x003F71F8 File Offset: 0x003F53F8
	// (set) Token: 0x0600AA10 RID: 43536 RVA: 0x003F7200 File Offset: 0x003F5400
	public string Header { get; set; }

	// Token: 0x0600AA11 RID: 43537 RVA: 0x003F7209 File Offset: 0x003F5409
	public void AppendLastValue(StringBuilder builder)
	{
		ProfilerUtils.AppendLastValue(this.recorder, builder);
	}

	// Token: 0x0600AA12 RID: 43538 RVA: 0x003F7217 File Offset: 0x003F5417
	public void Cleanup()
	{
		this.recorder.Dispose();
	}

	// Token: 0x04007E4A RID: 32330
	public ProfilerRecorder recorder;
}
