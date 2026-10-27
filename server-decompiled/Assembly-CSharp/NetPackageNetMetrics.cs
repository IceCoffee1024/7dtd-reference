using System;
using UnityEngine.Scripting;

// Token: 0x0200081C RID: 2076
[Preserve]
public class NetPackageNetMetrics : NetPackage
{
	// Token: 0x06003CD4 RID: 15572 RVA: 0x00184BD0 File Offset: 0x00182DD0
	public static NetPackageNetMetrics SetupClient(string content, string csv)
	{
		NetPackageNetMetrics package = NetPackageManager.GetPackage<NetPackageNetMetrics>();
		package.content = content;
		package.csv = csv;
		return package;
	}

	// Token: 0x06003CD5 RID: 15573 RVA: 0x00184BE5 File Offset: 0x00182DE5
	public static NetPackageNetMetrics SetupServer(bool enable, float duration, bool loop)
	{
		NetPackageNetMetrics package = NetPackageManager.GetPackage<NetPackageNetMetrics>();
		package.enable = enable;
		package.duration = duration;
		package.loop = loop;
		return package;
	}

	// Token: 0x06003CD6 RID: 15574 RVA: 0x00184C04 File Offset: 0x00182E04
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

	// Token: 0x06003CD7 RID: 15575 RVA: 0x00184C5C File Offset: 0x00182E5C
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

	// Token: 0x06003CD8 RID: 15576 RVA: 0x00184CBC File Offset: 0x00182EBC
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

	// Token: 0x0400316B RID: 12651
	public bool enable;

	// Token: 0x0400316C RID: 12652
	public float duration;

	// Token: 0x0400316D RID: 12653
	public bool loop;

	// Token: 0x0400316E RID: 12654
	public string content;

	// Token: 0x0400316F RID: 12655
	public string csv;
}
