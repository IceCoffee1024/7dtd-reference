using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x0200091E RID: 2334
public class PowerManager
{
	// Token: 0x17000740 RID: 1856
	// (get) Token: 0x060042DC RID: 17116 RVA: 0x001A3EFE File Offset: 0x001A20FE
	// (set) Token: 0x060042DD RID: 17117 RVA: 0x001A3F06 File Offset: 0x001A2106
	public byte CurrentFileVersion { get; set; }

	// Token: 0x17000741 RID: 1857
	// (get) Token: 0x060042DE RID: 17118 RVA: 0x001A3F0F File Offset: 0x001A210F
	public static PowerManager Instance
	{
		get
		{
			if (PowerManager.instance == null)
			{
				PowerManager.instance = new PowerManager();
			}
			return PowerManager.instance;
		}
	}

	// Token: 0x17000742 RID: 1858
	// (get) Token: 0x060042DF RID: 17119 RVA: 0x001A3F27 File Offset: 0x001A2127
	public static bool HasInstance
	{
		get
		{
			return PowerManager.instance != null;
		}
	}

	// Token: 0x060042E0 RID: 17120 RVA: 0x001A3F34 File Offset: 0x001A2134
	[PublicizedFrom(EAccessModifier.Private)]
	public PowerManager()
	{
		PowerManager.instance = this;
		this.Circuits = new List<PowerItem>();
		this.PowerSources = new List<PowerSource>();
		this.PowerTriggers = new List<PowerTrigger>();
	}

	// Token: 0x060042E1 RID: 17121 RVA: 0x001A3F8F File Offset: 0x001A218F
	public void SetTileEntityUpdate(TileEntityPoweredBlock tileEntity, bool shouldUpdate)
	{
		if (shouldUpdate)
		{
			this.ClientUpdateList.Add(tileEntity);
			return;
		}
		this.ClientUpdateList.Remove(tileEntity);
	}

