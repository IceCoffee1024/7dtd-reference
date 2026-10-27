using System;
using System.Collections.Generic;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;
using UnityEngine;

namespace Services.Analytics.Events
{
	// Token: 0x020016BE RID: 5822
	[EventTitle("Hardware Info")]
	public class HardwareInfoEventData : BaseEventData
	{
		// Token: 0x17001631 RID: 5681
		// (get) Token: 0x0600B5E2 RID: 46562 RVA: 0x0043DAA3 File Offset: 0x0043BCA3
		public override string EventType
		{
			get
			{
				return "hardware_info";
			}
		}

		// Token: 0x17001632 RID: 5682
		// (get) Token: 0x0600B5E3 RID: 46563 RVA: 0x0043DAAA File Offset: 0x0043BCAA
		// (set) Token: 0x0600B5E4 RID: 46564 RVA: 0x0043DAB2 File Offset: 0x0043BCB2
		[JsonProperty(PropertyName = "os")]
		public string OperatingSystem
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = SystemInfo.operatingSystem;

		// Token: 0x17001633 RID: 5683
		// (get) Token: 0x0600B5E5 RID: 46565 RVA: 0x0043DABB File Offset: 0x0043BCBB
		// (set) Token: 0x0600B5E6 RID: 46566 RVA: 0x0043DAC3 File Offset: 0x0043BCC3
		[JsonProperty(PropertyName = "cpu_data")]
		public Dictionary<string, string> CpuData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Dictionary<string, string>
		{
			{
				"type",
				SystemInfo.processorType
			},
			{
				"count",
				SystemInfo.processorCount.ToString()
			}
		};

		// Token: 0x17001634 RID: 5684
		// (get) Token: 0x0600B5E7 RID: 46567 RVA: 0x0043DACC File Offset: 0x0043BCCC
		// (set) Token: 0x0600B5E8 RID: 46568 RVA: 0x0043DAD4 File Offset: 0x0043BCD4
		[JsonProperty(PropertyName = "gpu_data")]
		public Dictionary<string, string> GpuData
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = new Dictionary<string, string>
		{
			{
				"name",
				SystemInfo.graphicsDeviceName
			},
			{
				"type",
				SystemInfo.graphicsDeviceType.ToString()
			},
			{
				"version",
				SystemInfo.graphicsDeviceVersion
			},
			{
				"memory",
				SystemInfo.graphicsMemorySize.ToString()
			},
			{
				"vendor",
				SystemInfo.graphicsDeviceVendor
			}
		};

		// Token: 0x17001635 RID: 5685
		// (get) Token: 0x0600B5E9 RID: 46569 RVA: 0x0043DADD File Offset: 0x0043BCDD
		// (set) Token: 0x0600B5EA RID: 46570 RVA: 0x0043DAE5 File Offset: 0x0043BCE5
		[JsonProperty(PropertyName = "memory_ram")]
		public int MemoryRam
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = SystemInfo.systemMemorySize;
	}
}
