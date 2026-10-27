using System;

// Token: 0x02000A7D RID: 2685
public interface ISelectionCategoryCallback
{
	// Token: 0x060050B8 RID: 20664
	void OnSelectionCategoryVisibilityChanged(SelectionCategory _category, bool _visible);

	// Token: 0x060050B9 RID: 20665
	void OnSelectionCategoryBoxAdded(SelectionBox _box);

	// Token: 0x060050BA RID: 20666
	void OnSelectionCategoryBoxRemoved(SelectionBox _box);

	// Token: 0x060050BB RID: 20667
	void OnSelectionCategoryCleared(SelectionCategory _category);
}
