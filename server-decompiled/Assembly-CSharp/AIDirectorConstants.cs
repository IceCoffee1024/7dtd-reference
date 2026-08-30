using System;
using UnityEngine.Scripting;

// Token: 0x0200040D RID: 1037
[Preserve]
public class AIDirectorConstants
{
	// Token: 0x040015D7 RID: 5591
	public static bool DebugOutput = true;

	// Token: 0x040015D8 RID: 5592
	public const int kFileVersion = 10;

	// Token: 0x040015D9 RID: 5593
	public const int kMaxSupplyCrates = 12;

	// Token: 0x040015DA RID: 5594
	public const float kStealthSightDistanceMultiplier = 0.8f;

	// Token: 0x040015DB RID: 5595
	public const float kStealthNighttimeSightDistanceMultiplier = 0.5f;

	// Token: 0x040015DC RID: 5596
	public const float kHordeMeterWarn1Threshold = 0.5f;

	// Token: 0x040015DD RID: 5597
	public const float kHordeMeterWarn2Threshold = 0.8f;

	// Token: 0x040015DE RID: 5598
	public const float kHordeMeterWarnResetThreshold = 0.2f;

	// Token: 0x040015DF RID: 5599
	public const int kHordeDaySpawnRangeMin = 45;

	// Token: 0x040015E0 RID: 5600
	public const int kHordeDaySpawnRangeMax = 55;

	// Token: 0x040015E1 RID: 5601
	public const int kHordeNightSpawnRangeMin = 55;

	// Token: 0x040015E2 RID: 5602
	public const int kHordeNightSpawnRangeMax = 70;

	// Token: 0x040015E3 RID: 5603
	public const float kHordeMeterDecayDelay = 8f;

	// Token: 0x040015E4 RID: 5604
	public const float kHordeMeterDecayRate = 4f;

	// Token: 0x040015E5 RID: 5605
	public const int kWanderingHordeGlobalStartTime = 28000;

	// Token: 0x040015E6 RID: 5606
	public const int kSpawnWanderingHordeMin = 12000;

	// Token: 0x040015E7 RID: 5607
	public const int kSpawnWanderingHordeMax = 24000;

	// Token: 0x040015E8 RID: 5608
	public const int kWanderingHordeGroupSize = 6;

	// Token: 0x040015E9 RID: 5609
	public const float kWanderingHordeSpawnDistance = 92f;

	// Token: 0x040015EA RID: 5610
	public const float kWanderingHordeSpawnMinDistance = 50f;

	// Token: 0x040015EB RID: 5611
	public const float kWanderingHordePlayerClusterSize = 30f;

	// Token: 0x040015EC RID: 5612
	public const int kSoundPriorityStart = 10;

	// Token: 0x040015ED RID: 5613
	public const int kSoundPriorityRange = 100;

	// Token: 0x040015EE RID: 5614
	public const int kScoutSpawnDistance = 80;

	// Token: 0x040015EF RID: 5615
	public const float kScoutScreamGraceTime = 2f;

	// Token: 0x040015F0 RID: 5616
	public const float kScoutScreamAgainTime = 18f;

	// Token: 0x040015F1 RID: 5617
	public const float kScoutSpawnAnotherScoutChance = 0.12f;

	// Token: 0x040015F2 RID: 5618
	public const int kScoutSummonedPerScream = 5;

	// Token: 0x040015F3 RID: 5619
	public const int kScoutSummonedTotal = 25;
}
