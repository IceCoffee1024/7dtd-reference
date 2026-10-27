using System;
using UnityEngine;

// Token: 0x02000A7C RID: 2684
public interface ISelectionBoxCallback
{
	// Token: 0x060050AF RID: 20655
	bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated);

	// Token: 0x060050B0 RID: 20656
	void OnSelectionBoxMoved(SelectionBox _box, Vector3 _moveVector);

	// Token: 0x060050B1 RID: 20657
	void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest);

	// Token: 0x060050B2 RID: 20658
	void OnSelectionBoxMirrored(Vector3i _axis);

	// Token: 0x060050B3 RID: 20659
	bool OnSelectionBoxDelete(SelectionBox _box, bool _checkCanDeleteOnly);

	// Token: 0x060050B4 RID: 20660
	bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria);

	// Token: 0x060050B5 RID: 20661
	void OnSelectionBoxShowProperties(bool _bVisible, GUIWindowManager _windowManager);

	// Token: 0x060050B6 RID: 20662
	void OnSelectionBoxRotated(SelectionBox _box);

	// Token: 0x060050B7 RID: 20663
	void OnSelectionBoxUserDataChanged(SelectionBox _box);
}
