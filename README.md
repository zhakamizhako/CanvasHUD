# Zhakami Zhako's Gauge Script / Canvas HUD System
#### For use with SaccFlight; Not to be redistributed.
#### Discord: ZhakamiZhako#2147 | Twitter: [@ZZhako](https://twitter.com/ZZhako/) | zhintamizhakami@gmail.com


## Introduction

Sacchan's Flight and Vehicles (SaccFlight) is an amazing system when it comes to creating Aircraft, Boats and Vehicles in VRChat and yet lacks an easier way in order to create your own custom instruments.

This system aims to solve a problem where people cannot effectively create their own instruments / HUD / Effects via Animations, Animation parameters, Animation layers and Text Elements.


## The Script Explained

Here's a comprehensive list of possible value readouts that the CanvasHUD may be able to create

### Animations


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




### Text

|Function|Description|Parameters|
|---|---|---|
|RPM|Aircraft's RPM / Current Engine output|
