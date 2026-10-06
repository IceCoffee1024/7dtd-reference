using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020015A4 RID: 5540
public class vp_Input : MonoBehaviour
{
	// Token: 0x1700142F RID: 5167
	// (get) Token: 0x0600ACF4 RID: 44276 RVA: 0x004072B4 File Offset: 0x004054B4
	public static vp_Input Instance
	{
		get
		{
			if (vp_Input.mIsDirty)
			{
				vp_Input.mIsDirty = false;
				if (vp_Input.m_Instance == null)
				{
					if (Application.isPlaying)
					{
						GameObject gameObject = Resources.Load("Input/vp_Input") as GameObject;
						if (gameObject == null)
						{
							vp_Input.m_Instance = new GameObject("vp_Input").AddComponent<vp_Input>();
						}
						else
						{
							vp_Input.m_Instance = gameObject.GetComponent<vp_Input>();
							if (vp_Input.m_Instance == null)
							{
								vp_Input.m_Instance = gameObject.AddComponent<vp_Input>();
							}
						}
					}
					vp_Input.m_Instance.SetupDefaults("");
				}
			}
			return vp_Input.m_Instance;
		}
	}

	// Token: 0x0600ACF5 RID: 44277 RVA: 0x000027FC File Offset: 0x000009FC
	public static void CreateIfNoExist()
	{
	}

	// Token: 0x0600ACF6 RID: 44278 RVA: 0x00407348 File Offset: 0x00405548
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void Awake()
	{
		if (vp_Input.m_Instance == null)
		{
			vp_Input.m_Instance = vp_Input.Instance;
		}
	}

	// Token: 0x0600ACF7 RID: 44279 RVA: 0x00407361 File Offset: 0x00405561
	public virtual void SetDirty(bool dirty)
	{
		vp_Input.mIsDirty = dirty;
	}

	// Token: 0x0600ACF8 RID: 44280 RVA: 0x0040736C File Offset: 0x0040556C
	public virtual void SetupDefaults(string type = "")
	{
		if ((type == "" || type == "Buttons") && this.ButtonKeys.Count == 0)
		{
			this.AddButton("Attack", KeyCode.Mouse0);
			this.AddButton("SetNextWeapon", KeyCode.E);
			this.AddButton("SetPrevWeapon", KeyCode.Q);
			this.AddButton("ClearWeapon", KeyCode.Backspace);
			this.AddButton("Zoom", KeyCode.Mouse1);
			this.AddButton("Reload", KeyCode.R);
			this.AddButton("Jump", KeyCode.Space);
			this.AddButton("Crouch", KeyCode.C);
			this.AddButton("Run", KeyCode.LeftShift);
			this.AddButton("Interact", KeyCode.F);
			this.AddButton("Accept1", KeyCode.Return);
			this.AddButton("Accept2", KeyCode.KeypadEnter);
			this.AddButton("Pause", KeyCode.P);
			this.AddButton("Menu", KeyCode.Escape);
		}
		if ((type == "" || type == "Axis") && this.AxisKeys.Count == 0)
		{
			this.AddAxis("Vertical", KeyCode.W, KeyCode.S);
			this.AddAxis("Horizontal", KeyCode.D, KeyCode.A);
		}
		if ((type == "" || type == "UnityAxis") && this.UnityAxis.Count == 0)
		{
			this.AddUnityAxis("Mouse X");
			this.AddUnityAxis("Mouse Y");
		}
		this.UpdateDictionaries();
	}

	// Token: 0x0600ACF9 RID: 44281 RVA: 0x004074F0 File Offset: 0x004056F0
	public virtual void AddButton(string n, KeyCode k = KeyCode.None)
	{
		if (this.ButtonKeys.Contains(n))
		{
			this.ButtonValues[this.ButtonKeys.IndexOf(n)] = k;
			return;
		}
		this.ButtonKeys.Add(n);
		this.ButtonValues.Add(k);
	}

