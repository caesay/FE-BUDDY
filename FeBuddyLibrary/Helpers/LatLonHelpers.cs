﻿using System;
using System.Linq;

namespace FeBuddyLibrary.Helpers
{
    public class LatLonHelpers
    {
        /// <summary>
        /// Convert the Lat AND Lon from the FAA Data into a standard [N-S-E-W]000.00.00.000 format.
        /// </summary>
        /// <param name="value">Needs to have 3 decimal points, and one of the following Letters [N-S-E-W]</param>
        /// <param name="Lat">Is this value a Lat, if so Put true</param>
        /// <param name="ConvertEast">Do you need ALL East Coords converted, if so put true.</param>
        /// <returns>standard [N-S-E-W]000.00.00.000 lat/lon format</returns>
        public static string CorrectLatLon(string value, bool Lat, bool ConvertEast)
        {
            // Valid format is N000.00.00.000 W000.00.000
            string correctedValue;

            // Split the value based on these char
            char[] splitValue = new char[] { '.', '-' };

            // Declare our variables.
            string degrees;
            string minutes;
            string seconds;
            string milSeconds;
            string declination = "";

            // If the value is a Lat
            if (Lat)
            {
                // Find the Character "N"
                if (value.IndexOf("N", 0, value.Length) != -1)
                {
                    // Set Declination to N
                    declination = "N";

                    // Delete the N from the Value
                    value = value.Replace("N", "");
                }

                // Find the Char "S"
                else if (value.IndexOf("S", 0, value.Length) != -1)
                {
                    // Set declination to S
                    declination = "S";

                    // Remove the S from our Value
                    value = value.Replace("S", "");
                }

                // Split the Value by our chars we defined above.
                string[] valueSplit = value.Split(splitValue);

                // Set our Variables
                degrees = valueSplit[0];
                minutes = valueSplit[1];
                seconds = valueSplit[2];

                // Check the length of our Milliseconds. 
                if (valueSplit[3].Length > 3)
                {
                    // If its greater then 3 we only want to keep the first three.
                    milSeconds = valueSplit[3].Substring(0, 3);
                }
                else
                {
                    // our the length is less than or equal to three so just set it. 
                    milSeconds = valueSplit[3];
                }

                // Correct Format is now set.
                correctedValue = $"{declination}{degrees.PadLeft(3, '0')}.{minutes.PadRight(2, '0')}.{seconds.PadRight(2, '0')}.{milSeconds.PadRight(3, '0')}";
            }

            // Value is a Lon
            else
            {
                // Check for "E" Char
                if (value.IndexOf("E", 0, value.Length) != -1)
                {
                    // Set Declination to E
                    declination = "E";

                    //Remove the E from our Value
                    value = value.Replace("E", "");
                }

                // Check for "W" char
                else if (value.IndexOf("W", 0, value.Length) != -1)
                {
                    // Set Declination to W
                    declination = "W";

                    // Remove the W from our Value.
                    value = value.Replace("W", "");
                }

                // Value does not have an E or W. 
                else
                {
                    // There has been an error in the value passed in.
                    return value;
                }

                // Split the value by our char we defined above.
                string[] valueSplit = value.Split(splitValue);

                // Set all of our variables.
                degrees = valueSplit[0];
                minutes = valueSplit[1];
                seconds = valueSplit[2];

                if (valueSplit[3].Length > 3)
                {
                    // If its greater then 3 we only want to keep the first three.
                    milSeconds = valueSplit[3].Substring(0, 3);
                }
                else
                {
                    // our the length is less than or equal to three so just set it. 
                    milSeconds = valueSplit[3];
                }


                // Set the Corrected Value
                correctedValue = $"{declination}{degrees.PadLeft(3, '0')}.{minutes.PadRight(2, '0')}.{seconds.PadRight(2, '0')}.{milSeconds.PadRight(3, '0')}";
            }

            // Check to see if Convert E is True and Check our Value's Declination to make sure it is an E Coord.
            if (ConvertEast && declination == "E")
            {
                double oldDecForm = double.Parse(CreateDecFormat(correctedValue, false));

                double newDecForm = 180 - oldDecForm;

                newDecForm = (newDecForm + 180) * -1;

                correctedValue = CreateDMS(newDecForm, false);

                // Return the corrected value. 
                return correctedValue;
            }

            // No Conversion needed
            else
            {
                // Return the corrected Value
                return correctedValue;
            }

        }

