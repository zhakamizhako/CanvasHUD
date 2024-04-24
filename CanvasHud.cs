using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using SaccFlightAndVehicles;


/*
/ 	Zhakami Modules - SaccFlight Gauge Script / Animator Driver
/ 	For use Internally in Sacchill/Jetboys and selected persons
/ 	D: ZhakamiZhako#2147 | TW: @ZZhako | zhintamizhakami@gmail.com
/
/ 	This script is meant to output values as animations based on the aircraft's behaviour. Including
/ the current rotation of the plane (Roll, pitch, yaw), Current speed, the rate of climb, Gs', AOA, the
/ distance of the aircraft to the ground dynamically (radar altimeter), the current altitude, Current RPM,
/ Current Velocity, Directional G's, Mach indicator (basic) even a pull up warning indicator. 
/ By default, values provided in the divisor will be based on knots/ft. Enabling useMetric will change them to
/ km / kmph / m. 
/ 
/ 
/ Guide:
/ Type					Animation (0 to 1)				Animation Loop
/ ADI Horizontal - 			-180, 0, +180					Looped
/ ADI Pitch - 				-90, 0, +90					Looped
/ Heading / Compass - 			0 to +360					Looped
/ Rate of Climb			min, 0, max					Not Looped
/ Speed1 / Speedometer	(Tens)		0 to maximum (SpeedDivisor1)			Either
/ Speed2 / Speedometer (Hunds)	0 to maximum (SpeedDivisor2)			Either
/ Speed3 / Speedometer (Thousands)	0 to maximum (SpeedDivisor3)			Either
/ Altimeter1 (Hundreds)		0 to maximum (altimeter1)			Looped
/ Altimeter2 (Thousands)		0 to maximum (altimeter2)			Looped
/ Altimeter3 (Tens of Thousands)	0 to maximum (altimeter3)			Looped
/ G's					min, 0, max					Either
/ Mach					0 to maximum (machdivider)			Either
/ RPM					0 to maximum (Based on SAV)			Not Looped
/ AOA					min, 0, max					Not Looped
/ Afterburner				Bool						Either
/ Gs_X					min, 0, max					Not Looped
/ Gs_Y					min, 0, max					Not Looped
/ Gs_Z					min, 0, max					Not looped
/ Side_G (Slide slip)			min, 0, max					Not Looped
/ VEL_R_X (Velocity Rotation X)	min, 0, max					Not Looped
/ VEL_R_Y (Velocity Rotation Y)	min, 0, max					Not looped
/ VEL_R_Z (Velocity Rotation Z)	min, 0, max					Not looped
/ VEL_I_X (Velocity Indicator X)	min, 0, max					Not Looped
/ VEL_I_Y (Velocity Indicator Y)	min, 0, max					Not looped
/ VEL_I_Z (Velcity Indicator Z)	min, 0, max					Not looped
/ Radar Altimeter 1			0 to maximum					Either
/ Radar Altimeter 2			0 to maximum					Either
/ Pull up				Bool						Either
*/


public class CanvasHud : UdonSharpBehaviour //Rename to GaugeScript
{
    [InspectorName("SAV Controller")] public SaccAirVehicle EngineControl;
    public DFUNC_Gear gear;
    public DFUNC_Gun[] DFUNC_Guns;
    public Text[] GunText;
    public int[] minAmmo;
    public int[] maxAmmo;

    [Header("Text Objects")] public Text[] AltimeterText;
    public bool showAltimeterUnit;

    public Text[] RadarAltimeterText;
    public bool showRadarAltimeterUnit;
    public bool hideIfOutofRange = true;

    public Text[] RateOfClimbSeconds;
    public bool showRateOfClimbUnit;

    public Text[] SpeedometerText;
    public bool showSpeedometerUnit;

    public Text[] FuelText;
    public Text[] RPMText;
    public Text[] FuelConsumptionText;
    public Text[] TemperatureText;
    
    public Text RotationTextDebug;
    public Text[] GsText;
    public Text[] HeadingText;
    

    public float goffset_value = 1;

    public float FuelLowerLimit = 0f;
    public float FuelUpperLimit = 23280;
    public float fuelConsumptionMultiplier = 60;

    [Header("Animation Strings")] [Header("Altimeter")]
    public string altimeter1 = "altimeter1";

