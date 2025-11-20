# TechDashboard 架构总览

## 1. 高层分层结构

- 表现层（WPF 视图）：MainWindow.xaml 以及主题资源字典 (Themes)
- ViewModel 层：状态 + 命令（MainViewModel）
- 服务层：横切关注点（LocalizationService、ThemeService）
- Core 核心层：可复用构件（Infrastructure、Constants、Converters、Extensions）
- 资源层：本地化 .resx 字典文件

## 2. 模块职责

### Core/Infrastructure
MVVM 基础支持：
- ObservableObject：实现 INotifyPropertyChanged，提供 SetProperty 辅助方法
- RelayCommand：ICommand 实现，用于命令绑定
- GridLengthAnimation：自定义动画类，支持列宽/行高平滑动画

### Core/Constants
集中不可变配置：
- NavigationConstants：导航面板行为相关数值
- ThemeConstants：主题标识、资源路径、资源键
- LanguageConstants：语言代码、显示名称、校验工具
- IconConstants：按语义分组的 Segoe MDL2 图标字形常量
- PageConstants：（新增）页面逻辑名称 + 对应本地化键映射

### Core/Converters
视图绑定转换：
- ThemeConverter / LanguageConverter：将当前主题/语言转换为 ToggleButton 的 IsChecked 布尔值
- BoolToVisibilityConverter：布尔值与 Visibility 互转
- IconConverter：（若使用）图标相关转换

### Core/Extensions
- ServiceCollectionExtensions：依赖注入注册助手（注册主题与本地化服务）

### Services
封装运行期可变行为：
- LocalizationService：封装 WPFLocalizeExtension，负责语言切换、字符串获取与格式化
- ThemeService：通过替换 ResourceDictionary 实现主题切换，并基于 ThemeConstants 校验

### ViewModels
- MainViewModel：聚合 UI 状态（导航、主题、语言、当前页面）；提供命令。除语言刷新使用 Dispatcher 外不引用 UI 类型。

### 表现层（视图 + 主题）
- MainWindow.xaml(.cs)：布局与交互胶水（拖拽导航宽度、动画）；调用 CalculateOptimalNavWidth 计算并执行动画
- Theme *.xaml：各主题资源字典，键与 ThemeConstants.ResourceKeys 对齐

## 3. 数据流

用户交互 → 命令 (RelayCommand) → ViewModel 状态更新 → PropertyChanged → 绑定刷新视图。

语言切换链路：
1. MainViewModel.ChangeLanguage → LocalizationService.ChangeLanguage（切换 Culture）
2. 视图监听 CurrentLanguage 属性变化 → 重新计算导航宽度 → 动画更新列宽

主题切换链路：
1. MainViewModel.ChangeTheme → ThemeService.ApplyTheme → 替换合并字典
2. 动态资源自动刷新；ViewModel 引发 CurrentThemeDisplay 变化

导航展开/折叠：
1. ToggleNavCommand 切换 IsNavExpanded
2. 视图监听属性变化 → AnimateNavWidth → 更新 NavWidth

## 4. 导航宽度逻辑（已更新）
- 使用 FormattedText 测量当前语言下所有导航标签的最大宽度
- 宽度 = 外边距 + 内边距 + 图标 + 间距 + 最长文本 + 6 像素缓冲 (ExpansionExtraBuffer)
- 在加载与语言切换时重新计算

## 5. 常量策略
所有“魔法数字/字符串”提升为常量类，降低维护成本并统一扩展路径。ViewModel 与服务仅依赖常量而非散乱字面值。

## 6. 可扩展性指引
- 新页面：在 PageConstants 中定义常量 + 添加本地化键 + 更新 XAML 按钮绑定
- 新语言：新增 .resx → LanguageConstants 增加 CultureCode 与显示名 → 注册进配置
- 新主题：新增主题 XAML → ThemeConstants 添加名称与路径 → 更新设置界面
- 新图标：修改 IconConstants，对 UI 透明无侵入

## 7. 错误处理与回退
- LocalizationService：缺失键时返回原键并输出调试日志
- ThemeService：加载失败回退默认主题
- 导航宽度：测量失败回退 DefaultExpandedWidth

## 8. 依赖注入
- 通过 ServiceCollectionExtensions 注册 ILocalizationService / IThemeService
- ViewModel 构造函数注入服务提升测试性（可使用模拟对象）

## 9. 测试建议
- PageConstants.GetStatusKey 映射正确性
- LocalizationService 文化切换与异常回退
- ThemeService 未知主题回退逻辑
- 不同语言下导航宽度计算是否符合预期

## 10. 未来改进想法
- 将导航按钮硬编码替换为 ObservableCollection<PageDefinition>
- 持久化用户偏好（主题 / 语言 / 导航宽度）到本地设置文件
- 为导航命令增加 CanExecute 条件（权限 / 状态）
- 引入 ILogger 抽象替换 Debug.WriteLine

---
架构说明（中文版本）。
