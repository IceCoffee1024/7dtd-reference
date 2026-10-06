using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x02000914 RID: 2324
public class PowerManager
{
	// Token: 0x1700073A RID: 1850
	// (get) Token: 0x060042B9 RID: 17081 RVA: 0x001A2E56 File Offset: 0x001A1056
	// (set) Token: 0x060042BA RID: 17082 RVA: 0x001A2E5E File Offset: 0x001A105E
	public byte CurrentFileVersion { get; set; }

	// Token: 0x1700073B RID: 1851
	// (get) Token: 0x060042BB RID: 17083 RVA: 0x001A2E67 File Offset: 0x001A1067
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

	// Token: 0x1700073C RID: 1852
	// (get) Token: 0x060042BC RID: 17084 RVA: 0x001A2E7F File Offset: 0x001A107F
	public static bool HasInstance
	{
		get
		{
			return PowerManager.instance != null;
		}
	}

	// Token: 0x060042BD RID: 17085 RVA: 0x001A2E8C File Offset: 0x001A108C
	[PublicizedFrom(EAccessModifier.Private)]
	public PowerManager()
	{
		PowerManager.instance = this;
		this.Circuits = new List<PowerItem>();
		this.PowerSources = new List<PowerSource>();
		this.PowerTriggers = new List<PowerTrigger>();
	}

	// Token: 0x060042BE RID: 17086 RVA: 0x001A2EE7 File Offset: 0x001A10E7
	public void SetTileEntityUpdate(TileEntityPoweredBlock tileEntity, bool shouldUpdate)
	{
		if (shouldUpdate)
		{
			this.ClientUpdateList.Add(tileEntity);
			return;
		}
		this.ClientUpdateList.Remove(tileEntity);
	}

	// Token: 0x060042BF RID: 17087 RVA: 0x001A2F08 File Offset: 0x001A1108
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

	// Token: 0x060042C0 RID: 17088 RVA: 0x001A306C File Offset: 0x001A126C
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

	// Token: 0x060042C1 RID: 17089 RVA: 0x001A30D8 File Offset: 0x001A12D8
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

	// Token: 0x060042C2 RID: 17090 RVA: 0x001A31E0 File Offset: 0x001A13E0
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

	// Token: 0x060042C3 RID: 17091 RVA: 0x001A326C File Offset: 0x001A146C
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

	// Token: 0x060042C4 RID: 17092 RVA: 0x001A32D8 File Offset: 0x001A14D8
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

	// Token: 0x060042C5 RID: 17093 RVA: 0x001A3330 File Offset: 0x001A1530
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

	// Token: 0x060042C6 RID: 17094 RVA: 0x001A337C File Offset: 0x001A157C
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

	// Token: 0x060042C7 RID: 17095 RVA: 0x001A33E4 File Offset: 0x001A15E4
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

	// Token: 0x060042C8 RID: 17096 RVA: 0x001A34AC File Offset: 0x001A16AC
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

	// Token: 0x060042C9 RID: 17097 RVA: 0x001A3510 File Offset: 0x001A1710
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

	// Token: 0x060042CA RID: 17098 RVA: 0x001A3580 File Offset: 0x001A1780
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CircularParentCheck(PowerItem Parent, PowerItem Child)
	{
		return Parent == Child || (Parent != null && Parent.Parent != null && this.CircularParentCheck(Parent.Parent, Child));
	}

	// Token: 0x060042CB RID: 17099 RVA: 0x001A35A4 File Offset: 0x001A17A4
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

	// Token: 0x060042CC RID: 17100 RVA: 0x001A361C File Offset: 0x001A181C
	public void RemoveChild(PowerItem child)
	{
		child.Parent.Children.Remove(child);
		child.Parent = null;
		this.Circuits.Add(child);
	}

	// Token: 0x060042CD RID: 17101 RVA: 0x001A3644 File Offset: 0x001A1844
	public void SetParent(Vector3i childPos, Vector3i parentPos)
	{
		PowerItem powerItemByWorldPos = this.GetPowerItemByWorldPos(parentPos);
		PowerItem powerItemByWorldPos2 = this.GetPowerItemByWorldPos(childPos);
		this.SetParent(powerItemByWorldPos2, powerItemByWorldPos);
	}

	// Token: 0x060042CE RID: 17102 RVA: 0x001A3669 File Offset: 0x001A1869
	public PowerItem GetPowerItemByWorldPos(Vector3i position)
	{
		if (this.PowerItemDictionary.ContainsKey(position))
		{
			return this.PowerItemDictionary[position];
		}
		return null;
	}

	// Token: 0x060042CF RID: 17103 RVA: 0x001A3688 File Offset: 0x001A1888
	public void LogPowerManager()
	{
		for (int i = 0; i < this.PowerSources.Count; i++)
		{
			this.LogChildren(this.PowerSources[i]);
		}
	}

	// Token: 0x060042D0 RID: 17104 RVA: 0x001A36C0 File Offset: 0x001A18C0
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

	// Token: 0x04003636 RID: 13878
	[PublicizedFrom(EAccessModifier.Private)]
	public const float UPDATE_TIME_SEC = 0.16f;

	// Token: 0x04003637 RID: 13879
	[PublicizedFrom(EAccessModifier.Private)]
	public const float SAVE_TIME_SEC = 120f;

	// Token: 0x04003638 RID: 13880
	public static byte FileVersion = 2;

	// Token: 0x0400363A RID: 13882
	[PublicizedFrom(EAccessModifier.Private)]
	public static PowerManager instance;

	// Token: 0x0400363B RID: 13883
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PowerItem> Circuits;

	// Token: 0x0400363C RID: 13884
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PowerSource> PowerSources;

	// Token: 0x0400363D RID: 13885
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PowerTrigger> PowerTriggers;

	// Token: 0x0400363E RID: 13886
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Vector3i, PowerItem> PowerItemDictionary = new Dictionary<Vector3i, PowerItem>();

	// Token: 0x0400363F RID: 13887
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;

	// Token: 0x04003640 RID: 13888
	[PublicizedFrom(EAccessModifier.Private)]
	public float saveTime = 120f;

	// Token: 0x04003641 RID: 13889
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo dataSaveThreadInfo;

	// Token: 0x04003642 RID: 13890
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<TileEntityPoweredBlock> ClientUpdateList = new HashSet<TileEntityPoweredBlock>();
}
