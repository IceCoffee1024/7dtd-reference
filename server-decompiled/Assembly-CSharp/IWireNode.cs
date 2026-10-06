using System;
using UnityEngine;

// Token: 0x02001358 RID: 4952
public interface IWireNode
{
	// Token: 0x06009C01 RID: 39937
	Vector3 GetStartPosition();

	// Token: 0x06009C02 RID: 39938
	Vector3 GetStartPositionOffset();

	// Token: 0x06009C03 RID: 39939
	void SetStartPosition(Vector3 pos);

	// Token: 0x06009C04 RID: 39940
	void SetStartPositionOffset(Vector3 pos);

	// Token: 0x06009C05 RID: 39941
	Vector3 GetEndPosition();

	// Token: 0x06009C06 RID: 39942
	Vector3 GetEndPositionOffset();

	// Token: 0x06009C07 RID: 39943
	void SetEndPosition(Vector3 pos);

	// Token: 0x06009C08 RID: 39944
	void SetEndPositionOffset(Vector3 pos);

	// Token: 0x06009C09 RID: 39945
	void SetWireDip(float _dist);

	// Token: 0x06009C0A RID: 39946
	float GetWireDip();

	// Token: 0x06009C0B RID: 39947
	void SetWireRadius(float _radius);

	// Token: 0x06009C0C RID: 39948
	void SetWireCanHide(bool _canHide);

	// Token: 0x06009C0D RID: 39949
	void SetVisible(bool _visible);

	// Token: 0x06009C0E RID: 39950
	void Reset();

	// Token: 0x06009C0F RID: 39951
	void BuildMesh();

	// Token: 0x06009C10 RID: 39952
	void SetPulseColor(Color color);

	// Token: 0x06009C11 RID: 39953
	void TogglePulse(bool isOn);

	// Token: 0x06009C12 RID: 39954
	GameObject GetGameObject();

	// Token: 0x06009C13 RID: 39955
	Bounds GetBounds();
}
