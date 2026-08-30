using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;

// Token: 0x02000C5F RID: 3167
public class TileEntityPowered : TileEntity, IPowered
{
	// Token: 0x170009D4 RID: 2516
	// (get) Token: 0x060060A5 RID: 24741 RVA: 0x00263828 File Offset: 0x00261A28
	// (set) Token: 0x060060A6 RID: 24742 RVA: 0x0026387C File Offset: 0x00261A7C
	public int RequiredPower
	{
		get
		{
			if (this.needBlockData && !SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				ushort num = (ushort)GameManager.Instance.World.GetBlock(base.ToWorldPos()).type;
				this.SetValuesFromBlock(num);
				this.needBlockData = false;
			}
			return this.requiredPower;
		}
		[PublicizedFrom(EAccessModifier.Private)]
		set
		{
			this.requiredPower = value;
		}
	}

	// Token: 0x170009D5 RID: 2517
	// (get) Token: 0x060060A7 RID: 24743 RVA: 0x00263885 File Offset: 0x00261A85
	public virtual int PowerUsed
	{
		get
		{
			return this.RequiredPower;
		}
	}

	// Token: 0x170009D6 RID: 2518
	// (get) Token: 0x060060A8 RID: 24744 RVA: 0x0026388D File Offset: 0x00261A8D
	public int ChildCount
	{
		get
		{
			return this.wireDataList.Count;
		}
	}

	// Token: 0x170009D7 RID: 2519
	// (get) Token: 0x060060A9 RID: 24745 RVA: 0x0026389A File Offset: 0x00261A9A
	// (set) Token: 0x060060AA RID: 24746 RVA: 0x002638A2 File Offset: 0x00261AA2
	public bool IsPlayerPlaced
	{
		get
		{
			return this.isPlayerPlaced;
		}
		set
		{
			this.isPlayerPlaced = value;
			this.setModified();
		}
	}

