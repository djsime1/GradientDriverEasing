# GradientDriverEasing

A [ResoniteModLoader](https://github.com/resonite-modding-group/ResoniteModLoader) mod for [Resonite](https://resonite.com/) that adds [easing function](https://easings.net/) presets to [ValueGradientDriver](https://wiki.resonite.com/Component:ValueGradientDriver) components.

*Depends on [CustomUILib](https://github.com/art0007i/CustomUILib) to add the inspector UI!*

![Screenshot](https://github.com/djsime1/GradientDriverEasing/assets/8518150/c1977850-5ec8-400d-b917-1049964dcf55)

## Installation
1. Install [ResoniteModLoader](https://github.com/resonite-modding-group/ResoniteModLoader).
2. Place [CustomUILib.dll](https://github.com/djsime1/GradientDriverEasing/releases/latest/download/CustomUILib.dll) and [GradientDriverEasing.dll](https://github.com/djsime1/GradientDriverEasing/releases/latest/download/GradientDriverEasing.dll) into your `rml_mods` folder. This folder should be at `C:\Program Files (x86)\Steam\steamapps\common\Resonite\rml_mods` for a default install. You can create it if it's missing, or if you launch the game once with ResoniteModLoader installed it will create this folder for you.
3. Start the game and browse to or create a ValueGradeintDriver component using the Inspector. Two new sections should appear on the component.
4. Consult the guide below to learn how the mod works.

## Usage guide

The additional UI is split into two sections: Easing utilities and Easing functions.

### Easing utilities

This section contains general settings and utilities. The first row controls the easing function endpoints.

- `Min position`: Defines the minimum interpolation position for easing functions.
- `Max position`: Defines the maximum interpolation position for easing functions.

If there are 2 or more points in the Points list, these values will be automatically set to the minimum and maximum positions of the list. Otherwise, they default to 0 and 1 respectively.

- <kbd>01</kbd>: Sets the min and max fields to 0 and 1 respectively.
- <kbd>Auto</kbd>: Sets the min and max fields to the minimum and maximum positions in the Points list.
- <kbd>Swap</kbd>: Swaps the minimum and maximum fields values.

The second row contains buttons to manipulate the Points list.

- <kbd>Sort position</kbd>: Sorts all Points in ascending order by their Position.
- <kbd>-Position</kbd>: Inverts the Position of every Point (MaxPosition - Position)
- <kbd>Subdivide</kbd>: For each pair of adjacent Points, adds a new Point between the two with values half-interpolated. Good for making easing curves more detailed.

### Easing functions

- `Target field`: Determines whether the easing function buttons apply to the Points Positions or Values.
- - <kbd>Position</kbd>: Applies easing functions to the Position of each point according to its order in the list, with the `Min position` and `Max position` fields determining the lowest and highest values.
- - <kbd>Value</kbd>: Applies easing functions to the Value of each point according to its Position, using the first and last list items as the curve start and end. **This will overwrite all the values inbetween!**

The rest of the buttons will apply the corresponding [easing function](https://easings.net/) to all the Points Positions or Values, depending on your selection above.

## Additional notes

- Back, Elastic, Bounce, and Spring easing presets are only available when `Target field` is set to <kbd>Value</kbd>.
- Back, Elastic, and Spring easing presets are clamped to the largest and smallest value in the Points list. Calculating interpolations beyond these values may be implemented in a future update.
- When animating the ValueGradientDriver, you probably want to use [ConstantLerpValue](https://wiki.resonite.com/Component:ConstantLerpValue) instead of [SmoothValue](https://wiki.resonite.com/Component:SmoothValue); Otherwise the target value will be double-smoothed.
- Thank you [cjddmut](https://github.com/cjddmut) for the [EasingFunctions.cs](https://gist.github.com/cjddmut/d789b9eb78216998e95c) code!
