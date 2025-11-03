# TechDashboard

- .NET 8 WPF 暗色科技风 Dashboard 示例
- 左侧可折叠导航栏（图标 + 文本）、主题切换（Dark / Light）
- MVVM（MainViewModel） + ObservableObject 基类

## 运行

1. 需要 .NET 8 SDK
2. 打开终端到项目根目录
   - `dotnet build`
   - `dotnet run --project TechDashboard/TechDashboard.csproj`
或在 Visual Studio 中打开 `TechDashboard.sln` 并运行。

##  新增功能

🎯 导航按钮选中动画

悬停时：背景渐变 + 向右滑动 4px
点击时：缩放动画 (1.05x)
选中状态：蓝色高亮 + 左侧边框指示器


📐 导航栏折叠/展开动画

宽度：260px ↔ 70px 平滑过渡
文本：淡入淡出效果
图标：自动切换方向 (◀ ▶)


🎨 蓝色科技风增强

渐变背景（暗色：深蓝黑 / 亮色：浅蓝白）
卡片边框：蓝色光晕效果
强调色：三色渐变 (#39C2FF → #00A3FF → #0077CC)
Logo/图标：发光效果 (DropShadowEffect)


💎 视觉优化

所有按钮：缩放 + 发光悬停效果
统计卡片：数字发光 + 边框渐变
进度条：渐变填充色
卡片阴影：柔和外发光
