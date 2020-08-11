using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGTAWPF
{
    class sucio
    {
    }
}
//tablaCAT21v21.Columns.Add("Number", typeof(int));  //0
//            tablaCAT21v21.Columns.Add("CAT number");  //1
//            tablaCAT21v21.Columns.Add("Category");  //2
//            tablaCAT21v21.Columns.Add("SAC"); //3
//            tablaCAT21v21.Columns.Add("SIC"); //4
//            tablaCAT21v21.Columns.Add("Target\nIdentification"); //5
//            tablaCAT21v21.Columns.Add("Track\n Number"); //6
//            tablaCAT21v21.Columns.Add("Target\nReport\nDescriptor"); //7
//            tablaCAT21v21.Columns.Add("Service\nIdentification"); //8
//            tablaCAT21v21.Columns.Add("Time of\nReport\nTransmission"); //9
//            tablaCAT21v21.Columns.Add("Position in WGS-84 co-ordinates"); //10
//            tablaCAT21v21.Columns.Add("Position in WGS-84 co-ordinates high res"); //11
//            tablaCAT21v21.Columns.Add("Air\nSpeed"); //12
//            tablaCAT21v21.Columns.Add("True Air\nSpeed"); //13
//            tablaCAT21v21.Columns.Add("Target Address"); //14
//            tablaCAT21v21.Columns.Add("Time of\nApplicability\nfor Position"); //0
//            tablaCAT21v21.Columns.Add("Time of\nMessage\nReception\nof Position"); //0
//            //        tablaCAT21v21.Columns.Add("Time of Message Reception of Position - High Precision");
//            tablaCAT21v21.Columns.Add("Time of\nApplicability\nfor Velocity"); //0
//            tablaCAT21v21.Columns.Add("Time of\nMessage\nReception\nof Velocity"); //0
//      //      tablaCAT21v21.Columns.Add("Time of Message Reception of Velocity - High Precision");
//            tablaCAT21v21.Columns.Add("Geometric\nHeight");  //0
//            tablaCAT21v21.Columns.Add("Quality\nIndicators");  //0
//            tablaCAT21v21.Columns.Add("MOPS Version"); 
//            tablaCAT21v21.Columns.Add("Mode\n3 / A\nCode");
//            tablaCAT21v21.Columns.Add("Roll\nAngle");
//            tablaCAT21v21.Columns.Add("Flight\nLevel");
//            tablaCAT21v21.Columns.Add("Magnetic\nHeading");
//            tablaCAT21v21.Columns.Add("Target\nStatus");
//            tablaCAT21v21.Columns.Add("Barometric\nVertical Rate");
//            tablaCAT21v21.Columns.Add("Geometric\nVertical Rate");
//            tablaCAT21v21.Columns.Add("Airborne Ground Vector");
//            tablaCAT21v21.Columns.Add("Track\nAngle\nRate");

