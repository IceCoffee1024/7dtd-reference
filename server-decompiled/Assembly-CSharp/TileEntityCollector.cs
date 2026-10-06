using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Audio;
using UnityEngine;

// Token: 0x02000C78 RID: 3192
public class TileEntityCollector : TileEntity, IInventory
{
	// Token: 0x170009F2 RID: 2546
	// (get) Token: 0x060060B3 RID: 24755 RVA: 0x00265CD4 File Offset: 0x00263ED4
	// (set) Token: 0x060060B4 RID: 24756 RVA: 0x00265CF9 File Offset: 0x00263EF9
	public Vector2i containerSize
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			this.containerSizeInternal.x = 3;
			this.containerSizeInternal.y = this.OutputWindowHeight;
			return this.containerSizeInternal;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.containerSizeInternal = value;
		}
	}

	// Token: 0x170009F3 RID: 2547
	// (get) Token: 0x060060B5 RID: 24757 RVA: 0x00265D04 File Offset: 0x00263F04
	public BlockCollector collector
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			return base.blockValue.Block as BlockCollector;
		}
	}

	// Token: 0x060060B6 RID: 24758 RVA: 0x00265D24 File Offset: 0x00263F24
	public string[] GetFuelTypes()
	{
		List<string> list = new List<string>();
		foreach (string text in this.collector.Outputs)
		{
			BlockCollector.OutputType outputType = this.collector.GetOutputType(text);
			foreach (string text2 in this.collector.GetFuelType(outputType.Fuel).Items)
			{
				list.Add(text2);
			}
		}
		return list.ToArray();
	}

	// Token: 0x060060B7 RID: 24759 RVA: 0x00265DA4 File Offset: 0x00263FA4
	public string[] GetCatalystTypes()
	{
		return this.collector.CatalystTypes;
	}

	// Token: 0x060060B8 RID: 24760 RVA: 0x00265DB1 File Offset: 0x00263FB1
	[PublicizedFrom(EAccessModifier.Private)]
	public int getFuelCost(BlockCollector.OutputType outputType)
	{
		return this.collector.GetSandboxModifiedFuelNeeded(outputType.FuelCost);
	}

	// Token: 0x060060B9 RID: 24761 RVA: 0x00265DC4 File Offset: 0x00263FC4
	[PublicizedFrom(EAccessModifier.Private)]
	public int getAdditionalFuelCost(BlockCollector.OutputType outputType)
	{
		return outputType.AdditionalFuelCost;
	}

	// Token: 0x060060BA RID: 24762 RVA: 0x00265DCC File Offset: 0x00263FCC
	[PublicizedFrom(EAccessModifier.Private)]
	public ulong getCurrentConvertSpeed(BlockCollector.OutputType outputType)
	{
		return (ulong)((long)(this.HasModSpeed ? outputType.ModdedConvertSpeedMultiplier : 1));
	}

	// Token: 0x060060BB RID: 24763 RVA: 0x00265DE0 File Offset: 0x00263FE0
	[PublicizedFrom(EAccessModifier.Private)]
	public int getCurrentConvertCount(BlockCollector.OutputType outputType)
	{
		int num = 0;
		int catalystCount = this.getCatalystCount();
		int num2;
		if (!this.collector.CatalystRequirements.TryGetValue(outputType.Name, out num2))
		{
			num2 = 0;
		}
		if (catalystCount >= num2)
		{
			int num3;
			if (!this.collector.CatalystMultipliers.TryGetValue(outputType.Name, out num3))
			{
				num3 = 1;
			}
			if (this.collector.UsesCatalyst() && num3 > 0)
			{
				num = this.getCatalystCount() * num3;
			}
			else
			{
				num = (this.HasModCount ? outputType.ModdedConvertCountMultiplier : 1);
			}
		}
		return this.collector.GetSandboxModifiedOutput(num);
	}

	// Token: 0x060060BC RID: 24764 RVA: 0x00265E6C File Offset: 0x0026406C
	[PublicizedFrom(EAccessModifier.Private)]
	public int getCatalystCount()
	{
		int num = 0;
		for (int i = 0; i < this.CatalystSlots.Length; i++)
		{
			ItemStack itemStack = this.CatalystSlots[i];
			if (!itemStack.IsEmpty())
			{
				string name = itemStack.itemValue.ItemClass.Name;
				string[] catalystTypes = this.collector.CatalystTypes;
				for (int j = 0; j < catalystTypes.Length; j++)
				{
					if (catalystTypes[j] == name)
					{
						num++;
						break;
					}
				}
			}
		}
		return num;
	}

	// Token: 0x060060BD RID: 24765 RVA: 0x00265EE4 File Offset: 0x002640E4
	public bool IsSlotDisabled(int _slot)
	{
		BlockCollector.OutputType outputType = this.collector.GetOutputType(this.collector.Outputs[_slot]);
		return this.isDisabled(outputType);
	}

	// Token: 0x060060BE RID: 24766 RVA: 0x00265F11 File Offset: 0x00264111
	public BlockCollector.OutputType GetSlotOutputType(int slot)
	{
		return this.collector.GetOutputType(this.collector.Outputs[slot]);
	}

	// Token: 0x060060BF RID: 24767 RVA: 0x00265F2C File Offset: 0x0026412C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool anyEnabled()
	{
		bool flag = false;
		foreach (string text in this.collector.Outputs)
		{
			BlockCollector.OutputType outputType = this.collector.GetOutputType(text);
			flag |= !this.isDisabled(outputType);
			if (flag)
			{
				break;
			}
		}
		return flag;
	}

	// Token: 0x060060C0 RID: 24768 RVA: 0x00265F79 File Offset: 0x00264179
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isDisabled(BlockCollector.OutputType _outputType)
	{
		return this.isBlocked || this.outputTypeIsOutOfFuel(_outputType) || this.isUnderwater || this.outputTypeIsFull(_outputType);
	}

	// Token: 0x060060C1 RID: 24769 RVA: 0x00265FA0 File Offset: 0x002641A0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool outputTypeIsFull(BlockCollector.OutputType outputType)
	{
		bool flag;
		if (!this.isFull.TryGetValue(outputType.Name, out flag))
		{
			flag = true;
		}
		return flag;
	}

	// Token: 0x060060C2 RID: 24770 RVA: 0x00265FC8 File Offset: 0x002641C8
	[PublicizedFrom(EAccessModifier.Private)]
	public bool outputTypeIsOutOfFuel(BlockCollector.OutputType outputType)
	{
		bool flag;
		if (!this.outOfFuel.TryGetValue(outputType.Name, out flag))
		{
			flag = true;
		}
		return flag;
	}

	// Token: 0x060060C3 RID: 24771 RVA: 0x00265FF0 File Offset: 0x002641F0
	[PublicizedFrom(EAccessModifier.Private)]
	public void safeIstackArrayCopy(ItemStack[] inArray, ItemStack[] outArray)
	{
		int i;
		for (i = 0; i < inArray.Length; i++)
		{
			if (i >= outArray.Length)
			{
				break;
			}
			outArray[i] = inArray[i].Clone();
		}
		while (i < outArray.Length)
		{
			outArray[i] = ItemStack.Empty;
			i++;
		}
	}

	// Token: 0x060060C4 RID: 24772 RVA: 0x00266030 File Offset: 0x00264230
	[PublicizedFrom(EAccessModifier.Private)]
	public void safeFillArrayCopy(float[] inArray, float[] outArray)
	{
		int i;
		for (i = 0; i < inArray.Length; i++)
		{
			if (i >= outArray.Length)
			{
				break;
			}
			outArray[i] = inArray[i];
		}
		while (i < outArray.Length)
		{
			outArray[i] = -1f;
			i++;
		}
	}

	// Token: 0x060060C5 RID: 24773 RVA: 0x0026606C File Offset: 0x0026426C
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] safeItemStackArray(int count)
	{
		ItemStack[] array = new ItemStack[count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = ItemStack.Empty;
		}
		return array;
	}

	// Token: 0x170009F4 RID: 2548
	// (get) Token: 0x060060C6 RID: 24774 RVA: 0x00266097 File Offset: 0x00264297
	// (set) Token: 0x060060C7 RID: 24775 RVA: 0x002660CA File Offset: 0x002642CA
	public ItemStack[] Items
	{
		get
		{
			if (this.itemsInternal == null)
			{
				this.itemsInternal = this.safeItemStackArray(this.containerSize.x * this.containerSize.y);
			}
			return this.itemsInternal;
		}
		set
		{
			this.safeIstackArrayCopy(value, this.Items);
		}
	}

	// Token: 0x170009F5 RID: 2549
	// (get) Token: 0x060060C8 RID: 24776 RVA: 0x002660D9 File Offset: 0x002642D9
	public ItemStack[] ModSlots
	{
		get
		{
			if (this.modSlotsInternal == null)
			{
				this.modSlotsInternal = this.safeItemStackArray(3);
			}
			return this.modSlotsInternal;
		}
	}

	// Token: 0x060060C9 RID: 24777 RVA: 0x002660F6 File Offset: 0x002642F6
	public void SetModSlots(ItemStack[] value, bool fromRead = false)
	{
		this.safeIstackArrayCopy(value, this.ModSlots);
		this.visibleChanged = true;
		this.modsChanged = true;
		this.HandleModChanged(fromRead);
	}

	// Token: 0x170009F6 RID: 2550
	// (get) Token: 0x060060CA RID: 24778 RVA: 0x0026611A File Offset: 0x0026431A
	// (set) Token: 0x060060CB RID: 24779 RVA: 0x0026613C File Offset: 0x0026433C
	public ItemStack[] FuelSlots
	{
		get
		{
			if (this.fuelSlotsInternal == null)
			{
				this.fuelSlotsInternal = this.safeItemStackArray(this.FuelGridLength);
			}
			return this.fuelSlotsInternal;
		}
		set
		{
			this.safeIstackArrayCopy(value, this.FuelSlots);
			this.visibleChanged = true;
		}
	}

	// Token: 0x170009F7 RID: 2551
	// (get) Token: 0x060060CC RID: 24780 RVA: 0x00266152 File Offset: 0x00264352
	public int FuelGridHeight
	{
		get
		{
			return this.collector.FuelGridInitialHeight;
		}
	}

	// Token: 0x170009F8 RID: 2552
	// (get) Token: 0x060060CD RID: 24781 RVA: 0x0026615F File Offset: 0x0026435F
	public int FuelGridLength
	{
		get
		{
			return this.FuelGridHeight * 3;
		}
	}

	// Token: 0x170009F9 RID: 2553
	// (get) Token: 0x060060CE RID: 24782 RVA: 0x00266169 File Offset: 0x00264369
	public int CatalystGridHeight
	{
		get
		{
			return this.collector.CatalystGridInitialHeight + (this.HasModExpand ? 1 : 0);
		}
	}

	// Token: 0x170009FA RID: 2554
	// (get) Token: 0x060060CF RID: 24783 RVA: 0x00266183 File Offset: 0x00264383
	public int OutputWindowHeight
	{
		get
		{
			return this.collector.OutputGridInitialHeight + (this.HasModExpand ? 1 : 0);
		}
	}

	// Token: 0x170009FB RID: 2555
	// (get) Token: 0x060060D0 RID: 24784 RVA: 0x0026619D File Offset: 0x0026439D
	public int CatalystGridLength
	{
		get
		{
			return this.CatalystGridHeight * 3;
		}
	}

	// Token: 0x170009FC RID: 2556
	// (get) Token: 0x060060D1 RID: 24785 RVA: 0x002661A7 File Offset: 0x002643A7
	// (set) Token: 0x060060D2 RID: 24786 RVA: 0x002661C9 File Offset: 0x002643C9
	public ItemStack[] CatalystSlots
	{
		get
		{
			if (this.catalystSlotsInternal == null)
			{
				this.catalystSlotsInternal = this.safeItemStackArray(this.CatalystGridLength);
			}
			return this.catalystSlotsInternal;
		}
		set
		{
			this.safeIstackArrayCopy(value, this.CatalystSlots);
			this.visibleChanged = true;
		}
	}

	// Token: 0x060060D3 RID: 24787 RVA: 0x002661E0 File Offset: 0x002643E0
	public TileEntityCollector(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x060060D4 RID: 24788 RVA: 0x00266258 File Offset: 0x00264458
	[PublicizedFrom(EAccessModifier.Private)]
	public TileEntityCollector(TileEntityCollector _other)
		: base(null)
	{
		this.containerSize = _other.containerSize;
		this.Items = _other.Items;
		this.FuelSlots = _other.FuelSlots;
		this.CatalystSlots = _other.CatalystSlots;
		this.SetModSlots(_other.ModSlots, false);
		this.worldTimeTouched = _other.worldTimeTouched;
		this.bUserAccessing = _other.bUserAccessing;
	}

	// Token: 0x060060D5 RID: 24789 RVA: 0x00266328 File Offset: 0x00264528
	public void SetupModMeshes()
	{
		BlockEntityData blockEntity = base.GetChunk().GetBlockEntity(base.ToWorldPos());
		if (blockEntity != null && blockEntity.transform != null)
		{
			int num = this.collector.ModTransformEnableNames.Length;
			this.modEnableRenderers = new GameObject[num];
			this.modDisableRenderers = new GameObject[num];
			List<Transform> list = new List<Transform>();
			blockEntity.transform.GetComponentsInChildren<Transform>(true, list);
			for (int i = 0; i < num; i++)
			{
				this.modEnableRenderers[i] = null;
				if (!string.IsNullOrEmpty(this.collector.ModTransformEnableNames[i]))
				{
					foreach (Transform transform in list)
					{
						if (transform.name == this.collector.ModTransformEnableNames[i])
						{
							this.modEnableRenderers[i] = transform.gameObject;
							break;
						}
					}
				}
				this.modDisableRenderers[i] = null;
				if (!string.IsNullOrEmpty(this.collector.ModTransformDisableNames[i]))
				{
					foreach (Transform transform2 in list)
					{
						if (transform2.name == this.collector.ModTransformDisableNames[i])
						{
							this.modDisableRenderers[i] = transform2.gameObject;
							break;
						}
					}
				}
			}
			this.modMeshesSetup = true;
		}
	}

	// Token: 0x060060D6 RID: 24790 RVA: 0x002664B8 File Offset: 0x002646B8
	public override void OnDestroy()
	{
		if (this.collector != null)
		{
			Manager.BroadcastStop(base.ToWorldPos(), this.collector.RunningSound);
		}
		base.OnDestroy();
	}

	// Token: 0x060060D7 RID: 24791 RVA: 0x002664E3 File Offset: 0x002646E3
	public override TileEntity Clone()
	{
		return new TileEntityCollector(this);
	}

	// Token: 0x060060D8 RID: 24792 RVA: 0x002664EC File Offset: 0x002646EC
	public override void UpdateTick(World world)
	{
		base.UpdateTick(world);
		this.HandleUpdate(world);
		Vector3? vector = new Vector3?(base.ToWorldCenterPos());
		if (vector.Value == Vector3.zero)
		{
			vector = null;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageTileEntity>().Setup(this, StreamModeWrite.ToClient), true, -1, -1, -1, vector, 192, false);
	}

	// Token: 0x060060D9 RID: 24793 RVA: 0x00266550 File Offset: 0x00264750
	public void HandleUpdate(World world)
	{
		this.HandleModChanged(false);
		this.isUnderwater = base.blockValue.Block.IsUnderwater(GameManager.Instance.World, base.ToWorldPos(), base.blockValue);
		if (this.countdownBlockedCheck.HasPassed())
		{
			this.isBlocked = this.HandleSkyCheck();
			this.countdownBlockedCheck.ResetAndRestart();
		}
		bool flag = false;
		foreach (KeyValuePair<BlockCollector.OutputType, List<int>> keyValuePair in this.collector.OrderedSlotOutputs)
		{
			flag |= this.handleUpdateForOutputType(world, keyValuePair.Key, keyValuePair.Value);
		}
		if (flag)
		{
			base.NotifyListeners();
			base.emitHeatMapEvent(world, EnumAIDirectorChunkEvent.Campfire);
			this.setModified();
		}
		int num = (this.anyEnabled() ? 1 : 0);
		if (this.productionEnabled < 1 && num == 1)
		{
			if (this.productionEnabled > -1)
			{
				Manager.BroadcastPlay(base.ToWorldPos(), this.collector.ActivateSound, 0f);
			}
			Manager.BroadcastPlay(base.ToWorldPos(), this.collector.RunningSound, 0f);
		}
		else if (this.productionEnabled == 1 && num < 1)
		{
			Manager.BroadcastStop(base.ToWorldPos(), this.collector.RunningSound);
		}
		this.productionEnabled = num;
	}

	// Token: 0x060060DA RID: 24794 RVA: 0x002666C4 File Offset: 0x002648C4
	[PublicizedFrom(EAccessModifier.Private)]
	public int getFirstFreeIndex(List<int> _slotIndices)
	{
		int num = -1;
		foreach (int num2 in _slotIndices)
		{
			if (num2 < this.Items.Length)
			{
				ItemStack itemStack = this.Items[num2];
				if (itemStack == null || itemStack.IsEmpty())
				{
					num = num2;
					break;
				}
			}
		}
		return num;
	}

	// Token: 0x060060DB RID: 24795 RVA: 0x00266734 File Offset: 0x00264934
	[PublicizedFrom(EAccessModifier.Private)]
	public bool handleUpdateForOutputType(World world, BlockCollector.OutputType _outputType, List<int> _slotIndicess)
	{
		bool flag = false;
		BlockCollector.FuelType fuelType = this.collector.GetFuelType(_outputType.Fuel);
		this.outOfFuel[_outputType.Name] = this.getMaxProductionCount(_outputType, fuelType) <= 0;
		int num = this.getFirstFreeIndex(_slotIndicess);
		this.isFull[_outputType.Name] = num < 0;
		bool flag2 = this.isDisabled(_outputType);
		bool flag3 = this.wasDisabled < 0 || this.wasDisabled != (flag2 ? 1 : 0);
		this.wasDisabled = (flag2 ? 1 : 0);
		if (flag2)
		{
			this.resetTimeValues(_outputType);
		}
		if (flag3)
		{
			this.setModified();
		}
		this.worldTimeTouched = world.worldTime;
		ulong worldTime = world.worldTime;
		ulong num2;
		if (!this.lastWorldTimes.TryGetValue(_outputType.Name, out num2))
		{
			num2 = worldTime;
			this.lastWorldTimes[_outputType.Name] = worldTime;
		}
		ulong num3 = worldTime - num2;
		int num4 = (int)((num2 != 0UL) ? (num3 * this.getCurrentConvertSpeed(_outputType)) : 0UL);
		int sandboxModifiedTime = this.collector.GetSandboxModifiedTime(num4);
		bool flag4 = false;
		if (sandboxModifiedTime >= 0)
		{
			num4 = sandboxModifiedTime;
		}
		else
		{
			flag4 = true;
		}
		this.lastWorldTimes[_outputType.Name] = world.worldTime;
		while (!flag2 && (num4 > 0 || flag4))
		{
			TileEntityCollector.FillData fillData;
			if (!this.fillDataLookup.TryGetValue(_outputType.Name, out fillData))
			{
				int num5 = GameManager.Instance.World.GetGameRandom().RandomRange(_outputType.MinConvertTime, _outputType.MaxConvertTime);
				fillData = new TileEntityCollector.FillData(num, num5, num5);
				this.fillDataLookup.Add(_outputType.Name, fillData);
			}
			if (fillData.fillTimeLeft <= num4 || flag4)
			{
				num4 -= fillData.fillTimeLeft;
				this.Items[fillData.slot] = this.newItem(_outputType, fuelType);
				this.fillDataLookup.Remove(_outputType.Name);
				num = this.getFirstFreeIndex(_slotIndicess);
				this.isFull[_outputType.Name] = num < 0;
				this.outOfFuel[_outputType.Name] = this.getMaxProductionCount(_outputType, fuelType) <= 0;
				flag2 = this.isDisabled(_outputType);
			}
			else
			{
				fillData.fillTimeLeft -= num4;
				num4 = 0;
			}
		}
		return flag;
	}

	// Token: 0x060060DC RID: 24796 RVA: 0x00266978 File Offset: 0x00264B78
	public TileEntityCollector.FillData GetSlotFillData(int _slot)
	{
		TileEntityCollector.FillData fillData = null;
		foreach (TileEntityCollector.FillData fillData2 in this.fillDataLookup.Values)
		{
			if (fillData2.slot == _slot)
			{
				fillData = fillData2;
				break;
			}
		}
		return fillData;
	}

	// Token: 0x060060DD RID: 24797 RVA: 0x002669DC File Offset: 0x00264BDC
	public bool IsCurrentStack(int _slot)
	{
		bool flag = false;
		using (Dictionary<string, TileEntityCollector.FillData>.ValueCollection.Enumerator enumerator = this.fillDataLookup.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.slot == _slot)
				{
					flag = true;
					break;
				}
			}
		}
		return flag;
	}

	// Token: 0x060060DE RID: 24798 RVA: 0x00266A3C File Offset: 0x00264C3C
	[PublicizedFrom(EAccessModifier.Private)]
	public int getFuelSlotFuelCount(ItemStack itemStack, BlockCollector.FuelType fuelType)
	{
		int num = 0;
		string[] items = fuelType.Items;
		for (int i = 0; i < items.Length; i++)
		{
			if (!itemStack.IsEmpty() && items[i] == itemStack.itemValue.ItemClass.Name)
			{
				num = itemStack.count;
				break;
			}
		}
		return num;
	}

	// Token: 0x060060DF RID: 24799 RVA: 0x00266A8C File Offset: 0x00264C8C
	[PublicizedFrom(EAccessModifier.Private)]
	public int getFuelCount(BlockCollector.FuelType fuelType)
	{
		int num = 0;
		foreach (ItemStack itemStack in this.FuelSlots)
		{
			num += this.getFuelSlotFuelCount(itemStack, fuelType);
		}
		return num;
	}

	// Token: 0x060060E0 RID: 24800 RVA: 0x00266AC0 File Offset: 0x00264CC0
	[PublicizedFrom(EAccessModifier.Private)]
	public int fuelCost(BlockCollector.OutputType outputType, int itemsToCreate)
	{
		int num = 1;
		if (this.HasModCost)
		{
			num = outputType.DiscountedFuelDivisor;
		}
		return (this.getFuelCost(outputType) + this.getAdditionalFuelCost(outputType) * (itemsToCreate - 1) + num - 1) / num;
	}

	// Token: 0x060060E1 RID: 24801 RVA: 0x00266AF8 File Offset: 0x00264CF8
	[PublicizedFrom(EAccessModifier.Private)]
	public int getMaxProductionCount(BlockCollector.OutputType outputType, BlockCollector.FuelType fuelType)
	{
		int num = this.getCurrentConvertCount(outputType);
		int fuelCount = this.getFuelCount(fuelType);
		if (!this.collector.UsesFuel())
		{
			return num;
		}
		while (num > 0 && this.fuelCost(outputType, num) > fuelCount)
		{
			num--;
		}
		return num;
	}

	// Token: 0x060060E2 RID: 24802 RVA: 0x00266B38 File Offset: 0x00264D38
	[PublicizedFrom(EAccessModifier.Private)]
	public void removeFuel(int itemCount, BlockCollector.OutputType outputType, BlockCollector.FuelType fuelType)
	{
		int num = this.fuelCost(outputType, itemCount);
		num = this.collector.GetSandboxModifiedFuelNeeded(num);
		if (num == 0)
		{
			return;
		}
		ItemStack[] fuelSlots = this.FuelSlots;
		for (int i = 0; i < fuelSlots.Length; i++)
		{
			ItemStack itemStack = fuelSlots[i];
			int fuelSlotFuelCount = this.getFuelSlotFuelCount(itemStack, fuelType);
			if (fuelSlotFuelCount > 0)
			{
				if (fuelSlotFuelCount >= num)
				{
					itemStack.count -= num;
					return;
				}
				num -= itemStack.count;
				itemStack.count = 0;
				if (itemStack.count < 1)
				{
					fuelSlots[i] = ItemStack.Empty;
				}
			}
		}
	}

	// Token: 0x060060E3 RID: 24803 RVA: 0x00266BBC File Offset: 0x00264DBC
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetTimeValues(BlockCollector.OutputType _outputType)
	{
		this.lastWorldTimes[_outputType.Name] = GameManager.Instance.World.worldTime;
	}

	// Token: 0x060060E4 RID: 24804 RVA: 0x00266BE0 File Offset: 0x00264DE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void HandleModChanged(bool fromRead = false)
	{
		if (this.modsChanged)
		{
			if (!this.modMeshesSetup)
			{
				this.SetupModMeshes();
			}
			this.modsChanged = !this.modMeshesSetup;
			this.HasModConvert = false;
			this.HasModModify = false;
			this.HasModCount = false;
			this.HasModSpeed = false;
			this.HasModCost = false;
			this.HasModExpand = false;
			ItemStack[] modSlots = this.ModSlots;
			for (int i = 0; i < modSlots.Length; i++)
			{
				bool flag = modSlots[i].IsEmpty();
				if (!flag)
				{
					switch (this.collector.ModTypes[i])
					{
					case BlockCollector.ModEffectTypes.Type:
						this.HasModConvert = true;
						break;
					case BlockCollector.ModEffectTypes.Speed:
						this.HasModSpeed = true;
						break;
					case BlockCollector.ModEffectTypes.Count:
						this.HasModCount = true;
						break;
					case BlockCollector.ModEffectTypes.Modify:
						this.HasModModify = true;
						break;
					case BlockCollector.ModEffectTypes.Expand:
						this.HasModExpand = true;
						break;
					case BlockCollector.ModEffectTypes.Cost:
						this.HasModCost = true;
						break;
					}
				}
				if (this.modMeshesSetup)
				{
					if (this.modEnableRenderers[i] != null)
					{
						this.modEnableRenderers[i].gameObject.SetActive(!flag);
					}
					if (this.modDisableRenderers[i] != null)
					{
						this.modDisableRenderers[i].gameObject.SetActive(flag);
					}
				}
			}
			List<ItemStack> list = new List<ItemStack>();
			int num = this.CatalystGridLength;
			int num2 = this.CatalystSlots.Length;
			if (num2 != num)
			{
				ItemStack[] array = this.safeItemStackArray(num);
				this.safeIstackArrayCopy(this.CatalystSlots, array);
				if (!fromRead && num2 > num)
				{
					for (int j = num; j < this.CatalystSlots.Length; j++)
					{
						list.Add(this.CatalystSlots[j]);
					}
				}
				this.catalystSlotsInternal = null;
				this.CatalystSlots = array;
			}
			num = this.containerSize.x * this.containerSize.y;
			num2 = this.Items.Length;
			if (num2 != num)
			{
				ItemStack[] array2 = this.safeItemStackArray(num);
				this.safeIstackArrayCopy(this.itemsInternal, array2);
				if (!fromRead && num2 > num)
				{
					for (int k = num; k < this.itemsInternal.Length; k++)
					{
						list.Add(this.itemsInternal[k]);
					}
				}
				this.itemsInternal = null;
				this.Items = array2;
			}
			this.dropItems(list);
		}
		base.NotifyListeners();
	}

	// Token: 0x060060E5 RID: 24805 RVA: 0x00266E20 File Offset: 0x00265020
	[PublicizedFrom(EAccessModifier.Private)]
	public bool HandleSkyCheck()
	{
		Vector3i localChunkPos = base.localChunkPos;
		for (int i = 0; i < 7; i++)
		{
			localChunkPos.y++;
			if (localChunkPos.y >= 256)
			{
				break;
			}
			BlockValue block = this.chunk.GetBlock(localChunkPos);
			if (block.Block != base.blockValue.Block && block.Block.IsCollideArrows)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060060E6 RID: 24806 RVA: 0x00266E90 File Offset: 0x00265090
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack newItem(BlockCollector.OutputType outputType, BlockCollector.FuelType fuelType)
	{
		ItemStack itemStack = null;
		int maxProductionCount = this.getMaxProductionCount(outputType, fuelType);
		if (maxProductionCount > 0)
		{
			this.removeFuel(maxProductionCount, outputType, fuelType);
			itemStack = new ItemStack(new ItemValue(this.HasModConvert ? ItemClass.GetItemClass(outputType.OutputItemModded, false).Id : ItemClass.GetItemClass(outputType.OutputItem, false).Id, false), maxProductionCount);
		}
		return itemStack;
	}

	// Token: 0x060060E7 RID: 24807 RVA: 0x00266EF0 File Offset: 0x002650F0
	public void SetWorldTime()
	{
		ulong worldTime = GameManager.Instance.World.worldTime;
		foreach (string text in this.collector.Outputs)
		{
			this.lastWorldTimes[text] = worldTime;
		}
	}

	// Token: 0x060060E8 RID: 24808 RVA: 0x00266F38 File Offset: 0x00265138
	public Vector2i GetContainerSize()
	{
		return this.containerSize;
	}

	// Token: 0x060060E9 RID: 24809 RVA: 0x00266F40 File Offset: 0x00265140
	[PublicizedFrom(EAccessModifier.Private)]
	public void dropOutOfRangeFillData()
	{
		List<string> list = null;
		foreach (KeyValuePair<string, TileEntityCollector.FillData> keyValuePair in this.fillDataLookup)
		{
			string text;
			TileEntityCollector.FillData fillData;
			keyValuePair.Deconstruct(out text, out fillData);
			string text2 = text;
			TileEntityCollector.FillData fillData2 = fillData;
			if (fillData2 == null || fillData2.slot < 0 || fillData2.slot >= this.Items.Length)
			{
				if (list == null)
				{
					list = new List<string>();
				}
				list.Add(text2);
			}
		}
		if (list == null)
		{
			return;
		}
		foreach (string text3 in list)
		{
			TileEntityCollector.FillData fillData3 = this.fillDataLookup[text3];
			Log.Warning(string.Format("TileEntityCollector {0}: dropping out of range fill slot {1} for output '{2}'", base.ToWorldPos(), (fillData3 != null) ? fillData3.slot : (-1), text3));
			this.fillDataLookup.Remove(text3);
		}
	}

	// Token: 0x060060EA RID: 24810 RVA: 0x00267054 File Offset: 0x00265254
	public override void read(PooledBinaryReader _br, StreamModeRead _eStreamMode)
	{
		base.read(_br, _eStreamMode);
		BlockCollector.OutputType outputType = this.collector.OrderedSlotOutputs.Keys.ElementAt<BlockCollector.OutputType>(0);
		IEnumerable<BlockCollector.OutputType> enumerable = this.collector.OrderedSlotOutputs.Keys.Skip<BlockCollector.OutputType>(1);
		int num;
		if (_eStreamMode != StreamModeRead.Persistency)
		{
			num = 21;
		}
		else if (base.UseLocalVersioning())
		{
			num = (int)_br.ReadUInt16();
		}
		else
		{
			num = base.GetLegacyForkVersion();
		}
		if (num < 20)
		{
			Vector2i vector2i = default(Vector2i);
			vector2i.x = (int)_br.ReadUInt16();
			vector2i.y = (int)_br.ReadUInt16();
		}
		int num2;
		if (num >= 20)
		{
			num2 = (int)_br.ReadUInt16();
			if (base.bWaitingForServerResponse)
			{
				for (int i = 0; i < num2; i++)
				{
					_br.ReadString();
					_br.ReadUInt64();
				}
			}
			else
			{
				for (int j = 0; j < num2; j++)
				{
					string text = _br.ReadString();
					ulong num3 = _br.ReadUInt64();
					if (!string.IsNullOrEmpty(text))
					{
						this.lastWorldTimes[text] = num3;
					}
				}
			}
		}
		else if (base.bWaitingForServerResponse)
		{
			_br.ReadUInt64();
		}
		else
		{
			this.lastWorldTimes[outputType.Name] = _br.ReadUInt64();
			ulong worldTime = GameManager.Instance.World.worldTime;
			foreach (BlockCollector.OutputType outputType2 in enumerable)
			{
				this.lastWorldTimes[outputType2.Name] = worldTime;
			}
		}
		float num4 = 0f;
		if (num < 20)
		{
			num4 = _br.ReadSingle();
			_br.ReadInt16();
			_br.ReadSingle();
		}
		if (num >= 20)
		{
			num2 = (int)_br.ReadUInt16();
			this.fillDataLookup.Clear();
			while (num2 != 0)
			{
				if (base.bWaitingForServerResponse)
				{
					_br.ReadString();
					_br.ReadUInt32();
					_br.ReadUInt32();
					_br.ReadUInt32();
				}
				else
				{
					string text2 = _br.ReadString();
					int num5 = (int)_br.ReadUInt32();
					int num6 = (int)_br.ReadUInt32();
					int num7 = (int)_br.ReadUInt32();
					this.fillDataLookup[text2] = new TileEntityCollector.FillData(num5, num6, num7);
				}
				num2--;
			}
		}
		if (num >= 15)
		{
			if (base.bWaitingForServerResponse)
			{
				_br.ReadBoolean();
				_br.ReadBoolean();
				if (num >= 16)
				{
					if (num >= 20)
					{
						num2 = (int)_br.ReadUInt16();
						for (int k = 0; k < num2; k++)
						{
							_br.ReadString();
							_br.ReadByte();
						}
					}
					else
					{
						_br.ReadBoolean();
					}
				}
				if (num >= 16)
				{
					if (num >= 20)
					{
						num2 = (int)_br.ReadUInt16();
						for (int l = 0; l < num2; l++)
						{
							_br.ReadString();
							_br.ReadByte();
						}
					}
					else
					{
						_br.ReadBoolean();
					}
				}
			}
			else
			{
				this.isUnderwater = _br.ReadBoolean();
				this.isBlocked = _br.ReadBoolean();
				if (num >= 21)
				{
					num2 = (int)_br.ReadUInt16();
					for (int m = 0; m < num2; m++)
					{
						string text3 = _br.ReadString();
						bool flag = _br.ReadByte() > 0;
						this.outOfFuel[text3] = flag;
					}
				}
				else
				{
					bool flag2 = _br.ReadByte() > 0;
					this.outOfFuel[outputType.Name] = flag2;
					foreach (BlockCollector.OutputType outputType3 in enumerable)
					{
						this.outOfFuel[outputType3.Name] = true;
					}
				}
				if (num >= 16)
				{
					this.isFull.Clear();
					if (num >= 20)
					{
						num2 = (int)_br.ReadUInt16();
						for (int n = 0; n < num2; n++)
						{
							string text4 = _br.ReadString();
							bool flag3 = _br.ReadByte() > 0;
							this.isFull[text4] = flag3;
						}
					}
					else
					{
						bool flag4 = _br.ReadBoolean();
						this.isFull[outputType.Name] = flag4;
						foreach (BlockCollector.OutputType outputType4 in enumerable)
						{
							this.isFull[outputType4.Name] = true;
						}
					}
				}
			}
		}
		num2 = (int)_br.ReadInt16();
		ItemStack[] array = this.safeItemStackArray(num2);
		for (int num8 = 0; num8 < num2; num8++)
		{
			array[num8].Read(_br);
		}
		if (num < 20)
		{
			float[] array2 = new float[num2];
			for (int num9 = 0; num9 < num2; num9++)
			{
				array2[num9] = _br.ReadSingle();
			}
			if (!base.bWaitingForServerResponse)
			{
				this.fillDataLookup.Clear();
				for (int num10 = 0; num10 < num2; num10++)
				{
					if (array2[num10] >= 0f)
					{
						this.fillDataLookup[outputType.Name] = new TileEntityCollector.FillData(num10, (int)array2[num10], (int)num4);
						break;
					}
				}
			}
		}
		if (num >= 11)
		{
			int num11 = (int)_br.ReadInt16();
			ItemStack[] array3 = this.safeItemStackArray(num11);
			for (int num12 = 0; num12 < num11; num12++)
			{
				array3[num12].Read(_br);
			}
			if (!base.bWaitingForServerResponse)
			{
				this.modsChanged = true;
				this.SetModSlots(array3, true);
			}
		}
		if (!base.bWaitingForServerResponse)
		{
			this.Items = array;
			this.dropOutOfRangeFillData();
		}
		if (num >= 14)
		{
			int num13 = (int)_br.ReadInt16();
			ItemStack[] array4 = this.safeItemStackArray(num13);
			for (int num14 = 0; num14 < num13; num14++)
			{
				array4[num14].Read(_br);
			}
			if (!base.bWaitingForServerResponse)
			{
				this.FuelSlots = array4;
			}
		}
		if (num >= 19)
		{
			int num15 = (int)_br.ReadInt16();
			ItemStack[] array5 = this.safeItemStackArray(num15);
			for (int num16 = 0; num16 < num15; num16++)
			{
				array5[num16].Read(_br);
			}
			if (!base.bWaitingForServerResponse)
			{
				this.CatalystSlots = array5;
			}
		}
		foreach (BlockCollector.OutputType outputType5 in this.collector.OrderedSlotOutputs.Keys)
		{
			if (this.isDisabled(outputType5))
			{
				this.resetTimeValues(outputType5);
			}
		}
	}

	// Token: 0x060060EB RID: 24811 RVA: 0x00267674 File Offset: 0x00265874
	public override void write(PooledBinaryWriter stream, StreamModeWrite _eStreamMode)
	{
		base.write(stream, _eStreamMode);
		if (_eStreamMode == StreamModeWrite.Persistency)
		{
			stream.Write(21);
		}
		stream.Write((ushort)this.lastWorldTimes.Count);
		foreach (KeyValuePair<string, ulong> keyValuePair in this.lastWorldTimes)
		{
			stream.Write(keyValuePair.Key);
			stream.Write(keyValuePair.Value);
		}
		stream.Write((ushort)this.fillDataLookup.Count);
		foreach (KeyValuePair<string, TileEntityCollector.FillData> keyValuePair2 in this.fillDataLookup)
		{
			stream.Write(keyValuePair2.Key);
			stream.Write((uint)keyValuePair2.Value.slot);
			stream.Write((uint)keyValuePair2.Value.fillTime);
			stream.Write((uint)keyValuePair2.Value.fillTimeLeft);
		}
		stream.Write(this.isUnderwater);
		stream.Write(this.isBlocked);
		stream.Write((ushort)this.outOfFuel.Count);
		foreach (KeyValuePair<string, bool> keyValuePair3 in this.outOfFuel)
		{
			stream.Write(keyValuePair3.Key);
			stream.Write(keyValuePair3.Value ? 1 : 0);
		}
		stream.Write((ushort)this.isFull.Count);
		foreach (KeyValuePair<string, bool> keyValuePair4 in this.isFull)
		{
			stream.Write(keyValuePair4.Key);
			stream.Write(keyValuePair4.Value ? 1 : 0);
		}
		stream.Write((short)this.Items.Length);
		foreach (ItemStack itemStack in this.Items)
		{
			if (itemStack == null)
			{
				itemStack = ItemStack.Empty;
			}
			itemStack.Clone().Write(stream);
		}
		ItemStack[] modSlots = this.ModSlots;
		stream.Write((short)modSlots.Length);
		ItemStack[] array = modSlots;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Clone().Write(stream);
		}
		stream.Write((short)this.FuelSlots.Length);
		array = this.FuelSlots;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Clone().Write(stream);
		}
		stream.Write((short)this.CatalystSlots.Length);
		array = this.CatalystSlots;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Clone().Write(stream);
		}
	}

	// Token: 0x060060EC RID: 24812 RVA: 0x000470B6 File Offset: 0x000452B6
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.Collector;
	}

	// Token: 0x060060ED RID: 24813 RVA: 0x0026797C File Offset: 0x00265B7C
	public ItemStack[] GetItems()
	{
		return this.Items;
	}

	// Token: 0x060060EE RID: 24814 RVA: 0x00267984 File Offset: 0x00265B84
	public override void UpgradeDowngradeFrom(TileEntity _other)
	{
		base.UpgradeDowngradeFrom(_other);
		this.OnDestroy();
		if (_other is TileEntityCollector)
		{
			TileEntityCollector tileEntityCollector = _other as TileEntityCollector;
			this.worldTimeTouched = tileEntityCollector.worldTimeTouched;
			this.Items = ItemStack.Clone(tileEntityCollector.Items, 0, this.containerSize.x * this.containerSize.y);
			if (this.Items.Length != this.containerSize.x * this.containerSize.y)
			{
				Log.Error("UpgradeDowngradeFrom: other.size={0}, other.length={1}, this.size={2}, this.length={3}", new object[]
				{
					tileEntityCollector.containerSize,
					tileEntityCollector.Items.Length,
					this.containerSize,
					this.Items.Length
				});
			}
		}
	}

	// Token: 0x060060EF RID: 24815 RVA: 0x00267A54 File Offset: 0x00265C54
	public override void ReplacedBy(BlockValue _bvOld, BlockValue _bvNew, TileEntity _teNew)
	{
		base.ReplacedBy(_bvOld, _bvNew, _teNew);
		TileEntityCollector tileEntityCollector;
		if (!_teNew.TryGetSelfOrFeature<TileEntityCollector>(out tileEntityCollector))
		{
			List<ItemStack> list = new List<ItemStack>();
			list.AddRange(this.Items);
			list.AddRange(this.ModSlots);
			list.AddRange(this.FuelSlots);
			list.AddRange(this.CatalystSlots);
			this.dropItems(list);
		}
	}

	// Token: 0x060060F0 RID: 24816 RVA: 0x00267AB4 File Offset: 0x00265CB4
	[PublicizedFrom(EAccessModifier.Private)]
	public void dropItems(List<ItemStack> list)
	{
		Vector3 vector = base.ToWorldCenterPos();
		vector.y += 0.9f;
		GameManager.Instance.DropContentInLootContainerServer(-1, "DroppedLootContainer", vector, list.ToArray(), true, null);
	}

	// Token: 0x060060F1 RID: 24817 RVA: 0x00267AF9 File Offset: 0x00265CF9
	public void UpdateSlot(int _idx, ItemStack _item)
	{
		if (_item == null)
		{
			this.Items[_idx] = ItemStack.Empty;
		}
		else
		{
			this.Items[_idx] = _item.Clone();
		}
		base.NotifyListeners();
	}

	// Token: 0x060060F2 RID: 24818 RVA: 0x00267B24 File Offset: 0x00265D24
	public bool IsWaterEmpty()
	{
		for (int i = 0; i < this.Items.Length; i++)
		{
			if (this.Items[i] != null && !this.Items[i].IsEmpty())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060060F3 RID: 24819 RVA: 0x00267B60 File Offset: 0x00265D60
	public bool IsEmpty()
	{
		for (int i = 0; i < this.Items.Length; i++)
		{
			if (this.Items[i] != null && !this.Items[i].IsEmpty())
			{
				return false;
			}
		}
		ItemStack[] modSlots = this.ModSlots;
		for (int j = 0; j < modSlots.Length; j++)
		{
			if (!modSlots[j].IsEmpty())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060060F4 RID: 24820 RVA: 0x00267BC0 File Offset: 0x00265DC0
	public void SetEmpty()
	{
		for (int i = 0; i < this.Items.Length; i++)
		{
			this.Items[i].Clear();
		}
		this.fillDataLookup.Clear();
		base.NotifyListeners();
		this.setModified();
	}

	// Token: 0x060060F5 RID: 24821 RVA: 0x00267C04 File Offset: 0x00265E04
	[return: TupleElementNames(new string[] { "anyMoved", "allMoved" })]
	public ValueTuple<bool, bool> TryStackItem(int startIndex, ItemStack _itemStack)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.Collector, -1))
		{
			return new ValueTuple<bool, bool>(false, false);
		}
		int count = _itemStack.count;
		int num = 0;
		bool flag = false;
		for (int i = startIndex; i < this.Items.Length; i++)
		{
			num = _itemStack.count;
			if (_itemStack.itemValue.type == this.Items[i].itemValue.type && this.Items[i].CanStackPartly(ref num))
			{
				this.Items[i].count += num;
				_itemStack.count -= num;
				this.setModified();
				flag = true;
				if (_itemStack.count == 0)
				{
					base.NotifyListeners();
					return new ValueTuple<bool, bool>(true, true);
				}
			}
		}
		if (_itemStack.count != count)
		{
			flag = true;
			base.NotifyListeners();
		}
		return new ValueTuple<bool, bool>(flag, false);
	}

	// Token: 0x060060F6 RID: 24822 RVA: 0x00267CD8 File Offset: 0x00265ED8
	public bool AddItem(ItemStack _itemStack)
	{
		if (!_itemStack.CanMoveTo(XUiC_ItemStack.StackLocationTypes.Collector, -1))
		{
			return false;
		}
		for (int i = 0; i < this.Items.Length; i++)
		{
			if (this.Items[i] != null && this.Items[i].IsEmpty())
			{
				this.UpdateSlot(i, _itemStack);
				base.NotifyListeners();
				return true;
			}
		}
		return false;
	}

	// Token: 0x060060F7 RID: 24823 RVA: 0x00267D30 File Offset: 0x00265F30
	public bool HasItem(ItemValue _item)
	{
		for (int i = 0; i < this.Items.Length; i++)
		{
			if (this.Items[i] != null && this.Items[i].itemValue.ItemClass == _item.ItemClass)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060060F8 RID: 24824 RVA: 0x00267D78 File Offset: 0x00265F78
	public void RemoveItem(ItemValue _item)
	{
		bool flag = false;
		for (int i = 0; i < this.Items.Length; i++)
		{
			if (this.Items[i] != null && this.Items[i].itemValue.ItemClass == _item.ItemClass)
			{
				this.UpdateSlot(i, ItemStack.Empty);
				flag = true;
			}
		}
		if (flag)
		{
			base.NotifyListeners();
		}
	}

	// Token: 0x060060F9 RID: 24825 RVA: 0x00267DD8 File Offset: 0x00265FD8
	public override bool OnLockResponseLocal(bool _success, PooledBinaryReader _brServerContext, ushort _channel)
	{
		this.UpdateTick(GameManager.Instance.World);
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		if (!_success)
		{
			GameManager.ShowTooltip(uiforPrimaryPlayer.entityPlayer, Localization.Get("ttNoInteractItem", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			return false;
		}
		((XUiC_DewCollectorWindowGroup)((XUiWindowGroup)uiforPrimaryPlayer.windowManager.GetWindow("dewcollector")).Controller).SetTileEntity(this);
		uiforPrimaryPlayer.windowManager.Open("dewcollector", true);
		return true;
	}

	// Token: 0x060060FA RID: 24826 RVA: 0x00267E60 File Offset: 0x00266060
	public void UpdateVisible()
	{
		this.HandleModChanged(false);
	}

	// Token: 0x04004C06 RID: 19462
	[PublicizedFrom(EAccessModifier.Private)]
	public new const int Version = 21;

	// Token: 0x04004C07 RID: 19463
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector2i containerSizeInternal = new Vector2i(-1, -1);

	// Token: 0x04004C08 RID: 19464
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject[] modEnableRenderers;

	// Token: 0x04004C09 RID: 19465
	[PublicizedFrom(EAccessModifier.Private)]
	public GameObject[] modDisableRenderers;

	// Token: 0x04004C0A RID: 19466
	public bool HasModConvert;

	// Token: 0x04004C0B RID: 19467
	public bool HasModCount;

	// Token: 0x04004C0C RID: 19468
	public bool HasModSpeed;

	// Token: 0x04004C0D RID: 19469
	public bool HasModModify;

	// Token: 0x04004C0E RID: 19470
	public bool HasModCost;

	// Token: 0x04004C0F RID: 19471
	public bool HasModExpand;

	// Token: 0x04004C10 RID: 19472
	[PublicizedFrom(EAccessModifier.Private)]
	public static global::System.Random r = new global::System.Random();

	// Token: 0x04004C11 RID: 19473
	[PublicizedFrom(EAccessModifier.Private)]
	public CountdownTimer countdownBlockedCheck = new CountdownTimer(5f + (float)TileEntityCollector.r.NextDouble(), true);

	// Token: 0x04004C12 RID: 19474
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isBlocked;

	// Token: 0x04004C13 RID: 19475
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isUnderwater;

	// Token: 0x04004C14 RID: 19476
	public int wasDisabled = -1;

	// Token: 0x04004C15 RID: 19477
	[PublicizedFrom(EAccessModifier.Private)]
	public bool visibleChanged;

	// Token: 0x04004C16 RID: 19478
	[PublicizedFrom(EAccessModifier.Private)]
	public bool modsChanged;

	// Token: 0x04004C17 RID: 19479
	[PublicizedFrom(EAccessModifier.Private)]
	public int productionEnabled = -1;

	// Token: 0x04004C18 RID: 19480
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, bool> isFull = new Dictionary<string, bool>();

	// Token: 0x04004C19 RID: 19481
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, bool> outOfFuel = new Dictionary<string, bool>();

	// Token: 0x04004C1A RID: 19482
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, ulong> lastWorldTimes = new Dictionary<string, ulong>();

	// Token: 0x04004C1B RID: 19483
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<string, TileEntityCollector.FillData> fillDataLookup = new Dictionary<string, TileEntityCollector.FillData>();

	// Token: 0x04004C1C RID: 19484
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] itemsInternal;

	// Token: 0x04004C1D RID: 19485
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] modSlotsInternal;

	// Token: 0x04004C1E RID: 19486
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] fuelSlotsInternal;

	// Token: 0x04004C1F RID: 19487
	[PublicizedFrom(EAccessModifier.Private)]
	public ItemStack[] catalystSlotsInternal;

	// Token: 0x04004C20 RID: 19488
	public ulong worldTimeTouched;

	// Token: 0x04004C21 RID: 19489
	[PublicizedFrom(EAccessModifier.Private)]
	public bool modMeshesSetup;

	// Token: 0x02000C79 RID: 3193
	public class FillData
	{
		// Token: 0x060060FC RID: 24828 RVA: 0x00267E75 File Offset: 0x00266075
		public FillData(int _slot, int _fillTime, int _fillTimeLeft)
		{
			this.slot = _slot;
			this.fillTime = _fillTime;
			this.fillTimeLeft = _fillTimeLeft;
		}

		// Token: 0x04004C22 RID: 19490
		public int slot;

		// Token: 0x04004C23 RID: 19491
		public int fillTime;

		// Token: 0x04004C24 RID: 19492
		public int fillTimeLeft;
	}
}
