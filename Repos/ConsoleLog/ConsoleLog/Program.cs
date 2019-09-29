using HtmlAgilityPack;
using Microsoft.Office.Interop.Word;
using System;
using log4net;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Configuration;

namespace ConsoleLog
{
    class Program
    {
        /// <summary>
        /// Required Packages from
        /// Install-Package HtmlAgilityPack -Version 1.8.5
        /// Install-Package Microsoft.Office.Interop.Word -Version 15.0.4797.1003
        /// </summary>
        /// <param name="args"></param>
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static void Main(string[] args)
        {
            var folderPath = ConfigurationManager.AppSettings["FileSource"];
            ParseHtml(folderPath);
        }

        public static void ParseHtml(string folderPath)
        {
            MassDataContext massDataContext = new MassDataContext();
            ConsoleLog consoleLog = new ConsoleLog();

            Application word = new Application();

            var sourcePath = folderPath;
            var archivePath = Directory.CreateDirectory($@"{folderPath}\Archive");
            var notProcessedPath = Directory.CreateDirectory($@"{folderPath}\NotProcessed");

            var files = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories)
                .Where(file => new string[] { ".htm", ".html" }
                .Contains(Path.GetExtension(file)))
                .ToList();

            foreach (var file in files)
            {
                try
                {
                    var unsupportedCharacters = new HashSet<char>("\r\n");
                    var text = File.ReadAllText(file);

                    String source = WebUtility.HtmlDecode(text);
                    HtmlDocument htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(source);
                    string title = null;
                    var fileName = Path.GetFileName(file);
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);

                    if (htmlDocument.DocumentNode.SelectNodes("//title") == null)
                    {
                        MoveFileAndFolder(file, notProcessedPath);
                        Log.Fatal($"There was a problem reading file {fileName} and moved to {notProcessedPath.FullName} folder");
                        continue;
                    }
                    else
                    {
                        title = htmlDocument.DocumentNode.SelectNodes("//title").ToList()[0].InnerHtml;
                    }

                    var isExists = massDataContext.ConsoleLogs.Where(a => a.Title == title);
                    massDataContext.ConsoleLogs.RemoveRange(isExists);
                    massDataContext.SaveChanges();
                    if (isExists.Count() == 0)
                    {
                        var query = from table in htmlDocument.DocumentNode.SelectNodes("//table")
                                    from row in table.SelectNodes("tr")
                                    from cell in row.SelectNodes("th|td")
                                    select new { Table = table.Id, CellText = cell.InnerText };

                        var result = query.Select(g => string.Join("", g.CellText.Where(c => !unsupportedCharacters.Contains(c)))).ToList();

                        List<string> innerTextResult = new List<string>();
                        foreach (var item in result)
                        {
                            var wordtWithoutSpace = Regex.Replace(item, " {2,}", " ");
                            innerTextResult.Add(wordtWithoutSpace);
                        }
                        Log.Info($"Started reading file {file}");

                        #region Main

                        int indexOfGMTDay = 0;
                        var indexOfGMTDayNewandMid = innerTextResult.FindIndex(g => g.Equals(" GMT Day: "));
                        var indexOfGMTDayOld = innerTextResult.FindIndex(g => g.Equals(" GMT "));
                        if (indexOfGMTDayNewandMid > 0)
                        {
                            indexOfGMTDay = indexOfGMTDayNewandMid;
                            var GMTDayData = innerTextResult[indexOfGMTDay + 1];
                            consoleLog.GMTDay = GMTDayData;
                        }
                        if (indexOfGMTDayOld > 0)
                        {
                            indexOfGMTDay = indexOfGMTDayOld;
                            var GMTDayDataforOld = innerTextResult[indexOfGMTDay + 1];
                            var GMTDayData = GMTDayDataforOld.Substring(9, 3);
                            consoleLog.GMTDay = GMTDayData;
                        }

                        int indexOfReportDate = 0;
                        var indexOfReportDateNewAndMid = innerTextResult.FindIndex(g => g.Equals(" Report Date: "));
                        var indexOfReportDateOld = innerTextResult.FindIndex(g => g.Equals(" Date "));

                        if (indexOfReportDateNewAndMid > 0)
                        {
                            indexOfReportDate = indexOfReportDateNewAndMid;
                            var reportdatedata = innerTextResult[indexOfReportDate + 1];
                            DateTime parsedDate = DateTime.Parse(reportdatedata);
                            consoleLog.ReportDate = parsedDate;
                        }
                        if (indexOfReportDateOld > 0)
                        {
                            var GMTDayforOld = innerTextResult[indexOfGMTDayOld + 1];
                            var GMTYearForOld = Convert.ToInt32(GMTDayforOld.Substring(2, 4));
                            indexOfReportDate = indexOfReportDateOld;
                            var reportdatedatas = innerTextResult[indexOfReportDate + 1];
                            DateTime parsedDates = DateTime.Parse(reportdatedatas);
                            DateTime newDate = new DateTime(GMTYearForOld, parsedDates.Month, parsedDates.Day);
                            consoleLog.ReportDate = newDate;
                        }

                        int indexOfGMTHours = 0;
                        var indexOfGMTHoursNewAndMid = innerTextResult.FindIndex(g => g.Equals(" GMT Hours: "));
                        var indexOfGMTHoursOld = innerTextResult.FindIndex(g => g.Equals(" Hours "));
                        if (indexOfGMTHoursNewAndMid > 0)
                        {
                            indexOfGMTHours = indexOfGMTHoursNewAndMid;
                        }
                        if (indexOfGMTHoursOld > 0)
                        {
                            indexOfGMTHours = indexOfGMTHoursOld;
                        }
                        var GMTHours = innerTextResult[indexOfGMTHours + 1];

                        var indexOfIncrement = 0;
                        var indexOfIncrementNew = innerTextResult.FindIndex(g => g.Equals(" Increment: "));
                        var indexOfIncrementMid = innerTextResult.FindIndex(g => g.Equals(" Flight: "));
                        var indexOfIncrementOld = innerTextResult.FindIndex(g => g.Equals(" Flight "));
                        if (indexOfIncrementNew > 0)
                        {
                            indexOfIncrement = indexOfIncrementNew;
                        }
                        if (indexOfIncrementMid > 0)
                        {
                            indexOfIncrement = indexOfIncrementMid;
                        }
                        if (indexOfIncrementOld > 0)
                        {
                            indexOfIncrement = indexOfIncrementOld;
                        }
                        var Increment = innerTextResult[indexOfIncrement + 1];

                        int indexOfPreparedByESL = 0;
                        var indexOfPreparedByESLNewAndMid = innerTextResult.FindIndex(g => g.Equals(" Prepared by (ESL): "));

                        if (indexOfPreparedByESLNewAndMid > 0)
                        {
                            indexOfPreparedByESL = indexOfPreparedByESLNewAndMid;
                            var PreparedByESL = innerTextResult[indexOfPreparedByESL + 1];
                            consoleLog.PreparedByESL = PreparedByESL;
                        }

                        int indexOfShiftDescription = 0;
                        var indexOfShiftDescriptionNewAndMid = innerTextResult.FindIndex(g => g.Equals(" Shift Description: "));
                        var indexOfShiftDescriptionOld = innerTextResult.FindIndex(g => g.Equals(" Shift Description "));
                        if (indexOfShiftDescriptionNewAndMid > 0)
                        {
                            indexOfShiftDescription = indexOfShiftDescriptionNewAndMid;
                        }
                        if (indexOfShiftDescriptionOld > 0)
                        {
                            indexOfShiftDescription = indexOfShiftDescriptionOld;
                        }
                        var ShiftDescription = innerTextResult[indexOfShiftDescription + 1];

                        consoleLog.Title = title;
                        consoleLog.GMTHours = GMTHours;
                        consoleLog.Increment = Increment;
                        consoleLog.ShiftDescription = ShiftDescription;

                        #endregion

                        #region Personnel
                        int indexOfOECLead = 0;
                        var indexOfOECLeadNew = innerTextResult.FindIndex(g => g.Equals(" OEC Lead "));
                        var indexOfOECLeadMidandOld = innerTextResult.FindIndex(g => g.Equals(" OEC Lead: "));
                        if (indexOfOECLeadNew > 0)
                        {
                            indexOfOECLead = indexOfOECLeadNew;
                        }
                        if (indexOfOECLeadMidandOld > 0)
                        {
                            indexOfOECLead = indexOfOECLeadMidandOld;
                        }
                        var OEC_Lead = innerTextResult[indexOfOECLead + 1];


                        int indexOfOECESL = 0;
                        var indexOfOECESLNew = innerTextResult.FindIndex(g => g.Equals(" ESL "));
                        var indexOfOECESLMid = innerTextResult.FindIndex(g => g.Equals(" ESL: "));
                        if (indexOfOECESLNew > 0)
                        {
                            indexOfOECESL = indexOfOECESLNew;
                        }
                        if (indexOfOECESLMid > 0)
                        {
                            indexOfOECESL = indexOfOECESLMid;
                        }
                        var OEC_ESL = innerTextResult[indexOfOECESL + 1];


                        int indexOfOECESC = 0;
                        var indexOfOECESCNew = innerTextResult.FindIndex(g => g.Equals(" ESC "));
                        var indexOfOECESCMid = innerTextResult.FindIndex(g => g.Equals(" ESC: "));
                        var indexOfOECESCOld = innerTextResult.FindIndex(g => g.Equals(" ESC Tech Lead: "));
                        if (indexOfOECESCNew > 0)
                        {
                            indexOfOECESC = indexOfOECESCNew;
                        }
                        else if (indexOfOECESCOld > 0 && indexOfOECESCMid > 0)
                        {
                            indexOfOECESC = indexOfOECESCOld;
                        }
                        else
                        {
                            indexOfOECESC = indexOfOECESCMid;
                        }
                        var OEC_ESC = innerTextResult[indexOfOECESC + 1];


                        int indexOfFPL = 0;
                        var indexOfFPLNew = innerTextResult.FindIndex(g => g.Equals(" FPL "));
                        var indexOfFPLMidandOld = innerTextResult.FindIndex(g => g.Equals(" FPL: "));
                        if (indexOfFPLNew > 0)
                        {
                            indexOfFPL = indexOfFPLNew;
                        }
                        if (indexOfFPLMidandOld > 0)
                        {
                            indexOfFPL = indexOfFPLMidandOld;
                        }
                        var OEC_FPL = innerTextResult[indexOfFPL + 1];


                        int indexOfOEC_OPR = 0;
                        var indexOfOECOPRNew = innerTextResult.FindIndex(g => g.Equals(" OPR "));
                        var indexOfOECOPRMid = innerTextResult.FindIndex(g => g.Equals(" OPR: "));
                        if (indexOfOECOPRNew > 0)
                        {
                            indexOfOEC_OPR = indexOfOECOPRNew;
                            var OEC_OPR = innerTextResult[indexOfOEC_OPR + 1];
                            consoleLog.OEC_OPR = OEC_OPR;
                        }
                        if (indexOfOECOPRMid > 0)
                        {
                            indexOfOEC_OPR = indexOfOECOPRMid;
                            var OEC_OPR = innerTextResult[indexOfOEC_OPR + 1];
                            consoleLog.OEC_OPR = OEC_OPR;
                        }


                        int indexOfROBOLead = 0;
                        var indexOfROBOLeadNew = innerTextResult.FindIndex(g => g.Equals(" ROBO "));
                        var indexOfROBOLeadMidandOld = innerTextResult.FindIndex(g => g.Equals(" ROBO: "));
                        if (indexOfROBOLeadNew > 0)
                        {
                            indexOfROBOLeadNew = innerTextResult.FindIndex(g => g.Equals(" ROBO ")) + 2;
                            indexOfROBOLead = indexOfROBOLeadNew;
                        }
                        if (indexOfROBOLeadMidandOld > 0)
                        {
                            indexOfROBOLeadMidandOld = innerTextResult.FindIndex(g => g.Equals(" ROBO: ")) + 2;
                            indexOfROBOLead = indexOfROBOLeadMidandOld;
                        }
                        var ROBOLeadData = innerTextResult[indexOfROBOLead];

                        int indexOfROBOSystems = 0;
                        var indexOfROBOSystemsNew = innerTextResult.FindIndex(g => g.Equals(" ROBO "));
                        var indexOfROBOSystemsMidandOld = innerTextResult.FindIndex(g => g.Equals(" ROBO: "));
                        if (indexOfROBOSystemsNew > 0)
                        {
                            indexOfROBOSystemsNew = innerTextResult.FindIndex(g => g.Equals(" ROBO ")) + 6;
                            indexOfROBOSystems = indexOfROBOSystemsNew;
                        }
                        if (indexOfROBOSystemsMidandOld > 0)
                        {
                            indexOfROBOSystemsMidandOld = innerTextResult.FindIndex(g => g.Equals(" ROBO: ")) + 4;
                            indexOfROBOSystems = indexOfROBOSystemsMidandOld;
                        }
                        var ROBOSystemsData = innerTextResult[indexOfROBOSystems];

                        int indexOfROBOTask = 0;
                        var indexOfROBOTaskNew = innerTextResult.FindIndex(g => g.Equals(" ROBO "));
                        var indexOfROBOTaskMidandOld = innerTextResult.FindIndex(g => g.Equals(" ROBO: "));
                        if (indexOfROBOTaskNew > 0)
                        {
                            indexOfROBOTaskNew = innerTextResult.FindIndex(g => g.Equals(" ROBO ")) + 10;
                            indexOfROBOTask = indexOfROBOTaskNew;
                        }
                        if (indexOfROBOTaskMidandOld > 0)
                        {
                            if (innerTextResult[indexOfROBOTaskMidandOld + 7] == " Task: ")
                            {
                                indexOfROBOTask = innerTextResult.FindIndex(g => g.Equals(" ROBO: ")) + 8;
                            }
                            else
                            {
                                indexOfROBOTask = innerTextResult.FindIndex(g => g.Equals(" ROBO: ")) + 6;
                            }
                        }
                        var ROBOTaskData = innerTextResult[indexOfROBOTask];

                        int indexOfEVR = 0;
                        var indexOfEVRNew = innerTextResult.FindIndex(g => g.Equals(" EVR "));
                        var indexOfEVRMidnadOld = innerTextResult.FindIndex(g => g.Equals(" EVR: "));
                        if (indexOfEVRNew > 0)
                        {
                            indexOfEVRNew = innerTextResult.FindIndex(g => g.Equals(" EVR ")) + 1;
                            indexOfEVR = indexOfEVRNew;
                        }
                        if (indexOfEVRMidnadOld > 0)
                        {
                            indexOfEVRMidnadOld = innerTextResult.FindIndex(g => g.Equals(" EVR: ")) + 2;
                            indexOfEVR = indexOfEVRMidnadOld;
                        }
                        var EVRData = innerTextResult[indexOfEVR];

                        int indexOfIPCanada = 0;
                        var indexOfIPCanadaNew = innerTextResult.FindIndex(g => g.Equals(" IP Canada "));
                        var indexOfIPCanadaMid = innerTextResult.FindIndex(g => g.Equals(" IP Canada: "));
                        var indexOfIPCanadaOld = innerTextResult.FindIndex(g => g.Equals(" IP Can: "));
                        if (indexOfIPCanadaNew > 0)
                        {
                            indexOfIPCanada = indexOfIPCanadaNew;
                        }
                        if (indexOfIPCanadaMid > 0)
                        {
                            indexOfIPCanada = indexOfIPCanadaMid;
                        }
                        if (indexOfIPCanadaOld > 0)
                        {
                            indexOfIPCanada = indexOfIPCanadaOld;
                        }
                        var IPCanadaData = innerTextResult[indexOfIPCanada + 1];

                        consoleLog.OEC_OECLead = OEC_Lead;
                        consoleLog.OEC_ESL = OEC_ESL;
                        consoleLog.OEC_ESC = OEC_ESC;
                        consoleLog.OEC_FPL = OEC_FPL;
                        consoleLog.ROBO_Lead = ROBOLeadData;
                        consoleLog.ROBO_Systems = ROBOSystemsData;
                        consoleLog.ROBO_Task = ROBOTaskData;
                        consoleLog.EVR = EVRData;
                        consoleLog.IPCanada = IPCanadaData;

                        #endregion

                        #region Shift Summary
                        int indexOfShiftSummary = 0;
                        var indexOfShiftSummaryNew = innerTextResult.FindIndex(g => g.Equals(" Shift Summary "));
                        var indexOfShiftSummaryMid = innerTextResult.FindIndex(g => g.Equals(" Shift Summary: "));
                        var indexOfShiftSummaryOld = innerTextResult.FindIndex(g => g.Equals(" Shift Summary/Highlights "));
                        if (indexOfShiftSummaryNew > 0)
                        {
                            indexOfShiftSummary = indexOfShiftSummaryNew;
                        }
                        if (indexOfShiftSummaryMid > 0)
                        {
                            indexOfShiftSummary = indexOfShiftSummaryMid;
                        }
                        if (indexOfShiftSummaryOld > 0)
                        {
                            indexOfShiftSummary = indexOfShiftSummaryOld;
                        }
                        var ShiftSummaryData = innerTextResult[indexOfShiftSummary + 1];

                        consoleLog.ShiftSummary = ShiftSummaryData;

                        #endregion

                        #region Forward/Open Work
                        int indexOfForwardOrOpenWork = 0;
                        var indexOfForwardOrOpenWorkNew = innerTextResult.FindIndex(g => g.Equals(" Forward/Open Work "));
                        var indexOfForwardOrOpenWorkMid = innerTextResult.FindIndex(g => g.Equals(" Forward/Open Work: "));
                        var indexOfForwardOrOpenWorkOld = innerTextResult.FindIndex(g => g.Equals(" Forward Work "));
                        if (indexOfForwardOrOpenWorkNew > 0)
                        {
                            indexOfForwardOrOpenWork = indexOfForwardOrOpenWorkNew;
                        }
                        if (indexOfForwardOrOpenWorkMid > 0)
                        {
                            indexOfForwardOrOpenWork = indexOfForwardOrOpenWorkMid;
                        }
                        if (indexOfForwardOrOpenWorkOld > 0)
                        {
                            indexOfForwardOrOpenWork = indexOfForwardOrOpenWorkOld;
                        }
                        var ForwardOrOpenWorkData = innerTextResult[indexOfForwardOrOpenWork + 1];

                        consoleLog.ForwardOrOpenWork = ForwardOrOpenWorkData;
                        #endregion

                        #region MSS Status at Start of Shift 
                        int indexOfMSSStatusAtStartofShift = 0;
                        int indexOfMSSStatusAtEndofShift = 0;
                        var indexOfMSSStatusAtStartofShiftNew = innerTextResult.FindIndex(g => g.Equals(" MSS Status at Start of Shift "));
                        var indexOfMSSStatusAtEndofShiftNew = innerTextResult.FindIndex(g => g.Equals(" MSS Status at End of Shift "));
                        var indexOfMSSStatusAtStartofShiftMid = innerTextResult.FindIndex(g => g.Equals(" MSS Status at Start of Shift: "));
                        var indexOfMSSStatusAtEndofShiftMid = innerTextResult.FindIndex(g => g.Equals(" MSS Status at End of Shift: "));

                        if (indexOfMSSStatusAtStartofShiftNew > 0 && indexOfMSSStatusAtEndofShiftNew > 0)
                        {
                            try
                            {
                                indexOfMSSStatusAtStartofShift = indexOfMSSStatusAtStartofShiftNew;
                                indexOfMSSStatusAtEndofShift = indexOfMSSStatusAtEndofShiftNew;

                                var MSSStatusatStartofShiftIndexRange = innerTextResult.GetRange(indexOfMSSStatusAtStartofShift, indexOfMSSStatusAtEndofShift - indexOfMSSStatusAtStartofShift + 1);
                                var MSSStart_RWS_Lab = innerTextResult[indexOfMSSStatusAtStartofShift + 15];

                                var MSSStart_RWS_Cup = innerTextResult[indexOfMSSStatusAtStartofShift + 16];
                                var MSSStart_MT_WS = innerTextResult[indexOfMSSStatusAtStartofShift + 17];
                                var MSSStart_SSRMS_LEE = innerTextResult[indexOfMSSStatusAtStartofShift + 18];
                                var MSSStart_SPDM_BasedOn = innerTextResult[indexOfMSSStatusAtStartofShift + 19];
                                var MSSStart_FSTStates_MBS = innerTextResult[indexOfMSSStatusAtStartofShift + 20];
                                var MSSStart_FSTStates_POA = innerTextResult[indexOfMSSStatusAtStartofShift + 21];
                                var MSSStart_FSTStates_SSRMS = innerTextResult[indexOfMSSStatusAtStartofShift + 22];
                                var MSSStart_FSTStates_SPDMPSU = innerTextResult[indexOfMSSStatusAtStartofShift + 23];
                                var MSSStart_FSTStates_SPDMBody = innerTextResult[indexOfMSSStatusAtStartofShift + 24];
                                var MSSStart_FSTStates_SPDMSACU = innerTextResult[indexOfMSSStatusAtStartofShift + 25];
                                var MSSStart_FSTStates_SPDMArms = innerTextResult[indexOfMSSStatusAtStartofShift + 26];
                                var MSSStart_Payloads_SSRMSTipLee = innerTextResult[indexOfMSSStatusAtStartofShift + 35];
                                var MSSStart_Payloads_SPDMLee = innerTextResult[indexOfMSSStatusAtStartofShift + 36];
                                var MSSStart_Payloads_OTCM1 = innerTextResult[indexOfMSSStatusAtStartofShift + 37];
                                var MSSStart_Payloads_OTCM2 = innerTextResult[indexOfMSSStatusAtStartofShift + 38];
                                var MSSStart_Payloads_MCAS = MSSStatusatStartofShiftIndexRange[MSSStatusatStartofShiftIndexRange.FindIndex(g => g.Equals(" MCAS ")) + 7];
                                var MSSStart_Payloads_EOTP = MSSStatusatStartofShiftIndexRange[MSSStatusatStartofShiftIndexRange.FindIndex(g => g.Equals(" EOTP ")) + 7];
                                var MSSStart_Payloads_POA = innerTextResult[indexOfMSSStatusAtStartofShift + 41];
                                var MSSStart_SSRMSConfiguration = innerTextResult[indexOfMSSStatusAtStartofShift + 43];
                                var MSSStart_SPDMConfiguration = innerTextResult[indexOfMSSStatusAtStartofShift + 45];
                                var MSSStart_Joints_SSRMS_SR = innerTextResult[indexOfMSSStatusAtStartofShift + 56];
                                var MSSStart_Joints_SSRMS_SY = innerTextResult[indexOfMSSStatusAtStartofShift + 57];
                                var MSSStart_Joints_SSRMS_SP = innerTextResult[indexOfMSSStatusAtStartofShift + 58];
                                var MSSStart_Joints_SSRMS_EP = innerTextResult[indexOfMSSStatusAtStartofShift + 59];
                                var MSSStart_Joints_SSRMS_WP = innerTextResult[indexOfMSSStatusAtStartofShift + 60];
                                var MSSStart_Joints_SSRMS_WY = innerTextResult[indexOfMSSStatusAtStartofShift + 61];
                                var MSSStart_Joints_SSRMS_WR = innerTextResult[indexOfMSSStatusAtStartofShift + 62];
                                var MSSStart_Joints_SSRMS_SPDMBR = innerTextResult[indexOfMSSStatusAtStartofShift + 63];
                                var MSSStart_Joints_ARM1_SR = innerTextResult[indexOfMSSStatusAtStartofShift + 65];
                                var MSSStart_Joints_ARM1_SY = innerTextResult[indexOfMSSStatusAtStartofShift + 66];
                                var MSSStart_Joints_ARM1_SP = innerTextResult[indexOfMSSStatusAtStartofShift + 67];
                                var MSSStart_Joints_ARM1_EP = innerTextResult[indexOfMSSStatusAtStartofShift + 68];
                                var MSSStart_Joints_ARM1_WP = innerTextResult[indexOfMSSStatusAtStartofShift + 69];
                                var MSSStart_Joints_ARM1_WY = innerTextResult[indexOfMSSStatusAtStartofShift + 70];
                                var MSSStart_Joints_ARM1_WR = innerTextResult[indexOfMSSStatusAtStartofShift + 71];
                                var MSSStart_Joints_ARM2_SR = innerTextResult[indexOfMSSStatusAtStartofShift + 74];
                                var MSSStart_Joints_ARM2_SY = innerTextResult[indexOfMSSStatusAtStartofShift + 75];
                                var MSSStart_Joints_ARM2_SP = innerTextResult[indexOfMSSStatusAtStartofShift + 76];
                                var MSSStart_Joints_ARM2_EP = innerTextResult[indexOfMSSStatusAtStartofShift + 77];
                                var MSSStart_Joints_ARM2_WP = innerTextResult[indexOfMSSStatusAtStartofShift + 78];
                                var MSSStart_Joints_ARM2_WY = innerTextResult[indexOfMSSStatusAtStartofShift + 79];
                                var MSSStart_Joints_ARM2_WR = innerTextResult[indexOfMSSStatusAtStartofShift + 80];
                                var MSSStart_Joints_EOTP = innerTextResult[indexOfMSSStatusAtStartofShift + 81];

                                consoleLog.MSSStart_RWS_Lab = MSSStart_RWS_Lab;
                                consoleLog.MSSStart_RWS_Cup = MSSStart_RWS_Cup;
                                consoleLog.MSSStart_MT_WS = Convert.ToInt32(MSSStart_MT_WS);
                                consoleLog.MSSStart_SSRMS_BasedOn_LEE = MSSStart_SSRMS_LEE;
                                consoleLog.MSSStart_SPDM_BasedOn = MSSStart_SPDM_BasedOn;
                                consoleLog.MSSStart_FSTStates_MBS = MSSStart_FSTStates_MBS;
                                consoleLog.MSSStart_FSTStates_POA = MSSStart_FSTStates_POA;
                                consoleLog.MSSStart_FSTStates_SSRMS = MSSStart_FSTStates_SSRMS;
                                consoleLog.MSSStart_FSTStates_SPDMPSU = MSSStart_FSTStates_SPDMPSU;
                                consoleLog.MSSStart_FSTStates_SPDMBody = MSSStart_FSTStates_SPDMBody;
                                consoleLog.MSSStart_FSTStates_SPDMSACU = MSSStart_FSTStates_SPDMSACU;
                                consoleLog.MSSStart_FSTStates_SPDMArms = MSSStart_FSTStates_SPDMArms;
                                consoleLog.MSSStart_Payloads_SSRMSTipLee = MSSStart_Payloads_SSRMSTipLee;
                                consoleLog.MSSStart_Payloads_SPDMLee = MSSStart_Payloads_SPDMLee;
                                consoleLog.MSSStart_Payloads_OTCM1 = MSSStart_Payloads_OTCM1;
                                consoleLog.MSSStart_Payloads_OTCM2 = MSSStart_Payloads_OTCM2;
                                consoleLog.MSSStart_Payloads_MCAS = MSSStart_Payloads_MCAS;
                                consoleLog.MSSStart_Payloads_EOTP = MSSStart_Payloads_EOTP;
                                consoleLog.MSSStart_Payloads_POA = MSSStart_Payloads_POA;
                                consoleLog.MSSStart_SSRMSConfiguration = MSSStart_SSRMSConfiguration;
                                consoleLog.MSSStart_SPDMConfiguration = MSSStart_SPDMConfiguration;
                                consoleLog.MSSStart_Joints_SSRMS_SR = Decimal.Parse(MSSStart_Joints_SSRMS_SR);
                                consoleLog.MSSStart_Joints_SSRMS_SY = Decimal.Parse(MSSStart_Joints_SSRMS_SY);
                                consoleLog.MSSStart_Joints_SSRMS_SP = Decimal.Parse(MSSStart_Joints_SSRMS_SP);
                                consoleLog.MSSStart_Joints_SSRMS_EP = Decimal.Parse(MSSStart_Joints_SSRMS_EP);
                                consoleLog.MSSStart_Joints_SSRMS_WP = Decimal.Parse(MSSStart_Joints_SSRMS_WP);
                                consoleLog.MSSStart_Joints_SSRMS_WY = Decimal.Parse(MSSStart_Joints_SSRMS_WY);
                                consoleLog.MSSStart_Joints_SSRMS_WR = Decimal.Parse(MSSStart_Joints_SSRMS_WR);
                                consoleLog.MSSStart_Joints_SSRMS_SPDMBR = Decimal.Parse(MSSStart_Joints_SSRMS_SPDMBR);
                                consoleLog.MSSStart_Joints_ARM1_SR = Decimal.Parse(MSSStart_Joints_ARM1_SR);
                                consoleLog.MSSStart_Joints_ARM1_SY = Decimal.Parse(MSSStart_Joints_ARM1_SY);
                                consoleLog.MSSStart_Joints_ARM1_SP = Decimal.Parse(MSSStart_Joints_ARM1_SP);
                                consoleLog.MSSStart_Joints_ARM1_EP = Decimal.Parse(MSSStart_Joints_ARM1_EP);
                                consoleLog.MSSStart_Joints_ARM1_WP = Decimal.Parse(MSSStart_Joints_ARM1_WP);
                                consoleLog.MSSStart_Joints_ARM1_WY = Decimal.Parse(MSSStart_Joints_ARM1_WY);
                                consoleLog.MSSStart_Joints_ARM1_WR = Decimal.Parse(MSSStart_Joints_ARM1_WR);
                                consoleLog.MSSStart_Joints_ARM2_SR = Decimal.Parse(MSSStart_Joints_ARM2_SR);
                                consoleLog.MSSStart_Joints_ARM2_SY = Decimal.Parse(MSSStart_Joints_ARM2_SY);
                                consoleLog.MSSStart_Joints_ARM2_SP = Decimal.Parse(MSSStart_Joints_ARM2_SP);
                                consoleLog.MSSStart_Joints_ARM2_EP = Decimal.Parse(MSSStart_Joints_ARM2_EP);
                                consoleLog.MSSStart_Joints_ARM2_WP = Decimal.Parse(MSSStart_Joints_ARM2_WP);
                                consoleLog.MSSStart_Joints_ARM2_WY = Decimal.Parse(MSSStart_Joints_ARM2_WY);
                                consoleLog.MSSStart_Joints_ARM2_WR = Decimal.Parse(MSSStart_Joints_ARM2_WR);
                                consoleLog.MSSStart_Joints_EOTP = Decimal.Parse(MSSStart_Joints_EOTP);
                            }
                            catch (Exception e)
                            {
                                Log.Fatal($"There is a problem reading file { fileName} in MSS Status at Start of Shift table and moved to notProcessed folder");
                                Log.Fatal(e.Message);
                                MoveFileAndFolder(file, notProcessedPath);
                                continue;
                            }
                            
                        }
                        else if (indexOfMSSStatusAtStartofShiftMid > 0 && indexOfMSSStatusAtEndofShiftMid > 0)
                        {
                            try
                            {
                                indexOfMSSStatusAtStartofShift = indexOfMSSStatusAtStartofShiftMid;
                                indexOfMSSStatusAtEndofShift = indexOfMSSStatusAtEndofShiftMid;

                                var MSSStatusatStartofShiftIndexRange = innerTextResult.GetRange(indexOfMSSStatusAtStartofShift, indexOfMSSStatusAtEndofShift - indexOfMSSStatusAtStartofShift + 1);
                                var MSSStart_RWS_Lab = innerTextResult[indexOfMSSStatusAtStartofShift + 15];
                                var MSSStart_RWS_Cup = innerTextResult[indexOfMSSStatusAtStartofShift + 16];
                                var MSSStart_MT_WS = innerTextResult[indexOfMSSStatusAtStartofShift + 17];
                                var MSSStart_SSRMS_BasedOn_LEE = innerTextResult[indexOfMSSStatusAtStartofShift + 18];
                                var MSSStart_SPDM_BasedOn = innerTextResult[indexOfMSSStatusAtStartofShift + 19];
                                var MSSStart_FSTStates_MBS = innerTextResult[indexOfMSSStatusAtStartofShift + 20];
                                var MSSStart_FSTStates_POA = innerTextResult[indexOfMSSStatusAtStartofShift + 21];
                                var MSSStart_FSTStates_SSRMS = innerTextResult[indexOfMSSStatusAtStartofShift + 22];
                                var MSSStart_FSTStates_SPDMPSU = innerTextResult[indexOfMSSStatusAtStartofShift + 23];
                                var MSSStart_FSTStates_SPDMBody = innerTextResult[indexOfMSSStatusAtStartofShift + 24];
                                var MSSStart_FSTStates_SPDMSACU = innerTextResult[indexOfMSSStatusAtStartofShift + 25];
                                var MSSStart_FSTStates_SPDMArms = innerTextResult[indexOfMSSStatusAtStartofShift + 26];
                                var MSSStart_Payloads_SSRMSTipLee = innerTextResult[indexOfMSSStatusAtStartofShift + 35];
                                var MSSStart_Payloads_SPDMLee = innerTextResult[indexOfMSSStatusAtStartofShift + 36];
                                var MSSStart_Payloads_OTCM1 = innerTextResult[indexOfMSSStatusAtStartofShift + 37];
                                var MSSStart_Payloads_OTCM2 = innerTextResult[indexOfMSSStatusAtStartofShift + 38];
                                var MSSStart_Payloads_MCAS = MSSStatusatStartofShiftIndexRange[MSSStatusatStartofShiftIndexRange.FindIndex(g => g.Equals(" MCAS ")) + 7];
                                var MSSStart_Payloads_EOTP = MSSStatusatStartofShiftIndexRange[MSSStatusatStartofShiftIndexRange.FindIndex(g => g.Equals(" EOTP ")) + 7];
                                var MSSStart_Payloads_POA = innerTextResult[indexOfMSSStatusAtStartofShift + 41];
                                var MSSStart_SSRMSConfiguration = innerTextResult[indexOfMSSStatusAtStartofShift + 43];
                                var MSSStart_SPDMConfiguration = innerTextResult[indexOfMSSStatusAtStartofShift + 45];
                                var MSSStart_Joints_SSRMS_SR = innerTextResult[indexOfMSSStatusAtStartofShift + 56];
                                var MSSStart_Joints_SSRMS_SY = innerTextResult[indexOfMSSStatusAtStartofShift + 57];
                                var MSSStart_Joints_SSRMS_SP = innerTextResult[indexOfMSSStatusAtStartofShift + 58];
                                var MSSStart_Joints_SSRMS_EP = innerTextResult[indexOfMSSStatusAtStartofShift + 59];
                                var MSSStart_Joints_SSRMS_WP = innerTextResult[indexOfMSSStatusAtStartofShift + 60];
                                var MSSStart_Joints_SSRMS_WY = innerTextResult[indexOfMSSStatusAtStartofShift + 61];
                                var MSSStart_Joints_SSRMS_WR = innerTextResult[indexOfMSSStatusAtStartofShift + 62];
                                var MSSStart_Joints_SSRMS_SPDMBR = innerTextResult[indexOfMSSStatusAtStartofShift + 63];
                                var MSSStart_Joints_ARM1_SR = innerTextResult[indexOfMSSStatusAtStartofShift + 65];
                                var MSSStart_Joints_ARM1_SY = innerTextResult[indexOfMSSStatusAtStartofShift + 66];
                                var MSSStart_Joints_ARM1_SP = innerTextResult[indexOfMSSStatusAtStartofShift + 67];
                                var MSSStart_Joints_ARM1_EP = innerTextResult[indexOfMSSStatusAtStartofShift + 68];
                                var MSSStart_Joints_ARM1_WP = innerTextResult[indexOfMSSStatusAtStartofShift + 69];
                                var MSSStart_Joints_ARM1_WY = innerTextResult[indexOfMSSStatusAtStartofShift + 70];
                                var MSSStart_Joints_ARM1_WR = innerTextResult[indexOfMSSStatusAtStartofShift + 71];
                                var MSSStart_Joints_ARM2_SR = innerTextResult[indexOfMSSStatusAtStartofShift + 74];
                                var MSSStart_Joints_ARM2_SY = innerTextResult[indexOfMSSStatusAtStartofShift + 75];
                                var MSSStart_Joints_ARM2_SP = innerTextResult[indexOfMSSStatusAtStartofShift + 76];
                                var MSSStart_Joints_ARM2_EP = innerTextResult[indexOfMSSStatusAtStartofShift + 77];
                                var MSSStart_Joints_ARM2_WP = innerTextResult[indexOfMSSStatusAtStartofShift + 78];
                                var MSSStart_Joints_ARM2_WY = innerTextResult[indexOfMSSStatusAtStartofShift + 79];
                                var MSSStart_Joints_ARM2_WR = innerTextResult[indexOfMSSStatusAtStartofShift + 80];
                                var MSSStart_Joints_EOTP = innerTextResult[indexOfMSSStatusAtStartofShift + 81];

                                consoleLog.MSSStart_RWS_Lab = MSSStart_RWS_Lab;
                                consoleLog.MSSStart_RWS_Cup = MSSStart_RWS_Cup;
                                consoleLog.MSSStart_MT_WS = Convert.ToInt32(MSSStart_MT_WS);
                                consoleLog.MSSStart_SSRMS_BasedOn_LEE = MSSStart_SSRMS_BasedOn_LEE;
                                consoleLog.MSSStart_SPDM_BasedOn = MSSStart_SPDM_BasedOn;
                                consoleLog.MSSStart_FSTStates_MBS = MSSStart_FSTStates_MBS;
                                consoleLog.MSSStart_FSTStates_POA = MSSStart_FSTStates_POA;
                                consoleLog.MSSStart_FSTStates_SSRMS = MSSStart_FSTStates_SSRMS;
                                consoleLog.MSSStart_FSTStates_SPDMPSU = MSSStart_FSTStates_SPDMPSU;
                                consoleLog.MSSStart_FSTStates_SPDMBody = MSSStart_FSTStates_SPDMBody;
                                consoleLog.MSSStart_FSTStates_SPDMSACU = MSSStart_FSTStates_SPDMSACU;
                                consoleLog.MSSStart_FSTStates_SPDMArms = MSSStart_FSTStates_SPDMArms;
                                consoleLog.MSSStart_Payloads_SSRMSTipLee = MSSStart_Payloads_SSRMSTipLee;
                                consoleLog.MSSStart_Payloads_SPDMLee = MSSStart_Payloads_SPDMLee;
                                consoleLog.MSSStart_Payloads_OTCM1 = MSSStart_Payloads_OTCM1;
                                consoleLog.MSSStart_Payloads_OTCM2 = MSSStart_Payloads_OTCM2;
                                consoleLog.MSSStart_Payloads_MCAS = MSSStart_Payloads_MCAS;
                                consoleLog.MSSStart_Payloads_EOTP = MSSStart_Payloads_EOTP;
                                consoleLog.MSSStart_Payloads_POA = MSSStart_Payloads_POA;
                                consoleLog.MSSStart_SSRMSConfiguration = MSSStart_SSRMSConfiguration;
                                consoleLog.MSSStart_SPDMConfiguration = MSSStart_SPDMConfiguration;
                                consoleLog.MSSStart_Joints_SSRMS_SR = Decimal.Parse(MSSStart_Joints_SSRMS_SR);
                                consoleLog.MSSStart_Joints_SSRMS_SY = Decimal.Parse(MSSStart_Joints_SSRMS_SY);
                                consoleLog.MSSStart_Joints_SSRMS_SP = Decimal.Parse(MSSStart_Joints_SSRMS_SP);
                                consoleLog.MSSStart_Joints_SSRMS_EP = Decimal.Parse(MSSStart_Joints_SSRMS_EP);
                                consoleLog.MSSStart_Joints_SSRMS_WP = Decimal.Parse(MSSStart_Joints_SSRMS_WP);
                                consoleLog.MSSStart_Joints_SSRMS_WY = Decimal.Parse(MSSStart_Joints_SSRMS_WY);
                                consoleLog.MSSStart_Joints_SSRMS_WR = Decimal.Parse(MSSStart_Joints_SSRMS_WR);
                                consoleLog.MSSStart_Joints_SSRMS_SPDMBR = Decimal.Parse(MSSStart_Joints_SSRMS_SPDMBR);
                                consoleLog.MSSStart_Joints_ARM1_SR = Decimal.Parse(MSSStart_Joints_ARM1_SR);
                                consoleLog.MSSStart_Joints_ARM1_SY = Decimal.Parse(MSSStart_Joints_ARM1_SY);
                                consoleLog.MSSStart_Joints_ARM1_SP = Decimal.Parse(MSSStart_Joints_ARM1_SP);
                                consoleLog.MSSStart_Joints_ARM1_EP = Decimal.Parse(MSSStart_Joints_ARM1_EP);
                                consoleLog.MSSStart_Joints_ARM1_WP = Decimal.Parse(MSSStart_Joints_ARM1_WP);
                                consoleLog.MSSStart_Joints_ARM1_WY = Decimal.Parse(MSSStart_Joints_ARM1_WY);
                                consoleLog.MSSStart_Joints_ARM1_WR = Decimal.Parse(MSSStart_Joints_ARM1_WR);
                                consoleLog.MSSStart_Joints_ARM2_SR = Decimal.Parse(MSSStart_Joints_ARM2_SR);
                                consoleLog.MSSStart_Joints_ARM2_SY = Decimal.Parse(MSSStart_Joints_ARM2_SY);
                                consoleLog.MSSStart_Joints_ARM2_SP = Decimal.Parse(MSSStart_Joints_ARM2_SP);
                                consoleLog.MSSStart_Joints_ARM2_EP = Decimal.Parse(MSSStart_Joints_ARM2_EP);
                                consoleLog.MSSStart_Joints_ARM2_WP = Decimal.Parse(MSSStart_Joints_ARM2_WP);
                                consoleLog.MSSStart_Joints_ARM2_WY = Decimal.Parse(MSSStart_Joints_ARM2_WY);
                                consoleLog.MSSStart_Joints_ARM2_WR = Decimal.Parse(MSSStart_Joints_ARM2_WR);
                                consoleLog.MSSStart_Joints_EOTP = Decimal.Parse(MSSStart_Joints_EOTP);
                            }
                            catch (Exception e)
                            {
                                Log.Fatal($"There is a problem reading file { fileName} in MSS Status at Start of Shift table and moved to notProcessed folder");
                                Log.Fatal(e.Message);
                                MoveFileAndFolder(file, notProcessedPath);
                                continue;
                            }                          
                        }
                        else if (indexOfMSSStatusAtStartofShiftNew < 0 && indexOfMSSStatusAtStartofShiftMid < 0)
                        {
                            if (indexOfIPCanadaOld < 0)
                            {
                                MoveFileAndFolder(file, notProcessedPath);
                                Log.Fatal($"There was a problem reading file {fileName} in MSS Status at Start of Shift and moved to {notProcessedPath.FullName} folder");
                                continue;
                            }
                        }

                        #endregion

                        #region MSS Status at End of Shift

                        int indexOShiftLog = 0;

                        var indexOfShiftLogNew = innerTextResult.FindIndex(g => g.Equals(" Shift Log "));
                        var indexOfShiftLogMid = innerTextResult.FindIndex(g => g.Equals(" Shift Log: "));

                        if (indexOfMSSStatusAtEndofShiftNew > 0 && indexOfShiftLogNew > 0)
                        {
                            try
                            {
                                indexOfMSSStatusAtEndofShift = indexOfMSSStatusAtEndofShiftNew;
                                indexOShiftLog = indexOfShiftLogNew;

                                var MSSStatusatEndofShiftIndexRange = innerTextResult.GetRange(indexOfMSSStatusAtEndofShift, indexOShiftLog - indexOfMSSStatusAtEndofShift + 1);
                                var MSSEnd_RWS_Lab = innerTextResult[indexOfMSSStatusAtEndofShift + 15];
                                var MSSEnd_RWS_Cup = innerTextResult[indexOfMSSStatusAtEndofShift + 16];
                                var MSSEnd_MT_WS = innerTextResult[indexOfMSSStatusAtEndofShift + 17];
                                var MSSEnd_SSRMS_BasedOn_LEE = innerTextResult[indexOfMSSStatusAtEndofShift + 18];
                                var MSSEnd_SPDM_BasedOn = innerTextResult[indexOfMSSStatusAtEndofShift + 19];
                                var MSSEnd_FSTStates_MBS = innerTextResult[indexOfMSSStatusAtEndofShift + 20];
                                var MSSEnd_FSTStates_POA = innerTextResult[indexOfMSSStatusAtEndofShift + 21];
                                var MSSEnd_FSTStates_SSRMS = innerTextResult[indexOfMSSStatusAtEndofShift + 22];
                                var MSSEnd_FSTStates_SPDMPSU = innerTextResult[indexOfMSSStatusAtEndofShift + 23];
                                var MSSEnd_FSTStates_SPDMBody = innerTextResult[indexOfMSSStatusAtEndofShift + 24];
                                var MSSEnd_FSTStates_SPDMSACU = innerTextResult[indexOfMSSStatusAtEndofShift + 25];
                                var MSSEnd_FSTStates_SPDMArms = innerTextResult[indexOfMSSStatusAtEndofShift + 26];
                                var MSSEnd_Payloads_SSRMSTipLee = innerTextResult[indexOfMSSStatusAtEndofShift + 35];
                                var MSSEnd_Payloads_SPDMLee = innerTextResult[indexOfMSSStatusAtEndofShift + 36];
                                var MSSEnd_Payloads_OTCM1 = innerTextResult[indexOfMSSStatusAtEndofShift + 37];
                                var MSSEnd_Payloads_OTCM2 = innerTextResult[indexOfMSSStatusAtEndofShift + 38];
                                //var MSSStart_Payloads_MCASData = innerTextResult[indexOfMSSStatusAtEndofShift + 39];
                                var MSSEnd_Payloads_MCAS = MSSStatusatEndofShiftIndexRange[MSSStatusatEndofShiftIndexRange.FindIndex(g => g.Equals(" MCAS ")) + 7];
                                //var MSSStart_Payloads_EOTPData = innerTextResult[indexOfMSSStatusAtEndofShift + 40];
                                var MSSEnd_Payloads_EOTP = MSSStatusatEndofShiftIndexRange[MSSStatusatEndofShiftIndexRange.FindIndex(g => g.Equals(" EOTP ")) + 7];
                                var MSSEnd_Payloads_POA = innerTextResult[indexOfMSSStatusAtEndofShift + 41];
                                var MSSEnd_SSRMSConfiguration = innerTextResult[indexOfMSSStatusAtEndofShift + 43];
                                var MSSEnd_SPDMConfiguration = innerTextResult[indexOfMSSStatusAtEndofShift + 45];
                                var MSSEnd_Joints_SSRMS_SR = innerTextResult[indexOfMSSStatusAtEndofShift + 56];
                                var MSSEnd_Joints_SSRMS_SY = innerTextResult[indexOfMSSStatusAtEndofShift + 57];
                                var MSSEnd_Joints_SSRMS_SP = innerTextResult[indexOfMSSStatusAtEndofShift + 58];
                                var MSSEnd_Joints_SSRMS_EP = innerTextResult[indexOfMSSStatusAtEndofShift + 59];
                                var MSSEnd_Joints_SSRMS_WP = innerTextResult[indexOfMSSStatusAtEndofShift + 60];
                                var MSSEnd_Joints_SSRMS_WY = innerTextResult[indexOfMSSStatusAtEndofShift + 61];
                                var MSSEnd_Joints_SSRMS_WR = innerTextResult[indexOfMSSStatusAtEndofShift + 62];
                                var MSSEnd_Joints_SSRMS_SPDMBR = innerTextResult[indexOfMSSStatusAtEndofShift + 63];
                                var MSSEnd_Joints_ARM1_SR = innerTextResult[indexOfMSSStatusAtEndofShift + 65];
                                var MSSEnd_Joints_ARM1_SY = innerTextResult[indexOfMSSStatusAtEndofShift + 66];
                                var MSSEnd_Joints_ARM1_SP = innerTextResult[indexOfMSSStatusAtEndofShift + 67];
                                var MSSEnd_Joints_ARM1_EP = innerTextResult[indexOfMSSStatusAtEndofShift + 68];
                                var MSSEnd_Joints_ARM1_WP = innerTextResult[indexOfMSSStatusAtEndofShift + 69];
                                var MSSEnd_Joints_ARM1_WY = innerTextResult[indexOfMSSStatusAtEndofShift + 70];
                                var MSSEnd_Joints_ARM1_WR = innerTextResult[indexOfMSSStatusAtEndofShift + 71];
                                var MSSEnd_Joints_ARM2_SR = innerTextResult[indexOfMSSStatusAtEndofShift + 74];
                                var MSSEnd_Joints_ARM2_SY = innerTextResult[indexOfMSSStatusAtEndofShift + 75];
                                var MSSEnd_Joints_ARM2_SP = innerTextResult[indexOfMSSStatusAtEndofShift + 76];
                                var MSSEnd_Joints_ARM2_EP = innerTextResult[indexOfMSSStatusAtEndofShift + 77];
                                var MSSEnd_Joints_ARM2_WP = innerTextResult[indexOfMSSStatusAtEndofShift + 78];
                                var MSSEnd_Joints_ARM2_WY = innerTextResult[indexOfMSSStatusAtEndofShift + 79];
                                var MSSEnd_Joints_ARM2_WR = innerTextResult[indexOfMSSStatusAtEndofShift + 80];
                                var MSSEnd_Joints_EOTP = innerTextResult[indexOfMSSStatusAtEndofShift + 81];
                                consoleLog.MSSEnd_RWS_Lab = MSSEnd_RWS_Lab;
                                consoleLog.MSSEnd_RWS_Cup = MSSEnd_RWS_Cup;
                                consoleLog.MSSEnd_MT_WS = Convert.ToInt32(MSSEnd_MT_WS);
                                consoleLog.MSSEnd_SSRMS_BasedOn_LEE = MSSEnd_SSRMS_BasedOn_LEE;
                                consoleLog.MSSEnd_SPDM_BasedOn = MSSEnd_SPDM_BasedOn;
                                consoleLog.MSSEnd_FSTStates_MBS = MSSEnd_FSTStates_MBS;
                                consoleLog.MSSEnd_FSTStates_POA = MSSEnd_FSTStates_POA;
                                consoleLog.MSSEnd_FSTStates_SSRMS = MSSEnd_FSTStates_SSRMS;
                                consoleLog.MSSEnd_FSTStates_SPDMPSU = MSSEnd_FSTStates_SPDMPSU;
                                consoleLog.MSSEnd_FSTStates_SPDMBody = MSSEnd_FSTStates_SPDMBody;
                                consoleLog.MSSEnd_FSTStates_SPDMSACU = MSSEnd_FSTStates_SPDMSACU;
                                consoleLog.MSSEnd_FSTStates_SPDMArms = MSSEnd_FSTStates_SPDMArms;
                                consoleLog.MSSEnd_Payloads_SSRMSTipLee = MSSEnd_Payloads_SSRMSTipLee;
                                consoleLog.MSSEnd_Payloads_SPDMLee = MSSEnd_Payloads_SPDMLee;
                                consoleLog.MSSEnd_Payloads_OTCM1 = MSSEnd_Payloads_OTCM1;
                                consoleLog.MSSEnd_Payloads_OTCM2 = MSSEnd_Payloads_OTCM2;
                                consoleLog.MSSEnd_Payloads_MCAS = MSSEnd_Payloads_MCAS;
                                consoleLog.MSSEnd_Payloads_EOTP = MSSEnd_Payloads_EOTP;
                                consoleLog.MSSEnd_Payloads_POA = MSSEnd_Payloads_POA;
                                consoleLog.MSSEnd_SSRMSConfiguration = MSSEnd_SSRMSConfiguration;
                                consoleLog.MSSEnd_SPDMConfiguration = MSSEnd_SPDMConfiguration;
                                consoleLog.MSSEnd_Joints_SSRMS_SR = Decimal.Parse(MSSEnd_Joints_SSRMS_SR);
                                consoleLog.MSSEnd_Joints_SSRMS_SY = Decimal.Parse(MSSEnd_Joints_SSRMS_SY);
                                consoleLog.MSSEnd_Joints_SSRMS_SP = Decimal.Parse(MSSEnd_Joints_SSRMS_SP);
                                consoleLog.MSSEnd_Joints_SSRMS_EP = Decimal.Parse(MSSEnd_Joints_SSRMS_EP);
                                consoleLog.MSSEnd_Joints_SSRMS_WP = Decimal.Parse(MSSEnd_Joints_SSRMS_WP);
                                consoleLog.MSSEnd_Joints_SSRMS_WY = Decimal.Parse(MSSEnd_Joints_SSRMS_WY);
                                consoleLog.MSSEnd_Joints_SSRMS_WR = Decimal.Parse(MSSEnd_Joints_SSRMS_WR);
                                consoleLog.MSSEnd_Joints_SSRMS_SPDMBR = Decimal.Parse(MSSEnd_Joints_SSRMS_SPDMBR);
                                consoleLog.MSSEnd_Joints_ARM1_SR = Decimal.Parse(MSSEnd_Joints_ARM1_SR);
                                consoleLog.MSSEnd_Joints_ARM1_SY = Decimal.Parse(MSSEnd_Joints_ARM1_SY);
                                consoleLog.MSSEnd_Joints_ARM1_SP = Decimal.Parse(MSSEnd_Joints_ARM1_SP);
                                consoleLog.MSSEnd_Joints_ARM1_EP = Decimal.Parse(MSSEnd_Joints_ARM1_EP);
                                consoleLog.MSSEnd_Joints_ARM1_WP = Decimal.Parse(MSSEnd_Joints_ARM1_WP);
                                consoleLog.MSSEnd_Joints_ARM1_WY = Decimal.Parse(MSSEnd_Joints_ARM1_WY);
                                consoleLog.MSSEnd_Joints_ARM1_WR = Decimal.Parse(MSSEnd_Joints_ARM1_WR);
                                consoleLog.MSSEnd_Joints_ARM2_SR = Decimal.Parse(MSSEnd_Joints_ARM2_SR);
                                consoleLog.MSSEnd_Joints_ARM2_SY = Decimal.Parse(MSSEnd_Joints_ARM2_SY);
                                consoleLog.MSSEnd_Joints_ARM2_SP = Decimal.Parse(MSSEnd_Joints_ARM2_SP);
                                consoleLog.MSSEnd_Joints_ARM2_EP = Decimal.Parse(MSSEnd_Joints_ARM2_EP);
                                consoleLog.MSSEnd_Joints_ARM2_WP = Decimal.Parse(MSSEnd_Joints_ARM2_WP);
                                consoleLog.MSSEnd_Joints_ARM2_WY = Decimal.Parse(MSSEnd_Joints_ARM2_WY);
                                consoleLog.MSSEnd_Joints_ARM2_WR = Decimal.Parse(MSSEnd_Joints_ARM2_WR);
                                consoleLog.MSSEnd_Joints_EOTP = Decimal.Parse(MSSEnd_Joints_EOTP);
                            }
                            catch (Exception e)
                            {
                                Log.Fatal($"There is a problem reading file { fileName} in MSS Status at End of Shift table and moved to notProcessed folder");
                                Log.Fatal(e.Message);
                                MoveFileAndFolder(file, notProcessedPath);
                                continue;
                            }
                            
                        }
                        else if (indexOfMSSStatusAtEndofShiftMid > 0 && indexOfShiftLogMid > 0)
                        {
                            try
                            {
                                indexOfMSSStatusAtEndofShift = indexOfMSSStatusAtEndofShiftMid;
                                indexOShiftLog = indexOfShiftLogMid;

                                var MSSStatusatEndofShiftIndexRange = innerTextResult.GetRange(indexOfMSSStatusAtEndofShift, indexOShiftLog - indexOfMSSStatusAtEndofShift + 1);
                                var MSSEnd_RWS_Lab = innerTextResult[indexOfMSSStatusAtEndofShift + 15];
                                var MSSEnd_RWS_Cup = innerTextResult[indexOfMSSStatusAtEndofShift + 16];
                                var MSSEnd_MT_WS = innerTextResult[indexOfMSSStatusAtEndofShift + 17];
                                var MSSEnd_SSRMS_BasedOn_LEE = innerTextResult[indexOfMSSStatusAtEndofShift + 18];
                                var MSSEnd_SPDM_BasedOn = innerTextResult[indexOfMSSStatusAtEndofShift + 19];
                                var MSSEnd_FSTStates_MBS = innerTextResult[indexOfMSSStatusAtEndofShift + 20];
                                var MSSEnd_FSTStates_POA = innerTextResult[indexOfMSSStatusAtEndofShift + 21];
                                var MSSEnd_FSTStates_SSRMS = innerTextResult[indexOfMSSStatusAtEndofShift + 22];
                                var MSSEnd_FSTStates_SPDMPSU = innerTextResult[indexOfMSSStatusAtEndofShift + 23];
                                var MSSEnd_FSTStates_SPDMBody = innerTextResult[indexOfMSSStatusAtEndofShift + 24];
                                var MSSEnd_FSTStates_SPDMSACU = innerTextResult[indexOfMSSStatusAtEndofShift + 25];
                                var MSSEnd_FSTStates_SPDMArms = innerTextResult[indexOfMSSStatusAtEndofShift + 26];
                                var MSSEnd_Payloads_SSRMSTipLee = innerTextResult[indexOfMSSStatusAtEndofShift + 35];
                                var MSSEnd_Payloads_SPDMLee = innerTextResult[indexOfMSSStatusAtEndofShift + 36];
                                var MSSEnd_Payloads_OTCM1 = innerTextResult[indexOfMSSStatusAtEndofShift + 37];
                                var MSSEnd_Payloads_OTCM2 = innerTextResult[indexOfMSSStatusAtEndofShift + 38];
                                var MSSEnd_Payloads_MCAS = MSSStatusatEndofShiftIndexRange[MSSStatusatEndofShiftIndexRange.FindIndex(g => g.Equals(" MCAS ")) + 7];
                                var MSSEnd_Payloads_EOTP = MSSStatusatEndofShiftIndexRange[MSSStatusatEndofShiftIndexRange.FindIndex(g => g.Equals(" EOTP ")) + 7];
                                var MSSEnd_Payloads_POA = innerTextResult[indexOfMSSStatusAtEndofShift + 41];
                                var MSSEnd_SSRMSConfiguration = innerTextResult[indexOfMSSStatusAtEndofShift + 43];
                                var MSSEnd_SPDMConfiguration = innerTextResult[indexOfMSSStatusAtEndofShift + 45];
                                var MSSEnd_Joints_SSRMS_SR = innerTextResult[indexOfMSSStatusAtEndofShift + 56];
                                var MSSEnd_Joints_SSRMS_SY = innerTextResult[indexOfMSSStatusAtEndofShift + 57];
                                var MSSEnd_Joints_SSRMS_SP = innerTextResult[indexOfMSSStatusAtEndofShift + 58];
                                var MSSEnd_Joints_SSRMS_EP = innerTextResult[indexOfMSSStatusAtEndofShift + 59];
                                var MSSEnd_Joints_SSRMS_WP = innerTextResult[indexOfMSSStatusAtEndofShift + 60];
                                var MSSEnd_Joints_SSRMS_WY = innerTextResult[indexOfMSSStatusAtEndofShift + 61];
                                var MSSEnd_Joints_SSRMS_WR = innerTextResult[indexOfMSSStatusAtEndofShift + 62];
                                var MSSEnd_Joints_SSRMS_SPDMBR = innerTextResult[indexOfMSSStatusAtEndofShift + 63];
                                var MSSEnd_Joints_ARM1_SR = innerTextResult[indexOfMSSStatusAtEndofShift + 65];
                                var MSSEnd_Joints_ARM1_SY = innerTextResult[indexOfMSSStatusAtEndofShift + 66];
                                var MSSEnd_Joints_ARM1_SP = innerTextResult[indexOfMSSStatusAtEndofShift + 67];
                                var MSSEnd_Joints_ARM1_EP = innerTextResult[indexOfMSSStatusAtEndofShift + 68];
                                var MSSEnd_Joints_ARM1_WP = innerTextResult[indexOfMSSStatusAtEndofShift + 69];
                                var MSSEnd_Joints_ARM1_WY = innerTextResult[indexOfMSSStatusAtEndofShift + 70];
                                var MSSEnd_Joints_ARM1_WR = innerTextResult[indexOfMSSStatusAtEndofShift + 71];
                                var MSSEnd_Joints_ARM2_SR = innerTextResult[indexOfMSSStatusAtEndofShift + 74];
                                var MSSEnd_Joints_ARM2_SY = innerTextResult[indexOfMSSStatusAtEndofShift + 75];
                                var MSSEnd_Joints_ARM2_SP = innerTextResult[indexOfMSSStatusAtEndofShift + 76];
                                var MSSEnd_Joints_ARM2_EP = innerTextResult[indexOfMSSStatusAtEndofShift + 77];
                                var MSSEnd_Joints_ARM2_WP = innerTextResult[indexOfMSSStatusAtEndofShift + 78];
                                var MSSEnd_Joints_ARM2_WY = innerTextResult[indexOfMSSStatusAtEndofShift + 79];
                                var MSSEnd_Joints_ARM2_WR = innerTextResult[indexOfMSSStatusAtEndofShift + 80];
                                var MSSEnd_Joints_EOTP = innerTextResult[indexOfMSSStatusAtEndofShift + 81];
                                consoleLog.MSSEnd_RWS_Lab = MSSEnd_RWS_Lab;
                                consoleLog.MSSEnd_RWS_Cup = MSSEnd_RWS_Cup;
                                consoleLog.MSSEnd_MT_WS = Convert.ToInt32(MSSEnd_MT_WS);
                                consoleLog.MSSEnd_SSRMS_BasedOn_LEE = MSSEnd_SSRMS_BasedOn_LEE;
                                consoleLog.MSSEnd_SPDM_BasedOn = MSSEnd_SPDM_BasedOn;
                                consoleLog.MSSEnd_FSTStates_MBS = MSSEnd_FSTStates_MBS;
                                consoleLog.MSSEnd_FSTStates_POA = MSSEnd_FSTStates_POA;
                                consoleLog.MSSEnd_FSTStates_SSRMS = MSSEnd_FSTStates_SSRMS;
                                consoleLog.MSSEnd_FSTStates_SPDMPSU = MSSEnd_FSTStates_SPDMPSU;
                                consoleLog.MSSEnd_FSTStates_SPDMBody = MSSEnd_FSTStates_SPDMBody;
                                consoleLog.MSSEnd_FSTStates_SPDMSACU = MSSEnd_FSTStates_SPDMSACU;
                                consoleLog.MSSEnd_FSTStates_SPDMArms = MSSEnd_FSTStates_SPDMArms;
                                consoleLog.MSSEnd_Payloads_SSRMSTipLee = MSSEnd_Payloads_SSRMSTipLee;
                                consoleLog.MSSEnd_Payloads_SPDMLee = MSSEnd_Payloads_SPDMLee;
                                consoleLog.MSSEnd_Payloads_OTCM1 = MSSEnd_Payloads_OTCM1;
                                consoleLog.MSSEnd_Payloads_OTCM2 = MSSEnd_Payloads_OTCM2;
                                consoleLog.MSSEnd_Payloads_MCAS = MSSEnd_Payloads_MCAS;
                                consoleLog.MSSEnd_Payloads_EOTP = MSSEnd_Payloads_EOTP;
                                consoleLog.MSSEnd_Payloads_POA = MSSEnd_Payloads_POA;
                                consoleLog.MSSEnd_SSRMSConfiguration = MSSEnd_SSRMSConfiguration;
                                consoleLog.MSSEnd_SPDMConfiguration = MSSEnd_SPDMConfiguration;
                                consoleLog.MSSEnd_Joints_SSRMS_SR = Decimal.Parse(MSSEnd_Joints_SSRMS_SR);
                                consoleLog.MSSEnd_Joints_SSRMS_SY = Decimal.Parse(MSSEnd_Joints_SSRMS_SY);
                                consoleLog.MSSEnd_Joints_SSRMS_SP = Decimal.Parse(MSSEnd_Joints_SSRMS_SP);
                                consoleLog.MSSEnd_Joints_SSRMS_EP = Decimal.Parse(MSSEnd_Joints_SSRMS_EP);
                                consoleLog.MSSEnd_Joints_SSRMS_WP = Decimal.Parse(MSSEnd_Joints_SSRMS_WP);
                                consoleLog.MSSEnd_Joints_SSRMS_WY = Decimal.Parse(MSSEnd_Joints_SSRMS_WY);
                                consoleLog.MSSEnd_Joints_SSRMS_WR = Decimal.Parse(MSSEnd_Joints_SSRMS_WR);
                                consoleLog.MSSEnd_Joints_SSRMS_SPDMBR = Decimal.Parse(MSSEnd_Joints_SSRMS_SPDMBR);
                                consoleLog.MSSEnd_Joints_ARM1_SR = Decimal.Parse(MSSEnd_Joints_ARM1_SR);
                                consoleLog.MSSEnd_Joints_ARM1_SY = Decimal.Parse(MSSEnd_Joints_ARM1_SY);
                                consoleLog.MSSEnd_Joints_ARM1_SP = Decimal.Parse(MSSEnd_Joints_ARM1_SP);
                                consoleLog.MSSEnd_Joints_ARM1_EP = Decimal.Parse(MSSEnd_Joints_ARM1_EP);
                                consoleLog.MSSEnd_Joints_ARM1_WP = Decimal.Parse(MSSEnd_Joints_ARM1_WP);
                                consoleLog.MSSEnd_Joints_ARM1_WY = Decimal.Parse(MSSEnd_Joints_ARM1_WY);
                                consoleLog.MSSEnd_Joints_ARM1_WR = Decimal.Parse(MSSEnd_Joints_ARM1_WR);
                                consoleLog.MSSEnd_Joints_ARM2_SR = Decimal.Parse(MSSEnd_Joints_ARM2_SR);
                                consoleLog.MSSEnd_Joints_ARM2_SY = Decimal.Parse(MSSEnd_Joints_ARM2_SY);
                                consoleLog.MSSEnd_Joints_ARM2_SP = Decimal.Parse(MSSEnd_Joints_ARM2_SP);
                                consoleLog.MSSEnd_Joints_ARM2_EP = Decimal.Parse(MSSEnd_Joints_ARM2_EP);
                                consoleLog.MSSEnd_Joints_ARM2_WP = Decimal.Parse(MSSEnd_Joints_ARM2_WP);
                                consoleLog.MSSEnd_Joints_ARM2_WY = Decimal.Parse(MSSEnd_Joints_ARM2_WY);
                                consoleLog.MSSEnd_Joints_ARM2_WR = Decimal.Parse(MSSEnd_Joints_ARM2_WR);
                                consoleLog.MSSEnd_Joints_EOTP = Decimal.Parse(MSSEnd_Joints_EOTP);
                            }
                            catch (Exception e)
                            {
                                Log.Fatal($"There is a problem reading file { fileName} in MSS Status at End of Shift table and moved to notProcessed folder");
                                Log.Fatal(e.Message);
                                MoveFileAndFolder(file, notProcessedPath);
                                continue;
                            }
                           
                        }
                        else if (indexOfShiftLogNew < 0 && indexOfShiftLogMid < 0)
                        {
                            if (indexOfIPCanadaOld < 0)
                            {
                                MoveFileAndFolder(file, notProcessedPath);
                                Log.Fatal($"There was a problem reading file {fileName} in MSS Status at End of Shift table and moved to {notProcessedPath.FullName} folder");
                                continue;
                            }
                        }
                        #endregion

                        #region Saving as PDF

                        Document wordDoc = new Document();

                        try
                        {
                            word.Visible = false;
                            Object filepath = file;
                            Object readOnly = false;
                            Object saveto = $@"{archivePath.FullName}\{fileNameWithoutExtension}.pdf";

                            wordDoc = word.Documents.Open(ref filepath, ref readOnly);
                            object fileFormat = WdSaveFormat.wdFormatPDF;
                            wordDoc.SaveAs(ref saveto, ref fileFormat);

                            Log.Info($"A .pdf has been genetared in {archivePath.FullName} for the file {fileName}");
                            wordDoc.Close();
                        }
                        catch (Exception e)
                        {
                            Log.Fatal("There was a problem creatig .pdf file, please see below exception for details");
                            Log.Fatal(e.Message);
                            wordDoc.Close();
                            continue;
                        }
                        #endregion

                        #region Moving file to Archive folder
                        try
                        {
                            MoveFileAndFolder(file, archivePath);
                            Log.Info($"File {fileName} reading compleated and  moved to {archivePath.FullName} folder");
                        }
                        catch (Exception e)
                        {
                            Log.Fatal(e.Message);
                            continue;
                        }

                        #endregion

                        try
                        {
                            massDataContext.ConsoleLogs.Add(consoleLog);
                            massDataContext.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Log.Fatal($@"There is a error saving file{fileName} and has moved to notprocessed folder");
                            MoveFileAndFolder(file, notProcessedPath);
                            Log.Fatal(e.Message);
                            continue;
                        }

                    }
                }

