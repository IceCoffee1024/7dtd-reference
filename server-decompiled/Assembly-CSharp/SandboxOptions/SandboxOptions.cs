using System;

namespace SandboxOptions
{
	// Token: 0x02001899 RID: 6297
	public enum SandboxOptions
	{
		// Token: 0x040092DF RID: 37599
		RangedDamage,
		// Token: 0x040092E0 RID: 37600
		MeleeDamage,
		// Token: 0x040092E1 RID: 37601
		BlockDamage,
		// Token: 0x040092E2 RID: 37602
		TerrainDamage,
		// Token: 0x040092E3 RID: 37603
		HeadshotMultiplier,
		// Token: 0x040092E4 RID: 37604
		CrouchSpeed,
		// Token: 0x040092E5 RID: 37605
		WalkSpeed,
		// Token: 0x040092E6 RID: 37606
		RunSpeed,
		// Token: 0x040092E7 RID: 37607
		JumpStrength,
		// Token: 0x040092E8 RID: 37608
		StaminaUsage,
		// Token: 0x040092E9 RID: 37609
		StaminaRegen,
		// Token: 0x040092EA RID: 37610
		PlayerLevelBonusApplied,
		// Token: 0x040092EB RID: 37611
		JarRefund,
		// Token: 0x040092EC RID: 37612
		ShowHealthBars,
		// Token: 0x040092ED RID: 37613
		ShowEnemyDamage,
		// Token: 0x040092EE RID: 37614
		NewbieCoat,
		// Token: 0x040092EF RID: 37615
		HeadshotMode,
		// Token: 0x040092F0 RID: 37616
		IncomingDamage,
		// Token: 0x040092F1 RID: 37617
		XPMultiplier,
		// Token: 0x040092F2 RID: 37618
		ShowXP,
		// Token: 0x040092F3 RID: 37619
		EncumbranceModifier,
		// Token: 0x040092F4 RID: 37620
		ItemDegradation,
		// Token: 0x040092F5 RID: 37621
		LoseItemsOnDeathType,
		// Token: 0x040092F6 RID: 37622
		LoseItemsOnDeathCount,
		// Token: 0x040092F7 RID: 37623
		DegradeItemsOnDeath,
		// Token: 0x040092F8 RID: 37624
		DegradeAmountOnDeath,
		// Token: 0x040092F9 RID: 37625
		DeathPenalty,
		// Token: 0x040092FA RID: 37626
		DropOnDeath,
		// Token: 0x040092FB RID: 37627
		DropOnQuit,
		// Token: 0x040092FC RID: 37628
		InfectionRate,
		// Token: 0x040092FD RID: 37629
		EnemySpawnMode,
		// Token: 0x040092FE RID: 37630
		EntityDamage,
		// Token: 0x040092FF RID: 37631
		BlockDamageAI,
		// Token: 0x04009300 RID: 37632
		BlockDamageAIBM,
		// Token: 0x04009301 RID: 37633
		ZombieMove,
		// Token: 0x04009302 RID: 37634
		ZombieMoveNight,
		// Token: 0x04009303 RID: 37635
		ZombieFeralMove,
		// Token: 0x04009304 RID: 37636
		ZombieBMMove,
		// Token: 0x04009305 RID: 37637
		ZombieFeralSense,
		// Token: 0x04009306 RID: 37638
		AISmellMode,
		// Token: 0x04009307 RID: 37639
		AllowZombieDigging,
		// Token: 0x04009308 RID: 37640
		ZombieRageChance,
		// Token: 0x04009309 RID: 37641
		EntityIncomingDamage,
		// Token: 0x0400930A RID: 37642
		MaxEnemyTier,
		// Token: 0x0400930B RID: 37643
		BiomeDayZombieRespawn,
		// Token: 0x0400930C RID: 37644
		BiomeDayAnimalRespawn,
		// Token: 0x0400930D RID: 37645
		BiomeDayEnemyDensity,
		// Token: 0x0400930E RID: 37646
		ZombiesEatAnimals,
		// Token: 0x0400930F RID: 37647
		BloodMoonFrequency,
		// Token: 0x04009310 RID: 37648
		BloodMoonRange,
		// Token: 0x04009311 RID: 37649
		BloodMoonWarning,
		// Token: 0x04009312 RID: 37650
		BloodMoonEnemyCount,
		// Token: 0x04009313 RID: 37651
		AirDropFrequency,
		// Token: 0x04009314 RID: 37652
		AirDropMarker,
		// Token: 0x04009315 RID: 37653
		AirDropRandomTime,
		// Token: 0x04009316 RID: 37654
		BiomeProgression,
		// Token: 0x04009317 RID: 37655
		TemperatureSurvival,
		// Token: 0x04009318 RID: 37656
		StormFreq,
		// Token: 0x04009319 RID: 37657
		StormWarning,
		// Token: 0x0400931A RID: 37658
		HeatMapSensitivity,
		// Token: 0x0400931B RID: 37659
		GlobalGSModifier,
		// Token: 0x0400931C RID: 37660
		BiomeGSModifier,
		// Token: 0x0400931D RID: 37661
		GlobalLSModifier,
		// Token: 0x0400931E RID: 37662
		BiomeLSModifier,
		// Token: 0x0400931F RID: 37663
		POITierLSModifier,
		// Token: 0x04009320 RID: 37664
		GlobalTSModifier,
		// Token: 0x04009321 RID: 37665
		DayNightLength,
		// Token: 0x04009322 RID: 37666
		DayLightLength,
		// Token: 0x04009323 RID: 37667
		AllowMap,
		// Token: 0x04009324 RID: 37668
		AllowCompass,
		// Token: 0x04009325 RID: 37669
		AllowScreenMarkers,
		// Token: 0x04009326 RID: 37670
		ShowLocationInfo,
		// Token: 0x04009327 RID: 37671
		ShowDayTime,
		// Token: 0x04009328 RID: 37672
		WorkstationsInTheWild,
		// Token: 0x04009329 RID: 37673
		MaxTechType,
		// Token: 0x0400932A RID: 37674
		LootRespawnDays,
		// Token: 0x0400932B RID: 37675
		LootTimer,
		// Token: 0x0400932C RID: 37676
		LootMaxTier,
		// Token: 0x0400932D RID: 37677
		GlobalLootCount,
		// Token: 0x0400932E RID: 37678
		FoodLootCount,
		// Token: 0x0400932F RID: 37679
		DrinkLootCount,
		// Token: 0x04009330 RID: 37680
		MedicalLootCount,
		// Token: 0x04009331 RID: 37681
		AmmoLootCount,
		// Token: 0x04009332 RID: 37682
		ResourceLootCount,
		// Token: 0x04009333 RID: 37683
		ArmorLootCount,
		// Token: 0x04009334 RID: 37684
		MeleeLootCount,
		// Token: 0x04009335 RID: 37685
		RangedLootCount,
		// Token: 0x04009336 RID: 37686
		DukesLootCount,
		// Token: 0x04009337 RID: 37687
		CraftingMagazinesLootCount,
		// Token: 0x04009338 RID: 37688
		TreasureMapChance,
		// Token: 0x04009339 RID: 37689
		LootBagChance,
		// Token: 0x0400933A RID: 37690
		CropOutput,
		// Token: 0x0400933B RID: 37691
		SeedDropOutput,
		// Token: 0x0400933C RID: 37692
		CropGrowthSpeed,
		// Token: 0x0400933D RID: 37693
		BackpackCrafting,
		// Token: 0x0400933E RID: 37694
		WorkstationCrafting,
		// Token: 0x0400933F RID: 37695
		CraftingProgression,
		// Token: 0x04009340 RID: 37696
		CraftingTime,
		// Token: 0x04009341 RID: 37697
		CraftingInput,
		// Token: 0x04009342 RID: 37698
		CraftingOutput,
		// Token: 0x04009343 RID: 37699
		CraftingMaxTier,
		// Token: 0x04009344 RID: 37700
		MiningOutput,
		// Token: 0x04009345 RID: 37701
		HarvestingOutput,
		// Token: 0x04009346 RID: 37702
		ScrappingOutput,
		// Token: 0x04009347 RID: 37703
		SmeltingType,
		// Token: 0x04009348 RID: 37704
		DewCollectorTime,
		// Token: 0x04009349 RID: 37705
		DewCollectorOutput,
		// Token: 0x0400934A RID: 37706
		DewCollectorInput,
		// Token: 0x0400934B RID: 37707
		ApiaryTime,
		// Token: 0x0400934C RID: 37708
		ApiaryOutput,
		// Token: 0x0400934D RID: 37709
		ApiaryInput,
		// Token: 0x0400934E RID: 37710
		BookLootCount,
		// Token: 0x0400934F RID: 37711
		CrouchRunSpeed,
		// Token: 0x04009350 RID: 37712
		RepairTypes,
		// Token: 0x04009351 RID: 37713
		MaxDegradationAmount,
		// Token: 0x04009352 RID: 37714
		PointsPerMagazine,
		// Token: 0x04009353 RID: 37715
		SkillGainRate,
		// Token: 0x04009354 RID: 37716
		SkillPointsPerLevel,
		// Token: 0x04009355 RID: 37717
		QuestsEnabled,
		// Token: 0x04009356 RID: 37718
		IntroQuestEnabled,
		// Token: 0x04009357 RID: 37719
		TraderToTraderQuestsEnabled,
		// Token: 0x04009358 RID: 37720
		StarterSkillPoints,
		// Token: 0x04009359 RID: 37721
		QuestsPerTier,
		// Token: 0x0400935A RID: 37722
		QuestProgressionDailyLimit,
		// Token: 0x0400935B RID: 37723
		BuriedQuestsEnabled,
		// Token: 0x0400935C RID: 37724
		POIQuestsEnabled,
		// Token: 0x0400935D RID: 37725
		TraderDialog,
		// Token: 0x0400935E RID: 37726
		TraderHours,
		// Token: 0x0400935F RID: 37727
		TradersEnabled,
		// Token: 0x04009360 RID: 37728
		VendingEnabled,
		// Token: 0x04009361 RID: 37729
		TraderSellPrices,
		// Token: 0x04009362 RID: 37730
		TraderBuyPrices,
		// Token: 0x04009363 RID: 37731
		TraderProtection,
		// Token: 0x04009364 RID: 37732
		TraderResetInterval,
		// Token: 0x04009365 RID: 37733
		TraderItemAbundance,
		// Token: 0x04009366 RID: 37734
		TraderBuyLimit,
		// Token: 0x04009367 RID: 37735
		TraderMaxTier,
		// Token: 0x04009368 RID: 37736
		VendingResetInterval,
		// Token: 0x04009369 RID: 37737
		VendingItemAbundance,
		// Token: 0x0400936A RID: 37738
		ChallengesEnabled,
		// Token: 0x0400936B RID: 37739
		IntroChallengesEnabled,
		// Token: 0x0400936C RID: 37740
		VehicleFuelUsage,
		// Token: 0x0400936D RID: 37741
		VehicleEntityDamage,
		// Token: 0x0400936E RID: 37742
		VehicleBlockDamage,
		// Token: 0x0400936F RID: 37743
		VehicleSelfDamage,
		// Token: 0x04009370 RID: 37744
		ElectricalOutput,
		// Token: 0x04009371 RID: 37745
		SillyCelebrate,
		// Token: 0x04009372 RID: 37746
		SillyBigHeads,
		// Token: 0x04009373 RID: 37747
		SillyTinyZombies,
		// Token: 0x04009374 RID: 37748
		SillySounds,
		// Token: 0x04009375 RID: 37749
		SillyLowGravity,
		// Token: 0x04009376 RID: 37750
		SillyBlackandWhite,
		// Token: 0x04009377 RID: 37751
		ChickenCoopTime,
		// Token: 0x04009378 RID: 37752
		ChickenCoopOutput,
		// Token: 0x04009379 RID: 37753
		ChickenCoopInput,
		// Token: 0x0400937A RID: 37754
		BiomeDayAnimalDensity,
		// Token: 0x0400937B RID: 37755
		BiomeNightZombieRespawn,
		// Token: 0x0400937C RID: 37756
		BiomeNightAnimalRespawn,
		// Token: 0x0400937D RID: 37757
		BiomeNightEnemyDensity,
		// Token: 0x0400937E RID: 37758
		BiomeNightAnimalDensity,
		// Token: 0x0400937F RID: 37759
		InfectionChance,
		// Token: 0x04009380 RID: 37760
		HungerMultiplier,
		// Token: 0x04009381 RID: 37761
		ThirstMultiplier,
		// Token: 0x04009382 RID: 37762
		StackSizeMultiplier,
		// Token: 0x04009383 RID: 37763
		FullChickenStressEvent,
		// Token: 0x04009384 RID: 37764
		Max
	}
}
