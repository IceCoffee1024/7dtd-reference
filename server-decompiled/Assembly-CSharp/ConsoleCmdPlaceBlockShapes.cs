using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x02000262 RID: 610
[Preserve]
public class ConsoleCmdPlaceBlockShapes : ConsoleCmdAbstract
{
	// Token: 0x170001D1 RID: 465
	// (get) Token: 0x06001203 RID: 4611 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001204 RID: 4612 RVA: 0x00070BAA File Offset: 0x0006EDAA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Places all shapes of the currently held variant helper block";
	}

	// Token: 0x06001205 RID: 4613 RVA: 0x00070BB1 File Offset: 0x0006EDB1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Places all variants of the helper block you currently hold in your hand. Starts\nat the current selection box and spreads out towards the right relative to the\ncurrent view direction of the player, starting a new row behind those every\n" + string.Format("{0} blocks. Spaces out each block by 1m meter.", 25);
	}

	// Token: 0x06001206 RID: 4614 RVA: 0x00070BCE File Offset: 0x0006EDCE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "placeblockshapes", "pbs" };
	}

	// Token: 0x06001207 RID: 4615 RVA: 0x00070BE8 File Offset: 0x0006EDE8
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (!_senderInfo.IsLocalGame)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Command can only be used on clients");
			return;
		}
		if (!BlockToolSelection.Instance.SelectionActive)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No selection active. Running this command requires an active 1x1x1 selection box.");
			return;
		}
		Vector3i selectionSize = BlockToolSelection.Instance.SelectionSize;
		Vector3i selectionStart = BlockToolSelection.Instance.SelectionStart;
		if (selectionSize != Vector3i.one)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Selection box size is not 1x1x1.");
			return;
		}
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		BlockValue blockValue = primaryPlayer.inventory.holdingItemItemValue.ToBlockValue(false);
		Block block = blockValue.Block;
		ItemInventoryData holdingItemData = primaryPlayer.inventory.holdingItemData;
		if (blockValue.isair || holdingItemData == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Player is not holding a block.");
			return;
		}
		if (block.AlternateBlockCount() == 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Block does not have any shapes");
			return;
		}
		float y = primaryPlayer.rotation.y;
		Vector3i zero = Vector3i.zero;
		Vector3i zero2 = Vector3i.zero;
		switch (GameUtils.GetClosestDirection(y, true))
		{
		case GameUtils.DirEightWay.N:
			zero.x = 2;
			zero2.z = 2;
			goto IL_0169;
		case GameUtils.DirEightWay.E:
			zero.z = -2;
			zero2.x = 2;
			goto IL_0169;
		case GameUtils.DirEightWay.S:
			zero.x = -2;
			zero2.z = -2;
			goto IL_0169;
		case GameUtils.DirEightWay.W:
			zero.z = 2;
			zero2.x = -2;
			goto IL_0169;
		}
		throw new ArgumentOutOfRangeException();
		IL_0169:
		Block[] altBlocks = block.GetAltBlocks();
		for (int i = 0; i < altBlocks.Length; i++)
		{
			Block block2 = altBlocks[i];
			Vector3i vector3i = selectionStart + i % 25 * zero + i / 25 * zero2;
			ConsoleCmdPlaceBlockShapes.PlaceBlock(block2.ToBlockValue(), holdingItemData, vector3i, primaryPlayer);
		}
	}

	// Token: 0x06001208 RID: 4616 RVA: 0x00070DB4 File Offset: 0x0006EFB4
	[PublicizedFrom(EAccessModifier.Private)]
	public static void PlaceBlock(BlockValue _blockValue, ItemInventoryData _holdingData, Vector3i _placementPos, EntityPlayerLocal _player)
	{
		BlockPlacement.Result result = new BlockPlacement.Result
		{
			blockValue = _blockValue,
			blockPos = _placementPos
		};
		Block block = _blockValue.Block;
		block.OnBlockPlaceBefore(GameManager.Instance.World, ref result, _player, GameManager.Instance.World.GetGameRandom());
		_blockValue = result.blockValue;
		if (_holdingData.itemValue.TextureFullArray.IsDefault || Block.list[_holdingData.itemValue.type].SelectAlternates)
		{
			block.PlaceBlock(GameManager.Instance.World, result, _player);
			return;
		}
		BlockChangeInfo blockChangeInfo = new BlockChangeInfo(_placementPos, _blockValue)
		{
			textureFull = _holdingData.itemValue.TextureFullArray,
			bChangeTexture = true
		};
		GameManager.Instance.World.SetBlocksRPC(new List<BlockChangeInfo> { blockChangeInfo });
	}

	// Token: 0x04000D02 RID: 3330
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BlocksPerRow = 25;
}
