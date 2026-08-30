using System;
using System.Collections;
using UnityEngine.Scripting;

// Token: 0x020007EF RID: 2031
[Preserve]
public class NetPackageGameStats : NetPackage
{
	// Token: 0x06003BD7 RID: 15319 RVA: 0x0017F96C File Offset: 0x0017DB6C
	public NetPackageGameStats Setup(GameStats _gs)
	{
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(this.ms);
			_gs.Write(pooledBinaryWriter);
		}
		return this;
	}

	// Token: 0x06003BD8 RID: 15320 RVA: 0x0017F9B8 File Offset: 0x0017DBB8
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageGameStats()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003BD9 RID: 15321 RVA: 0x0017F9F0 File Offset: 0x0017DBF0
	public override void read(PooledBinaryReader _reader)
	{
		int num = (int)_reader.ReadInt16();
		StreamUtils.StreamCopy(_reader.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003BDA RID: 15322 RVA: 0x0017FA18 File Offset: 0x0017DC18
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((short)this.ms.Length);
		this.ms.WriteTo(_writer.BaseStream);
	}

	// Token: 0x06003BDB RID: 15323 RVA: 0x0017FA44 File Offset: 0x0017DC44
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		ThreadManager.StartCoroutine(this.readStatsCo());
	}

	// Token: 0x06003BDC RID: 15324 RVA: 0x0017FA52 File Offset: 0x0017DC52
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

	// Token: 0x06003BDD RID: 15325 RVA: 0x0017FA61 File Offset: 0x0017DC61
	public override int GetLength()
	{
		return (int)this.ms.Length;
	}

	// Token: 0x17000635 RID: 1589
	// (get) Token: 0x06003BDE RID: 15326 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003089 RID: 12425
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);
}