                catch (Exception ex)
                {
                    Log.Fatal(ex.Message);
                }
            }
            word.Quit();
        }

        public static void MoveFileAndFolder(string source, DirectoryInfo destination)
        {
            var fileName = Path.GetFileName(source);
            var fileDestination = $@"{destination.FullName}\{fileName}";
            if (File.Exists(fileDestination))
            {
                File.Delete(fileDestination);
            }
            File.Move(source, fileDestination);

            var folderToMove = Path.GetFileNameWithoutExtension(source) + "_files";
            var folderDestination = $@"{destination.FullName}\{folderToMove}";
            if (Directory.Exists($@"{Path.GetDirectoryName(source)}\{folderToMove}"))
            {
                if (Directory.Exists(folderDestination))
                {
                    Directory.Delete(folderDestination, true);
                }
                Directory.Move($@"{Path.GetDirectoryName(source)}\{folderToMove}", folderDestination);
            }
        }

    }
}






























//static void Main(string[] args)
//{
//    ParseHtml(@"C:\Users\Shaiful\Desktop\Console Logs");
//}
//public static void ParseHtml(string folderPath)
//{
//    MassDataContext massDataContext = new MassDataContext();
//    ConsoleLog consoleLog = new ConsoleLog();

//    try
//    {
//        var sourcePath = folderPath;
//        var files = Directory.GetFiles(sourcePath, "*.*", SearchOption.TopDirectoryOnly)
//            .Where(file => new string[] { ".htm", ".html" }
//            .Contains(Path.GetExtension(file)))
//            .ToList();
//        //Application word = new Application();
//        var archivePath = Directory.CreateDirectory($@"{folderPath}\Archive");
//        var alreadyExistsPath = Directory.CreateDirectory($@"{folderPath}\AlreadyExists");
//        var notProcessedPath = Directory.CreateDirectory($@"{folderPath}\NotProcessed");

