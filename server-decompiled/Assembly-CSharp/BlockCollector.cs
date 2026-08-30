using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Scripting;

// Token: 0x02000114 RID: 276
[Preserve]
public class BlockCollector : Block
{
	// Token: 0x1700007A RID: 122
	// (get) Token: 0x06000738 RID: 1848 RVA: 0x00033F8C File Offset: 0x0003218C
	public string[] Outputs
	{
		get
		{
			return this.outputs;
		}
	}

	// Token: 0x06000739 RID: 1849 RVA: 0x00033F94 File Offset: 0x00032194
	public BlockCollector.FuelType GetFuelType(string name)
	{
		BlockCollector.FuelType fuelType;
		if (!this.fuelTypes.TryGetValue(name, out fuelType))
		{
			fuelType = null;
		}
		return fuelType;
	}

	// Token: 0x0600073A RID: 1850 RVA: 0x00033FB4 File Offset: 0x000321B4
	public BlockCollector.OutputType GetOutputType(string name)
	{
		BlockCollector.OutputType outputType;
		if (!this.outputTypes.TryGetValue(name, out outputType))
		{
			outputType = null;
		}
		return outputType;
	}

	// Token: 0x1700007B RID: 123
	// (get) Token: 0x0600073B RID: 1851 RVA: 0x00033FD4 File Offset: 0x000321D4
	public string[] ItemIconBackdrop
	{
		get
		{
			return this.itemIconBackdrop;
		}
	}

	// Token: 0x1700007C RID: 124
	// (get) Token: 0x0600073C RID: 1852 RVA: 0x00033FDC File Offset: 0x000321DC
	public string ItemBackgroundColor
	{
		get
		{
			return this.itemBackgroundColor;
		}
	}

	// Token: 0x1700007D RID: 125
	// (get) Token: 0x0600073D RID: 1853 RVA: 0x00033FE4 File Offset: 0x000321E4
	public string LootLabelKey
	{
		get
		{
			return this.lootLabelKey;
		}
	}

	// Token: 0x1700007E RID: 126
	// (get) Token: 0x0600073E RID: 1854 RVA: 0x00033FEC File Offset: 0x000321EC
	public string ActivationEvent
	{
		get
		{
			return this.activationEvent;
		}
	}

	// Token: 0x1700007F RID: 127
	// (get) Token: 0x0600073F RID: 1855 RVA: 0x00033FF4 File Offset: 0x000321F4
	public string CloseEvent
	{
		get
		{
			return this.closeEvent;
		}
	}

	// Token: 0x17000080 RID: 128
	// (get) Token: 0x06000740 RID: 1856 RVA: 0x00033FFC File Offset: 0x000321FC
	public string[] RequiredMods
	{
		get
		{
			return this.requiredMods;
		}
	}

