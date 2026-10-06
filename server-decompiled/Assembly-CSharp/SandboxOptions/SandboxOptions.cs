using System;

namespace SandboxOptions
{
	// Token: 0x020018C5 RID: 6341
	public enum SandboxOptions
	{
		// Token: 0x040093A5 RID: 37797
		RangedDamage,
		// Token: 0x040093A6 RID: 37798
		MeleeDamage,
		// Token: 0x040093A7 RID: 37799
		BlockDamage,
		// Token: 0x040093A8 RID: 37800
		TerrainDamage,
		// Token: 0x040093A9 RID: 37801
		HeadshotMultiplier,
		// Token: 0x040093AA RID: 37802
		CrouchSpeed,
		// Token: 0x040093AB RID: 37803
		WalkSpeed,
		// Token: 0x040093AC RID: 37804
		RunSpeed,
		// Token: 0x040093AD RID: 37805
		JumpStrength,
		// Token: 0x040093AE RID: 37806
		StaminaUsage,
		// Token: 0x040093AF RID: 37807
		StaminaRegen,
		// Token: 0x040093B0 RID: 37808
		PlayerLevelBonusApplied,
		// Token: 0x040093B1 RID: 37809
		JarRefund,
		// Token: 0x040093B2 RID: 37810
		ShowHealthBars,
		// Token: 0x040093B3 RID: 37811
		ShowEnemyDamage,
		// Token: 0x040093B4 RID: 37812
		NewbieCoat,
		// Token: 0x040093B5 RID: 37813
		HeadshotMode,
		// Token: 0x040093B6 RID: 37814
		IncomingDamage,
		// Token: 0x040093B7 RID: 37815
		XPMultiplier,
		// Token: 0x040093B8 RID: 37816
		ShowXP,
		// Token: 0x040093B9 RID: 37817
		EncumbranceModifier,
		// Token: 0x040093BA RID: 37818
		ItemDegradation,
		// Token: 0x040093BB RID: 37819
		LoseItemsOnDeathType,
		// Token: 0x040093BC RID: 37820
		LoseItemsOnDeathCount,
		// Token: 0x040093BD RID: 37821
		DegradeItemsOnDeath,
		// Token: 0x040093BE RID: 37822
		DegradeAmountOnDeath,
		// Token: 0x040093BF RID: 37823
		DeathPenalty,
		// Token: 0x040093C0 RID: 37824
		DropOnDeath,
		// Token: 0x040093C1 RID: 37825
		DropOnQuit,
		// Token: 0x040093C2 RID: 37826
		InfectionRate,
		// Token: 0x040093C3 RID: 37827
		EnemySpawnMode,
		// Token: 0x040093C4 RID: 37828
		EntityDamage,
		// Token: 0x040093C5 RID: 37829
		BlockDamageAI,
		// Token: 0x040093C6 RID: 37830
		BlockDamageAIBM,
		// Token: 0x040093C7 RID: 37831
		ZombieMove,
		// Token: 0x040093C8 RID: 37832
		ZombieMoveNight,
		// Token: 0x040093C9 RID: 37833
		ZombieFeralMove,
		// Token: 0x040093CA RID: 37834
		ZombieBMMove,
		// Token: 0x040093CB RID: 37835
		ZombieFeralSense,
		// Token: 0x040093CC RID: 37836
		AISmellMode,
		// Token: 0x040093CD RID: 37837
		AllowZombieDigging,
		// Token: 0x040093CE RID: 37838
		ZombieRageChance,
		// Token: 0x040093CF RID: 37839
		EntityIncomingDamage,
		// Token: 0x040093D0 RID: 37840
		MaxEnemyTier,
		// Token: 0x040093D1 RID: 37841
		BiomeDayZombieRespawn,
		// Token: 0x040093D2 RID: 37842
		BiomeDayAnimalRespawn,
		// Token: 0x040093D3 RID: 37843
		BiomeDayEnemyDensity,
		// Token: 0x040093D4 RID: 37844
		ZombiesEatAnimals,
		// Token: 0x040093D5 RID: 37845
		BloodMoonFrequency,
		// Token: 0x040093D6 RID: 37846
		BloodMoonRange,
		// Token: 0x040093D7 RID: 37847
		BloodMoonWarning,
		// Token: 0x040093D8 RID: 37848
		BloodMoonEnemyCount,
		// Token: 0x040093D9 RID: 37849
		AirDropFrequency,
		// Token: 0x040093DA RID: 37850
		AirDropMarker,
		// Token: 0x040093DB RID: 37851
		AirDropRandomTime,
		// Token: 0x040093DC RID: 37852
		BiomeProgression,
		// Token: 0x040093DD RID: 37853
		TemperatureSurvival,
		// Token: 0x040093DE RID: 37854
		StormFreq,
		// Token: 0x040093DF RID: 37855
		StormWarning,
		// Token: 0x040093E0 RID: 37856
		HeatMapSensitivity,
		// Token: 0x040093E1 RID: 37857
		GlobalGSModifier,
		// Token: 0x040093E2 RID: 37858
		BiomeGSModifier,
		// Token: 0x040093E3 RID: 37859
		GlobalLSModifier,
		// Token: 0x040093E4 RID: 37860
		BiomeLSModifier,
		// Token: 0x040093E5 RID: 37861
		POITierLSModifier,
		// Token: 0x040093E6 RID: 37862
		GlobalTSModifier,
		// Token: 0x040093E7 RID: 37863
		DayNightLength,
		// Token: 0x040093E8 RID: 37864
		DayLightLength,
		// Token: 0x040093E9 RID: 37865
		AllowMap,
		// Token: 0x040093EA RID: 37866
		AllowCompass,
		// Token: 0x040093EB RID: 37867
		AllowScreenMarkers,
		// Token: 0x040093EC RID: 37868
		ShowLocationInfo,
		// Token: 0x040093ED RID: 37869
		ShowDayTime,
		// Token: 0x040093EE RID: 37870
		WorkstationsInTheWild,
		// Token: 0x040093EF RID: 37871
		MaxTechType,
		// Token: 0x040093F0 RID: 37872
		LootRespawnDays,
		// Token: 0x040093F1 RID: 37873
		LootTimer,
		// Token: 0x040093F2 RID: 37874
		LootMaxTier,
		// Token: 0x040093F3 RID: 37875
		GlobalLootCount,
		// Token: 0x040093F4 RID: 37876
		FoodLootCount,
		// Token: 0x040093F5 RID: 37877
		DrinkLootCount,
		// Token: 0x040093F6 RID: 37878
		MedicalLootCount,
		// Token: 0x040093F7 RID: 37879
		AmmoLootCount,
		// Token: 0x040093F8 RID: 37880
		ResourceLootCount,
		// Token: 0x040093F9 RID: 37881
		ArmorLootCount,
		// Token: 0x040093FA RID: 37882
		MeleeLootCount,
		// Token: 0x040093FB RID: 37883
		RangedLootCount,
		// Token: 0x040093FC RID: 37884
		DukesLootCount,
		// Token: 0x040093FD RID: 37885
		CraftingMagazinesLootCount,
		// Token: 0x040093FE RID: 37886
		TreasureMapChance,
		// Token: 0x040093FF RID: 37887
		LootBagChance,
		// Token: 0x04009400 RID: 37888
		CropOutput,
		// Token: 0x04009401 RID: 37889
		SeedDropOutput,
		// Token: 0x04009402 RID: 37890
		CropGrowthSpeed,
		// Token: 0x04009403 RID: 37891
		BackpackCrafting,
		// Token: 0x04009404 RID: 37892
		WorkstationCrafting,
		// Token: 0x04009405 RID: 37893
		CraftingProgression,
		// Token: 0x04009406 RID: 37894
		CraftingTime,
		// Token: 0x04009407 RID: 37895
		CraftingInput,
		// Token: 0x04009408 RID: 37896
		CraftingOutput,
		// Token: 0x04009409 RID: 37897
		CraftingMaxTier,
		// Token: 0x0400940A RID: 37898
		MiningOutput,
		// Token: 0x0400940B RID: 37899
		HarvestingOutput,
		// Token: 0x0400940C RID: 37900
		ScrappingOutput,
		// Token: 0x0400940D RID: 37901
		SmeltingType,
		// Token: 0x0400940E RID: 37902
		DewCollectorTime,
		// Token: 0x0400940F RID: 37903
		DewCollectorOutput,
		// Token: 0x04009410 RID: 37904
		DewCollectorInput,
		// Token: 0x04009411 RID: 37905
		ApiaryTime,
		// Token: 0x04009412 RID: 37906
		ApiaryOutput,
		// Token: 0x04009413 RID: 37907
		ApiaryInput,
		// Token: 0x04009414 RID: 37908
		BookLootCount,
		// Token: 0x04009415 RID: 37909
		CrouchRunSpeed,
		// Token: 0x04009416 RID: 37910
		RepairTypes,
		// Token: 0x04009417 RID: 37911
		MaxDegradationAmount,
		// Token: 0x04009418 RID: 37912
		PointsPerMagazine,
		// Token: 0x04009419 RID: 37913
		SkillGainRate,
		// Token: 0x0400941A RID: 37914
		SkillPointsPerLevel,
		// Token: 0x0400941B RID: 37915
		QuestsEnabled,
		// Token: 0x0400941C RID: 37916
		IntroQuestEnabled,
		// Token: 0x0400941D RID: 37917
		TraderToTraderQuestsEnabled,
		// Token: 0x0400941E RID: 37918
		StarterSkillPoints,
		// Token: 0x0400941F RID: 37919
		QuestsPerTier,
		// Token: 0x04009420 RID: 37920
		QuestProgressionDailyLimit,
		// Token: 0x04009421 RID: 37921
		BuriedQuestsEnabled,
		// Token: 0x04009422 RID: 37922
		POIQuestsEnabled,
		// Token: 0x04009423 RID: 37923
		TraderDialog,
		// Token: 0x04009424 RID: 37924
		TraderHours,
		// Token: 0x04009425 RID: 37925
		TradersEnabled,
		// Token: 0x04009426 RID: 37926
		VendingEnabled,
		// Token: 0x04009427 RID: 37927
		TraderSellPrices,
		// Token: 0x04009428 RID: 37928
		TraderBuyPrices,
		// Token: 0x04009429 RID: 37929
		TraderProtection,
		// Token: 0x0400942A RID: 37930
		TraderResetInterval,
		// Token: 0x0400942B RID: 37931
		TraderItemAbundance,
		// Token: 0x0400942C RID: 37932
		TraderBuyLimit,
		// Token: 0x0400942D RID: 37933
		TraderMaxTier,
		// Token: 0x0400942E RID: 37934
		VendingResetInterval,
		// Token: 0x0400942F RID: 37935
		VendingItemAbundance,
		// Token: 0x04009430 RID: 37936
		ChallengesEnabled,
		// Token: 0x04009431 RID: 37937
		IntroChallengesEnabled,
		// Token: 0x04009432 RID: 37938
		VehicleFuelUsage,
		// Token: 0x04009433 RID: 37939
		VehicleEntityDamage,
		// Token: 0x04009434 RID: 37940
		VehicleBlockDamage,
		// Token: 0x04009435 RID: 37941
		VehicleSelfDamage,
		// Token: 0x04009436 RID: 37942
		ElectricalOutput,
		// Token: 0x04009437 RID: 37943
		SillyCelebrate,
		// Token: 0x04009438 RID: 37944
		SillyBigHeads,
		// Token: 0x04009439 RID: 37945
		SillyTinyZombies,
		// Token: 0x0400943A RID: 37946
		SillySounds,
		// Token: 0x0400943B RID: 37947
		SillyLowGravity,
		// Token: 0x0400943C RID: 37948
		SillyBlackandWhite,
		// Token: 0x0400943D RID: 37949
		ChickenCoopTime,
		// Token: 0x0400943E RID: 37950
		ChickenCoopOutput,
		// Token: 0x0400943F RID: 37951
		ChickenCoopInput,
		// Token: 0x04009440 RID: 37952
		BiomeDayAnimalDensity,
		// Token: 0x04009441 RID: 37953
		BiomeNightZombieRespawn,
		// Token: 0x04009442 RID: 37954
		BiomeNightAnimalRespawn,
		// Token: 0x04009443 RID: 37955
		BiomeNightEnemyDensity,
		// Token: 0x04009444 RID: 37956
		BiomeNightAnimalDensity,
		// Token: 0x04009445 RID: 37957
		InfectionChance,
		// Token: 0x04009446 RID: 37958
		HungerMultiplier,
		// Token: 0x04009447 RID: 37959
		ThirstMultiplier,
		// Token: 0x04009448 RID: 37960
		StackSizeMultiplier,
		// Token: 0x04009449 RID: 37961
		FullChickenStressEvent,
		// Token: 0x0400944A RID: 37962
		Max
	}
}
