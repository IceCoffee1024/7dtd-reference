using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C8C RID: 3212
public interface IPowered
{
	// Token: 0x0600617A RID: 24954
	Vector3i GetParent();

	// Token: 0x0600617B RID: 24955
	PowerItem GetPowerItem();

	// Token: 0x0600617C RID: 24956
	void DrawWires();

	// Token: 0x0600617D RID: 24957
	void RemoveWires();

	// Token: 0x0600617E RID: 24958
	void MarkWireDirty();

	// Token: 0x0600617F RID: 24959
	void MarkChanged();

	// Token: 0x06006180 RID: 24960
	void AddWireData(Vector3i child);

	// Token: 0x06006181 RID: 24961
	Vector3 GetWireOffset();

	// Token: 0x06006182 RID: 24962
	int GetRequiredPower();

	// Token: 0x06006183 RID: 24963
	bool CanHaveParent(IPowered newParent);

	// Token: 0x06006184 RID: 24964
	void SetParentWithWireTool(IPowered parent, int entityID);

	// Token: 0x06006185 RID: 24965
	void RemoveParentWithWiringTool(int wiringEntityID);

	// Token: 0x06006186 RID: 24966
	void SetWireData(List<Vector3i> wireChildren);

	// Token: 0x06006187 RID: 24967
	void SendWireData();

	// Token: 0x06006188 RID: 24968
	void CreateWireDataFromPowerItem();
}
