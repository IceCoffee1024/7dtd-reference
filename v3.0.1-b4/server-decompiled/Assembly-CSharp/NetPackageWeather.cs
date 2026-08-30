using System;
using UnityEngine.Scripting;

// Token: 0x02001360 RID: 4960
[Preserve]
public class NetPackageWeather : NetPackage
{
	// Token: 0x1700125F RID: 4703
	// (get) Token: 0x06009C51 RID: 40017 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06009C52 RID: 40018 RVA: 0x003B1778 File Offset: 0x003AF978
	public NetPackageWeather Setup(WeatherPackage[] _packages)
	{
		this.weatherPackages = _packages;
		return this;
	}

	// Token: 0x06009C53 RID: 40019 RVA: 0x003B1784 File Offset: 0x003AF984
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitPackages()
	{
		if (WeatherManager.Instance)
		{
			int count = WeatherManager.Instance.biomeWeather.Count;
			this.weatherPackages = new WeatherPackage[count];
			for (int i = 0; i < count; i++)
			{
				this.weatherPackages[i] = new WeatherPackage();
			}
		}
	}

	// Token: 0x06009C54 RID: 40020 RVA: 0x003B17D4 File Offset: 0x003AF9D4
	public override void read(PooledBinaryReader _br)
	{
		if (this.weatherPackages == null)
		{
			this.InitPackages();
			if (this.weatherPackages == null)
			{
				return;
			}
		}
		for (int i = 0; i < this.weatherPackages.Length; i++)
		{
			WeatherPackage weatherPackage = this.weatherPackages[i];
			weatherPackage.biomeId = _br.ReadByte();
			weatherPackage.groupIndex = _br.ReadByte();
			weatherPackage.remainingSeconds = _br.ReadByte();
			for (int j = 0; j < weatherPackage.param.Length; j++)
			{
				weatherPackage.param[j] = _br.ReadSingle();
			}
		}
	}

	// Token: 0x06009C55 RID: 40021 RVA: 0x003B185C File Offset: 0x003AFA5C
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		for (int i = 0; i < this.weatherPackages.Length; i++)
		{
			WeatherPackage weatherPackage = this.weatherPackages[i];
			_bw.Write(weatherPackage.biomeId);
			_bw.Write(weatherPackage.groupIndex);
			_bw.Write(weatherPackage.remainingSeconds);
			for (int j = 0; j < weatherPackage.param.Length; j++)
			{
				_bw.Write(weatherPackage.param[j]);
			}
		}
	}

	// Token: 0x06009C56 RID: 40022 RVA: 0x000027FC File Offset: 0x000009FC
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
	}

	// Token: 0x06009C57 RID: 40023 RVA: 0x00010E62 File Offset: 0x0000F062
	public override int GetLength()
	{
		return 0;
	}

	// Token: 0x0400766B RID: 30315
	[PublicizedFrom(EAccessModifier.Private)]
	public WeatherPackage[] weatherPackages;
}
