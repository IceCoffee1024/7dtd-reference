using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

// Token: 0x02001578 RID: 5496
public static class vp_Utility
{
	// Token: 0x0600ABC3 RID: 43971 RVA: 0x003FEE29 File Offset: 0x003FD029
	[Obsolete("Please use 'vp_MathUtility.NaNSafeFloat' instead.")]
	public static float NaNSafeFloat(float value, float prevValue = 0f)
	{
		return vp_MathUtility.NaNSafeFloat(value, prevValue);
	}

	// Token: 0x0600ABC4 RID: 43972 RVA: 0x003FEE32 File Offset: 0x003FD032
	[Obsolete("Please use 'vp_MathUtility.NaNSafeVector2' instead.")]
	public static Vector2 NaNSafeVector2(Vector2 vector, Vector2 prevVector = default(Vector2))
	{
		return vp_MathUtility.NaNSafeVector2(vector, prevVector);
	}

	// Token: 0x0600ABC5 RID: 43973 RVA: 0x003FEE3B File Offset: 0x003FD03B
	[Obsolete("Please use 'vp_MathUtility.NaNSafeVector3' instead.")]
	public static Vector3 NaNSafeVector3(Vector3 vector, Vector3 prevVector = default(Vector3))
	{
		return vp_MathUtility.NaNSafeVector3(vector, prevVector);
	}

	// Token: 0x0600ABC6 RID: 43974 RVA: 0x003FEE44 File Offset: 0x003FD044
	[Obsolete("Please use 'vp_MathUtility.NaNSafeQuaternion' instead.")]
	public static Quaternion NaNSafeQuaternion(Quaternion quaternion, Quaternion prevQuaternion = default(Quaternion))
	{
		return vp_MathUtility.NaNSafeQuaternion(quaternion, prevQuaternion);
	}

	// Token: 0x0600ABC7 RID: 43975 RVA: 0x003FEE4D File Offset: 0x003FD04D
	[Obsolete("Please use 'vp_MathUtility.SnapToZero' instead.")]
	public static Vector3 SnapToZero(Vector3 value, float epsilon = 0.0001f)
	{
		return vp_MathUtility.SnapToZero(value, epsilon);
	}

	// Token: 0x0600ABC8 RID: 43976 RVA: 0x003FEE56 File Offset: 0x003FD056
	[Obsolete("Please use 'vp_MathUtility.SnapToZero' instead.")]
	public static float SnapToZero(float value, float epsilon = 0.0001f)
	{
		return vp_MathUtility.SnapToZero(value, epsilon);
	}

	// Token: 0x0600ABC9 RID: 43977 RVA: 0x003FEE5F File Offset: 0x003FD05F
	[Obsolete("Please use 'vp_MathUtility.ReduceDecimals' instead.")]
	public static float ReduceDecimals(float value, float factor = 1000f)
	{
		return vp_MathUtility.ReduceDecimals(value, factor);
	}

	// Token: 0x0600ABCA RID: 43978 RVA: 0x003FEE68 File Offset: 0x003FD068
	[Obsolete("Please use 'vp_3DUtility.HorizontalVector' instead.")]
	public static Vector3 HorizontalVector(Vector3 value)
	{
		return vp_3DUtility.HorizontalVector(value);
	}

	// Token: 0x0600ABCB RID: 43979 RVA: 0x003FEE70 File Offset: 0x003FD070
	public static string GetErrorLocation(int level = 1, bool showOnlyLast = false)
	{
		StackTrace stackTrace = new StackTrace();
		string text = "";
		string text2 = "";
		for (int i = stackTrace.FrameCount - 1; i > level; i--)
		{
			if (i < stackTrace.FrameCount - 1)
			{
				text += " --> ";
			}
			StackFrame frame = stackTrace.GetFrame(i);
			if (frame.GetMethod().DeclaringType.ToString() == text2)
			{
				text = "";
			}
			text2 = frame.GetMethod().DeclaringType.ToString();
			text = text + text2 + ":" + frame.GetMethod().Name;
		}
		if (showOnlyLast)
		{
			try
			{
				text = text.Substring(text.LastIndexOf(" --> "));
				text = text.Replace(" --> ", "");
			}
			catch
			{
			}
		}
		return text;
	}

	// Token: 0x0600ABCC RID: 43980 RVA: 0x003FEF48 File Offset: 0x003FD148
	public static string GetTypeAlias(Type type)
	{
		string text = "";
		if (!vp_Utility.m_TypeAliases.TryGetValue(type, out text))
		{
			return type.ToString();
		}
		return text;
	}

	// Token: 0x0600ABCD RID: 43981 RVA: 0x003FEF72 File Offset: 0x003FD172
	public static void Activate(GameObject obj, bool activate = true)
	{
		obj.SetActive(activate);
	}

