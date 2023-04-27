# Zhakami Zhako's Gauge Script / Canvas HUD System
#### For use with SaccFlight; For internal Saccflight, BLACK ACES staff, 425th VTFTW, YURU YURU AVIATION members, UHR and selected individuals only. 
#### Not to be redistributed unless permitted.
#### Discord: ZhakamiZhako#2147 | Twitter: [@ZZhako](https://twitter.com/ZZhako/) | zhintamizhakami@gmail.com
#### Documentation is still in progress.
#### Tutorial is still being made.


## Introduction

Sacchan's Flight and Vehicles (SaccFlight) is an amazing system when it comes to creating Aircraft, Boats and Vehicles in VRChat and yet lacks an easier way in order to create your own custom instruments or HUD.

This system aims to allow people to create their projected instruments / HUD / Effects via Animations, Animation parameters, Animation layers and Text Elements.




## The Script Explained

The script works in a way where it takes certain values of the aircraft's state; Altitude, Speed, etc.
Any parameters that are empty will be ignored by the script; However... If you have animators without these parameters, you might want to either create these parameters or leave them empty; as Unity attempts to 'input' these values but will error out in the logs due to parameters missing from the said animators.


Reference:

### Animations

Typically, the way you would animate your gadgets/hud element/instrument is basically animating them from the 0 value, up to the maximum value in order for this to work. 
CanvasHUD/GaugeScript takes the values from the SaccAirVehicle components and translates these to a normalized parameter from 0~1f (Where 0 is the first frame of the animation, .5 is the middle of the animation, 1 is the final frame of the animation)

| Function          |Description| No. of Parameters |No. of dividers|Animation|Looped?|Parameter Range|
|--|--|--|--|--|--|--|
| Altimeter         | Altitude Readout based on aircraft's absolute position (+ if supplied, map offset.)   | 3 |3|0 to Maximum         ||0 - 1 Continuous|
| Speedometer       | Speed readout based on aircraft's velocity.                                           | 3 |3|0 to Maximum         ||0 - 1 Continuous|
| Radar Altimeter   | Altitude readout based on raycast shot from the bottom of the center of mass          | 2 |2|0 to Maximum         ||0 - 1 Continuous|
| Pitch             | Aircraft's absolute local rotation in X axis. For use with the ADI;                   | 1 | |-90, 0, +90          |Yes|0, 0.5, 1|
| Roll              | Aircraft's absolute local rotation in Z axis. For use with the ADI;                   | 1 | |-180, 0, +180        |Yes|0, 0.5, 1|
| Heading           | Aicraft's absolute rotation in Y Axis. For use with the ADI / Heading                 | 1 | |0 to +360            |Yes|0 - 1|
| Gs                | Aircraft's G's output.                                                                | 1 | |Minimum, 0, Maximum  ||0 - 1 Continuous|
| Mach              | Aircraft's Velocity in Mach                                                           | 1 | |0 to Maximum         ||0 - 1 Continuous|
| AoA               | Aircraft's Angle of Attack readout.                                                   | 1 | |0 to Maximum         ||0 - 1 Continuous|
| Afterburner       | Aircraft if Afterburner is engaged                                                    | 1 | |false, true          ||True/False|
| Directional G's X | Aircraft G's readout on the X axis                                                    | 1 | |0 to Maximum         ||0 - 1|
| Directional G's Y | Aircraft G's readout on the Y axis                                                    | 1 | |0 to Maximum         || 0 - 1|
| Directional G's Z | Aircraft G's readout on the Z axis                                                    | 1 | |0 to Maximum         || 0 - 1|
| Side G            | Aircraft's Gravity readout, Affected by the G's on the Y axis.                        | 1 | |Minimum, 0, Maximum  ||0, 0.5, 1|
| Velocity Indicator X| Aircraft's Velocity Indicator based on it's local X Axis.                           | 1 | |Minimum, 0, Maximum  |                       |0, 0.5, 1|
| Velocity Indicator Y| Aircraft's Velocity Indicator based on it's local Y Axis.                                                 | 1 | |Minimum, 0, Maximum  ||0, 0.5, 1|
| Velocity Indicator Z| Aircraft's Velocity Indicator based on it's local Z Axis.                                                 | 1 | |Minimum, 0, Maximum  ||0, 0.5, 1|
| Rotational Velocity Indicator X| Aircraft's rotational Velocity Indicator based on it's local X Axis.                           | 1 | |Minimum, 0, Maximum  ||0, 0.5, 1|
| Rotational Velocity Indicator Y| Aircraft's rotational Velocity Indicator based on it's local Y Axis.                           | 1 | |Minimum, 0, Maximum  ||0, 0.5, 1|
| Rotational Velocity Indicator Z| Aircraft's rotational Velocity Indicator based on it's local Z Axis.                           | 1 | |Minimum, 0, Maximum  ||0, 0.5, 1|
| Pull Up| Indicator if the aircraft has reached a lower altitude limit without the landing gears down | 1 | | True, False | | 
| Rate of Climb | Aircraft's climb rate in seconds | 1 | | Minimum, 0, Maximum |No | 0, 0.5, 1



