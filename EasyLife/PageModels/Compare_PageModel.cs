using EasyLife.Interfaces;
using EasyLife.Models;
using EasyLife.Pages;
using EasyLife.Services;
using FreshMvvm;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Pdf;
using iText.Layout.Borders;
using iText.Layout.Element;
using Microcharts;
using MvvmHelpers.Commands;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using MvvmHelpers;
using System.Transactions;
using CarouselView.FormsPlugin.Abstractions;
using static iText.Svg.SvgConstants;

namespace EasyLife.PageModels
{
    class Compare_PageModel : FreshBasePageModel
    {
        public Compare_PageModel()
        {
            Load_Command = new AsyncCommand(Load);
            Set_Item1_Command = new AsyncCommand(Set_Item1_Methode);
            Set_Item2_Command = new AsyncCommand(Set_Item2_Methode);
            Settings_Command = new AsyncCommand(Settings_Methode);
            Switch_Diff_Command = new AsyncCommand(Switch_Diff_Methode);
            Detail_Command = new AsyncCommand<int>(Detail_Methode);
        }

        public async Task Load()
        {
            try
            {
                int result0 = await Check_for_existing_Balanceprofile();

                if (result0 == 0)
                {
                    await Add_to_Balanceprofile();
                }
                if (result0 == -1)
                {
                    return;
                }
                if (result0 == 1)
                {
                    await Add_to_Balanceprofile();
                }
                if (result0 == 2)
                {
                    await Create_Balanceprofile();
                }

                var transaktionscontent = await ContentService.Get_all_enabeled_Transaktion();

                List<Transaktion> transaktions = new List<Transaktion>();

                foreach (Transaktion transaction in transaktionscontent)
                {
                    if (transaction != null)
                    {
                        if(transaction.Balance_Visibility == true)
                        {
                            transaktions.Add(transaction);
                        }
                    }
                }

                Transaktion_List = transaktions;

                Haushaltsbucher = new Haushaltsbücher(Transaktion_List);

                if(result1.Count() != 0)
                {
                    await LoadItem1();
                }

                if(result2.Count() != 0)
                {
                    await LoadItem2();
                }

            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public async Task LoadCompare()
        {
            try
            {
                if(Bilanceprofile == null)
                {
                    return;
                }

                if(Item1_Viewtime != null && Item2_Viewtime != null)
                {
                    if(result1.Count() != 0 && result2.Count() != 0)
                    {
                        await LoadDiff(result1,result2);
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public async Task<List<HelperComparer>> LoadItem1()
        {
            try
            {
                List<Stackholder> stackholderList = new List<Stackholder>();

                List<Transaktion> sorted_after_month_transaktionscontent = new List<Transaktion>();

                foreach (Transaktion trans in Transaktion_List)
                {
                    if (trans.Balance_Visibility == true)
                    {
                        if (trans.Datum.Year == Item1_Viewtime.Year)
                        {
                            if (trans.Datum.ToString("MMMM", new CultureInfo("de-DE")) == Item1_Viewtime.Month)
                            {
                                sorted_after_month_transaktionscontent.Add(trans);
                            }
                            if (Item1_Viewtime.Month == "")
                            {
                                sorted_after_month_transaktionscontent.Add(trans);
                            }
                        }
                    }
                }

                List<HelperComparer> output = await GetItem(sorted_after_month_transaktionscontent);

                output.Add( new HelperComparer { Value = output[0].Value + output[1].Value, ButonValueList = new List<string>() });

                output.Add(new HelperComparer { Value = output[2].Value + output[3].Value, ButonValueList = new List<string>() });

                Item1_Outcome_Account = Math.Round(output[0].Value, 2).ToString().Replace(".", ",")+" €";

                Item1_Income_Account = Math.Round(output[1].Value, 2).ToString().Replace(".", ",") + " €";

                Item1_Account_Result = Math.Round(output[5].Value, 2).ToString().Replace(".", ",") + " €";

                Item1_Outcome_Cash = Math.Round(output[2].Value, 2).ToString().Replace(".", ",") + " €";

                Item1_Income_Cash = Math.Round(output[3].Value, 2).ToString().Replace(".", ",") + " €";

                Item1_Cash_Result = Math.Round(output[6].Value, 2).ToString().Replace(".", ",") + " €";

                Item1_Result = Math.Round(output[4].Value, 2).ToString().Replace(".", ",") + " €";

                return output;
            }
            catch (Exception ex)
            {
                return null;

                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public async Task<List<HelperComparer>> LoadItem2()
        {
            try
            {
                List<Stackholder> stackholderList = new List<Stackholder>();

                List<Transaktion> sorted_after_month_transaktionscontent = new List<Transaktion>();

                foreach (Transaktion trans in Transaktion_List)
                {
                    if (trans.Balance_Visibility == true)
                    {
                        if (trans.Datum.Year == Item2_Viewtime.Year)
                        {
                            if (trans.Datum.ToString("MMMM", new CultureInfo("de-DE")) == Item2_Viewtime.Month)
                            {
                                sorted_after_month_transaktionscontent.Add(trans);
                            }
                            if (Item2_Viewtime.Month == "")
                            {
                                sorted_after_month_transaktionscontent.Add(trans);
                            }
                        }
                    }
                }

                List<HelperComparer> output = await GetItem(sorted_after_month_transaktionscontent);

                output.Add(new HelperComparer { Value = output[0].Value + output[1].Value, ButonValueList = new List<string>() });

                output.Add(new HelperComparer { Value = output[2].Value + output[3].Value, ButonValueList = new List<string>() });

                Item2_Outcome_Account = Math.Round(output[0].Value, 2).ToString().Replace(".", ",") + " €";

                Item2_Income_Account = Math.Round(output[1].Value, 2).ToString().Replace(".", ",") + " €";

                Item2_Account_Result = Math.Round(output[5].Value, 2).ToString().Replace(".", ",") + " €";

                Item2_Outcome_Cash = Math.Round(output[2].Value, 2).ToString().Replace(".", ",") + " €";

                Item2_Income_Cash = Math.Round(output[3].Value, 2).ToString().Replace(".", ",") + " €";

                Item2_Cash_Result = Math.Round(output[6].Value, 2).ToString().Replace(".", ",") + " €";

                Item2_Result = Math.Round(output[4].Value, 2).ToString().Replace(".", ",") + " €";

                return output;
            }
            catch (Exception ex)
            {
                return null;

                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public async Task LoadDiff(List<HelperComparer> output1 , List<HelperComparer> output2)
        {
            try
            {
                if (Math.Round(output1[0].Value - output2[0].Value, 2) < 0)
                {
                    Diff_Outcome_Account_Color = Color.Red;
                }
                else
                {
                    if (Math.Round(output1[0].Value - output2[0].Value, 2) == 0)
                    {
                        Diff_Outcome_Account_Color = Color.Gray;
                    }
                    else
                    {
                        Diff_Outcome_Account_Color = Color.ForestGreen;
                    }
                }



                if (Math.Round(output1[1].Value - output2[1].Value, 2) < 0)
                {
                    Diff_Income_Account_Color = Color.Red;
                }
                else
                {
                    if (Math.Round(output1[1].Value - output2[1].Value, 2) == 0)
                    {
                        Diff_Income_Account_Color = Color.Gray;
                    }
                    else
                    {
                        Diff_Income_Account_Color = Color.ForestGreen;
                    }
                }



                if (Math.Round(output1[2].Value - output2[2].Value, 2) < 0)
                {
                    Diff_Outcome_Cash_Color = Color.Red;
                }
                else
                {
                    if (Math.Round(output1[2].Value - output2[2].Value, 2) == 0)
                    {
                        Diff_Outcome_Cash_Color = Color.Gray;
                    }
                    else
                    {
                        Diff_Outcome_Cash_Color = Color.ForestGreen;
                    }
                }



                if (Math.Round(output1[3].Value - output2[3].Value, 2) < 0)
                {
                    Diff_Income_Cash_Color = Color.Red;
                }
                else
                {
                    if (Math.Round( output1[3].Value - output2[3].Value, 2) == 0)
                    {
                        Diff_Income_Cash_Color = Color.Gray;
                    }
                    else
                    {
                        Diff_Income_Cash_Color = Color.ForestGreen;
                    }
                }



                if (Math.Round(output1[4].Value - output2[4].Value, 2) < 0)
                {
                    Diff_Result_Color = Color.Red;
                }
                else
                {
                    if (Math.Round(output1[4].Value - output2[4].Value, 2) == 0)
                    {
                        Diff_Result_Color = Color.Gray;
                    }
                    else
                    {
                        Diff_Result_Color = Color.ForestGreen;
                    }
                }

                if (Math.Round(output1[5].Value - output2[5].Value, 2) < 0)
                {
                    Diff_Account_Result_Color = Color.Red;
                }
                else
                {
                    if (Math.Round(output1[5].Value - output2[5].Value, 2) == 0)
                    {
                        Diff_Account_Result_Color = Color.Gray;
                    }
                    else
                    {
                        Diff_Account_Result_Color = Color.ForestGreen;
                    }
                }

                if (Math.Round(output1[6].Value - output2[6].Value, 2) < 0)
                {
                    Diff_Cash_Result_Color = Color.Red;
                }
                else
                {
                    if (Math.Round(output1[6].Value - output2[6].Value, 2) == 0)
                    {
                        Diff_Cash_Result_Color = Color.Gray;
                    }
                    else
                    {
                        Diff_Cash_Result_Color = Color.ForestGreen;
                    }
                }


                Diff_Outcome_Account = Math.Round(output1[0].Value - output2[0].Value, 2).ToString().Replace(".", ",") + " €";

                Diff_Income_Account = Math.Round(output1[1].Value - output2[1].Value, 2).ToString().Replace(".", ",") + " €";

                Diff_Account_Result = Math.Round(output1[5].Value - output2[5].Value, 2).ToString().Replace(".", ",") + " €";

                Diff_Outcome_Cash = Math.Round(output1[2].Value - output2[2].Value, 2).ToString().Replace(".", ",") + " €";

                Diff_Income_Cash = Math.Round(output1[3].Value - output2[3].Value, 2).ToString().Replace(".", ",") + " €";

                Diff_Cash_Result = Math.Round(output1[6].Value - output2[6].Value, 2).ToString().Replace(".", ",") + " €";

                Diff_Result = Math.Round(output1[4].Value - output2[4].Value, 2).ToString().Replace(".", ",") + " €";
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public async Task<List<HelperComparer>> GetItem(List<Transaktion> transaktionslist)
        {
            try
            {
                List<HelperComparer> values = new List<HelperComparer>()
                {
                    new HelperComparer(){Value = 0, ButonValueList = new List<string>(), ButonReasonList = new List<string>()},
                    new HelperComparer(){Value = 0, ButonValueList = new List<string>(), ButonReasonList = new List<string>()},
                    new HelperComparer(){Value = 0, ButonValueList = new List<string>(), ButonReasonList = new List<string>()},
                    new HelperComparer(){Value = 0, ButonValueList = new List<string>(), ButonReasonList = new List<string>()},
                };

                List<List<string>> balanceprofile = new List<List<string>>
                {
                    Bilanceprofile.Outcome_Account,
                    Bilanceprofile.Income_Account,
                    Bilanceprofile.Outcome_Cash,
                    Bilanceprofile.Income_Cash
                };

                int h = 0;

                foreach (var placeholder in balanceprofile)
                {
                    List<string> reasons = new List<string>();

                    foreach (var group in placeholder)
                    {
                        reasons.Add(group.Substring(0, group.IndexOf(":")));
                    }

                    reasons.Sort();

                    double total_value1 = 0;

                    foreach (string st in reasons)
                    {
                        double reasonvalue = 0;

                        if (transaktionslist.Where(ts => ts.Zweck == st).Count() != 0)
                        {
                            double value = 0;

                            foreach (Transaktion trans in transaktionslist)
                            {
                                if (trans.Zweck == st)
                                {
                                    value += double.Parse(trans.Betrag, NumberStyles.Any, new CultureInfo("de-DE"));
                                }
                            }
                            reasonvalue = value;

                            total_value1 += value;
                        }

                        values[h].ButonReasonList.Add(st);
                        values[h].ButonValueList.Add("" + reasonvalue + " €");
                    }

                    values[h].Value +=total_value1 ;

                    h++;
                }

                double totalvalue1 = 0;

                foreach(var val in values)
                { 
                    totalvalue1 += val.Value ;
                }


                values.Add(new HelperComparer() { Value = totalvalue1});

                return values;
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));

                return null;
            }
        }

        public async Task Set_Item1_Methode()
        {
            try
            {
                var result = await Shell.Current.ShowPopupAsync(new Viewtime_Popup(Item1_Viewtime,Haushaltsbucher));

                if (result == null)
                {
                    return;
                }

                Item1_Viewtime = (Viewtime)result;

                if(Item1_Viewtime.Month == "")
                {
                    Item1_Name = ""+Item1_Viewtime.Year;
                }
                else
                {
                    Item1_Name = Item1_Viewtime.Month.ToString() + "\n" + Item1_Viewtime.Year;
                }

                result1 = await LoadItem1();

                await LoadCompare();
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));

                Item1_Viewtime = new Viewtime() { Year = DateTime.Now.Year, Month = DateTime.Now.ToString("MMMM", new CultureInfo("de-DE")) };

                await LoadCompare();
            }
        }

        public async Task Set_Item2_Methode()
        {
            try
            {
                var result = await Shell.Current.ShowPopupAsync(new Viewtime_Popup(Item2_Viewtime, Haushaltsbucher));

                if (result == null)
                {
                    return;
                }

                Item2_Viewtime = (Viewtime)result;

                if (Item2_Viewtime.Month == "")
                {
                    Item2_Name = "" + Item2_Viewtime.Year;
                }
                else
                {
                    Item2_Name = Item2_Viewtime.Month.ToString() + "\n" + Item2_Viewtime.Year;
                }

                result2 = await LoadItem2();

                await LoadCompare();
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));

                Item2_Viewtime = new Viewtime() { Year = DateTime.Now.Year, Month = DateTime.Now.ToString("MMMM", new CultureInfo("de-DE")) };

                await LoadCompare();
            }
        }

        public async Task Switch_Diff_Methode()
        {
            try
            {
                if (Bilanceprofile == null)
                {
                    return;
                }

                if (Item1_Viewtime != null && Item2_Viewtime != null)
                {

                    if (result1.Count() != 0 && result2.Count() != 0)
                    {
                        ISSwitched = !ISSwitched;

                        if (ISSwitched == false)
                        {
                            await LoadDiff(result1, result2);
                        }
                        else
                        {
                            await LoadDiff(result2, result1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public async Task Detail_Methode(int input)
        {
            try
            {
                if (result1.Count() != 0 || result2.Count() != 0)
                {
                    List<string> Titel = new List<string>() { "Ausgaben Konto" , "Einnahmen Konto" , "Barausgaben" , "Bareinnahmen" };

                    List<string> DetailList1 = new List<string>();

                    List<string> DetailList2 = new List<string>();

                    List<string[]> DetailList = new List<string[]> { };

                    int length = 13;

                    if(result1.Count() != 0)
                    {
                        try
                        {
                            DetailList1 = result1[input].ButonValueList;
                        }
                        catch
                        { return; }
                    }

                    if(result2.Count() != 0)
                    {
                        try
                        {
                            DetailList2 = result2[input].ButonValueList;
                        }
                        catch
                        { return; }
                    }

                    if(DetailList1.Count() < DetailList2.Count())
                    {
                        while(DetailList1.Count() < DetailList2.Count())
                        {
                            DetailList1.Add("0 €");
                        }

                        int count = 0;

                        foreach (string Detail in DetailList2)
                        {
                            int length1left = (int)Math.Round((length - Detail.Length)*0.5,0);

                            int length1right = (length - Detail.Length) - length1left;

                            string whitspaceleft1 = "";

                            string whitspaceright1 = "";

                            int i = 0;

                            while( i < length1left )
                            {
                                whitspaceleft1 += " ";

                                i++;
                            }

                            i = 0;

                            while (i < length1right)
                            {
                                whitspaceright1 += " ";

                                i++;
                            }

                            int length2left = (int)Math.Round((length - DetailList1[count].Length) * 0.5, 0);

                            int length2right = (length - DetailList1[count].Length) - length1left;

                            string whitspaceleft2 = "";

                            string whitspaceright2 = "";

                            i = 0;

                            while (i < length2left)
                            {
                                whitspaceleft2 += " ";

                                i++;
                            }

                            i = 0;

                            while (i < length2right)
                            {
                                whitspaceright2 += " ";

                                i++;
                            }

                            DetailList.Add(new string[] { result2[input].ButonReasonList[count], whitspaceleft1+Detail+whitspaceright1+ " | " +whitspaceright2+DetailList1[count]+whitspaceleft2 });

                            count++;
                        }
                    }
                    else
                    {
                        while (DetailList2.Count() < DetailList1.Count())
                        {
                            DetailList2.Add("0 €");
                        }

                        int count = 0;

                        foreach (string Detail in DetailList1)
                        {
                            int length1left = (int)Math.Round((length - Detail.Length) * 0.5, 0);

                            int length1right = (length - Detail.Length) - length1left;

                            string whitspaceleft1 = "";

                            string whitspaceright1 = "";

                            int i = 0;

                            while (i < length1left)
                            {
                                whitspaceleft1 += " ";

                                i++;
                            }

                            i = 0;

                            while (i < length1right)
                            {
                                whitspaceright1 += " ";

                                i++;
                            }

                            int length2left = (int)Math.Round((length - DetailList2[count].Length) * 0.5, 0);

                            int length2right = (length - DetailList2[count].Length) - length1left;

                            string whitspaceleft2 = "";

                            string whitspaceright2 = "";

                            i = 0;

                            while (i < length2left)
                            {
                                whitspaceleft2 += " ";

                                i++;
                            }

                            i = 0;

                            while (i < length2right)
                            {
                                whitspaceright2 += " ";

                                i++;
                            }

                            DetailList.Add(new string[] { result1[input].ButonReasonList[count], whitspaceleft1 + Detail + whitspaceright1 + " | " + whitspaceright2 + DetailList2[count] + whitspaceleft2 });

                            count++;
                        }
                    }

                    await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup(Titel[input],400,DetailList));
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
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
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        public async Task<int> Check_for_existing_Balanceprofile()
        {
            try
            {
                if (Bilanceprofile != null)
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

                    if (validate == false)
                    {
                        await Get_BalanceprofileList();

                        placeholder = Bilanceprofiles_List.FirstOrDefault();

                        if (placeholder == null)
                        {
                            return 2;
                        }
                    }

                    Bilanceprofile = placeholder;

                    Preferences.Set("Blanceprofile", Bilanceprofile.Id);

                    return 1;
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));

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

                List<string> checklist = new List<string>();

                checklist.AddRange(Bilanceprofile.Ignore);

                checklist.AddRange(Bilanceprofile.Outcome_Account);

                checklist.AddRange(Bilanceprofile.Income_Account);

                checklist.AddRange(Bilanceprofile.Outcome_Cash);

                checklist.AddRange(Bilanceprofile.Outcome_Cash);

                foreach (Zweck zw in count)
                {
                    if (checklist.Contains(zw.Benutzerdefinierter_Zweck) == false)
                    {
                        if (zw.Reason_Visibility == true)
                        {
                            return false;
                        }
                    }
                }

                bool indikator = false;

                foreach (string st in checklist)
                {
                    if (count.Where(zw => zw.Benutzerdefinierter_Zweck == st).Count() == 0)
                    {
                        if (Bilanceprofile.Outcome_Account.Contains(st) == true)
                        {
                            Bilanceprofile.Outcome_Account.Remove(st);

                            indikator = true;
                        }

                        if (Bilanceprofile.Income_Account.Contains(st) == true)
                        {
                            Bilanceprofile.Income_Account.Remove(st);

                            indikator = true;
                        }

                        if (Bilanceprofile.Outcome_Cash.Contains(st) == true)
                        {
                            Bilanceprofile.Outcome_Cash.Remove(st);

                            indikator = true;
                        }

                        if (Bilanceprofile.Income_Cash.Contains(st) == true)
                        {
                            Bilanceprofile.Income_Cash.Remove(st);

                            indikator = true;
                        }

                        if (Bilanceprofile.Ignore.Contains(st) == true)
                        {
                            Bilanceprofile.Ignore.Remove(st);

                            indikator = true;
                        }
                    }
                }

                if (indikator == true)
                {
                    await BalanceService.Edit_Balanceprofile(Bilanceprofile);
                }

                return true;
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));

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

                    initreasons.AddRange(Bilanceprofile.Ignore);

                    List<string> openreason = new List<string>();

                    foreach (Zweck zw in zwecklist)
                    {
                        if (initreasons.Contains(zw.Benutzerdefinierter_Zweck) == false)
                        {
                            if (zw.Reason_Visibility == true)
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
                            var result = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("" + reason + " zuordnen", 400, new List<string>() { "Ausgaben Konto", "Einnahmen Konto", "Barausgaben", "Bareinnahmen", "ignorieren" }));

                            if (result == null)
                            {
                                continue;
                            }

                            if ((string)result == "Ausgaben Konto")
                            {
                                Bilanceprofile.Outcome_Account.Add(reason);

                                indikator = true;
                            }

                            if ((string)result == "Einnahmen Konto")
                            {
                                Bilanceprofile.Income_Account.Add(reason);

                                indikator = true;
                            }

                            if ((string)result == "Barausgaben")
                            {
                                Bilanceprofile.Outcome_Cash.Add(reason);

                                indikator = true;
                            }

                            if ((string)result == "Bareinnahmen")
                            {
                                Bilanceprofile.Income_Cash.Add(reason);

                                indikator = true;
                            }

                            if ((string)result == "ignorieren")
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
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        private async Task Settings_Methode()
        {
            try
            {
                bool indikator = false;

                while (indikator == false)
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

                    var result0 = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Einstellungen", 380, new List<string>() { "Bilanzprofil auswählen", "Neues Bilanzprofil erstellen", "Bilanzprofil löschen", reorder_string }));

                    if (result0 == null)
                    {
                        indikator = true;

                        return;
                    }

                    if ((string)result0 == "Schließen")
                    {
                        indikator = true;

                        return;
                    }

                    if ((string)result0 == "Neues Bilanzprofil erstellen")
                    {
                        await Create_Balanceprofile();
                    }

                    if ((string)result0 == "Bilanzprofil auswählen")
                    {
                        await Choose_Balanceprofile_Methode();

                        await LoadCompare();
                    }

                    if ((string)result0 == "Bilanzprofil löschen")
                    {
                        await Delete_Balanceprofile_Methode();

                        await LoadCompare();
                    }

                    if ((string)result0 == "Neuordnung des Bilanzprofiles " + Preferences.Get("Blanceprofile", 0) + "")
                    {
                        await Reorder_Balanceprofile_Methode();

                        await LoadCompare();
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        private async Task Choose_Balanceprofile_Methode()
        {
            try
            {
                await Get_BalanceprofileList();

                if (Bilanceprofiles_List.Count() == 0)
                {
                    await Create_Balanceprofile();

                    return;
                }

                List<string> balanceprofile_list = new List<string>();

                foreach (Balanceprofile bp in Bilanceprofiles_List)
                {
                    balanceprofile_list.Add("Bilanzprofil " + bp.Id + "");
                }

                bool indikator = false;

                Balanceprofile CurrentBilanceprofil = Bilanceprofile;

                string current_balanceprofile_string = "Bilanzprofil auswählen\nAktuell : Bilanzprofil " + Bilanceprofile.Id + "";

                if (CurrentBilanceprofil == null)
                {
                    current_balanceprofile_string = "Bilanzprofil auswählen\nAktuell kein Bilanzprofil ausgewählt";
                }

                while (indikator == false)
                {
                    var result = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup(current_balanceprofile_string, 400, balanceprofile_list));

                    if (result == null)
                    {
                        if (Bilanceprofile != null)
                        {
                            return;
                        }
                    }
                    else
                    {
                        int id = int.Parse(result.ToString().Substring(12));

                        Balanceprofile ChoosenBilanceprofil = await BalanceService.Get_specific_Balanceprofile(id);

                        List<Zweck> count = new List<Zweck>(await ReasonService.Get_Enable_ReasonList());

                        List<string> checklist = new List<string>();

                        checklist.AddRange(ChoosenBilanceprofil.Ignore);

                        checklist.AddRange(ChoosenBilanceprofil.Outcome_Account);

                        checklist.AddRange(ChoosenBilanceprofil.Income_Account);

                        checklist.AddRange(ChoosenBilanceprofil.Outcome_Cash);

                        checklist.AddRange(ChoosenBilanceprofil.Outcome_Cash);

                        bool indikator2 = false;

                        foreach (string st in checklist)
                        {
                            if (count.Where(zw => zw.Benutzerdefinierter_Zweck == st).Count() == 0)
                            {
                                if (ChoosenBilanceprofil.Outcome_Account.Contains(st) == true)
                                {
                                    ChoosenBilanceprofil.Outcome_Account.Remove(st);

                                    indikator2 = true;
                                }

                                if (ChoosenBilanceprofil.Income_Account.Contains(st) == true)
                                {
                                    ChoosenBilanceprofil.Income_Account.Remove(st);

                                    indikator2 = true;
                                }

                                if (ChoosenBilanceprofil.Outcome_Cash.Contains(st) == true)
                                {
                                    ChoosenBilanceprofil.Outcome_Cash.Remove(st);

                                    indikator2 = true;
                                }

                                if (ChoosenBilanceprofil.Income_Cash.Contains(st) == true)
                                {
                                    ChoosenBilanceprofil.Income_Cash.Remove(st);

                                    indikator2 = true;
                                }

                                if (ChoosenBilanceprofil.Ignore.Contains(st) == true)
                                {
                                    ChoosenBilanceprofil.Ignore.Remove(st);

                                    indikator2 = true;
                                }
                            }
                        }

                        if (indikator2 == true)
                        {
                            await BalanceService.Edit_Balanceprofile(ChoosenBilanceprofil);
                        }


                        List<string[]> resultstring = new List<string[]>();

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

                        foreach (string re in ChoosenBilanceprofil.Ignore)
                        {
                            sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "ignorieren" });
                        }

                        foreach (Stackholderhelper sh in sortetinitreasons)
                        {
                            resultstring.Add(new string[] { sh.Reason.Substring(0, sh.Reason.LastIndexOf(":")) + " als " + sh.Reason.Substring(sh.Reason.LastIndexOf(":") + 1), "Gehört zu " + sh.Option + "" });
                        }

                        var result1 = await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Bilanzprofil " + ChoosenBilanceprofil.Id + "", 390, 0, "Auswählen", null, resultstring));

                        Bilanceprofile = ChoosenBilanceprofil;

                        if (result1 == null)
                        {
                            indikator = false;
                        }
                        else
                        {
                            Preferences.Set("Blanceprofile", ChoosenBilanceprofil.Id);

                            indikator = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
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
                        var result = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("" + reason.Benutzerdefinierter_Zweck + " zuordnen", 400, new List<string>() { "Ausgaben Konto", "Einnahmen Konto", "Barausgaben", "Bareinnahmen", "ignorieren" }));

                        if (result == null)
                        {
                            continue;
                        }

                        if ((string)result == "Ausgaben Konto")
                        {
                            ChoosenBilanceprofil.Outcome_Account.Add(reason.Benutzerdefinierter_Zweck);

                            indikator = true;
                        }

                        if ((string)result == "Einnahmen Konto")
                        {
                            ChoosenBilanceprofil.Income_Account.Add(reason.Benutzerdefinierter_Zweck);

                            indikator = true;
                        }

                        if ((string)result == "Barausgaben")
                        {
                            ChoosenBilanceprofil.Outcome_Cash.Add(reason.Benutzerdefinierter_Zweck);

                            indikator = true;
                        }

                        if ((string)result == "Bareinnahmen")
                        {
                            ChoosenBilanceprofil.Income_Cash.Add(reason.Benutzerdefinierter_Zweck);

                            indikator = true;
                        }

                        if ((string)result == "ignorieren")
                        {
                            ChoosenBilanceprofil.Ignore.Add(reason.Benutzerdefinierter_Zweck);

                            indikator = true;
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
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
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

                    List<string> balanceprofile_list = new List<string>();

                    foreach (Balanceprofile bp in Bilanceprofiles_List)
                    {
                        balanceprofile_list.Add("Bilanzprofil " + bp.Id);
                    }

                    bool indikator = false;

                    Balanceprofile CurrentBilanceprofil = Bilanceprofile;

                    while (indikator == false)
                    {
                        var result1 = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Bilanzprofil löschen\nAktuell : Bilanzprofil " + Bilanceprofile.Id + "", 400, balanceprofile_list));

                        if (result1 == null)
                        {
                            return;
                        }
                        else
                        {
                            int id = int.Parse(result1.ToString().Substring(12));

                            Balanceprofile ChoosenBilanceprofil = await BalanceService.Get_specific_Balanceprofile(id);

                            List<string[]> resultstring = new List<string[]>();

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

                            foreach (string re in ChoosenBilanceprofil.Ignore)
                            {
                                sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "ignorieren" });
                            }

                            foreach (Stackholderhelper sh in sortetinitreasons)
                            {
                                resultstring.Add(new string[] { sh.Reason.Substring(0, sh.Reason.LastIndexOf(":")) + " als " + sh.Reason.Substring(sh.Reason.LastIndexOf(":") + 1), "Gehört zu " + sh.Option + "" });
                            }

                            var result2 = await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Bilanzprofil " + ChoosenBilanceprofil.Id + "", 390, 0, "Löschen", null, resultstring));

                            Bilanceprofile = ChoosenBilanceprofil;

                            if (result2 == null)
                            {
                                indikator = false;
                            }
                            else
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
                                    balanceprofile_list.Clear();

                                    foreach (Balanceprofile bp in Bilanceprofiles_List)
                                    {
                                        balanceprofile_list.Add("Bilanzprofil " + bp.Id);
                                    }
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
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
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
                if (result0 == 2)
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

                foreach (string re in Bilanceprofile.Ignore)
                {
                    sortetinitreasons.Add(new Stackholderhelper() { Reason = re, Option = "ignorieren" });
                }

                Balanceprofile newbalanceprofile = new Balanceprofile();

                if (sortetinitreasons.Count() != 0)
                {
                    bool indikator = false;

                    List<string> resultstring = new List<string>();

                    foreach (Stackholderhelper sh in sortetinitreasons)
                    {
                        resultstring.Add(sh.Reason.Substring(0, sh.Reason.LastIndexOf(":")) + " als " + sh.Reason.Substring(sh.Reason.LastIndexOf(":") + 1).ToString());
                    }

                    while (indikator == false)
                    {


                        var result1 = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("Zweckzuordnung bearbeiten", 390, resultstring));

                        if (result1 == null)
                        {
                            indikator = true;

                            await BalanceService.Edit_Balanceprofile(newbalanceprofile);
                        }
                        else
                        {
                            var result = await Shell.Current.ShowPopupAsync(new CustomeAktionSheet_Popup("" + result1 + " zuordnen", 400, new List<string>() { "Ausgaben Konto", "Einnahmen Konto", "Barausgaben", "Bareinnahmen", "ignorieren" }));

                            if (result == null)
                            {
                                continue;
                            }

                            if ((string)result == "Ausgaben Konto")
                            {

                                sortetinitreasons[resultstring.IndexOf((string)result1)].Option = "Ausgaben Konto";
                            }

                            if ((string)result == "Einnahmen Konto")
                            {
                                sortetinitreasons[resultstring.IndexOf((string)result1)].Option = "Einnahmen Konto";
                            }

                            if ((string)result == "Barausgaben")
                            {
                                sortetinitreasons[resultstring.IndexOf((string)result1)].Option = "Barausgaben";
                            }

                            if ((string)result == "Bareinnahmen")
                            {
                                sortetinitreasons[resultstring.IndexOf((string)result1)].Option = "Bareinnahmen";
                            }

                            if ((string)result == "ignorieren")
                            {
                                sortetinitreasons[resultstring.IndexOf((string)result1)].Option = "ignorieren";
                            }

                            resultstring.Clear();

                            foreach (Stackholderhelper sh in sortetinitreasons)
                            {
                                resultstring.Add(sh.Reason.Substring(0, sh.Reason.LastIndexOf(":")) + " als " + sh.Reason.Substring(sh.Reason.LastIndexOf(":") + 1).ToString());
                            }
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
                await Shell.Current.ShowPopupAsync(new CustomeAlert_Popup("Fehler", 380, 0, null, null, "Es ist ein Fehler aufgetretten.\nFehler:" + ex.ToString() + ""));
            }
        }

        private async Task Notificater(string v)
        {
            await Shell.Current.DisplayToastAsync(v, 5000);
        }

        public Balanceprofile Bilanceprofile = null;

        public List<Balanceprofile> Bilanceprofiles_List = new List<Balanceprofile>();

        public Viewtime Item1_Viewtime = new Viewtime();

        public Viewtime Item2_Viewtime = new Viewtime();

        public List<Transaktion> Transaktion_List = new List<Transaktion>();

        public Haushaltsbücher Haushaltsbucher = null;

        public List<HelperComparer> result1 = new List<HelperComparer> { };

        public List<HelperComparer> result2 = new List<HelperComparer> { };

        public bool ssswitched = false;
        public bool ISSwitched
        {
            get { return ssswitched; }
            set
            {
                if (ISSwitched == value)
                {
                    return;
                }

                ssswitched = value; RaisePropertyChanged();
            }
        }
        public AsyncCommand Load_Command { get; }
        public AsyncCommand Period_Command { get; }
        public AsyncCommand Settings_Command { get; }
        public AsyncCommand Set_Item1_Command { get; }
        public AsyncCommand Set_Item2_Command { get; }
        public AsyncCommand Switch_Diff_Command { get; }
        public AsyncCommand<int> Detail_Command { get; }

        public string title = "Vergleichen";
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


        public string item1_outcome_account = "0 €";
        public string Item1_Outcome_Account
        {
            get { return item1_outcome_account; }
            set
            {
                if (Item1_Outcome_Account == value)
                {
                    return;
                }

                item1_outcome_account = value; RaisePropertyChanged();
            }
        }

        public string item2_outcome_account = "0 €";
        public string Item2_Outcome_Account
        {
            get { return item2_outcome_account; }
            set
            {
                if (Item2_Outcome_Account == value)
                {
                    return;
                }

                item2_outcome_account = value; RaisePropertyChanged();
            }
        }

        public string diff_outcome_account = "0 €";
        public string Diff_Outcome_Account
        {
            get { return diff_outcome_account; }
            set
            {
                if (Diff_Outcome_Account == value)
                {
                    return;
                }

                diff_outcome_account = value; RaisePropertyChanged();
            }
        }

        public string item1_income_account = "0 €";
        public string Item1_Income_Account
        {
            get { return item1_income_account; }
            set
            {
                if (Item1_Income_Account == value)
                {
                    return;
                }

                item1_income_account = value; RaisePropertyChanged();
            }
        }

        public string item2_income_account = "0 €";
        public string Item2_Income_Account
        {
            get { return item2_income_account; }
            set
            {
                if (Item2_Income_Account == value)
                {
                    return;
                }

                item2_income_account = value; RaisePropertyChanged();
            }
        }

        public string diff_income_account = "0 €";
        public string Diff_Income_Account
        {
            get { return diff_income_account; }
            set
            {
                if (Diff_Income_Account == value)
                {
                    return;
                }

                diff_income_account = value; RaisePropertyChanged();
            }
        }

        public string item1_account_result = "0 €";
        public string Item1_Account_Result
        {
            get { return item1_account_result; }
            set
            {
                if (Item1_Account_Result == value)
                {
                    return;
                }

                item1_account_result = value; RaisePropertyChanged();
            }
        }

        public string item2_account_result = "0 €";
        public string Item2_Account_Result
        {
            get { return item2_account_result; }
            set
            {
                if (Item2_Account_Result == value)
                {
                    return;
                }

                item2_account_result = value; RaisePropertyChanged();
            }
        }

        public string diff_account_result = "0 €";
        public string Diff_Account_Result
        {
            get { return diff_account_result; }
            set
            {
                if (Diff_Account_Result == value)
                {
                    return;
                }

                diff_account_result = value; RaisePropertyChanged();
            }
        }

        public string item1_outcome_cash = "0 €";
        public string Item1_Outcome_Cash
        {
            get { return item1_outcome_cash; }
            set
            {
                if (Item1_Outcome_Cash == value)
                {
                    return;
                }

                item1_outcome_cash = value; RaisePropertyChanged();
            }
        }

        public string item2_outcome_cash = "0 €";
        public string Item2_Outcome_Cash
        {
            get { return item2_outcome_cash; }
            set
            {
                if (Item2_Outcome_Cash == value)
                {
                    return;
                }

                item2_outcome_cash = value; RaisePropertyChanged();
            }
        }

        public string diff_outcome_cash = "0 €";
        public string Diff_Outcome_Cash
        {
            get { return diff_outcome_cash; }
            set
            {
                if (Diff_Outcome_Cash == value)
                {
                    return;
                }

                diff_outcome_cash = value; RaisePropertyChanged();
            }
        }

        public string item1_income_cash = "0 €";
        public string Item1_Income_Cash
        {
            get { return item1_income_cash; }
            set
            {
                if (Item1_Income_Cash == value)
                {
                    return;
                }

                item1_income_cash = value; RaisePropertyChanged();
            }
        }

        public string item2_income_cash = "0 €";
        public string Item2_Income_Cash
        {
            get { return item2_income_cash; }
            set
            {
                if (Item2_Income_Cash == value)
                {
                    return;
                }

                item2_income_cash = value; RaisePropertyChanged();
            }
        }

        public string diff_income_cash = "0 €";
        public string Diff_Income_Cash
        {
            get { return diff_income_cash; }
            set
            {
                if (Diff_Income_Cash == value)
                {
                    return;
                }

                diff_income_cash = value; RaisePropertyChanged();
            }
        }

        public string item1_cash_result = "0 €";
        public string Item1_Cash_Result
        {
            get { return item1_cash_result; }
            set
            {
                if (Item1_Cash_Result == value)
                {
                    return;
                }

                item1_cash_result = value; RaisePropertyChanged();
            }
        }

        public string item2_cash_result = "0 €";
        public string Item2_Cash_Result
        {
            get { return item2_cash_result; }
            set
            {
                if (Item2_Cash_Result == value)
                {
                    return;
                }

                item2_cash_result = value; RaisePropertyChanged();
            }
        }

        public string diff_cash_result = "0 €";
        public string Diff_Cash_Result
        {
            get { return diff_cash_result; }
            set
            {
                if (Diff_Cash_Result == value)
                {
                    return;
                }

                diff_cash_result = value; RaisePropertyChanged();
            }
        }

        public string item1_result = "0 €";
        public string Item1_Result
        {
            get { return item1_result; }
            set
            {
                if (Item1_Result == value)
                {
                    return;
                }

                item1_result = value; RaisePropertyChanged();
            }
        }

        public string item2_result = "0 €";
        public string Item2_Result
        {
            get { return item2_result; }
            set
            {
                if (Item2_Result == value)
                {
                    return;
                }

                item2_result = value; RaisePropertyChanged();
            }
        }

        public string diff_result = "0 €";
        public string Diff_Result
        {
            get { return diff_result; }
            set
            {
                if (Diff_Result == value)
                {
                    return;
                }

                diff_result = value; RaisePropertyChanged();
            }
        }

        public string item1_name = "Bilanz 1";
        public string Item1_Name
        {
            get { return item1_name; }
            set
            {
                if (Item1_Name == value)
                {
                    return;
                }

                item1_name = value; RaisePropertyChanged();
            }
        }

        public string item2_name = "Bilanz 2";
        public string Item2_Name
        {
            get { return item2_name; }
            set
            {
                if (Item2_Name == value)
                {
                    return;
                }

                item2_name = value; RaisePropertyChanged();
            }
        }

        public Color diff_outcome_account_color = Color.Gray;
        public Color Diff_Outcome_Account_Color
        {
            get { return diff_outcome_account_color; }
            set
            {
                if (Diff_Outcome_Account_Color == value)
                {
                    return;
                }

                diff_outcome_account_color = value; RaisePropertyChanged();
            }
        }

        public Color diff_intcome_account_color = Color.Gray;
        public Color Diff_Income_Account_Color
        {
            get { return diff_intcome_account_color; }
            set
            {
                if (Diff_Income_Account_Color == value)
                {
                    return;
                }

                diff_intcome_account_color = value; RaisePropertyChanged();
            }
        }

        public Color diff_account_result_color = Color.Gray;
        public Color Diff_Account_Result_Color
        {
            get { return diff_account_result_color; }
            set
            {
                if (Diff_Account_Result_Color == value)
                {
                    return;
                }

                diff_account_result_color = value; RaisePropertyChanged();
            }
        }

        public Color diff_outcome_cash_color = Color.Gray;
        public Color Diff_Outcome_Cash_Color
        {
            get { return diff_outcome_cash_color; }
            set
            {
                if (Diff_Outcome_Cash_Color == value)
                {
                    return;
                }

                diff_outcome_cash_color = value; RaisePropertyChanged();
            }
        }

        public Color diff_intcome_cash_color = Color.Gray;
        public Color Diff_Income_Cash_Color
        {
            get { return diff_intcome_cash_color; }
            set
            {
                if (Diff_Income_Cash_Color == value)
                {
                    return;
                }

                diff_intcome_cash_color = value; RaisePropertyChanged();
            }
        }

        public Color diff_cash_result_color = Color.Gray;
        public Color Diff_Cash_Result_Color
        {
            get { return diff_cash_result_color; }
            set
            {
                if (Diff_Cash_Result_Color == value)
                {
                    return;
                }

                diff_cash_result_color = value; RaisePropertyChanged();
            }
        }

        public Color diff_result_color = Color.Gray;
        public Color Diff_Result_Color
        {
            get { return diff_result_color; }
            set
            {
                if (Diff_Result_Color == value)
                {
                    return;
                }

                diff_result_color = value; RaisePropertyChanged();
            }
        }
    }

    public class HelperComparer
    {
        public double Value { get; set; }

        public List<string> ButonValueList {get; set;}

        public List<string> ButonReasonList { get; set; }
    }
}