//        foreach (var file in files)
//        {
//            var unsupportedCharacters = new HashSet<char>("\r\n");
//            var text = File.ReadAllText(file, Encoding.UTF8);

//            String source = WebUtility.HtmlDecode(text);
//            HtmlDocument htmlDocument = new HtmlDocument();
//            htmlDocument.LoadHtml(source);
//            var title = htmlDocument.DocumentNode.SelectNodes("//title").ToList()[0].InnerHtml;
//            var fileName = title;
//            var Extension = Path.GetExtension(file);

//            var isExists = massDataContext.ConsoleLogs.Where(a => a.Title == title);
//            if (isExists.Count() == 0)
//            {
//                var query = from table in htmlDocument.DocumentNode.SelectNodes("//table")
//                            from row in table.SelectNodes("tr")
//                            from cell in row.SelectNodes("th|td")
//                            select new { Table = table.Id, CellText = cell.InnerText };

//                var result = query.Select(g => string.Join("", g.CellText.Where(c => !unsupportedCharacters.Contains(c)))).ToList();

//                List<string> innerTextResult = new List<string>();
//                foreach (var item in result)
//                {
//                    var wordtWithoutSpace = Regex.Replace(item, " {2,}", " ");
//                    wordtWithoutSpace.Replace("�", "df");
//                    innerTextResult.Add(wordtWithoutSpace);
//                }

