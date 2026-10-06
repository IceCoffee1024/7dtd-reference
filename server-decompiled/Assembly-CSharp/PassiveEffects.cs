using System;

// Token: 0x02000658 RID: 1624
public enum PassiveEffects : byte
{
	// Token: 0x040029D4 RID: 10708
	None,
	// Token: 0x040029D5 RID: 10709
	EntityDamage,
	// Token: 0x040029D6 RID: 10710
	EntityHeal,
	// Token: 0x040029D7 RID: 10711
	BlockDamage,
	// Token: 0x040029D8 RID: 10712
	BlockRepairAmount,
	// Token: 0x040029D9 RID: 10713
	DamageModifier,
	// Token: 0x040029DA RID: 10714
	LandClaimDamageModifier,
	// Token: 0x040029DB RID: 10715
	DegradationPerUse,
	// Token: 0x040029DC RID: 10716
	DegradationMax,
	// Token: 0x040029DD RID: 10717
	MagazineSize,
	// Token: 0x040029DE RID: 10718
	DamageFalloffRange,
	// Token: 0x040029DF RID: 10719
	MaxRange,
	// Token: 0x040029E0 RID: 10720
	BlockRange,
	// Token: 0x040029E1 RID: 10721
	WeaponHandling,
	// Token: 0x040029E2 RID: 10722
	IncrementalSpreadMultiplier,
	// Token: 0x040029E3 RID: 10723
	BurstRoundCount,
	// Token: 0x040029E4 RID: 10724
	RoundRayCount,
	// Token: 0x040029E5 RID: 10725
	RoundsPerMinute,
	// Token: 0x040029E6 RID: 10726
	AttacksPerMinute,
	// Token: 0x040029E7 RID: 10727
	ExplosionBlockDamage,
	// Token: 0x040029E8 RID: 10728
	ExplosionEntityDamage,
	// Token: 0x040029E9 RID: 10729
	ExplosionRadius,
	// Token: 0x040029EA RID: 10730
	ExplosionIncomingDamage,
	// Token: 0x040029EB RID: 10731
	ModSlots,
	// Token: 0x040029EC RID: 10732
	ModPowerBonus,
	// Token: 0x040029ED RID: 10733
	SpreadMultiplierHip,
	// Token: 0x040029EE RID: 10734
	SpreadMultiplierAiming,
	// Token: 0x040029EF RID: 10735
	SpreadMultiplierRunning,
	// Token: 0x040029F0 RID: 10736
	SpreadMultiplierWalking,
	// Token: 0x040029F1 RID: 10737
	SpreadMultiplierCrouching,
	// Token: 0x040029F2 RID: 10738
	SpreadMultiplierIdle,
	// Token: 0x040029F3 RID: 10739
	SpreadDegreesVertical,
	// Token: 0x040029F4 RID: 10740
	SpreadDegreesHorizontal,
	// Token: 0x040029F5 RID: 10741
	KickDegreesVerticalMin,
	// Token: 0x040029F6 RID: 10742
	KickDegreesHorizontalMin,
	// Token: 0x040029F7 RID: 10743
	KickDegreesVerticalMax,
	// Token: 0x040029F8 RID: 10744
	KickDegreesHorizontalMax,
	// Token: 0x040029F9 RID: 10745
	SphereCastRadius,
	// Token: 0x040029FA RID: 10746
	GrazeDamageMultiplier,
	// Token: 0x040029FB RID: 10747
	GrazeStaminaMultiplier,
	// Token: 0x040029FC RID: 10748
	GeneralDamageResist,
	// Token: 0x040029FD RID: 10749
	PhysicalDamageResist,
	// Token: 0x040029FE RID: 10750
	PhysicalDamageResistMax,
	// Token: 0x040029FF RID: 10751
	ElementalDamageResist,
	// Token: 0x04002A00 RID: 10752
	ElementalDamageResistMax,
	// Token: 0x04002A01 RID: 10753
	HypothermalResist,
	// Token: 0x04002A02 RID: 10754
	HyperthermalResist,
	// Token: 0x04002A03 RID: 10755
	FallDamageReduction,
	// Token: 0x04002A04 RID: 10756
	Tier,
	// Token: 0x04002A05 RID: 10757
	LightIntensity,
	// Token: 0x04002A06 RID: 10758
	VehicleFuelMaxPer,
	// Token: 0x04002A07 RID: 10759
	VehicleFuelUsePer,
	// Token: 0x04002A08 RID: 10760
	VehicleVelocityMaxPer,
	// Token: 0x04002A09 RID: 10761
	VehicleMotorTorquePer,
	// Token: 0x04002A0A RID: 10762
	VehicleCarryCapacity,
	// Token: 0x04002A0B RID: 10763
	VehicleEntityDamage,
	// Token: 0x04002A0C RID: 10764
	VehicleBlockDamage,
	// Token: 0x04002A0D RID: 10765
	VehicleSelfDamage,
	// Token: 0x04002A0E RID: 10766
	VehicleStrongSelfDamage,
	// Token: 0x04002A0F RID: 10767
	VehicleSeats,
	// Token: 0x04002A10 RID: 10768
	VehicleTankSize,
	// Token: 0x04002A11 RID: 10769
	VehicleHopStrength,
	// Token: 0x04002A12 RID: 10770
	VehiclePlayerStaminaDrainRate,
	// Token: 0x04002A13 RID: 10771
	BatteryMaxLoadInVolts,
	// Token: 0x04002A14 RID: 10772
	BatteryDischargeTimeInMinutes,
	// Token: 0x04002A15 RID: 10773
	DistractionResistance,
	// Token: 0x04002A16 RID: 10774
	DistractionRadius,
	// Token: 0x04002A17 RID: 10775
	DistractionLifetime,
	// Token: 0x04002A18 RID: 10776
	DistractionStrength,
	// Token: 0x04002A19 RID: 10777
	DistractionEatTicks,
	// Token: 0x04002A1A RID: 10778
	ProjectileGravity,
	// Token: 0x04002A1B RID: 10779
	ProjectileVelocity,
	// Token: 0x04002A1C RID: 10780
	ProjectileStickChance,
	// Token: 0x04002A1D RID: 10781
	RecipeTagUnlocked,
	// Token: 0x04002A1E RID: 10782
	JunkTurretActiveRange,
	// Token: 0x04002A1F RID: 10783
	JunkTurretActiveCount,
	// Token: 0x04002A20 RID: 10784
	EconomicValue,
	// Token: 0x04002A21 RID: 10785
	LockPickTime,
	// Token: 0x04002A22 RID: 10786
	LockPickBreakChance,
	// Token: 0x04002A23 RID: 10787
	LootProb,
	// Token: 0x04002A24 RID: 10788
	LootDropProb,
	// Token: 0x04002A25 RID: 10789
	LootQuantity,
	// Token: 0x04002A26 RID: 10790
	Tracking,
	// Token: 0x04002A27 RID: 10791
	AttributeLevel,
	// Token: 0x04002A28 RID: 10792
	SkillLevel,
	// Token: 0x04002A29 RID: 10793
	PerkLevel,
	// Token: 0x04002A2A RID: 10794
	SkillExpGain,
	// Token: 0x04002A2B RID: 10795
	PlayerExpGain,
	// Token: 0x04002A2C RID: 10796
	NoiseMultiplier,
	// Token: 0x04002A2D RID: 10797
	LightMultiplier,
	// Token: 0x04002A2E RID: 10798
	CraftingTime,
	// Token: 0x04002A2F RID: 10799
	CraftingTier,
	// Token: 0x04002A30 RID: 10800
	CraftingOutputCount,
	// Token: 0x04002A31 RID: 10801
	ActiveCraftingSlots,
	// Token: 0x04002A32 RID: 10802
	CraftingSlots,
	// Token: 0x04002A33 RID: 10803
	CraftingSmeltTime,
	// Token: 0x04002A34 RID: 10804
	ExpDeficitMaxPercentage,
	// Token: 0x04002A35 RID: 10805
	ExpDeficitPerDeathPercentage,
	// Token: 0x04002A36 RID: 10806
	CarryCapacity,
	// Token: 0x04002A37 RID: 10807
	BagSize,
	// Token: 0x04002A38 RID: 10808
	RepairTime,
	// Token: 0x04002A39 RID: 10809
	RepairAmount,
	// Token: 0x04002A3A RID: 10810
	ReloadSpeedMultiplier,
	// Token: 0x04002A3B RID: 10811
	WaterRegenRate,
	// Token: 0x04002A3C RID: 10812
	HealthMax,
	// Token: 0x04002A3D RID: 10813
	HealthChangeOT,
	// Token: 0x04002A3E RID: 10814
	HealthGain,
	// Token: 0x04002A3F RID: 10815
	HealthLoss,
	// Token: 0x04002A40 RID: 10816
	HealthMaxBlockage,
	// Token: 0x04002A41 RID: 10817
	StaminaMax,
	// Token: 0x04002A42 RID: 10818
	StaminaChangeOT,
	// Token: 0x04002A43 RID: 10819
	StaminaGain,
	// Token: 0x04002A44 RID: 10820
	StaminaLoss,
	// Token: 0x04002A45 RID: 10821
	StaminaMaxBlockage,
	// Token: 0x04002A46 RID: 10822
	FoodMax,
	// Token: 0x04002A47 RID: 10823
	FoodChangeOT,
	// Token: 0x04002A48 RID: 10824
	FoodGain,
	// Token: 0x04002A49 RID: 10825
	FoodLoss,
	// Token: 0x04002A4A RID: 10826
	FoodLossPerHealthPointLost,
	// Token: 0x04002A4B RID: 10827
	FoodLossPerStaminaPointGained,
	// Token: 0x04002A4C RID: 10828
	FoodLossPerHealthPointGained,
	// Token: 0x04002A4D RID: 10829
	FoodMaxBlockage,
	// Token: 0x04002A4E RID: 10830
	WaterMax,
	// Token: 0x04002A4F RID: 10831
	WaterChangeOT,
	// Token: 0x04002A50 RID: 10832
	WaterGain,
	// Token: 0x04002A51 RID: 10833
	WaterLoss,
	// Token: 0x04002A52 RID: 10834
	WaterLossPerHealthPointGained,
	// Token: 0x04002A53 RID: 10835
	WaterLossPerStaminaPointGained,
	// Token: 0x04002A54 RID: 10836
	WaterMaxBlockage,
	// Token: 0x04002A55 RID: 10837
	CoreTempChangeOT,
	// Token: 0x04002A56 RID: 10838
	CoreTempGain,
	// Token: 0x04002A57 RID: 10839
	CoreTempLoss,
	// Token: 0x04002A58 RID: 10840
	JumpStrength,
	// Token: 0x04002A59 RID: 10841
	WalkSpeed,
	// Token: 0x04002A5A RID: 10842
	RunSpeed,
	// Token: 0x04002A5B RID: 10843
	CrouchSpeed,
	// Token: 0x04002A5C RID: 10844
	Mobility,
	// Token: 0x04002A5D RID: 10845
	LandMineImmunity,
	// Token: 0x04002A5E RID: 10846
	ScavengingTime,
	// Token: 0x04002A5F RID: 10847
	ScavengingTier,
	// Token: 0x04002A60 RID: 10848
	ScavengingItemCount,
	// Token: 0x04002A61 RID: 10849
	HarvestCount,
	// Token: 0x04002A62 RID: 10850
	ScrappingTime,
	// Token: 0x04002A63 RID: 10851
	DismemberSelfChance,
	// Token: 0x04002A64 RID: 10852
	DismemberChance,
	// Token: 0x04002A65 RID: 10853
	TreasureRadius,
	// Token: 0x04002A66 RID: 10854
	TreasureBlocksPerReduction,
	// Token: 0x04002A67 RID: 10855
	BreathHoldDuration,
	// Token: 0x04002A68 RID: 10856
	BarteringBuying,
	// Token: 0x04002A69 RID: 10857
	BarteringSelling,
	// Token: 0x04002A6A RID: 10858
	SecretStash,
	// Token: 0x04002A6B RID: 10859
	LootTier,
	// Token: 0x04002A6C RID: 10860
	HeatGain,
	// Token: 0x04002A6D RID: 10861
	MovementFactorMultiplier,
	// Token: 0x04002A6E RID: 10862
	QuestBonusItemReward,
	// Token: 0x04002A6F RID: 10863
	QuestRewardOptionCount,
	// Token: 0x04002A70 RID: 10864
	QuestRewardChoiceCount,
	// Token: 0x04002A71 RID: 10865
	GameStage,
	// Token: 0x04002A72 RID: 10866
	TraderStage,
	// Token: 0x04002A73 RID: 10867
	LootStage,
	// Token: 0x04002A74 RID: 10868
	LootStageMax,
	// Token: 0x04002A75 RID: 10869
	DamageBonus,
	// Token: 0x04002A76 RID: 10870
	InternalDamageModifier,
	// Token: 0x04002A77 RID: 10871
	TargetArmor,
	// Token: 0x04002A78 RID: 10872
	FallingBlockDamage,
	// Token: 0x04002A79 RID: 10873
	SilenceBlockSteps,
	// Token: 0x04002A7A RID: 10874
	TrackDistance,
	// Token: 0x04002A7B RID: 10875
	HealthSteal,
	// Token: 0x04002A7C RID: 10876
	TurretWakeUp,
	// Token: 0x04002A7D RID: 10877
	ElectricalTrapXP,
	// Token: 0x04002A7E RID: 10878
	TrapDoorTriggerDelay,
	// Token: 0x04002A7F RID: 10879
	LandMineTriggerDelay,
	// Token: 0x04002A80 RID: 10880
	TrapIncomingDamage,
	// Token: 0x04002A81 RID: 10881
	BlockPickup,
	// Token: 0x04002A82 RID: 10882
	HeadShotOnly,
	// Token: 0x04002A83 RID: 10883
	NegateDamageSelf,
	// Token: 0x04002A84 RID: 10884
	NegateDamageOther,
	// Token: 0x04002A85 RID: 10885
	DisableItem,
	// Token: 0x04002A86 RID: 10886
	DisableLoot,
	// Token: 0x04002A87 RID: 10887
	NoVehicle,
	// Token: 0x04002A88 RID: 10888
	FlipControls,
	// Token: 0x04002A89 RID: 10889
	CelebrationKill,
	// Token: 0x04002A8A RID: 10890
	ShuffledBackpack,
	// Token: 0x04002A8B RID: 10891
	EnemySearchDuration,
	// Token: 0x04002A8C RID: 10892
	TwitchViewerPointRate,
	// Token: 0x04002A8D RID: 10893
	TwitchSpawnMultiplier,
	// Token: 0x04002A8E RID: 10894
	TwitchAddCooldown,
	// Token: 0x04002A8F RID: 10895
	TwitchAddPimpPot,
	// Token: 0x04002A90 RID: 10896
	InfiniteAmmo,
	// Token: 0x04002A91 RID: 10897
	NoTimeDisplay,
	// Token: 0x04002A92 RID: 10898
	DisableGameEventNotify,
	// Token: 0x04002A93 RID: 10899
	NoTrader,
	// Token: 0x04002A94 RID: 10900
	DisableMovement,
	// Token: 0x04002A95 RID: 10901
	ExperienceGain,
	// Token: 0x04002A96 RID: 10902
	CriticalChance,
	// Token: 0x04002A97 RID: 10903
	BuffProcChance,
	// Token: 0x04002A98 RID: 10904
	BuffBlink,
	// Token: 0x04002A99 RID: 10905
	BuffResistance,
	// Token: 0x04002A9A RID: 10906
	CraftingIngredientCount,
	// Token: 0x04002A9B RID: 10907
	EntityPenetrationCount,
	// Token: 0x04002A9C RID: 10908
	BlockPenetrationFactor,
	// Token: 0x04002A9D RID: 10909
	JunkDroneModCooldown,
	// Token: 0x04002A9E RID: 10910
	HeadshotDamageModifier,
	// Token: 0x04002A9F RID: 10911
	Count
}
