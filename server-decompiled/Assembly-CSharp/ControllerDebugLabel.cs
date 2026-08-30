using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using InControl;
using UnityEngine;

// Token: 0x02001176 RID: 4470
public class ControllerDebugLabel : MonoBehaviour
{
	// Token: 0x06008ED4 RID: 36564 RVA: 0x0035C10C File Offset: 0x0035A30C
	public ControllerDebugLabel()
	{
		this.m_debugStringProviders = new SortedDictionary<string, Action<StringBuilder>>();
		this.m_debugStringProviderNames = new List<string>();
		this.m_debugStringBuilderMain = new StringBuilder(4096);
		this.m_debugStringBuilderForProvider = new StringBuilder(4096);
		this.m_debugStringBuilderForControlsString = new StringBuilder(512);
	}

	// Token: 0x06008ED5 RID: 36565 RVA: 0x0035C199 File Offset: 0x0035A399
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnEnable()
	{
		this.m_label = base.GetComponent<UILabel>();
		this.AddDebugProvider("Key Codes", new Action<StringBuilder>(this.BuildKeyCodeString));
		this.AddDebugProvider("Devices", new Action<StringBuilder>(this.BuildDevicesString));
	}

	// Token: 0x06008ED6 RID: 36566 RVA: 0x0035C1D5 File Offset: 0x0035A3D5
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnDisable()
	{
		this.RemoveDebugProvider("Key Codes");
		this.RemoveDebugProvider("Devices");
	}

	// Token: 0x06008ED7 RID: 36567 RVA: 0x0035C1F0 File Offset: 0x0035A3F0
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

	// Token: 0x06008ED8 RID: 36568 RVA: 0x0035C378 File Offset: 0x0035A578
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateProviderNames()
	{
		this.m_debugStringProviderNames.Clear();
		this.m_debugStringProviderNames.AddRange(this.m_debugStringProviders.Keys);
	}

	// Token: 0x06008ED9 RID: 36569 RVA: 0x0035C39B File Offset: 0x0035A59B
	public void AddDebugProvider(string providerName, Action<StringBuilder> provider)
	{
		bool flag = !this.m_debugStringProviders.ContainsKey(providerName);
		this.m_debugStringProviders[providerName] = provider;
		if (flag)
		{
			this.UpdateProviderNames();
		}
	}

	// Token: 0x06008EDA RID: 36570 RVA: 0x0035C3C1 File Offset: 0x0035A5C1
	public void RemoveDebugProvider(string providerName)
	{
		if (this.m_debugStringProviders.Remove(providerName))
		{
			this.UpdateProviderNames();
		}
	}

	// Token: 0x06008EDB RID: 36571 RVA: 0x0035C3D8 File Offset: 0x0035A5D8
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

	// Token: 0x06008EDC RID: 36572 RVA: 0x0035C428 File Offset: 0x0035A628
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

	// Token: 0x06008EDD RID: 36573 RVA: 0x0035C4A0 File Offset: 0x0035A6A0
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

	// Token: 0x04006983 RID: 27011
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string KEY_DEVICES = "Devices";

	// Token: 0x04006984 RID: 27012
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const string KEY_KEY_CODES = "Key Codes";

	// Token: 0x04006985 RID: 27013
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly SortedDictionary<string, Action<StringBuilder>> m_debugStringProviders;

	// Token: 0x04006986 RID: 27014
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<string> m_debugStringProviderNames;

	// Token: 0x04006987 RID: 27015
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly StringBuilder m_debugStringBuilderMain;

	// Token: 0x04006988 RID: 27016
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly StringBuilder m_debugStringBuilderForProvider;

	// Token: 0x04006989 RID: 27017
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly StringBuilder m_debugStringBuilderForControlsString;

	// Token: 0x0400698A RID: 27018
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public UILabel m_label;

	// Token: 0x0400698B RID: 27019
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly KeyCode[] m_allKeyCodes = (KeyCode[])Enum.GetValues(typeof(KeyCode));

	// Token: 0x0400698C RID: 27020
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly InputControlType[] m_allControls = (InputControlType[])Enum.GetValues(typeof(InputControlType));
}
