# TechDashboard - 现代 WPF 仪表板应用程序

[English](README.md) | **简体中文**

[![.NET](https://img.shields.io/badge/.NET-8.0-purple.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![WPF](https://img.shields.io/badge/WPF-Windows-blue.svg)](https://docs.microsoft.com/en-us/dotnet/desktop/wpf/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)
[![Version](https://img.shields.io/badge/version-1.3-blue.svg)](V1.3_UPDATE_NOTES.zh-CN.md)

基于 .NET 8 WPF 构建的现代、功能丰富的仪表板应用程序，展示先进的 UI/UX 模式和 MVVM 架构。

## 📸 界面截图

### 主题展示
<div align="center">
  <img src="Assets/theme-dark.png" alt="深色主题" width="45%">
  <img src="Assets/theme-light.png" alt="浅色主题" width="45%">
  <img src="Assets/theme-bluetech.png" alt="蓝色科技主题" width="45%">
</div>

### 语言支持
![语言支持](Assets/language-support.png)

*支持 5 种语言：英语、简体中文、繁体中文、韩语、日语*

### 导航面板
![导航面板](Assets/navigation-panel.png)

*智能导航面板：动态宽度计算 + 双击折叠*

## ✨ 功能特性

### 🎨 现代 UI/UX
- 三种主题：深色 / 浅色 / 蓝色科技
- 语义化状态颜色系统 ✨ v1.1
- 统一图标系统 ✨ v1.3（集中式 IconConstants）
- 平滑动画（导航宽度动画、悬停反馈）
- 动态导航宽度 ✨ v1.3：自动适配最长文本 + 6 像素缓冲，语言切换后自动更新

### 🌐 国际化 (i18n)
- 多语言实时切换，无需重启
- 使用 .resx 文件扩展新语言
- LanguageConstants 集中管理语言代码与显示名

### 🔄 智能导航
- 折叠/展开动画 200ms
- 双击空白区域快速折叠/展开
- 动态测量最长标签文字宽度并自适应

### 🧱 架构亮点
- 清晰 MVVM：View 与逻辑分离
- Core 层集中：常量、转换器、基础设施、扩展方法
- 服务层抽象：主题与本地化独立封装
- 消除魔法字符串：PageConstants / ThemeConstants / LanguageConstants / IconConstants

### 🎯 技术要点
- GridLengthAnimation 自定义动画类
- 使用 FormattedText 精准测量文字宽度
- 依赖注入方便测试与扩展
- 常量驱动，减少硬编码维护成本

## 📁 项目结构
```
TechDashboard/
├── App.xaml / App.xaml.cs
├── MainWindow.xaml / MainWindow.xaml.cs
├── Core/
│   ├── Infrastructure/ (ObservableObject, RelayCommand, GridLengthAnimation)
│   ├── Constants/ (Navigation, Theme, Language, Icon, Page)
│   ├── Converters/ (Theme, Language, BoolToVisibility, Icon)
│   └── Extensions/ (ServiceCollectionExtensions)
├── Services/ (LocalizationService, ThemeService + 接口)
├── ViewModels/ (MainViewModel)
├── Themes/ (*.xaml 主题资源)
├── Resources/ (*.resx 多语言文件)
└── ARCHITECTURE.md (架构文档)
```

## 🧩 模块说明
详见架构文档： [ARCHITECTURE.md](ARCHITECTURE.md)

## 🚀 快速开始
```bash
git clone https://github.com/PerryLuoLab/TechDashboard.git
cd TechDashboard
dotnet restore
dotnet run --project TechDashboard.csproj
```

## 🛠 配置与扩展
- 新主题：添加 XAML + 更新 ThemeConstants
- 新语言：添加 resx + 更新 LanguageConstants
- 新页面：扩展 PageConstants + 添加本地化字符串
- 新图标：直接在 IconConstants 中添加，不改动 UI

## 📐 导航宽度逻辑 (v1.3)
1. 使用 FormattedText 测量所有导航标签本地化后的宽度
2. 计算：图标宽 + 间距 + 标签文字宽 + 左右内边距 + 外部边距 + 6px 缓冲
3. 语言切换时重新计算并应用动画

## 🧪 测试建议
- PageConstants.GetStatusKey 映射测试
- 主题加载异常回退测试
- 切换语言后导航宽度自动更新验证

## 🐛 常见问题
- 设计器显示本地化扩展错误：设计时限制，可忽略
- 主题不生效：路径是否与 ThemeConstants 一致
- 语言不切换：确认 resx 文件命名及注册是否正确

## 📝 更新日志
### v1.3 - 2024
- 导航宽度精确动态适配 (+6 像素缓冲)
- 引入 PageConstants，消除页面字符串硬编码
- ThemeService 使用 ThemeConstants 规范化
- 新增 ARCHITECTURE.md 架构文档
- README 文档增强

### v1.2
- Core 层重构与结构优化

### v1.1
- 语义化颜色系统 + 语言与主题常量化

### v1.0
- 初始版本

## 🤝 贡献
欢迎提交 Pull Request。新增标识请添加到对应常量类中。

## 📝 许可证
MIT License（详见 LICENSE）。

## 🙏 致谢
- Microsoft WPF 团队
- 社区优秀实践

**使用 ❤️ 与 .NET 8 + WPF 构建**
