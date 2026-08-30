using System;
using UnityEngine;

// Token: 0x02000A57 RID: 2647
public interface ISelectionBoxCallback
{
	// Token: 0x06005031 RID: 20529
	bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated);

	// Token: 0x06005032 RID: 20530
	void OnSelectionBoxMoved(SelectionBox _box, Vector3 _moveVector);

	// Token: 0x06005033 RID: 20531
	void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest);

	// Token: 0x06005034 RID: 20532
	void OnSelectionBoxMirrored(Vector3i _axis);

	// Token: 0x06005035 RID: 20533
	bool OnSelectionBoxDelete(SelectionBox _box, bool _checkCanDeleteOnly);

	// Token: 0x06005036 RID: 20534
	bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria);

	// Token: 0x06005037 RID: 20535
	void OnSelectionBoxShowProperties(bool _bVisible, GUIWindowManager _windowManager);

	// Token: 0x06005038 RID: 20536
	void OnSelectionBoxRotated(SelectionBox _box);

	// Token: 0x06005039 RID: 20537
	void OnSelectionBoxUserDataChanged(SelectionBox _box);
}
