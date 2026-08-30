using System;

// Token: 0x02000642 RID: 1602
public interface IMeshGenerator
{
	// Token: 0x060033F9 RID: 13305
	bool IsLayerEmpty(int _layerIdx);

	// Token: 0x060033FA RID: 13306
	bool IsLayerEmpty(int _startLayerIdx, int _endLayerIdx);
}
