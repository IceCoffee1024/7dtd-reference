using System;
using UnityEngine;

// Token: 0x0200131E RID: 4894
public interface IWireNode
{
	// Token: 0x06009A5A RID: 39514
	Vector3 GetStartPosition();

	// Token: 0x06009A5B RID: 39515
	Vector3 GetStartPositionOffset();

	// Token: 0x06009A5C RID: 39516
	void SetStartPosition(Vector3 pos);

	// Token: 0x06009A5D RID: 39517
	void SetStartPositionOffset(Vector3 pos);

	// Token: 0x06009A5E RID: 39518
	Vector3 GetEndPosition();

	// Token: 0x06009A5F RID: 39519
	Vector3 GetEndPositionOffset();

	// Token: 0x06009A60 RID: 39520
	void SetEndPosition(Vector3 pos);

	// Token: 0x06009A61 RID: 39521
	void SetEndPositionOffset(Vector3 pos);

	// Token: 0x06009A62 RID: 39522
	void SetWireDip(float _dist);

	// Token: 0x06009A63 RID: 39523
	float GetWireDip();

	// Token: 0x06009A64 RID: 39524
	void SetWireRadius(float _radius);

	// Token: 0x06009A65 RID: 39525
	void SetWireCanHide(bool _canHide);

	// Token: 0x06009A66 RID: 39526
	void SetVisible(bool _visible);

	// Token: 0x06009A67 RID: 39527
	void Reset();

	// Token: 0x06009A68 RID: 39528
	void BuildMesh();

	// Token: 0x06009A69 RID: 39529
	void SetPulseColor(Color color);

	// Token: 0x06009A6A RID: 39530
	void TogglePulse(bool isOn);

	// Token: 0x06009A6B RID: 39531
	GameObject GetGameObject();

	// Token: 0x06009A6C RID: 39532
	Bounds GetBounds();
}
