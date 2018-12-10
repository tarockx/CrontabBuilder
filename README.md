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

You have three ways to obtain the resulting CRONTAB expression:
### Option 1
Simply read the `CrontabString` property of `CrontabEditorControl`. This will always hold the updated CRONTAB string, or null if the user has selected some invalid settings

### Option 2
You can also *bind* to the `CrontabString` property (it's a dependency property), so another option is to use data binding, like this:
```XML
<Grid>
    <ctb:CrontabEditorControl CrontabString="{Binding MyCrontabProperty}"></ctb:CrontabEditorControl>
</Grid>
  ```

### Option 3:
Lastly, the `CrontabEditorControl` exposes a `CrontabStringChanged` event, so you can subscribe to it to receive an update every time the user changes the scheduling. Example:
```C#
crontabEditorControl.CrontabStringChanged += (s, crontabString) => {
    Console.WriteLine("New crontab string: " + crontabString);
};
```

## Credits

This project uses [Cron Expression Descriptor](https://github.com/bradymholt/cron-expression-descriptor) by Brady Holt 