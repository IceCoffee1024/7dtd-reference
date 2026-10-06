using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

// Token: 0x020015B5 RID: 5557
public static class vp_Utility
{
	// Token: 0x0600AD73 RID: 44403 RVA: 0x00408FBD File Offset: 0x004071BD
	[Obsolete("Please use 'vp_MathUtility.NaNSafeFloat' instead.")]
	public static float NaNSafeFloat(float value, float prevValue = 0f)
	{
		return vp_MathUtility.NaNSafeFloat(value, prevValue);
	}

	// Token: 0x0600AD74 RID: 44404 RVA: 0x00408FC6 File Offset: 0x004071C6
	[Obsolete("Please use 'vp_MathUtility.NaNSafeVector2' instead.")]
	public static Vector2 NaNSafeVector2(Vector2 vector, Vector2 prevVector = default(Vector2))
	{
		return vp_MathUtility.NaNSafeVector2(vector, prevVector);
	}

	// Token: 0x0600AD75 RID: 44405 RVA: 0x00408FCF File Offset: 0x004071CF
	[Obsolete("Please use 'vp_MathUtility.NaNSafeVector3' instead.")]
	public static Vector3 NaNSafeVector3(Vector3 vector, Vector3 prevVector = default(Vector3))
	{
		return vp_MathUtility.NaNSafeVector3(vector, prevVector);
	}

	// Token: 0x0600AD76 RID: 44406 RVA: 0x00408FD8 File Offset: 0x004071D8
	[Obsolete("Please use 'vp_MathUtility.NaNSafeQuaternion' instead.")]
	public static Quaternion NaNSafeQuaternion(Quaternion quaternion, Quaternion prevQuaternion = default(Quaternion))
	{
		return vp_MathUtility.NaNSafeQuaternion(quaternion, prevQuaternion);
	}

	// Token: 0x0600AD77 RID: 44407 RVA: 0x00408FE1 File Offset: 0x004071E1
	[Obsolete("Please use 'vp_MathUtility.SnapToZero' instead.")]
	public static Vector3 SnapToZero(Vector3 value, float epsilon = 0.0001f)
	{
		return vp_MathUtility.SnapToZero(value, epsilon);
	}

	// Token: 0x0600AD78 RID: 44408 RVA: 0x00408FEA File Offset: 0x004071EA
	[Obsolete("Please use 'vp_MathUtility.SnapToZero' instead.")]
	public static float SnapToZero(float value, float epsilon = 0.0001f)
	{
		return vp_MathUtility.SnapToZero(value, epsilon);
	}

	// Token: 0x0600AD79 RID: 44409 RVA: 0x00408FF3 File Offset: 0x004071F3
	[Obsolete("Please use 'vp_MathUtility.ReduceDecimals' instead.")]
	public static float ReduceDecimals(float value, float factor = 1000f)
	{
		return vp_MathUtility.ReduceDecimals(value, factor);
	}

	// Token: 0x0600AD7A RID: 44410 RVA: 0x00408FFC File Offset: 0x004071FC
	[Obsolete("Please use 'vp_3DUtility.HorizontalVector' instead.")]
	public static Vector3 HorizontalVector(Vector3 value)
	{
		return vp_3DUtility.HorizontalVector(value);
	}

	// Token: 0x0600AD7B RID: 44411 RVA: 0x00409004 File Offset: 0x00407204
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

	// Token: 0x0600AD7C RID: 44412 RVA: 0x004090DC File Offset: 0x004072DC
	public static string GetTypeAlias(Type type)
	{
		string text = "";
		if (!vp_Utility.m_TypeAliases.TryGetValue(type, out text))
		{
			return type.ToString();
		}
		return text;
	}

	// Token: 0x0600AD7D RID: 44413 RVA: 0x00409106 File Offset: 0x00407306
	public static void Activate(GameObject obj, bool activate = true)
	{
		obj.SetActive(activate);
	}

	// Token: 0x0600AD7E RID: 44414 RVA: 0x0040910F File Offset: 0x0040730F
	public static bool IsActive(GameObject obj)
	{
		return obj.activeSelf;
	}

	// Token: 0x17001449 RID: 5193
	// (get) Token: 0x0600AD7F RID: 44415 RVA: 0x00409117 File Offset: 0x00407317
	// (set) Token: 0x0600AD80 RID: 44416 RVA: 0x00409121 File Offset: 0x00407321
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

	// Token: 0x0600AD81 RID: 44417 RVA: 0x0040913C File Offset: 0x0040733C
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