//            tablaCAT21v21.Columns.Add("Emitter Category");
//            tablaCAT21v21.Columns.Add("Met\nInformation");
//            tablaCAT21v21.Columns.Add("Selected Altitude");
//            tablaCAT21v21.Columns.Add("Final\nState\nSelected\nAltitude");
//            tablaCAT21v21.Columns.Add("Trajectory\nIntent");
//            tablaCAT21v21.Columns.Add("Service\nManagement");
//            tablaCAT21v21.Columns.Add("Aircraft\nOperational\nStatus");
//            tablaCAT21v21.Columns.Add("Surface\nCapabilities\nand\nCharacteristics");
//            tablaCAT21v21.Columns.Add("Message\nAmplitude");
//            tablaCAT21v21.Columns.Add("Mode S MB Data");
//            tablaCAT21v21.Columns.Add("ACAS\nResolution\nAdvisory\nReport");
//            tablaCAT21v21.Columns.Add("Receiver\nID");
//            tablaCAT21v21.Columns.Add("Data Ages");
//var row = tablaCAT10.NewRow();
//row["Number"] = Message.num;
//            row["CAT number"] = Message.cat10num;
//            if (Message.CAT != null) { row["Category"] = Message.CAT; }
//            else { row["Category"] = "No Data"; }
//            if (Message.SAC != null) { row["SAC"] = Message.SAC; }
//            else { row["SAC"] = "No Data"; }
//            if (Message.SIC != null) { row["SIC"] = Message.SIC; }
//            else { row["SIC"] = "No Data"; }
//            if (Message.TAR != null)
//            {
//                if (Message.TAR.Replace(" ","")!="" ) { row["Target\nIdentification"] = Message.TAR; }
//                else { row["Target\nIdentification"] = "No Data"; }
//            }
//            else { row["Target\nIdentification"] = "No Data"; }
//            if (Message.TYP != null) { row["Target\nReport\nDescriptor"] = "Click to expand"; }
//            else { row["Target\nReport\nDescriptor"] = "No Data"; }
//            if (Message.MESSAGE_TYPE != null) { row["Message Type"] = Message.MESSAGE_TYPE; }
//            else { row["Message Type"] = "No Data"; }
//            if (Message.Flight_Level != null) { row["Flight Level"] = Message.Flight_Level; }
//            else { row["Flight Level"] = "No Data"; }
//            if (Message.Track_Number != null) { row["Track\nNumber"] = Message.Track_Number; }
//            else { row["Track\nNumber"] = "No Data"; }
//            if (Message.Time_Of_Day != null) { row["Time of\nDay"] = Message.Time_Of_Day; }
//            else { row["Time of\nDay"] = "No Data"; }
//            if (Message.CNF != null) { row["Track Status"] = "Click to expand"; }
//            else { row["Track Status"] = "No Data"; }
//            if (Message.Latitude_in_WGS_84 != null && Message.Longitude_in_WGS_84 != null) { row["Position in WGS-84 Co-ordinates"] = "Latitude: " + Message.Latitude_in_WGS_84 + ", Longitude: " + Message.Longitude_in_WGS_84; }
//            else { row["Position in WGS-84 Co-ordinates"] = "No Data"; }
//            if (Message.Position_Cartesian_Coordinates != null) { row["Position in\nCartesian\nCo-ordinates"] = Message.Position_Cartesian_Coordinates; }
//            else { row["Position in\nCartesian\nCo-ordinates"] = "No Data"; }
//            if (Message.Position_In_Polar != null) { row["Position in\nPolar\nCo-ordinates"] = Message.Position_In_Polar; }
//            else { row["Position in\nPolar\nCo-ordinates"] = "No Data"; }
//            if (Message.Track_Velocity_Polar_Coordinates != null) { row["Track Velocity in Polar Coordinates"] = Message.Track_Velocity_Polar_Coordinates; }
//            else { row["Track Velocity in Polar Coordinates"] = "No Data"; }
//            if (Message.Track_Velocity_in_Cartesian_Coordinates != null) { row["Track Velocity in\nCartesian\nCoordinates"] = Message.Track_Velocity_in_Cartesian_Coordinates; }
//            else { row["Track Velocity in\nCartesian\nCoordinates"] = "No Data"; }
//            if (Message.Target_size_and_orientation != null) { row["Target Size\nand\nOrientation"] = Message.Target_size_and_orientation; }
//            else { row["Target Size\nand\nOrientation"] = "No Data"; }
//            if (Message.Target_Address != null) { row["Target\nAddress"] = Message.Target_Address; }
//            else { row["Target\nAddress"] = "No Data"; }
//            if (Message.NOGO != null) { row["System\nStatus"] = "Click to expand"; }
//            else { row["System\nStatus"] = "No Data"; }
//            if (Message.VFI != null) { row["Vehicle Fleet\nIdentification"] = Message.VFI; }
//            else { row["Vehicle Fleet\nIdentification"] = "No Data"; }
//            if (Message.Pre_programmed_message != null) { row["Pre-programmed\nMessage"] = Message.Pre_programmed_message; }
//            else { row["Pre-programmed\nMessage"] = "No Data"; }
//            if (Message.Measured_Height != null) { row["Measured\nHeight"] = Message.Measured_Height; }
//            else { row["Measured\nHeight"] = "No Data"; }
//            if (Message.Mode_3A != null) { row["Mode\n3 / A\nCode"] = "Click to expand"; }
//            else { row["Mode\n3 / A\nCode"] = "No Data"; }
//            if (Message.MB_Data != null) { row["Mode S MB\nData"] = "Click to expand"; }
//            else { row["Mode S MB\nData"] = "No Data"; }
//            if (Message.Deviation_X != null) { row["Standard\nDeviation\nof Position"] = "Click to expand"; }
//            else { row["Standard\nDeviation\nof Position"] = "No Data"; }
//            if (Message.REP_Presence != 0) { row["Presence"] = "Click to expand"; }
//            else { row["Presence"] = "No Data"; }
//            if (Message.PAM != null) { row["Amplitude\nof Primary\nPlot"] = Message.PAM; }
//            else { row["Amplitude\nof Primary\nPlot"] = "No Data"; }
//            if (Message.Calculated_Acceleration != null) { row["Calculated\nAcceleration"] = Message.Calculated_Acceleration; }
//            else { row["Calculated\nAcceleration"] = "No Data"; }