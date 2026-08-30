using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000813 RID: 2067
[Preserve]
public class NetPackageNavObject : NetPackage
{
	// Token: 0x06003CDF RID: 15583 RVA: 0x00183C7D File Offset: 0x00181E7D
	public NetPackageNavObject Setup(string _navObjectClass, string _displayName, Vector3 _position, bool _isAdd, bool _usingLocalizationId, int _entityId = -1)
	{
		this.navObjectClass = _navObjectClass;
		this.name = _displayName;
		this.position = _position;
		this.isAdd = _isAdd;
		this.usingLocalizationId = _usingLocalizationId;
		this.entityId = _entityId;
		return this;
	}

	// Token: 0x06003CE0 RID: 15584 RVA: 0x00183CAD File Offset: 0x00181EAD
	public NetPackageNavObject Setup(int _entityId)
	{
		this.navObjectClass = "";
		this.name = "";
		this.position = Vector3.zero;
		this.isAdd = false;
		this.usingLocalizationId = false;
		this.entityId = _entityId;
		return this;
	}

	// Token: 0x06003CE1 RID: 15585 RVA: 0x00183CE6 File Offset: 0x00181EE6
	public NetPackageNavObject Setup(string _navObjectClass, string _displayName, Vector3 _position, bool _isAdd, Color _overrideColor, bool _usingLocalizationId)
	{
		this.navObjectClass = _navObjectClass;
		this.name = _displayName;
		this.position = _position;
		this.isAdd = _isAdd;
		this.usingLocalizationId = _usingLocalizationId;
		this.useOverrideColor = true;
		this.overrideColor = _overrideColor;
		return this;
	}

	// Token: 0x06003CE2 RID: 15586 RVA: 0x00183D20 File Offset: 0x00181F20
	public override void read(PooledBinaryReader _br)
	{
		this.navObjectClass = _br.ReadString();
		this.name = _br.ReadString();
		this.position = StreamUtils.ReadVector3(_br);
		this.isAdd = _br.ReadBoolean();
		this.useOverrideColor = _br.ReadBoolean();
		this.overrideColor = StreamUtils.ReadColor32(_br);
		this.usingLocalizationId = _br.ReadBoolean();
		this.entityId = _br.ReadInt32();
	}

	// Token: 0x06003CE3 RID: 15587 RVA: 0x00183D90 File Offset: 0x00181F90
	public override void write(PooledBinaryWriter _bw)
	{
		base.write(_bw);
		_bw.Write(this.navObjectClass);
		_bw.Write(this.name ?? "");
		StreamUtils.Write(_bw, this.position);
		_bw.Write(this.isAdd);
		_bw.Write(this.useOverrideColor);
		StreamUtils.WriteColor32(_bw, this.overrideColor);
		_bw.Write(this.usingLocalizationId);
		_bw.Write(this.entityId);
	}

	// Token: 0x06003CE4 RID: 15588 RVA: 0x00183E10 File Offset: 0x00182010
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (_world == null)
		{
			return;
		}
		if (_world.IsRemote())
		{
			if (this.isAdd)
			{
				NavObject navObject = ((this.entityId == -1) ? null : NavObjectManager.Instance.GetNavObjectByEntityID(this.entityId));
				if (navObject != null && navObject.TrackType == NavObject.TrackTypes.Entity)
				{
					return;
				}
				NavObject navObject2 = NavObjectManager.Instance.RegisterNavObject(this.navObjectClass, this.position, "", false, -1, null);
				navObject2.name = this.name;
				navObject2.usingLocalizationId = this.usingLocalizationId;
				navObject2.EntityID = this.entityId;
				if (this.useOverrideColor)
				{
					navObject2.UseOverrideColor = true;
					navObject2.OverrideColor = this.overrideColor;
					return;
				}
			}
			else
			{
				if (this.entityId != -1)
				{
					NavObjectManager.Instance.UnRegisterNavObjectByEntityID(this.entityId);
					return;
				}
				NavObjectManager.Instance.UnRegisterNavObjectByPosition(this.position, this.navObjectClass);
			}
		}
	}

	// Token: 0x1700065A RID: 1626
	// (get) Token: 0x06003CE5 RID: 15589 RVA: 0x0002F168 File Offset: 0x0002D368
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003CE6 RID: 15590 RVA: 0x00105CC1 File Offset: 0x00103EC1
	public override int GetLength()
	{
		return 30;
	}

	// Token: 0x0400315F RID: 12639
	[PublicizedFrom(EAccessModifier.Private)]
	public string navObjectClass;

	// Token: 0x04003160 RID: 12640
	[PublicizedFrom(EAccessModifier.Private)]
	public string name;

	// Token: 0x04003161 RID: 12641
	[PublicizedFrom(EAccessModifier.Private)]
	public bool usingLocalizationId;

	// Token: 0x04003162 RID: 12642
	[PublicizedFrom(EAccessModifier.Private)]
	public Vector3 position;

	// Token: 0x04003163 RID: 12643
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isAdd;

	// Token: 0x04003164 RID: 12644
	[PublicizedFrom(EAccessModifier.Private)]
	public int entityId;

	// Token: 0x04003165 RID: 12645
	[PublicizedFrom(EAccessModifier.Private)]
	public bool useOverrideColor;

	// Token: 0x04003166 RID: 12646
	[PublicizedFrom(EAccessModifier.Private)]
	public Color overrideColor;
}
