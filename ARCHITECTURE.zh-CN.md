# TechDashboard 架构详解（中文版）

## 1. 高层架构分层

```
┌─────────────────────────────────────────────────────────────┐
│                     表现层 (Presentation)                    │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │ MainWindow   │  │ 主题 XAML    │  │ 资源字典     │      │
│  │ .xaml / .cs  │  │ 样式文件     │  │ .resx 文件   │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
└─────────────────────────────────────────────────────────────┘
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                     ViewModel 层                             │
│  ┌──────────────────────────────────────────────────────┐   │
│  │ MainViewModel (状态 + 命令)                          │   │
│  │ - 导航状态、主题、语言、当前页面                    │   │
│  └──────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────┘
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                      服务层 (Services)                       │
│  ┌──────────────────┐         ┌──────────────────┐         │
│  │ LocalizationSvc  │         │  ThemeService    │         │
│  │ (文化管理)       │         │  (主题加载)      │         │
│  └──────────────────┘         └──────────────────┘         │
└─────────────────────────────────────────────────────────────┘
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                      核心层 (Core)                           │
│  ┌──────────────┐ ┌──────────────┐ ┌──────────────┐        │
│  │Infrastructure│ │  Constants   │ │  Converters  │        │
│  │(MVVM 基类)   │ │ (常量配置)   │ │  (绑定转换)  │        │
│  └──────────────┘ └──────────────┘ └──────────────┘        │
│                   ┌──────────────┐                          │
│                   │  Extensions  │                          │
│                   │  (DI 配置)   │                          │
│                   └──────────────┘                          │
└─────────────────────────────────────────────────────────────┘
```

## 2. 模块详解与职责

### 2.1 Core/Infrastructure (MVVM 基础)
**用途**: 提供 MVVM 模式可复用基础抽象

| 类名 | 职责 | 关键方法 |
|------|------|---------|
| `ObservableObject` | ViewModel 基类，实现 INotifyPropertyChanged | `SetProperty<T>()`, `RaisePropertyChanged()` |
| `RelayCommand` | ICommand 实现，用于 UI 命令绑定 | `Execute()`, `CanExecute()` |
| `GridLengthAnimation` | 列/行宽度自定义动画 | `GetCurrentValue()` 支持缓动函数 |

### 2.2 Core/Constants (配置中心)
集中所有魔法字符串/数字，提升可维护性。详见 [ARCHITECTURE.md](ARCHITECTURE.md) 第 2.2 节。

### 2.3 Core/Converters (绑定桥接)
在 ViewModel 与 View 之间转换数据。详见 ARCHITECTURE.md 第 2.3 节。

### 2.4 Services 服务层
- **LocalizationService**: 封装 WPFLocalizeExtension 进行文化管理
- **ThemeService**: 管理 ResourceDictionary 切换实现主题

### 2.5 ViewModels/MainViewModel
聚合 UI 状态，暴露命令。包含导航、主题、语言状态及对应命令。

### 2.6 表现层
**MainWindow.xaml.cs**: UI 交互胶水（拖拽、动画、宽度计算）
**主题 XAML 文件**: 定义主题特定的 ResourceDictionary

## 3. 数据流图

详细流程请参阅 [ARCHITECTURE.md](ARCHITECTURE.md) 第 3 节，包括：
- 语言切换流程
- 主题切换流程
- 导航宽度计算 (v1.3)

## 4. 配置与常量策略

五个常量类消除魔法字符串：NavigationConstants, ThemeConstants, LanguageConstants, IconConstants, PageConstants。

**好处**:
1. 配置的唯一真实来源
2. 易于扩展
3. 编译时安全
4. IDE 自动完成支持

## 5. 扩展点

### 添加新页面
1. 在 `PageConstants` 定义常量
2. 添加本地化键
3. 在 `MainViewModel` 添加计算属性
4. 在 `MainWindow.xaml` 添加按钮与内容面板

### 添加新语言
1. 创建 `Strings.{code}.resx` 文件并翻译
2. 更新 `LanguageConstants`
3. 更新 `ServiceCollectionExtensions` 配置
4. 在设置页添加语言按钮

### 添加新主题
1. 创建主题 XAML 文件
2. 更新 `ThemeConstants`
3. 添加本地化键
4. 更新 `ThemeService.GetThemeDisplayName()`
5. 在设置页添加主题按钮

### 添加新图标
直接在 `IconConstants` 中添加 Segoe MDL2 代码，无需修改 UI 代码。

## 6. 错误处理与弹性

所有服务都具备回退机制：
- LocalizationService: 缺失键返回原键，无效文化回退默认
- ThemeService: 未知主题回退 Light
- MainWindow: 宽度计算失败使用默认值

**日志**: 当前使用 `Debug.WriteLine()`，v1.4 将迁移到 `ILogger`

## 7. 测试策略

### 单元测试
- 常量类映射验证
- 服务文化/主题切换与回退
- ViewModel 命令与状态变化

### 集成测试
- 语言切换导致导航宽度重算
- 主题切换更新所有绑定
- 动画完成验证

### UI 测试
- 导航面板交互（双击、拖拽）
- 语言/主题按钮即时更新

## 8. 性能优化

- 单例服务（一次实例化）
- 缓存测量结果
- 缓动动画减少布局抖动
- 单向绑定优化

## 9. 依赖关系

服务依赖常量，永不反向。视图依赖 ViewModels 和服务，服务永不引用视图。

详细依赖图请参阅 [ARCHITECTURE.md](ARCHITECTURE.md) 第 9 节。

## 10. 未来路线图

### v1.4 计划
- 日志抽象 (ILogger)
- 设置持久化
- 数据驱动导航
- 命令 CanExecute

### v2.0 愿景
- 插件系统
- 主题热重载
- 遥测分析
- 可访问性增强

---
**文档版本**: 1.3  
**最后更新**: 2024  
**完整英文版**: [ARCHITECTURE.md](ARCHITECTURE.md)