//                #region Main

//                int indexOfGMTDay = 0;
//                var indexOfGMTDayNewandMid = innerTextResult.FindIndex(g => g.Equals(" GMT Day: "));
//                var indexOfGMTDayOld = innerTextResult.FindIndex(g => g.Equals(" GMT "));
//                if (indexOfGMTDayNewandMid > 0)
//                {
//                    indexOfGMTDay = indexOfGMTDayNewandMid;
//                    var GMTDayData = innerTextResult[indexOfGMTDay + 1];
//                    consoleLog.GMTDay = GMTDayData;
//                }
//                if (indexOfGMTDayOld > 0)
//                {
//                    indexOfGMTDay = indexOfGMTDayOld;
//                    var GMTDayDataforOld = innerTextResult[indexOfGMTDay + 1];
//                    var GMTDayData = GMTDayDataforOld.Substring(9, 3);
//                    consoleLog.GMTDay = GMTDayData;
//                }

//                int indexOfReportDate = 0;
//                var indexOfReportDateNewAndMid = innerTextResult.FindIndex(g => g.Equals(" Report Date: "));
//                var indexOfReportDateOld = innerTextResult.FindIndex(g => g.Equals(" Date "));

//                if (indexOfReportDateNewAndMid > 0)
//                {
//                    indexOfReportDate = indexOfReportDateNewAndMid;
//                    var reportdatedata = innerTextResult[indexOfReportDate + 1];
//                    DateTime parsedDate = DateTime.Parse(reportdatedata);
//                    consoleLog.ReportDate = parsedDate;
//                }

