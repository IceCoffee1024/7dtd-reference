using System;
using UnityEngine.Scripting;

// Token: 0x02000814 RID: 2068
[Preserve]
public class NetPackageNetMetrics : NetPackage
{
	// Token: 0x06003CE8 RID: 15592 RVA: 0x00183EF0 File Offset: 0x001820F0
	public static NetPackageNetMetrics SetupClient(string content, string csv)
	{
		NetPackageNetMetrics package = NetPackageManager.GetPackage<NetPackageNetMetrics>();
		package.content = content;
		package.csv = csv;
		return package;
	}

	// Token: 0x06003CE9 RID: 15593 RVA: 0x00183F05 File Offset: 0x00182105
	public static NetPackageNetMetrics SetupServer(bool enable, float duration, bool loop)
	{
		NetPackageNetMetrics package = NetPackageManager.GetPackage<NetPackageNetMetrics>();
		package.enable = enable;
		package.duration = duration;
		package.loop = loop;
		return package;
	}

	// Token: 0x06003CEA RID: 15594 RVA: 0x00183F24 File Offset: 0x00182124
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

	// Token: 0x06003CEB RID: 15595 RVA: 0x00183F7C File Offset: 0x0018217C
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

	// Token: 0x06003CEC RID: 15596 RVA: 0x00183FDC File Offset: 0x001821DC
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

	// Token: 0x06003CED RID: 15597 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x04003167 RID: 12647
	public bool enable;

	// Token: 0x04003168 RID: 12648
	public float duration;

	// Token: 0x04003169 RID: 12649
	public bool loop;

	// Token: 0x0400316A RID: 12650
	public string content;

	// Token: 0x0400316B RID: 12651
	public string csv;
}
