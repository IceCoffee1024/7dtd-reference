using System;
using System.Collections;
using System.Collections.Generic;
using RaycastPathing;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020008E8 RID: 2280
[Preserve]
public class RaycastEntityPathGenerator
{
	// Token: 0x17000703 RID: 1795
	// (get) Token: 0x06004183 RID: 16771 RVA: 0x0019C7FD File Offset: 0x0019A9FD
	// (set) Token: 0x06004184 RID: 16772 RVA: 0x0019C805 File Offset: 0x0019AA05
	public World GameWorld
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000704 RID: 1796
	// (get) Token: 0x06004185 RID: 16773 RVA: 0x0019C80E File Offset: 0x0019AA0E
	// (set) Token: 0x06004186 RID: 16774 RVA: 0x0019C816 File Offset: 0x0019AA16
	public EntityAlive Entity
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000705 RID: 1797
	// (get) Token: 0x06004187 RID: 16775 RVA: 0x0019C81F File Offset: 0x0019AA1F
	// (set) Token: 0x06004188 RID: 16776 RVA: 0x0019C827 File Offset: 0x0019AA27
	public virtual RaycastPath Path
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x17000706 RID: 1798
	// (get) Token: 0x06004189 RID: 16777 RVA: 0x0019C830 File Offset: 0x0019AA30
	// (set) Token: 0x0600418A RID: 16778 RVA: 0x0019C838 File Offset: 0x0019AA38
	public bool isBuildingPath
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x17000707 RID: 1799
	// (get) Token: 0x0600418B RID: 16779 RVA: 0x0019C841 File Offset: 0x0019AA41
	// (set) Token: 0x0600418C RID: 16780 RVA: 0x0019C849 File Offset: 0x0019AA49
	public bool isPathReady
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600418D RID: 16781 RVA: 0x0019C852 File Offset: 0x0019AA52
	public RaycastEntityPathGenerator(World _world, EntityAlive _entity)
	{
		this.GameWorld = _world;
		this.Entity = _entity;
	}

	// Token: 0x0600418E RID: 16782 RVA: 0x0019C868 File Offset: 0x0019AA68
	public Vector3[] pathToArray()
	{
		Vector3[] array = new Vector3[this.Path.Nodes.Count - 1];
		for (int i = 0; i < this.Path.Nodes.Count; i++)
		{
			array[i] = this.Path.Nodes[i].Position;
		}
		return array;
	}

	// Token: 0x0600418F RID: 16783 RVA: 0x0019C8C8 File Offset: 0x0019AAC8
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

	// Token: 0x06004190 RID: 16784 RVA: 0x0019C919 File Offset: 0x0019AB19
	public void CreatePath(Vector3 start, Vector3 end, float speed, bool canBreakBlocks, float yHeightOffset = 0f)
	{
		this.cleanupPath();
		this.InitPath(start, end);
		this.beginPathProc();
	}

	// Token: 0x06004191 RID: 16785 RVA: 0x0019C92F File Offset: 0x0019AB2F
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InitPath(Vector3 start, Vector3 end)
	{
		this.Path = new RaycastPath(start, end);
	}

	// Token: 0x06004192 RID: 16786 RVA: 0x0019C93E File Offset: 0x0019AB3E
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual IEnumerator BuildPathProc()
	{
		this.finalizePathProc();
		yield break;
	}

	// Token: 0x06004193 RID: 16787 RVA: 0x0019C94D File Offset: 0x0019AB4D
	[PublicizedFrom(EAccessModifier.Private)]
	public void beginPathProc()
	{
		this.isBuildingPath = true;
		this.StartCoroutine(this.BuildPathProc());
	}

	// Token: 0x06004194 RID: 16788 RVA: 0x0019C962 File Offset: 0x0019AB62
	[PublicizedFrom(EAccessModifier.Private)]
	public void abortPathProc()
	{
		this.StopCoroutine(this.BuildPathProc());
		this.isBuildingPath = false;
	}

	// Token: 0x06004195 RID: 16789 RVA: 0x0019C977 File Offset: 0x0019AB77
	[PublicizedFrom(EAccessModifier.Protected)]
	public void finalizePathProc()
	{
		this.isBuildingPath = false;
		this.isPathReady = true;
	}

	// Token: 0x06004196 RID: 16790 RVA: 0x0019C987 File Offset: 0x0019AB87
	public void Clear()
	{
		this.cleanupPath();
	}

	// Token: 0x06004197 RID: 16791 RVA: 0x0019C98F File Offset: 0x0019AB8F
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

	// Token: 0x06004198 RID: 16792 RVA: 0x0019C9B8 File Offset: 0x0019ABB8
	[PublicizedFrom(EAccessModifier.Protected)]
	public void StartCoroutine(IEnumerator task)
	{
		GameManager.Instance.StartCoroutine(task);
	}

	// Token: 0x06004199 RID: 16793 RVA: 0x0019C9C6 File Offset: 0x0019ABC6
	[PublicizedFrom(EAccessModifier.Protected)]
	public void StopCoroutine(IEnumerator task)
	{
		GameManager.Instance.StopCoroutine(task);
	}

	// Token: 0x0600419A RID: 16794 RVA: 0x0019C9D3 File Offset: 0x0019ABD3
	public bool IsConfinedSpace(Vector3 pos, float size, bool debugDraw = false)
	{
		return RaycastPathWorldUtils.IsConfinedSpace(this.GameWorld, pos, size, debugDraw);
	}
}
