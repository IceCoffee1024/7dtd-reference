using System;
using UnityEngine;

// Token: 0x0200136A RID: 4970
public interface IWireNode
{
	// Token: 0x06009C59 RID: 40025
	Vector3 GetStartPosition();

	// Token: 0x06009C5A RID: 40026
	Vector3 GetStartPositionOffset();

	// Token: 0x06009C5B RID: 40027
	void SetStartPosition(Vector3 pos);

	// Token: 0x06009C5C RID: 40028
	void SetStartPositionOffset(Vector3 pos);

	// Token: 0x06009C5D RID: 40029
	Vector3 GetEndPosition();

	// Token: 0x06009C5E RID: 40030
	Vector3 GetEndPositionOffset();

	// Token: 0x06009C5F RID: 40031
	void SetEndPosition(Vector3 pos);

	// Token: 0x06009C60 RID: 40032
	void SetEndPositionOffset(Vector3 pos);

	// Token: 0x06009C61 RID: 40033
	void SetWireDip(float _dist);

	// Token: 0x06009C62 RID: 40034
	float GetWireDip();

	// Token: 0x06009C63 RID: 40035
	void SetWireRadius(float _radius);

	// Token: 0x06009C64 RID: 40036
	void SetWireCanHide(bool _canHide);

	// Token: 0x06009C65 RID: 40037
	void SetVisible(bool _visible);

	// Token: 0x06009C66 RID: 40038
	void Reset();

	// Token: 0x06009C67 RID: 40039
	void BuildMesh();

	// Token: 0x06009C68 RID: 40040
	void SetPulseColor(Color color);

	// Token: 0x06009C69 RID: 40041
	void TogglePulse(bool isOn);

	// Token: 0x06009C6A RID: 40042
	GameObject GetGameObject();

	// Token: 0x06009C6B RID: 40043
	Bounds GetBounds();
}
