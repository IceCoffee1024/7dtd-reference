using System;
using UnityEngine.Scripting;

// Token: 0x02000812 RID: 2066
[Preserve]
public class NetPackageNetMetrics : NetPackage
{
	// Token: 0x06003CB1 RID: 15537 RVA: 0x00183ACC File Offset: 0x00181CCC
	public static NetPackageNetMetrics SetupClient(string content, string csv)
	{
		NetPackageNetMetrics package = NetPackageManager.GetPackage<NetPackageNetMetrics>();
		package.content = content;
		package.csv = csv;
		return package;
	}

	// Token: 0x06003CB2 RID: 15538 RVA: 0x00183AE1 File Offset: 0x00181CE1
	public static NetPackageNetMetrics SetupServer(bool enable, float duration, bool loop)
	{
		NetPackageNetMetrics package = NetPackageManager.GetPackage<NetPackageNetMetrics>();
		package.enable = enable;
		package.duration = duration;
		package.loop = loop;
		return package;
	}

	// Token: 0x06003CB3 RID: 15539 RVA: 0x00183B00 File Offset: 0x00181D00
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

	// Token: 0x06003CB4 RID: 15540 RVA: 0x00183B58 File Offset: 0x00181D58
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

	// Token: 0x06003CB5 RID: 15541 RVA: 0x00183BB8 File Offset: 0x00181DB8
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

	// Token: 0x0400314B RID: 12619
	public bool enable;

	// Token: 0x0400314C RID: 12620
	public float duration;

	// Token: 0x0400314D RID: 12621
	public bool loop;

	// Token: 0x0400314E RID: 12622
	public string content;

	// Token: 0x0400314F RID: 12623
	public string csv;
}
