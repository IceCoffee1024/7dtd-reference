using System;
using System.Collections;
using System.Collections.Generic;
using RaycastPathing;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020008D7 RID: 2263
[Preserve]
public class RaycastEntityPathGenerator
{
	// Token: 0x170006D8 RID: 1752
	// (get) Token: 0x06004188 RID: 16776 RVA: 0x0019A611 File Offset: 0x00198811
	// (set) Token: 0x06004189 RID: 16777 RVA: 0x0019A619 File Offset: 0x00198819
	public World GameWorld
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006D9 RID: 1753
	// (get) Token: 0x0600418A RID: 16778 RVA: 0x0019A622 File Offset: 0x00198822
	// (set) Token: 0x0600418B RID: 16779 RVA: 0x0019A62A File Offset: 0x0019882A
	public EntityAlive Entity
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006DA RID: 1754
	// (get) Token: 0x0600418C RID: 16780 RVA: 0x0019A633 File Offset: 0x00198833
	// (set) Token: 0x0600418D RID: 16781 RVA: 0x0019A63B File Offset: 0x0019883B
	public virtual RaycastPath Path
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x170006DB RID: 1755
	// (get) Token: 0x0600418E RID: 16782 RVA: 0x0019A644 File Offset: 0x00198844
	// (set) Token: 0x0600418F RID: 16783 RVA: 0x0019A64C File Offset: 0x0019884C
	public bool isBuildingPath
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006DC RID: 1756
	// (get) Token: 0x06004190 RID: 16784 RVA: 0x0019A655 File Offset: 0x00198855
	// (set) Token: 0x06004191 RID: 16785 RVA: 0x0019A65D File Offset: 0x0019885D
	public bool isPathReady
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06004192 RID: 16786 RVA: 0x0019A666 File Offset: 0x00198866
	public RaycastEntityPathGenerator(World _world, EntityAlive _entity)
	{
		this.GameWorld = _world;
		this.Entity = _entity;
	}

	// Token: 0x06004193 RID: 16787 RVA: 0x0019A67C File Offset: 0x0019887C
	public Vector3[] pathToArray()
	{
		Vector3[] array = new Vector3[this.Path.Nodes.Count - 1];
		for (int i = 0; i < this.Path.Nodes.Count; i++)
		{
			array[i] = this.Path.Nodes[i].Position;
		}
		return array;
	}

	// Token: 0x06004194 RID: 16788 RVA: 0x0019A6DC File Offset: 0x001988DC
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

	// Token: 0x06004195 RID: 16789 RVA: 0x0019A72D File Offset: 0x0019892D
	public void CreatePath(Vector3 start, Vector3 end, float speed, bool canBreakBlocks, float yHeightOffset = 0f)
	{
		this.cleanupPath();
		this.InitPath(start, end);
		this.beginPathProc();
	}

	// Token: 0x06004196 RID: 16790 RVA: 0x0019A743 File Offset: 0x00198943
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InitPath(Vector3 start, Vector3 end)
	{
		this.Path = new RaycastPath(start, end);
	}

	// Token: 0x06004197 RID: 16791 RVA: 0x0019A752 File Offset: 0x00198952
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual IEnumerator BuildPathProc()
	{
		this.finalizePathProc();
		yield break;
	}

	// Token: 0x06004198 RID: 16792 RVA: 0x0019A761 File Offset: 0x00198961
	[PublicizedFrom(EAccessModifier.Private)]
	public void beginPathProc()
	{
		this.isBuildingPath = true;
		this.StartCoroutine(this.BuildPathProc());
	}

	// Token: 0x06004199 RID: 16793 RVA: 0x0019A776 File Offset: 0x00198976
	[PublicizedFrom(EAccessModifier.Private)]
	public void abortPathProc()
	{
		this.StopCoroutine(this.BuildPathProc());
		this.isBuildingPath = false;
	}

	// Token: 0x0600419A RID: 16794 RVA: 0x0019A78B File Offset: 0x0019898B
	[PublicizedFrom(EAccessModifier.Protected)]
	public void finalizePathProc()
	{
		this.isBuildingPath = false;
		this.isPathReady = true;
	}

	// Token: 0x0600419B RID: 16795 RVA: 0x0019A79B File Offset: 0x0019899B
	public void Clear()
	{
		this.cleanupPath();
	}

	// Token: 0x0600419C RID: 16796 RVA: 0x0019A7A3 File Offset: 0x001989A3
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

	// Token: 0x0600419D RID: 16797 RVA: 0x0019A7CC File Offset: 0x001989CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public void StartCoroutine(IEnumerator task)
	{
		GameManager.Instance.StartCoroutine(task);
	}

	// Token: 0x0600419E RID: 16798 RVA: 0x0019A7DA File Offset: 0x001989DA
	[PublicizedFrom(EAccessModifier.Protected)]
	public void StopCoroutine(IEnumerator task)
	{
		GameManager.Instance.StopCoroutine(task);
	}

	// Token: 0x0600419F RID: 16799 RVA: 0x0019A7E7 File Offset: 0x001989E7
	public bool IsConfinedSpace(Vector3 pos, float size, bool debugDraw = false)
	{
		return RaycastPathWorldUtils.IsConfinedSpace(this.GameWorld, pos, size, debugDraw);
	}
}
