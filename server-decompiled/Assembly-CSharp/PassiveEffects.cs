using System;

// Token: 0x0200065F RID: 1631
public enum PassiveEffects : byte
{
	// Token: 0x040029EF RID: 10735
	None,
	// Token: 0x040029F0 RID: 10736
	EntityDamage,
	// Token: 0x040029F1 RID: 10737
	EntityHeal,
	// Token: 0x040029F2 RID: 10738
	BlockDamage,
	// Token: 0x040029F3 RID: 10739
	BlockRepairAmount,
	// Token: 0x040029F4 RID: 10740
	DamageModifier,
	// Token: 0x040029F5 RID: 10741
	LandClaimDamageModifier,
	// Token: 0x040029F6 RID: 10742
	DegradationPerUse,
	// Token: 0x040029F7 RID: 10743
	DegradationMax,
	// Token: 0x040029F8 RID: 10744
	MagazineSize,
	// Token: 0x040029F9 RID: 10745
	DamageFalloffRange,
	// Token: 0x040029FA RID: 10746
	MaxRange,
	// Token: 0x040029FB RID: 10747
	BlockRange,
	// Token: 0x040029FC RID: 10748
	WeaponHandling,
	// Token: 0x040029FD RID: 10749
	IncrementalSpreadMultiplier,
	// Token: 0x040029FE RID: 10750
	BurstRoundCount,
	// Token: 0x040029FF RID: 10751
	RoundRayCount,
	// Token: 0x04002A00 RID: 10752
	RoundsPerMinute,
	// Token: 0x04002A01 RID: 10753
	AttacksPerMinute,
	// Token: 0x04002A02 RID: 10754
	ExplosionBlockDamage,
	// Token: 0x04002A03 RID: 10755
	ExplosionEntityDamage,
	// Token: 0x04002A04 RID: 10756
	ExplosionRadius,
	// Token: 0x04002A05 RID: 10757
	ExplosionIncomingDamage,
	// Token: 0x04002A06 RID: 10758
	ModSlots,
	// Token: 0x04002A07 RID: 10759
	ModPowerBonus,
	// Token: 0x04002A08 RID: 10760
	SpreadMultiplierHip,
	// Token: 0x04002A09 RID: 10761
	SpreadMultiplierAiming,
	// Token: 0x04002A0A RID: 10762
	SpreadMultiplierRunning,
	// Token: 0x04002A0B RID: 10763
	SpreadMultiplierWalking,
	// Token: 0x04002A0C RID: 10764
	SpreadMultiplierCrouching,
	// Token: 0x04002A0D RID: 10765
	SpreadMultiplierIdle,
	// Token: 0x04002A0E RID: 10766
	SpreadDegreesVertical,
	// Token: 0x04002A0F RID: 10767
	SpreadDegreesHorizontal,
	// Token: 0x04002A10 RID: 10768
	KickDegreesVerticalMin,
	// Token: 0x04002A11 RID: 10769
	KickDegreesHorizontalMin,
	// Token: 0x04002A12 RID: 10770
	KickDegreesVerticalMax,
	// Token: 0x04002A13 RID: 10771
	KickDegreesHorizontalMax,
	// Token: 0x04002A14 RID: 10772
	SphereCastRadius,
	// Token: 0x04002A15 RID: 10773
	GrazeDamageMultiplier,
	// Token: 0x04002A16 RID: 10774
	GrazeStaminaMultiplier,
	// Token: 0x04002A17 RID: 10775
	GeneralDamageResist,
	// Token: 0x04002A18 RID: 10776
	PhysicalDamageResist,
	// Token: 0x04002A19 RID: 10777
	PhysicalDamageResistMax,
	// Token: 0x04002A1A RID: 10778
	ElementalDamageResist,
	// Token: 0x04002A1B RID: 10779
	ElementalDamageResistMax,
	// Token: 0x04002A1C RID: 10780
	HypothermalResist,
	// Token: 0x04002A1D RID: 10781
	HyperthermalResist,
	// Token: 0x04002A1E RID: 10782
	FallDamageReduction,
	// Token: 0x04002A1F RID: 10783
	Tier,
	// Token: 0x04002A20 RID: 10784
	LightIntensity,
	// Token: 0x04002A21 RID: 10785
	VehicleFuelMaxPer,
	// Token: 0x04002A22 RID: 10786
	VehicleFuelUsePer,
	// Token: 0x04002A23 RID: 10787
	VehicleVelocityMaxPer,
	// Token: 0x04002A24 RID: 10788
	VehicleMotorTorquePer,
	// Token: 0x04002A25 RID: 10789
	VehicleCarryCapacity,
	// Token: 0x04002A26 RID: 10790
	VehicleEntityDamage,
	// Token: 0x04002A27 RID: 10791
	VehicleBlockDamage,
	// Token: 0x04002A28 RID: 10792
	VehicleSelfDamage,
	// Token: 0x04002A29 RID: 10793
	VehicleStrongSelfDamage,
	// Token: 0x04002A2A RID: 10794
	VehicleSeats,
	// Token: 0x04002A2B RID: 10795
	VehicleTankSize,
	// Token: 0x04002A2C RID: 10796
	VehicleHopStrength,
	// Token: 0x04002A2D RID: 10797
	VehiclePlayerStaminaDrainRate,
	// Token: 0x04002A2E RID: 10798
	BatteryMaxLoadInVolts,
	// Token: 0x04002A2F RID: 10799
	BatteryDischargeTimeInMinutes,
	// Token: 0x04002A30 RID: 10800
	DistractionResistance,
	// Token: 0x04002A31 RID: 10801
	DistractionRadius,
	// Token: 0x04002A32 RID: 10802
	DistractionLifetime,
	// Token: 0x04002A33 RID: 10803
	DistractionStrength,
	// Token: 0x04002A34 RID: 10804
	DistractionEatTicks,
	// Token: 0x04002A35 RID: 10805
	ProjectileGravity,
	// Token: 0x04002A36 RID: 10806
	ProjectileVelocity,
	// Token: 0x04002A37 RID: 10807
	ProjectileStickChance,
	// Token: 0x04002A38 RID: 10808
	RecipeTagUnlocked,
	// Token: 0x04002A39 RID: 10809
	JunkTurretActiveRange,
	// Token: 0x04002A3A RID: 10810
	JunkTurretActiveCount,
	// Token: 0x04002A3B RID: 10811
	EconomicValue,
	// Token: 0x04002A3C RID: 10812
	LockPickTime,
	// Token: 0x04002A3D RID: 10813
	LockPickBreakChance,
	// Token: 0x04002A3E RID: 10814
	LootProb,
	// Token: 0x04002A3F RID: 10815
	LootDropProb,
	// Token: 0x04002A40 RID: 10816
	LootQuantity,
	// Token: 0x04002A41 RID: 10817
	Tracking,
	// Token: 0x04002A42 RID: 10818
	AttributeLevel,
	// Token: 0x04002A43 RID: 10819
	SkillLevel,
	// Token: 0x04002A44 RID: 10820
	PerkLevel,
	// Token: 0x04002A45 RID: 10821
	SkillExpGain,
	// Token: 0x04002A46 RID: 10822
	PlayerExpGain,
	// Token: 0x04002A47 RID: 10823
	NoiseMultiplier,
	// Token: 0x04002A48 RID: 10824
	LightMultiplier,
	// Token: 0x04002A49 RID: 10825
	CraftingTime,
	// Token: 0x04002A4A RID: 10826
	CraftingTier,
	// Token: 0x04002A4B RID: 10827
	CraftingOutputCount,
	// Token: 0x04002A4C RID: 10828
	ActiveCraftingSlots,
	// Token: 0x04002A4D RID: 10829
	CraftingSlots,
	// Token: 0x04002A4E RID: 10830
	CraftingSmeltTime,
	// Token: 0x04002A4F RID: 10831
	ExpDeficitMaxPercentage,
	// Token: 0x04002A50 RID: 10832
	ExpDeficitPerDeathPercentage,
	// Token: 0x04002A51 RID: 10833
	CarryCapacity,
	// Token: 0x04002A52 RID: 10834
	BagSize,
	// Token: 0x04002A53 RID: 10835
	RepairTime,
	// Token: 0x04002A54 RID: 10836
	RepairAmount,
	// Token: 0x04002A55 RID: 10837
	ReloadSpeedMultiplier,
	// Token: 0x04002A56 RID: 10838
	WaterRegenRate,
	// Token: 0x04002A57 RID: 10839
	HealthMax,
	// Token: 0x04002A58 RID: 10840
	HealthChangeOT,
	// Token: 0x04002A59 RID: 10841
	HealthGain,
	// Token: 0x04002A5A RID: 10842
	HealthLoss,
	// Token: 0x04002A5B RID: 10843
	HealthMaxBlockage,
	// Token: 0x04002A5C RID: 10844
	StaminaMax,
	// Token: 0x04002A5D RID: 10845
	StaminaChangeOT,
	// Token: 0x04002A5E RID: 10846
	StaminaGain,
	// Token: 0x04002A5F RID: 10847
	StaminaLoss,
	// Token: 0x04002A60 RID: 10848
	StaminaMaxBlockage,
	// Token: 0x04002A61 RID: 10849
	FoodMax,
	// Token: 0x04002A62 RID: 10850
	FoodChangeOT,
	// Token: 0x04002A63 RID: 10851
	FoodGain,
	// Token: 0x04002A64 RID: 10852
	FoodLoss,
	// Token: 0x04002A65 RID: 10853
	FoodLossPerHealthPointLost,
	// Token: 0x04002A66 RID: 10854
	FoodLossPerStaminaPointGained,
	// Token: 0x04002A67 RID: 10855
	FoodLossPerHealthPointGained,
	// Token: 0x04002A68 RID: 10856
	FoodMaxBlockage,
	// Token: 0x04002A69 RID: 10857
	WaterMax,
	// Token: 0x04002A6A RID: 10858
	WaterChangeOT,
	// Token: 0x04002A6B RID: 10859
	WaterGain,
	// Token: 0x04002A6C RID: 10860
	WaterLoss,
	// Token: 0x04002A6D RID: 10861
	WaterLossPerHealthPointGained,
	// Token: 0x04002A6E RID: 10862
	WaterLossPerStaminaPointGained,
	// Token: 0x04002A6F RID: 10863
	WaterMaxBlockage,
	// Token: 0x04002A70 RID: 10864
	CoreTempChangeOT,
	// Token: 0x04002A71 RID: 10865
	CoreTempGain,
	// Token: 0x04002A72 RID: 10866
	CoreTempLoss,
	// Token: 0x04002A73 RID: 10867
	JumpStrength,
	// Token: 0x04002A74 RID: 10868
	WalkSpeed,
	// Token: 0x04002A75 RID: 10869
	RunSpeed,
	// Token: 0x04002A76 RID: 10870
	CrouchSpeed,
	// Token: 0x04002A77 RID: 10871
	Mobility,
	// Token: 0x04002A78 RID: 10872
	LandMineImmunity,
	// Token: 0x04002A79 RID: 10873
	ScavengingTime,
	// Token: 0x04002A7A RID: 10874
	ScavengingTier,
	// Token: 0x04002A7B RID: 10875
	ScavengingItemCount,
	// Token: 0x04002A7C RID: 10876
	HarvestCount,
	// Token: 0x04002A7D RID: 10877
	ScrappingTime,
	// Token: 0x04002A7E RID: 10878
	DismemberSelfChance,
	// Token: 0x04002A7F RID: 10879
	DismemberChance,
	// Token: 0x04002A80 RID: 10880
	TreasureRadius,
	// Token: 0x04002A81 RID: 10881
	TreasureBlocksPerReduction,
	// Token: 0x04002A82 RID: 10882
	BreathHoldDuration,
	// Token: 0x04002A83 RID: 10883
	BarteringBuying,
	// Token: 0x04002A84 RID: 10884
	BarteringSelling,
	// Token: 0x04002A85 RID: 10885
	SecretStash,
	// Token: 0x04002A86 RID: 10886
	LootTier,
	// Token: 0x04002A87 RID: 10887
	HeatGain,
	// Token: 0x04002A88 RID: 10888
	MovementFactorMultiplier,
	// Token: 0x04002A89 RID: 10889
	QuestBonusItemReward,
	// Token: 0x04002A8A RID: 10890
	QuestRewardOptionCount,
	// Token: 0x04002A8B RID: 10891
	QuestRewardChoiceCount,
	// Token: 0x04002A8C RID: 10892
	GameStage,
	// Token: 0x04002A8D RID: 10893
	TraderStage,
	// Token: 0x04002A8E RID: 10894
	LootStage,
	// Token: 0x04002A8F RID: 10895
	LootStageMax,
	// Token: 0x04002A90 RID: 10896
	DamageBonus,
	// Token: 0x04002A91 RID: 10897
	InternalDamageModifier,
	// Token: 0x04002A92 RID: 10898
	TargetArmor,
	// Token: 0x04002A93 RID: 10899
	FallingBlockDamage,
	// Token: 0x04002A94 RID: 10900
	SilenceBlockSteps,
	// Token: 0x04002A95 RID: 10901
	TrackDistance,
	// Token: 0x04002A96 RID: 10902
	HealthSteal,
	// Token: 0x04002A97 RID: 10903
	TurretWakeUp,
	// Token: 0x04002A98 RID: 10904
	ElectricalTrapXP,
	// Token: 0x04002A99 RID: 10905
	TrapDoorTriggerDelay,
	// Token: 0x04002A9A RID: 10906
	LandMineTriggerDelay,
	// Token: 0x04002A9B RID: 10907
	TrapIncomingDamage,
	// Token: 0x04002A9C RID: 10908
	BlockPickup,
	// Token: 0x04002A9D RID: 10909
	HeadShotOnly,
	// Token: 0x04002A9E RID: 10910
	NegateDamageSelf,
	// Token: 0x04002A9F RID: 10911
	NegateDamageOther,
	// Token: 0x04002AA0 RID: 10912
	DisableItem,
	// Token: 0x04002AA1 RID: 10913
	DisableLoot,
	// Token: 0x04002AA2 RID: 10914
	NoVehicle,
	// Token: 0x04002AA3 RID: 10915
	FlipControls,
	// Token: 0x04002AA4 RID: 10916
	CelebrationKill,
	// Token: 0x04002AA5 RID: 10917
	ShuffledBackpack,
	// Token: 0x04002AA6 RID: 10918
	EnemySearchDuration,
	// Token: 0x04002AA7 RID: 10919
	TwitchViewerPointRate,
	// Token: 0x04002AA8 RID: 10920
	TwitchSpawnMultiplier,
	// Token: 0x04002AA9 RID: 10921
	TwitchAddCooldown,
	// Token: 0x04002AAA RID: 10922
	TwitchAddPimpPot,
	// Token: 0x04002AAB RID: 10923
	InfiniteAmmo,
	// Token: 0x04002AAC RID: 10924
	NoTimeDisplay,
	// Token: 0x04002AAD RID: 10925
	DisableGameEventNotify,
	// Token: 0x04002AAE RID: 10926
	NoTrader,
	// Token: 0x04002AAF RID: 10927
	DisableMovement,
	// Token: 0x04002AB0 RID: 10928
	ExperienceGain,
	// Token: 0x04002AB1 RID: 10929
	CriticalChance,
	// Token: 0x04002AB2 RID: 10930
	BuffProcChance,
	// Token: 0x04002AB3 RID: 10931
	BuffBlink,
	// Token: 0x04002AB4 RID: 10932
	BuffResistance,
	// Token: 0x04002AB5 RID: 10933
	CraftingIngredientCount,
	// Token: 0x04002AB6 RID: 10934
	EntityPenetrationCount,
	// Token: 0x04002AB7 RID: 10935
	BlockPenetrationFactor,
	// Token: 0x04002AB8 RID: 10936
	JunkDroneModCooldown,
	// Token: 0x04002AB9 RID: 10937
	HeadshotDamageModifier,
	// Token: 0x04002ABA RID: 10938
	Count
}
