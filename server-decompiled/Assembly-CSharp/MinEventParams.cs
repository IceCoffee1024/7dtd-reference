using System;
using Challenges;
using UnityEngine;

// Token: 0x0200070C RID: 1804
public class MinEventParams
{
	// Token: 0x0600372A RID: 14122 RVA: 0x0016AC94 File Offset: 0x00168E94
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

	// Token: 0x04002C45 RID: 11333
	public static MinEventParams CachedEventParam = new MinEventParams();

	// Token: 0x04002C46 RID: 11334
	public MinEffectController.SourceParentType ParentType;

	// Token: 0x04002C47 RID: 11335
	public ITileEntity TileEntity;

	// Token: 0x04002C48 RID: 11336
	public EntityAlive Self;

	// Token: 0x04002C49 RID: 11337
	public EntityAlive Instigator;

	// Token: 0x04002C4A RID: 11338
	public EntityAlive Other;

	// Token: 0x04002C4B RID: 11339
	public EntityAlive[] Others;

	// Token: 0x04002C4C RID: 11340
	public ItemValue ItemValue;

	// Token: 0x04002C4D RID: 11341
	public ItemActionData ItemActionData;

	// Token: 0x04002C4E RID: 11342
	public ItemInventoryData ItemInventoryData;

	// Token: 0x04002C4F RID: 11343
	public Vector3 StartPosition;

	// Token: 0x04002C50 RID: 11344
	public Vector3 Position;

	// Token: 0x04002C51 RID: 11345
	public Transform Transform;

	// Token: 0x04002C52 RID: 11346
	public BuffValue Buff;

	// Token: 0x04002C53 RID: 11347
	public BlockValue BlockValue;

	// Token: 0x04002C54 RID: 11348
	public PropRef PropRef;

	// Token: 0x04002C55 RID: 11349
	public PropValue PropValue;

	// Token: 0x04002C56 RID: 11350
	public PrefabInstance POI;

	// Token: 0x04002C57 RID: 11351
	public Bounds Area;

	// Token: 0x04002C58 RID: 11352
	public BiomeDefinition Biome;

	// Token: 0x04002C59 RID: 11353
	public FastTags<TagGroup.Global> Tags;

	// Token: 0x04002C5A RID: 11354
	public DamageResponse DamageResponse;

	// Token: 0x04002C5B RID: 11355
	public ProgressionValue ProgressionValue;

	// Token: 0x04002C5C RID: 11356
	public Challenge Challenge;

	// Token: 0x04002C5D RID: 11357
	public int Seed;

	// Token: 0x04002C5E RID: 11358
	public bool IsLocal;
}
