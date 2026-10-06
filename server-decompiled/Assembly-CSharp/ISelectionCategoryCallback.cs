using System;

// Token: 0x02000A73 RID: 2675
public interface ISelectionCategoryCallback
{
	// Token: 0x06005094 RID: 20628
	void OnSelectionCategoryVisibilityChanged(SelectionCategory _category, bool _visible);

	// Token: 0x06005095 RID: 20629
	void OnSelectionCategoryBoxAdded(SelectionBox _box);

	// Token: 0x06005096 RID: 20630
	void OnSelectionCategoryBoxRemoved(SelectionBox _box);

	// Token: 0x06005097 RID: 20631
	void OnSelectionCategoryCleared(SelectionCategory _category);
}
