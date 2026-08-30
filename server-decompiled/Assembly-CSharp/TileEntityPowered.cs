using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;

// Token: 0x02000C71 RID: 3185
public class TileEntityPowered : TileEntity, IPowered
{
	// Token: 0x170009E7 RID: 2535
	// (get) Token: 0x06006130 RID: 24880 RVA: 0x00266710 File Offset: 0x00264910
	// (set) Token: 0x06006131 RID: 24881 RVA: 0x00266764 File Offset: 0x00264964
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

	// Token: 0x170009E8 RID: 2536
	// (get) Token: 0x06006132 RID: 24882 RVA: 0x0026676D File Offset: 0x0026496D
	public virtual int PowerUsed
	{
		get
		{
			return this.RequiredPower;
		}
	}

	// Token: 0x170009E9 RID: 2537
	// (get) Token: 0x06006133 RID: 24883 RVA: 0x00266775 File Offset: 0x00264975
	public int ChildCount
	{
		get
		{
			return this.wireDataList.Count;
		}
	}

	// Token: 0x170009EA RID: 2538
	// (get) Token: 0x06006134 RID: 24884 RVA: 0x00266782 File Offset: 0x00264982
	// (set) Token: 0x06006135 RID: 24885 RVA: 0x0026678A File Offset: 0x0026498A
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

	// Token: 0x170009EB RID: 2539
	// (get) Token: 0x06006136 RID: 24886 RVA: 0x00266799 File Offset: 0x00264999
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

	// Token: 0x170009EC RID: 2540
	// (get) Token: 0x06006137 RID: 24887 RVA: 0x002667C3 File Offset: 0x002649C3
	// (set) Token: 0x06006138 RID: 24888 RVA: 0x002667CC File Offset: 0x002649CC
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

