using System;

namespace PrefabVolumes
{
	// Token: 0x020018BC RID: 6332
	public abstract class PrefabVolumeAbs<T> : PrefabVolumeAbs where T : PrefabVolumeAbs<T>, new()
	{
		// Token: 0x0600C372 RID: 50034 RVA: 0x004836C5 File Offset: 0x004818C5
		public override PrefabVolumeAbs Clone()
		{
			return this.CloneGeneric();
		}

		// Token: 0x0600C373 RID: 50035 RVA: 0x004836D4 File Offset: 0x004818D4
		public virtual T CloneGeneric()
		{
			T t = new T();
			this.CopyValues(t, false);
			return t;
		}

		// Token: 0x0600C374 RID: 50036 RVA: 0x004836F8 File Offset: 0x004818F8
		public override void CopyValues(PrefabVolumeAbs _target, bool _nonBasicOnly = false)
		{
			T t = _target as T;
			if (t == null)
			{
				throw new ArgumentException("Can not copy values to volume of type " + _target.VolumeType.ToStringCached<PrefabVolumeAbs.EVolumeType>() + " from volume of type " + this.VolumeType.ToStringCached<PrefabVolumeAbs.EVolumeType>(), "_target");
			}
			this.CopyValues(t, _nonBasicOnly);
		}

		// Token: 0x0600C375 RID: 50037 RVA: 0x00483756 File Offset: 0x00481956
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void CopyValues(PrefabVolumeAbs<T> _target, bool _nonBasicOnly = false)
		{
			_target.used = base.Used;
			_target.startPos = this.startPos;
			_target.size = this.size;
		}

		// Token: 0x0600C376 RID: 50038 RVA: 0x0048377C File Offset: 0x0048197C
		[PublicizedFrom(EAccessModifier.Protected)]
		public PrefabVolumeAbs()
		{
		}
	}
}