    public string altimeter2 = "altimeter2";
    public string altimeter3 = "altimeter3";
    [Header("Rate of Climb")] public string rateOfClimb = "liftneedle";
    [Header("ADI")] public string roll = "level";
    public string pitch = "pitch";
    public string heading = "heading";
    public string gs = "gs";
    public string mach = "mach";
    public string aoa = "aoa";
    [Header("Radar Altimeter")] public string radarAltimeter1 = "radarAltimeter";
    public string radarAltimeter2 = "radarAltimeter2";
    public string RPM = "RPML";
    [Header("Speedometer")] public string speed1 = "speed1";
    public string speed2 = "speed2";
    public string speed3 = "speed3";
    public string afterburner = "AB";
    [Header("Directional G's")] public string Gs_X = "Gs_X";
    public string Gs_Y = "Gs_Y";
    public string Gs_Z = "Gs_Z";
    public string Side_G = "SideG";

    [Header("Velocity Rotation Indicator")]
    public string VEL_R_X = "VEL_R_X";

    public string VEL_R_Y = "VEL_R_Y";
    public string VEL_R_Z = "VEL_R_Z";

    [Header("Velocity Indicator")] public string VEL_I_X = "VEL_I_X";
    public string VEL_I_Y = "VEL_I_Y";
    public string VEL_I_Z = "VEL_I_Z";

    public string pullup_anim = "pull_up";


    // public SAV_EffectsController EffectsControl;


    [Header("Animators")] public Animator CanvasHUDAnimator;
    public Animator[] CanvasHUDAnimators;

    [Header("Setup")] [InspectorName("Rate Of Climb Divisor")]
    public float LiftDivisor = 1500f;

    [Header("Speedometer(s)")] public float SpeedDivisor1 = 360;
    public float SpeedDivisor2 = 360;
    public float SpeedDivisor3 = 360; // A third one because you all suck.
    [Header("Radar Altimeter(s)")] public float RadarAltimeterDivisor = 10000;
    public float RadarAltimeterDivisor2 = 10000;
    public float RadarAltimeterMaxDistance = 10000;
    [Header("Altimeter(s)")] public float AltDivisor1 = 1000;
    public float AltDivisor2 = 10000;
    public float AltDivisor3 = 100000;

    [Header("Misc")] [InspectorName("Use Metric")]
    public bool doMetric = false;

    [InspectorName("Velocity Rotation Divider (Seconds)")]
    public float rotationDivider = 60;

    [InspectorName("Velocity Divider")] public float velocityDivider = 1;
    public float Gs3Divider = 1f;
    public UdonBehaviour ZHK_OpenWorldMovementLogic;
    public float gdivider = 20;
    public float goffset = .5f;
    public float machdivider = 2;
    public float aoadivider = 30;
    public float aoadivideroffset = .5f;
    
    public float RPMLower = 60;
    public float RPMMax = 110;

    public float TempLower = 600;
    public float TempMax = 960;
    private float TempTemp = 0f;


    public float pullupDistance = 200f;
    public bool checkPullup = false;
    public float checkPullupEvery = 1f; // Checking pull up every x seconds

    public LayerMask RadarAltimeterDetector;

    public Vector3 offsets = new Vector3(.5f, 0, .5f);

    private float lastRPM = 0;
    private float checkpulluptimer = 0f;
    private int TempAltimeterVal = 0;
    private int TempRadarAltimeterVal = 0;
    private int TempSpeedometerVal = 0;
    private int TempHeadingVal = 0;
    [System.NonSerialized] private RaycastHit hit;
    private float _radarAltimeterDistance = 0f;
    private float _altimeterDistance = 0f;
    private Transform Map;
    private Vector3 xVel;

    private float SideG = 0f;

    // private Vector3 VelocityVectorBefore;
    private Vector3 CurrentVelocityVector;

    private Vector3 CurrentVelocityIndicator;
    // private Transform VehicleTransform;

    private bool pullup;

    private float fuelNormalize = 0f;
    private float consumptionRate = 0f;
    private float maxFuel = 0f;
    private float tempFuelConsumption = 0f;
    private float fuelconsumptionOutput = 0f;
    private float rpmoutput;
    private float tempOutput;

    private float angle = 0f;
    private float speed = 0f;
    private float gravity = 0f;

    

