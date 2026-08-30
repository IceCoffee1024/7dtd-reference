using System;

// Token: 0x02000A69 RID: 2665
public interface ISelectionCategoryCallback
{
	// Token: 0x060050C0 RID: 20672
	void OnSelectionCategoryVisibilityChanged(SelectionCategory _category, bool _visible);

	// Token: 0x060050C1 RID: 20673
	void OnSelectionCategoryBoxAdded(SelectionBox _box);

	// Token: 0x060050C2 RID: 20674
	void OnSelectionCategoryBoxRemoved(SelectionBox _box);

	// Token: 0x060050C3 RID: 20675
	void OnSelectionCategoryCleared(SelectionCategory _category);
}
