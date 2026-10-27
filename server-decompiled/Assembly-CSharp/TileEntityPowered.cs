using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;

// Token: 0x02000C8B RID: 3211
public class TileEntityPowered : TileEntity, IPowered
{
	// Token: 0x17000A03 RID: 2563
	// (get) Token: 0x06006152 RID: 24914 RVA: 0x0026B0D8 File Offset: 0x002692D8
	// (set) Token: 0x06006153 RID: 24915 RVA: 0x0026B12C File Offset: 0x0026932C
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

	// Token: 0x17000A04 RID: 2564
	// (get) Token: 0x06006154 RID: 24916 RVA: 0x0026B135 File Offset: 0x00269335
	public virtual int PowerUsed
	{
		get
		{
			return this.RequiredPower;
		}
	}

	// Token: 0x17000A05 RID: 2565
	// (get) Token: 0x06006155 RID: 24917 RVA: 0x0026B13D File Offset: 0x0026933D
	public int ChildCount
	{
		get
		{
			return this.wireDataList.Count;
		}
	}

	// Token: 0x17000A06 RID: 2566
	// (get) Token: 0x06006156 RID: 24918 RVA: 0x0026B14A File Offset: 0x0026934A
	// (set) Token: 0x06006157 RID: 24919 RVA: 0x0026B152 File Offset: 0x00269352
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

	// Token: 0x17000A07 RID: 2567
	// (get) Token: 0x06006158 RID: 24920 RVA: 0x0026B161 File Offset: 0x00269361
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

	// Token: 0x17000A08 RID: 2568
	// (get) Token: 0x06006159 RID: 24921 RVA: 0x0026B18B File Offset: 0x0026938B
	// (set) Token: 0x0600615A RID: 24922 RVA: 0x0026B194 File Offset: 0x00269394
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

	// Token: 0x0600615B RID: 24923 RVA: 0x0026B26C File Offset: 0x0026946C
	public TileEntityPowered(Chunk _chunk)
		: base(_chunk)
	{
	}

	// Token: 0x0600615C RID: 24924 RVA: 0x0026B2CD File Offset: 0x002694CD
	public Vector3i GetParent()
	{
		return this.parentPosition;
	}

	// Token: 0x0600615D RID: 24925 RVA: 0x0026B2D5 File Offset: 0x002694D5
	public bool HasParent()
	{
		return this.parentPosition.y != -9999;
	}

	// Token: 0x0600615E RID: 24926 RVA: 0x0026B2EC File Offset: 0x002694EC
	public PowerItem GetPowerItem()
	{
		return this.PowerItem;
	}

	// Token: 0x0600615F RID: 24927 RVA: 0x0026B2F4 File Offset: 0x002694F4
	public override void OnReadComplete()
	{
		base.OnReadComplete();
		this.InitializePowerData();
		this.CheckForNewWires();
	}

	// Token: 0x06006160 RID: 24928 RVA: 0x0026B308 File Offset: 0x00269508
	public override void read(PooledBinaryReader _br, StreamModeRead _eStreamMode)
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
		if (_eStreamMode == StreamModeRead.FromServer)
		{
			this.isPowered = _br.ReadBoolean();
		}
		this.activateDirty = true;
		this.wiresDirty = true;
		if (num > 0)
		{
			if (_eStreamMode == StreamModeRead.FromServer)
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

	// Token: 0x06006161 RID: 24929 RVA: 0x0026B438 File Offset: 0x00269638
	public override void write(PooledBinaryWriter _bw, StreamModeWrite _eStreamMode)
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
		if (_eStreamMode == StreamModeWrite.ToClient)
		{
			_bw.Write(this.IsPowered);
		}
		_bw.Write(this.CenteredPitch);
		_bw.Write(this.CenteredYaw);
	}

	// Token: 0x06006162 RID: 24930 RVA: 0x0026B4DC File Offset: 0x002696DC
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

	// Token: 0x06006163 RID: 24931 RVA: 0x0026B564 File Offset: 0x00269764
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

	// Token: 0x06006164 RID: 24932 RVA: 0x0026BA47 File Offset: 0x00269C47
	public void AddWireData(Vector3i child)
	{
		this.wireDataList.Add(child);
		this.SendWireData();
	}

