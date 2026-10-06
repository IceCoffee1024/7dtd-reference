using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200014E RID: 334
[Preserve]
public class BlockSleeper : Block
{
	// Token: 0x06000936 RID: 2358 RVA: 0x0004027C File Offset: 0x0003E47C
	public BlockSleeper()
	{
		this.IsSleeperBlock = true;
		this.HasTileEntity = true;
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x000402C4 File Offset: 0x0003E4C4
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

	// Token: 0x06000938 RID: 2360 RVA: 0x000403B4 File Offset: 0x0003E5B4
	public override bool CanPlaceBlockAt(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, bool _bOmitCollideCheck = false)
	{
		return _world.IsEditor() || base.CanPlaceBlockAt(_world, _blockPos, _blockValue, _bOmitCollideCheck);
	}

	// Token: 0x06000939 RID: 2361 RVA: 0x000403CC File Offset: 0x0003E5CC
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

	// Token: 0x0600093A RID: 2362 RVA: 0x0004043F File Offset: 0x0003E63F
	public bool ExcludesWalkType(int _walkType)
	{
		return this.excludedWalkTypes != null && this.excludedWalkTypes.Contains(_walkType);
	}

	// Token: 0x0600093B RID: 2363 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool IsTileEntitySavedInPrefab()
	{
		return false;
	}

	// Token: 0x040009A9 RID: 2473
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string PropPose = "Pose";

	// Token: 0x040009AA RID: 2474
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string PropLookIdentity = "LookIdentity";

	// Token: 0x040009AB RID: 2475
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string PropExcludeWalkType = "ExcludeWalkType";

	// Token: 0x040009AC RID: 2476
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string PropSpawnGroup = "SpawnGroup";

	// Token: 0x040009AD RID: 2477
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly string PropSpawnMode = "SpawnMode";

	// Token: 0x040009AE RID: 2478
	public int pose;

	// Token: 0x040009AF RID: 2479
	public Vector3 look;

	// Token: 0x040009B0 RID: 2480
	public string spawnGroup;

	// Token: 0x040009B1 RID: 2481
	public BlockSleeper.eMode spawnMode;

	// Token: 0x040009B2 RID: 2482
	[PublicizedFrom(EAccessModifier.Private)]
	public List<int> excludedWalkTypes;

	// Token: 0x040009B3 RID: 2483
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("open", "dummy", true, false, null)
	};

	// Token: 0x0200014F RID: 335
	public enum eMode
	{
		// Token: 0x040009B5 RID: 2485
		Normal,
		// Token: 0x040009B6 RID: 2486
		Bandit,
		// Token: 0x040009B7 RID: 2487
		Infested
	}
}
