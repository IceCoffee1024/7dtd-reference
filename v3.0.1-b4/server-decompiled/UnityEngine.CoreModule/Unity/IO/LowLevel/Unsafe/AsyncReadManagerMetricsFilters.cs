using System;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace Unity.IO.LowLevel.Unsafe
{
	// Token: 0x02000085 RID: 133
	[NativeConditional("ENABLE_PROFILER")]
	[NativeAsStruct]
	[RequiredByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	public class AsyncReadManagerMetricsFilters
	{
		// Token: 0x0600025F RID: 607 RVA: 0x00004881 File Offset: 0x00002A81
		public AsyncReadManagerMetricsFilters()
		{
			this.ClearFilters();
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00004892 File Offset: 0x00002A92
		public AsyncReadManagerMetricsFilters(ulong typeID)
		{
			this.ClearFilters();
			this.SetTypeIDFilter(typeID);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x000048AB File Offset: 0x00002AAB
		public AsyncReadManagerMetricsFilters(ProcessingState state)
		{
			this.ClearFilters();
			this.SetStateFilter(state);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x000048C4 File Offset: 0x00002AC4
		public AsyncReadManagerMetricsFilters(FileReadType readType)
		{
			this.ClearFilters();
			this.SetReadTypeFilter(readType);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x000048DD File Offset: 0x00002ADD
		public AsyncReadManagerMetricsFilters(Priority priorityLevel)
		{
			this.ClearFilters();
			this.SetPriorityFilter(priorityLevel);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x000048F6 File Offset: 0x00002AF6
		public AsyncReadManagerMetricsFilters(AssetLoadingSubsystem subsystem)
		{
			this.ClearFilters();
			this.SetSubsystemFilter(subsystem);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000490F File Offset: 0x00002B0F
		public AsyncReadManagerMetricsFilters(ulong[] typeIDs)
		{
			this.ClearFilters();
			this.SetTypeIDFilter(typeIDs);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00004928 File Offset: 0x00002B28
		public AsyncReadManagerMetricsFilters(ProcessingState[] states)
		{
			this.ClearFilters();
			this.SetStateFilter(states);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00004941 File Offset: 0x00002B41
		public AsyncReadManagerMetricsFilters(FileReadType[] readTypes)
		{
			this.ClearFilters();
			this.SetReadTypeFilter(readTypes);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000495A File Offset: 0x00002B5A
		public AsyncReadManagerMetricsFilters(Priority[] priorityLevels)
		{
			this.ClearFilters();
			this.SetPriorityFilter(priorityLevels);
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00004973 File Offset: 0x00002B73
		public AsyncReadManagerMetricsFilters(AssetLoadingSubsystem[] subsystems)
		{
			this.ClearFilters();
			this.SetSubsystemFilter(subsystems);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000498C File Offset: 0x00002B8C
		public AsyncReadManagerMetricsFilters(ulong[] typeIDs, ProcessingState[] states, FileReadType[] readTypes, Priority[] priorityLevels, AssetLoadingSubsystem[] subsystems)
		{
			this.ClearFilters();
			this.SetTypeIDFilter(typeIDs);
			this.SetStateFilter(states);
			this.SetReadTypeFilter(readTypes);
			this.SetPriorityFilter(priorityLevels);
			this.SetSubsystemFilter(subsystems);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x000049C7 File Offset: 0x00002BC7
		public void SetTypeIDFilter(ulong[] _typeIDs)
		{
			this.TypeIDs = _typeIDs;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x000049D1 File Offset: 0x00002BD1
		public void SetStateFilter(ProcessingState[] _states)
		{
			this.States = _states;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x000049DB File Offset: 0x00002BDB
		public void SetReadTypeFilter(FileReadType[] _readTypes)
		{
			this.ReadTypes = _readTypes;
		}

		// Token: 0x0600026E RID: 622 RVA: 0x000049E5 File Offset: 0x00002BE5
		public void SetPriorityFilter(Priority[] _priorityLevels)
		{
			this.PriorityLevels = _priorityLevels;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x000049EF File Offset: 0x00002BEF
		public void SetSubsystemFilter(AssetLoadingSubsystem[] _subsystems)
		{
			this.Subsystems = _subsystems;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x000049F9 File Offset: 0x00002BF9
		public void SetTypeIDFilter(ulong _typeID)
		{
			this.TypeIDs = new ulong[] { _typeID };
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00004A0C File Offset: 0x00002C0C
		public void SetStateFilter(ProcessingState _state)
		{
			this.States = new ProcessingState[] { _state };
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00004A1F File Offset: 0x00002C1F
		public void SetReadTypeFilter(FileReadType _readType)
		{
			this.ReadTypes = new FileReadType[] { _readType };
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00004A32 File Offset: 0x00002C32
		public void SetPriorityFilter(Priority _priorityLevel)
		{
			this.PriorityLevels = new Priority[] { _priorityLevel };
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00004A45 File Offset: 0x00002C45
		public void SetSubsystemFilter(AssetLoadingSubsystem _subsystem)
		{
			this.Subsystems = new AssetLoadingSubsystem[] { _subsystem };
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00004A58 File Offset: 0x00002C58
		public void RemoveTypeIDFilter()
		{
			this.TypeIDs = null;
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00004A62 File Offset: 0x00002C62
		public void RemoveStateFilter()
		{
			this.States = null;
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00004A6C File Offset: 0x00002C6C
		public void RemoveReadTypeFilter()
		{
			this.ReadTypes = null;
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00004A76 File Offset: 0x00002C76
		public void RemovePriorityFilter()
		{
			this.PriorityLevels = null;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00004A80 File Offset: 0x00002C80
		public void RemoveSubsystemFilter()
		{
			this.Subsystems = null;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00004A8A File Offset: 0x00002C8A
		public void ClearFilters()
		{
			this.RemoveTypeIDFilter();
			this.RemoveStateFilter();
			this.RemoveReadTypeFilter();
			this.RemovePriorityFilter();
			this.RemoveSubsystemFilter();
		}

		// Token: 0x04000202 RID: 514
		[NativeName("typeIDs")]
		internal ulong[] TypeIDs;

		// Token: 0x04000203 RID: 515
		[NativeName("states")]
		internal ProcessingState[] States;

		// Token: 0x04000204 RID: 516
		[NativeName("readTypes")]
		internal FileReadType[] ReadTypes;

		// Token: 0x04000205 RID: 517
		[NativeName("priorityLevels")]
		internal Priority[] PriorityLevels;

		// Token: 0x04000206 RID: 518
		[NativeName("subsystems")]
		internal AssetLoadingSubsystem[] Subsystems;
	}
}
