using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

// Token: 0x0200141D RID: 5149
public static class Extensions
{
	// Token: 0x0600A138 RID: 41272 RVA: 0x003C98FC File Offset: 0x003C7AFC
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

	// Token: 0x0600A139 RID: 41273 RVA: 0x003C9948 File Offset: 0x003C7B48
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

	// Token: 0x0600A13A RID: 41274 RVA: 0x003C999C File Offset: 0x003C7B9C
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

	// Token: 0x0600A13B RID: 41275 RVA: 0x003C9A11 File Offset: 0x003C7C11
	public static T GetComponentInChildren<T>(this GameObject o, bool searchInactive, bool avoidGC = false) where T : Component
	{
		return o.transform.GetComponentInChildren<T>(searchInactive, avoidGC);
	}

	// Token: 0x0600A13C RID: 41276 RVA: 0x003C9A20 File Offset: 0x003C7C20
	public static T GetComponentInChildren<T>(this Component c, bool searchInactive, bool avoidGC = false) where T : Component
	{
		return c.transform.GetComponentInChildren<T>(searchInactive, avoidGC);
	}

	// Token: 0x0600A13D RID: 41277 RVA: 0x003C9A30 File Offset: 0x003C7C30
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

	// Token: 0x0600A13E RID: 41278 RVA: 0x003C9AAC File Offset: 0x003C7CAC
	public static T GetOrAddComponent<T>(this GameObject go) where T : Component
	{
		T t = go.GetComponent<T>();
		if (t == null)
		{
			t = go.AddComponent<T>();
		}
		return t;
	}

	// Token: 0x0600A13F RID: 41279 RVA: 0x003C9AD8 File Offset: 0x003C7CD8
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

	// Token: 0x0600A140 RID: 41280 RVA: 0x003C9B30 File Offset: 0x003C7D30
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

	// Token: 0x0600A141 RID: 41281 RVA: 0x003C9B7C File Offset: 0x003C7D7C
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

	// Token: 0x0600A142 RID: 41282 RVA: 0x003C9BD4 File Offset: 0x003C7DD4
	public static void CopyTo<T>(this IList<T> _srcList, IList<T> _dest)
	{
		foreach (T t in _srcList)
		{
			_dest.Add(t);
		}
	}

	// Token: 0x0600A143 RID: 41283 RVA: 0x003C9C1C File Offset: 0x003C7E1C
	public static void Shuffle<T>(this List<T> _list)
	{
		_list.Shuffle<T>(GameManager.Instance.World.GetGameRandom());
	}

	// Token: 0x0600A144 RID: 41284 RVA: 0x003C9C34 File Offset: 0x003C7E34
	public static void Shuffle<T>(this List<T> _list, int _seed)
	{
		GameRandom gameRandom = GameRandomManager.Instance.CreateGameRandom(_seed);
		_list.Shuffle<T>(gameRandom);
		GameRandomManager.Instance.FreeGameRandom(gameRandom);
	}

	// Token: 0x0600A145 RID: 41285 RVA: 0x003C9C60 File Offset: 0x003C7E60
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

	// Token: 0x0600A146 RID: 41286 RVA: 0x003C9CB9 File Offset: 0x003C7EB9
	public static bool ColorEquals(this Color32 _a, Color32 _b)
	{
		return _a.r == _b.r && _a.g == _b.g && _a.b == _b.b && _a.a == _b.a;
	}

	// Token: 0x0600A147 RID: 41287 RVA: 0x003C9CF5 File Offset: 0x003C7EF5
	public static string ToHexCode(this Color _color, bool _includeAlpha = false)
	{
		return _color.ToHexCode(_includeAlpha);
	}

	// Token: 0x0600A148 RID: 41288 RVA: 0x003C9D03 File Offset: 0x003C7F03
	public static Color WithAlpha(this Color _color, float _alpha)
	{
		return new Color(_color.r, _color.g, _color.b, _alpha);
	}

	// Token: 0x0600A149 RID: 41289 RVA: 0x003C9D20 File Offset: 0x003C7F20
	public static string ToHexCode(this Color32 _color, bool _includeAlpha = false)
	{
		if (!_includeAlpha)
		{
			return string.Format("{0:X02}{1:X02}{2:X02}", _color.r, _color.g, _color.b);
		}
		return string.Format("{0:X02}{1:X02}{2:X02}{3:X02}", new object[] { _color.r, _color.g, _color.b, _color.a });
	}

	// Token: 0x0600A14A RID: 41290 RVA: 0x003C9DA4 File Offset: 0x003C7FA4
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

	// Token: 0x0600A14B RID: 41291 RVA: 0x003C9DF3 File Offset: 0x003C7FF3
	public static Bounds Offset(this Bounds _bounds, Vector3 _offset)
	{
		return new Bounds(_bounds.center + _offset, _bounds.size);
	}

