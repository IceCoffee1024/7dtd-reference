using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering
{
	// Token: 0x0200048C RID: 1164
	[NativeType(Header = "Runtime/2D/Sorting/SortingGroup.h")]
	[RequireComponent(typeof(Transform))]
	public sealed class SortingGroup : Behaviour
	{
		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06002806 RID: 10246
		[StaticAccessor("SortingGroup", StaticAccessorType.DoubleColon)]
		internal static extern int invalidSortingGroupID
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06002807 RID: 10247
		[StaticAccessor("SortingGroup", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void UpdateAllSortingGroups();

		// Token: 0x06002808 RID: 10248
		[StaticAccessor("SortingGroup", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern SortingGroup GetSortingGroupByIndex(int index);

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06002809 RID: 10249
		// (set) Token: 0x0600280A RID: 10250
		public extern string sortingLayerName
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000770 RID: 1904
		// (get) Token: 0x0600280B RID: 10251
		// (set) Token: 0x0600280C RID: 10252
		public extern int sortingLayerID
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x0600280D RID: 10253
		// (set) Token: 0x0600280E RID: 10254
		public extern int sortingOrder
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x0600280F RID: 10255
		// (set) Token: 0x06002810 RID: 10256
		public extern bool sortAtRoot
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x06002811 RID: 10257
		internal extern int sortingGroupID
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x06002812 RID: 10258
		internal extern int sortingGroupOrder
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x06002813 RID: 10259
		internal extern int index
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x06002814 RID: 10260
		internal extern uint sortingKey
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
	}
}
