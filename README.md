# CrontabBuilder

CrontabBuilder is a (very) simple WPF control that allows users to create or edit a CRONTAB string (must be in the Qaurtz format) in a visual way.

## Installation

The package is available on NuGet:
> https://www.nuget.org/packages/MasterT.WPF.CrontabBuilder

## Usage

To use the control, first add the following namespace to your XAML:

```XML
xmlns:ctb="clr-namespace:MasterT.WPF.CrontabBuilder;assembly=MasterT.WPF.CrontabBuilder"
```

then, just add the control:
```XML
<Grid>
    <ctb:CrontabEditorControl></ctb:CrontabEditorControl>
</Grid>
  ```
The user will be able to select a schduling thanks to a series of dedicated visual controls. 

### Setting an initial Crontab string
If you want, you can assign a value to the `CrontabString` dependency property of `CrontabEditorControl` in code (or via XAML) to update the current scheduling configuration. The control will update accordingly. However, note that this will only work for strings in the formats supported by the `CrontabEditorControl`. Equivalent strings but with a different format will instead activate the "Custom Crontab string" mode.

### Obtaining the current Crontab string
You have three ways to obtain the CRONTAB expression for the currently selected scheduling:

#### Option 1
Simply read the `CrontabString` property of `CrontabEditorControl`. This will always hold the updated CRONTAB string, or null if the user has selected some invalid settings

#### Option 2
You can also *bind* to the `CrontabString` property (it's a dependency property), so another option is to use data binding, like this:
```XML
<Grid>
    <ctb:CrontabEditorControl CrontabString="{Binding MyCrontabProperty}"></ctb:CrontabEditorControl>
</Grid>
  ```

#### Option 3:
Lastly, the `CrontabEditorControl` exposes a `CrontabStringChanged` event, so you can subscribe to it to receive an update every time the user changes the scheduling. Example:
```C#
crontabEditorControl.CrontabStringChanged += (s, crontabString) => {
    Console.WriteLine("New crontab string: " + crontabString);
};
```

## Further configuration

The `CrontabEditorControl` exposes the following dependency properties that can be used for further customization:
* `ShowCurrentCrontab` - shows or hides the bottom panel that shows the currently selected Crontab string
* `ShowCurrentCrontabDescription` - shows or hides the bottom panel that shows a human-readable description the currently selected Crontab string
* `ShowInfoOutsideMainScroller` - if set to `true`, this will move the description panels outside the main `ScrollViewer` that contains all the crontab editing controls. This means the description panel will always be visible even if the editing controls are partially hidden due to lack of vertical space.

## Credits

This project uses [Cron Expression Descriptor](https://github.com/bradymholt/cron-expression-descriptor) by Brady Holt 

Thanks to [Christoffer Pedersen](https://github.com/Tonur) for: localization support, danish translation