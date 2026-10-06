using System;

// Token: 0x02000C7A RID: 3194
public static class TileEntityExtensions
{
	// Token: 0x060060FD RID: 24829 RVA: 0x00267E94 File Offset: 0x00266094
	public static T GetSelfOrFeature<T>(this ITileEntity _te) where T : class
	{
		T t;
		_te.TryGetSelfOrFeature<T>(out t);
		return t;
	}

	// Token: 0x060060FE RID: 24830 RVA: 0x00267EAC File Offset: 0x002660AC
	public static bool TryGetSelfOrFeature<T>(this ITileEntity _te, out T _typedTe) where T : class
	{
		if (_te == null)
		{
			_typedTe = default(T);
			return false;
		}
		T t = _te as T;
		if (t != null)
		{
			_typedTe = t;
			return true;
		}
		TileEntityComposite tileEntityComposite = _te as TileEntityComposite;
		if (tileEntityComposite != null)
		{
			_typedTe = tileEntityComposite.GetFeature<T>();
			return _typedTe != null;
		}
		ITileEntityFeature tileEntityFeature = _te as ITileEntityFeature;
		if (tileEntityFeature != null)
		{
			_typedTe = tileEntityFeature.Parent.GetFeature<T>();
			return _typedTe != null;
		}
		_typedTe = default(T);
		return false;
	}
}
