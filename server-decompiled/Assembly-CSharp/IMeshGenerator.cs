using System;

// Token: 0x02000639 RID: 1593
public interface IMeshGenerator
{
	// Token: 0x060033A6 RID: 13222
	bool IsLayerEmpty(int _layerIdx);

	// Token: 0x060033A7 RID: 13223
	bool IsLayerEmpty(int _startLayerIdx, int _endLayerIdx);
}