//                if (indexOfReportDateOld > 0)
//                {
//                    var GMTDayforOld = innerTextResult[indexOfGMTDayOld + 1];
//                    var GMTYearForOld = Convert.ToInt32(GMTDayforOld.Substring(2, 4));
//                    indexOfReportDate = indexOfReportDateOld;
//                    var reportdatedatas = innerTextResult[indexOfReportDate + 1];
//                    DateTime parsedDates = DateTime.Parse(reportdatedatas);
//                    DateTime newDate = new DateTime(GMTYearForOld, parsedDates.Month, parsedDates.Day);
//                    consoleLog.ReportDate = newDate;
//                }

//                int indexOfGMTHours = 0;
//                var indexOfGMTHoursNewAndMid = innerTextResult.FindIndex(g => g.Equals(" GMT Hours: "));

//                var indexOfGMTHoursOld = innerTextResult.FindIndex(g => g.Equals(" Hours "));
//                if (indexOfGMTHoursNewAndMid > 0)
//                {
//                    indexOfGMTHours = indexOfGMTHoursNewAndMid;
//                }

//                if (indexOfGMTHoursOld > 0)
//                {
//                    indexOfGMTHours = indexOfGMTHoursOld;
//                }
//                var GMTHours = innerTextResult[indexOfGMTHours + 1];

//                var indexOfIncrement = 0;
//                var indexOfIncrementNew = innerTextResult.FindIndex(g => g.Equals(" Increment: "));
//                var indexOfIncrementMid = innerTextResult.FindIndex(g => g.Equals(" Flight: "));
//                var indexOfIncrementOld = innerTextResult.FindIndex(g => g.Equals(" Flight "));
//                if (indexOfIncrementNew > 0)
//                {
//                    indexOfIncrement = indexOfIncrementNew;
//                }
//                if (indexOfIncrementMid > 0)
//                {
//                    indexOfIncrement = indexOfIncrementMid;
//                }
//                if (indexOfIncrementOld > 0)
//                {
//                    indexOfIncrement = indexOfIncrementOld;
//                }
//                var Increment = innerTextResult[indexOfIncrement + 1];

//                int indexOfPreparedByESL = 0;
//                var indexOfPreparedByESLNewAndMid = innerTextResult.FindIndex(g => g.Equals(" Prepared by (ESL): "));

//                if (indexOfPreparedByESLNewAndMid > 0)
//                {
//                    indexOfPreparedByESL = indexOfPreparedByESLNewAndMid;
//                    var PreparedByESL = innerTextResult[indexOfPreparedByESL + 1];
//                    consoleLog.PreparedByESL = PreparedByESL;
//                }

//                int indexOfShiftDescription = 0;
//                var indexOfShiftDescriptionNewAndMid = innerTextResult.FindIndex(g => g.Equals(" Shift Description: "));

//                var indexOfShiftDescriptionOld = innerTextResult.FindIndex(g => g.Equals(" Shift Description "));
//                if (indexOfShiftDescriptionNewAndMid > 0)
//                {
//                    indexOfShiftDescription = indexOfShiftDescriptionNewAndMid;
//                }

//                if (indexOfShiftDescriptionOld > 0)
//                {
//                    indexOfShiftDescription = indexOfShiftDescriptionOld;
//                }
//                var ShiftDescription = innerTextResult[indexOfShiftDescription + 1];

//                consoleLog.Title = title;
//                consoleLog.GMTHours = GMTHours;
//                consoleLog.Increment = Increment;
//                consoleLog.ShiftDescription = ShiftDescription;

//                #endregion

//                #region Personnel
//                int indexOfOECLead = 0;
//                var indexOfOECLeadNew = innerTextResult.FindIndex(g => g.Equals(" OEC Lead "));
//                var indexOfOECLeadMidandOld = innerTextResult.FindIndex(g => g.Equals(" OEC Lead: "));
//                if (indexOfOECLeadNew > 0)
//                {
//                    indexOfOECLead = indexOfOECLeadNew;
//                }
//                if (indexOfOECLeadMidandOld > 0)
//                {
//                    indexOfOECLead = indexOfOECLeadMidandOld;
//                }
//                var OEC_Lead = innerTextResult[indexOfOECLead + 1];


//                int indexOfOECESL = 0;
//                var indexOfOECESLNew = innerTextResult.FindIndex(g => g.Equals(" ESL "));
//                var indexOfOECESLMid = innerTextResult.FindIndex(g => g.Equals(" ESL: "));

//                if (indexOfOECESLNew > 0)
//                {
//                    indexOfOECESL = indexOfOECESLNew;
//                }
//                if (indexOfOECESLMid > 0)
//                {
//                    indexOfOECESL = indexOfOECESLMid;
//                }

//                var OEC_ESL = innerTextResult[indexOfOECESL + 1];


//                int indexOfOECESC = 0;
//                var indexOfOECESCNew = innerTextResult.FindIndex(g => g.Equals(" ESC "));
//                var indexOfOECESCMid = innerTextResult.FindIndex(g => g.Equals(" ESC: "));
//                var indexOfOECESCOld = innerTextResult.FindIndex(g => g.Equals(" ESC Tech Lead: "));
//                if (indexOfOECESCNew > 0)
//                {
//                    indexOfOECESC = indexOfOECESCNew;
//                }
//                else if (indexOfOECESCOld > 0 && indexOfOECESCMid > 0)
//                {
//                    indexOfOECESC = indexOfOECESCOld;
//                }
//                else
//                {
//                    indexOfOECESC = indexOfOECESCMid;
//                }
//                var OEC_ESC = innerTextResult[indexOfOECESC + 1];


//                int indexOfFPL = 0;
//                var indexOfFPLNew = innerTextResult.FindIndex(g => g.Equals(" FPL "));
//                var indexOfFPLMidandOld = innerTextResult.FindIndex(g => g.Equals(" FPL: "));

//                if (indexOfFPLNew > 0)
//                {
//                    indexOfFPL = indexOfFPLNew;
//                }
//                if (indexOfFPLMidandOld > 0)
//                {
//                    indexOfFPL = indexOfFPLMidandOld;
//                }
//                var OEC_FPL = innerTextResult[indexOfFPL + 1];


//                int indexOfOEC_OPR = 0;
//                var indexOfOECOPRNew = innerTextResult.FindIndex(g => g.Equals(" OPR "));
//                var indexOfOECOPRMid = innerTextResult.FindIndex(g => g.Equals(" OPR: "));
//                if (indexOfOECOPRNew > 0)
//                {
//                    indexOfOEC_OPR = indexOfOECOPRNew;
//                    var OEC_OPR = innerTextResult[indexOfOEC_OPR + 1];
//                    consoleLog.OEC_OPR = OEC_OPR;
//                }
//                if (indexOfOECOPRMid > 0)
//                {
//                    indexOfOEC_OPR = indexOfOECOPRMid;
//                    var OEC_OPR = innerTextResult[indexOfOEC_OPR + 1];
//                    consoleLog.OEC_OPR = OEC_OPR;
//                }


//                int indexOfROBOLead = 0;
//                var indexOfROBOLeadNew = innerTextResult.FindIndex(g => g.Equals(" ROBO "));
//                var indexOfROBOLeadMidandOld = innerTextResult.FindIndex(g => g.Equals(" ROBO: "));
//                if (indexOfROBOLeadNew > 0)
//                {
//                    indexOfROBOLeadNew = innerTextResult.FindIndex(g => g.Equals(" ROBO ")) + 2;
//                    indexOfROBOLead = indexOfROBOLeadNew;
//                }
//                if (indexOfROBOLeadMidandOld > 0)
//                {
//                    indexOfROBOLeadMidandOld = innerTextResult.FindIndex(g => g.Equals(" ROBO: ")) + 2;
//                    indexOfROBOLead = indexOfROBOLeadMidandOld;
//                }
//                var ROBOLeadData = innerTextResult[indexOfROBOLead];

//                int indexOfROBOSystems = 0;
//                var indexOfROBOSystemsNew = innerTextResult.FindIndex(g => g.Equals(" ROBO "));
//                var indexOfROBOSystemsMidandOld = innerTextResult.FindIndex(g => g.Equals(" ROBO: "));
//                if (indexOfROBOSystemsNew > 0)
//                {
//                    indexOfROBOSystemsNew = innerTextResult.FindIndex(g => g.Equals(" ROBO ")) + 6;
//                    indexOfROBOSystems = indexOfROBOSystemsNew;
//                }
//                if (indexOfROBOSystemsMidandOld > 0)
//                {
//                    indexOfROBOSystemsMidandOld = innerTextResult.FindIndex(g => g.Equals(" ROBO: ")) + 4;
//                    indexOfROBOSystems = indexOfROBOSystemsMidandOld;
//                }
//                var ROBOSystemsData = innerTextResult[indexOfROBOSystems];

//                int indexOfROBOTask = 0;
//                var indexOfROBOTaskNew = innerTextResult.FindIndex(g => g.Equals(" ROBO "));
//                var indexOfROBOTaskMidandOld = innerTextResult.FindIndex(g => g.Equals(" ROBO: "));
//                if (indexOfROBOTaskNew > 0)
//                {
//                    indexOfROBOTaskNew = innerTextResult.FindIndex(g => g.Equals(" ROBO ")) + 10;
//                    indexOfROBOTask = indexOfROBOTaskNew;
//                }
//                if (indexOfROBOTaskMidandOld > 0)
//                {
//                    if (innerTextResult[indexOfROBOTaskMidandOld + 7] == " Task: ")
//                    {
//                        indexOfROBOTask = innerTextResult.FindIndex(g => g.Equals(" ROBO: ")) + 8;
//                    }
//                    else
//                    {
//                        indexOfROBOTask = innerTextResult.FindIndex(g => g.Equals(" ROBO: ")) + 6;
//                    }
//                }
//                var ROBOTaskData = innerTextResult[indexOfROBOTask];

//                int indexOfEVR = 0;
//                var indexOfEVRNew = innerTextResult.FindIndex(g => g.Equals(" EVR "));
//                var indexOfEVRMidnadOld = innerTextResult.FindIndex(g => g.Equals(" EVR: "));
//                if (indexOfEVRNew > 0)
//                {
//                    indexOfEVRNew = innerTextResult.FindIndex(g => g.Equals(" EVR ")) + 1;
//                    indexOfEVR = indexOfEVRNew;
//                }
//                if (indexOfEVRMidnadOld > 0)
//                {
//                    indexOfEVRMidnadOld = innerTextResult.FindIndex(g => g.Equals(" EVR: ")) + 2;
//                    indexOfEVR = indexOfEVRMidnadOld;
//                }
//                var EVRData = innerTextResult[indexOfEVR];

//                int indexOfIPCanada = 0;
//                var indexOfIPCanadaNew = innerTextResult.FindIndex(g => g.Equals(" IP Canada "));
//                var indexOfIPCanadaMid = innerTextResult.FindIndex(g => g.Equals(" IP Canada: "));
//                var indexOfIPCanadaOld = innerTextResult.FindIndex(g => g.Equals(" IP Can: "));
//                if (indexOfIPCanadaNew > 0)
//                {
//                    indexOfIPCanada = indexOfIPCanadaNew;
//                }
//                if (indexOfIPCanadaMid > 0)
//                {
//                    indexOfIPCanada = indexOfIPCanadaMid;
//                }
//                if (indexOfIPCanadaOld > 0)
//                {
//                    indexOfIPCanada = indexOfIPCanadaOld;
//                }
//                var IPCanadaData = innerTextResult[indexOfIPCanada + 1];

//                consoleLog.OEC_OECLead = OEC_Lead;
//                consoleLog.OEC_ESL = OEC_ESL;
//                consoleLog.OEC_ESC = OEC_ESC;
//                consoleLog.OEC_FPL = OEC_FPL;
//                consoleLog.ROBO_Lead = ROBOLeadData;
//                consoleLog.ROBO_Systems = ROBOSystemsData;
//                consoleLog.ROBO_Task = ROBOTaskData;
//                consoleLog.EVR = EVRData;
//                consoleLog.IPCanada = IPCanadaData;

//                #endregion

