# WellPlateUserControl

Note:
The colors are used with the Color class from Microsoft. It is possible to use RGB-colors by using: Color.FromRgb(0,0,0)
Make sure you set the <b>width</b>, <b>height</b> and <b>VerticalAlignment</b> of the usercontrol

<h3>A usercontrol that creates a grid for the wellplate.</h3>

<b>Functions:</b>
```
SetWellPlateSize(12, 8);
```
SetWellPlateSize is the command used to configure the amount of columns and rows.  
SetWellPlateSize takes two integers for arguments: width and height. 

```
GridColor = Colors.Black;
```
GridColor is the variable used to set the colors of the wells in the wellplate. 
Uses one argument.
Default is: Color.FromRgb(209, 232, 247). That is a light blue color.
RGB-colors are possible with ‘Color.FromRgb(0,0,0)’ for the argument. 
Hex-colors are possible with ‘(Color)ColorConverter.ConvertFromString("#ff0d00")’ for the argument.
```
ClickColor = Colors.Red;
```
ClickColor is the variable used to set the color of a well after you’ve clicked it. 
Default is: Colors.CadetBlue.
RGB-colors are possible with ‘Color.FromRgb(0,0,0)’ for the argument. 
Hex-colors are possible with ‘(Color)ColorConverter.ConvertFromString("#ff0d00")’ for the argument.
```
IsEditable = true;
```
This variable is used to make the wellplate clickable. If it is set, the user can click on a well in the wellplate and the well will change to color to the set ClickColor.
Default is: false. 
```
StrokeColor = Colors.Green;
```
This variable is used to set a small stroke around every well with a color. 
Default is no stroke.
Default size of the stroke is 16% of a well.
RGB-colors are possible with ‘Color.FromRgb(0,0,0)’ for the argument. 
Hex-colors are possible with ‘(Color)ColorConverter.ConvertFromString("#ff0d00")’ for the argument.
```
StrokeThickness = 20;
```
This variable is used to set the thickness of a stroke. It uses percentages.
Default is 16%. If the color of the stroke is not set, it will use black.
```
IsRectangle = true;
```
This variable is used to make every well rectangular instead of an ellipse.
Default is false.
```
WellSize = 30;
```
This variable is used to give every well a fixed size instead of calculating it according to the width and height of the wellplate. 
Warning: has a high chance of generating wells outside of the wellplate. Use at your own caution.
```
LabelColor = Colors.Black;
```
This variable is used to give the numeric and alphabetic coordinate labels a different color. 
Default is black.
RGB-colors are possible with ‘Color.FromRgb(0,0,0)’ for the argument. 
Hex-colors are possible with ‘(Color)ColorConverter.ConvertFromString("#ff0d00")’ for the argument.
```
BorderColor = Colors.LightGray;
```
This variable is used to give the border around the wellplate a different color. 
RGB-colors are possible with ‘Color.FromRgb(0,0,0)’ for the argument. 
Hex-colors are possible with ‘(Color)ColorConverter.ConvertFromString("#ff0d00")’ for the argument.
Default is light gray.
```
WellPlateBackground = Colors.Cyan;
```
This variable is used to give the background of the wellplate a color.
RGB-colors are possible with ‘Color.FromRgb(0,0,0)’ for the argument. 
Hex-colors are possible with ‘(Color)ColorConverter.ConvertFromString("#ff0d00")’ for the argument.
Default is transparent.
```
TurnCoordinatesOff = true;
```
This variable is used to hide the coordinates around the wellplate. 
Default is false.
--------------------------------------------------------------------------------------------------------------------------------------
```
DrawWellPlate();
```
This function is used to draw the wellplate. You won’t see the wellplate without this function.
Set this function beneath the arguments which you can find above.
Set this function above the arguments which you can find below.
--------------------------------------------------------------------------------------------------------------------------------------
```
ColorCoordinate(“A1”);
ColorCoordinate(4);
ColorCoordinate(“B2”, Colors.Red);
ColorCoordinate(5, Colors.Green);
```
These functions are used to color a specific well in a wellplate. You can use this function in 4 different ways. 
Method 1:  ```ColorCoordinate(“A1”);```
You enter only the coordinate. This will make sure that the well on coordinate ‘A1’ gets a different color than the other wells. It will choose the color that is used for the ClickColor. In case this one isn’t set either, it will use CadetBlue.
Method 2: ```ColorCoordinate(4);```
You enter only the number of a well. This will make sure that the 4th well gets a different color than the other wells. It will choose the color that is used for the ClickColor. In case this one isn’t set either, it will use CadetBlue.
Method 3: ```ColorCoordinate(“B2”, Colors.Red);```
You enter the coordinate of the well and the color it has to become. This will make sure that the well on coordinate ‘B2’ becomes red.
Method 4: ```ColorCoordinate(4, Colors.Green);```
You enter the number of the well and the color it has to become. This will make sure that the 5th well will become green.

```
CoordinateToNumber(“A4”);
```
This function will return the number of the well that belongs on coordinate ‘A4’. It will return ‘-1’ if it doesn’t find anything.
```
NumberToCoordinate(5);
```
This function will return the coordinate that belongs to the 5th well. It will return an empty string if it doesn’t find anything.
```
List<string> coloredCoordinates = globalWellPlate.ColoredCoordinates;
```
This variable returns a list with every coordinate that has been colored by either a click or the use of ‘ColorCoordinate()’.
```
List<string> notColoredCoordinates = globalWellPlate.NotColoredCoordinates;
```
This variable returns a list with every coordinate that has not been colored by either a click or the use of ‘ColorCoordinate()’.
```
string lastClicked = globalWellPlate.LastClickedCoordinate;
```
Returns the coordinate of the last clicked coordinate.
```
Clear(“A4”)
Clear(4)
Clear()
```
This function will change the color of a colored well to the color that is used for the wellplate itself.  
Use a coordinate like ‘B5’ to revert the color of B5 back to the color of the wellplate.
Use a number like ‘5’ to revert the color of the 5th well back to the color of the wellplate.
Leave the function empty to clear every color in the wellplate.