	// Token: 0x0600A14C RID: 41292 RVA: 0x003C9E10 File Offset: 0x003C8010
	public static bool ContainsInclusive(this BoundsInt bounds, Vector3Int position)
	{
		return position.x >= bounds.min.x && position.y >= bounds.min.y && position.z >= bounds.min.z && position.x <= bounds.max.x && position.y <= bounds.max.y && position.z <= bounds.max.z;
	}

	// Token: 0x0600A14D RID: 41293 RVA: 0x003C9EB4 File Offset: 0x003C80B4
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

	// Token: 0x0600A14E RID: 41294 RVA: 0x003C9F50 File Offset: 0x003C8150
	public static void CalculatePersistableHash(this Bounds _bounds, IncrementalHash calcHash)
	{
		calcHash.AppendDataNoAlloc(_bounds.center.x);
		calcHash.AppendDataNoAlloc(_bounds.center.y);
		calcHash.AppendDataNoAlloc(_bounds.center.z);
		calcHash.AppendDataNoAlloc(_bounds.size.x);
		calcHash.AppendDataNoAlloc(_bounds.size.y);
		calcHash.AppendDataNoAlloc(_bounds.size.z);
	}

	// Token: 0x0600A14F RID: 41295 RVA: 0x000063DF File Offset: 0x000045DF
	public static bool EqualsCaseInsensitive(this string _a, string _b)
	{
		return string.Equals(_a, _b, StringComparison.OrdinalIgnoreCase);
	}

	// Token: 0x0600A150 RID: 41296 RVA: 0x003C9FC9 File Offset: 0x003C81C9
	public static bool ContainsCaseInsensitive(this string _a, string _b)
	{
		return _a.IndexOf(_b, StringComparison.OrdinalIgnoreCase) >= 0;
	}

	// Token: 0x0600A151 RID: 41297 RVA: 0x003C9FD9 File Offset: 0x003C81D9
	public static string SeparateCamelCase(this string _value)
	{
		return Extensions.StringSeparationRegex.Replace(_value, " $1").Trim();
	}

	// Token: 0x0600A152 RID: 41298 RVA: 0x003C9FF0 File Offset: 0x003C81F0
	public static string UppercaseFirst(this string _s)
	{
		if (string.IsNullOrEmpty(_s))
		{
			return string.Empty;
		}
		return char.ToUpper(_s[0]).ToString() + _s.Substring(1);
	}

	// Token: 0x0600A153 RID: 41299 RVA: 0x003CA02B File Offset: 0x003C822B
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

	// Token: 0x0600A154 RID: 41300 RVA: 0x003CA06B File Offset: 0x003C826B
	public static string ToHexString(this byte[] bytes)
	{
		return bytes.ToHexString();
	}

	// Token: 0x0600A155 RID: 41301 RVA: 0x003CA078 File Offset: 0x003C8278
	public static string ToUnicodeCodepoints(this string _value)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (char c in _value)
		{
			stringBuilder.AppendFormat("\\u{0:X4} ", (int)c);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x0600A156 RID: 41302 RVA: 0x003CA0BE File Offset: 0x003C82BE
	public static string RemoveLineBreaks(this string _value)
	{
		return _value.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " ");
	}

	// Token: 0x0600A157 RID: 41303 RVA: 0x003CA0EE File Offset: 0x003C82EE
	public static int GetStableHashCode(this string _str)
	{
		return _str.AsSpan().GetStableHashCode();
	}

	// Token: 0x0600A158 RID: 41304 RVA: 0x003CA0FC File Offset: 0x003C82FC
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

	// Token: 0x0600A159 RID: 41305 RVA: 0x003CA174 File Offset: 0x003C8374
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

	// Token: 0x0600A15A RID: 41306 RVA: 0x003CA1D0 File Offset: 0x003C83D0
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

	// Token: 0x0600A15B RID: 41307 RVA: 0x003CA220 File Offset: 0x003C8420
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

	// Token: 0x0600A15C RID: 41308 RVA: 0x003CA267 File Offset: 0x003C8467
	public static StringBuilder Trim(this StringBuilder _sb)
	{
		if (_sb == null || _sb.Length == 0)
		{
			return _sb;
		}
		return _sb.TrimEnd().TrimStart();
	}

	// Token: 0x0600A15D RID: 41309 RVA: 0x003CA281 File Offset: 0x003C8481
	public static string ToCultureInvariantString(this float _value)
	{
		return _value.ToString(Utils.StandardCulture);
	}

	// Token: 0x0600A15E RID: 41310 RVA: 0x003CA28F File Offset: 0x003C848F
	public static string ToCultureInvariantString(this double _value)
	{
		return _value.ToString(Utils.StandardCulture);
	}

	// Token: 0x0600A15F RID: 41311 RVA: 0x003CA29D File Offset: 0x003C849D
	public static string ToCultureInvariantString(this float _value, string _format)
	{
		return _value.ToString(_format, Utils.StandardCulture);
	}

	// Token: 0x0600A160 RID: 41312 RVA: 0x003CA2AC File Offset: 0x003C84AC
	public static string ToCultureInvariantString(this double _value, string _format)
	{
		return _value.ToString(_format, Utils.StandardCulture);
	}

