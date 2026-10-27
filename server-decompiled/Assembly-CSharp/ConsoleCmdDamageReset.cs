using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000204 RID: 516
[Preserve]
public class ConsoleCmdDamageReset : ConsoleCmdAbstract
{
	// Token: 0x06000FB5 RID: 4021 RVA: 0x00065F68 File Offset: 0x00064168
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "damagereset" };
	}

	// Token: 0x06000FB6 RID: 4022 RVA: 0x00065F78 File Offset: 0x00064178
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Reset damage on all blocks in the currently loaded POI";
	}

	// Token: 0x06000FB7 RID: 4023 RVA: 0x00065F7F File Offset: 0x0006417F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "\r\n\t\t\t|Usage:\r\n\t\t\t|    damagereset [include doors]\r\n\t\t\t|By default the command only resets non-door blocks to full health. If the optional argument is \"true\" doors are also repaired.\r\n\t\t\t".Unindent(true);
	}

	// Token: 0x06000FB8 RID: 4024 RVA: 0x00065F8C File Offset: 0x0006418C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (GameManager.Instance.IsEditMode())
		{
			PrefabEditModeManager instance = PrefabEditModeManager.Instance;
			if (instance != null && instance.IsActive())
			{
				bool ignoreDoors = true;
				if (_params.Count > 0)
				{
					ignoreDoors = !ConsoleHelper.ParseParamBool(_params[0], true);
				}
				World world = GameManager.Instance.World;
				List<Chunk> chunkArrayCopySync = world.ChunkCache.GetChunkArrayCopySync();
				int fixedBlocks = 0;
				for (int i = 0; i < chunkArrayCopySync.Count; i++)
				{
					Chunk chunk = chunkArrayCopySync[i];
					Vector3i chunkWorldPos = chunk.GetWorldPos();
					chunk.LoopOverAllBlocks(delegate(int _x, int _y, int _z, BlockValue _bv)
					{
						int damage = _bv.damage;
						if (damage <= 0)
						{
							return;
						}
						Block block = _bv.Block;
						BlockCompositeTileEntity blockCompositeTileEntity = block as BlockCompositeTileEntity;
						bool flag = blockCompositeTileEntity != null && blockCompositeTileEntity.CompositeData.HasFeature<TEFeatureDoor>();
						if (ignoreDoors && flag)
						{
							return;
						}
						Vector3i vector3i = chunkWorldPos + new Vector3i(_x, _y, _z);
						block.DamageBlock(world, vector3i, _bv, -damage, -1, null, false, false);
						SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Repaired block at {0}, had {1} damage points", vector3i, damage));
						int fixedBlocks2 = fixedBlocks;
						fixedBlocks = fixedBlocks2 + 1;
					}, false, false);
				}
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Repaired {0} blocks", fixedBlocks));
				return;
			}
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Command has to be run while in Prefab Editor!");
	}
}