	// Token: 0x060042E2 RID: 17122 RVA: 0x001A3FB0 File Offset: 0x001A21B0
	public void Update()
	{
		if (GameManager.Instance.World == null || GameManager.Instance.World.Players == null || GameManager.Instance.World.Players.Count == 0)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && GameManager.Instance.gameStateManager.IsGameStarted())
		{
			this.updateTime -= Time.deltaTime;
			if (this.updateTime <= 0f)
			{
				for (int i = 0; i < this.PowerSources.Count; i++)
				{
					this.PowerSources[i].Update();
				}
				for (int j = 0; j < this.PowerTriggers.Count; j++)
				{
					this.PowerTriggers[j].CachedUpdateCall();
				}
				this.updateTime = 0.16f;
			}
			this.saveTime -= Time.deltaTime;
			if (this.saveTime <= 0f && (this.dataSaveThreadInfo == null || this.dataSaveThreadInfo.HasTerminated()))
			{
				this.saveTime = 120f;
				this.SavePowerManager();
			}
		}
		foreach (TileEntityPoweredBlock tileEntityPoweredBlock in this.ClientUpdateList)
		{
			tileEntityPoweredBlock.ClientUpdate();
		}
	}

	// Token: 0x060042E3 RID: 17123 RVA: 0x001A4114 File Offset: 0x001A2314
	[PublicizedFrom(EAccessModifier.Private)]
	public int savePowerDataThreaded(ThreadManager.ThreadInfo _threadInfo)
	{
		PooledExpandableMemoryStream pooledExpandableMemoryStream = (PooledExpandableMemoryStream)_threadInfo.parameter;
		string text = string.Format("{0}/{1}", GameIO.GetSaveGameDir(), "power.dat");
		if (SdFile.Exists(text))
		{
			SdFile.Copy(text, string.Format("{0}/{1}", GameIO.GetSaveGameDir(), "power.dat.bak"), true);
		}
		pooledExpandableMemoryStream.Position = 0L;
		StreamUtils.WriteStreamToFile(pooledExpandableMemoryStream, text);
		MemoryPools.poolMemoryStream.FreeSync(pooledExpandableMemoryStream);
		return -1;
	}

	// Token: 0x060042E4 RID: 17124 RVA: 0x001A4180 File Offset: 0x001A2380
	public void LoadPowerManager()
	{
		string text = string.Format("{0}/{1}", GameIO.GetSaveGameDir(), "power.dat");
		if (SdFile.Exists(text))
		{
			try
			{
				using (Stream stream = SdFile.OpenRead(text))
				{
					using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
					{
						pooledBinaryReader.SetBaseStream(stream);
						this.Read(pooledBinaryReader);
					}
				}
			}
			catch (Exception)
			{
				text = string.Format("{0}/{1}", GameIO.GetSaveGameDir(), "power.dat.bak");
				if (SdFile.Exists(text))
				{
					using (Stream stream2 = SdFile.OpenRead(text))
					{
						using (PooledBinaryReader pooledBinaryReader2 = MemoryPools.poolBinaryReader.AllocSync(false))
						{
							pooledBinaryReader2.SetBaseStream(stream2);
							this.Read(pooledBinaryReader2);
						}
					}
				}
			}
		}
	}

	// Token: 0x060042E5 RID: 17125 RVA: 0x001A4288 File Offset: 0x001A2488
	public void SavePowerManager()
	{
		if (this.dataSaveThreadInfo == null || !ThreadManager.ActiveThreads.ContainsKey("powerDataSave"))
		{
			PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true);
			using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
			{
				pooledBinaryWriter.SetBaseStream(pooledExpandableMemoryStream);
				this.Write(pooledBinaryWriter);
			}
			this.dataSaveThreadInfo = ThreadManager.StartThread("powerDataSave", null, new ThreadManager.ThreadFunctionLoopDelegate(this.savePowerDataThreaded), null, pooledExpandableMemoryStream, null, false, true);
		}
	}

	// Token: 0x060042E6 RID: 17126 RVA: 0x001A4314 File Offset: 0x001A2514
	public void Write(PooledBinaryWriter bw)
	{
		bw.Write(PowerManager.FileVersion);
		bw.Write(this.Circuits.Count);
		for (int i = 0; i < this.Circuits.Count; i++)
		{
			bw.Write((byte)this.Circuits[i].PowerItemType);
			this.Circuits[i].write(bw);
		}
	}

	// Token: 0x060042E7 RID: 17127 RVA: 0x001A4380 File Offset: 0x001A2580
	public void Read(PooledBinaryReader br)
	{
		this.CurrentFileVersion = br.ReadByte();
		this.Circuits.Clear();
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			PowerItem powerItem = PowerItem.CreateItem((PowerItem.PowerItemTypes)br.ReadByte());
			powerItem.read(br, this.CurrentFileVersion);
			this.AddPowerNode(powerItem, null);
		}
	}

	// Token: 0x060042E8 RID: 17128 RVA: 0x001A43D8 File Offset: 0x001A25D8
	public void Cleanup()
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.SavePowerManager();
		}
		PowerManager.instance = null;
		this.Circuits.Clear();
		if (this.dataSaveThreadInfo != null)
		{
			this.dataSaveThreadInfo.WaitForEnd(30);
			this.dataSaveThreadInfo = null;
		}
	}

	// Token: 0x060042E9 RID: 17129 RVA: 0x001A4424 File Offset: 0x001A2624
	public void AddPowerNode(PowerItem node, PowerItem parent = null)
	{
		this.Circuits.Add(node);
		this.SetParent(node, parent);
		if (node is PowerSource)
		{
			this.PowerSources.Add((PowerSource)node);
		}
		if (node is PowerTrigger)
		{
			this.PowerTriggers.Add((PowerTrigger)node);
		}
		this.PowerItemDictionary.Add(node.Position, node);
	}

	// Token: 0x060042EA RID: 17130 RVA: 0x001A448C File Offset: 0x001A268C
	public void RemovePowerNode(PowerItem node)
	{
		foreach (PowerItem powerItem in new List<PowerItem>(node.Children))
		{
			this.SetParent(powerItem, null);
		}
		this.SetParent(node, null);
		this.Circuits.Remove(node);
		if (node is PowerSource)
		{
			this.PowerSources.Remove((PowerSource)node);
		}
		if (node is PowerTrigger)
		{
			this.PowerTriggers.Remove((PowerTrigger)node);
		}
		if (this.PowerItemDictionary.ContainsKey(node.Position))
		{
			this.PowerItemDictionary.Remove(node.Position);
		}
	}

	// Token: 0x060042EB RID: 17131 RVA: 0x001A4554 File Offset: 0x001A2754
	public void FindPowerItems(Func<PowerItem, bool> _predicate, List<PowerItem> _results)
	{
		foreach (PowerItem powerItem in this.PowerItemDictionary.Values)
		{
			if (_predicate(powerItem))
			{
				_results.Add(powerItem);
			}
		}
	}

	// Token: 0x060042EC RID: 17132 RVA: 0x001A45B8 File Offset: 0x001A27B8
	public void SetParent(PowerItem child, PowerItem parent)
	{
		if (child == null)
		{
			return;
		}
		if (child.Parent == parent)
		{
			return;
		}
		if (this.CircularParentCheck(parent, child))
		{
			return;
		}
		if (child.Parent != null)
		{
			this.RemoveParent(child);
		}
		if (parent == null)
		{
			return;
		}
		if (child != null && this.Circuits.Contains(child))
		{
			this.Circuits.Remove(child);
		}
		parent.Children.Add(child);
		child.Parent = parent;
		child.SendHasLocalChangesToRoot();
	}

	// Token: 0x060042ED RID: 17133 RVA: 0x001A4628 File Offset: 0x001A2828
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CircularParentCheck(PowerItem Parent, PowerItem Child)
	{
		return Parent == Child || (Parent != null && Parent.Parent != null && this.CircularParentCheck(Parent.Parent, Child));
	}

	// Token: 0x060042EE RID: 17134 RVA: 0x001A464C File Offset: 0x001A284C
	public void RemoveParent(PowerItem node)
	{
		if (node.Parent != null)
		{
			PowerItem parent = node.Parent;
			node.Parent.Children.Remove(node);
			if (node.Parent.TileEntity != null)
			{
				node.Parent.TileEntity.CreateWireDataFromPowerItem();
				node.Parent.TileEntity.DrawWires();
			}
			node.Parent = null;
			this.Circuits.Add(node);
			parent.SendHasLocalChangesToRoot();
			node.HandleDisconnect();
		}
	}

	// Token: 0x060042EF RID: 17135 RVA: 0x001A46C4 File Offset: 0x001A28C4
	public void RemoveChild(PowerItem child)
	{
		child.Parent.Children.Remove(child);
		child.Parent = null;
		this.Circuits.Add(child);
	}

	// Token: 0x060042F0 RID: 17136 RVA: 0x001A46EC File Offset: 0x001A28EC
	public void SetParent(Vector3i childPos, Vector3i parentPos)
	{
		PowerItem powerItemByWorldPos = this.GetPowerItemByWorldPos(parentPos);
		PowerItem powerItemByWorldPos2 = this.GetPowerItemByWorldPos(childPos);
		this.SetParent(powerItemByWorldPos2, powerItemByWorldPos);
	}

	// Token: 0x060042F1 RID: 17137 RVA: 0x001A4711 File Offset: 0x001A2911
	public PowerItem GetPowerItemByWorldPos(Vector3i position)
	{
		if (this.PowerItemDictionary.ContainsKey(position))
		{
			return this.PowerItemDictionary[position];
		}
		return null;
	}

	// Token: 0x060042F2 RID: 17138 RVA: 0x001A4730 File Offset: 0x001A2930
	public void LogPowerManager()
	{
		for (int i = 0; i < this.PowerSources.Count; i++)
		{
			this.LogChildren(this.PowerSources[i]);
		}
	}

	// Token: 0x060042F3 RID: 17139 RVA: 0x001A4768 File Offset: 0x001A2968
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogChildren(PowerItem item)
	{
		try
		{
			Log.Out(string.Format("{0}{1}({2}) - Pos:{3} | Powered:{4}", new object[]
			{
				new string('\t', (int)((item.Depth > 100) ? 0 : (item.Depth + 1))),
				item.ToString(),
				item.Depth,
				item.Position,
				item.IsPowered
			}));
			for (int i = 0; i < item.Children.Count; i++)
			{
				this.LogChildren(item.Children[i]);
			}
		}
		catch (Exception ex)
		{
			Log.Exception(ex);
		}
	}

	// Token: 0x04003656 RID: 13910
	[PublicizedFrom(EAccessModifier.Private)]
	public const float UPDATE_TIME_SEC = 0.16f;

	// Token: 0x04003657 RID: 13911
	[PublicizedFrom(EAccessModifier.Private)]
	public const float SAVE_TIME_SEC = 120f;

	// Token: 0x04003658 RID: 13912
	public static byte FileVersion = 2;

	// Token: 0x0400365A RID: 13914
	[PublicizedFrom(EAccessModifier.Private)]
	public static PowerManager instance;

	// Token: 0x0400365B RID: 13915
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PowerItem> Circuits;

	// Token: 0x0400365C RID: 13916
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PowerSource> PowerSources;

	// Token: 0x0400365D RID: 13917
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PowerTrigger> PowerTriggers;

	// Token: 0x0400365E RID: 13918
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Vector3i, PowerItem> PowerItemDictionary = new Dictionary<Vector3i, PowerItem>();

	// Token: 0x0400365F RID: 13919
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;

	// Token: 0x04003660 RID: 13920
	[PublicizedFrom(EAccessModifier.Private)]
	public float saveTime = 120f;

	// Token: 0x04003661 RID: 13921
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo dataSaveThreadInfo;

	// Token: 0x04003662 RID: 13922
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<TileEntityPoweredBlock> ClientUpdateList = new HashSet<TileEntityPoweredBlock>();
}