### Text

|Function|Description|Parameters
|---|---|---|
|RPM|Aircraft's RPM / Current Engine output. Can be customized with Min RPM, Max RPM. Engine off will lerp back to 0.|Min RPM, Max RPM as mapped values
|Temperature|Aircraft's 'Virtual' Temperature output. Responsive according to RPM. Can be customized with Min Temp, Max Temp. Engine off will lerp back to 0.|Min Temp, Max Temp as mapped values
|Gs|Aircraft's G's readout. Can be useful for debugging|
|Altimeter|Aircraft's Altimeter readout. Can be useful for debugging. Can be customized with Altimeter Divisor. |3 Dividers
|Speedometer|Aircraft's Speed readout. Can be customized with Speed Divider| 3 Dividers
|Fuel|Aircraft's Fuel readout. Can be customized with Min Fuel and Max Fuel.| Min Fuel, Max Fuel as mapped values
|Gun Text| Gun Ammo readout in bullets. Requires to have DFUNC_GUN(s) assigned. Can be customized with Min ammo, Max Ammo. |Min Ammo, Max Ammo as mapped values
|Heading| Aircraft's absolute rotation in Y Axis readout. Returns 00X, 0XX, or XXX.|
|Rate of Climb in Seconds| Aircraft's Rate of climb in text.|


