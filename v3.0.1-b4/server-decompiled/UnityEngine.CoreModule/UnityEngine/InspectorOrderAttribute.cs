using System;

namespace UnityEngine
{
	// Token: 0x0200024B RID: 587
	[AttributeUsage(AttributeTargets.Enum)]
	public sealed class InspectorOrderAttribute : PropertyAttribute
	{
		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06001921 RID: 6433 RVA: 0x00029FAF File Offset: 0x000281AF
		// (set) Token: 0x06001922 RID: 6434 RVA: 0x00029FB7 File Offset: 0x000281B7
		internal InspectorSort m_inspectorSort { get; private set; }

		// Token: 0x170004D3 RID: 1235
		// (get) Token: 0x06001923 RID: 6435 RVA: 0x00029FC0 File Offset: 0x000281C0
		// (set) Token: 0x06001924 RID: 6436 RVA: 0x00029FC8 File Offset: 0x000281C8
		internal InspectorSortDirection m_sortDirection { get; private set; }

		// Token: 0x06001925 RID: 6437 RVA: 0x00029FD1 File Offset: 0x000281D1
		public InspectorOrderAttribute(InspectorSort inspectorSort = InspectorSort.ByName, InspectorSortDirection sortDirection = InspectorSortDirection.Ascending)
		{
			this.m_inspectorSort = inspectorSort;
			this.m_sortDirection = sortDirection;
		}
	}
}
