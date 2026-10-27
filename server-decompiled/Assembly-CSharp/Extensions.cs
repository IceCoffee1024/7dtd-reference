using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x0200142F RID: 5167
public static class Extensions
{
	// Token: 0x0600A190 RID: 41360 RVA: 0x003CBD00 File Offset: 0x003C9F00
	public static Transform FindInChildren(this Transform _t, string _name)
	{
		int childCount = _t.childCount;
		if (childCount == 0)
		{
			return null;
		}
		Transform transform = _t.Find(_name);
		if (!transform)
		{
			for (int i = 0; i < childCount; i++)
			{
				transform = _t.GetChild(i).FindInChildren(_name);
				if (transform)
				{
					break;
				}
			}
		}
		return transform;
	}

	// Token: 0x0600A191 RID: 41361 RVA: 0x003CBD4C File Offset: 0x003C9F4C
	public static Transform FindTagInChildren(this Transform _t, string _tag)
	{
		if (!_t)
		{
			return null;
		}
		if (_t.CompareTag(_tag))
		{
			return _t;
		}
		int childCount = _t.childCount;
		if (childCount == 0)
		{
			return null;
		}
		for (int i = 0; i < childCount; i++)
		{
			Transform transform = _t.GetChild(i).FindTagInChildren(_tag);
			if (transform)
			{
				return transform;
			}
		}
		return null;
	}

	// Token: 0x0600A192 RID: 41362 RVA: 0x003CBDA0 File Offset: 0x003C9FA0
	public static Transform FindInChilds(this Transform target, string name, bool onlyActive = false)
	{
		if (!target || name == null)
		{
			return null;
		}
		if (onlyActive && (!target.gameObject || !target.gameObject.activeSelf))
		{
			return null;
		}
		if (target.name == name)
		{
			return target;
		}
		for (int i = 0; i < target.childCount; i++)
		{
			Transform transform = target.GetChild(i).FindInChilds(name, onlyActive);
			if (transform != null)
			{
				return transform;
			}
		}
		return null;
	}

	// Token: 0x0600A193 RID: 41363 RVA: 0x003CBE15 File Offset: 0x003CA015
	public static T GetComponentInChildren<T>(this GameObject o, bool searchInactive, bool avoidGC = false) where T : Component
	{
		return o.transform.GetComponentInChildren<T>(searchInactive, avoidGC);
	}

	// Token: 0x0600A194 RID: 41364 RVA: 0x003CBE24 File Offset: 0x003CA024
	public static T GetComponentInChildren<T>(this Component c, bool searchInactive, bool avoidGC = false) where T : Component
	{
		return c.transform.GetComponentInChildren<T>(searchInactive, avoidGC);
	}

	// Token: 0x0600A195 RID: 41365 RVA: 0x003CBE34 File Offset: 0x003CA034
	public static T GetComponentInChildren<T>(this Transform t, bool searchInactive, bool avoidGC = false) where T : Component
	{
		if (!searchInactive)
		{
			return t.GetComponentInChildren<T>();
		}
		if (avoidGC)
		{
			T t2 = t.GetComponent<T>();
			if (t2 == null)
			{
				for (int i = 0; i < t.childCount; i++)
				{
					t2 = t.GetChild(i).GetComponentInChildren<T>(searchInactive, avoidGC);
					if (t2 != null)
					{
						break;
					}
				}
			}
			return t2;
		}
		T[] componentsInChildren = t.GetComponentsInChildren<T>(true);
		if (componentsInChildren.Length == 0)
		{
			return default(T);
		}
		return componentsInChildren[0];
	}

	// Token: 0x0600A196 RID: 41366 RVA: 0x003CBEB0 File Offset: 0x003CA0B0
	public static T GetOrAddComponent<T>(this GameObject go) where T : Component
	{
		T t = go.GetComponent<T>();
		if (t == null)
		{
			t = go.AddComponent<T>();
		}
		return t;
	}