        public static string CreateDMS(double value, bool lat)
        {

            int degrees;
            decimal degreeFloat = 0;

            int minutes;
            decimal minuteFloat = 0;

            int seconds;
            decimal secondFloat = 0;

            string miliseconds = "0";

            string dms;

            degrees = int.Parse(value.ToString().Split('.')[0]);

            if (value.ToString().Split('.').Count() > 1)
            {
                degreeFloat = decimal.Parse("0." + value.ToString().Split('.')[1]);
            }

            minutes = int.Parse((degreeFloat * 60).ToString().Split('.')[0]);

            if ((degreeFloat * 60).ToString().Split('.').Count() > 1)
            {
                minuteFloat = decimal.Parse("0." + (degreeFloat * 60).ToString().Split('.')[1]);
            }

            seconds = int.Parse((minuteFloat * 60).ToString().Split('.')[0]);

            if ((minuteFloat * 60).ToString().Split('.').Count() > 1)
            {
                secondFloat = decimal.Parse("0." + (minuteFloat * 60).ToString().Split('.')[1]);
            }

            secondFloat = Math.Round(secondFloat, 3);

            if (secondFloat.ToString().Split('.').Count() > 1)
            {
                miliseconds = secondFloat.ToString().Split('.')[1];
            }


            if (lat)
            {
                if (degrees < 0)
                {
                    degrees *= -1;
                    dms = $"S{degrees.ToString().PadLeft(3, '0')}.{minutes.ToString().PadLeft(2, '0')}.{seconds.ToString().PadLeft(2, '0')}.{miliseconds.ToString().PadLeft(3, '0')}";
                }
                else
                {
                    dms = $"N{degrees.ToString().PadLeft(3, '0')}.{minutes.ToString().PadLeft(2, '0')}.{seconds.ToString().PadLeft(2, '0')}.{miliseconds.ToString().PadLeft(3, '0')}";
                }
            }
            else
            {
                if (degrees < 0)
                {
                    degrees *= -1;
                    dms = $"W{degrees.ToString().PadLeft(3, '0')}.{minutes.ToString().PadLeft(2, '0')}.{seconds.ToString().PadLeft(2, '0')}.{miliseconds.ToString().PadLeft(3, '0')}";
                }
                else
                {
                    dms = $"E{degrees.ToString().PadLeft(3, '0')}.{minutes.ToString().PadLeft(2, '0')}.{seconds.ToString().PadLeft(2, '0')}.{miliseconds.ToString().PadLeft(3, '0')}";
                }
            }

            return dms;
        }

        /// <summary>
        /// Do some math to convert lat/lon to decimal format
        /// </summary>
        /// <param name="value">lat OR Lon</param>
        /// <returns>Decimal Format of the value past in.</returns>
        public static string CreateDecFormat(string value, bool roundSixPlaces)
        {
            // Split the value at decimal points.
            string[] splitValue = value.Split('.');

            // set our values
            string declination = splitValue[0].Substring(0, 1);
            string degrees = splitValue[0].Substring(1, 3);
            string minutes = splitValue[1];
            string seconds = splitValue[2];
            string miliSeconds = splitValue[3];
            string decFormatSeconds = $"{seconds}.{miliSeconds}";

            // Do some math with all of our variables.
            string decFormat = (double.Parse(degrees) + (double.Parse(minutes) / 60) + (double.Parse(decFormatSeconds) / 3600)).ToString();

            // Check the Declination
            if (declination == "S" || declination == "W")
            {
                // if it is S or W it needs to be a negative number.
                decFormat = $"-{decFormat}";
            }

            if (roundSixPlaces)
            {
                // Round the Decimal format to 6 places after the decimal.
                decFormat = Math.Round(double.Parse(decFormat), 6).ToString();
            }

            // Return the Decimal Format.
            return decFormat;
        }

    }
}
