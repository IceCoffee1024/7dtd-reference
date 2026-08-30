using System;
using UnityEngine;

// Token: 0x02001338 RID: 4920
public interface IWireNode
{
	// Token: 0x06009B55 RID: 39765
	Vector3 GetStartPosition();

	// Token: 0x06009B56 RID: 39766
	Vector3 GetStartPositionOffset();

	// Token: 0x06009B57 RID: 39767
	void SetStartPosition(Vector3 pos);

	// Token: 0x06009B58 RID: 39768
	void SetStartPositionOffset(Vector3 pos);

	// Token: 0x06009B59 RID: 39769
	Vector3 GetEndPosition();

	// Token: 0x06009B5A RID: 39770
	Vector3 GetEndPositionOffset();

	// Token: 0x06009B5B RID: 39771
	void SetEndPosition(Vector3 pos);

	// Token: 0x06009B5C RID: 39772
	void SetEndPositionOffset(Vector3 pos);

	// Token: 0x06009B5D RID: 39773
	void SetWireDip(float _dist);

	// Token: 0x06009B5E RID: 39774
	float GetWireDip();

	// Token: 0x06009B5F RID: 39775
	void SetWireRadius(float _radius);

	// Token: 0x06009B60 RID: 39776
	void SetWireCanHide(bool _canHide);

	// Token: 0x06009B61 RID: 39777
	void SetVisible(bool _visible);

	// Token: 0x06009B62 RID: 39778
	void Reset();

	// Token: 0x06009B63 RID: 39779
	void BuildMesh();

	// Token: 0x06009B64 RID: 39780
	void SetPulseColor(Color color);

	// Token: 0x06009B65 RID: 39781
	void TogglePulse(bool isOn);

	// Token: 0x06009B66 RID: 39782
	GameObject GetGameObject();

	// Token: 0x06009B67 RID: 39783
	Bounds GetBounds();
}
