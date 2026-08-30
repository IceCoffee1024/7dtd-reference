using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

// Token: 0x02001593 RID: 5523
public static class vp_Utility
{
	// Token: 0x0600ACC0 RID: 44224 RVA: 0x00404169 File Offset: 0x00402369
	[Obsolete("Please use 'vp_MathUtility.NaNSafeFloat' instead.")]
	public static float NaNSafeFloat(float value, float prevValue = 0f)
	{
		return vp_MathUtility.NaNSafeFloat(value, prevValue);
	}

	// Token: 0x0600ACC1 RID: 44225 RVA: 0x00404172 File Offset: 0x00402372
	[Obsolete("Please use 'vp_MathUtility.NaNSafeVector2' instead.")]
	public static Vector2 NaNSafeVector2(Vector2 vector, Vector2 prevVector = default(Vector2))
	{
		return vp_MathUtility.NaNSafeVector2(vector, prevVector);
	}

	// Token: 0x0600ACC2 RID: 44226 RVA: 0x0040417B File Offset: 0x0040237B
	[Obsolete("Please use 'vp_MathUtility.NaNSafeVector3' instead.")]
	public static Vector3 NaNSafeVector3(Vector3 vector, Vector3 prevVector = default(Vector3))
	{
		return vp_MathUtility.NaNSafeVector3(vector, prevVector);
	}

	// Token: 0x0600ACC3 RID: 44227 RVA: 0x00404184 File Offset: 0x00402384
	[Obsolete("Please use 'vp_MathUtility.NaNSafeQuaternion' instead.")]
	public static Quaternion NaNSafeQuaternion(Quaternion quaternion, Quaternion prevQuaternion = default(Quaternion))
	{
		return vp_MathUtility.NaNSafeQuaternion(quaternion, prevQuaternion);
	}

	// Token: 0x0600ACC4 RID: 44228 RVA: 0x0040418D File Offset: 0x0040238D
	[Obsolete("Please use 'vp_MathUtility.SnapToZero' instead.")]
	public static Vector3 SnapToZero(Vector3 value, float epsilon = 0.0001f)
	{
		return vp_MathUtility.SnapToZero(value, epsilon);
	}

	// Token: 0x0600ACC5 RID: 44229 RVA: 0x00404196 File Offset: 0x00402396
	[Obsolete("Please use 'vp_MathUtility.SnapToZero' instead.")]
	public static float SnapToZero(float value, float epsilon = 0.0001f)
	{
		return vp_MathUtility.SnapToZero(value, epsilon);
	}

	// Token: 0x0600ACC6 RID: 44230 RVA: 0x0040419F File Offset: 0x0040239F
	[Obsolete("Please use 'vp_MathUtility.ReduceDecimals' instead.")]
	public static float ReduceDecimals(float value, float factor = 1000f)
	{
		return vp_MathUtility.ReduceDecimals(value, factor);
	}

	// Token: 0x0600ACC7 RID: 44231 RVA: 0x004041A8 File Offset: 0x004023A8
	[Obsolete("Please use 'vp_3DUtility.HorizontalVector' instead.")]
	public static Vector3 HorizontalVector(Vector3 value)
	{
		return vp_3DUtility.HorizontalVector(value);
	}

	// Token: 0x0600ACC8 RID: 44232 RVA: 0x004041B0 File Offset: 0x004023B0
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

	// Token: 0x0600ACC9 RID: 44233 RVA: 0x00404288 File Offset: 0x00402488
	public static string GetTypeAlias(Type type)
	{
		string text = "";
		if (!vp_Utility.m_TypeAliases.TryGetValue(type, out text))
		{
			return type.ToString();
		}
		return text;
	}

	// Token: 0x0600ACCA RID: 44234 RVA: 0x004042B2 File Offset: 0x004024B2
	public static void Activate(GameObject obj, bool activate = true)
	{
		obj.SetActive(activate);
	}

	// Token: 0x0600ACCB RID: 44235 RVA: 0x004042BB File Offset: 0x004024BB
	public static bool IsActive(GameObject obj)
	{
		return obj.activeSelf;
	}

	// Token: 0x17001404 RID: 5124
	// (get) Token: 0x0600ACCC RID: 44236 RVA: 0x004042C3 File Offset: 0x004024C3
	// (set) Token: 0x0600ACCD RID: 44237 RVA: 0x004042CD File Offset: 0x004024CD
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

	// Token: 0x0600ACCE RID: 44238 RVA: 0x004042E8 File Offset: 0x004024E8
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

