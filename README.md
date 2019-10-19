# ExtendWebBrowser
extend winform webbrowser control
项目描述请看https://blog.csdn.net/wodeshijianhrf/article/details/90746142
## 环境
- .net framework4.5
## 在wpf项目中使用
- 需要项目引用两个dll WindowsFormsIntegration.dll，System.Windows.Forms.dll
- 在xmal中使用扩展的控件需要加 
```xml
 xmlns:wfi ="clr-namespace:System.Windows.Forms.Integration;assembly=WindowsFormsIntegration"
```
```xml
<wfi:WindowsFormsHost>
    <local:ExtendedWebBrowser x:Name="Browser" Url="www.baidu.com"></local:ExtendedWebBrowser>
</wfi:WindowsFormsHost>
```
## 扩展
1. DWebBrowserEvents2 中加入需要扩展的事件 https://docs.microsoft.com/en-us/dotnet/api/shdocvw.dwebbrowserevents2?view=dynamics-usd-3
2. ExtendedWebBrowser与IWebBrowserEvent中加入对外暴露的事件
3. WebBrowserExtendedEvents 中进行中转事件
