using System;
using Challenges;
using UnityEngine;

// Token: 0x02000700 RID: 1792
public class MinEventParams
{
	// Token: 0x06003691 RID: 13969 RVA: 0x00168158 File Offset: 0x00166358
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

	// Token: 0x04002BD8 RID: 11224
	public static MinEventParams CachedEventParam = new MinEventParams();

	// Token: 0x04002BD9 RID: 11225
	public MinEffectController.SourceParentType ParentType;

	// Token: 0x04002BDA RID: 11226
	public ITileEntity TileEntity;

	// Token: 0x04002BDB RID: 11227
	public EntityAlive Self;

	// Token: 0x04002BDC RID: 11228
	public EntityAlive Instigator;

	// Token: 0x04002BDD RID: 11229
	public EntityAlive Other;

	// Token: 0x04002BDE RID: 11230
	public EntityAlive[] Others;

	// Token: 0x04002BDF RID: 11231
	public ItemValue ItemValue;

	// Token: 0x04002BE0 RID: 11232
	public ItemActionData ItemActionData;

	// Token: 0x04002BE1 RID: 11233
	public ItemInventoryData ItemInventoryData;

	// Token: 0x04002BE2 RID: 11234
	public Vector3 StartPosition;

	// Token: 0x04002BE3 RID: 11235
	public Vector3 Position;

	// Token: 0x04002BE4 RID: 11236
	public Transform Transform;

	// Token: 0x04002BE5 RID: 11237
	public BuffValue Buff;

	// Token: 0x04002BE6 RID: 11238
	public BlockValue BlockValue;

	// Token: 0x04002BE7 RID: 11239
	public PropRef PropRef;

	// Token: 0x04002BE8 RID: 11240
	public PropValue PropValue;

	// Token: 0x04002BE9 RID: 11241
	public PrefabInstance POI;

	// Token: 0x04002BEA RID: 11242
	public Bounds Area;

	// Token: 0x04002BEB RID: 11243
	public BiomeDefinition Biome;

	// Token: 0x04002BEC RID: 11244
	public FastTags<TagGroup.Global> Tags;

	// Token: 0x04002BED RID: 11245
	public DamageResponse DamageResponse;

	// Token: 0x04002BEE RID: 11246
	public ProgressionValue ProgressionValue;

	// Token: 0x04002BEF RID: 11247
	public Challenge Challenge;

	// Token: 0x04002BF0 RID: 11248
	public int Seed;

	// Token: 0x04002BF1 RID: 11249
	public bool IsLocal;
}
