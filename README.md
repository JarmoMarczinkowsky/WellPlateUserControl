# WellPlateUserControl

Note:
The colors are used with the Color class from Microsoft. It is possible to use RGB-colors by using: Color.FromRgb(0,0,0)

<h3>A usercontrol that creates a grid for the wellplate.</h3>

<b>Functions:</b>
```
SetWellPlateSize(8, 6)
```
Sets the size of the wellplate in wells. The first argument '8' is the width and the second argument '6' is the height of the wellplate. 
Set this function after the colors, circlesize and/or the rectangle and before the coordinateconverter and/or functions that return items<br>
[Optional]
```
SetGridColor(Colors.Black) 
```
Gives the grid the color that you wish it to be. Default is black.
Set this function before the DrawWellPlate.<br>
[Optional]
```
SetClickColor(Colors.Red)
```
Sets the color of a clicked well to the color you wished. Default is red. 
Set this function before the DrawWellPlate.<br>
[Optional]
```
SetMaxWidth = <int>
```
Set this function before 'DrawWellPlate'.
Set the maximum width of the wells that need to be generated. 
Uses an integer for the 'width in pixels'.
Default is 600 pixels.
Will choose the highest line of code if both are set.<br>
[Optional]
```
SetMaxHeight = <int>
```
Set this function before 'DrawWellPlate'.
Set the maximum height of the wells that need to be generated. 
Uses an integer for the 'height in pixels'.
Default is 600 pixels.
Will choose the highest line of code if both are set.<br>
[Optional]
```
IsEditable = <bool>
```
Set this function before 'DrawWellPlate'.
Makes the wellplate editable so you can color wells by clicking on them.<br>
[Optional]
```
SetStrokeColor = Colors.Blue
```
Set this variable before the DrawWellPlate.
Set the stroke of the wells in the wellplate.<br>
[Optional]
```
SetStrokeThickness = 20
```
Thickness of the stroke is set in percentages.
Will create a black stroke is SetStrokeColor is not set.<br>
Set this variable before DrawWellPlate.<br>
[Optional]
```
IsRectangle = <bool>
```
Makes every well a rectangle instead of a circle. Will also increase the distance between the wells with 5 percent.
Set this variable before the DrawWellPlate.<br>
[Optional]
```
DrawWellPlate
```
Set this function after the colors, circlesize and/or the rectangle and before the coordinateconverter and/or functions that return items.
Generates the wellplate based on the values the user entered. If it doesn't find any values that the user entered, it will use default values:<br>
The default values are:<br>
SetGridColor = Color.FromRgb(209, 232, 247);<br>
SetClickColor = Color.FromRgb(97, 172, 223);<br>
IsEditable = false;<br>
IsRectangle = false;<br>
[Mandatory]
```
ColorCoordinate("A1")
ColorCoordinate("6")
ColorCoordinate("A5", Colors.Green)
ColorCoordinate(5, Colors.Blue)
```
Works with strings and numbers. It colors the coordinate that you enter with the 'clickColor' you entered earlier. 
Enter "A5", with the double quotes, to color coordinate a5. Enter "3", with the double quotes, to color the third well in the wellplate.
The second argument is optional and takes care of the color of the chosen well.
Set this function after the DrawWellPlate.<br>
[Optional]
```
NumberToCoordinate(4)
```
Returns the coordinate belonging to that number. For example: '4' returns 'D4' in a 8x6 wellplate.
Set this function after the DrawWellPlate.<br>
[Optional]
```
CoordinateToNumber("D4")
```
Returns the number belonging to that coordinate. 
For example: "D4" returns '4' in a 8x6 wellplate.
Set this function after the DrawWellPlate.<br>
[Optional]
```
GetColoredList()
```
Returns a list with the coordinates with every coordinate that is currently colored.
Set this function after the DrawWellPlate.<br>
[Optional]
```
GetNotColoredList()
```
Returns a list with the coordinates of every well that is currently not colored.
Set this function after DrawWellPlate.<br>
[Optional]
```
LastClickedCoordinate
```
Will return the coordinate of the last clicked well. Also works when IsEditable is off.<br>
[Optional]