	// Token: 0x0600A197 RID: 41367 RVA: 0x003CBEDC File Offset: 0x003CA0DC
	public static string GetGameObjectPath(this GameObject _obj)
	{
		string text = "/" + _obj.name;
		while (_obj.transform.parent)
		{
			_obj = _obj.transform.parent.gameObject;
			text = "/" + _obj.name + text;
		}
		return text;
	}

	// Token: 0x0600A198 RID: 41368 RVA: 0x003CBF34 File Offset: 0x003CA134
	public static bool ContainsWithComparer<T>(this List<T> _list, T _item, IEqualityComparer<T> _comparer)
	{
		if (_list == null)
		{
			throw new ArgumentNullException("_list");
		}
		if (_comparer == null)
		{
			_comparer = EqualityComparer<T>.Default;
		}
		for (int i = 0; i < _list.Count; i++)
		{
			if (_comparer.Equals(_list[i], _item))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600A199 RID: 41369 RVA: 0x003CBF80 File Offset: 0x003CA180
	public static bool ContainsCaseInsensitive(this IList<string> _list, string _item)
	{
		if (_item == null)
		{
			for (int i = 0; i < _list.Count; i++)
			{
				if (_list[i] == null)
				{
					return true;
				}
			}
			return false;
		}
		for (int j = 0; j < _list.Count; j++)
		{
			if (StringComparer.OrdinalIgnoreCase.Equals(_list[j], _item))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600A19A RID: 41370 RVA: 0x003CBFD8 File Offset: 0x003CA1D8
	public static void CopyTo<T>(this IList<T> _srcList, IList<T> _dest)
	{
		foreach (T t in _srcList)
		{
			_dest.Add(t);
		}
	}

	// Token: 0x0600A19B RID: 41371 RVA: 0x003CC020 File Offset: 0x003CA220
	public static void Shuffle<T>(this List<T> _list)
	{
		_list.Shuffle<T>(GameManager.Instance.World.GetGameRandom());
	}

	// Token: 0x0600A19C RID: 41372 RVA: 0x003CC038 File Offset: 0x003CA238
	public static void Shuffle<T>(this List<T> _list, int _seed)
	{
		GameRandom gameRandom = GameRandomManager.Instance.CreateGameRandom(_seed);
		_list.Shuffle<T>(gameRandom);
		GameRandomManager.Instance.FreeGameRandom(gameRandom);
	}

	// Token: 0x0600A19D RID: 41373 RVA: 0x003CC064 File Offset: 0x003CA264
	public static void Shuffle<T>(this List<T> _list, GameRandom _rand)
	{
		int i = _list.Count;
		while (i > 1)
		{
			int num = _rand.RandomRange(0, i--);
			int num2 = num;
			int num3 = i;
			T t = _list[i];
			T t2 = _list[num];
			_list[num2] = t;
			_list[num3] = t2;
		}
	}

	// Token: 0x0600A19E RID: 41374 RVA: 0x003CC0BD File Offset: 0x003CA2BD
	public static bool ColorEquals(this Color32 _a, Color32 _b)
	{
		return _a.r == _b.r && _a.g == _b.g && _a.b == _b.b && _a.a == _b.a;
	}

	// Token: 0x0600A19F RID: 41375 RVA: 0x003CC0F9 File Offset: 0x003CA2F9
	public static string ToHexCode(this Color _color, bool _includeAlpha = false)
	{
		return _color.ToHexCode(_includeAlpha);
	}

	// Token: 0x0600A1A0 RID: 41376 RVA: 0x003CC107 File Offset: 0x003CA307
	public static Color WithAlpha(this Color _color, float _alpha)
	{
		return new Color(_color.r, _color.g, _color.b, _alpha);
	}

	// Token: 0x0600A1A1 RID: 41377 RVA: 0x003CC124 File Offset: 0x003CA324
	public static string ToHexCode(this Color32 _color, bool _includeAlpha = false)
	{
		if (!_includeAlpha)
		{
			return string.Format("{0:X02}{1:X02}{2:X02}", _color.r, _color.g, _color.b);
		}
		return string.Format("{0:X02}{1:X02}{2:X02}{3:X02}", new object[] { _color.r, _color.g, _color.b, _color.a });
	}

	// Token: 0x0600A1A2 RID: 41378 RVA: 0x003CC1A8 File Offset: 0x003CA3A8
	public unsafe static void WriteToBuffer(this Guid _value, byte[] _dest, int _offset)
	{
		if (_dest.Length - _offset < 16)
		{
			throw new ArgumentException("buffer too small");
		}
		fixed (byte[] array = _dest)
		{
			byte* ptr;
			if (_dest == null || array.Length == 0)
			{
				ptr = null;
			}
			else
			{
				ptr = &array[0];
			}
			long* ptr2 = (long*)(&_value);
			long* ptr3 = (long*)(ptr + _offset);
			*ptr3 = *ptr2;
			ptr3[1] = ptr2[1];
		}
	}

	// Token: 0x0600A1A3 RID: 41379 RVA: 0x003CC1F7 File Offset: 0x003CA3F7
	public static Bounds Offset(this Bounds _bounds, Vector3 _offset)
	{
		return new Bounds(_bounds.center + _offset, _bounds.size);
	}

	// Token: 0x0600A1A4 RID: 41380 RVA: 0x003CC214 File Offset: 0x003CA414
	public static bool ContainsInclusive(this BoundsInt bounds, Vector3Int position)
	{
		return position.x >= bounds.min.x && position.y >= bounds.min.y && position.z >= bounds.min.z && position.x <= bounds.max.x && position.y <= bounds.max.y && position.z <= bounds.max.z;
	}

	// Token: 0x0600A1A5 RID: 41381 RVA: 0x003CC2B8 File Offset: 0x003CA4B8
	public static BoundsInt InitFromPoints(IEnumerable<Vector3i> points)
	{
		Vector3i vector3i = new Vector3i(int.MaxValue, int.MaxValue, int.MaxValue);
		Vector3i vector3i2 = new Vector3i(int.MinValue, int.MinValue, int.MinValue);
		foreach (Vector3i vector3i3 in points)
		{
			vector3i = Vector3i.Min(vector3i, vector3i3);
			vector3i2 = Vector3i.Max(vector3i2, vector3i3);
		}
		BoundsInt boundsInt = default(BoundsInt);
		boundsInt.SetMinMax(vector3i, vector3i2);
		return boundsInt;
	}

	// Token: 0x0600A1A6 RID: 41382 RVA: 0x003CC354 File Offset: 0x003CA554
	public static void CalculatePersistableHash(this Bounds _bounds, IncrementalHash calcHash)
	{
		calcHash.AppendDataNoAlloc(_bounds.center.x);
		calcHash.AppendDataNoAlloc(_bounds.center.y);
		calcHash.AppendDataNoAlloc(_bounds.center.z);
		calcHash.AppendDataNoAlloc(_bounds.size.x);
		calcHash.AppendDataNoAlloc(_bounds.size.y);
		calcHash.AppendDataNoAlloc(_bounds.size.z);
	}

	// Token: 0x0600A1A7 RID: 41383 RVA: 0x000063DF File Offset: 0x000045DF
	public static bool EqualsCaseInsensitive(this string _a, string _b)
	{
		return string.Equals(_a, _b, StringComparison.OrdinalIgnoreCase);
	}

	// Token: 0x0600A1A8 RID: 41384 RVA: 0x003CC3CD File Offset: 0x003CA5CD
	public static bool ContainsCaseInsensitive(this string _a, string _b)
	{
		return _a.IndexOf(_b, StringComparison.OrdinalIgnoreCase) >= 0;
	}

	// Token: 0x0600A1A9 RID: 41385 RVA: 0x003CC3DD File Offset: 0x003CA5DD
	public static string SeparateCamelCase(this string _value)
	{
		return Extensions.StringSeparationRegex.Replace(_value, " $1").Trim();
	}

	// Token: 0x0600A1AA RID: 41386 RVA: 0x003CC3F4 File Offset: 0x003CA5F4
	public static string UppercaseFirst(this string _s)
	{
		if (string.IsNullOrEmpty(_s))
		{
			return string.Empty;
		}
		return char.ToUpper(_s[0]).ToString() + _s.Substring(1);
	}

	// Token: 0x0600A1AB RID: 41387 RVA: 0x003CC42F File Offset: 0x003CA62F
	public unsafe static string ToHexString(this ReadOnlyMemory<byte> bytes)
	{
		if (bytes.Length <= 0)
		{
			return string.Empty;
		}
		return string.Create<ReadOnlyMemory<byte>>(bytes.Length * 2, bytes, delegate(Span<char> dest, ReadOnlyMemory<byte> srcMemory)
		{
			ReadOnlySpan<byte> span = srcMemory.Span;
			for (int i = 0; i < span.Length; i++)
			{
				int num = i * 2;
				*dest[num] = Extensions.HexLookup[*span[i] >> 4];
				*dest[num + 1] = Extensions.HexLookup[(int)(*span[i] & 15)];
			}
		});
	}

	// Token: 0x0600A1AC RID: 41388 RVA: 0x003CC46F File Offset: 0x003CA66F
	public static string ToHexString(this byte[] bytes)
	{
		return bytes.ToHexString();
	}

	// Token: 0x0600A1AD RID: 41389 RVA: 0x003CC47C File Offset: 0x003CA67C
	public static string ToUnicodeCodepoints(this string _value)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (char c in _value)
		{
			stringBuilder.AppendFormat("\\u{0:X4} ", (int)c);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x0600A1AE RID: 41390 RVA: 0x003CC4C2 File Offset: 0x003CA6C2
	public static string RemoveLineBreaks(this string _value)
	{
		return _value.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ");
	}

	// Token: 0x0600A1AF RID: 41391 RVA: 0x003CC4F2 File Offset: 0x003CA6F2
	public static int GetStableHashCode(this string _str)
	{
		return _str.AsSpan().GetStableHashCode();
	}

	// Token: 0x0600A1B0 RID: 41392 RVA: 0x003CC500 File Offset: 0x003CA700
	public unsafe static int GetStableHashCode(this ReadOnlySpan<char> _str)
	{
		int num = 5381;
		int num2 = num;
		int num3 = 0;
		while (num3 < _str.Length && *_str[num3] != 0)
		{
			num = ((num << 5) + num) ^ (int)(*_str[num3]);
			if (num3 == _str.Length - 1 || *_str[num3 + 1] == 0)
			{
				break;
			}
			num2 = ((num2 << 5) + num2) ^ (int)(*_str[num3 + 1]);
			num3 += 2;
		}
		return num + num2 * 1566083941;
	}

	// Token: 0x0600A1B1 RID: 41393 RVA: 0x003CC578 File Offset: 0x003CA778
	public static string Unindent(this string _indented, bool _trimEmptyLines = true)
	{
		if (_trimEmptyLines)
		{
			_indented = Extensions.unindentEmptyBeginning.Replace(_indented, string.Empty);
			_indented = Extensions.unindentEmptyEnd.Replace(_indented, string.Empty);
		}
		_indented = Extensions.unindentIndentationNoLinebreak.Replace(_indented, " $1");
		_indented = Extensions.unindentIndentationRegularLinebreak.Replace(_indented, string.Empty);
		return _indented;
	}

	// Token: 0x0600A1B2 RID: 41394 RVA: 0x003CC5D4 File Offset: 0x003CA7D4
	public static StringBuilder TrimEnd(this StringBuilder _sb)
	{
		if (_sb == null || _sb.Length == 0)
		{
			return _sb;
		}
		int num = _sb.Length - 1;
		while (num >= 0 && char.IsWhiteSpace(_sb[num]))
		{
			num--;
		}
		if (num < _sb.Length - 1)
		{
			_sb.Length = num + 1;
		}
		return _sb;
	}

	// Token: 0x0600A1B3 RID: 41395 RVA: 0x003CC624 File Offset: 0x003CA824
	public static StringBuilder TrimStart(this StringBuilder _sb)
	{
		if (_sb == null || _sb.Length == 0)
		{
			return _sb;
		}
		int num = 0;
		while (num < _sb.Length && char.IsWhiteSpace(_sb[num]))
		{
			num++;
		}
		if (num > 0)
		{
			_sb.Remove(0, num);
		}
		return _sb;
	}

	// Token: 0x0600A1B4 RID: 41396 RVA: 0x003CC66B File Offset: 0x003CA86B
	public static StringBuilder Trim(this StringBuilder _sb)
	{
		if (_sb == null || _sb.Length == 0)
		{
			return _sb;
		}
		return _sb.TrimEnd().TrimStart();
	}

	// Token: 0x0600A1B5 RID: 41397 RVA: 0x003CC685 File Offset: 0x003CA885
	public static string ToCultureInvariantString(this float _value)
	{
		return _value.ToString(Utils.StandardCulture);
	}

	// Token: 0x0600A1B6 RID: 41398 RVA: 0x003CC693 File Offset: 0x003CA893
	public static string ToCultureInvariantString(this double _value)
	{
		return _value.ToString(Utils.StandardCulture);
	}

	// Token: 0x0600A1B7 RID: 41399 RVA: 0x003CC6A1 File Offset: 0x003CA8A1
	public static string ToCultureInvariantString(this float _value, string _format)
	{
		return _value.ToString(_format, Utils.StandardCulture);
	}

	// Token: 0x0600A1B8 RID: 41400 RVA: 0x003CC6B0 File Offset: 0x003CA8B0
	public static string ToCultureInvariantString(this double _value, string _format)
	{
		return _value.ToString(_format, Utils.StandardCulture);
	}

	// Token: 0x0600A1B9 RID: 41401 RVA: 0x003CC6BF File Offset: 0x003CA8BF
	public static string ToCultureInvariantString(this decimal _value)
	{
		return _value.ToString(Utils.StandardCulture);
	}

	// Token: 0x0600A1BA RID: 41402 RVA: 0x003CC6CD File Offset: 0x003CA8CD
	public static string ToCultureInvariantString(this decimal _value, string _format)
	{
		return _value.ToString(_format, Utils.StandardCulture);
	}

	// Token: 0x0600A1BB RID: 41403 RVA: 0x003CC6DC File Offset: 0x003CA8DC
	public static string ToCultureInvariantString(this DateTime _value)
	{
		return _value.ToString(Utils.StandardCulture);
	}

	// Token: 0x0600A1BC RID: 41404 RVA: 0x003CC6EC File Offset: 0x003CA8EC
	public static string ToCultureInvariantString(this Vector2 _value)
	{
		return string.Concat(new string[]
		{
			"(",
			_value.x.ToCultureInvariantString("F1"),
			", ",
			_value.y.ToCultureInvariantString("F1"),
			")"
		});
	}

	// Token: 0x0600A1BD RID: 41405 RVA: 0x003CC744 File Offset: 0x003CA944
	public static string ToCultureInvariantString(this Vector2 _value, string _format)
	{
		return string.Concat(new string[]
		{
			"(",
			_value.x.ToCultureInvariantString(_format),
			", ",
			_value.y.ToCultureInvariantString(_format),
			")"
		});
	}

	// Token: 0x0600A1BE RID: 41406 RVA: 0x003CC794 File Offset: 0x003CA994
	public static string ToCultureInvariantString(this Vector3 _value)
	{
		return string.Concat(new string[]
		{
			"(",
			_value.x.ToCultureInvariantString("F1"),
			", ",
			_value.y.ToCultureInvariantString("F1"),
			", ",
			_value.z.ToCultureInvariantString("F1"),
			")"
		});
	}

	// Token: 0x0600A1BF RID: 41407 RVA: 0x003CC808 File Offset: 0x003CAA08
	public static string ToCultureInvariantString(this Vector3 _value, string _format)
	{
		return string.Concat(new string[]
		{
			"(",
			_value.x.ToCultureInvariantString(_format),
			", ",
			_value.y.ToCultureInvariantString(_format),
			", ",
			_value.z.ToCultureInvariantString(_format),
			")"
		});
	}

	// Token: 0x0600A1C0 RID: 41408 RVA: 0x003CC870 File Offset: 0x003CAA70
	public static string ToCultureInvariantString(this Vector4 _value)
	{
		return string.Concat(new string[]
		{
			"(",
			_value.x.ToCultureInvariantString("F1"),
			", ",
			_value.y.ToCultureInvariantString("F1"),
			", ",
			_value.z.ToCultureInvariantString("F1"),
			", ",
			_value.w.ToCultureInvariantString("F1"),
			")"
		});
	}

	// Token: 0x0600A1C1 RID: 41409 RVA: 0x003CC900 File Offset: 0x003CAB00
	public static string ToCultureInvariantString(this Vector4 _value, string _format)
	{
		return string.Concat(new string[]
		{
			"(",
			_value.x.ToCultureInvariantString(_format),
			", ",
			_value.y.ToCultureInvariantString(_format),
			", ",
			_value.z.ToCultureInvariantString(_format),
			", ",
			_value.w.ToCultureInvariantString(_format),
			")"
		});
	}

	// Token: 0x0600A1C2 RID: 41410 RVA: 0x003CC97D File Offset: 0x003CAB7D
	public static string ToCultureInvariantString(this Bounds _value)
	{
		return "Center: " + _value.center.ToCultureInvariantString() + ", Extents: " + _value.extents.ToCultureInvariantString();
	}

	// Token: 0x0600A1C3 RID: 41411 RVA: 0x003CC9A8 File Offset: 0x003CABA8
	public static string ToCultureInvariantString(this Rect _value)
	{
		return string.Concat(new string[]
		{
			"(x:",
			_value.x.ToCultureInvariantString("F2"),
			", y:",
			_value.y.ToCultureInvariantString("F2"),
			", width:",
			_value.width.ToCultureInvariantString("F2"),
			", height:",
			_value.height.ToCultureInvariantString("F2"),
			")"
		});
	}

	// Token: 0x0600A1C4 RID: 41412 RVA: 0x003CCA3C File Offset: 0x003CAC3C
	public static string ToCultureInvariantString(this Quaternion _value)
	{
		return string.Concat(new string[]
		{
			"(",
			_value.x.ToCultureInvariantString("F1"),
			", ",
			_value.y.ToCultureInvariantString("F1"),
			", ",
			_value.z.ToCultureInvariantString("F1"),
			", ",
			_value.w.ToCultureInvariantString("F1"),
			")"
		});
	}

	// Token: 0x0600A1C5 RID: 41413 RVA: 0x003CCACC File Offset: 0x003CACCC
	public static string ToCultureInvariantString(this Matrix4x4 _value)
	{
		return string.Concat(new string[]
		{
			_value.m00.ToCultureInvariantString("F5"),
			"\t",
			_value.m01.ToCultureInvariantString("F5"),
			"\t",
			_value.m02.ToCultureInvariantString("F5"),
			"\t",
			_value.m03.ToCultureInvariantString("F5"),
			"\n",
			_value.m10.ToCultureInvariantString("F5"),
			"\t",
			_value.m11.ToCultureInvariantString("F5"),
			"\t",
			_value.m12.ToCultureInvariantString("F5"),
			"\t",
			_value.m13.ToCultureInvariantString("F5"),
			"\n",
			_value.m20.ToCultureInvariantString("F5"),
			"\t",
			_value.m21.ToCultureInvariantString("F5"),
			"\t",
			_value.m22.ToCultureInvariantString("F5"),
			"\t",
			_value.m23.ToCultureInvariantString("F5"),
			"\n",
			_value.m30.ToCultureInvariantString("F5"),
			"\t",
			_value.m31.ToCultureInvariantString("F5"),
			"\t",
			_value.m32.ToCultureInvariantString("F5"),
			"\t",
			_value.m33.ToCultureInvariantString("F5"),
			"\n"
		});
	}

	// Token: 0x0600A1C6 RID: 41414 RVA: 0x003CCCAC File Offset: 0x003CAEAC
	public static string ToCultureInvariantString(this Color _value)
	{
		return string.Concat(new string[]
		{
			"RGBA(",
			_value.r.ToCultureInvariantString("F3"),
			", ",
			_value.g.ToCultureInvariantString("F3"),
			", ",
			_value.b.ToCultureInvariantString("F3"),
			", ",
			_value.a.ToCultureInvariantString("F3"),
			")"
		});
	}

	// Token: 0x0600A1C7 RID: 41415 RVA: 0x003CCD3C File Offset: 0x003CAF3C
	public static string ToCultureInvariantString(this Plane _value)
	{
		return string.Concat(new string[]
		{
			"(normal:(",
			_value.normal.x.ToCultureInvariantString("F1"),
			", ",
			_value.normal.y.ToCultureInvariantString("F1"),
			", ",
			_value.normal.z.ToCultureInvariantString("F1"),
			"), distance:",
			_value.distance.ToCultureInvariantString("F1"),
			")"
		});
	}

	// Token: 0x0600A1C8 RID: 41416 RVA: 0x003CCDDC File Offset: 0x003CAFDC
	public static string ToCultureInvariantString(this Ray _value)
	{
		return "Origin: " + _value.origin.ToCultureInvariantString() + ", Dir: " + _value.direction.ToCultureInvariantString();
	}

	// Token: 0x0600A1C9 RID: 41417 RVA: 0x003CCE05 File Offset: 0x003CB005
	public static string ToCultureInvariantString(this Ray2D _value)
	{
		return "Origin: " + _value.origin.ToCultureInvariantString() + ", Dir: " + _value.direction.ToCultureInvariantString();
	}

	// Token: 0x0600A1CA RID: 41418 RVA: 0x003CCE2E File Offset: 0x003CB02E
	public static Vector3 NormalizeReturnMagnitude(this Vector3 _value, out float magnitude)
	{
		magnitude = Vector3.Magnitude(_value);
		if ((double)magnitude > 9.999999747378752E-06)
		{
			_value /= magnitude;
		}
		else
		{
			_value = Vector3.zero;
		}
		return _value;
	}

	// Token: 0x0600A1CB RID: 41419 RVA: 0x003CCE5A File Offset: 0x003CB05A
	public static Vector3 Abs(this Vector3 _value)
	{
		return new Vector3(Math.Abs(_value.x), Math.Abs(_value.y), Math.Abs(_value.z));
	}

	// Token: 0x0600A1CC RID: 41420 RVA: 0x003CCE84 File Offset: 0x003CB084
	public static Vector3 Max(this Vector3 _value, params Vector3[] _others)
	{
		Vector3 vector = _value;
		foreach (Vector3 vector2 in _others)
		{
			vector = Vector3.Max(vector, vector2);
		}
		return vector;
	}

	// Token: 0x040079E1 RID: 31201
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex StringSeparationRegex = new Regex("((?<=\\p{Ll})\\p{Lu}|\\p{Lu}(?=\\p{Ll}))", RegexOptions.IgnorePatternWhitespace);

	// Token: 0x040079E2 RID: 31202
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly char[] HexLookup = new char[]
	{
		'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
		'a', 'b', 'c', 'd', 'e', 'f'
	};

	// Token: 0x040079E3 RID: 31203
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex unindentEmptyBeginning = new Regex("^\\s*\r?\n", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

	// Token: 0x040079E4 RID: 31204
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex unindentEmptyEnd = new Regex("\r?\n\\s*$", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

	// Token: 0x040079E5 RID: 31205
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex unindentIndentationNoLinebreak = new Regex("\\s*\r?\n\\s*([^\\s|])", RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant);

	// Token: 0x040079E6 RID: 31206
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex unindentIndentationRegularLinebreak = new Regex("^\\s*\\|", RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant);
}
