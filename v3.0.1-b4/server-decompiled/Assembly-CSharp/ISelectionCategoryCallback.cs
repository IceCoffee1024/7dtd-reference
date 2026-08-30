using System;

// Token: 0x02000A58 RID: 2648
public interface ISelectionCategoryCallback
{
	// Token: 0x0600503A RID: 20538
	void OnSelectionCategoryVisibilityChanged(SelectionCategory _category, bool _visible);

	// Token: 0x0600503B RID: 20539
	void OnSelectionCategoryBoxAdded(SelectionBox _box);

	// Token: 0x0600503C RID: 20540
	void OnSelectionCategoryBoxRemoved(SelectionBox _box);

	// Token: 0x0600503D RID: 20541
	void OnSelectionCategoryCleared(SelectionCategory _category);
}