	// Token: 0x0600ACFA RID: 44282 RVA: 0x0040753C File Offset: 0x0040573C
	public virtual void AddAxis(string n, KeyCode pk = KeyCode.None, KeyCode nk = KeyCode.None)
	{
		if (this.AxisKeys.Contains(n))
		{
			this.AxisValues[this.AxisKeys.IndexOf(n)] = new vp_Input.vp_InputAxis
			{
				Positive = pk,
				Negative = nk
			};
			return;
		}
		this.AxisKeys.Add(n);
		this.AxisValues.Add(new vp_Input.vp_InputAxis
		{
			Positive = pk,
			Negative = nk
		});
	}

	// Token: 0x0600ACFB RID: 44283 RVA: 0x004075AC File Offset: 0x004057AC
	public virtual void AddUnityAxis(string n)
	{
		if (this.UnityAxis.Contains(n))
		{
			this.UnityAxis[this.UnityAxis.IndexOf(n)] = n;
			return;
		}
		this.UnityAxis.Add(n);
	}

	// Token: 0x0600ACFC RID: 44284 RVA: 0x004075E4 File Offset: 0x004057E4
	public virtual void UpdateDictionaries()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		this.Buttons.Clear();
		for (int i = 0; i < this.ButtonKeys.Count; i++)
		{
			this.Buttons.Add(this.ButtonKeys[i], this.ButtonValues[i]);
		}
		this.Axis.Clear();
		for (int j = 0; j < this.AxisKeys.Count; j++)
		{
			this.Axis.Add(this.AxisKeys[j], new vp_Input.vp_InputAxis
			{
				Positive = this.AxisValues[j].Positive,
				Negative = this.AxisValues[j].Negative
			});
		}
	}

	// Token: 0x0600ACFD RID: 44285 RVA: 0x004076A8 File Offset: 0x004058A8
	public static bool GetButtonAny(string button)
	{
		return vp_Input.Instance.DoGetButtonAny(button);
	}

	// Token: 0x0600ACFE RID: 44286 RVA: 0x004076B8 File Offset: 0x004058B8
	public virtual bool DoGetButtonAny(string button)
	{
		if (this.Buttons.ContainsKey(button))
		{
			return Input.GetKey(this.Buttons[button]) || Input.GetKeyDown(this.Buttons[button]) || Input.GetKeyUp(this.Buttons[button]);
		}
		Debug.LogError("\"" + button + "\" is not in VP Input Manager's Buttons. You must add it for this Button to work.");
		return false;
	}

	// Token: 0x0600ACFF RID: 44287 RVA: 0x00407723 File Offset: 0x00405923
	public static bool GetButton(string button)
	{
		return vp_Input.Instance.DoGetButton(button);
	}

	// Token: 0x0600AD00 RID: 44288 RVA: 0x00407730 File Offset: 0x00405930
	public virtual bool DoGetButton(string button)
	{
		if (this.Buttons.ContainsKey(button))
		{
			return Input.GetKey(this.Buttons[button]);
		}
		Debug.LogError("\"" + button + "\" is not in VP Input Manager's Buttons. You must add it for this Button to work.");
		return false;
	}

	// Token: 0x0600AD01 RID: 44289 RVA: 0x00407768 File Offset: 0x00405968
	public static bool GetButtonDown(string button)
	{
		return vp_Input.Instance.DoGetButtonDown(button);
	}

	// Token: 0x0600AD02 RID: 44290 RVA: 0x00407775 File Offset: 0x00405975
	public virtual bool DoGetButtonDown(string button)
	{
		if (this.Buttons.ContainsKey(button))
		{
			return Input.GetKeyDown(this.Buttons[button]);
		}
		Debug.LogError("\"" + button + "\" is not in VP Input Manager's Buttons. You must add it for this Button to work.");
		return false;
	}

	// Token: 0x0600AD03 RID: 44291 RVA: 0x004077AD File Offset: 0x004059AD
	public static bool GetButtonUp(string button)
	{
		return vp_Input.Instance.DoGetButtonUp(button);
	}

	// Token: 0x0600AD04 RID: 44292 RVA: 0x004077BA File Offset: 0x004059BA
	public virtual bool DoGetButtonUp(string button)
	{
		if (this.Buttons.ContainsKey(button))
		{
			return Input.GetKeyUp(this.Buttons[button]);
		}
		Debug.LogError("\"" + button + "\" is not in VP Input Manager's Buttons. You must add it for this Button to work.");
		return false;
	}

	// Token: 0x0600AD05 RID: 44293 RVA: 0x004077F2 File Offset: 0x004059F2
	public static float GetAxisRaw(string axis)
	{
		return vp_Input.Instance.DoGetAxisRaw(axis);
	}

	// Token: 0x0600AD06 RID: 44294 RVA: 0x00407800 File Offset: 0x00405A00
	public virtual float DoGetAxisRaw(string axis)
	{
		if (this.Axis.ContainsKey(axis) && this.ControlType == 0)
		{
			float num = 0f;
			if (Input.GetKey(this.Axis[axis].Positive))
			{
				num = 1f;
			}
			if (Input.GetKey(this.Axis[axis].Negative))
			{
				num = -1f;
			}
			return num;
		}
		if (this.UnityAxis.Contains(axis))
		{
			return Input.GetAxisRaw(axis);
		}
		Debug.LogError("\"" + axis + "\" is not in VP Input Manager's Unity Axis. You must add it for this Axis to work.");
		return 0f;
	}

	// Token: 0x0600AD07 RID: 44295 RVA: 0x00407898 File Offset: 0x00405A98
	public static void ChangeButtonKey(string button, KeyCode keyCode, bool save = false)
	{
		if (!vp_Input.Instance.Buttons.ContainsKey(button))
		{
			Debug.LogWarning("The Button \"" + button + "\" Doesn't Exist");
			return;
		}
		if (save)
		{
			vp_Input.Instance.ButtonValues[vp_Input.Instance.ButtonKeys.IndexOf(button)] = keyCode;
		}
		vp_Input.Instance.Buttons[button] = keyCode;
	}

	// Token: 0x0600AD08 RID: 44296 RVA: 0x00407904 File Offset: 0x00405B04
	public static void ChangeAxis(string n, KeyCode pk = KeyCode.None, KeyCode nk = KeyCode.None, bool save = false)
	{
		if (!vp_Input.Instance.AxisKeys.Contains(n))
		{
			Debug.LogWarning("The Axis \"" + n + "\" Doesn't Exist");
			return;
		}
		if (save)
		{
			vp_Input.Instance.AxisValues[vp_Input.Instance.AxisKeys.IndexOf(n)] = new vp_Input.vp_InputAxis
			{
				Positive = pk,
				Negative = nk
			};
		}
		vp_Input.Instance.Axis[n] = new vp_Input.vp_InputAxis
		{
			Positive = pk,
			Negative = nk
		};
	}

	// Token: 0x04007FF4 RID: 32756
	public int ControlType;

	// Token: 0x04007FF5 RID: 32757
	public Dictionary<string, KeyCode> Buttons = new Dictionary<string, KeyCode>();

	// Token: 0x04007FF6 RID: 32758
	public List<string> ButtonKeys = new List<string>();

	// Token: 0x04007FF7 RID: 32759
	public List<KeyCode> ButtonValues = new List<KeyCode>();

	// Token: 0x04007FF8 RID: 32760
	public Dictionary<string, vp_Input.vp_InputAxis> Axis = new Dictionary<string, vp_Input.vp_InputAxis>();

	// Token: 0x04007FF9 RID: 32761
	public List<string> AxisKeys = new List<string>();

	// Token: 0x04007FFA RID: 32762
	public List<vp_Input.vp_InputAxis> AxisValues = new List<vp_Input.vp_InputAxis>();

	// Token: 0x04007FFB RID: 32763
	public List<string> UnityAxis = new List<string>();

	// Token: 0x04007FFC RID: 32764
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static string m_FolderPath = "UltimateFPS/Content/Resources/Input";

	// Token: 0x04007FFD RID: 32765
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static string m_PrefabPath = "Assets/UltimateFPS/Content/Resources/Input/vp_Input.prefab";

	// Token: 0x04007FFE RID: 32766
	public static bool mIsDirty = true;

	// Token: 0x04007FFF RID: 32767
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public static vp_Input m_Instance;

	// Token: 0x020015A5 RID: 5541
	[Serializable]
	public class vp_InputAxis
	{
		// Token: 0x04008000 RID: 32768
		public KeyCode Positive;

		// Token: 0x04008001 RID: 32769
		public KeyCode Negative;
	}
}
