# WellPlateUserControl

<h3>A usercontrol that creates a grid for the wellplate.</h3>

<b>Functions:</b>
```
<b>SetGridColor(Colors.[wishedColor])</b> 
```
Gives the grid the color that you wish it to be. Default is black.
Set this function before the WellPlateSize.
[Optional]
```
<b>SetClickColor(Colors.[wishedColor])</b>
```
Sets the color of a clicked well to the color you wished. Default is red. 
Set this function before the WellPlateSize.
[Optional]
```
<b>SetStrokeColor(Colors.[wishedColor])</b>
```
Set the stroke of the wells in the wellplate.
Set this function before the WellPlateSize.
[Optional]
```
<b>SetCircleSize(number)</b>
```
Set the size of the wells in your usercontrol. It works as a multiplier, so '2' is 2 times as big as it is normally.
'0.5' is half so big as it is normally.
Set this function before the WellPlateSize.
[Optional]
```
<b>IsRectangle()</b>
```
Makes every well a rectangle instead of a circle. Will also increase the distance between the wells with 5 percent.
Set this function before the WellPlateSize.
[Optional]
```
<b>SetWellPlateSize(width, height)</b>
```
Sets the size of the wellplate. The first number is the width and the second number is the height of the wellplate. 
Set this function after the colors, circlesize and/or the rectangle and before the coordinateconverter and/or functions that return items
[Mandatory]
```
<b>ColorCoordinate(coordinate or number)</b>
```
Works with a string. It colors the coordinate that you enter with the 'clickColor' you entered earlier. 
Enter "A5", with the double quotes, to color coordinate a5. Enter "3", with the double quotes, to color the third well in the wellplate.
Set this function after the WellPlateSize.
[Optional]
```
<b>CoordinateConverter(coordinate or number)</b>
```
Returns the coordinate belonging to that number. For example: '4' returns 'D4' in a 8x6 wellplate.
Also works with coordinates. For example: "D4" returns '4' in a 8x6 wellplate.
Set this function after the WellPlateSize.
[Optional]
```
<b>GiveColoredList()</b>
```
Returns a list with the coordinates with every coordinate that is currently colored.
Set this function after the WellPlateSize.
[Optional]
```
<b>GiveNotColoredList()</b>
```
Returns a list with the coordinates of every well that is currently not colored.
Set this function after the WellPlateSize.
[Optional]