### Inspector Properties
|Variable Name|Required?|Description
|---|---|---|
|Engine Control|Y|The Aircraft's Sacc Air vehicle Object|
|DFUNC_Guns||Gun DFUNC's of the vehicle. You may add as many as you like; However, **GunText, Min Ammo and Max ammo** will be dependent on DFUNC_Guns's array order and length. E.g. DFUNC_Gun[0] are the 30mms, GunText[0] will represent 30mms value, MinAmmo[0] is the mininum gun amount for DFUNC_GUN[0], as well as MaxAmmo[0]; as DFUNC_Gun[1] represents another gun, as to GunText[1], MinAmmo[1], MaxAmmo[1]... etc.|
|Gun Text||Gun Text representative of each array entry. Will output the mapped value of the gun ammo.
Min Ammo||Min Gun ammo representative of each array entry. Unless for any other purposes, it should remain as 0.
Max Ammo ||Maximum Gun ammo representative of each array entry. Will represent the maximum loadout of the specific DFUNC_GUN.
|Altimeter Text||Array of Texts representative of the Altimeter. You may provide as many text objects as needed.
Show Altimeter Unit| | When enabled, each altimeter text will show a unit symbol (ft, km) 
Radar Altimeter Text||Array of texts representative of the Radar Altimeter. You may provide as many text objects as needed.
|Show Radar Altimeter Unit||When enabled, each radar altimeter text will show a unit symbol (ft, km)
Hide if Out of range ||Sets the text output of the radar altimeter texts to an empty string when the aircraft is out of the radar altimeter's range.
|Rate of Climb Seconds||Array of texts that represents the rate of climb in seconds. 
Show rate of climb unit||When enabled, shows unit symbol (ft/s, km/s)
Speedometer Text||Array of texts representative of the speedometer.
Show Speedometer unit||When enabled, shows unit symbol (kt, km)
Fuel Text||Array of texts representative of the mapped values of fuel. Setting up variables of **Fuel Lower Limit, Fuel upper limit** is required.
RPM Text||Array of texts representative of the aircraft's RPM. Setting up **RPM Lower**, **RPM Max** is required.
Fuel Consumption Text||Array of texts representative of the fuel consumption per second. **Fuel Consumption Multiplier** may need to be set first.
Temperature Text||Array of texts representative of the mapped values of temperature; This is **NOT** actual temperature, and is tied to the vehicle's RPM. Variables **Temp Lower** and **Temp Max** may be needed to be assigned.
Rotation Text Debug||When assigned, it will output the vehicle's rotational axis via X, Y, Z output to the text object.
Gs Text||Output's G's.
Heading Text||Outputs the Y Rotation of the vehicle relative to the world space of the vehicle. Offset is applied by the Y axis of the **Offsets** variable.
Goffset_value||Offset applied to G's. Default is 1.
Fuel Lower Limit||Mapped value lerped to when the fuel is empty.
Fuel Upper Limit||Mapped value lerped to when fuel is full.
Fuel Consumption Multiplier||Fuel consumption multiplier
**Animation Parameters**
Altimeter 1, 2 ,3||Animation parameters to use on each animation controller representative of 1,2,3. Each may signify Altitude in Hundreds, Thousands, Tens of Thousands.
Rate of Climb||Animation paramters to use for the rate of climb.
**Roll**||Animation parameters representing roll angle
**Pitch**||Animation parameters representing pitch angle
Heading||Animation parameters representing the heading / Y rotation of the vehicle relative to world space.
Gs||Animation parameters representing G's.
Mach||Animation parameters representing Mach
AOA||Animation parameters representing Angle of attack
Radar Altimeter 1, 2||Animation Parameters representing Radar Altimeter; Each may represent Hundreds, Thousands... depending on the divisor used.
Speedometer 1, 2, 3||Animation parameters represending the aircraft's speed. Each may represent tens, hundreds, thousands... depending on the divisor.
Afterburner||Animation parameter representing when the aircraft enters afterburner. Uses Bool.
Directional G's X, Y, Z, SideG||Animation parameters representing directional Gs'.
Velocity Rotation Indicator X, Y, Z||Animation parameters representing each rotational velocity's axes.
Velocity Indicator X, Y, Z||Animation parameters representing Velocity in X, Y, Z directions.
Pullup_anim||Animation paramater for pull up. Bool.
Canvas HUD Animator||Animator to use and set the parameters. You may use this if you're using only one animator.
Canvas HUD Animators||Array of animators to use and to set the parameters. You may assign more when needed.
**Setup**|| Each variable/value will be divided accordingly to set as a normalized value of the animator. (E.g. Maximum value of the instrument / gauge / hud.)
Lift Divisor|| Aircraft Velocity * 60 / LiftDivisor
Speed Divisor 1 ||Aircraft Speed / Speed Divisor 
Speed Divisor 2 ||Aircraft Speed / Speed Divisor 2
Speed Divisor 3 ||Aircraft Speed / Speed Divisor 3
Radar Altimeter Divisor||Aircraft Radar Altimeter Readout / RAD1
Radar Altimeter Divisor 2|| Aircraft Radar Altimeter Readout / RAD2
Radar Altimeter Max Distance|| Maximum distance before the radar altimeter stops reporting altitude
Alt Divisor 1 || Aircraft Altitude / Alt Divisor 1
Alt Divisor 2 || Aircraft Altitude / Alt Divisor 2
Alt Divisor 3 || Aircraft Altitude / Alt Divisor 3
Do Metric? || Switches between using **kt/ft** to **m / km**
Rotation Divider||  Aircraft Rotational Velocity / rotationDivider
Velocity Divider || Aircraft Directional Velocity / Velocity Divider
Gs3 Divider || Aircraft directional G's / GS3Divider
ZHK_Open World Movement Logic||**Optional**. Assign OWMLScript here to adapt with the moving map's altitude.
GDivider || Aircraft G's / GDivider
GOffset || Aircraft G's / GDivider + GOffset
MachDivider|| Aircraft Mach / MachDivider
AOADivider || Aircraft AOA / AoADivider.
AOADivider Offset || Aircraft AOA / AoADivider + AOAOffset
RPMLower || RPM Text's Lower RPM limit. The RPM Text will lerp towards this value on Throttle 0.
RPMMax || RPM Text's Upper RPM Limit. The RPM Text will lerp towards this value on Afterburner or Max throttle.
TempLower || Temperature text's lower temperature limit. Temperature Text will lerp towards this value on Throttle 0.
TempMax || Temperature text's upper temperature limit. Temp text will lerp towards this value on max throttle.
PullUp Distance||Any altitude below this distance will result the pull_up parameter to true.
Check Pullup||Enables the Radar Altimeter checker for the distance between your aircraft to the ground.
Check Pullup Every||When checkpullup is enabled, it will drop a raycast from the center of mass of the aircraft to check the distance between the aircraft to the ground on every X seconds.
RadarAltimeter Detector||Collision layer for the raycast to collide with for the radar altimeter
Offsets||X = Pitch (Default 0.5), Y = Heading (Default is 0), Z = Roll (Default 0.5).


