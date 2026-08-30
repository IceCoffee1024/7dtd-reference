using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Services.Analytics.Events
{
	// Token: 0x02001664 RID: 5732
	public class HardwareInfoEventData : BaseEventData
	{
		// Token: 0x17001578 RID: 5496
		// (get) Token: 0x0600B37D RID: 45949 RVA: 0x00431BFE File Offset: 0x0042FDFE
		public override string EventType
		{
			get
			{
				return "hardware_info";
			}
		}

		// Token: 0x17001579 RID: 5497
		// (get) Token: 0x0600B37E RID: 45950 RVA: 0x00431C05 File Offset: 0x0042FE05
		// (set) Token: 0x0600B37F RID: 45951 RVA: 0x00431C0D File Offset: 0x0042FE0D
		[JsonProperty(PropertyName = "os")]
		public string OperatingSystem
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = SystemInfo.operatingSystem;

		// Token: 0x1700157A RID: 5498
		// (get) Token: 0x0600B380 RID: 45952 RVA: 0x00431C16 File Offset: 0x0042FE16
		// (set) Token: 0x0600B381 RID: 45953 RVA: 0x00431C1E File Offset: 0x0042FE1E
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

		// Token: 0x1700157B RID: 5499
		// (get) Token: 0x0600B382 RID: 45954 RVA: 0x00431C27 File Offset: 0x0042FE27
		// (set) Token: 0x0600B383 RID: 45955 RVA: 0x00431C2F File Offset: 0x0042FE2F
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

		// Token: 0x1700157C RID: 5500
		// (get) Token: 0x0600B384 RID: 45956 RVA: 0x00431C38 File Offset: 0x0042FE38
		// (set) Token: 0x0600B385 RID: 45957 RVA: 0x00431C40 File Offset: 0x0042FE40
		[JsonProperty(PropertyName = "memory_ram")]
		public int MemoryRam
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		} = SystemInfo.systemMemorySize;
	}
}
