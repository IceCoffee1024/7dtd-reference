using System;

namespace Unity.Profiling
{
	// Token: 0x02000067 RID: 103
	internal sealed class ProfilerRecorderDebugView
	{
		// Token: 0x06000190 RID: 400 RVA: 0x00003983 File Offset: 0x00001B83
		public ProfilerRecorderDebugView(ProfilerRecorder r)
		{
			this.m_Recorder = r;
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00003994 File Offset: 0x00001B94
		public ProfilerRecorderSample[] Items
		{
			get
			{
				return this.m_Recorder.ToArray();
			}
		}

		// Token: 0x04000157 RID: 343
		private ProfilerRecorder m_Recorder;
	}
}
