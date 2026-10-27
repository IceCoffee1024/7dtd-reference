using System;
using Challenges;
using UnityEngine;

// Token: 0x02000714 RID: 1812
public class MinEventParams
{
	// Token: 0x06003741 RID: 14145 RVA: 0x0016BC2C File Offset: 0x00169E2C
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

	// Token: 0x04002C61 RID: 11361
	public static MinEventParams CachedEventParam = new MinEventParams();

	// Token: 0x04002C62 RID: 11362
	public MinEffectController.SourceParentType ParentType;

	// Token: 0x04002C63 RID: 11363
	public ITileEntity TileEntity;

	// Token: 0x04002C64 RID: 11364
	public EntityAlive Self;

	// Token: 0x04002C65 RID: 11365
	public EntityAlive Instigator;

	// Token: 0x04002C66 RID: 11366
	public EntityAlive Other;

	// Token: 0x04002C67 RID: 11367
	public EntityAlive[] Others;

	// Token: 0x04002C68 RID: 11368
	public ItemValue ItemValue;

	// Token: 0x04002C69 RID: 11369
	public ItemActionData ItemActionData;

	// Token: 0x04002C6A RID: 11370
	public ItemInventoryData ItemInventoryData;

	// Token: 0x04002C6B RID: 11371
	public Vector3 StartPosition;

	// Token: 0x04002C6C RID: 11372
	public Vector3 Position;

	// Token: 0x04002C6D RID: 11373
	public Transform Transform;

	// Token: 0x04002C6E RID: 11374
	public BuffValue Buff;

	// Token: 0x04002C6F RID: 11375
	public BlockValue BlockValue;

	// Token: 0x04002C70 RID: 11376
	public PropRef PropRef;

	// Token: 0x04002C71 RID: 11377
	public PropValue PropValue;

	// Token: 0x04002C72 RID: 11378
	public PrefabInstance POI;

	// Token: 0x04002C73 RID: 11379
	public Bounds Area;

	// Token: 0x04002C74 RID: 11380
	public BiomeDefinition Biome;

	// Token: 0x04002C75 RID: 11381
	public FastTags<TagGroup.Global> Tags;

	// Token: 0x04002C76 RID: 11382
	public DamageResponse DamageResponse;

	// Token: 0x04002C77 RID: 11383
	public ProgressionValue ProgressionValue;

	// Token: 0x04002C78 RID: 11384
	public Challenge Challenge;

	// Token: 0x04002C79 RID: 11385
	public int Seed;

	// Token: 0x04002C7A RID: 11386
	public bool IsLocal;
}
