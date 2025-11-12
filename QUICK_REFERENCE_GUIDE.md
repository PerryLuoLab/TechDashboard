# ?? 重构后使用指南 - 快速参考

## ?? 目录
1. [如何获取服务](#如何获取服务)
2. [如何添加新服务](#如何添加新服务)
3. [如何修改配置](#如何修改配置)
4. [如何编写测试](#如何编写测试)
5. [常见问题](#常见问题)

---

## ?? 如何获取服务

### 方式1: 构造函数注入（推荐）?

```csharp
public class YourViewModel
{
    private readonly ILocalizationService _localizationService;
    private readonly IThemeService _themeService;

    // IoC容器自动注入
    public YourViewModel(
        ILocalizationService localizationService,
        IThemeService themeService)
    {
        _localizationService = localizationService;
        _themeService = themeService;
    }

    public void DoSomething()
    {
        var text = _localizationService.GetString("Menu_File");
        _themeService.ApplyTheme("Dark");
    }
}
```

### 方式2: 服务定位器（不推荐，但可用）

```csharp
public class SomeClass
{
    public void DoSomething()
    {
        // 从全局服务提供者获取
        var localizationService = App.Services.GetRequiredService<ILocalizationService>();
        var text = localizationService.GetString("Menu_File");
    }
}
```

---

## ? 如何添加新服务

### 步骤 1: 创建接口

```csharp
// Services/Interfaces/ISettingsService.cs
namespace TechDashboard.Services.Interfaces
{
    public interface ISettingsService
    {
        T GetSetting<T>(string key);
        void SaveSetting<T>(string key, T value);
        void ResetSettings();
    }
}
```

### 步骤 2: 创建实现

```csharp
// Services/SettingsService.cs
using TechDashboard.Services.Interfaces;

namespace TechDashboard.Services
{
    public class SettingsService : ISettingsService
    {
        public T GetSetting<T>(string key)
        {
            // 实现代码
        }

        public void SaveSetting<T>(string key, T value)
        {
            // 实现代码
        }

        public void ResetSettings()
        {
            // 实现代码
        }
    }
}
```

### 步骤 3: 注册服务

在 `ServiceCollectionExtensions.cs` 中添加：

```csharp
public static IServiceCollection AddApplicationServices(this IServiceCollection services)
{
    // ...existing services...

    // 添加新服务
    services.AddSingleton<ISettingsService, SettingsService>();

    return services;
}
```

### 步骤 4: 使用服务

```csharp
public class MainViewModel
{
    private readonly ILocalizationService _localizationService;
    private readonly IThemeService _themeService;
    private readonly ISettingsService _settingsService; // 新服务

    public MainViewModel(
        ILocalizationService localizationService,
        IThemeService themeService,
        ISettingsService settingsService) // 注入新服务
    {
        _localizationService = localizationService;
        _themeService = themeService;
        _settingsService = settingsService;
    }
}
```

---

## ?? 如何修改配置

### 修改本地化配置

在 `ServiceCollectionExtensions.cs` 中：

```csharp
services.Configure<LocalizationOptions>(options =>
{
    // 修改程序集名称
    options.AssemblyName = "YourAppName"; // 改这里

    // 修改字典名称
    options.DictionaryName = "YourResources"; // 改这里

    // 修改默认语言
    options.DefaultCulture = "fr-FR"; // 改这里

    // 修改可用语言
    options.AvailableCultures = new[]
    {
        "en-US",
        "fr-FR",
        "de-DE",
        "es-ES"
    };
});
```

### 创建新的配置选项

```csharp
// 1. 创建选项类
public class ThemeOptions
{
    public string DefaultTheme { get; set; } = "Dark";
    public string[] AvailableThemes { get; set; } = { "Dark", "Light", "BlueTech" };
}

// 2. 注册配置
services.Configure<ThemeOptions>(options =>
{
    options.DefaultTheme = "Light";
});

// 3. 在服务中使用
public class ThemeService : IThemeService
{
    private readonly ThemeOptions _options;

    public ThemeService(IOptions<ThemeOptions> options)
    {
        _options = options.Value;
    }
}
```

---

## ?? 如何编写测试

### 安装测试包

```bash
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package MSTest.TestAdapter
dotnet add package MSTest.TestFramework
dotnet add package Moq
```

### 测试服务

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Options;
using TechDashboard.Services;
using TechDashboard.Options;

[TestClass]
public class LocalizationServiceTests
{
    private LocalizationService CreateService()
    {
        var options = Options.Create(new LocalizationOptions
        {
            AssemblyName = "TechDashboard",
            DictionaryName = "Strings",
            DefaultCulture = "en-US"
        });
        return new LocalizationService(options);
    }

    [TestMethod]
    public void ChangeLanguage_ShouldUpdateCulture()
    {
        // Arrange
        var service = CreateService();

        // Act
        service.ChangeLanguage("zh-CN");

        // Assert
        Assert.AreEqual("zh-CN", service.CurrentCulture.Name);
    }

    [TestMethod]
    public void GetString_ShouldReturnNonEmptyString()
    {
        // Arrange
        var service = CreateService();

        // Act
        var result = service.GetString("Menu_File");

        // Assert
        Assert.IsFalse(string.IsNullOrEmpty(result));
    }
}
```

### 测试 ViewModel（使用 Mock）

```csharp
using Moq;
using TechDashboard.ViewModels;
using TechDashboard.Services.Interfaces;

[TestClass]
public class MainViewModelTests
{
    [TestMethod]
    public void ChangeLanguage_ShouldCallService()
    {
        // Arrange
        var mockLocalization = new Mock<ILocalizationService>();
        var mockTheme = new Mock<IThemeService>();
        
        var viewModel = new MainViewModel(
            mockLocalization.Object,
            mockTheme.Object);

        // Act
        viewModel.ChangeLanguage("zh-CN");

        // Assert
        mockLocalization.Verify(
            x => x.ChangeLanguage("zh-CN"), 
            Times.Once);
    }

    [TestMethod]
    public void CurrentLanguageDisplay_ShouldReturnCorrectValue()
    {
        // Arrange
        var mockLocalization = new Mock<ILocalizationService>();
        mockLocalization
            .Setup(x => x.GetLanguageDisplayName(It.IsAny<string>()))
            .Returns("简体中文");

        var mockTheme = new Mock<IThemeService>();
        
        var viewModel = new MainViewModel(
            mockLocalization.Object,
            mockTheme.Object);

        // Act
        var result = viewModel.CurrentLanguageDisplay;

        // Assert
        Assert.AreEqual("简体中文", result);
    }
}
```

---

## ? 常见问题

### Q1: 如何在非注入的类中获取服务？

A: 使用 `App.Services`:

```csharp
var service = App.Services.GetRequiredService<ILocalizationService>();
```

### Q2: 服务生命周期有哪些？

A: 三种生命周期：

```csharp
// 单例 - 整个应用只有一个实例
services.AddSingleton<IMyService, MyService>();

// 作用域 - 每个作用域一个实例（WPF中不常用）
services.AddScoped<IMyService, MyService>();

// 瞬态 - 每次请求创建新实例
services.AddTransient<IMyService, MyService>();
```

### Q3: 如何替换服务实现？

A: 修改注册：

```csharp
// 之前
services.AddSingleton<ILocalizationService, LocalizationService>();

// 之后（替换为新实现）
services.AddSingleton<ILocalizationService, MyCustomLocalizationService>();
```

### Q4: 如何注入多个相同接口的实现？

A: 使用命名注册：

```csharp
// 注册多个实现
services.AddSingleton<ILocalizationService, LocalizationServiceV1>();
services.AddSingleton<ILocalizationService, LocalizationServiceV2>();

// 获取所有实现
var allServices = App.Services.GetServices<ILocalizationService>();
```

### Q5: 如何查看所有注册的服务？

A: 在调试时检查 `IServiceCollection`:

```csharp
protected override void OnStartup(StartupEventArgs e)
{
    var services = new ServiceCollection();
    services.AddApplicationServices();

    // 打印所有服务
    foreach (var service in services)
    {
        Debug.WriteLine($"{service.ServiceType.Name} -> {service.ImplementationType?.Name}");
    }

    _serviceProvider = services.BuildServiceProvider();
}
```

### Q6: 如何处理循环依赖？

A: 避免循环依赖，或使用 `IServiceProvider`:

```csharp
// 不好：A依赖B，B依赖A（循环）
public class ServiceA
{
    public ServiceA(ServiceB b) { }
}

public class ServiceB
{
    public ServiceB(ServiceA a) { }
}

// 好：使用IServiceProvider延迟解析
public class ServiceA
{
    private readonly IServiceProvider _services;

    public ServiceA(IServiceProvider services)
    {
        _services = services;
    }

    public void DoSomething()
    {
        var b = _services.GetRequiredService<ServiceB>();
    }
}
```

---

## ?? 最佳实践

### ? DO（推荐）

1. **使用构造函数注入**
   ```csharp
   public MyClass(IMyService service) { }
   ```

2. **依赖接口而非实现**
   ```csharp
   private readonly ILocalizationService _service; // ?
   ```

3. **服务保持单一职责**
   ```csharp
   public interface ILocalizationService // 只负责本地化
   {
       string GetString(string key);
   }
   ```

4. **使用Options模式配置**
   ```csharp
   services.Configure<MyOptions>(options => { });
   ```

### ? DON'T（避免）

1. **不要直接new服务**
   ```csharp
   var service = new LocalizationService(); // ?
   ```

2. **不要依赖具体实现**
   ```csharp
   private readonly LocalizationService _service; // ?
   ```

3. **不要在服务中使用静态方法**
   ```csharp
   public static string GetString() { } // ?
   ```

4. **不要硬编码配置**
   ```csharp
   var text = GetString("TechDashboard", "Strings", key); // ?
   ```

---

## ?? 相关文档

- [REFACTORING_COMPLETE_FINAL_REPORT.md](REFACTORING_COMPLETE_FINAL_REPORT.md) - 完整重构报告
- [REFACTORING_MVVM_IOC_SUMMARY.md](REFACTORING_MVVM_IOC_SUMMARY.md) - 重构总结

---

## ?? 需要帮助？

如果遇到问题：

1. 检查 `App.xaml.cs` 中服务是否正确注册
2. 确认接口和实现是否匹配
3. 查看 Debug 输出窗口的错误信息
4. 参考完整文档

---

**快速参考已准备就绪！** ??
