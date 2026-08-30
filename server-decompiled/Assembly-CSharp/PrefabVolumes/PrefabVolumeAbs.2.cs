using System;

namespace PrefabVolumes
{
	// Token: 0x02001896 RID: 6294
	public abstract class PrefabVolumeAbs<T> : PrefabVolumeAbs where T : PrefabVolumeAbs<T>, new()
	{
		// Token: 0x0600C1EB RID: 49643 RVA: 0x0047D599 File Offset: 0x0047B799
		public override PrefabVolumeAbs Clone()
		{
			return this.CloneGeneric();
		}

		// Token: 0x0600C1EC RID: 49644 RVA: 0x0047D5A8 File Offset: 0x0047B7A8
		public virtual T CloneGeneric()
		{
			T t = new T();
			this.CopyValues(t, false);
			return t;
		}

		// Token: 0x0600C1ED RID: 49645 RVA: 0x0047D5CC File Offset: 0x0047B7CC
		public override void CopyValues(PrefabVolumeAbs _target, bool _nonBasicOnly = false)
		{
			T t = _target as T;
			if (t == null)
			{
				throw new ArgumentException("Can not copy values to volume of type " + _target.VolumeType.ToStringCached<PrefabVolumeAbs.EVolumeType>() + " from volume of type " + this.VolumeType.ToStringCached<PrefabVolumeAbs.EVolumeType>(), "_target");
			}
			this.CopyValues(t, _nonBasicOnly);
		}

		// Token: 0x0600C1EE RID: 49646 RVA: 0x0047D62A File Offset: 0x0047B82A
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void CopyValues(PrefabVolumeAbs<T> _target, bool _nonBasicOnly = false)
		{
			_target.used = base.Used;
			_target.startPos = this.startPos;
			_target.size = this.size;
		}

		// Token: 0x0600C1EF RID: 49647 RVA: 0x0047D650 File Offset: 0x0047B850
		[PublicizedFrom(EAccessModifier.Protected)]
		public PrefabVolumeAbs()
		{
		}
	}
}
