using System;
using System.Collections;
using UnityEngine.Scripting;

// Token: 0x020007FD RID: 2045
[Preserve]
public class NetPackageGameStats : NetPackage
{
	// Token: 0x06003C33 RID: 15411 RVA: 0x00182258 File Offset: 0x00180458
	public NetPackageGameStats Setup(GameStats _gs)
	{
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(this.ms);
			_gs.Write(pooledBinaryWriter);
		}
		return this;
	}

	// Token: 0x06003C34 RID: 15412 RVA: 0x001822A4 File Offset: 0x001804A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageGameStats()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003C35 RID: 15413 RVA: 0x001822DC File Offset: 0x001804DC
	public override void read(PooledBinaryReader _reader)
	{
		int num = (int)_reader.ReadInt16();
		StreamUtils.StreamCopy(_reader.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003C36 RID: 15414 RVA: 0x00182304 File Offset: 0x00180504
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((short)this.ms.Length);
		this.ms.WriteTo(_writer.BaseStream);
	}

	// Token: 0x06003C37 RID: 15415 RVA: 0x00182330 File Offset: 0x00180530
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		ThreadManager.StartCoroutine(this.readStatsCo());
	}

	// Token: 0x06003C38 RID: 15416 RVA: 0x0018233E File Offset: 0x0018053E
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

	// Token: 0x17000665 RID: 1637
	// (get) Token: 0x06003C39 RID: 15417 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003101 RID: 12545
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);
}