	// Token: 0x170009D8 RID: 2520
	// (get) Token: 0x060060AB RID: 24747 RVA: 0x002638B1 File Offset: 0x00261AB1
	public bool IsPowered
	{
		get
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				return this.PowerItem != null && this.PowerItem.IsPowered;
			}
			return this.isPowered;
		}
	}

	// Token: 0x170009D9 RID: 2521
	// (get) Token: 0x060060AC RID: 24748 RVA: 0x002638DB File Offset: 0x00261ADB
	// (set) Token: 0x060060AD RID: 24749 RVA: 0x002638E4 File Offset: 0x00261AE4
	public Transform BlockTransform
	{
		get
		{
			return this.blockTransform;
		}
		set
		{
			this.blockTransform = value;
			BlockValue block = GameManager.Instance.World.GetBlock(base.ToWorldPos());
			if (this.blockTransform != null)
			{
				Transform transform = this.blockTransform.Find("WireOffset");
				if (transform != null)
				{
					Vector3 vector = block.Block.shape.GetRotation(block) * transform.localPosition;
					this.WireOffset = vector;
					return;
				}
			}
			if (block.Block.Properties.Values.ContainsKey("WireOffset"))
			{
				Vector3 vector2 = block.Block.shape.GetRotation(block) * StringParsers.ParseVector3(block.Block.Properties.Values["WireOffset"], 0, -1);
				this.WireOffset = vector2;
			}
		}
	}

	// Token: 0x060060AE RID: 24750 RVA: 0x002639BC File Offset: 0x00261BBC
	public TileEntityPowered(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x060060AF RID: 24751 RVA: 0x00263A1D File Offset: 0x00261C1D
	public Vector3i GetParent()
	{
		return this.parentPosition;
	}

	// Token: 0x060060B0 RID: 24752 RVA: 0x00263A25 File Offset: 0x00261C25
	public bool HasParent()
	{
		return this.parentPosition.y != -9999;
	}

	// Token: 0x060060B1 RID: 24753 RVA: 0x00263A3C File Offset: 0x00261C3C
	public PowerItem GetPowerItem()
	{
		return this.PowerItem;
	}

	// Token: 0x060060B2 RID: 24754 RVA: 0x00263A44 File Offset: 0x00261C44
	public override void OnReadComplete()
	{
		base.OnReadComplete();
		this.InitializePowerData();
		this.CheckForNewWires();
	}

	// Token: 0x060060B3 RID: 24755 RVA: 0x00263A58 File Offset: 0x00261C58
	public override void read(PooledBinaryReader _br, TileEntity.StreamModeRead _eStreamMode)
	{
		base.read(_br, _eStreamMode);
		int num = _br.ReadInt32();
		this.isPlayerPlaced = _br.ReadBoolean();
		this.PowerItemType = (PowerItem.PowerItemTypes)_br.ReadByte();
		this.needBlockData = true;
		int num2 = (int)_br.ReadByte();
		this.wireDataList.Clear();
		for (int i = 0; i < num2; i++)
		{
			Vector3i vector3i = StreamUtils.ReadVector3i(_br);
			this.wireDataList.Add(vector3i);
		}
		this.parentPosition = StreamUtils.ReadVector3i(_br);
		if (_eStreamMode == TileEntity.StreamModeRead.FromServer)
		{
			this.isPowered = _br.ReadBoolean();
		}
		this.activateDirty = true;
		this.wiresDirty = true;
		if (num > 0)
		{
			if (_eStreamMode == TileEntity.StreamModeRead.FromServer)
			{
				bool flag = false;
				GUIWindow guiwindow;
				if (LocalPlayerUI.GetUIForPrimaryPlayer().windowManager.TryGetWindow(XUiC_PowerCameraWindowGroup.ID, out guiwindow))
				{
					XUiC_PowerCameraWindowGroup xuiC_PowerCameraWindowGroup = (XUiC_PowerCameraWindowGroup)((XUiWindowGroup)guiwindow).Controller;
					flag = base.IsUserAccessing() && xuiC_PowerCameraWindowGroup != null && xuiC_PowerCameraWindowGroup.TileEntity == this;
				}
				if (!flag)
				{
					this.CenteredPitch = _br.ReadSingle();
					this.CenteredYaw = _br.ReadSingle();
					return;
				}
				_br.ReadSingle();
				_br.ReadSingle();
				return;
			}
			else
			{
				this.CenteredPitch = _br.ReadSingle();
				this.CenteredYaw = _br.ReadSingle();
			}
		}
	}

	// Token: 0x060060B4 RID: 24756 RVA: 0x00263B88 File Offset: 0x00261D88
	public override void write(PooledBinaryWriter _bw, TileEntity.StreamModeWrite _eStreamMode)
	{
		base.write(_bw, _eStreamMode);
		_bw.Write(1);
		_bw.Write(this.isPlayerPlaced);
		_bw.Write((byte)this.PowerItemType);
		_bw.Write((byte)this.wireDataList.Count);
		for (int i = 0; i < this.wireDataList.Count; i++)
		{
			StreamUtils.Write(_bw, this.wireDataList[i]);
		}
		StreamUtils.Write(_bw, this.parentPosition);
		if (_eStreamMode == TileEntity.StreamModeWrite.ToClient)
		{
			_bw.Write(this.IsPowered);
		}
		_bw.Write(this.CenteredPitch);
		_bw.Write(this.CenteredYaw);
	}

	// Token: 0x060060B5 RID: 24757 RVA: 0x00263C2C File Offset: 0x00261E2C
	public void CheckForNewWires()
	{
		if (GameManager.Instance == null)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			for (int i = 0; i < this.wireDataList.Count; i++)
			{
				Vector3 vector = this.wireDataList[i].ToVector3();
				if (this.PowerItem.GetChild(vector) == null)
				{
					PowerItem powerItemByWorldPos = PowerManager.Instance.GetPowerItemByWorldPos(this.wireDataList[i]);
					PowerManager.Instance.SetParent(powerItemByWorldPos, this.PowerItem);
				}
			}
		}
	}

	// Token: 0x060060B6 RID: 24758 RVA: 0x00263CB4 File Offset: 0x00261EB4
	public void DrawWires()
	{
		if (this.BlockTransform == null)
		{
			this.wiresDirty = true;
			return;
		}
		WireManager instance = WireManager.Instance;
		bool flag = instance.ShowPulse;
		bool wiresShowing = instance.WiresShowing;
		if (this.wireDataList.Count > 0)
		{
			World world = GameManager.Instance.World;
			if (flag)
			{
				flag = world.CanPlaceBlockAt(base.ToWorldPos(), world.gameManager.GetPersistentLocalPlayer(), false);
			}
		}
		for (int i = 0; i < this.wireDataList.Count; i++)
		{
			Vector3i vector3i = this.wireDataList[i];
			if (GameManager.Instance.World.GetChunkFromWorldPos(vector3i) is Chunk)
			{
				TileEntityPowered tileEntityPowered = GameManager.Instance.World.GetTileEntity(vector3i) as TileEntityPowered;
				bool flag2 = false;
				if (tileEntityPowered != null && tileEntityPowered.BlockTransform != null)
				{
					flag2 = true;
				}
				if (!flag2)
				{
					this.wiresDirty = true;
					return;
				}
			}
		}
		int num = 0;
		for (int j = 0; j < this.wireDataList.Count; j++)
		{
			Vector3i vector3i2 = this.wireDataList[j];
			if (GameManager.Instance.World.GetChunkFromWorldPos(vector3i2) is Chunk)
			{
				TileEntityPowered tileEntityPowered2 = GameManager.Instance.World.GetTileEntity(vector3i2) as TileEntityPowered;
				if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer || !GameManager.IsDedicatedServer || tileEntityPowered2 == null || (this.PowerItemType == PowerItem.PowerItemTypes.TripWireRelay && tileEntityPowered2.PowerItemType == PowerItem.PowerItemTypes.TripWireRelay))
				{
					if (num >= this.currentWireNodes.Count)
					{
						IWireNode wireNodeFromPool = WireManager.Instance.GetWireNodeFromPool();
						this.currentWireNodes.Add(wireNodeFromPool);
					}
					this.currentWireNodes[num].SetStartPosition(this.BlockTransform.position + Origin.position);
					this.currentWireNodes[num].SetStartPositionOffset(this.WireOffset);
					if (tileEntityPowered2 != null)
					{
						if (this.PowerItemType == PowerItem.PowerItemTypes.ElectricWireRelay && tileEntityPowered2.PowerItemType == PowerItem.PowerItemTypes.ElectricWireRelay)
						{
							this.currentWireNodes[num].SetPulseColor(new Color32(0, 97, byte.MaxValue, byte.MaxValue));
							this.currentWireNodes[num].SetWireRadius(0.005f);
							this.currentWireNodes[num].SetWireDip(0f);
							ElectricWireController electricWireController = this.currentWireNodes[num].GetGameObject().GetComponent<ElectricWireController>();
							if (electricWireController == null)
							{
								electricWireController = this.currentWireNodes[num].GetGameObject().AddComponent<ElectricWireController>();
							}
							electricWireController.TileEntityParent = this as TileEntityPoweredMeleeTrap;
							electricWireController.TileEntityChild = tileEntityPowered2 as TileEntityPoweredMeleeTrap;
							electricWireController.WireNode = this.currentWireNodes[num];
							electricWireController.Init(this.chunk.GetBlock(base.localChunkPos).Block.Properties);
							electricWireController.WireNode.SetWireCanHide(false);
						}
						else if (this.PowerItemType == PowerItem.PowerItemTypes.TripWireRelay && tileEntityPowered2.PowerItemType == PowerItem.PowerItemTypes.TripWireRelay)
						{
							this.currentWireNodes[num].SetPulseColor(Color.magenta);
							this.currentWireNodes[num].SetWireRadius(0.0035f);
							this.currentWireNodes[num].SetWireDip(0f);
							TripWireController tripWireController = this.currentWireNodes[num].GetGameObject().GetComponent<TripWireController>();
							if (tripWireController == null)
							{
								tripWireController = this.currentWireNodes[num].GetGameObject().AddComponent<TripWireController>();
							}
							tripWireController.TileEntityParent = this as TileEntityPoweredTrigger;
							tripWireController.TileEntityChild = tileEntityPowered2 as TileEntityPoweredTrigger;
							tripWireController.WireNode = this.currentWireNodes[num];
							tripWireController.WireNode.SetWireCanHide(false);
						}
						else
						{
							global::UnityEngine.Object.Destroy(this.currentWireNodes[num].GetGameObject().GetComponent<ElectricWireController>());
							global::UnityEngine.Object.Destroy(this.currentWireNodes[num].GetGameObject().GetComponent<TripWireController>());
							this.currentWireNodes[num].SetWireCanHide(true);
						}
					}
					this.currentWireNodes[num].SetEndPosition(vector3i2.ToVector3());
					if (tileEntityPowered2 != null)
					{
						this.currentWireNodes[num].SetEndPositionOffset(tileEntityPowered2.WireOffset + new Vector3(0.5f, 0.5f, 0.5f));
					}
					this.currentWireNodes[num].BuildMesh();
					this.currentWireNodes[num].TogglePulse(flag);
					this.currentWireNodes[num].SetVisible(wiresShowing);
					num++;
				}
			}
		}
		for (int k = num; k < this.currentWireNodes.Count; k++)
		{
			IWireNode wireNode = this.currentWireNodes[num];
			WireManager.Instance.ReturnToPool(wireNode);
			this.currentWireNodes.Remove(wireNode);
		}
		this.wiresDirty = false;
	}

	// Token: 0x060060B7 RID: 24759 RVA: 0x00264197 File Offset: 0x00262397
	public void AddWireData(Vector3i child)
	{
		this.wireDataList.Add(child);
		this.SendWireData();
	}

	// Token: 0x060060B8 RID: 24760 RVA: 0x002641AC File Offset: 0x002623AC
	public void SendWireData()
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageWireActions>().Setup(NetPackageWireActions.WireActions.SendWires, base.ToWorldPos(), this.wireDataList, -1), false, -1, -1, -1, null, 192, false);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageWireActions>().Setup(NetPackageWireActions.WireActions.SendWires, base.ToWorldPos(), this.wireDataList, -1), false);
	}

	// Token: 0x060060B9 RID: 24761 RVA: 0x00264220 File Offset: 0x00262420
	public void CreateWireDataFromPowerItem()
	{
		this.wireDataList.Clear();
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			for (int i = 0; i < this.PowerItem.Children.Count; i++)
			{
				this.wireDataList.Add(this.PowerItem.Children[i].Position);
			}
		}
	}

	// Token: 0x060060BA RID: 24762 RVA: 0x00264280 File Offset: 0x00262480
	public void RemoveWires()
	{
		for (int i = 0; i < this.currentWireNodes.Count; i++)
		{
			WireManager.Instance.ReturnToPool(this.currentWireNodes[i]);
		}
		this.currentWireNodes.Clear();
	}

	// Token: 0x060060BB RID: 24763 RVA: 0x002642C4 File Offset: 0x002624C4
	public void MarkWireDirty()
	{
		this.wiresDirty = true;
	}

	// Token: 0x060060BC RID: 24764 RVA: 0x002642CD File Offset: 0x002624CD
	public void MarkChanged()
	{
		base.SetModified();
	}

	// Token: 0x060060BD RID: 24765 RVA: 0x002642D8 File Offset: 0x002624D8
	public void InitializePowerData()
	{
		if (GameManager.Instance == null)
		{
			return;
		}
		ushort num = (ushort)GameManager.Instance.World.GetBlock(base.ToWorldPos()).type;
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.PowerItem = PowerManager.Instance.GetPowerItemByWorldPos(base.ToWorldPos());
			if (this.PowerItem == null)
			{
				this.CreatePowerItemForTileEntity(num);
			}
			else
			{
				num = this.PowerItem.BlockID;
			}
			this.PowerItem.AddTileEntity(this);
			base.SetModified();
			this.activateDirty = true;
		}
		this.SetValuesFromBlock(num);
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.DrawWires();
		}
	}

	// Token: 0x060060BE RID: 24766 RVA: 0x00264384 File Offset: 0x00262584
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SetValuesFromBlock(ushort blockID)
	{
		if (Block.list[(int)blockID].Properties.Values.ContainsKey("RequiredPower"))
		{
			this.RequiredPower = Convert.ToInt32(Block.list[(int)blockID].Properties.Values["RequiredPower"]);
			return;
		}
		this.RequiredPower = 5;
	}

	// Token: 0x060060BF RID: 24767 RVA: 0x002643DC File Offset: 0x002625DC
	public override void UpdateTick(World world)
	{
		base.UpdateTick(world);
		if (this.BlockTransform != null)
		{
			if (this.wiresDirty)
			{
				this.DrawWires();
			}
			if (this.activateDirty)
			{
				this.Activate(this.PowerItem.IsPowered);
				this.activateDirty = false;
			}
		}
	}

	// Token: 0x060060C0 RID: 24768 RVA: 0x00264430 File Offset: 0x00262630
	public PowerItem CreatePowerItemForTileEntity(ushort blockID)
	{
		if (this.PowerItem == null)
		{
			this.PowerItem = this.CreatePowerItem();
			this.PowerItem.Position = base.ToWorldPos();
			this.PowerItem.BlockID = blockID;
			this.PowerItem.SetValuesFromBlock();
			PowerManager.Instance.AddPowerNode(this.PowerItem, null);
		}
		return this.PowerItem;
	}

	// Token: 0x060060C1 RID: 24769 RVA: 0x00264490 File Offset: 0x00262690
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual PowerItem CreatePowerItem()
	{
		return PowerItem.CreateItem(this.PowerItemType);
	}

	// Token: 0x060060C2 RID: 24770 RVA: 0x0026449D File Offset: 0x0026269D
	public override void OnUnload(World world)
	{
		base.OnUnload(world);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.PowerItem.RemoveTileEntity(this);
		}
		this.RemoveWires();
	}

	// Token: 0x060060C3 RID: 24771 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool Activate(bool activated)
	{
		return false;
	}

	// Token: 0x060060C4 RID: 24772 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ActivateOnce()
	{
		return false;
	}

	// Token: 0x060060C5 RID: 24773 RVA: 0x002644C4 File Offset: 0x002626C4
	public Vector3 GetWireOffset()
	{
		return this.WireOffset;
	}

	// Token: 0x060060C6 RID: 24774 RVA: 0x00263885 File Offset: 0x00261A85
	public int GetRequiredPower()
	{
		return this.RequiredPower;
	}

	// Token: 0x060060C7 RID: 24775 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool CanHaveParent(IPowered powered)
	{
		return true;
	}

	// Token: 0x060060C8 RID: 24776 RVA: 0x002644CC File Offset: 0x002626CC
	public void SetParentWithWireTool(IPowered newParentTE, int wiringEntityID)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			PowerItem powerItem = newParentTE.GetPowerItem();
			PowerItem parent = this.PowerItem.Parent;
			PowerManager.Instance.SetParent(this.PowerItem, powerItem);
			if (parent != null && parent.TileEntity != null)
			{
				parent.TileEntity.CreateWireDataFromPowerItem();
				parent.TileEntity.SendWireData();
				parent.TileEntity.RemoveWires();
				parent.TileEntity.DrawWires();
			}
			newParentTE.CreateWireDataFromPowerItem();
			newParentTE.SendWireData();
			newParentTE.RemoveWires();
			newParentTE.DrawWires();
			Manager.BroadcastPlay(base.ToWorldPos().ToVector3(), powerItem.IsPowered ? "wire_live_connect" : "wire_dead_connect", 0f);
		}
		else
		{
			this.parentPosition = ((TileEntity)newParentTE).ToWorldPos();
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageWireActions>().Setup(NetPackageWireActions.WireActions.SetParent, base.ToWorldPos(), new List<Vector3i> { this.parentPosition }, wiringEntityID), false);
			Manager.BroadcastPlay(base.ToWorldPos().ToVector3(), this.IsPowered ? "wire_live_connect" : "wire_dead_connect", 0f);
		}
		base.SetModified();
	}

	// Token: 0x060060C9 RID: 24777 RVA: 0x002645FC File Offset: 0x002627FC
	public void RemoveParentWithWiringTool(int wiringEntityID)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			if (this.PowerItem.Parent != null)
			{
				Vector3i position = this.PowerItem.Parent.Position;
				PowerItem parent = this.PowerItem.Parent;
				this.PowerItem.RemoveSelfFromParent();
				if (parent.TileEntity != null)
				{
					parent.TileEntity.CreateWireDataFromPowerItem();
					parent.TileEntity.SendWireData();
					parent.TileEntity.RemoveWires();
					parent.TileEntity.DrawWires();
				}
				Manager.BroadcastPlay(position.ToVector3(), this.PowerItem.IsPowered ? "wire_live_break" : "wire_dead_break", 0f);
			}
		}
		else
		{
			Vector3i vector3i = base.ToWorldPos();
			Vector3 vector = vector3i.ToVector3();
			this.parentPosition = new Vector3i(-9999, -9999, -9999);
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageWireActions>().Setup(NetPackageWireActions.WireActions.RemoveParent, vector3i, new List<Vector3i>(), wiringEntityID), false);
			Manager.BroadcastPlay(vector, this.IsPowered ? "wire_live_break" : "wire_dead_break", 0f);
		}
		base.SetModified();
	}

	// Token: 0x060060CA RID: 24778 RVA: 0x00264719 File Offset: 0x00262919
	public void SetWireData(List<Vector3i> wireChildren)
	{
		this.wireDataList = wireChildren;
		this.RemoveWires();
		this.DrawWires();
	}

	// Token: 0x060060CB RID: 24779 RVA: 0x001AFF0C File Offset: 0x001AE10C
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.Powered;
	}

	// Token: 0x060060CC RID: 24780 RVA: 0x00048695 File Offset: 0x00046895
	public override bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return LocalPlayerUI.GetUIForPrimaryPlayer() != null;
	}

	// Token: 0x060060CD RID: 24781 RVA: 0x00264730 File Offset: 0x00262930
	public override void OnLockedLocal(bool _success, ILockContext _context, ushort _channel)
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		if (!_success)
		{
			GameManager.ShowTooltip(uiforPrimaryPlayer.entityPlayer, Localization.Get("ttNoInteractItem", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			return;
		}
		((XUiC_PoweredGenericWindowGroup)((XUiWindowGroup)uiforPrimaryPlayer.windowManager.GetWindow(this.WindowGroupToOpen)).Controller).TileEntity = this;
		uiforPrimaryPlayer.windowManager.Open(this.WindowGroupToOpen, true);
	}

	// Token: 0x04004B7F RID: 19327
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int ver = 1;

	// Token: 0x04004B80 RID: 19328
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool wiresDirty;

	// Token: 0x04004B81 RID: 19329
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isPlayerPlaced;

	// Token: 0x04004B82 RID: 19330
	public PowerItem.PowerItemTypes PowerItemType = PowerItem.PowerItemTypes.Consumer;

	// Token: 0x04004B83 RID: 19331
	[PublicizedFrom(EAccessModifier.Protected)]
	public PowerItem PowerItem;

	// Token: 0x04004B84 RID: 19332
	public Vector3 WireOffset = Vector3.zero;

	// Token: 0x04004B85 RID: 19333
	public float CenteredPitch;

	// Token: 0x04004B86 RID: 19334
	public float CenteredYaw;

	// Token: 0x04004B87 RID: 19335
	public string WindowGroupToOpen = string.Empty;

	// Token: 0x04004B88 RID: 19336
	[PublicizedFrom(EAccessModifier.Private)]
	public bool needBlockData;

	// Token: 0x04004B89 RID: 19337
	[PublicizedFrom(EAccessModifier.Private)]
	public int requiredPower;

	// Token: 0x04004B8A RID: 19338
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPowered;

	// Token: 0x04004B8B RID: 19339
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform blockTransform;

	// Token: 0x04004B8C RID: 19340
	[PublicizedFrom(EAccessModifier.Private)]
	public List<IWireNode> currentWireNodes = new List<IWireNode>();

	// Token: 0x04004B8D RID: 19341
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3i> wireDataList = new List<Vector3i>();

	// Token: 0x04004B8E RID: 19342
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activateDirty;

	// Token: 0x04004B8F RID: 19343
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i parentPosition = new Vector3i(-9999, -9999, -9999);
}
