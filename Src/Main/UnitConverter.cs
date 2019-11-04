using System;
using USC.GISResearchLab.Common.GeographicObjects.Coordinates;

namespace USC.GISResearchLab.Common.Geographics.Units
{
    public class UnitConverter
    {


        public static double ConvertArea(LinearUnitTypes inputUnits, LinearUnitTypes outputUnits, double area)
        {
            double ret = -1;
            double conversionFactor = 0;
            try
            {
                if (area <= 0)
                {
                    ret = 0;
                }
                else
                {
                    if (inputUnits != outputUnits)
                    {
                        if (inputUnits == LinearUnitTypes.Meters)
                        {
                            conversionFactor = GetAreaConversionFactorFromMeters(outputUnits);
                        }
                        else if (outputUnits == LinearUnitTypes.Meters)
                        {
                            conversionFactor = GetAreaConversionFactorToMeters(inputUnits);
                        }
                        else
                        {
                            throw new Exception("Unsupported linear unit type was encountered: input: " + inputUnits + " outputUnits:" + outputUnits);
                        }

                        ret = area * conversionFactor;
                    }
                    else
                    {
                        ret = area;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("An Exception occurred in ConvertArea ", e);
            }

            return ret;
        }

        public static double ConvertLength(LinearUnitTypes inputUnits, LinearUnitTypes outputUnits, double length)
        {
            double ret = -1;
            try
            {
                if (length <= 0)
                {
                    ret = 0;
                }
                else
                {
                    double conversionFactor = 0;
                    if (inputUnits == LinearUnitTypes.Meters)
                    {
                        conversionFactor = GetLengthConversionFactorFromMeters(outputUnits);
                    }
                    else if (outputUnits == LinearUnitTypes.Meters)
                    {
                        conversionFactor = GetLengthConversionFactorToMeters(inputUnits);
                    }
                    else
                    {
                        throw new Exception("Unsupported linear unit type was encountered: input: " + inputUnits + " outputUnits:" + outputUnits);
                    }

                    ret = length * conversionFactor;
                }

            }
            catch (Exception e)
            {
                throw new Exception("An Exception occurred in ConvertLength ", e);
            }
            return ret;
        }

        public static double DMSToDD(double degrees, double minutes, double seconds)
        {
            return new DegreesMinutesSeconds(Convert.ToInt32(degrees), Convert.ToInt32(minutes), seconds).ToDecimalDegrees();
        }

        public static double[] DDToDMS(double degrees)
        {
            return new DecimalDegrees(degrees).ToDegreesMinutesSeconds();
        }

        public static double MetersPerDD(double latitude)
        {
            double offset = Math.Cos(latitude);
            double ret = LengthConversionConstants.METERS_PER_DEGREE_AT_EQUATOR * offset;
            return Math.Abs(ret);
        }

        public static double MetersToDD(double meters, double latitude)
        {
            return (meters / MetersPerDD(latitude));
        }

        public static double DD2Meters(double decimalDegrees, double latitude)
        {
            return (decimalDegrees * (MetersPerDD(latitude)));
        }

        // from last entry of http://answers.google.com/answers/threadview/id/342332.html
        public static double deg2rad(double deg)
        {
            double conv_factor = (2.0 * Math.PI) / 360.0;
            return (deg * conv_factor);
        }

        // from last entry of http://answers.google.com/answers/threadview/id/342332.html
        public static double rad2deg(double rad)
        {
            double conv_factor = 360 / (2.0 * Math.PI);
            return (rad * conv_factor);
        }

        // from last entry of http://answers.google.com/answers/threadview/id/342332.html
        public static double[] MetersPerDDAlternative(double latitude)
        {
            double[] ret = new double[2];

            // ret[0] = meters dper degree of latitude;
            // ret[1] = meters dper degree of longitude;

            // Convert latitude to radians
            double latRadians = deg2rad(latitude);

            // Set up "Constants"
            double m1 = 111132.92;		// latitude calculation term 1
            double m2 = -559.82;		// latitude calculation term 2
            double m3 = 1.175;			// latitude calculation term 3
            double m4 = -0.0023;		// latitude calculation term 4
            double p1 = 111412.84;		// longitude calculation term 1
            double p2 = -93.5;			// longitude calculation term 2
            double p3 = 0.118;			// longitude calculation term 3

            // Calculate the length of a degree of latitude and longitude in meters
            double latlen = m1 + (m2 * Math.Cos(2 * latRadians)) + (m3 * Math.Cos(4 * latRadians)) + (m4 * Math.Cos(6 * latRadians));
            double longlen = (p1 * Math.Cos(latRadians)) + (p2 * Math.Cos(3 * latRadians)) + (p3 * Math.Cos(5 * latRadians));

            // Place values in output fields
            double latmeters = Math.Round(latlen);
            double latfeet = Math.Round(latlen / 12 * 39.370079);
            double latsm = latfeet / 5280;
            double latnm = latsm / 1.15077945;
            double longmeters = Math.Round(longlen);
            double longfeet = Math.Round(longlen / 12 * 39.370079);
            double longsm = longfeet / 5280;
            double longnm = longsm / 1.15077945;

            ret[0] = latmeters;
            ret[1] = longmeters;

            return ret;
        }

        public static double GetAreaConversionFactorFromMeters(LinearUnitTypes output)
        {
            double ret = 0;
            switch (output)
            {
                case LinearUnitTypes.Feet:
                    ret = AreaConversionConstants.SQUARE_FEET_PER_SQUARE_METERS;
                    break;
                case LinearUnitTypes.Inches:
                    ret = AreaConversionConstants.SQUARE_INCHES_PER_SQUARE_METERS;
                    break;
                case LinearUnitTypes.Kilometers:
                    ret = AreaConversionConstants.SQUARE_KILOMETERS_PER_SQUARE_METERS;
                    break;
                case LinearUnitTypes.Miles:
                    ret = AreaConversionConstants.SQUARE_MILES_PER_SQUARE_METERS;
                    break;
                case LinearUnitTypes.Meters:
                    ret = 1;
                    break;
                case LinearUnitTypes.Yards:
                    ret = AreaConversionConstants.SQUARE_YARDS_PER_SQUARE_METERS;
                    break;
                default:
                    throw new Exception("An unsupported linear unit type was encountered: " + output);
            }
            return ret;
        }

        public static double GetLengthConversionFactorFromMeters(LinearUnitTypes output)
        {
            double ret = 0;
            switch (output)
            {
                case LinearUnitTypes.Feet:
                    ret = LengthConversionConstants.FEET_PER_METER;
                    break;
                case LinearUnitTypes.Inches:
                    ret = LengthConversionConstants.INCHES_PER_METER;
                    break;
                case LinearUnitTypes.Kilometers:
                    ret = LengthConversionConstants.KILOMETERS_PER_METER;
                    break;
                case LinearUnitTypes.Miles:
                    ret = LengthConversionConstants.MILE_PER_METER;
                    break;
                case LinearUnitTypes.Meters:
                    ret = 1;
                    break;
                case LinearUnitTypes.Yards:
                    ret = LengthConversionConstants.YARDS_PER_METER;
                    break;
                default:
                    throw new Exception("An unsupported linear unit type was encountered: " + output);
            }
            return ret;
        }

        public static double GetAreaConversionFactorToMeters(LinearUnitTypes input)
        {
            double ret = 0;
            switch (input)
            {
                case LinearUnitTypes.Feet:
                    ret = AreaConversionConstants.SQUARE_METERS_PER_SQUARE_FOOT;
                    break;
                case LinearUnitTypes.Inches:
                    ret = AreaConversionConstants.SQUARE_METERS_PER_SQUARE_INCH;
                    break;
                case LinearUnitTypes.Kilometers:
                    ret = AreaConversionConstants.SQUARE_METERS_PER_SQUARE_KILOMETERS;
                    break;
                case LinearUnitTypes.Miles:
                    ret = AreaConversionConstants.SQUARE_METERS_PER_SQUARE_MILE;
                    break;
                case LinearUnitTypes.Meters:
                    ret = 1;
                    break;
                case LinearUnitTypes.Yards:
                    ret = AreaConversionConstants.SQUARE_METERS_PER_SQUARE_YARD;
                    break;
                default:
                    throw new Exception("An unsupported linear unit type was encountered: " + input);
            }
            return ret;
        }

        public static double GetLengthConversionFactorToMeters(LinearUnitTypes input)
        {
            double ret = 0;
            switch (input)
            {
                case LinearUnitTypes.Feet:
                    ret = LengthConversionConstants.METERS_PER_FOOT;
                    break;
                case LinearUnitTypes.Inches:
                    ret = LengthConversionConstants.METERS_PER_INCH;
                    break;
                case LinearUnitTypes.Kilometers:
                    ret = LengthConversionConstants.METERS_PER_KILOMETER;
                    break;
                case LinearUnitTypes.Miles:
                    ret = LengthConversionConstants.METERS_PER_MILE;
                    break;
                case LinearUnitTypes.Meters:
                    ret = 1;
                    break;
                case LinearUnitTypes.Yards:
                    ret = LengthConversionConstants.METERS_PER_YARD;
                    break;
                default:
                    throw new Exception("An unsupported linear unit type was encountered: " + input);
            }
            return ret;
        }

    }
}


