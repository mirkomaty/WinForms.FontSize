# WinForms.FontSize

With the high resolution of modern monitors the forms designed with the WindowsForms designer may appear 
very small on some screens. Over the years, Microsoft has made some efforts to address the issue. 
These different approaches make the issue so complicated today.

If you leave your WindowsForms application running as is, you will definitely run into problems. 
So you have to choose a method that allows you to scale your WinForms as gently as possible.

## The best scaling approach

The best method for this purpose is to scale a Form using the Font scaling mode:

```
AutoScaleMode = AutoScaleMode.Font;
```

Don't expect miracles from this scaling. But before you have to switch your application 
to a vector-based solution, you can achieve usable results with font-based scaling.

## Stop Windows from cheating

Windows tries all sorts of upside downs to properly scale legacy WinForms applications. 
This includes system functions producing incorrect results when querying the resolution 
and scaling of the screen. So you first need to make sure that Windows returns you the correct values.

Tell Windows in your .csproj file that you support the latest version of Monitor Awareness: 

```
<ApplicationHighDpiMode>PerMonitorV2</ApplicationHighDpiMode>
```

However, the WinForms designer runs into problems with this setting. 
Visual Studio will display a warning about this if you have set your monitor to scale other than 100%. 
With another setting you can ensure that this mode does not apply to the design time:

```
<ForceDesignerDpiUnaware>true</ForceDesignerDpiUnaware>
```

Both settings together produce a reasonable result.

## Anwendung dieser Bibliothek

Now our font calculator comes into play. The default font for Windows Forms is usually Segue UI at 9pt size.

Now determine the optimal font size for the monitor on which your WinForm is currently located:

```
public MyForm
{
	InitializeComponent();
	// Calculate the new font size after InitializeComponent
	var newFontSize = FontCalculator.Calculate(Screen.FromControl(this), Font.Size);
	if (newFontSize > Font.Size)
		Font = new Font( "Segoe UI", newFontSize, FontStyle.Regular, GraphicsUnit.Point, 0 );
}
```

Combined with AutoScaleMode.Font you should now have a reasonable display on the screen.

When multiple monitors are used, you can react to window movement and recalculate the font size if necessary.