//                #region Shift Summary
//                int indexOfShiftSummary = 0;
//                var indexOfShiftSummaryNew = innerTextResult.FindIndex(g => g.Equals(" Shift Summary "));
//                var indexOfShiftSummaryMid = innerTextResult.FindIndex(g => g.Equals(" Shift Summary: "));
//                var indexOfShiftSummaryOld = innerTextResult.FindIndex(g => g.Equals(" Shift Summary/Highlights "));
//                if (indexOfShiftSummaryNew > 0)
//                {
//                    indexOfShiftSummary = indexOfShiftSummaryNew;
//                }
//                if (indexOfShiftSummaryMid > 0)
//                {
//                    indexOfShiftSummary = indexOfShiftSummaryMid;
//                }
//                if (indexOfShiftSummaryOld > 0)
//                {
//                    indexOfShiftSummary = indexOfShiftSummaryOld;
//                }
//                var ShiftSummaryData = innerTextResult[indexOfShiftSummary + 1];

//                consoleLog.ShiftSummary = ShiftSummaryData;

//                #endregion

//                #region Forward/Open Work
//                int indexOfForwardOrOpenWork = 0;
//                var indexOfForwardOrOpenWorkNew = innerTextResult.FindIndex(g => g.Equals(" Forward/Open Work "));
//                var indexOfForwardOrOpenWorkMid = innerTextResult.FindIndex(g => g.Equals(" Forward/Open Work: "));
//                var indexOfForwardOrOpenWorkOld = innerTextResult.FindIndex(g => g.Equals(" Forward Work "));
//                if (indexOfForwardOrOpenWorkNew > 0)
//                {
//                    indexOfForwardOrOpenWork = indexOfForwardOrOpenWorkNew;
//                }
//                if (indexOfForwardOrOpenWorkMid > 0)
//                {
//                    indexOfForwardOrOpenWork = indexOfForwardOrOpenWorkMid;
//                }
//                if (indexOfForwardOrOpenWorkOld > 0)
//                {
//                    indexOfForwardOrOpenWork = indexOfForwardOrOpenWorkOld;
//                }
//                var ForwardOrOpenWorkData = innerTextResult[indexOfForwardOrOpenWork + 1];

//                consoleLog.ForwardOrOpenWork = ForwardOrOpenWorkData;
//                #endregion

//                #region MSS Status at Start of Shift 
//                int indexOfMSSStatusAtStartofShift = 0;
//                int indexOfMSSStatusAtEndofShift = 0;
//                var indexOfMSSStatusAtStartofShiftNew = innerTextResult.FindIndex(g => g.Equals(" MSS Status at Start of Shift "));
//                var indexOfMSSStatusAtEndofShiftNew = innerTextResult.FindIndex(g => g.Equals(" MSS Status at End of Shift "));
//                var indexOfMSSStatusAtStartofShiftMid = innerTextResult.FindIndex(g => g.Equals(" MSS Status at Start of Shift: "));
//                var indexOfMSSStatusAtEndofShiftMid = innerTextResult.FindIndex(g => g.Equals(" MSS Status at End of Shift: "));

//                if (indexOfMSSStatusAtStartofShiftNew < 0 && indexOfMSSStatusAtStartofShiftMid < 0)
//                {
//                    if (indexOfIPCanadaOld < 0)
//                    {
//                        var notProcessedFileDestination = $@"{ notProcessedPath.FullName}\{fileName}{Extension}";
//                        var existingFolderToMove = Path.GetFileNameWithoutExtension(file) + "_files";
//                        var notProcessedFolderDestination = $@"{notProcessedPath.FullName}\{existingFolderToMove}";

//                        if (File.Exists(notProcessedFileDestination))
//                        {
//                            File.Delete(notProcessedFileDestination);
//                        }
//                        File.Move(file, notProcessedFileDestination);
//                        if (Directory.Exists($@"{folderPath}\{existingFolderToMove}"))
//                        {
//                            if (Directory.Exists(notProcessedFolderDestination))
//                            {
//                                Directory.Delete(notProcessedFolderDestination, true);
//                            }
//                            Directory.Move($@"{folderPath}\{existingFolderToMove}", notProcessedFolderDestination);
//                        }
//                    }
//                }
//                if (indexOfMSSStatusAtStartofShiftNew > 0 && indexOfMSSStatusAtEndofShiftNew > 0)
//                {
//                    indexOfMSSStatusAtStartofShift = indexOfMSSStatusAtStartofShiftNew;
//                    indexOfMSSStatusAtEndofShift = indexOfMSSStatusAtEndofShiftNew;

//                    var MSSStatusatStartofShiftIndexRange = innerTextResult.GetRange(indexOfMSSStatusAtStartofShift, indexOfMSSStatusAtEndofShift - indexOfMSSStatusAtStartofShift + 1);
//                    var MSSStart_RWS_Lab = innerTextResult[indexOfMSSStatusAtStartofShift + 15];

//                    var MSSStart_RWS_Cup = innerTextResult[indexOfMSSStatusAtStartofShift + 16];
//                    var MSSStart_MT_WS = innerTextResult[indexOfMSSStatusAtStartofShift + 17];
//                    var MSSStart_SSRMS_LEE = innerTextResult[indexOfMSSStatusAtStartofShift + 18];
//                    var MSSStart_SPDM_BasedOn = innerTextResult[indexOfMSSStatusAtStartofShift + 19];
//                    var MSSStart_FSTStates_MBS = innerTextResult[indexOfMSSStatusAtStartofShift + 20];
//                    var MSSStart_FSTStates_POA = innerTextResult[indexOfMSSStatusAtStartofShift + 21];
//                    var MSSStart_FSTStates_SSRMS = innerTextResult[indexOfMSSStatusAtStartofShift + 22];
//                    var MSSStart_FSTStates_SPDMPSU = innerTextResult[indexOfMSSStatusAtStartofShift + 23];
//                    var MSSStart_FSTStates_SPDMBody = innerTextResult[indexOfMSSStatusAtStartofShift + 24];
//                    var MSSStart_FSTStates_SPDMSACU = innerTextResult[indexOfMSSStatusAtStartofShift + 25];
//                    var MSSStart_FSTStates_SPDMArms = innerTextResult[indexOfMSSStatusAtStartofShift + 26];
//                    var MSSStart_Payloads_SSRMSTipLee = innerTextResult[indexOfMSSStatusAtStartofShift + 35];
//                    var MSSStart_Payloads_SPDMLee = innerTextResult[indexOfMSSStatusAtStartofShift + 36];
//                    var MSSStart_Payloads_OTCM1 = innerTextResult[indexOfMSSStatusAtStartofShift + 37];
//                    var MSSStart_Payloads_OTCM2 = innerTextResult[indexOfMSSStatusAtStartofShift + 38];
//                    var MSSStart_Payloads_MCAS = MSSStatusatStartofShiftIndexRange[MSSStatusatStartofShiftIndexRange.FindIndex(g => g.Equals("  MCAS  ")) + 7];
//                    var MSSStart_Payloads_EOTP = MSSStatusatStartofShiftIndexRange[MSSStatusatStartofShiftIndexRange.FindIndex(g => g.Equals("  EOTP  ")) + 7];
//                    var MSSStart_Payloads_POA = innerTextResult[indexOfMSSStatusAtStartofShift + 41];
//                    var MSSStart_SSRMSConfiguration = innerTextResult[indexOfMSSStatusAtStartofShift + 43];
//                    var MSSStart_SPDMConfiguration = innerTextResult[indexOfMSSStatusAtStartofShift + 45];
//                    var MSSStart_Joints_SSRMS_SR = innerTextResult[indexOfMSSStatusAtStartofShift + 56];
//                    var MSSStart_Joints_SSRMS_SY = innerTextResult[indexOfMSSStatusAtStartofShift + 57];
//                    var MSSStart_Joints_SSRMS_SP = innerTextResult[indexOfMSSStatusAtStartofShift + 58];
//                    var MSSStart_Joints_SSRMS_EP = innerTextResult[indexOfMSSStatusAtStartofShift + 59];
//                    var MSSStart_Joints_SSRMS_WP = innerTextResult[indexOfMSSStatusAtStartofShift + 60];
//                    var MSSStart_Joints_SSRMS_WY = innerTextResult[indexOfMSSStatusAtStartofShift + 61];
//                    var MSSStart_Joints_SSRMS_WR = innerTextResult[indexOfMSSStatusAtStartofShift + 62];
//                    var MSSStart_Joints_SSRMS_SPDMBR = innerTextResult[indexOfMSSStatusAtStartofShift + 63];
//                    var MSSStart_Joints_ARM1_SR = innerTextResult[indexOfMSSStatusAtStartofShift + 65];
//                    var MSSStart_Joints_ARM1_SY = innerTextResult[indexOfMSSStatusAtStartofShift + 66];
//                    var MSSStart_Joints_ARM1_SP = innerTextResult[indexOfMSSStatusAtStartofShift + 67];
//                    var MSSStart_Joints_ARM1_EP = innerTextResult[indexOfMSSStatusAtStartofShift + 68];
//                    var MSSStart_Joints_ARM1_WP = innerTextResult[indexOfMSSStatusAtStartofShift + 69];
//                    var MSSStart_Joints_ARM1_WY = innerTextResult[indexOfMSSStatusAtStartofShift + 70];
//                    var MSSStart_Joints_ARM1_WR = innerTextResult[indexOfMSSStatusAtStartofShift + 71];
//                    var MSSStart_Joints_ARM2_SR = innerTextResult[indexOfMSSStatusAtStartofShift + 74];
//                    var MSSStart_Joints_ARM2_SY = innerTextResult[indexOfMSSStatusAtStartofShift + 75];
//                    var MSSStart_Joints_ARM2_SP = innerTextResult[indexOfMSSStatusAtStartofShift + 76];
//                    var MSSStart_Joints_ARM2_EP = innerTextResult[indexOfMSSStatusAtStartofShift + 77];
//                    var MSSStart_Joints_ARM2_WP = innerTextResult[indexOfMSSStatusAtStartofShift + 78];
//                    var MSSStart_Joints_ARM2_WY = innerTextResult[indexOfMSSStatusAtStartofShift + 79];
//                    var MSSStart_Joints_ARM2_WR = innerTextResult[indexOfMSSStatusAtStartofShift + 80];
//                    var MSSStart_Joints_EOTP = innerTextResult[indexOfMSSStatusAtStartofShift + 81];

//                    consoleLog.MSSStart_RWS_Lab = MSSStart_RWS_Lab;
//                    consoleLog.MSSStart_RWS_Cup = MSSStart_RWS_Cup;
//                    consoleLog.MSSStart_MT_WS = MSSStart_MT_WS;
//                    consoleLog.MSSStart_SSRMS_BasedOn_LEE = MSSStart_SSRMS_LEE;
//                    consoleLog.MSSStart_SPDM_BasedOn = MSSStart_SPDM_BasedOn;
//                    consoleLog.MSSStart_FSTStates_MBS = MSSStart_FSTStates_MBS;
//                    consoleLog.MSSStart_FSTStates_POA = MSSStart_FSTStates_POA;
//                    consoleLog.MSSStart_FSTStates_SSRMS = MSSStart_FSTStates_SSRMS;
//                    consoleLog.MSSStart_FSTStates_SPDMPSU = MSSStart_FSTStates_SPDMPSU;
//                    consoleLog.MSSStart_FSTStates_SPDMBody = MSSStart_FSTStates_SPDMBody;
//                    consoleLog.MSSStart_FSTStates_SPDMSACU = MSSStart_FSTStates_SPDMSACU;
//                    consoleLog.MSSStart_FSTStates_SPDMArms = MSSStart_FSTStates_SPDMArms;
//                    consoleLog.MSSStart_Payloads_SSRMSTipLee = MSSStart_Payloads_SSRMSTipLee;
//                    consoleLog.MSSStart_Payloads_SPDMLee = MSSStart_Payloads_SPDMLee;
//                    consoleLog.MSSStart_Payloads_OTCM1 = MSSStart_Payloads_OTCM1;
//                    consoleLog.MSSStart_Payloads_OTCM2 = MSSStart_Payloads_OTCM2;
//                    consoleLog.MSSStart_Payloads_MCAS = MSSStart_Payloads_MCAS;
//                    consoleLog.MSSStart_Payloads_EOTP = MSSStart_Payloads_EOTP;
//                    consoleLog.MSSStart_Payloads_POA = MSSStart_Payloads_POA;
//                    consoleLog.MSSStart_SSRMSConfiguration = MSSStart_SSRMSConfiguration;
//                    consoleLog.MSSStart_SPDMConfiguration = MSSStart_SPDMConfiguration;
//                    consoleLog.MSSStart_Joints_SSRMS_SR = MSSStart_Joints_SSRMS_SR;
//                    consoleLog.MSSStart_Joints_SSRMS_SY = MSSStart_Joints_SSRMS_SY;
//                    consoleLog.MSSStart_Joints_SSRMS_SP = MSSStart_Joints_SSRMS_SP;
//                    consoleLog.MSSStart_Joints_SSRMS_EP = MSSStart_Joints_SSRMS_EP;
//                    consoleLog.MSSStart_Joints_SSRMS_WP = MSSStart_Joints_SSRMS_WP;
//                    consoleLog.MSSStart_Joints_SSRMS_WY = MSSStart_Joints_SSRMS_WY;
//                    consoleLog.MSSStart_Joints_SSRMS_WR = MSSStart_Joints_SSRMS_WR;
//                    consoleLog.MSSStart_Joints_SSRMS_SPDMBR = MSSStart_Joints_SSRMS_SPDMBR;
//                    consoleLog.MSSStart_Joints_ARM1_SR = MSSStart_Joints_ARM1_SR;
//                    consoleLog.MSSStart_Joints_ARM1_SY = MSSStart_Joints_ARM1_SY;
//                    consoleLog.MSSStart_Joints_ARM1_SP = MSSStart_Joints_ARM1_SP;
//                    consoleLog.MSSStart_Joints_ARM1_EP = MSSStart_Joints_ARM1_EP;
//                    consoleLog.MSSStart_Joints_ARM1_WP = MSSStart_Joints_ARM1_WP;
//                    consoleLog.MSSStart_Joints_ARM1_WY = MSSStart_Joints_ARM1_WY;
//                    consoleLog.MSSStart_Joints_ARM1_WR = MSSStart_Joints_ARM1_WR;
//                    consoleLog.MSSStart_Joints_ARM2_SR = MSSStart_Joints_ARM2_SR;
//                    consoleLog.MSSStart_Joints_ARM2_SY = MSSStart_Joints_ARM2_SY;
//                    consoleLog.MSSStart_Joints_ARM2_SP = MSSStart_Joints_ARM2_SP;
//                    consoleLog.MSSStart_Joints_ARM2_EP = MSSStart_Joints_ARM2_EP;
//                    consoleLog.MSSStart_Joints_ARM2_WP = MSSStart_Joints_ARM2_WP;
//                    consoleLog.MSSStart_Joints_ARM2_WY = MSSStart_Joints_ARM2_WY;
//                    consoleLog.MSSStart_Joints_ARM2_WR = MSSStart_Joints_ARM2_WR;
//                    consoleLog.MSSStart_Joints_EOTP = MSSStart_Joints_EOTP;
//                }
//                if (indexOfMSSStatusAtStartofShiftMid > 0 && indexOfMSSStatusAtEndofShiftMid > 0)
//                {
//                    indexOfMSSStatusAtStartofShift = indexOfMSSStatusAtStartofShiftMid;
//                    indexOfMSSStatusAtEndofShift = indexOfMSSStatusAtEndofShiftMid;

