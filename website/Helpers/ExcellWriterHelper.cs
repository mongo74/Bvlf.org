using System.Collections;
using System.Collections.Generic;
using System.IO;
using bvlf_v2.Models;
using OfficeOpenXml;

namespace bvlf_v2.Helpers
{
    internal static class ExcellWriterHelper
    {
        public static byte[] CreateStudiedagInschrijvingenExportFile(StudiedagInschrijvingforExportViewModel model)
        {
            using (var ms = new MemoryStream())
            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("worksheet");

                var i = 1;

                // set columnheads
                ws.Cells["A1"].Value = "nr";
                ws.Cells["B1"].Value = "Lidnr";
                ws.Cells["C1"].Value = "Email";
                ws.Cells["D1"].Value = "Naam";
                ws.Cells["E1"].Value = "Voornaam";
                ws.Cells["F1"].Value = "At 1";
                ws.Cells["G1"].Value = "At 2";
                ws.Cells["H1"].Value = "At 3";

                ws.Cells["J1"].Value = "Straat";
                ws.Cells["K1"].Value = "nr";
                ws.Cells["L1"].Value = "bus";
                ws.Cells["M1"].Value = "postcode";
                ws.Cells["N1"].Value = "plaats";
                ws.Cells["O1"].Value = "tel";
                ws.Cells["P1"].Value = "mobile";

                ws.Cells["Q1"].Value = "SCHOOL - NAAM";
                ws.Cells["R1"].Value = "SCHOOL - ADRES";
                ws.Cells["S1"].Value = "SCHOOL - TEL";
                ws.Cells["U1"].Value = "SCHOOL - EMAIL";

                ws.Cells["V1"].Value = "STATUS";

                ws.Cells["W1"].Value = "BETAALD DOOR SCHOOL";


                foreach (var inschrijving in model.Inschrijvingen)
                {
                    i++;
                    var member = inschrijving.Member;
                    ws.Cells["A" + i].Value = i - 1;
                    ws.Cells["B" + i].Value = member.LidNr;
                    ws.Cells["C" + i].Value = member.Email;
                    ws.Cells["D" + i].Value = member.Name.ToUpper();
                    ws.Cells["E" + i].Value = member.FirstName;

                    LoadSessions(ws, i, inschrijving.Sessions);

                    ws.Cells["J" + i].Value = member.Street;
                    ws.Cells["K" + i].Value = member.Nr;
                    ws.Cells["L" + i].Value = member.Box;
                    ws.Cells["m" + i].Value = member.Zipcode;
                    ws.Cells["N" + i].Value = member.Location;
                    ws.Cells["O" + i].Value = member.Phone;
                    ws.Cells["P" + i].Value = member.Mobile;

                    ws.Cells["Q" + i].Value = member.School1Naam;
                    var schoolInfo = string.Format("{0} {1} {2}, {3} {4}",
                        member.School1Street, member.School1Nr, member.School1Box, member.School1Zipcode,
                        member.School1_Naam);

                    ws.Cells["R" + i].Value = schoolInfo;

                    ws.Cells["S" + i].Value = member.School1Phone;
                    ws.Cells["T" + i].Value = member.School1Email;


                    ws.Cells["U" + i].Value = string.Format("{0}-{1}", member.Name.ToUpper(),
                        member.FirstName.ToUpper());

                    ws.Cells["V" + i].Value = inschrijving.Status;
                    ws.Cells["W" + i].Value = inschrijving.PaidBySchool;
                }
                package.SaveAs(ms);
                return ms.ToArray();
            }
        }

        private static void LoadSessions(ExcelWorksheet ws, int i, List<string> Sessions)
        {
            var arrayList = new ArrayList {"F", "G", "H", "I"};
            for (var j = 0; j < Sessions.Count; j++)
            {
                var cell = string.Format("{0}{1}", arrayList[j], i);
                ws.Cells[cell].Value = Sessions[j] ?? "";
            }
        }

        public static byte[] CreateLedenlijstExportFile(MemberProfileListForExortViewModel model)
        {
            using (var ms = new MemoryStream())
            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("worksheet");

                var i = 1;

                // set columnheads
                ws.Cells["A1"].Value = "nr";
                ws.Cells["B1"].Value = "Lidnr";
                ws.Cells["C1"].Value = "Status";
                ws.Cells["D1"].Value = "Email";
                ws.Cells["E1"].Value = "Gender";
                ws.Cells["F1"].Value = "Naam";
                ws.Cells["G1"].Value = "Voornaam";
                ws.Cells["H1"].Value = "Straat";
                ws.Cells["I1"].Value = "nr";
                ws.Cells["J1"].Value = "bus";

                ws.Cells["K1"].Value = "postcode";
                ws.Cells["L1"].Value = "plaats";
                ws.Cells["M1"].Value = "tel";
                ws.Cells["N1"].Value = "mobile";
                ws.Cells["O1"].Value = "Fipf nr";

                ws.Cells["Q1"].Value = "School 1";
                ws.Cells["X1"].Value = "School 2";

                foreach (var lid in model.Ledenlijst)
                {
                    i++;
                    ws.Cells["A" + i].Value = i - 1;
                    ws.Cells["B" + i].Value = lid.LidNr;
                    ws.Cells["C" + i].Value = lid.BvlfMemberStatus;
                    ws.Cells["D" + i].Value = lid.Email;
                    ws.Cells["E" + i].Value = lid.Gender;
                    ws.Cells["F" + i].Value = lid.Name.ToUpper();
                    ws.Cells["G" + i].Value = lid.FirstName.ToUpper();
                    ws.Cells["H" + i].Value = lid.Street;
                    ws.Cells["I" + i].Value = lid.Nr;
                    ws.Cells["J" + i].Value = lid.Box;

                    ws.Cells["K" + i].Value = lid.Zipcode;
                    ws.Cells["L" + i].Value = lid.Location;
                    ws.Cells["M" + i].Value = lid.Phone;
                    ws.Cells["N" + i].Value = lid.Mobile;
                    ws.Cells["O" + i].Value = lid.FipfNr;

                    ws.Cells["Q" + i].Value = lid.School1Naam;
                    ws.Cells["R" + i].Value = lid.School1Street;
                    ws.Cells["S" + i].Value = lid.School1Nr;
                    ws.Cells["T" + i].Value = lid.School1Box;
                    ws.Cells["U" + i].Value = lid.School1Zipcode;
                    ws.Cells["V" + i].Value = lid.School1_Naam;
                    ws.Cells["W" + i].Value = lid.School1Phone;
                    ws.Cells["X" + i].Value = lid.School1Email;

                    ws.Cells["Y" + i].Value = lid.School2Name;
                    ws.Cells["Z" + i].Value = lid.School2Street;
                    ws.Cells["AA" + i].Value = lid.School2Nr;
                    ws.Cells["AB" + i].Value = lid.School2Box;
                    ws.Cells["AC" + i].Value = lid.School2Zipcode;
                    ws.Cells["AD" + i].Value = lid.School2Plaats;
                    ws.Cells["AE" + i].Value = lid.School2Tel;
                    ws.Cells["AF" + i].Value = lid.School2Email;
                    ws.Cells["AG" + i].Value = lid.School2Email;
                }

                package.SaveAs(ms);
                return ms.ToArray();
            }
        }
    }
}