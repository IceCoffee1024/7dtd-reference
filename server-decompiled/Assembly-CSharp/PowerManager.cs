using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Token: 0x0200090F RID: 2319
public class PowerManager
{
	// Token: 0x1700071B RID: 1819
	// (get) Token: 0x06004327 RID: 17191 RVA: 0x001A2D52 File Offset: 0x001A0F52
	// (set) Token: 0x06004328 RID: 17192 RVA: 0x001A2D5A File Offset: 0x001A0F5A
	public byte CurrentFileVersion { get; set; }

	// Token: 0x1700071C RID: 1820
	// (get) Token: 0x06004329 RID: 17193 RVA: 0x001A2D63 File Offset: 0x001A0F63
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

	// Token: 0x1700071D RID: 1821
	// (get) Token: 0x0600432A RID: 17194 RVA: 0x001A2D7B File Offset: 0x001A0F7B
	public static bool HasInstance
	{
		get
		{
			return PowerManager.instance != null;
		}
	}

	// Token: 0x0600432B RID: 17195 RVA: 0x001A2D88 File Offset: 0x001A0F88
	[PublicizedFrom(EAccessModifier.Private)]
	public PowerManager()
	{
		PowerManager.instance = this;
		this.Circuits = new List<PowerItem>();
		this.PowerSources = new List<PowerSource>();
		this.PowerTriggers = new List<PowerTrigger>();
	}

	// Token: 0x0600432C RID: 17196 RVA: 0x001A2DE3 File Offset: 0x001A0FE3
	public void SetTileEntityUpdate(TileEntityPoweredBlock tileEntity, bool shouldUpdate)
	{
		if (shouldUpdate)
		{
			this.ClientUpdateList.Add(tileEntity);
			return;
		}
		this.ClientUpdateList.Remove(tileEntity);
	}

	// Token: 0x0600432D RID: 17197 RVA: 0x001A2E04 File Offset: 0x001A1004
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

	// Token: 0x0600432E RID: 17198 RVA: 0x001A2F68 File Offset: 0x001A1168
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

	// Token: 0x0600432F RID: 17199 RVA: 0x001A2FD4 File Offset: 0x001A11D4
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

	// Token: 0x06004330 RID: 17200 RVA: 0x001A30DC File Offset: 0x001A12DC
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

	// Token: 0x06004331 RID: 17201 RVA: 0x001A3168 File Offset: 0x001A1368
	public void Write(BinaryWriter bw)
	{
		bw.Write(PowerManager.FileVersion);
		bw.Write(this.Circuits.Count);
		for (int i = 0; i < this.Circuits.Count; i++)
		{
			bw.Write((byte)this.Circuits[i].PowerItemType);
			this.Circuits[i].write(bw);
		}
	}

	// Token: 0x06004332 RID: 17202 RVA: 0x001A31D4 File Offset: 0x001A13D4
	public void Read(BinaryReader br)
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

	// Token: 0x06004333 RID: 17203 RVA: 0x001A322C File Offset: 0x001A142C
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

	// Token: 0x06004334 RID: 17204 RVA: 0x001A3278 File Offset: 0x001A1478
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

	// Token: 0x06004335 RID: 17205 RVA: 0x001A32E0 File Offset: 0x001A14E0
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

	// Token: 0x06004336 RID: 17206 RVA: 0x001A33A8 File Offset: 0x001A15A8
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

	// Token: 0x06004337 RID: 17207 RVA: 0x001A340C File Offset: 0x001A160C
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

	// Token: 0x06004338 RID: 17208 RVA: 0x001A347C File Offset: 0x001A167C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool CircularParentCheck(PowerItem Parent, PowerItem Child)
	{
		return Parent == Child || (Parent != null && Parent.Parent != null && this.CircularParentCheck(Parent.Parent, Child));
	}

	// Token: 0x06004339 RID: 17209 RVA: 0x001A34A0 File Offset: 0x001A16A0
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

	// Token: 0x0600433A RID: 17210 RVA: 0x001A3518 File Offset: 0x001A1718
	public void RemoveChild(PowerItem child)
	{
		child.Parent.Children.Remove(child);
		child.Parent = null;
		this.Circuits.Add(child);
	}

	// Token: 0x0600433B RID: 17211 RVA: 0x001A3540 File Offset: 0x001A1740
	public void SetParent(Vector3i childPos, Vector3i parentPos)
	{
		PowerItem powerItemByWorldPos = this.GetPowerItemByWorldPos(parentPos);
		PowerItem powerItemByWorldPos2 = this.GetPowerItemByWorldPos(childPos);
		this.SetParent(powerItemByWorldPos2, powerItemByWorldPos);
	}

	// Token: 0x0600433C RID: 17212 RVA: 0x001A3565 File Offset: 0x001A1765
	public PowerItem GetPowerItemByWorldPos(Vector3i position)
	{
		if (this.PowerItemDictionary.ContainsKey(position))
		{
			return this.PowerItemDictionary[position];
		}
		return null;
	}

	// Token: 0x0600433D RID: 17213 RVA: 0x001A3584 File Offset: 0x001A1784
	public void LogPowerManager()
	{
		for (int i = 0; i < this.PowerSources.Count; i++)
		{
			this.LogChildren(this.PowerSources[i]);
		}
	}

	// Token: 0x0600433E RID: 17214 RVA: 0x001A35BC File Offset: 0x001A17BC
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

	// Token: 0x04003642 RID: 13890
	[PublicizedFrom(EAccessModifier.Private)]
	public const float UPDATE_TIME_SEC = 0.16f;

	// Token: 0x04003643 RID: 13891
	[PublicizedFrom(EAccessModifier.Private)]
	public const float SAVE_TIME_SEC = 120f;

	// Token: 0x04003644 RID: 13892
	public static byte FileVersion = 2;

	// Token: 0x04003646 RID: 13894
	[PublicizedFrom(EAccessModifier.Private)]
	public static PowerManager instance;

	// Token: 0x04003647 RID: 13895
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PowerItem> Circuits;

	// Token: 0x04003648 RID: 13896
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PowerSource> PowerSources;

	// Token: 0x04003649 RID: 13897
	[PublicizedFrom(EAccessModifier.Private)]
	public List<PowerTrigger> PowerTriggers;

	// Token: 0x0400364A RID: 13898
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<Vector3i, PowerItem> PowerItemDictionary = new Dictionary<Vector3i, PowerItem>();

	// Token: 0x0400364B RID: 13899
	[PublicizedFrom(EAccessModifier.Private)]
	public float updateTime;

	// Token: 0x0400364C RID: 13900
	[PublicizedFrom(EAccessModifier.Private)]
	public float saveTime = 120f;

	// Token: 0x0400364D RID: 13901
	[PublicizedFrom(EAccessModifier.Private)]
	public ThreadManager.ThreadInfo dataSaveThreadInfo;

	// Token: 0x0400364E RID: 13902
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<TileEntityPoweredBlock> ClientUpdateList = new HashSet<TileEntityPoweredBlock>();
}
