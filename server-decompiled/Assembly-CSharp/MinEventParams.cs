using System;
using Challenges;
using UnityEngine;

// Token: 0x0200070A RID: 1802
public class MinEventParams
{
	// Token: 0x060036E7 RID: 14055 RVA: 0x0016A330 File Offset: 0x00168530
	public static void CopyTo(MinEventParams _source, MinEventParams _destination)
	{
		_destination.TileEntity = _source.TileEntity;
		_destination.Self = _source.Self;
		_destination.Instigator = _source.Instigator;
		_destination.Other = _source.Other;
		_destination.Others = _source.Others;
		_destination.ItemValue = _source.ItemValue;
		_destination.ItemActionData = _source.ItemActionData;
		_destination.ItemInventoryData = _source.ItemInventoryData;
		_destination.StartPosition = _source.StartPosition;
		_destination.Position = _source.Position;
		_destination.Transform = _source.Transform;
		_destination.Buff = _source.Buff;
		_destination.BlockValue = _source.BlockValue;
		_destination.PropRef = _source.PropRef;
		_destination.PropValue = _source.PropValue;
		_destination.POI = _source.POI;
		_destination.Area = _source.Area;
		_destination.Biome = _source.Biome;
		_destination.Tags = _source.Tags;
		_destination.DamageResponse = _source.DamageResponse;
		_destination.ProgressionValue = _source.ProgressionValue;
		_destination.Seed = _source.Seed;
	}

	// Token: 0x04002C4C RID: 11340
	public static MinEventParams CachedEventParam = new MinEventParams();

	// Token: 0x04002C4D RID: 11341
	public MinEffectController.SourceParentType ParentType;

	// Token: 0x04002C4E RID: 11342
	public ITileEntity TileEntity;

	// Token: 0x04002C4F RID: 11343
	public EntityAlive Self;

	// Token: 0x04002C50 RID: 11344
	public EntityAlive Instigator;

	// Token: 0x04002C51 RID: 11345
	public EntityAlive Other;

	// Token: 0x04002C52 RID: 11346
	public EntityAlive[] Others;

	// Token: 0x04002C53 RID: 11347
	public ItemValue ItemValue;

	// Token: 0x04002C54 RID: 11348
	public ItemActionData ItemActionData;

	// Token: 0x04002C55 RID: 11349
	public ItemInventoryData ItemInventoryData;

	// Token: 0x04002C56 RID: 11350
	public Vector3 StartPosition;

	// Token: 0x04002C57 RID: 11351
	public Vector3 Position;

	// Token: 0x04002C58 RID: 11352
	public Transform Transform;

	// Token: 0x04002C59 RID: 11353
	public BuffValue Buff;

	// Token: 0x04002C5A RID: 11354
	public BlockValue BlockValue;

	// Token: 0x04002C5B RID: 11355
	public PropRef PropRef;

	// Token: 0x04002C5C RID: 11356
	public PropValue PropValue;

	// Token: 0x04002C5D RID: 11357
	public PrefabInstance POI;

	// Token: 0x04002C5E RID: 11358
	public Bounds Area;

	// Token: 0x04002C5F RID: 11359
	public BiomeDefinition Biome;

	// Token: 0x04002C60 RID: 11360
	public FastTags<TagGroup.Global> Tags;

	// Token: 0x04002C61 RID: 11361
	public DamageResponse DamageResponse;

	// Token: 0x04002C62 RID: 11362
	public ProgressionValue ProgressionValue;

	// Token: 0x04002C63 RID: 11363
	public Challenge Challenge;

	// Token: 0x04002C64 RID: 11364
	public int Seed;

	// Token: 0x04002C65 RID: 11365
	public bool IsLocal;
}
