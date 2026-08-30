# 7DTD Reference（中文）

英文文档：[README.md](README.md)

## 仓库用途

本私有仓库用于分析《7 Days to Die》专用服务器并开发 7DPanel。反编译源码和运行时文件是兼容性分析证据；共享历史资源仅用于视觉参考。

## 目录结构

仓库只保留最新版本，参考文件位于根目录；历史版本通过 Git 托管平台的 Release 资产保存：

```text
7dtd-reference/
|-- README.md
|-- README.zh-CN.md
|-- metadata.json
|-- assets/
|   `-- Sprites/
|-- scripts/
|   |-- Set-DecompiledTargetFramework.ps1
|   |-- Verify-Reference.ps1
|   `-- New-ReferenceRelease.ps1
|-- server-decompiled/
`-- runtime/
    |-- 7DaysToDieServer_Data/
    |-- Mods/
    |-- MonoBleedingEdge/
    |-- serverconfig.xml
    `-- startdedicated.bat
```

不要混用不同游戏构建的文件。当前版本、构建号、提取工具和归档哈希记录在 `metadata.json` 中，并使用同名 Release tag 标识。

## 运行时参考文件

`runtime/` 下的文件应保持官方 7DTD Dedicated Server 分发内容不变。`serverconfig.xml` 和 `startdedicated.bat` 是原始分发文件，不是贡献者服务器的运行配置。

本地或远程测试时，请将 `serverconfig.xml` 复制到仓库外，再在副本中设置服务器名称、路径、凭据、端口和游戏参数。运行时参考文件不得包含在 7DPanel 发布包中。

## 共享资源与访问边界

`assets/` 保存跨版本稳定的视觉参考资源，不能证明特定服务器构建的兼容性，也不得作为运行时或 API 兼容性证据。仓库应保持私有，仅向授权项目贡献者开放。私有可见性不自动授予复制、托管、分发、逆向工程或反编译游戏内容的权利。参见[官方 EULA](https://7daystodie.com/eula/)。

## 反编译项目目标框架

dnSpy 生成的项目可能以 `net40` 为目标框架。重定向脚本会将根目录 `server-decompiled/` 下的项目更新到 .NET Framework 4.8，并跳过多目标或无法识别目标框架的项目。

预览变更：

```powershell
pwsh ./scripts/Set-DecompiledTargetFramework.ps1 -WhatIf
```

应用变更：

```powershell
pwsh ./scripts/Set-DecompiledTargetFramework.ps1
```

## 前置条件与兼容性

- 推荐使用 PowerShell 7。
- 编译需要 .NET Framework 4.8 Developer Pack、当前 .NET SDK 和 MSBuild。
- Windows 是 .NET Framework 4.8 项目的支持构建环境。

重定向只改变编辑器和编译器引用行为，不会改变原始游戏程序集，也不能证明游戏 Mono 支持所有 .NET Framework 4.8 API。要在服务器中运行的代码必须针对 `runtime/7DaysToDieServer_Data/Managed` 下的实际程序集，并在游戏 Mono 进程中验证。

## 检查

```powershell
pwsh ./scripts/Verify-Reference.ps1
```

该脚本检查项目目标框架和 `.csproj` 中 `HintPath` 指向的文件是否存在。

## 最新版本与 Release 归档

替换当前版本前，先生成完整归档和 SHA-256：

```powershell
pwsh ./scripts/Verify-Reference.ps1
pwsh ./scripts/New-ReferenceRelease.ps1 -Version v3.0.1-b4
```

将以下文件上传到同名 Release tag：

```text
dist/7dtd-reference-v3.0.1-b4.zip
dist/7dtd-reference-v3.0.1-b4.zip.sha256
```

更新新版本时，替换根目录的 `server-decompiled/` 和 `runtime/`，更新 `metadata.json`，执行校验后提交并推送 `main`。不要在 Git 中保留旧版本目录或 tag；历史版本通过 Release 资产保存。

普通提交删除文件后，旧文件仍会存在于 Git 历史中。转换为本策略时需要一次性重写历史，并在推送重写后的 `main` 前备份原仓库。远程更新应使用 `--force-with-lease`，并提前通知所有贡献者。