	// Token: 0x0600AD82 RID: 44418 RVA: 0x00409182 File Offset: 0x00407382
	public static T RandomObject<T>(this List<T> list)
	{
		List<T> list2 = new List<T>();
		list2.AddRange(list);
		list2.RandomizeList<T>();
		return list2.FirstOrDefault<T>();
	}

	// Token: 0x0600AD83 RID: 44419 RVA: 0x0040919B File Offset: 0x0040739B
	public static List<T> ChildComponentsToList<T>(this Transform t) where T : Component
	{
		return t.GetComponentsInChildren<T>().ToList<T>();
	}

	// Token: 0x0600AD84 RID: 44420 RVA: 0x004091A8 File Offset: 0x004073A8
	public static bool IsDescendant(Transform descendant, Transform potentialAncestor)
	{
		return !(descendant == null) && !(potentialAncestor == null) && !(descendant.parent == descendant) && (descendant.parent == potentialAncestor || vp_Utility.IsDescendant(descendant.parent, potentialAncestor));
	}

	// Token: 0x0600AD85 RID: 44421 RVA: 0x004091F7 File Offset: 0x004073F7
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

	// Token: 0x0600AD86 RID: 44422 RVA: 0x00409224 File Offset: 0x00407424
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

	// Token: 0x0600AD87 RID: 44423 RVA: 0x004092E8 File Offset: 0x004074E8
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

	// Token: 0x0600AD88 RID: 44424 RVA: 0x00409389 File Offset: 0x00407589
	public static global::UnityEngine.Object Instantiate(global::UnityEngine.Object original)
	{
		return vp_Utility.Instantiate(original, Vector3.zero, Quaternion.identity);
	}

	// Token: 0x0600AD89 RID: 44425 RVA: 0x0040939B File Offset: 0x0040759B
	public static global::UnityEngine.Object Instantiate(global::UnityEngine.Object original, Vector3 position, Quaternion rotation)
	{
		if (vp_PoolManager.Instance == null || !vp_PoolManager.Instance.enabled)
		{
			return global::UnityEngine.Object.Instantiate(original, position, rotation);
		}
		return vp_GlobalEventReturn<global::UnityEngine.Object, Vector3, Quaternion, global::UnityEngine.Object>.Send("vp_PoolManager Instantiate", original, position, rotation);
	}

	// Token: 0x0600AD8A RID: 44426 RVA: 0x004093CC File Offset: 0x004075CC
	public static void Destroy(global::UnityEngine.Object obj)
	{
		vp_Utility.Destroy(obj, 0f);
	}

	// Token: 0x0600AD8B RID: 44427 RVA: 0x004093D9 File Offset: 0x004075D9
	public static void Destroy(global::UnityEngine.Object obj, float t)
	{
		if (vp_PoolManager.Instance == null || !vp_PoolManager.Instance.enabled)
		{
			global::UnityEngine.Object.Destroy(obj, t);
			return;
		}
		vp_GlobalEvent<global::UnityEngine.Object, float>.Send("vp_PoolManager Destroy", obj, t);
	}

	// Token: 0x1700144A RID: 5194
	// (get) Token: 0x0600AD8C RID: 44428 RVA: 0x00409408 File Offset: 0x00407608
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

	// Token: 0x0600AD8D RID: 44429 RVA: 0x0040945D File Offset: 0x0040765D
	public static void ClearUniqueIDs()
	{
		vp_Utility.m_UniqueIDs.Clear();
	}

	// Token: 0x0600AD8E RID: 44430 RVA: 0x00409469 File Offset: 0x00407669
	public static int PositionToID(Vector3 position)
	{
		return (int)Mathf.Abs(position.x * 10000f + position.y * 1000f + position.z * 100f);
	}

	// Token: 0x0600AD8F RID: 44431 RVA: 0x00409497 File Offset: 0x00407697
	[Obsolete("Please use 'vp_AudioUtility.PlayRandomSound' instead.")]
	public static void PlayRandomSound(AudioSource audioSource, List<AudioClip> sounds, Vector2 pitchRange)
	{
		vp_AudioUtility.PlayRandomSound(audioSource, sounds, pitchRange);
	}

	// Token: 0x0600AD90 RID: 44432 RVA: 0x004094A1 File Offset: 0x004076A1
	[Obsolete("Please use 'vp_AudioUtility.PlayRandomSound' instead.")]
	public static void PlayRandomSound(AudioSource audioSource, List<AudioClip> sounds)
	{
		vp_AudioUtility.PlayRandomSound(audioSource, sounds);
	}

	// Token: 0x04008045 RID: 32837
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

	// Token: 0x04008046 RID: 32838
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<int, int> m_UniqueIDs = new Dictionary<int, int>();
}
