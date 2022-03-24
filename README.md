# WellPlateUserControl

<h3>A usercontrol that creates a grid for the wellplate.</h3>

<b>Functions:</b>
```
SetGridColor(Colors.[wishedColor]) 
```
Gives the grid the color that you wish it to be. Default is black.
Set this function before the WellPlateSize.
[Optional]
```
SetClickColor(Colors.[wishedColor])
```
Sets the color of a clicked well to the color you wished. Default is red. 
Set this function before the WellPlateSize.
[Optional]
```
SetStrokeColor(Colors.[wishedColor])
```
Set the stroke of the wells in the wellplate.
Set this function before the WellPlateSize.
[Optional]
```
SetCircleSize(number)
```
Set the size of the wells in your usercontrol. It works as a multiplier, so '2' is 2 times as big as it is normally.
'0.5' is half so big as it is normally.
Set this function before the WellPlateSize.
[Optional]
```
IsRectangle()
```
Makes every well a rectangle instead of a circle. Will also increase the distance between the wells with 5 percent.
Set this function before the WellPlateSize.
[Optional]
```
SetWellPlateSize(width, height)
```
Sets the size of the wellplate. The first number is the width and the second number is the height of the wellplate. 
Set this function after the colors, circlesize and/or the rectangle and before the coordinateconverter and/or functions that return items
[Mandatory]
```
ColorCoordinate(coordinate or number)
```
Works with a string. It colors the coordinate that you enter with the 'clickColor' you entered earlier. 
Enter "A5", with the double quotes, to color coordinate a5. Enter "3", with the double quotes, to color the third well in the wellplate.
Set this function after the WellPlateSize.
[Optional]
```
CoordinateConverter(coordinate or number)
```
Returns the coordinate belonging to that number. For example: '4' returns 'D4' in a 8x6 wellplate.
Also works with coordinates. For example: "D4" returns '4' in a 8x6 wellplate.
Set this function after the WellPlateSize.
[Optional]
```
GiveColoredList()
```
Returns a list with the coordinates with every coordinate that is currently colored.
Set this function after the WellPlateSize.
[Optional]
```
GiveNotColoredList()
```
Returns a list with the coordinates of every well that is currently not colored.
Set this function after the WellPlateSize.
[Optional]
```
SetMaxWidth(width in pixels)
```
Set the maximum width of the wells that need to be generated. 
Uses an integer for the 'width in pixels'.
Default is 600 pixels.
Will choose the highest line of code if both are set.
[Optional]
```
SetMaxHeight(height in pixels)
```
Set the maximum height of the wells that need to be generated. 
Uses an integer for the 'height in pixels'.
Default is 600 pixels.
Will choose the highest line of code if both are set.
[Optional]

