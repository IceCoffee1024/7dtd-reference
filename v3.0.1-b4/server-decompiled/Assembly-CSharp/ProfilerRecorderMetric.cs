using System;
using System.Text;
using Unity.Profiling;

// Token: 0x0200150B RID: 5387
public class ProfilerRecorderMetric : IMetric
{
	// Token: 0x1700138B RID: 5003
	// (get) Token: 0x0600A912 RID: 43282 RVA: 0x003F1EB8 File Offset: 0x003F00B8
	// (set) Token: 0x0600A913 RID: 43283 RVA: 0x003F1EC0 File Offset: 0x003F00C0
	public string Header { get; set; }

	// Token: 0x0600A914 RID: 43284 RVA: 0x003F1EC9 File Offset: 0x003F00C9
	public void AppendLastValue(StringBuilder builder)
	{
		ProfilerUtils.AppendLastValue(this.recorder, builder);
	}

	// Token: 0x0600A915 RID: 43285 RVA: 0x003F1ED7 File Offset: 0x003F00D7
	public void Cleanup()
	{
		this.recorder.Dispose();
	}

	// Token: 0x04007D71 RID: 32113
	public ProfilerRecorder recorder;
}
