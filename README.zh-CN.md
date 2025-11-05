# TechDashboard - 现代 WPF 仪表板应用程序

[English](README.md) | **简体中文**

[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![WPF](https://img.shields.io/badge/WPF-Windows-blue.svg)](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)

基于 .NET 8 WPF 构建的现代、功能丰富的仪表板应用程序，展示了先进的 UI/UX 模式和 MVVM 架构。

## 📸 界面截图

### 主界面
![主界面](images/main-interface.png)

*主仪表板界面，包含导航面板*

### 主题展示
<div align="center">
  <img src="images/theme-dark.png" alt="深色主题" width="45%">
  <img src="images/theme-light.png" alt="浅色主题" width="45%">
  <img src="images/theme-bluetech.png" alt="蓝色科技主题" width="45%">
</div>

*三种精美主题：深色（黑灰色调）、浅色和蓝色科技*

### 语言支持
![语言支持](images/language-support.png)

*多语言支持，包含 5 种语言：英语、简体中文、繁体中文、韩语和日语*

### 导航面板
![导航面板](images/navigation-panel.png)

*智能导航面板，支持自动宽度计算和拖拽调整大小*

## ✨ 功能特性

### 🎨 现代 UI/UX
- **三种精美主题**：深色（黑灰色调）、浅色和蓝色科技
- **流畅动画**：所有 UI 过渡都使用缓动函数进行动画处理
- **响应式布局**：适配各种屏幕尺寸的自适应设计
- **渐变效果**：整个应用使用精美的渐变和阴影效果

### 🌐 国际化 (i18n)
- **多语言支持**：英语、简体中文（简体中文）、繁体中文（繁體中文）、韩语（한국어）、日语（日本語）
- **动态切换**：无需重启即可即时更改语言
- **基于资源**：通过创建新的资源字典轻松添加更多语言

### 🔄 智能导航
- **可折叠侧边栏**：200ms 流畅展开/折叠动画
- **自动宽度计算**：导航宽度根据最长文本自动调整（包括 DASHBOARD Logo 和所有导航项）
- **语言感知调整**：语言更改时自动重新计算并更新宽度
- **拖拽调整大小**：拖拽导航面板边缘自定义宽度
- **双击展开/折叠**：双击空白区域展开（折叠时）或折叠（展开时）
- **视觉反馈**：悬停效果和选中状态指示器

### 🎯 技术亮点
- **清晰的 MVVM 架构**：适当的关注点分离
- **观察者模式**：使用 `INotifyPropertyChanged` 进行响应式属性更新
- **命令模式**：可复用的 `RelayCommand` 实现
- **主题管理**：使用合并字典进行动态主题切换
- **类型安全资源**：对本地化字符串进行强类型访问

## 📋 环境要求

- **.NET 8 SDK** 或更高版本
- **Windows 10/11**（WPF 仅支持 Windows）
- **Visual Studio 2022**（推荐）或任何 .NET 兼容 IDE

## 🚀 快速开始

### 安装

1. **克隆仓库**
   ```bash
   git clone https://github.com/PerryLuoLab/TechDashboard.git
   cd TechDashboard
   ```

2. **构建项目**
   ```bash
   dotnet restore
   dotnet build
   ```

3. **运行应用程序**
   ```bash
   dotnet run --project TechDashboard.csproj
   ```

### 使用 Visual Studio

1. 在 Visual Studio 2022 中打开 `TechDashboard.sln`
2. 按 `F5` 构建并运行
3. 或使用 `Ctrl+F5` 在无调试模式下运行

## 📁 项目结构

```
TechDashboard/
├── App.xaml                    # 应用程序入口点和资源
├── App.xaml.cs                 # 主题和语言管理逻辑
├── MainWindow.xaml             # 主窗口 UI 定义
├── MainWindow.xaml.cs          # 窗口逻辑和拖拽处理
│
├── Commands/
│   └── RelayCommand.cs         # 通用命令实现
│
├── Converters/
│   └── ThemeConverter.cs        # 主题/语言切换转换器
│
├── Infrastructure/
│   └── ObservableObject.cs     # ViewModel 基类
│
├── ViewModels/
│   └── MainViewModel.cs        # 主窗口 ViewModel
│
├── Themes/
│   ├── DarkTheme.xaml          # 深色主题（黑灰色调）
│   ├── LightTheme.xaml         # 浅色主题
│   └── BlueTechTheme.xaml      # 蓝色科技主题
│
└── Languages/
    ├── en-US.xaml              # 英语资源
    ├── zh-CN.xaml              # 简体中文资源
    ├── zh-TW.xaml              # 繁体中文资源
    ├── ko-KR.xaml              # 韩语资源
    └── ja-JP.xaml              # 日语资源
```

## 🎨 主题定制

### 添加新主题

1. 在 `Themes/` 文件夹中创建新的 XAML 文件（例如，`GreenTheme.xaml`）
2. 定义与现有主题模式匹配的颜色资源：

```xml
<ResourceDictionary>
    <!-- 定义您的颜色 -->
    <Color x:Key="WindowBgColor">#YourColor</Color>
    <Color x:Key="NavBgColor">#YourColor</Color>
    <!-- ... 更多颜色 ... -->
    
    <!-- 创建画笔 -->
    <SolidColorBrush x:Key="NavBackgroundBrush" Color="{StaticResource NavBgColor}"/>
    <!-- ... 更多画笔 ... -->
</ResourceDictionary>
```

3. 在 `MainWindow.xaml` 设置页面添加切换按钮
4. 选择后主题会自动应用

### 关键主题资源

| 资源键 | 描述 |
|-------------|-------------|
| `WindowBackgroundBrush` | 主窗口背景 |
| `NavBackgroundBrush` | 导航面板背景 |
| `CardBackgroundBrush` | 仪表板卡片背景 |
| `TextBrush` | 主要文本颜色 |
| `TextSecondaryBrush` | 次要文本颜色 |
| `AccentBrush` | 强调/高亮颜色 |
| `BorderBrush` | 边框颜色 |

## 🌍 添加新语言

### 分步指南

1. **创建语言资源文件**
   - 从 `Languages/` 文件夹复制现有语言文件
   - 重命名为匹配语言代码（例如，日语使用 `ja-JP.xaml`）

2. **翻译所有字符串资源**
   ```xml
   <ResourceDictionary xmlns:system="clr-namespace:System;assembly=mscorlib">
       <system:String x:Key="Menu_File">ファイル</system:String>
       <system:String x:Key="Menu_Edit">編集</system:String>
       <!-- ... 更多翻译 ... -->
   </ResourceDictionary>
   ```

3. 在 `MainWindow.xaml` 中添加语言选择器按钮：
   ```xml
   <ToggleButton Content="日本語" 
                 Style="{StaticResource LanguageToggleButton}"
                 IsChecked="{Binding CurrentLanguage, 
                            Converter={StaticResource LanguageConverter}, 
                            ConverterParameter=ja-JP}"
                 Command="{Binding ChangeLanguageCommand}" 
                 CommandParameter="ja-JP">
       <ToggleButton.Tag>
           <SolidColorBrush Color="#BC002D"/>
       </ToggleButton.Tag>
   </ToggleButton>
   ```

4. 在 `MainViewModel.cs` 中更新 ViewModel 显示名称映射：
   ```csharp
   public string CurrentLanguageDisplay
   {
       get
       {
           return CurrentLanguage switch
           {
               "en-US" => "English",
               "zh-CN" => "简体中文",
               "ko-KR" => "한국어",
               "ja-JP" => "日本語",  // 添加此行
               _ => "English"
           };
       }
   }
   ```

## 🔧 高级功能

### 导航面板行为

| 操作 | 行为 |
|--------|----------|
| 点击切换按钮 | 流畅的展开/折叠动画 |
| 拖拽面板边缘 | 调整到自定义宽度 |
| 双击（折叠时） | 快速展开 |
| 鼠标悬停边缘 | 显示调整大小光标 |

### 宽度计算

导航宽度根据以下因素自动调整：
- 最长导航项文本长度
- 当前字体大小和字体族
- 图标宽度和内边距
- 配置的边距

公式：`Width = IconWidth + Margin + MaxTextWidth`

### 主题切换逻辑

```
用户点击主题按钮
    ↓
ViewModel.ChangeTheme(themeName)
    ↓
App.ApplyTheme(themeName)
    ↓
移除旧主题 ResourceDictionary
    ↓
加载新主题 ResourceDictionary
    ↓
通过 DynamicResource 绑定自动更新 UI
```

## 🎯 关键实现细节

### 1. 智能宽度计算
```csharp
private void CalculateOptimalNavWidth()
{
    // 使用 FormattedText 测量实际文本宽度
    // 添加图标、内边距和边距
    // 限制在最小值（60）和最大值（350）之间
}
```

### 2. 流畅拖拽处理
```csharp
// 拖拽阈值：100px 中点
// 对齐到折叠（60px）或展开（计算的宽度）
// 动画持续时间：200ms，使用 EaseInOut
```

### 3. 资源管理
- 主题使用 `DynamicResource` 进行热交换
- 语言使用 `system:String` 进行本地化
- 所有资源都适当作用域和类型化

## 🐛 故障排除

### 主题未应用
- 确保主题文件存在于 `Themes/` 文件夹中
- 检查主题和使用之间的资源键是否匹配
- 验证使用了 `DynamicResource`（而非 `StaticResource`）

### 语言未更改
- 确认语言文件存在于 `Languages/` 文件夹中
- 检查是否定义了所有必需的字符串键
- 如果更改未立即显示，请重启应用

### 导航面板问题
- 如果拖拽不工作：检查是否有元素阻止鼠标输入
- 如果宽度不正确：验证计算中的字体大小是否与 UI 匹配
- 如果动画不流畅：确保没有其他动画同时运行

## 📈 性能提示

1. **资源字典**：合并字典加载一次并缓存
2. **动画**：使用 `BeginAnimation` 进行硬件加速变换
3. **绑定**：只读属性使用 OneWay 绑定减少开销
4. **布局**：尽可能设置固定高度以避免不必要的布局传递

## 🤝 贡献

欢迎贡献！请按照以下步骤：

1. Fork 仓库
2. 创建功能分支（`git checkout -b feature/AmazingFeature`）
3. 提交更改（`git commit -m 'Add some AmazingFeature'`）
4. 推送到分支（`git push origin feature/AmazingFeature`）
5. 打开 Pull Request

## 📝 许可证

本项目采用 MIT 许可证 - 查看 [LICENSE](LICENSE) 文件了解详情。

## 🙏 致谢

- Microsoft 的 WPF 团队提供优秀的框架
- Material Design 图标（Segoe MDL2 Assets）
- 社区贡献者的反馈和建议

## 📞 支持

- **Issues**：[GitHub Issues](https://github.com/PerryLuoLab/TechDashboard/issues)
- **Discussions**：[GitHub Discussions](https://github.com/PerryLuoLab/TechDashboard/discussions)
- **Email**：perryluox@yeah.net

---

**使用 ❤️ 和 .NET 8 以及 WPF 制作**
