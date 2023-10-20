using EasyLife.Interfaces;
using EasyLife.Models;
using EasyLife.Pages;
using EasyLife.Services;
using FreshMvvm;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.StyledXmlParser.Jsoup.Nodes;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using static SQLite.SQLite3;
using Cell = iText.Layout.Element.Cell;
using Document = iText.Layout.Document;
using TextAlignment = iText.Layout.Properties.TextAlignment;

namespace EasyLife.PageModels
{
    class Balance_PageModel : FreshBasePageModel
    {
        public Balance_PageModel()
        {
            Bundles = new ObservableRangeCollection<StackholderBundle>();

            Stackholderbundel_Outcome_Account = new ObservableRangeCollection<Stackholder>();
            Stackholderbundel_Income_Account = new ObservableRangeCollection<Stackholder>();
            Stackholderbundel_Outcome_Cash = new ObservableRangeCollection<Stackholder>();
            Stackholderbundel_Income_Cash = new ObservableRangeCollection<Stackholder>();


            Load_Command = new AsyncCommand(Load);
            Period_Command = new AsyncCommand(Period_Popup);
            Settings_Command = new AsyncCommand(Settings_Methode);
            Create_PDF_Command = new AsyncCommand(Create_PDF_Methode);
        }

        private async Task Create_PDF_Methode()
        {
            try
            {
                if (Bundles.Count() != 0)
                {
                    var result0 = await Shell.Current.DisplayAlert("PDF erstellen", "Wollen Sie wirklich eine PDF von der Bilanz " + Current_Viewtime.Month + " " + Current_Viewtime.Year + " erstellen?", "Ja", "Nein");

                    if(result0 == true)
                    {
                        string pdf_filename = "Bilanz " + Current_Viewtime.Month + " " + Current_Viewtime.Year + ".pdf";

                        string pdf_path = DependencyService.Get<IAccessFile>().CreateFile(pdf_filename);

                        if (File.Exists(pdf_path) == true)
                        {
                            var result = await Shell.Current.DisplayAlert("Datei existiert schon", "Diese Bilanz existiert schon auf ihrem Gerät. Wollen Sie diese Bilanz mit der jetzigen Bilanz übeschreiben?", "Ja", "Nein, neue Datei anlegen");

                            if (result == false)
                            {
                                bool indicator = false;

                                int identifier = 1;

                                while (indicator == false)
                                {
                                    pdf_filename = "Bilanz " + Current_Viewtime.Month + " " + Current_Viewtime.Year + " (" + identifier + ").pdf";

                                    if (File.Exists(DependencyService.Get<IAccessFile>().CreateFile(pdf_filename)) == false)
                                    {
                                        indicator = true;

                                        pdf_path = DependencyService.Get<IAccessFile>().CreateFile(pdf_filename);
                                    }

                                    identifier++;
                                }
                            }
                        }

                        FileStream pdf_stream = new FileStream(pdf_path, FileMode.OpenOrCreate);

                        PdfWriter writer = new PdfWriter(pdf_stream);

                        PdfDocument pdfDocument = new PdfDocument(writer);

                        Document doc = new Document(pdfDocument);

                        Paragraph header = new Paragraph("Haushaltsbuchbilanz").SetTextAlignment((iText.Layout.Properties.TextAlignment?)TextAlignment.CENTER).SetFontSize(20);
                        doc.Add(header);

                        Paragraph subheader = new Paragraph("vom " + Current_Viewtime.Month + " " + Current_Viewtime.Year + "").SetTextAlignment((iText.Layout.Properties.TextAlignment?)TextAlignment.CENTER).SetFontSize(15);
                        doc.Add(subheader);

                        LineSeparator ls = new LineSeparator(new SolidLine());
                        doc.Add(ls);

                        Paragraph spacerow = new Paragraph("       ").SetHeight(20);
                        doc.Add(spacerow);

                        Table table = new Table(new float[] { 300, 50, 120, 30 }).SetWidth(500).SetPadding(0).SetFontColor(ColorConstants.BLACK).SetFontSize(15).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

                        Border borderbottomheader = new SolidBorder(ColorConstants.BLACK, 1);

                        Cell cell1 = new Cell(1, 1).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold().SetBorderRight(Border.NO_BORDER).SetBorderBottom(borderbottomheader).SetBorderLeft(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER).Add(new Paragraph("Zweck"));
                        table.AddHeaderCell(cell1);

                        Cell cell2 = new Cell(1, 1).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold().SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderBottom(borderbottomheader).SetBorderTop(Border.NO_BORDER).Add(new Paragraph("Anzahl"));
                        table.AddHeaderCell(cell2);

                        Cell cell3 = new Cell(1, 1).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold().SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderBottom(borderbottomheader).SetBorderTop(Border.NO_BORDER).Add(new Paragraph("Summe"));
                        table.AddHeaderCell(cell3);

                        Cell cell4 = new Cell(1, 1).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER).SetBorderBottom(borderbottomheader);
                        table.AddHeaderCell(cell4);

                        int follower = 0;

                        foreach (var stackholderbundle in Bundles)
                        {
                            if (stackholderbundle.Visibility == true)
                            {
                                if (stackholderbundle.StackholderSource.Count() == 0)
                                {
                                    if (stackholderbundle.Total_Sum == null)
                                    {
                                        Cell cellsubsum = new Cell(1, 3).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold().SetUnderline(2, -4).SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER).Add(new Paragraph("" + stackholderbundle.Total_Text + " " + stackholderbundle.Sum + " €     "));
                                        table.AddCell(cellsubsum);

                                        Cell cell_space = new Cell(1, 1).SetBorderBottom(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER);
                                        table.AddCell(cell_space);

                                        Cell cell_spacerow = new Cell(1, 4).SetBorderBottom(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetHeight(5);
                                        table.AddCell(cell_spacerow);
                                    }
                                    else
                                    {
                                        if (follower == 0)
                                        {
                                            Cell cellsubsum = new Cell(1, 3).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold().SetUnderline(2, -3).SetUnderline(2, -6).SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER).Add(new Paragraph("" + stackholderbundle.Total_Text + " " + stackholderbundle.Total_Sum + " €     "));
                                            table.AddCell(cellsubsum);

                                            Cell cell_space = new Cell(1, 1).SetBorderBottom(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER);
                                            table.AddCell(cell_space);

                                            Cell cell_line = new Cell(1, 4).SetBorderBottom(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBackgroundColor(ColorConstants.GRAY).SetHeight((float)0.5);
                                            table.AddCell(cell_line);

                                            Cell cell_spacerow = new Cell(1, 4).SetBorderBottom(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetHeight(5);
                                            table.AddCell(cell_spacerow);

                                            follower++;
                                        }
                                        else
                                        {
                                            Cell cellsubsum = new Cell(1, 3).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold().SetUnderline(2, -3).SetUnderline(2, -6).SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER).Add(new Paragraph("" + stackholderbundle.Total_Text + " " + stackholderbundle.Total_Sum + " €     "));
                                            table.AddCell(cellsubsum);

                                            Cell cell_space = new Cell(1, 1).SetBorderBottom(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER);
                                            table.AddCell(cell_space);

                                            Cell cell_spacerow = new Cell(1, 4).SetBorderBottom(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetHeight(5);
                                            table.AddCell(cell_spacerow);
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (var stackholder in stackholderbundle.StackholderSource)
                                    {

                                        Cell cell_spacerow = new Cell(1, 4).SetBorderBottom(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetHeight(5);
                                        table.AddCell(cell_spacerow);

                                        Cell cell_1 = new Cell(1, 1).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold().SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER).Add(new Paragraph("" + stackholder.Reason + ""));
                                        table.AddCell(cell_1);

                                        Cell cell_2 = new Cell(1, 1).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetBold().SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER).Add(new Paragraph("" + stackholder.Count + ""));
                                        table.AddCell(cell_2);

                                        Cell cell_3 = new Cell(1, 1).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold().SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER).Add(new Paragraph("" + stackholder.Value + " €     "));
                                        table.AddCell(cell_3);

                                        Cell cell_4 = new Cell(1, 1).SetBorderBottom(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER);
                                        table.AddCell(cell_4);
                                    }
                                }
                            }
                        }

                        doc.Add(table);

                        Table table2 = new Table(new float[] { 300, 50, 120, 30 }).SetWidth(500).SetPadding(0).SetFontColor(ColorConstants.BLACK).SetFontSize(15).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

                        Cell cell0 = new Cell(1, 3).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT).SetBold().SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER).Add(new Paragraph("Restgeld : " + Total + " €     "));
                        table2.AddHeaderCell(cell0);

                        Cell cell04 = new Cell(1, 1).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorderBottom(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER);
                        table2.AddHeaderCell(cell04);

                        doc.Add(table2);

                        doc.Close();

                        await Notificater("Die Bilanz wurder erfolgreich zu der PDF (" + pdf_filename + ") exportiert.\nSie finden diese PDF im Ordner Phone/Documents oder Interner Speicher/Documents.");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert("Leere Bilanz", "Es kann keine PDF erstellt werden, wenn die Bilanz leer ist.", "Verstanden");
                }

            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        public async Task Load()
        {
            try
            {
                int result0 = await Check_for_existing_Balanceprofile();

                if(result0 == 0)
                {
                    await Add_to_Balanceprofile();
                }
                if(result0 == -1)
                {
                    return;
                }
                if(result0 == 1)
                {
                    await Add_to_Balanceprofile();
                }
                if(result0 == 2)
                {
                    await Create_Balanceprofile();
                }


                List<Stackholder> stackholderList = new List<Stackholder>();

                var transaktionscontent = await ContentService.Get_all_enabeled_Transaktion();

                List<Transaktion> sorted_after_month_transaktionscontent = new List<Transaktion>();

                foreach (var trans in transaktionscontent)
                {
                    if (trans.Balance_Visibility == true)
                    {
                        if (trans.Datum.Year == Current_Viewtime.Year)
                        {
                            if (trans.Datum.ToString("MMMM", new CultureInfo("de-DE")) == Current_Viewtime.Month)
                            {
                                sorted_after_month_transaktionscontent.Add(trans);
                            }
                            if (Current_Viewtime.Month == "")
                            {
                                sorted_after_month_transaktionscontent.Add(trans);
                            }
                        }
                    }
                }

                List<List<string>> balanceprofile = new List<List<string>>
                {
                    Bilanceprofile.Outcome_Account,
                    Bilanceprofile.Income_Account,
                    Bilanceprofile.Outcome_Cash,
                    Bilanceprofile.Income_Cash,
                    Bilanceprofile.Letter_Outcome,
                    Bilanceprofile.Letter_Income
                };

                List<ObservableRangeCollection<Stackholder>> bundle = new List<ObservableRangeCollection<Stackholder>>
                {
                    new ObservableRangeCollection<Stackholder>(),
                    new ObservableRangeCollection<Stackholder>(),
                    new ObservableRangeCollection<Stackholder>(),
                    new ObservableRangeCollection<Stackholder>(),
                    new ObservableRangeCollection<Stackholder>(),
                    new ObservableRangeCollection<Stackholder>()
                };

                List<string> total_value_bundle = new List<string>
                {
                   null,
                   null,
                   null,
                   null,
                   null,
                   null
                };

                List<Xamarin.Forms.Color> evaluating_of_total_bundle = new List<Xamarin.Forms.Color>
                {
                    new Xamarin.Forms.Color(),
                    new Xamarin.Forms.Color(),
                    new Xamarin.Forms.Color(),
                    new Xamarin.Forms.Color(),
                    new Xamarin.Forms.Color(),
                    new Xamarin.Forms.Color()
                };

                List<bool> stackholder_visivility_bundle = new List<bool>
                {
                    false,
                    false,
                    false,
                    false,
                    false,
                    false
                };

                List<string> total_text = new List<string>()
                {
                    "Ausgaben Konto Gesamt : ",
                    "Einnahmen Konto Gesamt : ",
                    "Barausgaben Gesamt : ",
                    "Bareinnahmen Gesamt : ",
                    "Ausgaben Briefumschlag Gesamt : ",
                    "Einnahmen Briefumschlag Gesamt : "
                };

                List<StackholderBundle> bundles = new List<StackholderBundle>();

                int h = 0;

                Bundles.Clear();

                foreach (var placeholder in balanceprofile)
                {
                    List<string> reasons = new List<string>();

                    foreach (var group in placeholder)
                    {
                        reasons.Add(group.Substring(0, group.IndexOf(":")));
                    }

                    reasons.Sort();

                    double total_value = 0;

                    foreach (string st in reasons)
                    {
                        if (sorted_after_month_transaktionscontent.Where(ts => ts.Zweck == st).Count() != 0)
                        {
                            double value = 0;

                            foreach (Transaktion trans in sorted_after_month_transaktionscontent)
                            {
                                if (trans.Zweck == st)
                                {
                                    value += double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE"));
                                }
                            }

                            Xamarin.Forms.Color color = Xamarin.Forms.Color.White;

                            if (value < 0)
                            {
                                color = Xamarin.Forms.Color.Red;
                            }
                            if (value > 0)
                            {
                                color = Xamarin.Forms.Color.Green;
                            }
                            if (value == 0)
                            {
                                color = Xamarin.Forms.Color.DarkGray;
                            }

                            total_value += value;

                            bundle[h].Add(new Stackholder { Reason = st, Count = sorted_after_month_transaktionscontent.Where(ts => ts.Zweck == st).Count(), Value = Math.Round(value, 2).ToString().Replace(".", ","), Evaluating = color, Detail = sorted_after_month_transaktionscontent.Where(ts => ts.Zweck == st).ToList(), });
                        }
                    }

                    if (total_value < 0)
                    {
                        evaluating_of_total_bundle[h] = Xamarin.Forms.Color.Red;
                    }
                    if (total_value > 0)
                    {
                        evaluating_of_total_bundle[h] = Xamarin.Forms.Color.Green;
                    }
                    if (total_value == 0)
                    {
                        evaluating_of_total_bundle[h] = Xamarin.Forms.Color.DarkGray;
                    }

                    total_value_bundle[h] = Math.Round(total_value, 2).ToString().Replace(".", ",");

                    if (bundle[h].Count() != 0)
                    {
                        stackholder_visivility_bundle[h] = true;
                    }
                    else
                    {
                        stackholder_visivility_bundle[h] = false;
                    }


                    bundles.Add(new StackholderBundle()
                    {
                        StackholderSource = bundle[h],
                        Visibility = stackholder_visivility_bundle[h],
                        Evaluation = Xamarin.Forms.Color.White,
                        Height = bundle[h].Count() * 38,
                        Definition = 0
                    });

                    bundles.Add(new StackholderBundle()
                    {
                        Visibility = stackholder_visivility_bundle[h],
                        Evaluation = evaluating_of_total_bundle[h],
                        Sum = total_value_bundle[h],
                        Total_Text = total_text[h],
                        Definition = 1
                    });

                    if (h == 1)
                    {
                        bool total_visibility = false;

                        Xamarin.Forms.Color total_color = Xamarin.Forms.Color.White;

                        double totalsum = 0;

                        if (String.IsNullOrEmpty(bundles[1].Sum) == false)
                        {
                            totalsum += double.Parse(bundles[1].Sum, NumberStyles.Any, new CultureInfo("de-DE"));
                        }

                        if (String.IsNullOrEmpty(bundles[3].Sum) == false)
                        {
                            totalsum += double.Parse(bundles[3].Sum, NumberStyles.Any, new CultureInfo("de-DE"));
                        }

                        if (bundles[1].Visibility == true && bundles[3].Visibility == true)
                        {
                            total_visibility = true;
                        }

                        if (totalsum < 0)
                        {
                            total_color = Xamarin.Forms.Color.Red;
                        }
                        if (totalsum > 0)
                        {
                            total_color = Xamarin.Forms.Color.Green;
                        }
                        if (totalsum == 0)
                        {
                            total_color = Xamarin.Forms.Color.DarkGray;
                        }

                        bundles.Add(new StackholderBundle()
                        {
                            Visibility = total_visibility,
                            Evaluation = total_color,
                            Total_Sum = Math.Round(totalsum, 2).ToString().Replace(".", ","),
                            Total_Text = "Rest Konto : ",
                            Definition = 2
                        });
                    }

                    if (h == 3)
                    {
                        bool total_visibility = false;

                        Xamarin.Forms.Color total_color = Xamarin.Forms.Color.White;

                        double totalsum = 0;

                        if (String.IsNullOrEmpty(bundles[6].Sum) == false)
                        {
                            totalsum += double.Parse(bundles[6].Sum, NumberStyles.Any, new CultureInfo("de-DE"));
                        }

                        if (String.IsNullOrEmpty(bundles[8].Sum) == false)
                        {
                            totalsum += double.Parse(bundles[8].Sum, NumberStyles.Any, new CultureInfo("de-DE"));
                        }

                        if (bundles[6].Visibility == true && bundles[8].Visibility == true)
                        {
                            total_visibility = true;
                        }

                        if (totalsum < 0)
                        {
                            total_color = Xamarin.Forms.Color.Red;
                        }
                        if (totalsum > 0)
                        {
                            total_color = Xamarin.Forms.Color.Green;
                        }
                        if (totalsum == 0)
                        {
                            total_color = Xamarin.Forms.Color.DarkGray;
                        }

                        bundles.Add(new StackholderBundle()
                        {
                            Visibility = total_visibility,
                            Evaluation = total_color,
                            Total_Sum = Math.Round(totalsum, 2).ToString().Replace(".", ","),
                            Total_Text = "Rest Bar : ",
                            Definition = 2
                        });
                    }

                    if (h == 5)
                    {
                        bool total_visibility = false;

                        Xamarin.Forms.Color total_color = Xamarin.Forms.Color.White;

                        double totalsum = 0;

                        if (String.IsNullOrEmpty(bundles[11].Sum) == false)
                        {
                            totalsum += double.Parse(bundles[11].Sum, NumberStyles.Any, new CultureInfo("de-DE"));
                        }

                        if (String.IsNullOrEmpty(bundles[13].Sum) == false)
                        {
                            totalsum += double.Parse(bundles[13].Sum, NumberStyles.Any, new CultureInfo("de-DE"));
                        }

                        if (bundles[11].Visibility == true && bundles[13].Visibility == true)
                        {
                            total_visibility = true;
                        }

                        if (totalsum < 0)
                        {
                            total_color = Xamarin.Forms.Color.Red;
                        }
                        if (totalsum > 0)
                        {
                            total_color = Xamarin.Forms.Color.Green;
                        }
                        if (totalsum == 0)
                        {
                            total_color = Xamarin.Forms.Color.DarkGray;
                        }

                        bundles.Add(new StackholderBundle()
                        {
                            Visibility = total_visibility,
                            Evaluation = total_color,
                            Total_Sum = Math.Round(totalsum, 2).ToString().Replace(".", ","),
                            Total_Text = "Rest Briefumschlag : ",
                            Definition = 2
                        });
                    }

                    h++;
                }

                foreach (StackholderBundle sb in bundles)
                {
                    if (sb.Visibility == true)
                    {
                        Bundles.Add(sb);
                    }
                }

                if (Bundles.Count() == 0)
                {
                    Stackholder_Bundle_Visibility = false;
                    Kein_Ergebnis_Stackholder_Status = true;
                    Title = "Bilanz";
                }
                else
                {

                    double total_value = 0;

                    int g = 0;

                    foreach (string st in total_value_bundle)
                    {
                        if (double.TryParse(st, NumberStyles.Any, new CultureInfo("de-DE"), out double result) == true)
                        {
                            if(g != 4 && g != 5)
                            {
                                total_value += result;
                            }
                        }

                        g++;
                    }

                    Total = Math.Round(total_value, 2).ToString().Replace(".", ",");

                    if (total_value < 0)
                    {
                        Evaluating_of_Totoal = Xamarin.Forms.Color.Red;
                    }
                    if (total_value > 0)
                    {
                        Evaluating_of_Totoal = Xamarin.Forms.Color.Green;
                    }
                    if (total_value == 0)
                    {
                        Evaluating_of_Totoal = Xamarin.Forms.Color.DarkGray;
                    }

                    Stackholder_Bundle_Visibility = true;
                    Kein_Ergebnis_Stackholder_Status = false;
                    Title = "" + Current_Viewtime.Month + " " + Current_Viewtime.Year + "";
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        public async Task Get_BalanceprofileList()
        {
            try
            {
                Bilanceprofiles_List.Clear();

                Bilanceprofiles_List.AddRange(await BalanceService.Get_all_Balanceprofile());
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        public async Task<int> Check_for_existing_Balanceprofile()
        {
            try
            {
                if(Bilanceprofile != null)
                {
                    return 0;
                }
                else
                {
                    Balanceprofile placeholder = new Balanceprofile();

                    bool validate = false;

                    try
                    {
                        placeholder = await BalanceService.Get_specific_Balanceprofile(Preferences.Get("Blanceprofile", 0));

                        validate = true;
                    }
                    catch 
                    {
                        validate = false;
                    }

                    if(validate == false)
                    {
                        await Get_BalanceprofileList();

                        placeholder = Bilanceprofiles_List.FirstOrDefault();

                        if(placeholder == null)
                        {
                            return 2;
                        }
                    }

                    Bilanceprofile = placeholder;

                    return 1;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");

                return -1;
            }
        }

        private async Task<bool> Check_for_Changes()
        {
            try
            {
                var result0 = await Check_for_existing_Balanceprofile();

                if (result0 == -1)
                {
                    return true;
                }
                if (result0 == 2)
                {
                    return true;
                }

                List<Zweck> count = new List<Zweck>(await ReasonService.Get_Enable_ReasonList());

                if ((Bilanceprofile.Outcome_Account.Count + Bilanceprofile.Income_Account.Count + Bilanceprofile.Outcome_Cash.Count + Bilanceprofile.Income_Cash.Count + Bilanceprofile.Ignore.Count + Bilanceprofile.Letter_Outcome.Count + Bilanceprofile.Letter_Income.Count) != count.Count)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");

                return true;
            }
        }

        private async Task Add_to_Balanceprofile()
        {
            try
            {
                if (await Check_for_Changes() == false)
                {
                    var zwecklist = await ReasonService.Get_all_Reason();

                    var result0 = await Check_for_existing_Balanceprofile();

                    if (result0 == -1)
                    {
                        return;
                    }
                    if (result0 == 2)
                    {
                        return;
                    }

                    List<string> initreasons = new List<string>();

                    initreasons.AddRange(Bilanceprofile.Income_Account);

                    initreasons.AddRange(Bilanceprofile.Outcome_Account);

                    initreasons.AddRange(Bilanceprofile.Income_Cash);

                    initreasons.AddRange(Bilanceprofile.Outcome_Cash);

                    initreasons.AddRange(Bilanceprofile.Letter_Outcome);

                    initreasons.AddRange(Bilanceprofile.Letter_Income);

                    initreasons.AddRange(Bilanceprofile.Ignore);

                    List<string> openreason = new List<string>();

                    foreach (Zweck zw in zwecklist)
                    {
                        if (initreasons.Contains(zw.Benutzerdefinierter_Zweck) == false)
                        {
                            if(zw.Reason_Visibility == true)
                            {
                                openreason.Add(zw.Benutzerdefinierter_Zweck);
                            }
                        }
                        else
                        {
                            if (zw.Reason_Visibility == false)
                            {
                                if (Bilanceprofile.Outcome_Account.Contains(zw.Benutzerdefinierter_Zweck) == true)
                                {
                                    Bilanceprofile.Outcome_Account.Remove(zw.Benutzerdefinierter_Zweck);
                                }

                                if (Bilanceprofile.Income_Account.Contains(zw.Benutzerdefinierter_Zweck) == true)
                                {
                                    Bilanceprofile.Income_Account.Remove(zw.Benutzerdefinierter_Zweck);
                                }

                                if (Bilanceprofile.Outcome_Cash.Contains(zw.Benutzerdefinierter_Zweck) == true)
                                {
                                    Bilanceprofile.Outcome_Cash.Remove(zw.Benutzerdefinierter_Zweck);
                                }

                                if (Bilanceprofile.Income_Cash.Contains(zw.Benutzerdefinierter_Zweck) == true)
                                {
                                    Bilanceprofile.Income_Cash.Remove(zw.Benutzerdefinierter_Zweck);
                                }

                                if (Bilanceprofile.Letter_Outcome.Contains(zw.Benutzerdefinierter_Zweck) == true)
                                {
                                    Bilanceprofile.Letter_Outcome.Remove(zw.Benutzerdefinierter_Zweck);
                                }

                                if (Bilanceprofile.Letter_Income.Contains(zw.Benutzerdefinierter_Zweck) == true)
                                {
                                    Bilanceprofile.Letter_Income.Remove(zw.Benutzerdefinierter_Zweck);
                                }

                                if (Bilanceprofile.Ignore.Contains(zw.Benutzerdefinierter_Zweck) == true)
                                {
                                    Bilanceprofile.Ignore.Remove(zw.Benutzerdefinierter_Zweck);
                                }
                            }
                        }
                    }


                    foreach (string reason in openreason)
                    {
                        bool indikator = false;

                        while (indikator == false)
                        {
                            var result = await Shell.Current.DisplayActionSheet("" + reason + " zuordnen", null, null, new string[] { "Ausgaben Konto", "Einnahmen Konto", "Barausgaben", "Bareinnahmen", "Ausgaben Briefumschlag" , "Einnahmen Briefumschlag", "ignorieren" });

                            if(result == null)
                            {
                                continue;
                            }

                            if (result == "Ausgaben Konto")
                            {
                                Bilanceprofile.Outcome_Account.Add(reason);

                                indikator = true;
                            }

                            if (result == "Einnahmen Konto")
                            {
                                Bilanceprofile.Income_Account.Add(reason);

                                indikator = true;
                            }

                            if (result == "Barausgaben")
                            {
                                Bilanceprofile.Outcome_Cash.Add(reason);

                                indikator = true;
                            }

                            if (result == "Bareinnahmen")
                            {
                                Bilanceprofile.Income_Cash.Add(reason);

                                indikator = true;
                            }

                            if (result == "Ausgaben Briefumschlag")
                            {
                                Bilanceprofile.Letter_Outcome.Add(reason);

                                indikator = true;
                            }

                            if (result == "Einnahmen Briefumschlag")
                            {
                                Bilanceprofile.Letter_Income.Add(reason);

                                indikator = true;
                            }

                            if (result == "ignorieren")
                            {
                                Bilanceprofile.Ignore.Add(reason);

                                indikator = true;
                            }
                        }
                    }

                    await BalanceService.Edit_Balanceprofile(Bilanceprofile); 
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async Task Settings_Methode()
        {
            try
            {
                bool indikator = false;

                while(indikator == false)
                {
                    var result = await Check_for_existing_Balanceprofile();

                    if (result == -1)
                    {
                        return;
                    }

                    string reorder_string = "";

                    if (result == 2)
                    {
                        reorder_string = "Es wurde kein Bilanzprofil ausgewählt, was neu geordnet werden kann.";
                    }
                    else
                    {
                        reorder_string = "Neuordnung des Bilanzprofiles " + Preferences.Get("Blanceprofile", 0) + "";
                    }

                    var result0 = await Shell.Current.DisplayActionSheet("Einstellungen", "Schließen", null, new string[] { "Bilanzprofil auswählen", "Neues Bilanzprofil erstellen", "Bilanzprofil löschen", reorder_string });

                    if (result0 == null)
                    {
                        indikator = true;

                        return;
                    }

                    if (result0 == "Schließen")
                    {
                        indikator = true;

                        return;
                    }

                    if (result0 == "Neues Bilanzprofil erstellen")
                    {
                        await Create_Balanceprofile();
                    }

                    if (result0 == "Bilanzprofil auswählen")
                    {
                        await Choose_Balanceprofile_Methode();

                        await Load();
                    }

                    if (result0 == "Bilanzprofil löschen")
                    {
                        await Delete_Balanceprofile_Methode();

                        await Load();
                    }

                    if (result0 == "Neuordnung des Bilanzprofiles " + Preferences.Get("Blanceprofile", 0) + "")
                    {
                        await Reorder_Balanceprofile_Methode();

                        await Load();
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async Task Choose_Balanceprofile_Methode()
        {
            try
            {
                await Get_BalanceprofileList();

                if(Bilanceprofiles_List.Count() == 0)
                {
                    await Create_Balanceprofile();

                    return;
                }

                string[] balanceprofile_list_string = new string[] { };

                List<string> balanceprofile_list = new List<string>();

                foreach (Balanceprofile bp in Bilanceprofiles_List)
                {
                    balanceprofile_list.Add("Bilanzprofil "+bp.Id+"");
                }

                balanceprofile_list_string = balanceprofile_list.ToArray();

                bool indikator = false;

                Balanceprofile CurrentBilanceprofil = Bilanceprofile;

                string current_balanceprofile_string = "Bilanzprofil auswählen\nAktuell : Bilanzprofil " + Bilanceprofile.Id + "";

                if(CurrentBilanceprofil == null)
                {
                    current_balanceprofile_string = "Bilanzprofil auswählen\nAktuell kein Bilanzprofil ausgewählt";
                }

                while (indikator == false)
                {
                    var result = await Shell.Current.DisplayActionSheet(current_balanceprofile_string, "Zurück", null, balanceprofile_list_string);

                    if (result == null)
                    {
                        if (Bilanceprofile != null)
                        {
                            return;
                        }
                    }
                    if (result == "Zurück")
                    {
                        if(Bilanceprofile != null)
                        {
                            indikator = true;
                        }
                    }
                    else
                    {
                        int id = int.Parse(result.ToString().Substring(12));

                        Balanceprofile ChoosenBilanceprofil = await BalanceService.Get_specific_Balanceprofile(id);


                        string reason = "";

                        List<string> sortetinitreasons_substring = new List<string>();

                        List<Stackholderhelper> sortetinitreasons = new List<Stackholderhelper>();

                        foreach (string re in ChoosenBilanceprofil.Outcome_Account)
                        {
                            sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Ausgaben Konto" });
                        }

                        foreach (string re in ChoosenBilanceprofil.Income_Account)
                        {
                            sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Einnahmen Konto" });
                        }

                        foreach (string re in ChoosenBilanceprofil.Outcome_Cash)
                        {
                            sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Barausgaben" });
                        }

                        foreach (string re in ChoosenBilanceprofil.Income_Cash)
                        {
                            sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Bareinnahmen" });
                        }

                        foreach (string re in ChoosenBilanceprofil.Letter_Outcome)
                        {
                            sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Ausgaben Briefumschlag" });
                        }

                        foreach (string re in ChoosenBilanceprofil.Letter_Income)
                        {
                            sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Einnahmen Briefumschlag" });
                        }

                        foreach (string re in ChoosenBilanceprofil.Ignore)
                        {
                            sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "ignorieren" });
                        }

                        foreach (Stackholderhelper sh in sortetinitreasons)
                        {
                            reason += sh.Substring+"\n\n";
                        }

                        var result1 = await Shell.Current.DisplayAlert("Bilanzprofil "+ ChoosenBilanceprofil.Id+"",reason, "Auswählen","Zurück");

                        Bilanceprofile = ChoosenBilanceprofil;

                        if(result1 == true)
                        {
                            Preferences.Set("Blanceprofile", ChoosenBilanceprofil.Id);

                            indikator = true;
                        }
                        else
                        {
                            indikator = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async Task Create_Balanceprofile()
        {
            try
            {
                Balanceprofile ChoosenBilanceprofil = new Balanceprofile();

                var zwecklist = await ReasonService.Get_all_Reason();

                foreach (Zweck reason in zwecklist)
                {
                    bool indikator = false;

                    while (indikator == false)
                    {
                        var result = await Shell.Current.DisplayActionSheet("" + reason.Benutzerdefinierter_Zweck + " zuordnen","Verwerfen", null, new string[] { "Ausgaben Konto", "Einnahmen Konto", "Barausgaben", "Bareinnahmen", "Ausgaben Briefumschlag", "Einnahmen Briefumschlag", "ignorieren" });

                        if (result == "Ausgaben Konto")
                        {
                            ChoosenBilanceprofil.Outcome_Account.Add(reason.Benutzerdefinierter_Zweck);

                            indikator = true;
                        }

                        if (result == "Einnahmen Konto")
                        {
                            ChoosenBilanceprofil.Income_Account.Add(reason.Benutzerdefinierter_Zweck);

                            indikator = true;
                        }

                        if (result == "Barausgaben")
                        {
                            ChoosenBilanceprofil.Outcome_Cash.Add(reason.Benutzerdefinierter_Zweck);

                            indikator = true;
                        }

                        if (result == "Bareinnahmen")
                        {
                            ChoosenBilanceprofil.Income_Cash.Add(reason.Benutzerdefinierter_Zweck);

                            indikator = true;
                        }

                        if (result == "Ausgaben Briefumschlag")
                        {
                            ChoosenBilanceprofil.Letter_Outcome.Add(reason.Benutzerdefinierter_Zweck);

                            indikator = true;
                        }

                        if (result == "Einnahmen Briefumschlag")
                        {
                            ChoosenBilanceprofil.Letter_Income.Add(reason.Benutzerdefinierter_Zweck);

                            indikator = true;
                        }

                        if (result == "ignorieren")
                        {
                            ChoosenBilanceprofil.Ignore.Add(reason.Benutzerdefinierter_Zweck);

                            indikator = true;
                        }

                        if(result == "Verwerfen")
                        {
                            return;
                        }
                    }

                }

                await BalanceService.Add_Balanceprofile(ChoosenBilanceprofil);


                await Notificater("Das Bilanzprofil " + ChoosenBilanceprofil.Id + " wurde erfolgreich gespeichert");

                await Get_BalanceprofileList();

                if (Bilanceprofiles_List.Count() == 1)
                {
                    Bilanceprofile = ChoosenBilanceprofil;

                    Preferences.Set("Blanceprofile", Bilanceprofile.Id);
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async Task Delete_Balanceprofile_Methode()
        {
            try
            {
                if (Bilanceprofile != null)
                {
                    await Get_BalanceprofileList();

                    if (Bilanceprofiles_List.Count() == 0)
                    {
                        await Create_Balanceprofile();

                        return;
                    }

                    string[] balanceprofile_list_string = new string[] { };

                    List<string> balanceprofile_list = new List<string>();

                    foreach (Balanceprofile bp in Bilanceprofiles_List)
                    {
                        balanceprofile_list.Add("Bilanzprofil " + bp.Id);
                    }

                    balanceprofile_list_string = balanceprofile_list.ToArray();

                    bool indikator = false;

                    Balanceprofile CurrentBilanceprofil = Bilanceprofile;

                    while (indikator == false)
                    {
                        var result1 = await Shell.Current.DisplayActionSheet("Bilanzprofil löschen\nAktuell : Bilanzprofil "+Bilanceprofile.Id+"", "Zurück", null, balanceprofile_list_string);

                        if (result1 == null)
                        {
                            return;
                        }
                        if (result1 == "Zurück")
                        {
                            indikator = true;
                        }
                        else
                        {
                            int id = int.Parse(result1.ToString().Substring(12));

                            Balanceprofile ChoosenBilanceprofil = await BalanceService.Get_specific_Balanceprofile(id);

                            string reason = "";

                            List<string> sortetinitreasons_substring = new List<string>();

                            List<Stackholderhelper> sortetinitreasons = new List<Stackholderhelper>();

                            foreach (string re in ChoosenBilanceprofil.Outcome_Account)
                            {
                                sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Ausgaben Konto" });
                            }

                            foreach (string re in ChoosenBilanceprofil.Income_Account)
                            {
                                sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Einnahmen Konto" });
                            }

                            foreach (string re in ChoosenBilanceprofil.Outcome_Cash)
                            {
                                sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Barausgaben" });
                            }

                            foreach (string re in ChoosenBilanceprofil.Income_Cash)
                            {
                                sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Bareinnahmen" });
                            }

                            foreach (string re in ChoosenBilanceprofil.Letter_Outcome)
                            {
                                sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Ausgaben Briefumschlag" });
                            }

                            foreach (string re in ChoosenBilanceprofil.Letter_Income)
                            {
                                sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Einnahmen Briefumschlag" });
                            }

                            foreach (string re in ChoosenBilanceprofil.Ignore)
                            {
                                sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "ignorieren" });
                            }

                            foreach (Stackholderhelper sh in sortetinitreasons)
                            {
                                reason += sh.Substring + "\n\n";
                            }

                            var result2 = await Shell.Current.DisplayAlert("Bilanzprofil " + ChoosenBilanceprofil.Id + "", reason,"Löschen","Zurück");

                            Bilanceprofile = ChoosenBilanceprofil;

                            if (result2 == true)
                            {
                                var result = await BalanceService.Remove_Balanceprofile(Bilanceprofile.Id);

                                if (result == true)
                                {
                                    await Notificater("Das Bilanzprofil " + Bilanceprofile.Id + " wurde erfolgreich gelöscht");

                                    Bilanceprofile = null;
                                }
                                else
                                {
                                    await Notificater("Es ist beim löschen des Bilanzprofiles " + Bilanceprofile.Id + " ist ein Fehler aufgetretten.");
                                }

                                await Get_BalanceprofileList();

                                if (Bilanceprofiles_List.Count() == 0)
                                {
                                    await Create_Balanceprofile();
                                }
                                else
                                {
                                    balanceprofile_list_string = null;

                                    balanceprofile_list.Clear();

                                    foreach (Balanceprofile bp in Bilanceprofiles_List)
                                    {
                                        balanceprofile_list.Add("Bilanzprofil " + bp.Id);
                                    }

                                    balanceprofile_list_string = balanceprofile_list.ToArray();
                                }

                                if (CurrentBilanceprofil != null)
                                {
                                    if (id != CurrentBilanceprofil.Id)
                                    {
                                        Bilanceprofile = CurrentBilanceprofil;
                                    }
                                    else
                                    {
                                        await Choose_Balanceprofile_Methode();

                                        return;
                                    }
                                }

                                indikator = false;
                            }
                            else
                            {
                                indikator = false;
                            }
                        }
                    }
                }
                else
                {
                    await Notificater("Es wurde kein Bilanzprofil gefunden was gelöscht werden kann");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        private async Task Reorder_Balanceprofile_Methode()
        {
            try
            {
                var result0 = await Check_for_existing_Balanceprofile();

                if (result0 == -1)
                {
                    return;
                }
                if(result0 == 2)
                {
                    return;
                }

                List<Stackholderhelper> sortetinitreasons = new List<Stackholderhelper>();

                foreach (string re in Bilanceprofile.Outcome_Account)
                {
                    sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Ausgaben Konto" });
                }

                foreach (string re in Bilanceprofile.Income_Account)
                {
                    sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Einnahmen Konto" });
                }

                foreach (string re in Bilanceprofile.Outcome_Cash)
                {
                    sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Barausgaben" });
                }

                foreach (string re in Bilanceprofile.Income_Cash)
                {
                    sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Bareinnahmen" });
                }

                foreach (string re in Bilanceprofile.Letter_Outcome)
                {
                    sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Ausgaben Briefumschlag" });
                }

                foreach (string re in Bilanceprofile.Letter_Income)
                {
                    sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "Einnahmen Briefumschlag" });
                }

                foreach (string re in Bilanceprofile.Ignore)
                {
                    sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "ignorieren" });
                }

                Balanceprofile newbalanceprofile = new Balanceprofile();

                if (sortetinitreasons.Count() != 0)
                {
                    bool indikator = false;

                    string[] reason = new string[] { };

                    List<string> sortetinitreasons_substring = new List<string>();

                    foreach (Stackholderhelper sh in sortetinitreasons)
                    {
                        sortetinitreasons_substring.Add(sh.Substring);
                    }

                    reason = sortetinitreasons_substring.ToArray();

                    while (indikator == false)
                    {
                        var result1 = await Shell.Current.DisplayActionSheet("Zweckzuordnung bearbeiten", "Speichern", null, reason);

                        if (result1 == null)
                        {
                            return;
                        }
                        if (result1 == "Speichern")
                        {
                            indikator = true;

                            await BalanceService.Edit_Balanceprofile(newbalanceprofile);
                        }
                        else
                        {
                            var result = await Shell.Current.DisplayActionSheet("" + result1 + " neu zuordnen", "Zurück", null, new string[] { "Ausgaben Konto", "Einnahmen Konto", "Barausgaben", "Bareinnahmen", "Ausgaben Briefumschlag", "Einnahmen Briefumschlag", "ignorieren" });

                            if (result == null)
                            {
                                continue;
                            }

                            if (result == "Ausgaben Konto")
                            {

                                sortetinitreasons[sortetinitreasons_substring.IndexOf(result1)].Option = "Ausgaben Konto";
                            }

                            if (result == "Einnahmen Konto")
                            {
                                sortetinitreasons[sortetinitreasons_substring.IndexOf(result1)].Option = "Einnahmen Konto";
                            }

                            if (result == "Barausgaben")
                            {
                                sortetinitreasons[sortetinitreasons_substring.IndexOf(result1)].Option = "Barausgaben";
                            }

                            if (result == "Bareinnahmen")
                            {
                                sortetinitreasons[sortetinitreasons_substring.IndexOf(result1)].Option = "Bareinnahmen";
                            }

                            if (result == "Ausgaben Briefumschlag")
                            {
                                sortetinitreasons[sortetinitreasons_substring.IndexOf(result1)].Option = "Ausgaben Briefumschlag";
                            }

                            if (result == "Einnahmen Briefumschlag")
                            {
                                sortetinitreasons[sortetinitreasons_substring.IndexOf(result1)].Option = "Einnahmen Briefumschlag";
                            }

                            if (result == "ignorieren")
                            {
                                sortetinitreasons[sortetinitreasons_substring.IndexOf(result1)].Option = "ignorieren";
                            }

                            sortetinitreasons_substring.Clear();

                            foreach (Stackholderhelper sh in sortetinitreasons)
                            {
                                sortetinitreasons_substring.Add(sh.Substring);
                            }

                            reason = null;

                            reason = sortetinitreasons_substring.ToArray();
                        }
                    }

                    newbalanceprofile.Id = Bilanceprofile.Id;

                    foreach (Stackholderhelper sh in sortetinitreasons)
                    {
                        if (sh.Option == "Ausgaben Konto")
                        {
                            newbalanceprofile.Outcome_Account.Add(sh.Reason);
                        }

                        if (sh.Option == "Einnahmen Konto")
                        {
                            newbalanceprofile.Income_Account.Add(sh.Reason);
                        }

                        if (sh.Option == "Barausgaben")
                        {
                            newbalanceprofile.Outcome_Cash.Add(sh.Reason);
                        }

                        if (sh.Option == "Bareinnahmen")
                        {
                            newbalanceprofile.Income_Cash.Add(sh.Reason);
                        }

                        if (sh.Option == "ignorieren")
                        {
                            newbalanceprofile.Ignore.Add(sh.Reason);
                        }
                    }

                    Bilanceprofile = newbalanceprofile;

                    await BalanceService.Edit_Balanceprofile(Bilanceprofile);

                    await Notificater("Das Bilanzprofil " + Preferences.Get("Blanceprofile", 0) + " wurde erfolgreich gespeichert");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");
            }
        }

        public async Task Period_Popup()
        {
            try
            {
                var result = await Shell.Current.ShowPopupAsync(new Viewtime_Popup(Current_Viewtime));

                if (result == null)
                {
                    return;
                }

                Current_Viewtime = (Viewtime)result;

                await Load();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + "", "Verstanden");

                Current_Viewtime = new Viewtime() { Year = DateTime.Now.Year, Month = DateTime.Now.ToString("MMMM", new CultureInfo("de-DE")) };

                await Load();
            }
        }

        private async Task Notificater(string v)
        {
            await Shell.Current.DisplayToastAsync(v, 5000);
        }

        public ObservableRangeCollection<StackholderBundle> Bundles { get; set; }


        public ObservableRangeCollection<Stackholder> stackholderbundel_outcome_account;
        public ObservableRangeCollection<Stackholder> Stackholderbundel_Outcome_Account
        {
            get { return stackholderbundel_outcome_account; }
            set
            {
                if (stackholderbundel_outcome_account == value)
                {
                    return;
                }
                stackholderbundel_outcome_account = value; RaisePropertyChanged();
            }
        }

        public ObservableRangeCollection<Stackholder> stackholderbundel_income_account;
        public ObservableRangeCollection<Stackholder> Stackholderbundel_Income_Account
        {
            get { return stackholderbundel_income_account; }
            set
            {
                if (stackholderbundel_income_account == value)
                {
                    return;
                }
                stackholderbundel_income_account = value; RaisePropertyChanged();
            }
        }

        public ObservableRangeCollection<Stackholder> stackholderbundel_outcome_cash;
        public ObservableRangeCollection<Stackholder> Stackholderbundel_Outcome_Cash
        {
            get { return stackholderbundel_outcome_cash; }
            set
            {
                if (stackholderbundel_outcome_cash == value)
                {
                    return;
                }
                stackholderbundel_outcome_cash = value; RaisePropertyChanged();
            }
        }

        public ObservableRangeCollection<Stackholder> stackholderbundel_income_cash;
        public ObservableRangeCollection<Stackholder> Stackholderbundel_Income_Cash
        {
            get { return stackholderbundel_income_cash; }
            set
            {
                if (stackholderbundel_income_cash == value)
                {
                    return;
                }
                stackholderbundel_income_cash = value; RaisePropertyChanged();
            }
        }

        public Balanceprofile balanceprofile;
        public Balanceprofile Bilanceprofile
        {
            get { return balanceprofile; }
            set
            {
                if (balanceprofile == value)
                {
                    return;
                }
                balanceprofile = value; RaisePropertyChanged();
            }
        }

        public List<Balanceprofile> Bilanceprofiles_List = new List<Balanceprofile>();

        public bool stackholder_bundle_visibility = false;
        public bool Stackholder_Bundle_Visibility
        {
            get { return stackholder_bundle_visibility; }
            set
            {

                if (stackholder_bundle_visibility == value)
                {
                    return;
                }

                stackholder_bundle_visibility = value; RaisePropertyChanged();
            }
        }

        public bool stackholder_outcome_account_visibility = false;
        public bool Stackholder_Outcome_Account_Visibility
        {
            get { return stackholder_outcome_account_visibility; }
            set
            {

                if (stackholder_outcome_account_visibility == value)
                {
                    return;
                }

                stackholder_outcome_account_visibility = value; RaisePropertyChanged();
            }
        }

        public bool stackholder_income_account_visibility = false;
        public bool Stackholder_Income_Account_Visibility
        {
            get { return stackholder_income_account_visibility; }
            set
            {

                if (stackholder_income_account_visibility == value)
                {
                    return;
                }

                stackholder_income_account_visibility = value; RaisePropertyChanged();
            }
        }

        public bool stackholder_outcome_cash_visibility = false;
        public bool Stackholder_Outcome_Cash_Visibility
        {
            get { return stackholder_outcome_cash_visibility; }
            set
            {

                if (stackholder_outcome_cash_visibility == value)
                {
                    return;
                }

                stackholder_outcome_cash_visibility = value; RaisePropertyChanged();
            }
        }

        public bool stackholder_income_cash_visibility = false;
        public bool Stackholder_Income_Cash_Visibility
        {
            get { return stackholder_income_cash_visibility; }
            set
            {

                if (stackholder_income_cash_visibility == value)
                {
                    return;
                }

                stackholder_income_cash_visibility = value; RaisePropertyChanged();
            }
        }

        public bool kein_ergebnis_stackholder_Status = false;
        public bool Kein_Ergebnis_Stackholder_Status
        {
            get { return kein_ergebnis_stackholder_Status; }
            set
            {
                if (Kein_Ergebnis_Stackholder_Status == value)
                    return;
                kein_ergebnis_stackholder_Status = value; RaisePropertyChanged();
                if (kein_ergebnis_stackholder_Status == true)
                {
                    Title = "Bilanz";
                }
            }
        }

        public string title = "Bilanz";
        public string Title
        {
            get { return title; }
            set
            {
                if (title == value)
                {
                    return;
                }

                title = value; RaisePropertyChanged();
            }
        }

        public Viewtime current_viewtime = new Viewtime() { Year = DateTime.Now.Year, Month = DateTime.Now.ToString("MMMM", new CultureInfo("de-DE")) };
        public Viewtime Current_Viewtime
        {
            get { return current_viewtime; }
            set
            {
                if (current_viewtime == value)
                {
                    return;
                }

                current_viewtime = value; RaisePropertyChanged();
            }
        }

        public AsyncCommand Load_Command { get; }

        public AsyncCommand Period_Command { get; }

        public AsyncCommand Settings_Command { get; }
        public AsyncCommand Create_PDF_Command { get; }

        public Xamarin.Forms.Color evaluating_of_totoal_outcome_account;
        public Xamarin.Forms.Color Evaluating_of_Totoal_Outcome_Account
        {
            get { return evaluating_of_totoal_outcome_account; }
            set
            {
                if (evaluating_of_totoal_outcome_account == value)
                {
                    return;
                }

                evaluating_of_totoal_outcome_account = value; RaisePropertyChanged();
            }
        }

        public Xamarin.Forms.Color evaluating_of_totoal_income_account;
        public Xamarin.Forms.Color Evaluating_of_Totoal_Income_Account
        {
            get { return evaluating_of_totoal_income_account; }
            set
            {
                if (evaluating_of_totoal_income_account == value)
                {
                    return;
                }

                evaluating_of_totoal_income_account = value; RaisePropertyChanged();
            }
        }

        public Xamarin.Forms.Color evaluating_of_totoal_outcome_cash;
        public Xamarin.Forms.Color Evaluating_of_Totoal_Outcome_Cash
        {
            get { return evaluating_of_totoal_outcome_cash; }
            set
            {
                if (evaluating_of_totoal_outcome_cash == value)
                {
                    return;
                }

                evaluating_of_totoal_outcome_cash = value; RaisePropertyChanged();
            }
        }

        public Xamarin.Forms.Color evaluating_of_totoal_income_cash;
        public Xamarin.Forms.Color Evaluating_of_Totoal_Income_Cash
        {
            get { return evaluating_of_totoal_income_cash; }
            set
            {
                if (evaluating_of_totoal_income_cash == value)
                {
                    return;
                }

                evaluating_of_totoal_income_cash = value; RaisePropertyChanged();
            }
        }

        public Xamarin.Forms.Color evaluating_of_totoal;
        public Xamarin.Forms.Color Evaluating_of_Totoal
        {
            get { return evaluating_of_totoal; }
            set
            {
                if (evaluating_of_totoal == value)
                {
                    return;
                }

                evaluating_of_totoal = value; RaisePropertyChanged();
            }
        }

        public string total_outcome_accaount;
        public string Total_Outcome_Account
        {
            get { return total_outcome_accaount; }
            set
            {
                if (total_outcome_accaount == value)
                {
                    return;
                }

                total_outcome_accaount = value; RaisePropertyChanged();
            }
        }

        public string total_income_accaount;
        public string Total_Income_Account
        {
            get { return total_income_accaount; }
            set
            {
                if (total_income_accaount == value)
                {
                    return;
                }

                total_income_accaount = value; RaisePropertyChanged();
            }
        }

        public string total_outcome_cash;
        public string Total_Outcome_Cash
        {
            get { return total_outcome_cash; }
            set
            {
                if (total_outcome_cash == value)
                {
                    return;
                }

                total_outcome_cash = value; RaisePropertyChanged();
            }
        }

        public string total_income_cash;
        public string Total_Income_Cash
        {
            get { return total_income_cash; }
            set
            {
                if (total_income_cash == value)
                {
                    return;
                }

                total_income_cash = value; RaisePropertyChanged();
            }
        }

        public string total;
        public string Total
        {
            get { return total; }
            set
            {
                if (total == value)
                {
                    return;
                }

                total = value; RaisePropertyChanged();
            }
        }
    }

    public class Stackholderhelper
    {
        public string Reason { get; set; }

        public string Option { get; set; }

        public string substring;
        public string Substring 
        {
            get 
            {
                return substring = "" + Reason + " als " + Option + "";
            }

            set
            {
                 substring = value;
            }
        }

    }
}
