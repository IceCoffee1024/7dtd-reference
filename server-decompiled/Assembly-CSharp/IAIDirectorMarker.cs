using System;
using UnityEngine;

// Token: 0x0200042F RID: 1071
public interface IAIDirectorMarker
{
	// Token: 0x060020B9 RID: 8377
	void Reference();

	// Token: 0x060020BA RID: 8378
	bool Release();

	// Token: 0x060020BB RID: 8379
	void Tick(double dt);

	// Token: 0x170003CE RID: 974
	// (get) Token: 0x060020BC RID: 8380
	EntityPlayer Player { get; }

	// Token: 0x060020BD RID: 8381
	double IntensityForPosition(Vector3 position);

	// Token: 0x170003CF RID: 975
	// (get) Token: 0x060020BE RID: 8382
	Vector3 Position { get; }

	// Token: 0x170003D0 RID: 976
	// (get) Token: 0x060020BF RID: 8383
	Vector3 TargetPosition { get; }

	// Token: 0x170003D1 RID: 977
	// (get) Token: 0x060020C0 RID: 8384
	bool Valid { get; }

	// Token: 0x170003D2 RID: 978
	// (get) Token: 0x060020C1 RID: 8385
	float MaxRadius { get; }

	// Token: 0x170003D3 RID: 979
	// (get) Token: 0x060020C2 RID: 8386
	float Radius { get; }

	// Token: 0x170003D4 RID: 980
	// (get) Token: 0x060020C3 RID: 8387
	float TimeToLive { get; }

	// Token: 0x170003D5 RID: 981
	// (get) Token: 0x060020C4 RID: 8388
	float ValidTime { get; }

	// Token: 0x170003D6 RID: 982
	// (get) Token: 0x060020C5 RID: 8389
	float Speed { get; }

	// Token: 0x170003D7 RID: 983
	// (get) Token: 0x060020C6 RID: 8390
	int Priority { get; }

	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x060020C7 RID: 8391
	bool InterruptsNonPlayerAttack { get; }

	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x060020C8 RID: 8392
	bool IsDistraction { get; }
}