	// Token: 0x0600ACCF RID: 44239 RVA: 0x0040432E File Offset: 0x0040252E
	public static T RandomObject<T>(this List<T> list)
	{
		List<T> list2 = new List<T>();
		list2.AddRange(list);
		list2.RandomizeList<T>();
		return list2.FirstOrDefault<T>();
	}

	// Token: 0x0600ACD0 RID: 44240 RVA: 0x00404347 File Offset: 0x00402547
	public static List<T> ChildComponentsToList<T>(this Transform t) where T : Component
	{
		return t.GetComponentsInChildren<T>().ToList<T>();
	}

	// Token: 0x0600ACD1 RID: 44241 RVA: 0x00404354 File Offset: 0x00402554
	public static bool IsDescendant(Transform descendant, Transform potentialAncestor)
	{
		return !(descendant == null) && !(potentialAncestor == null) && !(descendant.parent == descendant) && (descendant.parent == potentialAncestor || vp_Utility.IsDescendant(descendant.parent, potentialAncestor));
	}

	// Token: 0x0600ACD2 RID: 44242 RVA: 0x004043A3 File Offset: 0x004025A3
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

	// Token: 0x0600ACD3 RID: 44243 RVA: 0x004043D0 File Offset: 0x004025D0
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

	// Token: 0x0600ACD4 RID: 44244 RVA: 0x00404494 File Offset: 0x00402694
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

	// Token: 0x0600ACD5 RID: 44245 RVA: 0x00404535 File Offset: 0x00402735
	public static global::UnityEngine.Object Instantiate(global::UnityEngine.Object original)
	{
		return vp_Utility.Instantiate(original, Vector3.zero, Quaternion.identity);
	}

	// Token: 0x0600ACD6 RID: 44246 RVA: 0x00404547 File Offset: 0x00402747
	public static global::UnityEngine.Object Instantiate(global::UnityEngine.Object original, Vector3 position, Quaternion rotation)
	{
		if (vp_PoolManager.Instance == null || !vp_PoolManager.Instance.enabled)
		{
			return global::UnityEngine.Object.Instantiate(original, position, rotation);
		}
		return vp_GlobalEventReturn<global::UnityEngine.Object, Vector3, Quaternion, global::UnityEngine.Object>.Send("vp_PoolManager Instantiate", original, position, rotation);
	}

	// Token: 0x0600ACD7 RID: 44247 RVA: 0x00404578 File Offset: 0x00402778
	public static void Destroy(global::UnityEngine.Object obj)
	{
		vp_Utility.Destroy(obj, 0f);
	}

	// Token: 0x0600ACD8 RID: 44248 RVA: 0x00404585 File Offset: 0x00402785
	public static void Destroy(global::UnityEngine.Object obj, float t)
	{
		if (vp_PoolManager.Instance == null || !vp_PoolManager.Instance.enabled)
		{
			global::UnityEngine.Object.Destroy(obj, t);
			return;
		}
		vp_GlobalEvent<global::UnityEngine.Object, float>.Send("vp_PoolManager Destroy", obj, t);
	}

	// Token: 0x17001405 RID: 5125
	// (get) Token: 0x0600ACD9 RID: 44249 RVA: 0x004045B4 File Offset: 0x004027B4
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

	// Token: 0x0600ACDA RID: 44250 RVA: 0x00404609 File Offset: 0x00402809
	public static void ClearUniqueIDs()
	{
		vp_Utility.m_UniqueIDs.Clear();
	}

	// Token: 0x0600ACDB RID: 44251 RVA: 0x00404615 File Offset: 0x00402815
	public static int PositionToID(Vector3 position)
	{
		return (int)Mathf.Abs(position.x * 10000f + position.y * 1000f + position.z * 100f);
	}

	// Token: 0x0600ACDC RID: 44252 RVA: 0x00404643 File Offset: 0x00402843
	[Obsolete("Please use 'vp_AudioUtility.PlayRandomSound' instead.")]
	public static void PlayRandomSound(AudioSource audioSource, List<AudioClip> sounds, Vector2 pitchRange)
	{
		vp_AudioUtility.PlayRandomSound(audioSource, sounds, pitchRange);
	}

	// Token: 0x0600ACDD RID: 44253 RVA: 0x0040464D File Offset: 0x0040284D
	[Obsolete("Please use 'vp_AudioUtility.PlayRandomSound' instead.")]
	public static void PlayRandomSound(AudioSource audioSource, List<AudioClip> sounds)
	{
		vp_AudioUtility.PlayRandomSound(audioSource, sounds);
	}

	// Token: 0x04007FC2 RID: 32706
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

	// Token: 0x04007FC3 RID: 32707
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<int, int> m_UniqueIDs = new Dictionary<int, int>();
}