	// Token: 0x06006139 RID: 24889 RVA: 0x002668A4 File Offset: 0x00264AA4
	public TileEntityPowered(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x0600613A RID: 24890 RVA: 0x00266905 File Offset: 0x00264B05
	public Vector3i GetParent()
	{
		return this.parentPosition;
	}

	// Token: 0x0600613B RID: 24891 RVA: 0x0026690D File Offset: 0x00264B0D
	public bool HasParent()
	{
		return this.parentPosition.y != -9999;
	}

	// Token: 0x0600613C RID: 24892 RVA: 0x00266924 File Offset: 0x00264B24
	public PowerItem GetPowerItem()
	{
		return this.PowerItem;
	}

	// Token: 0x0600613D RID: 24893 RVA: 0x0026692C File Offset: 0x00264B2C
	public override void OnReadComplete()
	{
		base.OnReadComplete();
		this.InitializePowerData();
		this.CheckForNewWires();
	}

	// Token: 0x0600613E RID: 24894 RVA: 0x00266940 File Offset: 0x00264B40
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

	// Token: 0x0600613F RID: 24895 RVA: 0x00266A70 File Offset: 0x00264C70
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

	// Token: 0x06006140 RID: 24896 RVA: 0x00266B14 File Offset: 0x00264D14
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

	// Token: 0x06006141 RID: 24897 RVA: 0x00266B9C File Offset: 0x00264D9C
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

	// Token: 0x06006142 RID: 24898 RVA: 0x0026707F File Offset: 0x0026527F
	public void AddWireData(Vector3i child)
	{
		this.wireDataList.Add(child);
		this.SendWireData();
	}

	// Token: 0x06006143 RID: 24899 RVA: 0x00267094 File Offset: 0x00265294
	public void SendWireData()
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageWireActions>().Setup(NetPackageWireActions.WireActions.SendWires, base.ToWorldPos(), this.wireDataList, -1), false, -1, -1, -1, null, 192, false);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageWireActions>().Setup(NetPackageWireActions.WireActions.SendWires, base.ToWorldPos(), this.wireDataList, -1), false);
	}

	// Token: 0x06006144 RID: 24900 RVA: 0x00267108 File Offset: 0x00265308
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

	// Token: 0x06006145 RID: 24901 RVA: 0x00267168 File Offset: 0x00265368
	public void RemoveWires()
	{
		for (int i = 0; i < this.currentWireNodes.Count; i++)
		{
			WireManager.Instance.ReturnToPool(this.currentWireNodes[i]);
		}
		this.currentWireNodes.Clear();
	}

	// Token: 0x06006146 RID: 24902 RVA: 0x002671AC File Offset: 0x002653AC
	public void MarkWireDirty()
	{
		this.wiresDirty = true;
	}

	// Token: 0x06006147 RID: 24903 RVA: 0x002671B5 File Offset: 0x002653B5
	public void MarkChanged()
	{
		base.SetModified();
	}

	// Token: 0x06006148 RID: 24904 RVA: 0x002671C0 File Offset: 0x002653C0
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

	// Token: 0x06006149 RID: 24905 RVA: 0x0026726C File Offset: 0x0026546C
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

	// Token: 0x0600614A RID: 24906 RVA: 0x002672C4 File Offset: 0x002654C4
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

	// Token: 0x0600614B RID: 24907 RVA: 0x00267318 File Offset: 0x00265518
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

	// Token: 0x0600614C RID: 24908 RVA: 0x00267378 File Offset: 0x00265578
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual PowerItem CreatePowerItem()
	{
		return PowerItem.CreateItem(this.PowerItemType);
	}

	// Token: 0x0600614D RID: 24909 RVA: 0x00267385 File Offset: 0x00265585
	public override void OnUnload(World world)
	{
		base.OnUnload(world);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.PowerItem.RemoveTileEntity(this);
		}
		this.RemoveWires();
	}

	// Token: 0x0600614E RID: 24910 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool Activate(bool activated)
	{
		return false;
	}

	// Token: 0x0600614F RID: 24911 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ActivateOnce()
	{
		return false;
	}

	// Token: 0x06006150 RID: 24912 RVA: 0x002673AC File Offset: 0x002655AC
	public Vector3 GetWireOffset()
	{
		return this.WireOffset;
	}

	// Token: 0x06006151 RID: 24913 RVA: 0x0026676D File Offset: 0x0026496D
	public int GetRequiredPower()
	{
		return this.RequiredPower;
	}

	// Token: 0x06006152 RID: 24914 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual bool CanHaveParent(IPowered powered)
	{
		return true;
	}

	// Token: 0x06006153 RID: 24915 RVA: 0x002673B4 File Offset: 0x002655B4
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

	// Token: 0x06006154 RID: 24916 RVA: 0x002674E4 File Offset: 0x002656E4
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

	// Token: 0x06006155 RID: 24917 RVA: 0x00267601 File Offset: 0x00265801
	public void SetWireData(List<Vector3i> wireChildren)
	{
		this.wireDataList = wireChildren;
		this.RemoveWires();
		this.DrawWires();
	}

	// Token: 0x06006156 RID: 24918 RVA: 0x001B24E4 File Offset: 0x001B06E4
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.Powered;
	}

	// Token: 0x06006157 RID: 24919 RVA: 0x00048AAD File Offset: 0x00046CAD
	public override bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return LocalPlayerUI.GetUIForPrimaryPlayer() != null;
	}

	// Token: 0x06006158 RID: 24920 RVA: 0x00267618 File Offset: 0x00265818
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

	// Token: 0x04004C1C RID: 19484
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int ver = 1;

	// Token: 0x04004C1D RID: 19485
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool wiresDirty;

	// Token: 0x04004C1E RID: 19486
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isPlayerPlaced;

	// Token: 0x04004C1F RID: 19487
	public PowerItem.PowerItemTypes PowerItemType = PowerItem.PowerItemTypes.Consumer;

	// Token: 0x04004C20 RID: 19488
	[PublicizedFrom(EAccessModifier.Protected)]
	public PowerItem PowerItem;

	// Token: 0x04004C21 RID: 19489
	public Vector3 WireOffset = Vector3.zero;

	// Token: 0x04004C22 RID: 19490
	public float CenteredPitch;

	// Token: 0x04004C23 RID: 19491
	public float CenteredYaw;

	// Token: 0x04004C24 RID: 19492
	public string WindowGroupToOpen = string.Empty;

	// Token: 0x04004C25 RID: 19493
	[PublicizedFrom(EAccessModifier.Private)]
	public bool needBlockData;

	// Token: 0x04004C26 RID: 19494
	[PublicizedFrom(EAccessModifier.Private)]
	public int requiredPower;

	// Token: 0x04004C27 RID: 19495
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPowered;

	// Token: 0x04004C28 RID: 19496
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform blockTransform;

	// Token: 0x04004C29 RID: 19497
	[PublicizedFrom(EAccessModifier.Private)]
	public List<IWireNode> currentWireNodes = new List<IWireNode>();

	// Token: 0x04004C2A RID: 19498
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3i> wireDataList = new List<Vector3i>();

	// Token: 0x04004C2B RID: 19499
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activateDirty;

	// Token: 0x04004C2C RID: 19500
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i parentPosition = new Vector3i(-9999, -9999, -9999);
}
