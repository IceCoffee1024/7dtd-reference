using System;

// Token: 0x02000C6B RID: 3179
public static class TileEntityExtensions
{
	// Token: 0x06006100 RID: 24832 RVA: 0x00264A20 File Offset: 0x00262C20
	public static T GetSelfOrFeature<T>(this ITileEntity _te) where T : class
	{
		T t;
		_te.TryGetSelfOrFeature<T>(out t);
		return t;
	}

	// Token: 0x06006101 RID: 24833 RVA: 0x00264A38 File Offset: 0x00262C38
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
