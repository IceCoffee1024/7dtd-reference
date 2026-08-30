using System;
using UnityEngine;

// Token: 0x02000A68 RID: 2664
public interface ISelectionBoxCallback
{
	// Token: 0x060050B7 RID: 20663
	bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated);

	// Token: 0x060050B8 RID: 20664
	void OnSelectionBoxMoved(SelectionBox _box, Vector3 _moveVector);

	// Token: 0x060050B9 RID: 20665
	void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest);

	// Token: 0x060050BA RID: 20666
	void OnSelectionBoxMirrored(Vector3i _axis);

	// Token: 0x060050BB RID: 20667
	bool OnSelectionBoxDelete(SelectionBox _box, bool _checkCanDeleteOnly);

	// Token: 0x060050BC RID: 20668
	bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria);

	// Token: 0x060050BD RID: 20669
	void OnSelectionBoxShowProperties(bool _bVisible, GUIWindowManager _windowManager);

	// Token: 0x060050BE RID: 20670
	void OnSelectionBoxRotated(SelectionBox _box);

	// Token: 0x060050BF RID: 20671
	void OnSelectionBoxUserDataChanged(SelectionBox _box);
}
