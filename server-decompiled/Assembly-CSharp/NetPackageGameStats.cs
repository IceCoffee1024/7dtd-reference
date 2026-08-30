using System;
using System.Collections;
using UnityEngine.Scripting;

// Token: 0x020007FA RID: 2042
[Preserve]
public class NetPackageGameStats : NetPackage
{
	// Token: 0x06003C36 RID: 15414 RVA: 0x00181D44 File Offset: 0x0017FF44
	public NetPackageGameStats Setup(GameStats _gs)
	{
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(this.ms);
			_gs.Write(pooledBinaryWriter);
		}
		return this;
	}

	// Token: 0x06003C37 RID: 15415 RVA: 0x00181D90 File Offset: 0x0017FF90
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~NetPackageGameStats()
	{
		MemoryPools.poolMemoryStream.FreeSync(this.ms);
	}

	// Token: 0x06003C38 RID: 15416 RVA: 0x00181DC8 File Offset: 0x0017FFC8
	public override void read(PooledBinaryReader _reader)
	{
		int num = (int)_reader.ReadInt16();
		StreamUtils.StreamCopy(_reader.BaseStream, this.ms, num, null, true);
	}

	// Token: 0x06003C39 RID: 15417 RVA: 0x00181DF0 File Offset: 0x0017FFF0
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write((short)this.ms.Length);
		this.ms.WriteTo(_writer.BaseStream);
	}

	// Token: 0x06003C3A RID: 15418 RVA: 0x00181E1C File Offset: 0x0018001C
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		ThreadManager.StartCoroutine(this.readStatsCo());
	}

	// Token: 0x06003C3B RID: 15419 RVA: 0x00181E2A File Offset: 0x0018002A
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

	// Token: 0x06003C3C RID: 15420 RVA: 0x00181E39 File Offset: 0x00180039
	public override int GetLength()
	{
		return (int)this.ms.Length;
	}

	// Token: 0x17000640 RID: 1600
	// (get) Token: 0x06003C3D RID: 15421 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x04003108 RID: 12552
	[PublicizedFrom(EAccessModifier.Private)]
	public PooledExpandableMemoryStream ms = MemoryPools.poolMemoryStream.AllocSync(true);
}