	// Token: 0x06006165 RID: 24933 RVA: 0x0026BA5C File Offset: 0x00269C5C
	public void SendWireData()
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageWireActions>().Setup(NetPackageWireActions.WireActions.SendWires, base.ToWorldPos(), this.wireDataList, -1), false, -1, -1, -1, null, 192, false);
			return;
		}
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageWireActions>().Setup(NetPackageWireActions.WireActions.SendWires, base.ToWorldPos(), this.wireDataList, -1), false);
	}

	// Token: 0x06006166 RID: 24934 RVA: 0x0026BAD0 File Offset: 0x00269CD0
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

	// Token: 0x06006167 RID: 24935 RVA: 0x0026BB30 File Offset: 0x00269D30
	public void RemoveWires()
	{
		for (int i = 0; i < this.currentWireNodes.Count; i++)
		{
			WireManager.Instance.ReturnToPool(this.currentWireNodes[i]);
		}
		this.currentWireNodes.Clear();
	}

	// Token: 0x06006168 RID: 24936 RVA: 0x0026BB74 File Offset: 0x00269D74
	public void MarkWireDirty()
	{
		this.wiresDirty = true;
	}

	// Token: 0x06006169 RID: 24937 RVA: 0x0026BB7D File Offset: 0x00269D7D
	public void MarkChanged()
	{
		base.SetModified();
	}

	// Token: 0x0600616A RID: 24938 RVA: 0x0026BB88 File Offset: 0x00269D88
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

	// Token: 0x0600616B RID: 24939 RVA: 0x0026BC34 File Offset: 0x00269E34
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

	// Token: 0x0600616C RID: 24940 RVA: 0x0026BC8C File Offset: 0x00269E8C
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

	// Token: 0x0600616D RID: 24941 RVA: 0x0026BCE0 File Offset: 0x00269EE0
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

	// Token: 0x0600616E RID: 24942 RVA: 0x0026BD40 File Offset: 0x00269F40
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual PowerItem CreatePowerItem()
	{
		return PowerItem.CreateItem(this.PowerItemType);
	}

	// Token: 0x0600616F RID: 24943 RVA: 0x0026BD4D File Offset: 0x00269F4D
	public override void OnUnload(World world)
	{
		base.OnUnload(world);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.PowerItem.RemoveTileEntity(this);
		}
		this.RemoveWires();
	}

	// Token: 0x06006170 RID: 24944 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool Activate(bool activated)
	{
		return false;
	}

	// Token: 0x06006171 RID: 24945 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ActivateOnce()
	{
		return false;
	}

	// Token: 0x06006172 RID: 24946 RVA: 0x0026BD74 File Offset: 0x00269F74
	public Vector3 GetWireOffset()
	{
		return this.WireOffset;
	}

	// Token: 0x06006173 RID: 24947 RVA: 0x0026B135 File Offset: 0x00269335
	public int GetRequiredPower()
	{
		return this.RequiredPower;
	}

	// Token: 0x06006174 RID: 24948 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool CanHaveParent(IPowered powered)
	{
		return true;
	}

	// Token: 0x06006175 RID: 24949 RVA: 0x0026BD7C File Offset: 0x00269F7C
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

	// Token: 0x06006176 RID: 24950 RVA: 0x0026BEAC File Offset: 0x0026A0AC
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

	// Token: 0x06006177 RID: 24951 RVA: 0x0026BFC9 File Offset: 0x0026A1C9
	public void SetWireData(List<Vector3i> wireChildren)
	{
		this.wireDataList = wireChildren;
		this.RemoveWires();
		this.DrawWires();
	}

	// Token: 0x06006178 RID: 24952 RVA: 0x001B39BC File Offset: 0x001B1BBC
	public override TileEntityType GetTileEntityType()
	{
		return TileEntityType.Powered;
	}

	// Token: 0x06006179 RID: 24953 RVA: 0x0026BFE0 File Offset: 0x0026A1E0
	public override bool OnLockResponseLocal(bool _success, PooledBinaryReader _brServerContext, ushort _channel)
	{
		LocalPlayerUI uiforPrimaryPlayer = LocalPlayerUI.GetUIForPrimaryPlayer();
		if (!_success)
		{
			GameManager.ShowTooltip(uiforPrimaryPlayer.entityPlayer, Localization.Get("ttNoInteractItem", false, null), string.Empty, "ui_denied", null, false, false, 0f);
			return false;
		}
		((XUiC_PoweredGenericWindowGroup)((XUiWindowGroup)uiforPrimaryPlayer.windowManager.GetWindow(this.WindowGroupToOpen)).Controller).TileEntity = this;
		uiforPrimaryPlayer.windowManager.Open(this.WindowGroupToOpen, true);
		return true;
	}

	// Token: 0x04004C78 RID: 19576
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int ver = 1;

	// Token: 0x04004C79 RID: 19577
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool wiresDirty;

	// Token: 0x04004C7A RID: 19578
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool isPlayerPlaced;

	// Token: 0x04004C7B RID: 19579
	public PowerItem.PowerItemTypes PowerItemType = PowerItem.PowerItemTypes.Consumer;

	// Token: 0x04004C7C RID: 19580
	[PublicizedFrom(EAccessModifier.Protected)]
	public PowerItem PowerItem;

	// Token: 0x04004C7D RID: 19581
	public Vector3 WireOffset = Vector3.zero;

	// Token: 0x04004C7E RID: 19582
	public float CenteredPitch;

	// Token: 0x04004C7F RID: 19583
	public float CenteredYaw;

	// Token: 0x04004C80 RID: 19584
	public string WindowGroupToOpen = string.Empty;

	// Token: 0x04004C81 RID: 19585
	[PublicizedFrom(EAccessModifier.Private)]
	public bool needBlockData;

	// Token: 0x04004C82 RID: 19586
	[PublicizedFrom(EAccessModifier.Private)]
	public int requiredPower;

	// Token: 0x04004C83 RID: 19587
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isPowered;

	// Token: 0x04004C84 RID: 19588
	[PublicizedFrom(EAccessModifier.Private)]
	public Transform blockTransform;

	// Token: 0x04004C85 RID: 19589
	[PublicizedFrom(EAccessModifier.Private)]
	public List<IWireNode> currentWireNodes = new List<IWireNode>();

	// Token: 0x04004C86 RID: 19590
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3i> wireDataList = new List<Vector3i>();

	// Token: 0x04004C87 RID: 19591
	[PublicizedFrom(EAccessModifier.Private)]
	public bool activateDirty;

	// Token: 0x04004C88 RID: 19592
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3i parentPosition = new Vector3i(-9999, -9999, -9999);
}
