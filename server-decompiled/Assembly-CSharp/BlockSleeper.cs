using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200014B RID: 331
[Preserve]
public class BlockSleeper : Block
{
	// Token: 0x06000926 RID: 2342 RVA: 0x0003FF3C File Offset: 0x0003E13C
	public BlockSleeper()
	{
		this.IsSleeperBlock = true;
		this.HasTileEntity = true;
	}

	// Token: 0x06000927 RID: 2343 RVA: 0x0003FF84 File Offset: 0x0003E184
	public override void Init()
	{
		base.Init();
		base.Properties.ParseInt(BlockSleeper.PropPose, ref this.pose);
		this.look = Vector3.forward;
		base.Properties.ParseVec(BlockSleeper.PropLookIdentity, ref this.look);
		string @string = base.Properties.GetString(BlockSleeper.PropExcludeWalkType);
		if (@string.Length > 0)
		{
			string[] array = @string.Split(',', StringSplitOptions.None);
			this.excludedWalkTypes = new List<int>();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == "Crawler")
				{
					this.excludedWalkTypes.Add(21);
				}
				else
				{
					Log.Warning("Block {0}, invalid ExcludeWalkType {1}", new object[]
					{
						base.GetBlockName(),
						array[i]
					});
				}
			}
		}
		base.Properties.ParseString(BlockSleeper.PropSpawnGroup, ref this.spawnGroup);
		base.Properties.ParseEnum<BlockSleeper.eMode>(BlockSleeper.PropSpawnMode, ref this.spawnMode);
	}

	// Token: 0x06000928 RID: 2344 RVA: 0x00040074 File Offset: 0x0003E274
	public override bool CanPlaceBlockAt(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool _bOmitCollideCheck = false)
	{
		return _world.IsEditor() || base.CanPlaceBlockAt(_world, _blockPos, _blockValue, _bOmitCollideCheck);
	}

	// Token: 0x06000929 RID: 2345 RVA: 0x0004008C File Offset: 0x0003E28C
	public float GetSleeperRotation(BlockValue _blockValue)
	{
		byte rotation = _blockValue.rotation;
		switch (rotation)
		{
		case 1:
			return 90f;
		case 2:
			return 180f;
		case 3:
			return 270f;
		default:
			switch (rotation)
			{
			case 24:
				return 45f;
			case 25:
				return 135f;
			case 26:
				return 225f;
			case 27:
				return 315f;
			default:
				return 0f;
			}
			break;
		}
	}

	// Token: 0x0600092A RID: 2346 RVA: 0x000400FF File Offset: 0x0003E2FF
	public bool ExcludesWalkType(int _walkType)
	{
		return this.excludedWalkTypes != null && this.excludedWalkTypes.Contains(_walkType);
	}

	// Token: 0x0600092B RID: 2347 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsTileEntitySavedInPrefab()
	{
		return false;
	}

	// Token: 0x040009A4 RID: 2468
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string PropPose = "Pose";

	// Token: 0x040009A5 RID: 2469
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string PropLookIdentity = "LookIdentity";

	// Token: 0x040009A6 RID: 2470
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string PropExcludeWalkType = "ExcludeWalkType";

	// Token: 0x040009A7 RID: 2471
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string PropSpawnGroup = "SpawnGroup";

	// Token: 0x040009A8 RID: 2472
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string PropSpawnMode = "SpawnMode";

	// Token: 0x040009A9 RID: 2473
	public int pose;

	// Token: 0x040009AA RID: 2474
	public Vector3 look;

	// Token: 0x040009AB RID: 2475
	public string spawnGroup;

	// Token: 0x040009AC RID: 2476
	public BlockSleeper.eMode spawnMode;

	// Token: 0x040009AD RID: 2477
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> excludedWalkTypes;

	// Token: 0x040009AE RID: 2478
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("open", "dummy", true, false, null)
	};

	// Token: 0x0200014C RID: 332
	public enum eMode
	{
		// Token: 0x040009B0 RID: 2480
		Normal,
		// Token: 0x040009B1 RID: 2481
		Bandit,
		// Token: 0x040009B2 RID: 2482
		Infested
	}
}
