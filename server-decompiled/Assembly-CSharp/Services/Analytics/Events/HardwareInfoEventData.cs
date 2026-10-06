using System;
using System.Collections.Generic;
using BhvrAnalyticsServices.Attributes;
using Newtonsoft.Json;
using UnityEngine;

namespace Services.Analytics.Events
{
	// Token: 0x020016AB RID: 5803
	[EventTitle("Hardware Info")]
	public class HardwareInfoEventData : BaseEventData
	{
		// Token: 0x1700161D RID: 5661
		// (get) Token: 0x0600B577 RID: 46455 RVA: 0x0043B571 File Offset: 0x00439771
		public override string EventType
		{
			get
			{
				return "hardware_info";
			}
		}

		// Token: 0x1700161E RID: 5662
		// (get) Token: 0x0600B578 RID: 46456 RVA: 0x0043B578 File Offset: 0x00439778
		// (set) Token: 0x0600B579 RID: 46457 RVA: 0x0043B580 File Offset: 0x00439780
		[JsonProperty(PropertyName = "os")]
		public string OperatingSystem
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = SystemInfo.operatingSystem;

		// Token: 0x1700161F RID: 5663
		// (get) Token: 0x0600B57A RID: 46458 RVA: 0x0043B589 File Offset: 0x00439789
		// (set) Token: 0x0600B57B RID: 46459 RVA: 0x0043B591 File Offset: 0x00439791
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

		// Token: 0x17001620 RID: 5664
		// (get) Token: 0x0600B57C RID: 46460 RVA: 0x0043B59A File Offset: 0x0043979A
		// (set) Token: 0x0600B57D RID: 46461 RVA: 0x0043B5A2 File Offset: 0x004397A2
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

		// Token: 0x17001621 RID: 5665
		// (get) Token: 0x0600B57E RID: 46462 RVA: 0x0043B5AB File Offset: 0x004397AB
		// (set) Token: 0x0600B57F RID: 46463 RVA: 0x0043B5B3 File Offset: 0x004397B3
		[JsonProperty(PropertyName = "memory_ram")]
		public int MemoryRam
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = SystemInfo.systemMemorySize;
	}
}
