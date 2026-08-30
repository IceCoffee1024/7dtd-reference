using System;
using Unity.Profiling;
using Unity.Profiling.LowLevel;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine.Scripting;

namespace UnityEngine.Profiling
{
	// Token: 0x020002BB RID: 699
	[UsedByNativeCode]
	public sealed class Recorder
	{
		// Token: 0x06001DF0 RID: 7664 RVA: 0x00009E2F File Offset: 0x0000802F
		internal Recorder()
		{
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x000313E4 File Offset: 0x0002F5E4
		internal Recorder(ProfilerRecorderHandle handle)
		{
			bool flag = !handle.Valid;
			if (!flag)
			{
				this.m_RecorderCPU = new ProfilerRecorder(handle, 1, (ProfilerRecorderOptions)153);
				bool flag2 = (ProfilerRecorderHandle.GetDescription(handle).Flags & MarkerFlags.SampleGPU) > MarkerFlags.Default;
				if (flag2)
				{
					this.m_RecorderGPU = new ProfilerRecorder(handle, 1, (ProfilerRecorderOptions)217);
				}
			}
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x00031448 File Offset: 0x0002F648
		~Recorder()
		{
			this.m_RecorderCPU.Dispose();
			this.m_RecorderGPU.Dispose();
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x0003148C File Offset: 0x0002F68C
		public static Recorder Get(string samplerName)
		{
			ProfilerRecorderHandle profilerRecorderHandle = ProfilerRecorderHandle.Get(ProfilerCategory.Any, samplerName);
			bool flag = !profilerRecorderHandle.Valid;
			Recorder recorder;
			if (flag)
			{
				recorder = Recorder.s_InvalidRecorder;
			}
			else
			{
				recorder = new Recorder(profilerRecorderHandle);
			}
			return recorder;
		}

		// Token: 0x170005E8 RID: 1512
		// (get) Token: 0x06001DF4 RID: 7668 RVA: 0x000314C8 File Offset: 0x0002F6C8
		public bool isValid
		{
			get
			{
				return this.m_RecorderCPU.handle > 0UL;
			}
		}

		// Token: 0x170005E9 RID: 1513
		// (get) Token: 0x06001DF5 RID: 7669 RVA: 0x000314EC File Offset: 0x0002F6EC
		// (set) Token: 0x06001DF6 RID: 7670 RVA: 0x00031509 File Offset: 0x0002F709
		public bool enabled
		{
			get
			{
				return this.m_RecorderCPU.IsRunning;
			}
			set
			{
				this.SetEnabled(value);
			}
		}

		// Token: 0x170005EA RID: 1514
		// (get) Token: 0x06001DF7 RID: 7671 RVA: 0x00031514 File Offset: 0x0002F714
		public long elapsedNanoseconds
		{
			get
			{
				bool flag = !this.m_RecorderCPU.Valid;
				long num;
				if (flag)
				{
					num = 0L;
				}
				else
				{
					num = this.m_RecorderCPU.LastValue;
				}
				return num;
			}
		}

		// Token: 0x170005EB RID: 1515
		// (get) Token: 0x06001DF8 RID: 7672 RVA: 0x00031548 File Offset: 0x0002F748
		public long gpuElapsedNanoseconds
		{
			get
			{
				bool flag = !this.m_RecorderGPU.Valid;
				long num;
				if (flag)
				{
					num = 0L;
				}
				else
				{
					num = this.m_RecorderGPU.LastValue;
				}
				return num;
			}
		}

		// Token: 0x170005EC RID: 1516
		// (get) Token: 0x06001DF9 RID: 7673 RVA: 0x0003157C File Offset: 0x0002F77C
		public int sampleBlockCount
		{
			get
			{
				bool flag = !this.m_RecorderCPU.Valid;
				int num;
				if (flag)
				{
					num = 0;
				}
				else
				{
					bool flag2 = this.m_RecorderCPU.Count != 1;
					if (flag2)
					{
						num = 0;
					}
					else
					{
						num = (int)this.m_RecorderCPU.GetSample(0).Count;
					}
				}
				return num;
			}
		}

		// Token: 0x170005ED RID: 1517
		// (get) Token: 0x06001DFA RID: 7674 RVA: 0x000315D4 File Offset: 0x0002F7D4
		public int gpuSampleBlockCount
		{
			get
			{
				bool flag = !this.m_RecorderGPU.Valid;
				int num;
				if (flag)
				{
					num = 0;
				}
				else
				{
					bool flag2 = this.m_RecorderGPU.Count != 1;
					if (flag2)
					{
						num = 0;
					}
					else
					{
						num = (int)this.m_RecorderGPU.GetSample(0).Count;
					}
				}
				return num;
			}
		}

		// Token: 0x06001DFB RID: 7675 RVA: 0x0003162C File Offset: 0x0002F82C
		public void FilterToCurrentThread()
		{
			bool flag = !this.m_RecorderCPU.Valid;
			if (!flag)
			{
				this.m_RecorderCPU.FilterToCurrentThread();
			}
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x0003165C File Offset: 0x0002F85C
		public void CollectFromAllThreads()
		{
			bool flag = !this.m_RecorderCPU.Valid;
			if (!flag)
			{
				this.m_RecorderCPU.CollectFromAllThreads();
			}
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x0003168C File Offset: 0x0002F88C
		private void SetEnabled(bool state)
		{
			if (state)
			{
				this.m_RecorderCPU.Start();
				bool valid = this.m_RecorderGPU.Valid;
				if (valid)
				{
					this.m_RecorderGPU.Start();
				}
			}
			else
			{
				this.m_RecorderCPU.Stop();
				bool valid2 = this.m_RecorderGPU.Valid;
				if (valid2)
				{
					this.m_RecorderGPU.Stop();
				}
			}
		}

		// Token: 0x040009EF RID: 2543
		private const ProfilerRecorderOptions s_RecorderDefaultOptions = (ProfilerRecorderOptions)153;

		// Token: 0x040009F0 RID: 2544
		internal static Recorder s_InvalidRecorder = new Recorder();

		// Token: 0x040009F1 RID: 2545
		private ProfilerRecorder m_RecorderCPU;

		// Token: 0x040009F2 RID: 2546
		private ProfilerRecorder m_RecorderGPU;
	}
}