## CanvasHUD to Instruments / Elements
This section will explain on how the script converts parameters from the vehicle to animation controller to animation.


### Attitude / Artificial Horizon Indicator

Suppose that you already have a model / canvas that indicates where the aircraft is facing at, you may have to create two empties before the mesh itself; One that represents Roll, and one that represents pitch.

![image](https://user-images.githubusercontent.com/19369963/234722827-15175562-7067-4658-997c-df17e186a73f.png)

#### Pitching

Let's focus on the pitching. Make an animation that represents the aircraft when facing -90 degrees down, level, then 90 degrees up. In short... First keyframe of the animation must represent -90 degrees. Second keyframe should represent the level state. Then the last keyframe must be +90 degrees. The distance between keyframes must be equal, the same way as how you would animate ailerons/elevators on your aircraft; Length shouldn't matter. 

![Blank diagram (5)](https://user-images.githubusercontent.com/19369963/234766539-b0d54860-be82-4aeb-bdb3-39752b36aeee.png)



Once done, go to your Animation Controller. Create the parameter that represents your pitching parameter as a float. If the layer does not exist, create a layer that represents your aircraft's pitch for the instrument/HUD.

![image](https://user-images.githubusercontent.com/19369963/234723047-0f07fd5d-6cb8-4301-9dc0-003d7f8e27c1.png)
![image](https://user-images.githubusercontent.com/19369963/234723076-b751a0d0-cfa3-44c7-8beb-e1f5f260fed1.png)



In your Layer, insert the animation. Set the animation's Motion time to the parameter you have created that represents the pitch. 

![image](https://user-images.githubusercontent.com/19369963/234722977-9c8fa38b-7f91-4348-a645-320dd3fca954.png)


In the CanvasHUD script, set the Pitch parameter. Set the animation controller as well if not assigned.

![image](https://user-images.githubusercontent.com/19369963/234723225-598e67d7-19ff-4aaf-9a98-e83d2cc755fa.png)
![image](https://user-images.githubusercontent.com/19369963/234723400-db486a6a-afab-42b9-92c7-ae2dac2a5fa5.png)

The end result should have the pitch animation similar to this.

![ezgif-2-f40f58ecdb](https://user-images.githubusercontent.com/19369963/234764574-7e2ca3f2-8f9d-4ff4-9104-83505e83b7dd.gif)

#### Rolling

![CanvasHUD - Rolling (3)](https://user-images.githubusercontent.com/19369963/234774075-39aeec69-9d77-44df-95c7-caeaabe572e8.png)

#### Heading

a

### Speedometer

### Altimeter

### Rate of Climb

### Gs

### Mach

### AOA

### Radar Altimeter

#### Pull up

### Velocity Indicator

### Rotational Velocity Indicator

