using System;
using System.Collections;
using UnityEngine.Scripting;

// Token: 0x02000807 RID: 2055
[Preserve]
public class NetPackageGameStats : NetPackage
{
	// Token: 0x06003C56 RID: 15446 RVA: 0x0018335C File Offset: 0x0018155C
	public NetPackageGameStats Setup(GameStats _gs)
	{
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(this.ms);
			_gs.Write(pooledBinaryWriter);
		}
		return this;
	}

	// Token: 0x06003C57 RID: 15447 RVA: 0x001833A8 File Offset: 0x001815A8
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageGameStats()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003C58 RID: 15448 RVA: 0x001833E0 File Offset: 0x001815E0
	public override void read(PooledBinaryReader _reader)
	{
		int num = (int)_reader.ReadInt16();
		StreamUtils.StreamCopy(_reader.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003C59 RID: 15449 RVA: 0x00183408 File Offset: 0x00181608
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((short)this.ms.Length);
		this.ms.WriteTo(_writer.BaseStream);
	}

	// Token: 0x06003C5A RID: 15450 RVA: 0x00183434 File Offset: 0x00181634
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		ThreadManager.StartCoroutine(this.readStatsCo());
	}

	// Token: 0x06003C5B RID: 15451 RVA: 0x00183442 File Offset: 0x00181642
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator readStatsCo()
	{
		while (GameManager.Instance.World == null && SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
		{
			yield return null;
		}
		if (GameManager.Instance.World == null)
		{
			yield break;
		}
		using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
		{
			PooledExpandableMemoryStream pooledExpandableMemoryStream = this.ms;
			lock (pooledExpandableMemoryStream)
			{
				pooledBinaryReader.SetBaseStream(this.ms);
				this.ms.Position = 0L;
				GameStats.Instance.Read(pooledBinaryReader);
			}
		}
		GameManager.Instance.GetGameStateManager().OnUpdateTick();
		yield break;
	}

	// Token: 0x1700066B RID: 1643
	// (get) Token: 0x06003C5C RID: 15452 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003121 RID: 12577
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);
}
