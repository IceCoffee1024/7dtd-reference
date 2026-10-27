using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

// Token: 0x020015C7 RID: 5575
public static class vp_Utility
{
	// Token: 0x0600ADCB RID: 44491 RVA: 0x0040B3C1 File Offset: 0x004095C1
	[Obsolete("Please use 'vp_MathUtility.NaNSafeFloat' instead.")]
	public static float NaNSafeFloat(float value, float prevValue = 0f)
	{
		return vp_MathUtility.NaNSafeFloat(value, prevValue);
	}

	// Token: 0x0600ADCC RID: 44492 RVA: 0x0040B3CA File Offset: 0x004095CA
	[Obsolete("Please use 'vp_MathUtility.NaNSafeVector2' instead.")]
	public static Vector2 NaNSafeVector2(Vector2 vector, Vector2 prevVector = default(Vector2))
	{
		return vp_MathUtility.NaNSafeVector2(vector, prevVector);
	}

	// Token: 0x0600ADCD RID: 44493 RVA: 0x0040B3D3 File Offset: 0x004095D3
	[Obsolete("Please use 'vp_MathUtility.NaNSafeVector3' instead.")]
	public static Vector3 NaNSafeVector3(Vector3 vector, Vector3 prevVector = default(Vector3))
	{
		return vp_MathUtility.NaNSafeVector3(vector, prevVector);
	}

	// Token: 0x0600ADCE RID: 44494 RVA: 0x0040B3DC File Offset: 0x004095DC
	[Obsolete("Please use 'vp_MathUtility.NaNSafeQuaternion' instead.")]
	public static Quaternion NaNSafeQuaternion(Quaternion quaternion, Quaternion prevQuaternion = default(Quaternion))
	{
		return vp_MathUtility.NaNSafeQuaternion(quaternion, prevQuaternion);
	}

	// Token: 0x0600ADCF RID: 44495 RVA: 0x0040B3E5 File Offset: 0x004095E5
	[Obsolete("Please use 'vp_MathUtility.SnapToZero' instead.")]
	public static Vector3 SnapToZero(Vector3 value, float epsilon = 0.0001f)
	{
		return vp_MathUtility.SnapToZero(value, epsilon);
	}

	// Token: 0x0600ADD0 RID: 44496 RVA: 0x0040B3EE File Offset: 0x004095EE
	[Obsolete("Please use 'vp_MathUtility.SnapToZero' instead.")]
	public static float SnapToZero(float value, float epsilon = 0.0001f)
	{
		return vp_MathUtility.SnapToZero(value, epsilon);
	}

	// Token: 0x0600ADD1 RID: 44497 RVA: 0x0040B3F7 File Offset: 0x004095F7
	[Obsolete("Please use 'vp_MathUtility.ReduceDecimals' instead.")]
	public static float ReduceDecimals(float value, float factor = 1000f)
	{
		return vp_MathUtility.ReduceDecimals(value, factor);
	}

	// Token: 0x0600ADD2 RID: 44498 RVA: 0x0040B400 File Offset: 0x00409600
	[Obsolete("Please use 'vp_3DUtility.HorizontalVector' instead.")]
	public static Vector3 HorizontalVector(Vector3 value)
	{
		return vp_3DUtility.HorizontalVector(value);
	}

	// Token: 0x0600ADD3 RID: 44499 RVA: 0x0040B408 File Offset: 0x00409608
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

	// Token: 0x0600ADD4 RID: 44500 RVA: 0x0040B4E0 File Offset: 0x004096E0
	public static string GetTypeAlias(Type type)
	{
		string text = "";
		if (!vp_Utility.m_TypeAliases.TryGetValue(type, out text))
		{
			return type.ToString();
		}
		return text;
	}

	// Token: 0x0600ADD5 RID: 44501 RVA: 0x0040B50A File Offset: 0x0040970A
	public static void Activate(GameObject obj, bool activate = true)
	{
		obj.SetActive(activate);
	}

	// Token: 0x0600ADD6 RID: 44502 RVA: 0x0040B513 File Offset: 0x00409713
	public static bool IsActive(GameObject obj)
	{
		return obj.activeSelf;
	}

	// Token: 0x17001455 RID: 5205
	// (get) Token: 0x0600ADD7 RID: 44503 RVA: 0x0040B51B File Offset: 0x0040971B
	// (set) Token: 0x0600ADD8 RID: 44504 RVA: 0x0040B525 File Offset: 0x00409725
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

	// Token: 0x0600ADD9 RID: 44505 RVA: 0x0040B540 File Offset: 0x00409740
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