	// Token: 0x0600A161 RID: 41313 RVA: 0x003CA2BB File Offset: 0x003C84BB
	public static string ToCultureInvariantString(this decimal _value)
	{
		return _value.ToString(Utils.StandardCulture);
	}

	// Token: 0x0600A162 RID: 41314 RVA: 0x003CA2C9 File Offset: 0x003C84C9
	public static string ToCultureInvariantString(this decimal _value, string _format)
	{
		return _value.ToString(_format, Utils.StandardCulture);
	}

	// Token: 0x0600A163 RID: 41315 RVA: 0x003CA2D8 File Offset: 0x003C84D8
	public static string ToCultureInvariantString(this DateTime _value)
	{
		return _value.ToString(Utils.StandardCulture);
	}

	// Token: 0x0600A164 RID: 41316 RVA: 0x003CA2E8 File Offset: 0x003C84E8
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

	// Token: 0x0600A165 RID: 41317 RVA: 0x003CA340 File Offset: 0x003C8540
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

	// Token: 0x0600A166 RID: 41318 RVA: 0x003CA390 File Offset: 0x003C8590
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

	// Token: 0x0600A167 RID: 41319 RVA: 0x003CA404 File Offset: 0x003C8604
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

	// Token: 0x0600A168 RID: 41320 RVA: 0x003CA46C File Offset: 0x003C866C
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

	// Token: 0x0600A169 RID: 41321 RVA: 0x003CA4FC File Offset: 0x003C86FC
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

	// Token: 0x0600A16A RID: 41322 RVA: 0x003CA579 File Offset: 0x003C8779
	public static string ToCultureInvariantString(this Bounds _value)
	{
		return "Center: " + _value.center.ToCultureInvariantString() + ", Extents: " + _value.extents.ToCultureInvariantString();
	}

	// Token: 0x0600A16B RID: 41323 RVA: 0x003CA5A4 File Offset: 0x003C87A4
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

	// Token: 0x0600A16C RID: 41324 RVA: 0x003CA638 File Offset: 0x003C8838
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

	// Token: 0x0600A16D RID: 41325 RVA: 0x003CA6C8 File Offset: 0x003C88C8
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

	// Token: 0x0600A16E RID: 41326 RVA: 0x003CA8A8 File Offset: 0x003C8AA8
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

	// Token: 0x0600A16F RID: 41327 RVA: 0x003CA938 File Offset: 0x003C8B38
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

	// Token: 0x0600A170 RID: 41328 RVA: 0x003CA9D8 File Offset: 0x003C8BD8
	public static string ToCultureInvariantString(this Ray _value)
	{
		return "Origin: " + _value.origin.ToCultureInvariantString() + ", Dir: " + _value.direction.ToCultureInvariantString();
	}

	// Token: 0x0600A171 RID: 41329 RVA: 0x003CAA01 File Offset: 0x003C8C01
	public static string ToCultureInvariantString(this Ray2D _value)
	{
		return "Origin: " + _value.origin.ToCultureInvariantString() + ", Dir: " + _value.direction.ToCultureInvariantString();
	}

	// Token: 0x0600A172 RID: 41330 RVA: 0x003CAA2A File Offset: 0x003C8C2A
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

	// Token: 0x0600A173 RID: 41331 RVA: 0x003CAA56 File Offset: 0x003C8C56
	public static Vector3 Abs(this Vector3 _value)
	{
		return new Vector3(Math.Abs(_value.x), Math.Abs(_value.y), Math.Abs(_value.z));
	}

	// Token: 0x0600A174 RID: 41332 RVA: 0x003CAA80 File Offset: 0x003C8C80
	public static Vector3 Max(this Vector3 _value, params Vector3[] _others)
	{
		Vector3 vector = _value;
		foreach (Vector3 vector2 in _others)
		{
			vector = Vector3.Max(vector, vector2);
		}
		return vector;
	}

	// Token: 0x040079A0 RID: 31136
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex StringSeparationRegex = new Regex("((?<=\\p{Ll})\\p{Lu}|\\p{Lu}(?=\\p{Ll}))", RegexOptions.IgnorePatternWhitespace);

	// Token: 0x040079A1 RID: 31137
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly char[] HexLookup = new char[]
	{
		'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
		'a', 'b', 'c', 'd', 'e', 'f'
	};

	// Token: 0x040079A2 RID: 31138
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex unindentEmptyBeginning = new Regex("^\\s*\r?\n", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

	// Token: 0x040079A3 RID: 31139
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex unindentEmptyEnd = new Regex("\r?\n\\s*$", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.CultureInvariant);

	// Token: 0x040079A4 RID: 31140
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex unindentIndentationNoLinebreak = new Regex("\\s*\r?\n\\s*([^\\s|])", RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant);

	// Token: 0x040079A5 RID: 31141
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Regex unindentIndentationRegularLinebreak = new Regex("^\\s*\\|", RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant);
}
