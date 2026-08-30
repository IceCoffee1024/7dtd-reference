using System;
using System.Collections;
using System.Collections.Generic;
using RaycastPathing;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020008E3 RID: 2275
[Preserve]
public class RaycastEntityPathGenerator
{
	// Token: 0x170006E4 RID: 1764
	// (get) Token: 0x060041ED RID: 16877 RVA: 0x0019C6E9 File Offset: 0x0019A8E9
	// (set) Token: 0x060041EE RID: 16878 RVA: 0x0019C6F1 File Offset: 0x0019A8F1
	public World GameWorld
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006E5 RID: 1765
	// (get) Token: 0x060041EF RID: 16879 RVA: 0x0019C6FA File Offset: 0x0019A8FA
	// (set) Token: 0x060041F0 RID: 16880 RVA: 0x0019C702 File Offset: 0x0019A902
	public EntityAlive Entity
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006E6 RID: 1766
	// (get) Token: 0x060041F1 RID: 16881 RVA: 0x0019C70B File Offset: 0x0019A90B
	// (set) Token: 0x060041F2 RID: 16882 RVA: 0x0019C713 File Offset: 0x0019A913
	public virtual RaycastPath Path
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x170006E7 RID: 1767
	// (get) Token: 0x060041F3 RID: 16883 RVA: 0x0019C71C File Offset: 0x0019A91C
	// (set) Token: 0x060041F4 RID: 16884 RVA: 0x0019C724 File Offset: 0x0019A924
	public bool isBuildingPath
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006E8 RID: 1768
	// (get) Token: 0x060041F5 RID: 16885 RVA: 0x0019C72D File Offset: 0x0019A92D
	// (set) Token: 0x060041F6 RID: 16886 RVA: 0x0019C735 File Offset: 0x0019A935
	public bool isPathReady
	{
		get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x060041F7 RID: 16887 RVA: 0x0019C73E File Offset: 0x0019A93E
	public RaycastEntityPathGenerator(World _world, EntityAlive _entity)
	{
		this.GameWorld = _world;
		this.Entity = _entity;
	}

	// Token: 0x060041F8 RID: 16888 RVA: 0x0019C754 File Offset: 0x0019A954
	public Vector3[] pathToArray()
	{
		Vector3[] array = new Vector3[this.Path.Nodes.Count - 1];
		for (int i = 0; i < this.Path.Nodes.Count; i++)
		{
			array[i] = this.Path.Nodes[i].Position;
		}
		return array;
	}

	// Token: 0x060041F9 RID: 16889 RVA: 0x0019C7B4 File Offset: 0x0019A9B4
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

	// Token: 0x060041FA RID: 16890 RVA: 0x0019C805 File Offset: 0x0019AA05
	public void CreatePath(Vector3 start, Vector3 end, float speed, bool canBreakBlocks, float yHeightOffset = 0f)
	{
		this.cleanupPath();
		this.InitPath(start, end);
		this.beginPathProc();
	}

	// Token: 0x060041FB RID: 16891 RVA: 0x0019C81B File Offset: 0x0019AA1B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InitPath(Vector3 start, Vector3 end)
	{
		this.Path = new RaycastPath(start, end);
	}

	// Token: 0x060041FC RID: 16892 RVA: 0x0019C82A File Offset: 0x0019AA2A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual IEnumerator BuildPathProc()
	{
		this.finalizePathProc();
		yield break;
	}

	// Token: 0x060041FD RID: 16893 RVA: 0x0019C839 File Offset: 0x0019AA39
	[PublicizedFrom(EAccessModifier.Private)]
	public void beginPathProc()
	{
		this.isBuildingPath = true;
		this.StartCoroutine(this.BuildPathProc());
	}

	// Token: 0x060041FE RID: 16894 RVA: 0x0019C84E File Offset: 0x0019AA4E
	[PublicizedFrom(EAccessModifier.Private)]
	public void abortPathProc()
	{
		this.StopCoroutine(this.BuildPathProc());
		this.isBuildingPath = false;
	}

	// Token: 0x060041FF RID: 16895 RVA: 0x0019C863 File Offset: 0x0019AA63
	[PublicizedFrom(EAccessModifier.Protected)]
	public void finalizePathProc()
	{
		this.isBuildingPath = false;
		this.isPathReady = true;
	}

	// Token: 0x06004200 RID: 16896 RVA: 0x0019C873 File Offset: 0x0019AA73
	public void Clear()
	{
		this.cleanupPath();
	}

	// Token: 0x06004201 RID: 16897 RVA: 0x0019C87B File Offset: 0x0019AA7B
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

	// Token: 0x06004202 RID: 16898 RVA: 0x0019C8A4 File Offset: 0x0019AAA4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void StartCoroutine(IEnumerator task)
	{
		GameManager.Instance.StartCoroutine(task);
	}

	// Token: 0x06004203 RID: 16899 RVA: 0x0019C8B2 File Offset: 0x0019AAB2
	[PublicizedFrom(EAccessModifier.Protected)]
	public void StopCoroutine(IEnumerator task)
	{
		GameManager.Instance.StopCoroutine(task);
	}

	// Token: 0x06004204 RID: 16900 RVA: 0x0019C8BF File Offset: 0x0019AABF
	public bool IsConfinedSpace(Vector3 pos, float size, bool debugDraw = false)
	{
		return RaycastPathWorldUtils.IsConfinedSpace(this.GameWorld, pos, size, debugDraw);
	}
}
