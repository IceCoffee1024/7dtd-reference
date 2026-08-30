using System;
using System.Collections.Generic;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;
using UnityEngine;

namespace Services.Analytics.Events
{
	// Token: 0x02001689 RID: 5769
	[EventTitle("Hardware Info")]
	public class HardwareInfoEventData : BaseEventData
	{
		// Token: 0x170015D7 RID: 5591
		// (get) Token: 0x0600B4DA RID: 46298 RVA: 0x00437846 File Offset: 0x00435A46
		public override string EventType
		{
			get
			{
				return "hardware_info";
			}
		}

		// Token: 0x170015D8 RID: 5592
		// (get) Token: 0x0600B4DB RID: 46299 RVA: 0x0043784D File Offset: 0x00435A4D
		// (set) Token: 0x0600B4DC RID: 46300 RVA: 0x00437855 File Offset: 0x00435A55
		[JsonProperty(PropertyName = "os")]
		public string OperatingSystem
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = SystemInfo.operatingSystem;

		// Token: 0x170015D9 RID: 5593
		// (get) Token: 0x0600B4DD RID: 46301 RVA: 0x0043785E File Offset: 0x00435A5E
		// (set) Token: 0x0600B4DE RID: 46302 RVA: 0x00437866 File Offset: 0x00435A66
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

		// Token: 0x170015DA RID: 5594
		// (get) Token: 0x0600B4DF RID: 46303 RVA: 0x0043786F File Offset: 0x00435A6F
		// (set) Token: 0x0600B4E0 RID: 46304 RVA: 0x00437877 File Offset: 0x00435A77
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

		// Token: 0x170015DB RID: 5595
		// (get) Token: 0x0600B4E1 RID: 46305 RVA: 0x00437880 File Offset: 0x00435A80
		// (set) Token: 0x0600B4E2 RID: 46306 RVA: 0x00437888 File Offset: 0x00435A88
		[JsonProperty(PropertyName = "memory_ram")]
		public int MemoryRam
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = SystemInfo.systemMemorySize;
	}
}