	// Token: 0x0600ABCE RID: 43982 RVA: 0x003FEF7B File Offset: 0x003FD17B
	public static bool IsActive(GameObject obj)
	{
		return obj.activeSelf;
	}

	// Token: 0x170013CB RID: 5067
	// (get) Token: 0x0600ABCF RID: 43983 RVA: 0x003FEF83 File Offset: 0x003FD183
	// (set) Token: 0x0600ABD0 RID: 43984 RVA: 0x003FEF8D File Offset: 0x003FD18D
	public static bool LockCursor
	{
		get
		{
			return Cursor.lockState == CursorLockMode.Locked;
		}
		set
		{
			Cursor.visible = !value;
			Cursor.lockState = (value ? CursorLockMode.Locked : SoftCursor.DefaultCursorLockState);
		}
	}

	// Token: 0x0600ABD1 RID: 43985 RVA: 0x003FEFA8 File Offset: 0x003FD1A8
	public static void RandomizeList<T>(this List<T> list)
	{
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			int num = global::UnityEngine.Random.Range(i, count);
			T t = list[i];
			list[i] = list[num];
			list[num] = t;
		}
	}

	// Token: 0x0600ABD2 RID: 43986 RVA: 0x003FEFEE File Offset: 0x003FD1EE
	public static T RandomObject<T>(this List<T> list)
	{
		List<T> list2 = new List<T>();
		list2.AddRange(list);
		list2.RandomizeList<T>();
		return list2.FirstOrDefault<T>();
	}

	// Token: 0x0600ABD3 RID: 43987 RVA: 0x003FF007 File Offset: 0x003FD207
	public static List<T> ChildComponentsToList<T>(this Transform t) where T : Component
	{
		return t.GetComponentsInChildren<T>().ToList<T>();
	}

	// Token: 0x0600ABD4 RID: 43988 RVA: 0x003FF014 File Offset: 0x003FD214
	public static bool IsDescendant(Transform descendant, Transform potentialAncestor)
	{
		return !(descendant == null) && !(potentialAncestor == null) && !(descendant.parent == descendant) && (descendant.parent == potentialAncestor || vp_Utility.IsDescendant(descendant.parent, potentialAncestor));
	}

	// Token: 0x0600ABD5 RID: 43989 RVA: 0x003FF063 File Offset: 0x003FD263
	public static Component GetParent(Component target)
	{
		if (target == null)
		{
			return null;
		}
		if (target != target.transform)
		{
			return target.transform;
		}
		return target.transform.parent;
	}

	// Token: 0x0600ABD6 RID: 43990 RVA: 0x003FF090 File Offset: 0x003FD290
	public static Transform GetTransformByNameInChildren(Transform trans, string name, bool includeInactive = false, bool subString = false)
	{
		name = name.ToLower();
		foreach (object obj in trans)
		{
			Transform transform = (Transform)obj;
			if (!subString)
			{
				if (transform.name.ToLower() == name && (includeInactive || transform.gameObject.activeInHierarchy))
				{
					return transform;
				}
			}
			else if (transform.name.ToLower().Contains(name) && (includeInactive || transform.gameObject.activeInHierarchy))
			{
				return transform;
			}
			Transform transformByNameInChildren = vp_Utility.GetTransformByNameInChildren(transform, name, includeInactive, subString);
			if (transformByNameInChildren != null)
			{
				return transformByNameInChildren;
			}
		}
		return null;
	}

	// Token: 0x0600ABD7 RID: 43991 RVA: 0x003FF154 File Offset: 0x003FD354
	public static Transform GetTransformByNameInAncestors(Transform trans, string name, bool includeInactive = false, bool subString = false)
	{
		if (trans.parent == null)
		{
			return null;
		}
		name = name.ToLower();
		if (!subString)
		{
			if (trans.parent.name.ToLower() == name && (includeInactive || trans.gameObject.activeInHierarchy))
			{
				return trans.parent;
			}
		}
		else if (trans.parent.name.ToLower().Contains(name) && (includeInactive || trans.gameObject.activeInHierarchy))
		{
			return trans.parent;
		}
		Transform transformByNameInAncestors = vp_Utility.GetTransformByNameInAncestors(trans.parent, name, includeInactive, subString);
		if (transformByNameInAncestors != null)
		{
			return transformByNameInAncestors;
		}
		return null;
	}

	// Token: 0x0600ABD8 RID: 43992 RVA: 0x003FF1F5 File Offset: 0x003FD3F5
	public static global::UnityEngine.Object Instantiate(global::UnityEngine.Object original)
	{
		return vp_Utility.Instantiate(original, Vector3.zero, Quaternion.identity);
	}

	// Token: 0x0600ABD9 RID: 43993 RVA: 0x003FF207 File Offset: 0x003FD407
	public static global::UnityEngine.Object Instantiate(global::UnityEngine.Object original, Vector3 position, Quaternion rotation)
	{
		if (vp_PoolManager.Instance == null || !vp_PoolManager.Instance.enabled)
		{
			return global::UnityEngine.Object.Instantiate(original, position, rotation);
		}
		return vp_GlobalEventReturn<global::UnityEngine.Object, Vector3, Quaternion, global::UnityEngine.Object>.Send("vp_PoolManager Instantiate", original, position, rotation);
	}

	// Token: 0x0600ABDA RID: 43994 RVA: 0x003FF238 File Offset: 0x003FD438
	public static void Destroy(global::UnityEngine.Object obj)
	{
		vp_Utility.Destroy(obj, 0f);
	}

	// Token: 0x0600ABDB RID: 43995 RVA: 0x003FF245 File Offset: 0x003FD445
	public static void Destroy(global::UnityEngine.Object obj, float t)
	{
		if (vp_PoolManager.Instance == null || !vp_PoolManager.Instance.enabled)
		{
			global::UnityEngine.Object.Destroy(obj, t);
			return;
		}
		vp_GlobalEvent<global::UnityEngine.Object, float>.Send("vp_PoolManager Destroy", obj, t);
	}

	// Token: 0x170013CC RID: 5068
	// (get) Token: 0x0600ABDC RID: 43996 RVA: 0x003FF274 File Offset: 0x003FD474
	public static int UniqueID
	{
		get
		{
			int num;
			for (;;)
			{
				num = global::UnityEngine.Random.Range(0, 1000000000);
				if (!vp_Utility.m_UniqueIDs.ContainsKey(num))
				{
					break;
				}
				if (vp_Utility.m_UniqueIDs.Count >= 1000000000)
				{
					vp_Utility.ClearUniqueIDs();
					global::UnityEngine.Debug.LogWarning("Warning (vp_Utility.UniqueID) More than 1 billion unique IDs have been generated. This seems like an awful lot for a game client. Clearing dictionary and starting over!");
				}
			}
			vp_Utility.m_UniqueIDs.Add(num, 0);
			return num;
		}
	}

	// Token: 0x0600ABDD RID: 43997 RVA: 0x003FF2C9 File Offset: 0x003FD4C9
	public static void ClearUniqueIDs()
	{
		vp_Utility.m_UniqueIDs.Clear();
	}

	// Token: 0x0600ABDE RID: 43998 RVA: 0x003FF2D5 File Offset: 0x003FD4D5
	public static int PositionToID(Vector3 position)
	{
		return (int)Mathf.Abs(position.x * 10000f + position.y * 1000f + position.z * 100f);
	}

	// Token: 0x0600ABDF RID: 43999 RVA: 0x003FF303 File Offset: 0x003FD503
	[Obsolete("Please use 'vp_AudioUtility.PlayRandomSound' instead.")]
	public static void PlayRandomSound(AudioSource audioSource, List<AudioClip> sounds, Vector2 pitchRange)
	{
		vp_AudioUtility.PlayRandomSound(audioSource, sounds, pitchRange);
	}

	// Token: 0x0600ABE0 RID: 44000 RVA: 0x003FF30D File Offset: 0x003FD50D
	[Obsolete("Please use 'vp_AudioUtility.PlayRandomSound' instead.")]
	public static void PlayRandomSound(AudioSource audioSource, List<AudioClip> sounds)
	{
		vp_AudioUtility.PlayRandomSound(audioSource, sounds);
	}

	// Token: 0x04007EE9 RID: 32489
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly Dictionary<Type, string> m_TypeAliases = new Dictionary<Type, string>
	{
		{
			typeof(void),
			"void"
		},
		{
			typeof(byte),
			"byte"
		},
		{
			typeof(sbyte),
			"sbyte"
		},
		{
			typeof(short),
			"short"
		},
		{
			typeof(ushort),
			"ushort"
		},
		{
			typeof(int),
			"int"
		},
		{
			typeof(uint),
			"uint"
		},
		{
			typeof(long),
			"long"
		},
		{
			typeof(ulong),
			"ulong"
		},
		{
			typeof(float),
			"float"
		},
		{
			typeof(double),
			"double"
		},
		{
			typeof(decimal),
			"decimal"
		},
		{
			typeof(object),
			"object"
		},
		{
			typeof(bool),
			"bool"
		},
		{
			typeof(char),
			"char"
		},
		{
			typeof(string),
			"string"
		},
		{
			typeof(Vector2),
			"Vector2"
		},
		{
			typeof(Vector3),
			"Vector3"
		},
		{
			typeof(Vector4),
			"Vector4"
		}
	};

	// Token: 0x04007EEA RID: 32490
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<int, int> m_UniqueIDs = new Dictionary<int, int>();
}
