using System;
using UnityEngine;

// Token: 0x02000A72 RID: 2674
public interface ISelectionBoxCallback
{
	// Token: 0x0600508B RID: 20619
	bool OnSelectionBoxActivated(SelectionBox _box, bool _bActivated);

	// Token: 0x0600508C RID: 20620
	void OnSelectionBoxMoved(SelectionBox _box, Vector3 _moveVector);

	// Token: 0x0600508D RID: 20621
	void OnSelectionBoxSized(SelectionBox _box, int _dTop, int _dBottom, int _dNorth, int _dSouth, int _dEast, int _dWest);

	// Token: 0x0600508E RID: 20622
	void OnSelectionBoxMirrored(Vector3i _axis);

	// Token: 0x0600508F RID: 20623
	bool OnSelectionBoxDelete(SelectionBox _box, bool _checkCanDeleteOnly);

	// Token: 0x06005090 RID: 20624
	bool OnSelectionBoxIsAvailable(EnumSelectionBoxAvailabilities _criteria);

	// Token: 0x06005091 RID: 20625
	void OnSelectionBoxShowProperties(bool _bVisible, GUIWindowManager _windowManager);

	// Token: 0x06005092 RID: 20626
	void OnSelectionBoxRotated(SelectionBox _box);

	// Token: 0x06005093 RID: 20627
	void OnSelectionBoxUserDataChanged(SelectionBox _box);
}
