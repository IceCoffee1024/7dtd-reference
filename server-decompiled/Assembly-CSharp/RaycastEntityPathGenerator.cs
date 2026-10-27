using System;
using System.Collections;
using System.Collections.Generic;
using RaycastPathing;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020008F2 RID: 2290
[Preserve]
public class RaycastEntityPathGenerator
{
	// Token: 0x17000709 RID: 1801
	// (get) Token: 0x060041A6 RID: 16806 RVA: 0x0019D8A5 File Offset: 0x0019BAA5
	// (set) Token: 0x060041A7 RID: 16807 RVA: 0x0019D8AD File Offset: 0x0019BAAD
	public World GameWorld
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700070A RID: 1802
	// (get) Token: 0x060041A8 RID: 16808 RVA: 0x0019D8B6 File Offset: 0x0019BAB6
	// (set) Token: 0x060041A9 RID: 16809 RVA: 0x0019D8BE File Offset: 0x0019BABE
	public EntityAlive Entity
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700070B RID: 1803
	// (get) Token: 0x060041AA RID: 16810 RVA: 0x0019D8C7 File Offset: 0x0019BAC7
	// (set) Token: 0x060041AB RID: 16811 RVA: 0x0019D8CF File Offset: 0x0019BACF
	public virtual RaycastPath Path
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x1700070C RID: 1804
	// (get) Token: 0x060041AC RID: 16812 RVA: 0x0019D8D8 File Offset: 0x0019BAD8
	// (set) Token: 0x060041AD RID: 16813 RVA: 0x0019D8E0 File Offset: 0x0019BAE0
	public bool isBuildingPath
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700070D RID: 1805
	// (get) Token: 0x060041AE RID: 16814 RVA: 0x0019D8E9 File Offset: 0x0019BAE9
	// (set) Token: 0x060041AF RID: 16815 RVA: 0x0019D8F1 File Offset: 0x0019BAF1
	public bool isPathReady
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060041B0 RID: 16816 RVA: 0x0019D8FA File Offset: 0x0019BAFA
	public RaycastEntityPathGenerator(World _world, EntityAlive _entity)
	{
		this.GameWorld = _world;
		this.Entity = _entity;
	}

	// Token: 0x060041B1 RID: 16817 RVA: 0x0019D910 File Offset: 0x0019BB10
	public Vector3[] pathToArray()
	{
		Vector3[] array = new Vector3[this.Path.Nodes.Count - 1];
		for (int i = 0; i < this.Path.Nodes.Count; i++)
		{
			array[i] = this.Path.Nodes[i].Position;
		}
		return array;
	}

	// Token: 0x060041B2 RID: 16818 RVA: 0x0019D970 File Offset: 0x0019BB70
	public List<Vector3> pathToList()
	{
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < this.Path.Nodes.Count; i++)
		{
			list.Add(this.Path.Nodes[i].Position);
		}
		list.Reverse();
		return list;
	}

	// Token: 0x060041B3 RID: 16819 RVA: 0x0019D9C1 File Offset: 0x0019BBC1
	public void CreatePath(Vector3 start, Vector3 end, float speed, bool canBreakBlocks, float yHeightOffset = 0f)
	{
		this.cleanupPath();
		this.InitPath(start, end);
		this.beginPathProc();
	}

	// Token: 0x060041B4 RID: 16820 RVA: 0x0019D9D7 File Offset: 0x0019BBD7
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InitPath(Vector3 start, Vector3 end)
	{
		this.Path = new RaycastPath(start, end);
	}

	// Token: 0x060041B5 RID: 16821 RVA: 0x0019D9E6 File Offset: 0x0019BBE6
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual IEnumerator BuildPathProc()
	{
		this.finalizePathProc();
		yield break;
	}

	// Token: 0x060041B6 RID: 16822 RVA: 0x0019D9F5 File Offset: 0x0019BBF5
	[PublicizedFrom(EAccessModifier.Private)]
	public void beginPathProc()
	{
		this.isBuildingPath = true;
		this.StartCoroutine(this.BuildPathProc());
	}

	// Token: 0x060041B7 RID: 16823 RVA: 0x0019DA0A File Offset: 0x0019BC0A
	[PublicizedFrom(EAccessModifier.Private)]
	public void abortPathProc()
	{
		this.StopCoroutine(this.BuildPathProc());
		this.isBuildingPath = false;
	}

	// Token: 0x060041B8 RID: 16824 RVA: 0x0019DA1F File Offset: 0x0019BC1F
	[PublicizedFrom(EAccessModifier.Protected)]
	public void finalizePathProc()
	{
		this.isBuildingPath = false;
		this.isPathReady = true;
	}

	// Token: 0x060041B9 RID: 16825 RVA: 0x0019DA2F File Offset: 0x0019BC2F
	public void Clear()
	{
		this.cleanupPath();
	}

	// Token: 0x060041BA RID: 16826 RVA: 0x0019DA37 File Offset: 0x0019BC37
	[PublicizedFrom(EAccessModifier.Private)]
	public void cleanupPath()
	{
		this.isPathReady = false;
		if (this.Path != null)
		{
			this.abortPathProc();
			this.Path.Destruct();
			this.Path = null;
		}
	}

	// Token: 0x060041BB RID: 16827 RVA: 0x0019DA60 File Offset: 0x0019BC60
	[PublicizedFrom(EAccessModifier.Protected)]
	public void StartCoroutine(IEnumerator task)
	{
		GameManager.Instance.StartCoroutine(task);
	}

	// Token: 0x060041BC RID: 16828 RVA: 0x0019DA6E File Offset: 0x0019BC6E
	[PublicizedFrom(EAccessModifier.Protected)]
	public void StopCoroutine(IEnumerator task)
	{
		GameManager.Instance.StopCoroutine(task);
	}

	// Token: 0x060041BD RID: 16829 RVA: 0x0019DA7B File Offset: 0x0019BC7B
	public bool IsConfinedSpace(Vector3 pos, float size, bool debugDraw = false)
	{
		return RaycastPathWorldUtils.IsConfinedSpace(this.GameWorld, pos, size, debugDraw);
	}
}