//                    var MSSStatusatStartofShiftIndexRange = innerTextResult.GetRange(indexOfMSSStatusAtStartofShift, indexOfMSSStatusAtEndofShift - indexOfMSSStatusAtStartofShift + 1);
//                    var MSSStart_RWS_Lab = innerTextResult[indexOfMSSStatusAtStartofShift + 15];
//                    var MSSStart_RWS_Cup = innerTextResult[indexOfMSSStatusAtStartofShift + 16];
//                    var MSSStart_MT_WS = innerTextResult[indexOfMSSStatusAtStartofShift + 17];
//                    var MSSStart_SSRMS_BasedOn_LEE = innerTextResult[indexOfMSSStatusAtStartofShift + 18];
//                    var MSSStart_SPDM_BasedOn = innerTextResult[indexOfMSSStatusAtStartofShift + 19];
//                    var MSSStart_FSTStates_MBS = innerTextResult[indexOfMSSStatusAtStartofShift + 20];
//                    var MSSStart_FSTStates_POA = innerTextResult[indexOfMSSStatusAtStartofShift + 21];
//                    var MSSStart_FSTStates_SSRMS = innerTextResult[indexOfMSSStatusAtStartofShift + 22];
//                    var MSSStart_FSTStates_SPDMPSU = innerTextResult[indexOfMSSStatusAtStartofShift + 23];
//                    var MSSStart_FSTStates_SPDMBody = innerTextResult[indexOfMSSStatusAtStartofShift + 24];
//                    var MSSStart_FSTStates_SPDMSACU = innerTextResult[indexOfMSSStatusAtStartofShift + 25];
//                    var MSSStart_FSTStates_SPDMArms = innerTextResult[indexOfMSSStatusAtStartofShift + 26];
//                    var MSSStart_Payloads_SSRMSTipLee = innerTextResult[indexOfMSSStatusAtStartofShift + 35];
//                    var MSSStart_Payloads_SPDMLee = innerTextResult[indexOfMSSStatusAtStartofShift + 36];
//                    var MSSStart_Payloads_OTCM1 = innerTextResult[indexOfMSSStatusAtStartofShift + 37];
//                    var MSSStart_Payloads_OTCM2 = innerTextResult[indexOfMSSStatusAtStartofShift + 38];
//                    var MSSStart_Payloads_MCAS = MSSStatusatStartofShiftIndexRange[MSSStatusatStartofShiftIndexRange.FindIndex(g => g.Equals("  MCAS  ")) + 7];
//                    var MSSStart_Payloads_EOTP = MSSStatusatStartofShiftIndexRange[MSSStatusatStartofShiftIndexRange.FindIndex(g => g.Equals("  EOTP  ")) + 7];
//                    var MSSStart_Payloads_POA = innerTextResult[indexOfMSSStatusAtStartofShift + 41];
//                    var MSSStart_SSRMSConfiguration = innerTextResult[indexOfMSSStatusAtStartofShift + 43];
//                    var MSSStart_SPDMConfiguration = innerTextResult[indexOfMSSStatusAtStartofShift + 45];
//                    var MSSStart_Joints_SSRMS_SR = innerTextResult[indexOfMSSStatusAtStartofShift + 56];
//                    var MSSStart_Joints_SSRMS_SY = innerTextResult[indexOfMSSStatusAtStartofShift + 57];
//                    var MSSStart_Joints_SSRMS_SP = innerTextResult[indexOfMSSStatusAtStartofShift + 58];
//                    var MSSStart_Joints_SSRMS_EP = innerTextResult[indexOfMSSStatusAtStartofShift + 59];
//                    var MSSStart_Joints_SSRMS_WP = innerTextResult[indexOfMSSStatusAtStartofShift + 60];
//                    var MSSStart_Joints_SSRMS_WY = innerTextResult[indexOfMSSStatusAtStartofShift + 61];
//                    var MSSStart_Joints_SSRMS_WR = innerTextResult[indexOfMSSStatusAtStartofShift + 62];
//                    var MSSStart_Joints_SSRMS_SPDMBR = innerTextResult[indexOfMSSStatusAtStartofShift + 63];
//                    var MSSStart_Joints_ARM1_SR = innerTextResult[indexOfMSSStatusAtStartofShift + 65];
//                    var MSSStart_Joints_ARM1_SY = innerTextResult[indexOfMSSStatusAtStartofShift + 66];
//                    var MSSStart_Joints_ARM1_SP = innerTextResult[indexOfMSSStatusAtStartofShift + 67];
//                    var MSSStart_Joints_ARM1_EP = innerTextResult[indexOfMSSStatusAtStartofShift + 68];
//                    var MSSStart_Joints_ARM1_WP = innerTextResult[indexOfMSSStatusAtStartofShift + 69];
//                    var MSSStart_Joints_ARM1_WY = innerTextResult[indexOfMSSStatusAtStartofShift + 70];
//                    var MSSStart_Joints_ARM1_WR = innerTextResult[indexOfMSSStatusAtStartofShift + 71];
//                    var MSSStart_Joints_ARM2_SR = innerTextResult[indexOfMSSStatusAtStartofShift + 74];
//                    var MSSStart_Joints_ARM2_SY = innerTextResult[indexOfMSSStatusAtStartofShift + 75];
//                    var MSSStart_Joints_ARM2_SP = innerTextResult[indexOfMSSStatusAtStartofShift + 76];
//                    var MSSStart_Joints_ARM2_EP = innerTextResult[indexOfMSSStatusAtStartofShift + 77];
//                    var MSSStart_Joints_ARM2_WP = innerTextResult[indexOfMSSStatusAtStartofShift + 78];
//                    var MSSStart_Joints_ARM2_WY = innerTextResult[indexOfMSSStatusAtStartofShift + 79];
//                    var MSSStart_Joints_ARM2_WR = innerTextResult[indexOfMSSStatusAtStartofShift + 80];
//                    var MSSStart_Joints_EOTP = innerTextResult[indexOfMSSStatusAtStartofShift + 81];

//                    consoleLog.MSSStart_RWS_Lab = MSSStart_RWS_Lab;
//                    consoleLog.MSSStart_RWS_Cup = MSSStart_RWS_Cup;
//                    consoleLog.MSSStart_MT_WS = MSSStart_MT_WS;
//                    consoleLog.MSSStart_SSRMS_BasedOn_LEE = MSSStart_SSRMS_BasedOn_LEE;
//                    consoleLog.MSSStart_SPDM_BasedOn = MSSStart_SPDM_BasedOn;
//                    consoleLog.MSSStart_FSTStates_MBS = MSSStart_FSTStates_MBS;
//                    consoleLog.MSSStart_FSTStates_POA = MSSStart_FSTStates_POA;
//                    consoleLog.MSSStart_FSTStates_SSRMS = MSSStart_FSTStates_SSRMS;
//                    consoleLog.MSSStart_FSTStates_SPDMPSU = MSSStart_FSTStates_SPDMPSU;
//                    consoleLog.MSSStart_FSTStates_SPDMBody = MSSStart_FSTStates_SPDMBody;
//                    consoleLog.MSSStart_FSTStates_SPDMSACU = MSSStart_FSTStates_SPDMSACU;
//                    consoleLog.MSSStart_FSTStates_SPDMArms = MSSStart_FSTStates_SPDMArms;
//                    consoleLog.MSSStart_Payloads_SSRMSTipLee = MSSStart_Payloads_SSRMSTipLee;
//                    consoleLog.MSSStart_Payloads_SPDMLee = MSSStart_Payloads_SPDMLee;
//                    consoleLog.MSSStart_Payloads_OTCM1 = MSSStart_Payloads_OTCM1;
//                    consoleLog.MSSStart_Payloads_OTCM2 = MSSStart_Payloads_OTCM2;
//                    consoleLog.MSSStart_Payloads_MCAS = MSSStart_Payloads_MCAS;
//                    consoleLog.MSSStart_Payloads_EOTP = MSSStart_Payloads_EOTP;
//                    consoleLog.MSSStart_Payloads_POA = MSSStart_Payloads_POA;
//                    consoleLog.MSSStart_SSRMSConfiguration = MSSStart_SSRMSConfiguration;
//                    consoleLog.MSSStart_SPDMConfiguration = MSSStart_SPDMConfiguration;
//                    consoleLog.MSSStart_Joints_SSRMS_SR = MSSStart_Joints_SSRMS_SR;
//                    consoleLog.MSSStart_Joints_SSRMS_SY = MSSStart_Joints_SSRMS_SY;
//                    consoleLog.MSSStart_Joints_SSRMS_SP = MSSStart_Joints_SSRMS_SP;
//                    consoleLog.MSSStart_Joints_SSRMS_EP = MSSStart_Joints_SSRMS_EP;
//                    consoleLog.MSSStart_Joints_SSRMS_WP = MSSStart_Joints_SSRMS_WP;
//                    consoleLog.MSSStart_Joints_SSRMS_WY = MSSStart_Joints_SSRMS_WY;
//                    consoleLog.MSSStart_Joints_SSRMS_WR = MSSStart_Joints_SSRMS_WR;
//                    consoleLog.MSSStart_Joints_SSRMS_SPDMBR = MSSStart_Joints_SSRMS_SPDMBR;
//                    consoleLog.MSSStart_Joints_ARM1_SR = MSSStart_Joints_ARM1_SR;
//                    consoleLog.MSSStart_Joints_ARM1_SY = MSSStart_Joints_ARM1_SY;
//                    consoleLog.MSSStart_Joints_ARM1_SP = MSSStart_Joints_ARM1_SP;
//                    consoleLog.MSSStart_Joints_ARM1_EP = MSSStart_Joints_ARM1_EP;
//                    consoleLog.MSSStart_Joints_ARM1_WP = MSSStart_Joints_ARM1_WP;
//                    consoleLog.MSSStart_Joints_ARM1_WY = MSSStart_Joints_ARM1_WY;
//                    consoleLog.MSSStart_Joints_ARM1_WR = MSSStart_Joints_ARM1_WR;
//                    consoleLog.MSSStart_Joints_ARM2_SR = MSSStart_Joints_ARM2_SR;
//                    consoleLog.MSSStart_Joints_ARM2_SY = MSSStart_Joints_ARM2_SY;
//                    consoleLog.MSSStart_Joints_ARM2_SP = MSSStart_Joints_ARM2_SP;
//                    consoleLog.MSSStart_Joints_ARM2_EP = MSSStart_Joints_ARM2_EP;
//                    consoleLog.MSSStart_Joints_ARM2_WP = MSSStart_Joints_ARM2_WP;
//                    consoleLog.MSSStart_Joints_ARM2_WY = MSSStart_Joints_ARM2_WY;
//                    consoleLog.MSSStart_Joints_ARM2_WR = MSSStart_Joints_ARM2_WR;
//                    consoleLog.MSSStart_Joints_EOTP = MSSStart_Joints_EOTP;
//                }

//                #endregion

//                #region MSS Status at End of Shift

//                int indexOShiftLog = 0;

//                var indexOfShiftLogNew = innerTextResult.FindIndex(g => g.Equals(" Shift Log "));
//                var indexOfShiftLogMid = innerTextResult.FindIndex(g => g.Equals(" Shift Log: "));

//                if (indexOfMSSStatusAtEndofShiftNew > 0 && indexOfShiftLogNew > 0)
//                {
//                    indexOfMSSStatusAtEndofShift = indexOfMSSStatusAtEndofShiftNew;
//                    indexOShiftLog = indexOfShiftLogNew;

//                    var MSSStatusatEndofShiftIndexRange = innerTextResult.GetRange(indexOfMSSStatusAtEndofShift, indexOShiftLog - indexOfMSSStatusAtEndofShift + 1);
//                    var MSSEnd_RWS_Lab = innerTextResult[indexOfMSSStatusAtEndofShift + 15];
//                    var MSSEnd_RWS_Cup = innerTextResult[indexOfMSSStatusAtEndofShift + 16];
//                    var MSSEnd_MT_WS = innerTextResult[indexOfMSSStatusAtEndofShift + 17];
//                    var MSSEnd_SSRMS_BasedOn_LEE = innerTextResult[indexOfMSSStatusAtEndofShift + 18];
//                    var MSSEnd_SPDM_BasedOn = innerTextResult[indexOfMSSStatusAtEndofShift + 19];
//                    var MSSEnd_FSTStates_MBS = innerTextResult[indexOfMSSStatusAtEndofShift + 20];
//                    var MSSEnd_FSTStates_POA = innerTextResult[indexOfMSSStatusAtEndofShift + 21];
//                    var MSSEnd_FSTStates_SSRMS = innerTextResult[indexOfMSSStatusAtEndofShift + 22];
//                    var MSSEnd_FSTStates_SPDMPSU = innerTextResult[indexOfMSSStatusAtEndofShift + 23];
//                    var MSSEnd_FSTStates_SPDMBody = innerTextResult[indexOfMSSStatusAtEndofShift + 24];
//                    var MSSEnd_FSTStates_SPDMSACU = innerTextResult[indexOfMSSStatusAtEndofShift + 25];
//                    var MSSEnd_FSTStates_SPDMArms = innerTextResult[indexOfMSSStatusAtEndofShift + 26];
//                    var MSSEnd_Payloads_SSRMSTipLee = innerTextResult[indexOfMSSStatusAtEndofShift + 35];
//                    var MSSEnd_Payloads_SPDMLee = innerTextResult[indexOfMSSStatusAtEndofShift + 36];
//                    var MSSEnd_Payloads_OTCM1 = innerTextResult[indexOfMSSStatusAtEndofShift + 37];
//                    var MSSEnd_Payloads_OTCM2 = innerTextResult[indexOfMSSStatusAtEndofShift + 38];
//                    var MSSStart_Payloads_MCASData = innerTextResult[indexOfMSSStatusAtEndofShift + 39];
//                    var MSSEnd_Payloads_MCAS = MSSStatusatEndofShiftIndexRange[MSSStatusatEndofShiftIndexRange.FindIndex(g => g.Equals("  MCAS  ")) + 7];
//                    var MSSStart_Payloads_EOTPData = innerTextResult[indexOfMSSStatusAtEndofShift + 40];
//                    var MSSEnd_Payloads_EOTP = MSSStatusatEndofShiftIndexRange[MSSStatusatEndofShiftIndexRange.FindIndex(g => g.Equals("  EOTP  ")) + 7];
//                    var MSSEnd_Payloads_POA = innerTextResult[indexOfMSSStatusAtEndofShift + 41];
//                    var MSSEnd_SSRMSConfiguration = innerTextResult[indexOfMSSStatusAtEndofShift + 43];
//                    var MSSEnd_SPDMConfiguration = innerTextResult[indexOfMSSStatusAtEndofShift + 45];
//                    var MSSEnd_Joints_SSRMS_SR = innerTextResult[indexOfMSSStatusAtEndofShift + 56];
//                    var MSSEnd_Joints_SSRMS_SY = innerTextResult[indexOfMSSStatusAtEndofShift + 57];
//                    var MSSEnd_Joints_SSRMS_SP = innerTextResult[indexOfMSSStatusAtEndofShift + 58];
//                    var MSSEnd_Joints_SSRMS_EP = innerTextResult[indexOfMSSStatusAtEndofShift + 59];
//                    var MSSEnd_Joints_SSRMS_WP = innerTextResult[indexOfMSSStatusAtEndofShift + 60];
//                    var MSSEnd_Joints_SSRMS_WY = innerTextResult[indexOfMSSStatusAtEndofShift + 61];
//                    var MSSEnd_Joints_SSRMS_WR = innerTextResult[indexOfMSSStatusAtEndofShift + 62];
//                    var MSSEnd_Joints_SSRMS_SPDMBR = innerTextResult[indexOfMSSStatusAtEndofShift + 63];
//                    var MSSEnd_Joints_ARM1_SR = innerTextResult[indexOfMSSStatusAtEndofShift + 65];
//                    var MSSEnd_Joints_ARM1_SY = innerTextResult[indexOfMSSStatusAtEndofShift + 66];
//                    var MSSEnd_Joints_ARM1_SP = innerTextResult[indexOfMSSStatusAtEndofShift + 67];
//                    var MSSEnd_Joints_ARM1_EP = innerTextResult[indexOfMSSStatusAtEndofShift + 68];
//                    var MSSEnd_Joints_ARM1_WP = innerTextResult[indexOfMSSStatusAtEndofShift + 69];
//                    var MSSEnd_Joints_ARM1_WY = innerTextResult[indexOfMSSStatusAtEndofShift + 70];
//                    var MSSEnd_Joints_ARM1_WR = innerTextResult[indexOfMSSStatusAtEndofShift + 71];
//                    var MSSEnd_Joints_ARM2_SR = innerTextResult[indexOfMSSStatusAtEndofShift + 74];
//                    var MSSEnd_Joints_ARM2_SY = innerTextResult[indexOfMSSStatusAtEndofShift + 75];
//                    var MSSEnd_Joints_ARM2_SP = innerTextResult[indexOfMSSStatusAtEndofShift + 76];
//                    var MSSEnd_Joints_ARM2_EP = innerTextResult[indexOfMSSStatusAtEndofShift + 77];
//                    var MSSEnd_Joints_ARM2_WP = innerTextResult[indexOfMSSStatusAtEndofShift + 78];
//                    var MSSEnd_Joints_ARM2_WY = innerTextResult[indexOfMSSStatusAtEndofShift + 79];
//                    var MSSEnd_Joints_ARM2_WR = innerTextResult[indexOfMSSStatusAtEndofShift + 80];
//                    var MSSEnd_Joints_EOTP = innerTextResult[indexOfMSSStatusAtEndofShift + 81];
//                    consoleLog.MSSEnd_RWS_Lab = MSSEnd_RWS_Lab;
//                    consoleLog.MSSEnd_RWS_Cup = MSSEnd_RWS_Cup;
//                    consoleLog.MSSEnd_MT_WS = MSSEnd_MT_WS;
//                    consoleLog.MSSEnd_SSRMS_BasedOn_LEE = MSSEnd_SSRMS_BasedOn_LEE;
//                    consoleLog.MSSEnd_SPDM_BasedOn = MSSEnd_SPDM_BasedOn;
//                    consoleLog.MSSEnd_FSTStates_MBS = MSSEnd_FSTStates_MBS;
//                    consoleLog.MSSEnd_FSTStates_POA = MSSEnd_FSTStates_POA;
//                    consoleLog.MSSEnd_FSTStates_SSRMS = MSSEnd_FSTStates_SSRMS;
//                    consoleLog.MSSEnd_FSTStates_SPDMPSU = MSSEnd_FSTStates_SPDMPSU;
//                    consoleLog.MSSEnd_FSTStates_SPDMBody = MSSEnd_FSTStates_SPDMBody;
//                    consoleLog.MSSEnd_FSTStates_SPDMSACU = MSSEnd_FSTStates_SPDMSACU;
//                    consoleLog.MSSEnd_FSTStates_SPDMArms = MSSEnd_FSTStates_SPDMArms;
//                    consoleLog.MSSEnd_Payloads_SSRMSTipLee = MSSEnd_Payloads_SSRMSTipLee;
//                    consoleLog.MSSEnd_Payloads_SPDMLee = MSSEnd_Payloads_SPDMLee;
//                    consoleLog.MSSEnd_Payloads_OTCM1 = MSSEnd_Payloads_OTCM1;
//                    consoleLog.MSSEnd_Payloads_OTCM2 = MSSEnd_Payloads_OTCM2;
//                    consoleLog.MSSEnd_Payloads_MCAS = MSSEnd_Payloads_MCAS;
//                    consoleLog.MSSEnd_Payloads_EOTP = MSSEnd_Payloads_EOTP;
//                    consoleLog.MSSEnd_Payloads_POA = MSSEnd_Payloads_POA;
//                    consoleLog.MSSEnd_SSRMSConfiguration = MSSEnd_SSRMSConfiguration;
//                    consoleLog.MSSEnd_SPDMConfiguration = MSSEnd_SPDMConfiguration;
//                    consoleLog.MSSEnd_Joints_SSRMS_SR = MSSEnd_Joints_SSRMS_SR;
//                    consoleLog.MSSEnd_Joints_SSRMS_SY = MSSEnd_Joints_SSRMS_SY;
//                    consoleLog.MSSEnd_Joints_SSRMS_SP = MSSEnd_Joints_SSRMS_SP;
//                    consoleLog.MSSEnd_Joints_SSRMS_EP = MSSEnd_Joints_SSRMS_EP;
//                    consoleLog.MSSEnd_Joints_SSRMS_WP = MSSEnd_Joints_SSRMS_WP;
//                    consoleLog.MSSEnd_Joints_SSRMS_WY = MSSEnd_Joints_SSRMS_WY;
//                    consoleLog.MSSEnd_Joints_SSRMS_WR = MSSEnd_Joints_SSRMS_WR;
//                    consoleLog.MSSEnd_Joints_SSRMS_SPDMBR = MSSEnd_Joints_SSRMS_SPDMBR;
//                    consoleLog.MSSEnd_Joints_ARM1_SR = MSSEnd_Joints_ARM1_SR;
//                    consoleLog.MSSEnd_Joints_ARM1_SY = MSSEnd_Joints_ARM1_SY;
//                    consoleLog.MSSEnd_Joints_ARM1_SP = MSSEnd_Joints_ARM1_SP;
//                    consoleLog.MSSEnd_Joints_ARM1_EP = MSSEnd_Joints_ARM1_EP;
//                    consoleLog.MSSEnd_Joints_ARM1_WP = MSSEnd_Joints_ARM1_WP;
//                    consoleLog.MSSEnd_Joints_ARM1_WY = MSSEnd_Joints_ARM1_WY;
//                    consoleLog.MSSEnd_Joints_ARM1_WR = MSSEnd_Joints_ARM1_WR;
//                    consoleLog.MSSEnd_Joints_ARM2_SR = MSSEnd_Joints_ARM2_SR;
//                    consoleLog.MSSEnd_Joints_ARM2_SY = MSSEnd_Joints_ARM2_SY;
//                    consoleLog.MSSEnd_Joints_ARM2_SP = MSSEnd_Joints_ARM2_SP;
//                    consoleLog.MSSEnd_Joints_ARM2_EP = MSSEnd_Joints_ARM2_EP;
//                    consoleLog.MSSEnd_Joints_ARM2_WP = MSSEnd_Joints_ARM2_WP;
//                    consoleLog.MSSEnd_Joints_ARM2_WY = MSSEnd_Joints_ARM2_WY;
//                    consoleLog.MSSEnd_Joints_ARM2_WR = MSSEnd_Joints_ARM2_WR;
//                    consoleLog.MSSEnd_Joints_EOTP = MSSEnd_Joints_EOTP;
//                }
//                if (indexOfMSSStatusAtEndofShiftMid > 0 && indexOfShiftLogMid > 0)
//                {
//                    indexOfMSSStatusAtEndofShift = indexOfMSSStatusAtEndofShiftMid;
//                    indexOShiftLog = indexOfShiftLogMid;