	// Token: 0x0600ADDA RID: 44506 RVA: 0x0040B586 File Offset: 0x00409786
	public static T RandomObject<T>(this List<T> list)
	{
		List<T> list2 = new List<T>();
		list2.AddRange(list);
		list2.RandomizeList<T>();
		return list2.FirstOrDefault<T>();
	}

	// Token: 0x0600ADDB RID: 44507 RVA: 0x0040B59F File Offset: 0x0040979F
	public static List<T> ChildComponentsToList<T>(this Transform t) where T : Component
	{
		return t.GetComponentsInChildren<T>().ToList<T>();
	}

	// Token: 0x0600ADDC RID: 44508 RVA: 0x0040B5AC File Offset: 0x004097AC
	public static bool IsDescendant(Transform descendant, Transform potentialAncestor)
	{
		return !(descendant == null) && !(potentialAncestor == null) && !(descendant.parent == descendant) && (descendant.parent == potentialAncestor || vp_Utility.IsDescendant(descendant.parent, potentialAncestor));
	}

	// Token: 0x0600ADDD RID: 44509 RVA: 0x0040B5FB File Offset: 0x004097FB
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

	// Token: 0x0600ADDE RID: 44510 RVA: 0x0040B628 File Offset: 0x00409828
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

	// Token: 0x0600ADDF RID: 44511 RVA: 0x0040B6EC File Offset: 0x004098EC
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

	// Token: 0x0600ADE0 RID: 44512 RVA: 0x0040B78D File Offset: 0x0040998D
	public static global::UnityEngine.Object Instantiate(global::UnityEngine.Object original)
	{
		return vp_Utility.Instantiate(original, Vector3.zero, Quaternion.identity);
	}

	// Token: 0x0600ADE1 RID: 44513 RVA: 0x0040B79F File Offset: 0x0040999F
	public static global::UnityEngine.Object Instantiate(global::UnityEngine.Object original, Vector3 position, Quaternion rotation)
	{
		if (vp_PoolManager.Instance == null || !vp_PoolManager.Instance.enabled)
		{
			return global::UnityEngine.Object.Instantiate(original, position, rotation);
		}
		return vp_GlobalEventReturn<global::UnityEngine.Object, Vector3, Quaternion, global::UnityEngine.Object>.Send("vp_PoolManager Instantiate", original, position, rotation);
	}

	// Token: 0x0600ADE2 RID: 44514 RVA: 0x0040B7D0 File Offset: 0x004099D0
	public static void Destroy(global::UnityEngine.Object obj)
	{
		vp_Utility.Destroy(obj, 0f);
	}

	// Token: 0x0600ADE3 RID: 44515 RVA: 0x0040B7DD File Offset: 0x004099DD
	public static void Destroy(global::UnityEngine.Object obj, float t)
	{
		if (vp_PoolManager.Instance == null || !vp_PoolManager.Instance.enabled)
		{
			global::UnityEngine.Object.Destroy(obj, t);
			return;
		}
		vp_GlobalEvent<global::UnityEngine.Object, float>.Send("vp_PoolManager Destroy", obj, t);
	}

	// Token: 0x17001456 RID: 5206
	// (get) Token: 0x0600ADE4 RID: 44516 RVA: 0x0040B80C File Offset: 0x00409A0C
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

	// Token: 0x0600ADE5 RID: 44517 RVA: 0x0040B861 File Offset: 0x00409A61
	public static void ClearUniqueIDs()
	{
		vp_Utility.m_UniqueIDs.Clear();
	}

	// Token: 0x0600ADE6 RID: 44518 RVA: 0x0040B86D File Offset: 0x00409A6D
	public static int PositionToID(Vector3 position)
	{
		return (int)Mathf.Abs(position.x * 10000f + position.y * 1000f + position.z * 100f);
	}

	// Token: 0x0600ADE7 RID: 44519 RVA: 0x0040B89B File Offset: 0x00409A9B
	[Obsolete("Please use 'vp_AudioUtility.PlayRandomSound' instead.")]
	public static void PlayRandomSound(AudioSource audioSource, List<AudioClip> sounds, Vector2 pitchRange)
	{
		vp_AudioUtility.PlayRandomSound(audioSource, sounds, pitchRange);
	}

	// Token: 0x0600ADE8 RID: 44520 RVA: 0x0040B8A5 File Offset: 0x00409AA5
	[Obsolete("Please use 'vp_AudioUtility.PlayRandomSound' instead.")]
	public static void PlayRandomSound(AudioSource audioSource, List<AudioClip> sounds)
	{
		vp_AudioUtility.PlayRandomSound(audioSource, sounds);
	}

	// Token: 0x04008086 RID: 32902
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

	// Token: 0x04008087 RID: 32903
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<int, int> m_UniqueIDs = new Dictionary<int, int>();
}
