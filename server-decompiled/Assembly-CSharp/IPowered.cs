using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C60 RID: 3168
public interface IPowered
{
	// Token: 0x060060CE RID: 24782
	Vector3i GetParent();

	// Token: 0x060060CF RID: 24783
	PowerItem GetPowerItem();

	// Token: 0x060060D0 RID: 24784
	void DrawWires();

	// Token: 0x060060D1 RID: 24785
	void RemoveWires();

	// Token: 0x060060D2 RID: 24786
	void MarkWireDirty();

	// Token: 0x060060D3 RID: 24787
	void MarkChanged();

	// Token: 0x060060D4 RID: 24788
	void AddWireData(Vector3i child);

	// Token: 0x060060D5 RID: 24789
	Vector3 GetWireOffset();

	// Token: 0x060060D6 RID: 24790
	int GetRequiredPower();

	// Token: 0x060060D7 RID: 24791
	bool CanHaveParent(IPowered newParent);

	// Token: 0x060060D8 RID: 24792
	void SetParentWithWireTool(IPowered parent, int entityID);

	// Token: 0x060060D9 RID: 24793
	void RemoveParentWithWiringTool(int wiringEntityID);

	// Token: 0x060060DA RID: 24794
	void SetWireData(List<Vector3i> wireChildren);

	// Token: 0x060060DB RID: 24795
	void SendWireData();

	// Token: 0x060060DC RID: 24796
	void CreateWireDataFromPowerItem();
}
