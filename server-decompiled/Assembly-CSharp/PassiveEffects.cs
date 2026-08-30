using System;

// Token: 0x02000656 RID: 1622
public enum PassiveEffects : byte
{
	// Token: 0x040029DB RID: 10715
	None,
	// Token: 0x040029DC RID: 10716
	EntityDamage,
	// Token: 0x040029DD RID: 10717
	EntityHeal,
	// Token: 0x040029DE RID: 10718
	BlockDamage,
	// Token: 0x040029DF RID: 10719
	BlockRepairAmount,
	// Token: 0x040029E0 RID: 10720
	DamageModifier,
	// Token: 0x040029E1 RID: 10721
	LandClaimDamageModifier,
	// Token: 0x040029E2 RID: 10722
	DegradationPerUse,
	// Token: 0x040029E3 RID: 10723
	DegradationMax,
	// Token: 0x040029E4 RID: 10724
	MagazineSize,
	// Token: 0x040029E5 RID: 10725
	DamageFalloffRange,
	// Token: 0x040029E6 RID: 10726
	MaxRange,
	// Token: 0x040029E7 RID: 10727
	BlockRange,
	// Token: 0x040029E8 RID: 10728
	WeaponHandling,
	// Token: 0x040029E9 RID: 10729
	IncrementalSpreadMultiplier,
	// Token: 0x040029EA RID: 10730
	BurstRoundCount,
	// Token: 0x040029EB RID: 10731
	RoundRayCount,
	// Token: 0x040029EC RID: 10732
	RoundsPerMinute,
	// Token: 0x040029ED RID: 10733
	AttacksPerMinute,
	// Token: 0x040029EE RID: 10734
	ExplosionBlockDamage,
	// Token: 0x040029EF RID: 10735
	ExplosionEntityDamage,
	// Token: 0x040029F0 RID: 10736
	ExplosionRadius,
	// Token: 0x040029F1 RID: 10737
	ExplosionIncomingDamage,
	// Token: 0x040029F2 RID: 10738
	ModSlots,
	// Token: 0x040029F3 RID: 10739
	ModPowerBonus,
	// Token: 0x040029F4 RID: 10740
	SpreadMultiplierHip,
	// Token: 0x040029F5 RID: 10741
	SpreadMultiplierAiming,
	// Token: 0x040029F6 RID: 10742
	SpreadMultiplierRunning,
	// Token: 0x040029F7 RID: 10743
	SpreadMultiplierWalking,
	// Token: 0x040029F8 RID: 10744
	SpreadMultiplierCrouching,
	// Token: 0x040029F9 RID: 10745
	SpreadMultiplierIdle,
	// Token: 0x040029FA RID: 10746
	SpreadDegreesVertical,
	// Token: 0x040029FB RID: 10747
	SpreadDegreesHorizontal,
	// Token: 0x040029FC RID: 10748
	KickDegreesVerticalMin,
	// Token: 0x040029FD RID: 10749
	KickDegreesHorizontalMin,
	// Token: 0x040029FE RID: 10750
	KickDegreesVerticalMax,
	// Token: 0x040029FF RID: 10751
	KickDegreesHorizontalMax,
	// Token: 0x04002A00 RID: 10752
	SphereCastRadius,
	// Token: 0x04002A01 RID: 10753
	GrazeDamageMultiplier,
	// Token: 0x04002A02 RID: 10754
	GrazeStaminaMultiplier,
	// Token: 0x04002A03 RID: 10755
	GeneralDamageResist,
	// Token: 0x04002A04 RID: 10756
	PhysicalDamageResist,
	// Token: 0x04002A05 RID: 10757
	PhysicalDamageResistMax,
	// Token: 0x04002A06 RID: 10758
	ElementalDamageResist,
	// Token: 0x04002A07 RID: 10759
	ElementalDamageResistMax,
	// Token: 0x04002A08 RID: 10760
	HypothermalResist,
	// Token: 0x04002A09 RID: 10761
	HyperthermalResist,
	// Token: 0x04002A0A RID: 10762
	FallDamageReduction,
	// Token: 0x04002A0B RID: 10763
	Tier,
	// Token: 0x04002A0C RID: 10764
	LightIntensity,
	// Token: 0x04002A0D RID: 10765
	VehicleFuelMaxPer,
	// Token: 0x04002A0E RID: 10766
	VehicleFuelUsePer,
	// Token: 0x04002A0F RID: 10767
	VehicleVelocityMaxPer,
	// Token: 0x04002A10 RID: 10768
	VehicleMotorTorquePer,
	// Token: 0x04002A11 RID: 10769
	VehicleCarryCapacity,
	// Token: 0x04002A12 RID: 10770
	VehicleEntityDamage,
	// Token: 0x04002A13 RID: 10771
	VehicleBlockDamage,
	// Token: 0x04002A14 RID: 10772
	VehicleSelfDamage,
	// Token: 0x04002A15 RID: 10773
	VehicleStrongSelfDamage,
	// Token: 0x04002A16 RID: 10774
	VehicleSeats,
	// Token: 0x04002A17 RID: 10775
	VehicleTankSize,
	// Token: 0x04002A18 RID: 10776
	VehicleHopStrength,
	// Token: 0x04002A19 RID: 10777
	VehiclePlayerStaminaDrainRate,
	// Token: 0x04002A1A RID: 10778
	BatteryMaxLoadInVolts,
	// Token: 0x04002A1B RID: 10779
	BatteryDischargeTimeInMinutes,
	// Token: 0x04002A1C RID: 10780
	DistractionResistance,
	// Token: 0x04002A1D RID: 10781
	DistractionRadius,
	// Token: 0x04002A1E RID: 10782
	DistractionLifetime,
	// Token: 0x04002A1F RID: 10783
	DistractionStrength,
	// Token: 0x04002A20 RID: 10784
	DistractionEatTicks,
	// Token: 0x04002A21 RID: 10785
	ProjectileGravity,
	// Token: 0x04002A22 RID: 10786
	ProjectileVelocity,
	// Token: 0x04002A23 RID: 10787
	ProjectileStickChance,
	// Token: 0x04002A24 RID: 10788
	RecipeTagUnlocked,
	// Token: 0x04002A25 RID: 10789
	JunkTurretActiveRange,
	// Token: 0x04002A26 RID: 10790
	JunkTurretActiveCount,
	// Token: 0x04002A27 RID: 10791
	EconomicValue,
	// Token: 0x04002A28 RID: 10792
	LockPickTime,
	// Token: 0x04002A29 RID: 10793
	LockPickBreakChance,
	// Token: 0x04002A2A RID: 10794
	LootProb,
	// Token: 0x04002A2B RID: 10795
	LootDropProb,
	// Token: 0x04002A2C RID: 10796
	LootQuantity,
	// Token: 0x04002A2D RID: 10797
	Tracking,
	// Token: 0x04002A2E RID: 10798
	AttributeLevel,
	// Token: 0x04002A2F RID: 10799
	SkillLevel,
	// Token: 0x04002A30 RID: 10800
	PerkLevel,
	// Token: 0x04002A31 RID: 10801
	SkillExpGain,
	// Token: 0x04002A32 RID: 10802
	PlayerExpGain,
	// Token: 0x04002A33 RID: 10803
	NoiseMultiplier,
	// Token: 0x04002A34 RID: 10804
	LightMultiplier,
	// Token: 0x04002A35 RID: 10805
	CraftingTime,
	// Token: 0x04002A36 RID: 10806
	CraftingTier,
	// Token: 0x04002A37 RID: 10807
	CraftingOutputCount,
	// Token: 0x04002A38 RID: 10808
	ActiveCraftingSlots,
	// Token: 0x04002A39 RID: 10809
	CraftingSlots,
	// Token: 0x04002A3A RID: 10810
	CraftingSmeltTime,
	// Token: 0x04002A3B RID: 10811
	ExpDeficitMaxPercentage,
	// Token: 0x04002A3C RID: 10812
	ExpDeficitPerDeathPercentage,
	// Token: 0x04002A3D RID: 10813
	CarryCapacity,
	// Token: 0x04002A3E RID: 10814
	BagSize,
	// Token: 0x04002A3F RID: 10815
	RepairTime,
	// Token: 0x04002A40 RID: 10816
	RepairAmount,
	// Token: 0x04002A41 RID: 10817
	ReloadSpeedMultiplier,
	// Token: 0x04002A42 RID: 10818
	WaterRegenRate,
	// Token: 0x04002A43 RID: 10819
	HealthMax,
	// Token: 0x04002A44 RID: 10820
	HealthChangeOT,
	// Token: 0x04002A45 RID: 10821
	HealthGain,
	// Token: 0x04002A46 RID: 10822
	HealthLoss,
	// Token: 0x04002A47 RID: 10823
	HealthMaxBlockage,
	// Token: 0x04002A48 RID: 10824
	StaminaMax,
	// Token: 0x04002A49 RID: 10825
	StaminaChangeOT,
	// Token: 0x04002A4A RID: 10826
	StaminaGain,
	// Token: 0x04002A4B RID: 10827
	StaminaLoss,
	// Token: 0x04002A4C RID: 10828
	StaminaMaxBlockage,
	// Token: 0x04002A4D RID: 10829
	FoodMax,
	// Token: 0x04002A4E RID: 10830
	FoodChangeOT,
	// Token: 0x04002A4F RID: 10831
	FoodGain,
	// Token: 0x04002A50 RID: 10832
	FoodLoss,
	// Token: 0x04002A51 RID: 10833
	FoodLossPerHealthPointLost,
	// Token: 0x04002A52 RID: 10834
	FoodLossPerStaminaPointGained,
	// Token: 0x04002A53 RID: 10835
	FoodLossPerHealthPointGained,
	// Token: 0x04002A54 RID: 10836
	FoodMaxBlockage,
	// Token: 0x04002A55 RID: 10837
	WaterMax,
	// Token: 0x04002A56 RID: 10838
	WaterChangeOT,
	// Token: 0x04002A57 RID: 10839
	WaterGain,
	// Token: 0x04002A58 RID: 10840
	WaterLoss,
	// Token: 0x04002A59 RID: 10841
	WaterLossPerHealthPointGained,
	// Token: 0x04002A5A RID: 10842
	WaterLossPerStaminaPointGained,
	// Token: 0x04002A5B RID: 10843
	WaterMaxBlockage,
	// Token: 0x04002A5C RID: 10844
	CoreTempChangeOT,
	// Token: 0x04002A5D RID: 10845
	CoreTempGain,
	// Token: 0x04002A5E RID: 10846
	CoreTempLoss,
	// Token: 0x04002A5F RID: 10847
	JumpStrength,
	// Token: 0x04002A60 RID: 10848
	WalkSpeed,
	// Token: 0x04002A61 RID: 10849
	RunSpeed,
	// Token: 0x04002A62 RID: 10850
	CrouchSpeed,
	// Token: 0x04002A63 RID: 10851
	Mobility,
	// Token: 0x04002A64 RID: 10852
	LandMineImmunity,
	// Token: 0x04002A65 RID: 10853
	ScavengingTime,
	// Token: 0x04002A66 RID: 10854
	ScavengingTier,
	// Token: 0x04002A67 RID: 10855
	ScavengingItemCount,
	// Token: 0x04002A68 RID: 10856
	HarvestCount,
	// Token: 0x04002A69 RID: 10857
	ScrappingTime,
	// Token: 0x04002A6A RID: 10858
	DismemberSelfChance,
	// Token: 0x04002A6B RID: 10859
	DismemberChance,
	// Token: 0x04002A6C RID: 10860
	TreasureRadius,
	// Token: 0x04002A6D RID: 10861
	TreasureBlocksPerReduction,
	// Token: 0x04002A6E RID: 10862
	BreathHoldDuration,
	// Token: 0x04002A6F RID: 10863
	BarteringBuying,
	// Token: 0x04002A70 RID: 10864
	BarteringSelling,
	// Token: 0x04002A71 RID: 10865
	SecretStash,
	// Token: 0x04002A72 RID: 10866
	LootTier,
	// Token: 0x04002A73 RID: 10867
	HeatGain,
	// Token: 0x04002A74 RID: 10868
	MovementFactorMultiplier,
	// Token: 0x04002A75 RID: 10869
	QuestBonusItemReward,
	// Token: 0x04002A76 RID: 10870
	QuestRewardOptionCount,
	// Token: 0x04002A77 RID: 10871
	QuestRewardChoiceCount,
	// Token: 0x04002A78 RID: 10872
	GameStage,
	// Token: 0x04002A79 RID: 10873
	TraderStage,
	// Token: 0x04002A7A RID: 10874
	LootStage,
	// Token: 0x04002A7B RID: 10875
	LootStageMax,
	// Token: 0x04002A7C RID: 10876
	DamageBonus,
	// Token: 0x04002A7D RID: 10877
	InternalDamageModifier,
	// Token: 0x04002A7E RID: 10878
	TargetArmor,
	// Token: 0x04002A7F RID: 10879
	FallingBlockDamage,
	// Token: 0x04002A80 RID: 10880
	SilenceBlockSteps,
	// Token: 0x04002A81 RID: 10881
	TrackDistance,
	// Token: 0x04002A82 RID: 10882
	HealthSteal,
	// Token: 0x04002A83 RID: 10883
	TurretWakeUp,
	// Token: 0x04002A84 RID: 10884
	ElectricalTrapXP,
	// Token: 0x04002A85 RID: 10885
	TrapDoorTriggerDelay,
	// Token: 0x04002A86 RID: 10886
	LandMineTriggerDelay,
	// Token: 0x04002A87 RID: 10887
	TrapIncomingDamage,
	// Token: 0x04002A88 RID: 10888
	BlockPickup,
	// Token: 0x04002A89 RID: 10889
	HeadShotOnly,
	// Token: 0x04002A8A RID: 10890
	NegateDamageSelf,
	// Token: 0x04002A8B RID: 10891
	NegateDamageOther,
	// Token: 0x04002A8C RID: 10892
	DisableItem,
	// Token: 0x04002A8D RID: 10893
	DisableLoot,
	// Token: 0x04002A8E RID: 10894
	NoVehicle,
	// Token: 0x04002A8F RID: 10895
	FlipControls,
	// Token: 0x04002A90 RID: 10896
	CelebrationKill,
	// Token: 0x04002A91 RID: 10897
	ShuffledBackpack,
	// Token: 0x04002A92 RID: 10898
	EnemySearchDuration,
	// Token: 0x04002A93 RID: 10899
	TwitchViewerPointRate,
	// Token: 0x04002A94 RID: 10900
	TwitchSpawnMultiplier,
	// Token: 0x04002A95 RID: 10901
	TwitchAddCooldown,
	// Token: 0x04002A96 RID: 10902
	TwitchAddPimpPot,
	// Token: 0x04002A97 RID: 10903
	InfiniteAmmo,
	// Token: 0x04002A98 RID: 10904
	NoTimeDisplay,
	// Token: 0x04002A99 RID: 10905
	DisableGameEventNotify,
	// Token: 0x04002A9A RID: 10906
	NoTrader,
	// Token: 0x04002A9B RID: 10907
	DisableMovement,
	// Token: 0x04002A9C RID: 10908
	ExperienceGain,
	// Token: 0x04002A9D RID: 10909
	CriticalChance,
	// Token: 0x04002A9E RID: 10910
	BuffProcChance,
	// Token: 0x04002A9F RID: 10911
	BuffBlink,
	// Token: 0x04002AA0 RID: 10912
	BuffResistance,
	// Token: 0x04002AA1 RID: 10913
	CraftingIngredientCount,
	// Token: 0x04002AA2 RID: 10914
	EntityPenetrationCount,
	// Token: 0x04002AA3 RID: 10915
	BlockPenetrationFactor,
	// Token: 0x04002AA4 RID: 10916
	JunkDroneModCooldown,
	// Token: 0x04002AA5 RID: 10917
	HeadshotDamageModifier,
	// Token: 0x04002AA6 RID: 10918
	Count
}