	// Token: 0x17000081 RID: 129
	// (get) Token: 0x06000741 RID: 1857 RVA: 0x00034004 File Offset: 0x00032204
	public bool RequiredModsOnly
	{
		get
		{
			return this.requiredModsOnly;
		}
	}

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x06000742 RID: 1858 RVA: 0x0003400C File Offset: 0x0003220C
	public string[] ModTransformEnableNames
	{
		get
		{
			return this.modTransformEnableNames;
		}
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x06000743 RID: 1859 RVA: 0x00034014 File Offset: 0x00032214
	public string[] ModTransformDisableNames
	{
		get
		{
			return this.modTransformDisableNames;
		}
	}

	// Token: 0x17000084 RID: 132
	// (get) Token: 0x06000744 RID: 1860 RVA: 0x0003401C File Offset: 0x0003221C
	public int FuelGridInitialHeight
	{
		get
		{
			return this.fuelGridHeight;
		}
	}

	// Token: 0x17000085 RID: 133
	// (get) Token: 0x06000745 RID: 1861 RVA: 0x00034024 File Offset: 0x00032224
	public int CatalystGridInitialHeight
	{
		get
		{
			return this.catalystGridHeight;
		}
	}

	// Token: 0x17000086 RID: 134
	// (get) Token: 0x06000746 RID: 1862 RVA: 0x0003402C File Offset: 0x0003222C
	public int OutputGridInitialHeight
	{
		get
		{
			return this.outputGridInitialHeight;
		}
	}

	// Token: 0x17000087 RID: 135
	// (get) Token: 0x06000747 RID: 1863 RVA: 0x00034034 File Offset: 0x00032234
	public string FuelTitleText
	{
		get
		{
			return this.fuelTitleText;
		}
	}

	// Token: 0x17000088 RID: 136
	// (get) Token: 0x06000748 RID: 1864 RVA: 0x0003403C File Offset: 0x0003223C
	public string FuelTitleSprite
	{
		get
		{
			return this.fuelTitleSprite;
		}
	}

	// Token: 0x17000089 RID: 137
	// (get) Token: 0x06000749 RID: 1865 RVA: 0x00034044 File Offset: 0x00032244
	public string FuelTitleSpriteAtlas
	{
		get
		{
			return this.fuelTitleSpriteAtlas;
		}
	}

	// Token: 0x1700008A RID: 138
	// (get) Token: 0x0600074A RID: 1866 RVA: 0x0003404C File Offset: 0x0003224C
	public string CatalystTitleText
	{
		get
		{
			return this.catalystTitleText;
		}
	}

	// Token: 0x1700008B RID: 139
	// (get) Token: 0x0600074B RID: 1867 RVA: 0x00034054 File Offset: 0x00032254
	public string CatalystTitleSprite
	{
		get
		{
			return this.catalystTitleSprite;
		}
	}

	// Token: 0x1700008C RID: 140
	// (get) Token: 0x0600074C RID: 1868 RVA: 0x0003405C File Offset: 0x0003225C
	public string CatalystTitleSpriteAtlas
	{
		get
		{
			return this.catalystTitleSpriteAtlas;
		}
	}

	// Token: 0x1700008D RID: 141
	// (get) Token: 0x0600074D RID: 1869 RVA: 0x00034064 File Offset: 0x00032264
	public string[] FuelTypesSprites
	{
		get
		{
			return this.fuelTypesSprites;
		}
	}

	// Token: 0x1700008E RID: 142
	// (get) Token: 0x0600074E RID: 1870 RVA: 0x0003406C File Offset: 0x0003226C
	public string[] FuelTypesSpriteAtlases
	{
		get
		{
			return this.fuelTypesSpriteAtlases;
		}
	}

	// Token: 0x1700008F RID: 143
	// (get) Token: 0x0600074F RID: 1871 RVA: 0x00034074 File Offset: 0x00032274
	public string[] CatalystTypesSprites
	{
		get
		{
			return this.catalystTypesSprites;
		}
	}

	// Token: 0x17000090 RID: 144
	// (get) Token: 0x06000750 RID: 1872 RVA: 0x0003407C File Offset: 0x0003227C
	public string[] CatalystTypesSpriteAtlases
	{
		get
		{
			return this.catalystTypesSpriteAtlases;
		}
	}

	// Token: 0x17000091 RID: 145
	// (get) Token: 0x06000751 RID: 1873 RVA: 0x00034084 File Offset: 0x00032284
	public string[] CatalystTypes
	{
		get
		{
			return this.catalystTypes;
		}
	}

	// Token: 0x06000752 RID: 1874 RVA: 0x0003408C File Offset: 0x0003228C
	public BlockCollector()
	{
		this.HasTileEntity = true;
	}

	// Token: 0x06000753 RID: 1875 RVA: 0x000341DC File Offset: 0x000323DC
	public override void Init()
	{
		base.Init();
		string text = "";
		base.Properties.ParseString(BlockCollector.PropCollectorType, ref text);
		if (text != "")
		{
			Enum.TryParse<BlockCollector.CollectorTypes>(text, out this.CollectorType);
		}
		string text2 = null;
		base.Properties.ParseString(BlockCollector.PropFuelTypes, ref text2);
		string[] array = text2.TrimEnd('}').Split('{', 125, StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < array.Length; i++)
		{
			BlockCollector.FuelType fuelType = new BlockCollector.FuelType(array[i]);
			this.fuelTypes[fuelType.Name] = fuelType;
		}
		string text3 = null;
		base.Properties.ParseString(BlockCollector.PropOutputTypes, ref text3);
		string[] array2 = text3.Split('{', 125, StringSplitOptions.RemoveEmptyEntries);
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j] = array2[j].TrimEnd('}');
			BlockCollector.OutputType outputType = new BlockCollector.OutputType(array2[j]);
			this.outputTypes.Add(outputType.Name, outputType);
		}
		string text4 = null;
		base.Properties.ParseString(BlockCollector.PropOutputs, ref text4);
		this.outputs = text4.Split(',', StringSplitOptions.None);
		for (int k = 0; k < this.outputs.Length; k++)
		{
			BlockCollector.OutputType outputType2 = this.GetOutputType(this.outputs[k]);
			if (outputType2 != null)
			{
				List<int> list;
				if (!this.OrderedSlotOutputs.TryGetValue(outputType2, out list))
				{
					list = new List<int>();
					this.OrderedSlotOutputs[outputType2] = list;
				}
				list.Add(k);
			}
		}
		base.Properties.ParseString(BlockCollector.PropOpenSound, ref this.OpenSound);
		base.Properties.ParseString(BlockCollector.PropRunningSound, ref this.RunningSound);
		base.Properties.ParseString(BlockCollector.PropActivateSound, ref this.ActivateSound);
		base.Properties.ParseString(BlockCollector.PropCloseSound, ref this.CloseSound);
		base.Properties.ParseFloat(BlockCollector.PropTakeDelay, ref this.TakeDelay);
		string text5 = null;
		base.Properties.ParseString(BlockCollector.PropItemIconBackdrop, ref text5);
		this.itemIconBackdrop = text5.Split(',', StringSplitOptions.None);
		base.Properties.ParseString(BlockCollector.PropLootLabelKey, ref this.lootLabelKey);
		base.Properties.ParseString(BlockCollector.PropActivationEvent, ref this.activationEvent);
		base.Properties.ParseString(BlockCollector.PropCloseEvent, ref this.closeEvent);
		base.Properties.ParseString(BlockCollector.PropItemBackgroundColor, ref this.itemBackgroundColor);
		string text6 = "";
		this.requiredMods = null;
		base.Properties.ParseString(BlockCollector.PropRequiredMods, ref text6);
		if (!string.IsNullOrEmpty(text6))
		{
			this.requiredMods = text6.Split(',', StringSplitOptions.None);
		}
		base.Properties.ParseBool(BlockCollector.PropRequiredModsOnly, ref this.requiredModsOnly);
		string text7 = ",,";
		base.Properties.ParseString(BlockCollector.PropModTransformEnableNames, ref text7);
		this.modTransformEnableNames = text7.Split(',', StringSplitOptions.None);
		text7 = ",,";
		base.Properties.ParseString(BlockCollector.PropModTransformDisableNames, ref text7);
		this.modTransformDisableNames = text7.Split(',', StringSplitOptions.None);
		text7 = "Count,Speed,Type";
		base.Properties.ParseString(BlockCollector.PropModTypes, ref text7);
		string[] array3 = text7.Split(',', StringSplitOptions.None);
		this.ModTypes = new BlockCollector.ModEffectTypes[array3.Length];
		int num = 0;
		while (num < array3.Length && num < this.ModTypes.Length)
		{
			this.ModTypes[num] = Enum.Parse<BlockCollector.ModEffectTypes>(array3[num]);
			num++;
		}
		base.Properties.ParseString(BlockCollector.PropEmptyText, ref this.emptyText);
		base.Properties.ParseString(BlockCollector.PropHasItem1Text, ref this.hasItem1Text);
		base.Properties.ParseString(BlockCollector.PropHasItem2Text, ref this.hasItem2Text);
		base.Properties.ParseInt(BlockCollector.PropOutputGridHeight, ref this.outputGridInitialHeight);
		base.Properties.ParseInt(BlockCollector.PropFuelGridHeight, ref this.fuelGridHeight);
		base.Properties.ParseString(BlockCollector.PropFuelTitleText, ref this.fuelTitleText);
		base.Properties.ParseString(BlockCollector.PropFuelTitleSprite, ref this.fuelTitleSprite);
		base.Properties.ParseString(BlockCollector.PropFuelTitleSpriteAtlas, ref this.fuelTitleSpriteAtlas);
		string text8 = null;
		base.Properties.ParseString(BlockCollector.PropFuelTypesSprites, ref text8);
		if (text8 != null)
		{
			string[] array4 = text8.Split(',', StringSplitOptions.None);
			int num2 = array4.Length / 2;
			this.fuelTypesSprites = new string[num2];
			this.fuelTypesSpriteAtlases = new string[num2];
			for (int l = 0; l < num2; l++)
			{
				int num3 = l * 2;
				this.fuelTypesSprites[l] = array4[num3];
				this.FuelTypesSpriteAtlases[l] = array4[num3 + 1];
			}
		}
		base.Properties.ParseInt(BlockCollector.PropCatalystGridHeight, ref this.catalystGridHeight);
		text8 = null;
		base.Properties.ParseString(BlockCollector.PropCatalystTypes, ref text8);
		if (text8 != null)
		{
			this.catalystTypes = text8.Split(',', StringSplitOptions.None);
		}
		base.Properties.ParseString(BlockCollector.PropCatalystTypes, ref text8);
		if (text8 != null)
		{
			string[] array5 = text8.Split(',', StringSplitOptions.None);
			int num4 = array5.Length / 2;
			this.CatalystConverts = new BlockCollector.CatalystConvert[num4];
			for (int m = 0; m < num4; m++)
			{
				this.CatalystConverts[m] = new BlockCollector.CatalystConvert(array5[m * 2], array5[m * 2 + 1]);
			}
		}
		base.Properties.ParseString(BlockCollector.PropCatalystMultiplier, ref text8);
		if (text8 != null)
		{
			array = text8.Split(",", StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array6 = array[i].Split("=", StringSplitOptions.None);
				if (array6.Length == 2)
				{
					int num5;
					if (!int.TryParse(array6[1], out num5))
					{
						num5 = 1;
					}
					this.CatalystMultipliers.Add(array6[0], num5);
				}
			}
		}
		base.Properties.ParseString(BlockCollector.PropCatalystRequirements, ref text8);
		if (text8 != null)
		{
			array = text8.Split(",", StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				string[] array7 = array[i].Split("=", StringSplitOptions.None);
				if (array7.Length == 2)
				{
					int num6;
					if (!int.TryParse(array7[1], out num6))
					{
						num6 = 1;
					}
					this.CatalystRequirements.Add(array7[0], num6);
				}
			}
		}
		base.Properties.ParseString(BlockCollector.PropCatalystTitleText, ref this.catalystTitleText);
		base.Properties.ParseString(BlockCollector.PropCatalystTitleSprite, ref this.catalystTitleSprite);
		base.Properties.ParseString(BlockCollector.PropCatalystTitleSpriteAtlas, ref this.catalystTitleSpriteAtlas);
		base.Properties.ParseString(BlockCollector.PropCatalystTypesSprites, ref text8);
		if (text8 != null)
		{
			string[] array8 = text8.Split(',', StringSplitOptions.None);
			int num7 = array8.Length / 2;
			this.catalystTypesSprites = new string[num7];
			this.catalystTypesSpriteAtlases = new string[num7];
			for (int n = 0; n < num7; n++)
			{
				int num8 = n * 2;
				this.catalystTypesSprites[n] = array8[num8];
				this.catalystTypesSpriteAtlases[n] = array8[num8 + 1];
			}
		}
	}

	// Token: 0x06000754 RID: 1876 RVA: 0x000348BF File Offset: 0x00032ABF
	public override void OnBlockAdded(WorldBase world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue, PlatformUserIdentifierAbs _addedByPlayer)
	{
		base.OnBlockAdded(world, _chunk, _blockPos, _blockValue, _addedByPlayer);
		if (_blockValue.ischild)
		{
			return;
		}
		this.addTileEntity(world, _chunk, _blockPos, _blockValue);
	}

	// Token: 0x06000755 RID: 1877 RVA: 0x000348E3 File Offset: 0x00032AE3
	public override bool OnBlockActivated(string _commandName, WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		if (_commandName == "Search")
		{
			return this.OnBlockActivated(_world, _blockPos, _blockValue, _player);
		}
		if (!(_commandName == "take"))
		{
			return false;
		}
		base.takeItemWithTimer(_blockPos, _blockValue, _player, this.TakeDelay);
		return true;
	}

	// Token: 0x06000756 RID: 1878 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool HasBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		return true;
	}

	// Token: 0x06000757 RID: 1879 RVA: 0x00034924 File Offset: 0x00032B24
	public override BlockActivationCommand[] GetBlockActivationCommands(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		bool flag = _world.IsMyLandProtectedBlock(_blockPos, _world.GetGameManager().GetPersistentLocalPlayer());
		this.cmds[1].enabled = flag && this.TakeDelay > 0f;
		return this.cmds;
	}

	// Token: 0x06000758 RID: 1880 RVA: 0x00034970 File Offset: 0x00032B70
	public override void PlaceBlock(WorldBase _world, BlockPlacement.Result _result, EntityAlive _ea)
	{
		base.PlaceBlock(_world, _result, _ea);
		TileEntityCollector tileEntityCollector = _world.GetTileEntity(_result.blockPos) as TileEntityCollector;
		if (tileEntityCollector == null)
		{
			return;
		}
		if (_ea != null && _ea.entityType == EntityType.Player)
		{
			tileEntityCollector.worldTimeTouched = _world.GetWorldTime();
			tileEntityCollector.SetEmpty();
		}
	}

	// Token: 0x06000759 RID: 1881 RVA: 0x000349C0 File Offset: 0x00032BC0
	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, Vector3i _blockPos, EntityAlive _entityFocusing)
	{
		TileEntityCollector tileEntityCollector = _world.GetTileEntity(_blockPos) as TileEntityCollector;
		if (tileEntityCollector == null)
		{
			return string.Empty;
		}
		string localizedBlockName = _blockValue.Block.GetLocalizedBlockName();
		PlayerActionsLocal playerInput = ((EntityPlayerLocal)_entityFocusing).playerInput;
		string text = playerInput.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null) + playerInput.PermanentActions.Activate.GetBindingXuiMarkupString(XUiUtils.EmptyBindingStyle.EmptyString, XUiUtils.DisplayStyle.Plain, null);
		if (tileEntityCollector.IsWaterEmpty())
		{
			return string.Format(Localization.Get(this.emptyText, false, null), text, localizedBlockName);
		}
		if (tileEntityCollector.HasModConvert)
		{
			return string.Format(Localization.Get(this.hasItem2Text, false, null), text, localizedBlockName);
		}
		return string.Format(Localization.Get(this.hasItem1Text, false, null), text, localizedBlockName);
	}

	// Token: 0x0600075A RID: 1882 RVA: 0x00034A74 File Offset: 0x00032C74
	public override void OnBlockRemoved(WorldBase world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		base.OnBlockRemoved(world, _chunk, _blockPos, _blockValue);
		TileEntityCollector tileEntityCollector = world.GetTileEntity(_blockPos) as TileEntityCollector;
		if (tileEntityCollector != null)
		{
			tileEntityCollector.OnDestroy();
		}
		this.removeTileEntity(world, _chunk, _blockPos, _blockValue);
	}

	// Token: 0x0600075B RID: 1883 RVA: 0x00034AB0 File Offset: 0x00032CB0
	[PublicizedFrom(EAccessModifier.Protected)]
	public void addTileEntity(WorldBase world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		TileEntityCollector tileEntityCollector = new TileEntityCollector(_chunk);
		tileEntityCollector.localChunkPos = World.toBlock(_blockPos);
		tileEntityCollector.SetWorldTime();
		_chunk.AddTileEntity(tileEntityCollector);
	}

	// Token: 0x0600075C RID: 1884 RVA: 0x00034ADD File Offset: 0x00032CDD
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void removeTileEntity(WorldBase world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		_chunk.RemoveTileEntityAt<TileEntityCollector>((World)world, World.toBlock(_blockPos));
	}

	// Token: 0x0600075D RID: 1885 RVA: 0x00034AF4 File Offset: 0x00032CF4
	public override Block.DestroyedResult OnBlockDestroyedBy(WorldBase _world, BlockValueRef _bvRef, BlockValue _blockValue, int _entityId, bool _bUseHarvestTool)
	{
		TileEntityCollector tileEntityCollector = _world.GetTileEntity(_bvRef) as TileEntityCollector;
		if (tileEntityCollector != null)
		{
			tileEntityCollector.OnDestroy();
		}
		if (!GameManager.IsDedicatedServer)
		{
			XUiC_DewCollectorWindowGroup.CloseIfOpenAtPos(_bvRef, null);
		}
		return Block.DestroyedResult.Downgrade;
	}

	// Token: 0x0600075E RID: 1886 RVA: 0x00034B2B File Offset: 0x00032D2B
	public override void OnBlockEntityTransformBeforeActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
		base.OnBlockEntityTransformBeforeActivated(_world, _blockPos, _blockValue, _ebcd);
		this.UpdateVisible(_world, _blockPos);
	}

	// Token: 0x0600075F RID: 1887 RVA: 0x00034B40 File Offset: 0x00032D40
	public void UpdateVisible(WorldBase _world, Vector3i _blockPos)
	{
		TileEntityCollector tileEntityCollector = _world.GetTileEntity(_blockPos) as TileEntityCollector;
		if (tileEntityCollector != null)
		{
			tileEntityCollector.UpdateVisible();
		}
	}

	// Token: 0x06000760 RID: 1888 RVA: 0x00034B64 File Offset: 0x00032D64
	public override bool OnBlockActivated(WorldBase _world, Vector3i _blockPos, BlockValue _blockValue, EntityPlayerLocal _player)
	{
		if (_player.inventory.IsHoldingItemActionRunning())
		{
			return false;
		}
		TileEntityCollector tileEntityCollector = _world.GetTileEntity(_blockPos) as TileEntityCollector;
		if (tileEntityCollector == null)
		{
			return false;
		}
		_player.AimingGun = false;
		LockManager.Instance.LockRequestLocal(tileEntityCollector, null, 0);
		return true;
	}

	// Token: 0x06000761 RID: 1889 RVA: 0x00034BAC File Offset: 0x00032DAC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool takeItemWithTimerCanTake(Vector3i _blockPos, EntityAlive _player)
	{
		if ((GameManager.Instance.World.GetTileEntity(_blockPos) as TileEntityCollector).IsEmpty())
		{
			return true;
		}
		GameManager.ShowTooltip(_player as EntityPlayerLocal, Localization.Get("ttWorkstationNotEmpty", false, null), string.Empty, "ui_denied", null, false, false, 0f);
		return false;
	}

	// Token: 0x06000762 RID: 1890 RVA: 0x00034C04 File Offset: 0x00032E04
	public bool UsesFuel()
	{
		bool flag = false;
		switch (this.CollectorType)
		{
		case BlockCollector.CollectorTypes.DewCollector:
			flag = XUiM_Recipes.DewCollectorInput == 0f;
			break;
		case BlockCollector.CollectorTypes.Apiary:
			flag = XUiM_Recipes.ApiaryInput == 0f;
			break;
		}
		return !flag && this.fuelTypes.Values.Count > 0;
	}

	// Token: 0x06000763 RID: 1891 RVA: 0x00034C62 File Offset: 0x00032E62
	public bool UsesCatalyst()
	{
		return this.CatalystTypes.Length != 0;
	}

	// Token: 0x06000764 RID: 1892 RVA: 0x00034C6E File Offset: 0x00032E6E
	[PublicizedFrom(EAccessModifier.Private)]
	public int modifyTime(int time, float modifier)
	{
		if (modifier == 0f)
		{
			time = 0;
		}
		else
		{
			time = (int)(1f / modifier * (float)time);
		}
		return time;
	}

	// Token: 0x06000765 RID: 1893 RVA: 0x00034C8C File Offset: 0x00032E8C
	public int GetSandboxModifiedTime(int time)
	{
		if (time != 0)
		{
			switch (this.CollectorType)
			{
			case BlockCollector.CollectorTypes.DewCollector:
				time = this.modifyTime(time, XUiM_Recipes.DewCollectorTimeModifier);
				break;
			case BlockCollector.CollectorTypes.Apiary:
				time = this.modifyTime(time, XUiM_Recipes.ApiaryTimeModifier);
				break;
			}
			if (time == 0)
			{
				time = -1;
			}
		}
		return time;
	}

	// Token: 0x06000766 RID: 1894 RVA: 0x00034CDC File Offset: 0x00032EDC
	public int GetSandboxModifiedFuelNeeded(int cost)
	{
		switch (this.CollectorType)
		{
		case BlockCollector.CollectorTypes.DewCollector:
			cost = (int)(XUiM_Recipes.DewCollectorInput * (float)cost);
			break;
		case BlockCollector.CollectorTypes.Apiary:
			cost = (int)(XUiM_Recipes.ApiaryInput * (float)cost);
			break;
		}
		return cost;
	}

	// Token: 0x06000767 RID: 1895 RVA: 0x00034D20 File Offset: 0x00032F20
	public int GetSandboxModifiedOutput(int cost)
	{
		switch (this.CollectorType)
		{
		case BlockCollector.CollectorTypes.DewCollector:
			cost = (int)(XUiM_Recipes.DewCollectorOutput * (float)cost);
			break;
		case BlockCollector.CollectorTypes.Apiary:
			cost = (int)(XUiM_Recipes.ApiaryOutput * (float)cost);
			break;
		}
		return cost;
	}

	// Token: 0x04000878 RID: 2168
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropCollectorType = "CollectorType";

	// Token: 0x04000879 RID: 2169
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropFuelTypes = "FuelTypes";

	// Token: 0x0400087A RID: 2170
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropOutputTypes = "OutputTypes";

	// Token: 0x0400087B RID: 2171
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropOutputs = "Outputs";

	// Token: 0x0400087C RID: 2172
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropOpenSound = "OpenSound";

	// Token: 0x0400087D RID: 2173
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropRunningSound = "RunningSound";

	// Token: 0x0400087E RID: 2174
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropActivateSound = "ActivateSound";

	// Token: 0x0400087F RID: 2175
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropCloseSound = "CloseSound";

	// Token: 0x04000880 RID: 2176
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropTakeDelay = "TakeDelay";

	// Token: 0x04000881 RID: 2177
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropItemIconBackdrop = "ItemIconBackdrop";

	// Token: 0x04000882 RID: 2178
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropLootLabelKey = "LootLabelKey";

	// Token: 0x04000883 RID: 2179
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropActivationEvent = "ActivationEvent";

	// Token: 0x04000884 RID: 2180
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropCloseEvent = "CloseEvent";

	// Token: 0x04000885 RID: 2181
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropRequiredMods = "RequiredMods";

	// Token: 0x04000886 RID: 2182
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropRequiredModsOnly = "RequiredModsOnly";

	// Token: 0x04000887 RID: 2183
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropModTransformEnableNames = "ModTransformEnableNames";

	// Token: 0x04000888 RID: 2184
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropModTransformDisableNames = "ModTransformDisableNames";

	// Token: 0x04000889 RID: 2185
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropModTypes = "ModTypes";

	// Token: 0x0400088A RID: 2186
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropItemBackgroundColor = "ItemBackgroundColor";

	// Token: 0x0400088B RID: 2187
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropEmptyText = "EmptyText";

	// Token: 0x0400088C RID: 2188
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropHasItem1Text = "HasItem1Text";

	// Token: 0x0400088D RID: 2189
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropHasItem2Text = "HasItem2Text";

	// Token: 0x0400088E RID: 2190
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropOutputGridHeight = "OutputGridHeight";

	// Token: 0x0400088F RID: 2191
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropFuelGridHeight = "FuelGridHeight";

	// Token: 0x04000890 RID: 2192
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropFuelTitleText = "FuelTitleText";

	// Token: 0x04000891 RID: 2193
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropFuelTitleSprite = "FuelTitleSprite";

	// Token: 0x04000892 RID: 2194
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropFuelTitleSpriteAtlas = "FuelTitleSpriteAtlas";

	// Token: 0x04000893 RID: 2195
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropFuelTypesSprites = "FuelTypesSprites";

	// Token: 0x04000894 RID: 2196
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropCatalystGridHeight = "CatalystGridHeight";

	// Token: 0x04000895 RID: 2197
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropCatalystTypes = "CatalystTypes";

	// Token: 0x04000896 RID: 2198
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropCatalystMultiplier = "CatalystMultiplier";

	// Token: 0x04000897 RID: 2199
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropCatalystRequirements = "CatalystRequirements";

	// Token: 0x04000898 RID: 2200
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropCatalystConvert = "CatalystConvert";

	// Token: 0x04000899 RID: 2201
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropCatalystTitleText = "CatalystTitleText";

	// Token: 0x0400089A RID: 2202
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropCatalystTitleSprite = "CatalystTitleSprite";

	// Token: 0x0400089B RID: 2203
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropCatalystTitleSpriteAtlas = "CatalystTitleSpriteAtlas";

	// Token: 0x0400089C RID: 2204
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string PropCatalystTypesSprites = "CatalystTypesSprites";

	// Token: 0x0400089D RID: 2205
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] outputs;

	// Token: 0x0400089E RID: 2206
	[PublicizedFrom(EAccessModifier.Protected)]
	public BlockCollector.CollectorTypes CollectorType;

	// Token: 0x0400089F RID: 2207
	public Dictionary<BlockCollector.OutputType, List<int>> OrderedSlotOutputs = new Dictionary<BlockCollector.OutputType, List<int>>();

	// Token: 0x040008A0 RID: 2208
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, BlockCollector.FuelType> fuelTypes = new Dictionary<string, BlockCollector.FuelType>();

	// Token: 0x040008A1 RID: 2209
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, BlockCollector.OutputType> outputTypes = new Dictionary<string, BlockCollector.OutputType>();

	// Token: 0x040008A2 RID: 2210
	public string OpenSound;

	// Token: 0x040008A3 RID: 2211
	public string CloseSound;

	// Token: 0x040008A4 RID: 2212
	public string RunningSound;

	// Token: 0x040008A5 RID: 2213
	public string ActivateSound;

	// Token: 0x040008A6 RID: 2214
	[PublicizedFrom(EAccessModifier.Protected)]
	public float TakeDelay = 2f;

	// Token: 0x040008A7 RID: 2215
	[PublicizedFrom(EAccessModifier.Protected)]
	public string[] itemIconBackdrop;

	// Token: 0x040008A8 RID: 2216
	[PublicizedFrom(EAccessModifier.Private)]
	public string itemBackgroundColor = "255,255,255,255";

	// Token: 0x040008A9 RID: 2217
	[PublicizedFrom(EAccessModifier.Private)]
	public string lootLabelKey;

	// Token: 0x040008AA RID: 2218
	[PublicizedFrom(EAccessModifier.Private)]
	public string activationEvent;

	// Token: 0x040008AB RID: 2219
	[PublicizedFrom(EAccessModifier.Private)]
	public string closeEvent;

	// Token: 0x040008AC RID: 2220
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] requiredMods;

	// Token: 0x040008AD RID: 2221
	[PublicizedFrom(EAccessModifier.Private)]
	public bool requiredModsOnly;

	// Token: 0x040008AE RID: 2222
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] modTransformEnableNames;

	// Token: 0x040008AF RID: 2223
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] modTransformDisableNames;

	// Token: 0x040008B0 RID: 2224
	[PublicizedFrom(EAccessModifier.Protected)]
	public string emptyText = "CollectorEmptyText";

	// Token: 0x040008B1 RID: 2225
	[PublicizedFrom(EAccessModifier.Protected)]
	public string hasItem1Text = "CollectorHasItem1Text";

	// Token: 0x040008B2 RID: 2226
	[PublicizedFrom(EAccessModifier.Protected)]
	public string hasItem2Text = "CollectorHasItem2Text";

	// Token: 0x040008B3 RID: 2227
	[PublicizedFrom(EAccessModifier.Protected)]
	public int fuelGridHeight = 1;

	// Token: 0x040008B4 RID: 2228
	[PublicizedFrom(EAccessModifier.Protected)]
	public int catalystGridHeight = 1;

	// Token: 0x040008B5 RID: 2229
	[PublicizedFrom(EAccessModifier.Protected)]
	public int outputGridInitialHeight = 1;

	// Token: 0x040008B6 RID: 2230
	[PublicizedFrom(EAccessModifier.Protected)]
	public string fuelTitleText;

	// Token: 0x040008B7 RID: 2231
	[PublicizedFrom(EAccessModifier.Protected)]
	public string fuelTitleSprite = string.Empty;

	// Token: 0x040008B8 RID: 2232
	[PublicizedFrom(EAccessModifier.Protected)]
	public string fuelTitleSpriteAtlas = string.Empty;

	// Token: 0x040008B9 RID: 2233
	[PublicizedFrom(EAccessModifier.Protected)]
	public string catalystTitleText;

	// Token: 0x040008BA RID: 2234
	[PublicizedFrom(EAccessModifier.Protected)]
	public string catalystTitleSprite = string.Empty;

	// Token: 0x040008BB RID: 2235
	[PublicizedFrom(EAccessModifier.Protected)]
	public string catalystTitleSpriteAtlas = string.Empty;

	// Token: 0x040008BC RID: 2236
	[PublicizedFrom(EAccessModifier.Protected)]
	public string[] fuelTypesSprites = new string[0];

	// Token: 0x040008BD RID: 2237
	[PublicizedFrom(EAccessModifier.Protected)]
	public string[] fuelTypesSpriteAtlases = new string[0];

	// Token: 0x040008BE RID: 2238
	[PublicizedFrom(EAccessModifier.Protected)]
	public string[] catalystTypesSprites = new string[0];

	// Token: 0x040008BF RID: 2239
	[PublicizedFrom(EAccessModifier.Protected)]
	public string[] catalystTypesSpriteAtlases = new string[0];

	// Token: 0x040008C0 RID: 2240
	public BlockCollector.ModEffectTypes[] ModTypes;

	// Token: 0x040008C1 RID: 2241
	[PublicizedFrom(EAccessModifier.Private)]
	public string[] catalystTypes = new string[0];

	// Token: 0x040008C2 RID: 2242
	public Dictionary<string, int> CatalystMultipliers = new Dictionary<string, int>();

	// Token: 0x040008C3 RID: 2243
	public Dictionary<string, int> CatalystRequirements = new Dictionary<string, int>();

	// Token: 0x040008C4 RID: 2244
	public BlockCollector.CatalystConvert[] CatalystConverts = new BlockCollector.CatalystConvert[0];

	// Token: 0x040008C5 RID: 2245
	[PublicizedFrom(EAccessModifier.Private)]
	public new BlockActivationCommand[] cmds = new BlockActivationCommand[]
	{
		new BlockActivationCommand("Search", "search", true, false, null),
		new BlockActivationCommand("take", "hand", false, false, null)
	};

	// Token: 0x02000115 RID: 277
	[PublicizedFrom(EAccessModifier.Protected)]
	public enum CollectorTypes
	{
		// Token: 0x040008C7 RID: 2247
		DewCollector,
		// Token: 0x040008C8 RID: 2248
		Apiary,
		// Token: 0x040008C9 RID: 2249
		ChickenCoop
	}

	// Token: 0x02000116 RID: 278
	public class CatalystConvert
	{
		// Token: 0x06000769 RID: 1897 RVA: 0x00034EE3 File Offset: 0x000330E3
		public CatalystConvert(string _convertFrom, string _convertTo)
		{
			this.convertFrom = _convertFrom;
			this.convertTo = _convertTo;
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x00034EFC File Offset: 0x000330FC
		public ItemStack Convert(ItemStack _inStack)
		{
			ItemStack itemStack = null;
			if (!_inStack.IsEmpty() && _inStack.itemValue.ItemClass.Name == this.convertFrom)
			{
				itemStack = new ItemStack(ItemClass.GetItem(this.convertTo, false), _inStack.count);
			}
			return itemStack;
		}

		// Token: 0x040008CA RID: 2250
		[PublicizedFrom(EAccessModifier.Private)]
		public string convertFrom;

		// Token: 0x040008CB RID: 2251
		[PublicizedFrom(EAccessModifier.Private)]
		public string convertTo;
	}

	// Token: 0x02000117 RID: 279
	public class FuelType
	{
		// Token: 0x0600076B RID: 1899 RVA: 0x00034F4C File Offset: 0x0003314C
		public FuelType(string ftDef)
		{
			string[] array = ftDef.Split(',', StringSplitOptions.None);
			this.Name = array[0];
			this.Items = array.Skip<string>(1).ToArray<string>();
			for (int i = 0; i < this.Items.Length; i++)
			{
				this.Items[i] = this.Items[i].Trim();
			}
		}

		// Token: 0x040008CC RID: 2252
		public string Name;

		// Token: 0x040008CD RID: 2253
		public string[] Items;
	}

	// Token: 0x02000118 RID: 280
	public class OutputType
	{
		// Token: 0x0600076C RID: 1900 RVA: 0x00034FAC File Offset: 0x000331AC
		public OutputType(string otDef)
		{
			string[] array = otDef.Split(",", StringSplitOptions.None);
			int num = 0;
			this.Name = array[num++];
			this.Fuel = array[num++];
			this.FuelCost = this.safeParseInt(array[num++]);
			this.AdditionalFuelCost = this.safeParseInt(array[num++]);
			this.DiscountedFuelDivisor = this.safeParseInt(array[num++]);
			this.OutputItem = array[num++];
			this.OutputItemModded = array[num++];
			this.ModdedConvertSpeedMultiplier = this.safeParseInt(array[num++]);
			this.ModdedConvertCountMultiplier = this.safeParseInt(array[num++]);
			this.MinConvertTime = this.safeParseInt(array[num++]);
			this.MaxConvertTime = this.safeParseInt(array[num++]);
			this.ConvertSound = array[num++];
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x00035094 File Offset: 0x00033294
		[PublicizedFrom(EAccessModifier.Private)]
		public int safeParseInt(string intString)
		{
			int num;
			if (!int.TryParse(intString, out num))
			{
				num = 0;
			}
			return num;
		}

		// Token: 0x040008CE RID: 2254
		public string Name;

		// Token: 0x040008CF RID: 2255
		public string Fuel;

		// Token: 0x040008D0 RID: 2256
		public int FuelCost;

		// Token: 0x040008D1 RID: 2257
		public int AdditionalFuelCost;

		// Token: 0x040008D2 RID: 2258
		public int DiscountedFuelDivisor;

		// Token: 0x040008D3 RID: 2259
		public string OutputItem;

		// Token: 0x040008D4 RID: 2260
		public string OutputItemModded;

		// Token: 0x040008D5 RID: 2261
		public int ModdedConvertSpeedMultiplier;

		// Token: 0x040008D6 RID: 2262
		public int ModdedConvertCountMultiplier;

		// Token: 0x040008D7 RID: 2263
		public int MinConvertTime;

		// Token: 0x040008D8 RID: 2264
		public int MaxConvertTime;

		// Token: 0x040008D9 RID: 2265
		public string ConvertSound;
	}

	// Token: 0x02000119 RID: 281
	public enum ModEffectTypes
	{
		// Token: 0x040008DB RID: 2267
		Type,
		// Token: 0x040008DC RID: 2268
		Speed,
		// Token: 0x040008DD RID: 2269
		Count,
		// Token: 0x040008DE RID: 2270
		Modify,
		// Token: 0x040008DF RID: 2271
		Expand,
		// Token: 0x040008E0 RID: 2272
		Cost
	}
}
