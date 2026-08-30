using System;

// Token: 0x02000C59 RID: 3161
public static class TileEntityExtensions
{
	// Token: 0x06006075 RID: 24693 RVA: 0x00261B48 File Offset: 0x0025FD48
	public static T GetSelfOrFeature<T>(this ITileEntity _te) where T : class
	{
		T t;
		_te.TryGetSelfOrFeature<T>(out t);
		return t;
	}

	// Token: 0x06006076 RID: 24694 RVA: 0x00261B60 File Offset: 0x0025FD60
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