    public void Start()
    {
        if (ZHK_OpenWorldMovementLogic != null && ZHK_OpenWorldMovementLogic.GetProgramVariable("Map") != null)
        {
            Map = (Transform) ZHK_OpenWorldMovementLogic.GetProgramVariable("Map");
        }

        // VehicleTransform = EngineControl.VehicleTransform;
        maxFuel = EngineControl.FullFuel;
    }

    void Update()
    {
        if (EngineControl != null)
        {
            angle = -EngineControl.CenterOfMass.transform.rotation.eulerAngles.y;

            _altimeterDistance = (EngineControl.CenterOfMass.position.y -
                                  EngineControl.SeaLevel - (Map != null ? Map.position.y : 0)) *
                                 (doMetric ? (1) : 3.28084f);

            speed = EngineControl.CurrentVel.magnitude * (doMetric ? 1 : 1.9438445f);

            if (checkpulluptimer < checkPullupEvery)
            {
                checkpulluptimer = checkpulluptimer + Time.deltaTime;
            }
            else
            {
                Physics.Raycast(EngineControl.GroundDetector.position, -(EngineControl.GroundDetector.up), out hit,
                    RadarAltimeterMaxDistance, RadarAltimeterDetector, QueryTriggerInteraction.UseGlobal);
                _radarAltimeterDistance = hit.collider != null
                    ? Vector3.Distance(EngineControl.CenterOfMass.transform.position, hit.point) *
                      (doMetric ? 1 : 3.28084f)
                    : _altimeterDistance;
                checkpulluptimer = 0f;
            }

            xVel = transform.InverseTransformDirection(EngineControl.VehicleRigidbody.angularVelocity);

            if (RateOfClimbSeconds != null && RateOfClimbSeconds.Length > 0)
            {
                foreach (Text x in RateOfClimbSeconds)
                {
                    x.text = ((EngineControl.CurrentVel.y * (doMetric ? 1 : 3.28084f))) +
                                              (showRateOfClimbUnit ? (doMetric ? "m/s" : "ft/s") : "");    
                }
                
            }

            if (AltimeterText != null)
            {
                int testAltimeterVal = Mathf.RoundToInt(_altimeterDistance);
                if (testAltimeterVal != TempAltimeterVal)
                {
                    TempAltimeterVal = testAltimeterVal;
                    foreach(var x in AltimeterText) x.text = string.Format("{0}" + (showAltimeterUnit ? (doMetric ? "m" : "ft") : ""),
                        testAltimeterVal);
                }
            }

            if (SpeedometerText != null && SpeedometerText.Length > 0)
            {
                int testSpeedometerVal = Mathf.RoundToInt(speed);
                if (testSpeedometerVal != TempSpeedometerVal)
                {
                    TempSpeedometerVal = testSpeedometerVal;
                    foreach (Text x in SpeedometerText)
                    {
                        x.text =
                            string.Format("{0}" + (showSpeedometerUnit ? (doMetric ? "km/h" : "kt") : ""),
                                testSpeedometerVal);    
                    }
                }
            }

            if (FuelText != null && FuelText.Length > 0)
            {
                fuelNormalize = EngineControl.Fuel / maxFuel;
                float mappedValue = Mathf.Lerp(FuelLowerLimit, FuelUpperLimit, fuelNormalize);
                
                foreach(Text xx in FuelText)
                {
                    xx.text = Mathf.RoundToInt((mappedValue))+"";
                }
            }
            
            if(GunText!=null && DFUNC_Guns!=null && GunText.Length > 0) calculateGunAmmo();

            if (FuelConsumptionText != null && FuelConsumptionText.Length > 0)
            {
                 consumptionRate = Mathf.Lerp(EngineControl.MinFuelConsumption, EngineControl.FuelConsumption,
                    EngineControl.EngineOutput);
                if (EngineControl.AfterburnerOn)
                {
                    consumptionRate = EngineControl.FuelConsumptionAB;
                }

                tempFuelConsumption = Mathf.Lerp(tempFuelConsumption, ((consumptionRate * FuelUpperLimit) / 60f)*fuelConsumptionMultiplier, Time.deltaTime);
                fuelconsumptionOutput = Mathf.Lerp(fuelconsumptionOutput,
                    EngineControl.EngineOn ? tempFuelConsumption : 0, Time.deltaTime);

                foreach (Text xx in FuelConsumptionText)
                {
                    xx.text = Mathf.RoundToInt(fuelconsumptionOutput) + "";
                }
            }

            if (TemperatureText != null && TemperatureText.Length > 0)
            {
                TempTemp = Mathf.Lerp(TempLower, TempMax, EngineControl.EngineOutput);
                tempOutput = Mathf.Lerp(tempOutput, EngineControl.EngineOn ? TempTemp : 0, Time.deltaTime);
                foreach (var x in TemperatureText)
                {
                    x.text = "" + Mathf.RoundToInt(tempOutput);
                }
            }

            if (GsText != null && GsText.Length > 0)
            {
                foreach(var x in GsText) x.text = EngineControl.VertGs + "";
            }

            if (RPMText != null)
            {
                float tempRPM = EngineControl.EngineOutput;
                float mappedValue = Mathf.Lerp(RPMLower, RPMMax, tempRPM);
                rpmoutput = Mathf.Lerp(rpmoutput,EngineControl.EngineOn ? mappedValue:0, Time.deltaTime);
                if (tempRPM != lastRPM)
                {
                    foreach (var x in RPMText)
                    {
                        // x.text = "" + (tempRPM + rpmOffset);
                        x.text = "" + Mathf.RoundToInt(rpmoutput);
                    }
                }

                lastRPM = tempRPM;
            }

            if (RotationTextDebug != null)
            {
                RotationTextDebug.text = "X: " + xVel.x + "\nY:" + xVel.y + "\nZ:" + xVel.z;
            }

            if (RadarAltimeterText != null && RadarAltimeterText.Length > 0)
            {
                int testAltimeterVal = Mathf.RoundToInt(_radarAltimeterDistance);
                if (hideIfOutofRange && _radarAltimeterDistance > RadarAltimeterMaxDistance)
                {
                    foreach(var x in RadarAltimeterText) x.text = "";
                }
                else
                {
                    if (testAltimeterVal != TempRadarAltimeterVal)
                    {
                        TempRadarAltimeterVal = testAltimeterVal;
                        foreach (var x in RadarAltimeterText)
                        {
                            x.text =
                                string.Format("{0}" + (showRadarAltimeterUnit ? (doMetric ? "m" : "ft") : ""),
                                    testAltimeterVal);    
                        }
                        
                        
                    }
                }
            }

            if (HeadingText != null)
            {
                int testHeadingVal = Mathf.RoundToInt(angle) * -1;
                if (testHeadingVal != TempHeadingVal)
                {
                    TempHeadingVal = testHeadingVal;
                    foreach(var x in HeadingText) x.text = testHeadingVal < 10 ? "00" + testHeadingVal : testHeadingVal < 100 ? "0" + testHeadingVal : testHeadingVal + "";
                }
            }

            CurrentVelocityVector = EngineControl.CurrentVel;
            CurrentVelocityIndicator = transform.InverseTransformDirection(EngineControl.VehicleRigidbody.velocity);

            gravity = 9.81f * Time.deltaTime;
            Vector3 Gs3 = EngineControl.Gs3;
            SideG = Gs3.x / (gravity + Gs3.y);

            // VelocityVectorBefore = CurrentVelocityVector;

            if (CanvasHUDAnimator)
            {
                ProcessAnimator(CanvasHUDAnimator);
            }

            if (CanvasHUDAnimators != null)
            {
                foreach (Animator x in CanvasHUDAnimators)
                {
                    ProcessAnimator(x);
                }
            }
        }
    }
    
