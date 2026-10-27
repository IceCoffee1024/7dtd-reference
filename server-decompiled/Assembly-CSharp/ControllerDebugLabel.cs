using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using InControl;
using UnityEngine;

// Token: 0x020011B9 RID: 4537
public class ControllerDebugLabel : MonoBehaviour
{
	// Token: 0x060090AE RID: 37038 RVA: 0x00367608 File Offset: 0x00365808
	public ControllerDebugLabel()
	{
		this.m_debugStringProviders = new SortedDictionary<string, Action<StringBuilder>>();
		this.m_debugStringProviderNames = new List<string>();
		this.m_debugStringBuilderMain = new StringBuilder(4096);
		this.m_debugStringBuilderForProvider = new StringBuilder(4096);
		this.m_debugStringBuilderForControlsString = new StringBuilder(512);
	}

	// Token: 0x060090AF RID: 37039 RVA: 0x00367695 File Offset: 0x00365895
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.m_label = base.GetComponent<UILabel>();
		this.AddDebugProvider("Key Codes", new Action<StringBuilder>(this.BuildKeyCodeString));
		this.AddDebugProvider("Devices", new Action<StringBuilder>(this.BuildDevicesString));
	}

	// Token: 0x060090B0 RID: 37040 RVA: 0x003676D1 File Offset: 0x003658D1
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.RemoveDebugProvider("Key Codes");
		this.RemoveDebugProvider("Devices");
	}

	// Token: 0x060090B1 RID: 37041 RVA: 0x003676EC File Offset: 0x003658EC
	[PublicizedFrom(EAccessModifier.Private)]
	public void Update()
	{
		if (this.m_label == null)
		{
			return;
		}
		this.m_debugStringBuilderMain.Clear();
		for (int i = 0; i < this.m_debugStringProviderNames.Count; i++)
		{
			string text = this.m_debugStringProviderNames[i];
			Action<StringBuilder> action = this.m_debugStringProviders[text];
			this.m_debugStringBuilderForProvider.Clear();
			try
			{
				action(this.m_debugStringBuilderForProvider);
			}
			catch (Exception ex)
			{
				this.m_debugStringBuilderForProvider.Clear();
				this.m_debugStringBuilderForProvider.Append(ex.Message);
			}
			if (this.m_debugStringBuilderForProvider.Length != 0)
			{
				if (this.m_debugStringBuilderMain.Length != 0)
				{
					this.m_debugStringBuilderMain.Append('\n');
				}
				this.m_debugStringBuilderMain.Append(text);
				this.m_debugStringBuilderMain.Append(": ");
				for (int j = 0; j < this.m_debugStringBuilderForProvider.Length; j++)
				{
					char c = this.m_debugStringBuilderForProvider[j];
					this.m_debugStringBuilderMain.Append(c);
					if (c == '\n')
					{
						for (int k = 0; k < text.Length + 2; k++)
						{
							this.m_debugStringBuilderMain.Append(' ');
						}
					}
				}
			}
		}
		if (!this.m_debugStringBuilderMain.Equals(this.m_label.text))
		{
			this.m_label.text = this.m_debugStringBuilderMain.ToString();
		}
	}

	// Token: 0x060090B2 RID: 37042 RVA: 0x00367874 File Offset: 0x00365A74
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateProviderNames()
	{
		this.m_debugStringProviderNames.Clear();
		this.m_debugStringProviderNames.AddRange(this.m_debugStringProviders.Keys);
	}

	// Token: 0x060090B3 RID: 37043 RVA: 0x00367897 File Offset: 0x00365A97
	public void AddDebugProvider(string providerName, Action<StringBuilder> provider)
	{
		bool flag = !this.m_debugStringProviders.ContainsKey(providerName);
		this.m_debugStringProviders[providerName] = provider;
		if (flag)
		{
			this.UpdateProviderNames();
		}
	}

	// Token: 0x060090B4 RID: 37044 RVA: 0x003678BD File Offset: 0x00365ABD
	public void RemoveDebugProvider(string providerName)
	{
		if (this.m_debugStringProviders.Remove(providerName))
		{
			this.UpdateProviderNames();
		}
	}

	// Token: 0x060090B5 RID: 37045 RVA: 0x003678D4 File Offset: 0x00365AD4
	[PublicizedFrom(EAccessModifier.Private)]
	public void BuildKeyCodeString(StringBuilder builder)
	{
		foreach (KeyCode keyCode in this.m_allKeyCodes)
		{
			if (Input.GetKey(keyCode))
			{
				if (builder.Length != 0)
				{
					builder.Append(" + ");
				}
				builder.Append(keyCode.ToStringCached<KeyCode>());
			}
		}
	}

	// Token: 0x060090B6 RID: 37046 RVA: 0x00367924 File Offset: 0x00365B24
	[PublicizedFrom(EAccessModifier.Private)]
	public void BuildDevicesString(StringBuilder builder)
	{
		ReadOnlyCollection<InputDevice> devices = InputManager.Devices;
		for (int i = 0; i < devices.Count; i++)
		{
			InputDevice inputDevice = devices[i];
			if (inputDevice.IsActive)
			{
				this.m_debugStringBuilderForControlsString.Clear();
				this.BuildControlsString(this.m_debugStringBuilderForControlsString, inputDevice);
				if (this.m_debugStringBuilderForControlsString.Length != 0)
				{
					if (builder.Length != 0)
					{
						builder.Append('\n');
					}
					builder.Append(this.m_debugStringBuilderForControlsString);
				}
			}
		}
	}

	// Token: 0x060090B7 RID: 37047 RVA: 0x0036799C File Offset: 0x00365B9C
	[PublicizedFrom(EAccessModifier.Private)]
	public void BuildControlsString(StringBuilder builder, InputDevice device)
	{
		bool flag = false;
		builder.Append(device.Name);
		builder.Append(" (");
		builder.Append(device.Meta);
		builder.Append("): ");
		foreach (InputControlType inputControlType in this.m_allControls)
		{
			if (inputControlType != InputControlType.None && inputControlType != InputControlType.Count)
			{
				InputControl control = device.GetControl(inputControlType);
				if (control.IsPressed || control.RawValue != 0f)
				{
					flag = true;
					if (builder.Length != 0)
					{
						builder.Append(" + ");
					}
					builder.Append(inputControlType.ToStringCached<InputControlType>());
					if (control.IsAnalog)
					{
						builder.AppendFormat("={0:F4}", control.RawValue);
					}
				}
			}
		}
		if (!flag)
		{
			builder.Clear();
		}
	}

	// Token: 0x04006AF8 RID: 27384
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string KEY_DEVICES = "Devices";

	// Token: 0x04006AF9 RID: 27385
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string KEY_KEY_CODES = "Key Codes";

	// Token: 0x04006AFA RID: 27386
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly SortedDictionary<string, Action<StringBuilder>> m_debugStringProviders;

	// Token: 0x04006AFB RID: 27387
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<string> m_debugStringProviderNames;

	// Token: 0x04006AFC RID: 27388
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly StringBuilder m_debugStringBuilderMain;

	// Token: 0x04006AFD RID: 27389
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly StringBuilder m_debugStringBuilderForProvider;

	// Token: 0x04006AFE RID: 27390
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly StringBuilder m_debugStringBuilderForControlsString;

	// Token: 0x04006AFF RID: 27391
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UILabel m_label;

	// Token: 0x04006B00 RID: 27392
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly KeyCode[] m_allKeyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));

	// Token: 0x04006B01 RID: 27393
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly InputControlType[] m_allControls = (InputControlType[])Enum.GetValues(typeof(InputControlType));
}