//                    var MSSStatusatEndofShiftIndexRange = innerTextResult.GetRange(indexOfMSSStatusAtEndofShift, indexOShiftLog - indexOfMSSStatusAtEndofShift + 1);
//                    var MSSEnd_RWS_Lab = innerTextResult[indexOfMSSStatusAtEndofShift + 15];
//                    var MSSEnd_RWS_Cup = innerTextResult[indexOfMSSStatusAtEndofShift + 16];
//                    var MSSEnd_MT_WS = innerTextResult[indexOfMSSStatusAtEndofShift + 17];
//                    var MSSEnd_SSRMS_BasedOn_LEE = innerTextResult[indexOfMSSStatusAtEndofShift + 18];
//                    var MSSEnd_SPDM_BasedOn = innerTextResult[indexOfMSSStatusAtEndofShift + 19];
//                    var MSSEnd_FSTStates_MBS = innerTextResult[indexOfMSSStatusAtEndofShift + 20];
//                    var MSSEnd_FSTStates_POA = innerTextResult[indexOfMSSStatusAtEndofShift + 21];
//                    var MSSEnd_FSTStates_SSRMS = innerTextResult[indexOfMSSStatusAtEndofShift + 22];
//                    var MSSEnd_FSTStates_SPDMPSU = innerTextResult[indexOfMSSStatusAtEndofShift + 23];
//                    var MSSEnd_FSTStates_SPDMBody = innerTextResult[indexOfMSSStatusAtEndofShift + 24];
//                    var MSSEnd_FSTStates_SPDMSACU = innerTextResult[indexOfMSSStatusAtEndofShift + 25];
//                    var MSSEnd_FSTStates_SPDMArms = innerTextResult[indexOfMSSStatusAtEndofShift + 26];
//                    var MSSEnd_Payloads_SSRMSTipLee = innerTextResult[indexOfMSSStatusAtEndofShift + 35];
//                    var MSSEnd_Payloads_SPDMLee = innerTextResult[indexOfMSSStatusAtEndofShift + 36];
//                    var MSSEnd_Payloads_OTCM1 = innerTextResult[indexOfMSSStatusAtEndofShift + 37];
//                    var MSSEnd_Payloads_OTCM2 = innerTextResult[indexOfMSSStatusAtEndofShift + 38];
//                    var MSSEnd_Payloads_MCAS = MSSStatusatEndofShiftIndexRange[MSSStatusatEndofShiftIndexRange.FindIndex(g => g.Equals("  MCAS  ")) + 7];
//                    var MSSEnd_Payloads_EOTP = MSSStatusatEndofShiftIndexRange[MSSStatusatEndofShiftIndexRange.FindIndex(g => g.Equals("  EOTP  ")) + 7];
//                    var MSSEnd_Payloads_POA = innerTextResult[indexOfMSSStatusAtEndofShift + 41];
//                    var MSSEnd_SSRMSConfiguration = innerTextResult[indexOfMSSStatusAtEndofShift + 43];
//                    var MSSEnd_SPDMConfiguration = innerTextResult[indexOfMSSStatusAtEndofShift + 45];
//                    var MSSEnd_Joints_SSRMS_SR = innerTextResult[indexOfMSSStatusAtEndofShift + 56];
//                    var MSSEnd_Joints_SSRMS_SY = innerTextResult[indexOfMSSStatusAtEndofShift + 57];
//                    var MSSEnd_Joints_SSRMS_SP = innerTextResult[indexOfMSSStatusAtEndofShift + 58];
//                    var MSSEnd_Joints_SSRMS_EP = innerTextResult[indexOfMSSStatusAtEndofShift + 59];
//                    var MSSEnd_Joints_SSRMS_WP = innerTextResult[indexOfMSSStatusAtEndofShift + 60];
//                    var MSSEnd_Joints_SSRMS_WY = innerTextResult[indexOfMSSStatusAtEndofShift + 61];
//                    var MSSEnd_Joints_SSRMS_WR = innerTextResult[indexOfMSSStatusAtEndofShift + 62];
//                    var MSSEnd_Joints_SSRMS_SPDMBR = innerTextResult[indexOfMSSStatusAtEndofShift + 63];
//                    var MSSEnd_Joints_ARM1_SR = innerTextResult[indexOfMSSStatusAtEndofShift + 65];
//                    var MSSEnd_Joints_ARM1_SY = innerTextResult[indexOfMSSStatusAtEndofShift + 66];
//                    var MSSEnd_Joints_ARM1_SP = innerTextResult[indexOfMSSStatusAtEndofShift + 67];
//                    var MSSEnd_Joints_ARM1_EP = innerTextResult[indexOfMSSStatusAtEndofShift + 68];
//                    var MSSEnd_Joints_ARM1_WP = innerTextResult[indexOfMSSStatusAtEndofShift + 69];
//                    var MSSEnd_Joints_ARM1_WY = innerTextResult[indexOfMSSStatusAtEndofShift + 70];
//                    var MSSEnd_Joints_ARM1_WR = innerTextResult[indexOfMSSStatusAtEndofShift + 71];
//                    var MSSEnd_Joints_ARM2_SR = innerTextResult[indexOfMSSStatusAtEndofShift + 74];
//                    var MSSEnd_Joints_ARM2_SY = innerTextResult[indexOfMSSStatusAtEndofShift + 75];
//                    var MSSEnd_Joints_ARM2_SP = innerTextResult[indexOfMSSStatusAtEndofShift + 76];
//                    var MSSEnd_Joints_ARM2_EP = innerTextResult[indexOfMSSStatusAtEndofShift + 77];
//                    var MSSEnd_Joints_ARM2_WP = innerTextResult[indexOfMSSStatusAtEndofShift + 78];
//                    var MSSEnd_Joints_ARM2_WY = innerTextResult[indexOfMSSStatusAtEndofShift + 79];
//                    var MSSEnd_Joints_ARM2_WR = innerTextResult[indexOfMSSStatusAtEndofShift + 80];
//                    var MSSEnd_Joints_EOTP = innerTextResult[indexOfMSSStatusAtEndofShift + 81];
//                    consoleLog.MSSEnd_RWS_Lab = MSSEnd_RWS_Lab;
//                    consoleLog.MSSEnd_RWS_Cup = MSSEnd_RWS_Cup;
//                    consoleLog.MSSEnd_MT_WS = MSSEnd_MT_WS;
//                    consoleLog.MSSEnd_SSRMS_BasedOn_LEE = MSSEnd_SSRMS_BasedOn_LEE;
//                    consoleLog.MSSEnd_SPDM_BasedOn = MSSEnd_SPDM_BasedOn;
//                    consoleLog.MSSEnd_FSTStates_MBS = MSSEnd_FSTStates_MBS;
//                    consoleLog.MSSEnd_FSTStates_POA = MSSEnd_FSTStates_POA;
//                    consoleLog.MSSEnd_FSTStates_SSRMS = MSSEnd_FSTStates_SSRMS;
//                    consoleLog.MSSEnd_FSTStates_SPDMPSU = MSSEnd_FSTStates_SPDMPSU;
//                    consoleLog.MSSEnd_FSTStates_SPDMBody = MSSEnd_FSTStates_SPDMBody;
//                    consoleLog.MSSEnd_FSTStates_SPDMSACU = MSSEnd_FSTStates_SPDMSACU;
//                    consoleLog.MSSEnd_FSTStates_SPDMArms = MSSEnd_FSTStates_SPDMArms;
//                    consoleLog.MSSEnd_Payloads_SSRMSTipLee = MSSEnd_Payloads_SSRMSTipLee;
//                    consoleLog.MSSEnd_Payloads_SPDMLee = MSSEnd_Payloads_SPDMLee;
//                    consoleLog.MSSEnd_Payloads_OTCM1 = MSSEnd_Payloads_OTCM1;
//                    consoleLog.MSSEnd_Payloads_OTCM2 = MSSEnd_Payloads_OTCM2;
//                    consoleLog.MSSEnd_Payloads_MCAS = MSSEnd_Payloads_MCAS;
//                    consoleLog.MSSEnd_Payloads_EOTP = MSSEnd_Payloads_EOTP;
//                    consoleLog.MSSEnd_Payloads_POA = MSSEnd_Payloads_POA;
//                    consoleLog.MSSEnd_SSRMSConfiguration = MSSEnd_SSRMSConfiguration;
//                    consoleLog.MSSEnd_SPDMConfiguration = MSSEnd_SPDMConfiguration;
//                    consoleLog.MSSEnd_Joints_SSRMS_SR = MSSEnd_Joints_SSRMS_SR;
//                    consoleLog.MSSEnd_Joints_SSRMS_SY = MSSEnd_Joints_SSRMS_SY;
//                    consoleLog.MSSEnd_Joints_SSRMS_SP = MSSEnd_Joints_SSRMS_SP;
//                    consoleLog.MSSEnd_Joints_SSRMS_EP = MSSEnd_Joints_SSRMS_EP;
//                    consoleLog.MSSEnd_Joints_SSRMS_WP = MSSEnd_Joints_SSRMS_WP;
//                    consoleLog.MSSEnd_Joints_SSRMS_WY = MSSEnd_Joints_SSRMS_WY;
//                    consoleLog.MSSEnd_Joints_SSRMS_WR = MSSEnd_Joints_SSRMS_WR;
//                    consoleLog.MSSEnd_Joints_SSRMS_SPDMBR = MSSEnd_Joints_SSRMS_SPDMBR;
//                    consoleLog.MSSEnd_Joints_ARM1_SR = MSSEnd_Joints_ARM1_SR;
//                    consoleLog.MSSEnd_Joints_ARM1_SY = MSSEnd_Joints_ARM1_SY;
//                    consoleLog.MSSEnd_Joints_ARM1_SP = MSSEnd_Joints_ARM1_SP;
//                    consoleLog.MSSEnd_Joints_ARM1_EP = MSSEnd_Joints_ARM1_EP;
//                    consoleLog.MSSEnd_Joints_ARM1_WP = MSSEnd_Joints_ARM1_WP;
//                    consoleLog.MSSEnd_Joints_ARM1_WY = MSSEnd_Joints_ARM1_WY;
//                    consoleLog.MSSEnd_Joints_ARM1_WR = MSSEnd_Joints_ARM1_WR;
//                    consoleLog.MSSEnd_Joints_ARM2_SR = MSSEnd_Joints_ARM2_SR;
//                    consoleLog.MSSEnd_Joints_ARM2_SY = MSSEnd_Joints_ARM2_SY;
//                    consoleLog.MSSEnd_Joints_ARM2_SP = MSSEnd_Joints_ARM2_SP;
//                    consoleLog.MSSEnd_Joints_ARM2_EP = MSSEnd_Joints_ARM2_EP;
//                    consoleLog.MSSEnd_Joints_ARM2_WP = MSSEnd_Joints_ARM2_WP;
//                    consoleLog.MSSEnd_Joints_ARM2_WY = MSSEnd_Joints_ARM2_WY;
//                    consoleLog.MSSEnd_Joints_ARM2_WR = MSSEnd_Joints_ARM2_WR;
//                    consoleLog.MSSEnd_Joints_EOTP = MSSEnd_Joints_EOTP;
//                }

//                #endregion

//                massDataContext.ConsoleLogs.Add(consoleLog);
//                //massDataContext.SaveChanges();

//                #region Saving as PDF

//                Document wordDoc = new Document();
//                try
//                {

//                    Object oMissing = System.Reflection.Missing.Value;
//                   // wordDoc = word.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
//                    //word.Visible = false;
//                    Object filepath = file;
//                    Object confirmconversion = System.Reflection.Missing.Value;
//                    Object readOnly = false;
//                    Object saveto = $@"{archivePath.FullName}\{fileName}.pdf";
//                    Object oallowsubstitution = System.Reflection.Missing.Value;

//                    //wordDoc = word.Documents.Open(ref filepath, ref confirmconversion, ref readOnly, ref oMissing);
//                    object fileFormat = WdSaveFormat.wdFormatPDF;
//                    wordDoc.SaveAs(ref saveto, ref fileFormat, ref oallowsubstitution, ref oMissing);

//                    wordDoc.Close();

//                    var archiveFileDestination = $@"{ archivePath.FullName}\{fileName}{Extension}";
//                    var archivefolderToMove = Path.GetFileNameWithoutExtension(file) + "_files";
//                    var archiveFolderDestination = $@"{archivePath.FullName}\{archivefolderToMove}";

//                    if (File.Exists(archiveFileDestination))
//                    {
//                        File.Delete(archiveFileDestination);
//                    }
//                    File.Move(file, archiveFileDestination);
//                    if (Directory.Exists($@"{folderPath}\{archivefolderToMove}"))
//                    {
//                        if (Directory.Exists(archiveFolderDestination))
//                        {
//                            Directory.Delete(archiveFolderDestination, true);
//                        }
//                        Directory.Move($@"{folderPath}\{archivefolderToMove}", archiveFolderDestination);
//                    }
//                }
//                catch (Exception ex)
//                {
//                    wordDoc.Close();
//                    //word.Quit();
//                    Console.WriteLine(ex.Message);
//                    return;
//                }
//            }
//            else
//            {
//                var existingFileDestination = $@"{ alreadyExistsPath.FullName}\{fileName}{Extension}";
//                var existingFolderToMove = Path.GetFileNameWithoutExtension(file) + "_files";
//                var existingFolderDestination = $@"{alreadyExistsPath.FullName}\{existingFolderToMove}";

//                if (File.Exists(existingFileDestination))
//                {
//                    File.Delete(existingFileDestination);
//                }
//                File.Move(file, existingFileDestination);
//                if (Directory.Exists($@"{folderPath}\{existingFolderToMove}"))
//                {
//                    if (Directory.Exists(existingFolderDestination))
//                    {
//                        Directory.Delete(existingFolderDestination, true);
//                    }
//                    Directory.Move($@"{folderPath}\{existingFolderToMove}", existingFolderDestination);
//                }
//            }

//            #endregion
//        }
//        //word.Quit();
//    }

//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.Message);
//    }
//}