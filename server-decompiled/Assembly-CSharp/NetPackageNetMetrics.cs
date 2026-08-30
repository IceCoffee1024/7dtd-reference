using System;
using UnityEngine.Scripting;

// Token: 0x02000809 RID: 2057
[Preserve]
public class NetPackageNetMetrics : NetPackage
{
	// Token: 0x06003C89 RID: 15497 RVA: 0x00181B18 File Offset: 0x0017FD18
	public static NetPackageNetMetrics SetupClient(string content, string csv)
	{
		NetPackageNetMetrics package = NetPackageManager.GetPackage<NetPackageNetMetrics>();
		package.content = content;
		package.csv = csv;
		return package;
	}

	// Token: 0x06003C8A RID: 15498 RVA: 0x00181B2D File Offset: 0x0017FD2D
	public static NetPackageNetMetrics SetupServer(bool enable, float duration, bool loop)
	{
		NetPackageNetMetrics package = NetPackageManager.GetPackage<NetPackageNetMetrics>();
		package.enable = enable;
		package.duration = duration;
		package.loop = loop;
		return package;
	}

	// Token: 0x06003C8B RID: 15499 RVA: 0x00181B4C File Offset: 0x0017FD4C
	public override void read(PooledBinaryReader _reader)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			this.content = _reader.ReadString();
			this.csv = _reader.ReadString();
			return;
		}
		this.enable = _reader.ReadBoolean();
		this.duration = _reader.ReadSingle();
		this.loop = _reader.ReadBoolean();
	}

	// Token: 0x06003C8C RID: 15500 RVA: 0x00181BA4 File Offset: 0x0017FDA4
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			_writer.Write(this.enable);
			_writer.Write(this.duration);
			_writer.Write(this.loop);
			return;
		}
		_writer.Write(this.content);
		_writer.Write(this.csv);
	}

	// Token: 0x06003C8D RID: 15501 RVA: 0x00181C04 File Offset: 0x0017FE04
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			Log.Out("RECEIVED STATS BACK");
			Log.Out(this.content);
			Log.Out(this.csv);
			GameManager.Instance.netpackageMetrics.AppendClientCSV(this.csv);
			return;
		}
		Log.Out("REQUESTED TO RECORD STATS");
		if (this.enable)
		{
			NetPackageMetrics.Instance.RecordForPeriod(this.duration);
		}
	}

	// Token: 0x06003C8E RID: 15502 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x040030E8 RID: 12520
	public bool enable;

	// Token: 0x040030E9 RID: 12521
	public float duration;

	// Token: 0x040030EA RID: 12522
	public bool loop;

	// Token: 0x040030EB RID: 12523
	public string content;

	// Token: 0x040030EC RID: 12524
	public string csv;
}
