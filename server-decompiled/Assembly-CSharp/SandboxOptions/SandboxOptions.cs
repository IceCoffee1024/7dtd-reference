using System;

namespace SandboxOptions
{
	// Token: 0x02001873 RID: 6259
	public enum SandboxOptions
	{
		// Token: 0x040091C3 RID: 37315
		RangedDamage,
		// Token: 0x040091C4 RID: 37316
		MeleeDamage,
		// Token: 0x040091C5 RID: 37317
		BlockDamage,
		// Token: 0x040091C6 RID: 37318
		TerrainDamage,
		// Token: 0x040091C7 RID: 37319
		HeadshotMultiplier,
		// Token: 0x040091C8 RID: 37320
		CrouchSpeed,
		// Token: 0x040091C9 RID: 37321
		WalkSpeed,
		// Token: 0x040091CA RID: 37322
		RunSpeed,
		// Token: 0x040091CB RID: 37323
		JumpStrength,
		// Token: 0x040091CC RID: 37324
		StaminaUsage,
		// Token: 0x040091CD RID: 37325
		StaminaRegen,
		// Token: 0x040091CE RID: 37326
		PlayerLevelBonusApplied,
		// Token: 0x040091CF RID: 37327
		JarRefund,
		// Token: 0x040091D0 RID: 37328
		ShowHealthBars,
		// Token: 0x040091D1 RID: 37329
		ShowEnemyDamage,
		// Token: 0x040091D2 RID: 37330
		NewbieCoat,
		// Token: 0x040091D3 RID: 37331
		HeadshotMode,
		// Token: 0x040091D4 RID: 37332
		IncomingDamage,
		// Token: 0x040091D5 RID: 37333
		XPMultiplier,
		// Token: 0x040091D6 RID: 37334
		ShowXP,
		// Token: 0x040091D7 RID: 37335
		EncumbranceModifier,
		// Token: 0x040091D8 RID: 37336
		ItemDegradation,
		// Token: 0x040091D9 RID: 37337
		LoseItemsOnDeathType,
		// Token: 0x040091DA RID: 37338
		LoseItemsOnDeathCount,
		// Token: 0x040091DB RID: 37339
		DegradeItemsOnDeath,
		// Token: 0x040091DC RID: 37340
		DegradeAmountOnDeath,
		// Token: 0x040091DD RID: 37341
		DeathPenalty,
		// Token: 0x040091DE RID: 37342
		DropOnDeath,
		// Token: 0x040091DF RID: 37343
		DropOnQuit,
		// Token: 0x040091E0 RID: 37344
		InfectionRate,
		// Token: 0x040091E1 RID: 37345
		EnemySpawnMode,
		// Token: 0x040091E2 RID: 37346
		EntityDamage,
		// Token: 0x040091E3 RID: 37347
		BlockDamageAI,
		// Token: 0x040091E4 RID: 37348
		BlockDamageAIBM,
		// Token: 0x040091E5 RID: 37349
		ZombieMove,
		// Token: 0x040091E6 RID: 37350
		ZombieMoveNight,
		// Token: 0x040091E7 RID: 37351
		ZombieFeralMove,
		// Token: 0x040091E8 RID: 37352
		ZombieBMMove,
		// Token: 0x040091E9 RID: 37353
		ZombieFeralSense,
		// Token: 0x040091EA RID: 37354
		AISmellMode,
		// Token: 0x040091EB RID: 37355
		AllowZombieDigging,
		// Token: 0x040091EC RID: 37356
		ZombieRageChance,
		// Token: 0x040091ED RID: 37357
		EntityIncomingDamage,
		// Token: 0x040091EE RID: 37358
		MaxEnemyTier,
		// Token: 0x040091EF RID: 37359
		BiomeZombieRespawn,
		// Token: 0x040091F0 RID: 37360
		BiomeAnimalRespawn,
		// Token: 0x040091F1 RID: 37361
		BiomeEnemyDensity,
		// Token: 0x040091F2 RID: 37362
		ZombiesEatAnimals,
		// Token: 0x040091F3 RID: 37363
		BloodMoonFrequency,
		// Token: 0x040091F4 RID: 37364
		BloodMoonRange,
		// Token: 0x040091F5 RID: 37365
		BloodMoonWarning,
		// Token: 0x040091F6 RID: 37366
		BloodMoonEnemyCount,
		// Token: 0x040091F7 RID: 37367
		AirDropFrequency,
		// Token: 0x040091F8 RID: 37368
		AirDropMarker,
		// Token: 0x040091F9 RID: 37369
		AirDropRandomTime,
		// Token: 0x040091FA RID: 37370
		BiomeProgression,
		// Token: 0x040091FB RID: 37371
		TemperatureSurvival,
		// Token: 0x040091FC RID: 37372
		StormFreq,
		// Token: 0x040091FD RID: 37373
		StormWarning,
		// Token: 0x040091FE RID: 37374
		HeatMapSensitivity,
		// Token: 0x040091FF RID: 37375
		GlobalGSModifier,
		// Token: 0x04009200 RID: 37376
		BiomeGSModifier,
		// Token: 0x04009201 RID: 37377
		GlobalLSModifier,
		// Token: 0x04009202 RID: 37378
		BiomeLSModifier,
		// Token: 0x04009203 RID: 37379
		POITierLSModifier,
		// Token: 0x04009204 RID: 37380
		GlobalTSModifier,
		// Token: 0x04009205 RID: 37381
		DayNightLength,
		// Token: 0x04009206 RID: 37382
		DayLightLength,
		// Token: 0x04009207 RID: 37383
		AllowMap,
		// Token: 0x04009208 RID: 37384
		AllowCompass,
		// Token: 0x04009209 RID: 37385
		AllowScreenMarkers,
		// Token: 0x0400920A RID: 37386
		ShowLocationInfo,
		// Token: 0x0400920B RID: 37387
		ShowDayTime,
		// Token: 0x0400920C RID: 37388
		WorkstationsInTheWild,
		// Token: 0x0400920D RID: 37389
		MaxTechType,
		// Token: 0x0400920E RID: 37390
		LootRespawnDays,
		// Token: 0x0400920F RID: 37391
		LootTimer,
		// Token: 0x04009210 RID: 37392
		LootMaxTier,
		// Token: 0x04009211 RID: 37393
		GlobalLootCount,
		// Token: 0x04009212 RID: 37394
		FoodLootCount,
		// Token: 0x04009213 RID: 37395
		DrinkLootCount,
		// Token: 0x04009214 RID: 37396
		MedicalLootCount,
		// Token: 0x04009215 RID: 37397
		AmmoLootCount,
		// Token: 0x04009216 RID: 37398
		ResourceLootCount,
		// Token: 0x04009217 RID: 37399
		ArmorLootCount,
		// Token: 0x04009218 RID: 37400
		MeleeLootCount,
		// Token: 0x04009219 RID: 37401
		RangedLootCount,
		// Token: 0x0400921A RID: 37402
		DukesLootCount,
		// Token: 0x0400921B RID: 37403
		CraftingMagazinesLootCount,
		// Token: 0x0400921C RID: 37404
		TreasureMapChance,
		// Token: 0x0400921D RID: 37405
		LootBagChance,
		// Token: 0x0400921E RID: 37406
		CropOutput,
		// Token: 0x0400921F RID: 37407
		SeedDropOutput,
		// Token: 0x04009220 RID: 37408
		CropGrowthSpeed,
		// Token: 0x04009221 RID: 37409
		BackpackCrafting,
		// Token: 0x04009222 RID: 37410
		WorkstationCrafting,
		// Token: 0x04009223 RID: 37411
		CraftingProgression,
		// Token: 0x04009224 RID: 37412
		CraftingTime,
		// Token: 0x04009225 RID: 37413
		CraftingInput,
		// Token: 0x04009226 RID: 37414
		CraftingOutput,
		// Token: 0x04009227 RID: 37415
		CraftingMaxTier,
		// Token: 0x04009228 RID: 37416
		MiningOutput,
		// Token: 0x04009229 RID: 37417
		HarvestingOutput,
		// Token: 0x0400922A RID: 37418
		ScrappingOutput,
		// Token: 0x0400922B RID: 37419
		SmeltingType,
		// Token: 0x0400922C RID: 37420
		DewCollectorTime,
		// Token: 0x0400922D RID: 37421
		DewCollectorOutput,
		// Token: 0x0400922E RID: 37422
		DewCollectorInput,
		// Token: 0x0400922F RID: 37423
		ApiaryTime,
		// Token: 0x04009230 RID: 37424
		ApiaryOutput,
		// Token: 0x04009231 RID: 37425
		ApiaryInput,
		// Token: 0x04009232 RID: 37426
		BookLootCount,
		// Token: 0x04009233 RID: 37427
		CrouchRunSpeed,
		// Token: 0x04009234 RID: 37428
		RepairTypes,
		// Token: 0x04009235 RID: 37429
		MaxDegradationAmount,
		// Token: 0x04009236 RID: 37430
		PointsPerMagazine,
		// Token: 0x04009237 RID: 37431
		SkillGainRate,
		// Token: 0x04009238 RID: 37432
		SkillPointsPerLevel,
		// Token: 0x04009239 RID: 37433
		QuestsEnabled,
		// Token: 0x0400923A RID: 37434
		IntroQuestEnabled,
		// Token: 0x0400923B RID: 37435
		TraderToTraderQuestsEnabled,
		// Token: 0x0400923C RID: 37436
		StarterSkillPoints,
		// Token: 0x0400923D RID: 37437
		QuestsPerTier,
		// Token: 0x0400923E RID: 37438
		QuestProgressionDailyLimit,
		// Token: 0x0400923F RID: 37439
		BuriedQuestsEnabled,
		// Token: 0x04009240 RID: 37440
		POIQuestsEnabled,
		// Token: 0x04009241 RID: 37441
		TraderDialog,
		// Token: 0x04009242 RID: 37442
		TraderHours,
		// Token: 0x04009243 RID: 37443
		TradersEnabled,
		// Token: 0x04009244 RID: 37444
		VendingEnabled,
		// Token: 0x04009245 RID: 37445
		TraderSellPrices,
		// Token: 0x04009246 RID: 37446
		TraderBuyPrices,
		// Token: 0x04009247 RID: 37447
		TraderProtection,
		// Token: 0x04009248 RID: 37448
		TraderResetInterval,
		// Token: 0x04009249 RID: 37449
		TraderItemAbundance,
		// Token: 0x0400924A RID: 37450
		TraderBuyLimit,
		// Token: 0x0400924B RID: 37451
		TraderMaxTier,
		// Token: 0x0400924C RID: 37452
		VendingResetInterval,
		// Token: 0x0400924D RID: 37453
		VendingItemAbundance,
		// Token: 0x0400924E RID: 37454
		ChallengesEnabled,
		// Token: 0x0400924F RID: 37455
		IntroChallengesEnabled,
		// Token: 0x04009250 RID: 37456
		VehicleFuelUsage,
		// Token: 0x04009251 RID: 37457
		VehicleEntityDamage,
		// Token: 0x04009252 RID: 37458
		VehicleBlockDamage,
		// Token: 0x04009253 RID: 37459
		VehicleSelfDamage,
		// Token: 0x04009254 RID: 37460
		ElectricalOutput,
		// Token: 0x04009255 RID: 37461
		SillyCelebrate,
		// Token: 0x04009256 RID: 37462
		SillyBigHeads,
		// Token: 0x04009257 RID: 37463
		SillyTinyZombies,
		// Token: 0x04009258 RID: 37464
		SillySounds,
		// Token: 0x04009259 RID: 37465
		SillyLowGravity,
		// Token: 0x0400925A RID: 37466
		SillyBlackandWhite,
		// Token: 0x0400925B RID: 37467
		Max
	}
}
