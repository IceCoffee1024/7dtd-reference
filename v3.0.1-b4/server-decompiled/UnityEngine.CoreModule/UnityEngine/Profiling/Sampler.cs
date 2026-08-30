using System;
using System.Collections.Generic;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Profiling
{
	// Token: 0x020002BC RID: 700
	[UsedByNativeCode]
	[NativeHeader("Runtime/Profiler/ScriptBindings/Sampler.bindings.h")]
	public class Sampler
	{
		// Token: 0x06001DFF RID: 7679 RVA: 0x00009E2F File Offset: 0x0000802F
		internal Sampler()
		{
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x000316FF File Offset: 0x0002F8FF
		internal Sampler(IntPtr ptr)
		{
			this.m_Ptr = ptr;
		}

		// Token: 0x170005EE RID: 1518
		// (get) Token: 0x06001E01 RID: 7681 RVA: 0x00031710 File Offset: 0x0002F910
		public bool isValid
		{
			get
			{
				return this.m_Ptr != IntPtr.Zero;
			}
		}

		// Token: 0x06001E02 RID: 7682 RVA: 0x00031734 File Offset: 0x0002F934
		public Recorder GetRecorder()
		{
			ProfilerRecorderHandle profilerRecorderHandle = new ProfilerRecorderHandle((ulong)this.m_Ptr.ToInt64());
			return new Recorder(profilerRecorderHandle);
		}

		// Token: 0x06001E03 RID: 7683 RVA: 0x00031760 File Offset: 0x0002F960
		public static Sampler Get(string name)
		{
			IntPtr marker = ProfilerUnsafeUtility.GetMarker(name);
			bool flag = marker == IntPtr.Zero;
			Sampler sampler;
			if (flag)
			{
				sampler = Sampler.s_InvalidSampler;
			}
			else
			{
				sampler = new Sampler(marker);
			}
			return sampler;
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x00031798 File Offset: 0x0002F998
		public static int GetNames(List<string> names)
		{
			List<ProfilerRecorderHandle> list = new List<ProfilerRecorderHandle>();
			ProfilerRecorderHandle.GetAvailable(list);
			bool flag = names != null;
			if (flag)
			{
				bool flag2 = names.Count < list.Count;
				if (flag2)
				{
					names.Capacity = list.Count;
					for (int i = names.Count; i < list.Count; i++)
					{
						names.Add(null);
					}
				}
				int num = 0;
				foreach (ProfilerRecorderHandle profilerRecorderHandle in list)
				{
					names[num] = ProfilerRecorderHandle.GetDescription(profilerRecorderHandle).Name;
					num++;
				}
			}
			return list.Count;
		}

		// Token: 0x170005EF RID: 1519
		// (get) Token: 0x06001E05 RID: 7685 RVA: 0x00031874 File Offset: 0x0002FA74
		public string name
		{
			get
			{
				return ProfilerUnsafeUtility.Internal_GetName(this.m_Ptr);
			}
		}

		// Token: 0x040009F3 RID: 2547
		internal IntPtr m_Ptr;

		// Token: 0x040009F4 RID: 2548
		internal static Sampler s_InvalidSampler = new Sampler();
	}
}