    void calculateGunAmmo()
    {
        for (int xx = 0; xx < DFUNC_Guns.Length; xx++)
        {
            float normalized = DFUNC_Guns[xx].GunAmmoInSeconds / DFUNC_Guns[xx].FullGunAmmoInSeconds;
            float mappedValue = Mathf.Lerp(minAmmo[xx], maxAmmo[xx], normalized);

            GunText[xx].text = Mathf.RoundToInt(mappedValue) + "";
        }
    }

    public void ProcessAnimator(Animator x)
    {
        if (x != null)
        {
            if(altimeter1!="") x.SetFloat(altimeter1, (_altimeterDistance / AltDivisor1));
            if(altimeter2!="") x.SetFloat(altimeter2, (_altimeterDistance / AltDivisor2));
            if(altimeter3!="") x.SetFloat(altimeter3, (_altimeterDistance / AltDivisor3));
            if(rateOfClimb!="")x.SetFloat(rateOfClimb,
                ((EngineControl.CurrentVel.y * (doMetric ? 1 : 3.28084f)) * 60) / LiftDivisor +
                .5f); // Note: Lift on every 60 seconds / Total value  ?. 

            //Level
            var body = EngineControl.CenterOfMass.transform.rotation.eulerAngles;
            if(roll!="") x.SetFloat(roll, (body.z / 360) + offsets.z);
            if(pitch!="") x.SetFloat(pitch, (body.x / 180) + offsets.x);

            //heading

            if(heading!="") x.SetFloat(heading, (angle / 360) + offsets.y);

            if(gs!="") x.SetFloat(gs, ((EngineControl.VertGs + goffset_value) / gdivider) + goffset);
            if(mach!="") x.SetFloat(mach, (speed / 343f) / machdivider);
            if(aoa!="") x.SetFloat(aoa, (EngineControl.AngleOfAttack / aoadivider) + aoadivideroffset);

            if(radarAltimeter1!="")x.SetFloat(radarAltimeter1,
                Mathf.Lerp(x.GetFloat(radarAltimeter1), _radarAltimeterDistance / RadarAltimeterDivisor,
                    Time.deltaTime));
            if(radarAltimeter2!="")x.SetFloat(radarAltimeter2,
                Mathf.Lerp(x.GetFloat(radarAltimeter2), _radarAltimeterDistance / RadarAltimeterDivisor2,
                    Time.deltaTime));

            if(RPM!="")x.SetFloat(RPM, EngineControl.EngineOutput);
            if(speed1!="") x.SetFloat(speed1, (((speed) * (doMetric ? 1 : 1.9438445f)) / SpeedDivisor1));
            if(speed2!="") x.SetFloat(speed2, (((speed) * (doMetric ? 1 : 1.9438445f)) / SpeedDivisor2));
            if(speed3!="") x.SetFloat(speed3, (((speed) * (doMetric ? 1 : 1.9438445f)) / SpeedDivisor3));
            if(afterburner!="")x.SetBool(afterburner, EngineControl.AfterburnerOn);

            //xVel.y yaw per minute, for use with turn and slip indicator
            if(VEL_R_X!="") x.SetFloat(VEL_R_X, (xVel.x / rotationDivider) + .5f);
            if(VEL_R_Y!="") x.SetFloat(VEL_R_Y, (xVel.y / rotationDivider) + .5f);
            if(VEL_R_Z!="") x.SetFloat(VEL_R_Z, (xVel.z / rotationDivider) + .5f);

            if(VEL_I_X!="") x.SetFloat(VEL_I_X, (CurrentVelocityIndicator.x / velocityDivider) + .5f);
            if(VEL_I_Y!="") x.SetFloat(VEL_I_Y, (CurrentVelocityIndicator.y / velocityDivider) + .5f);
            if(VEL_I_Z!="") x.SetFloat(VEL_I_Z, (CurrentVelocityIndicator.z / velocityDivider) + .5f);

            //EngineControl.Gs3. Need to determine gravity + g's where gravity = 0.5f
            if(Gs_X!="") x.SetFloat(Gs_X, (EngineControl.Gs3.x / Gs3Divider) + .5f);
            if(Gs_Y!="") x.SetFloat(Gs_Y, (EngineControl.Gs3.y / Gs3Divider) + .5f);
            if(Gs_Z!="") x.SetFloat(Gs_Z, (EngineControl.Gs3.z / Gs3Divider) + .5f);

            if(Side_G!="")x.SetFloat(Side_G, Mathf.Lerp(x.GetFloat("SideG"), (SideG / gravity) + .5f, Time.deltaTime));

            if (checkPullup)
            {
                if(pullup_anim!="")
                if (gear && gear.GearUp)
                {
                    if (hit.collider != null && _radarAltimeterDistance < (pullupDistance * (doMetric ? 1 : 3.28084f)))
                        x.SetBool(pullup_anim, true);
                    else x.SetBool(pullup_anim, false);
                }
                else
                {
                    x.SetBool(pullup_anim, false);
                }
            }
        }
    }
}