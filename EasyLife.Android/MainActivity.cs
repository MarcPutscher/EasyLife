using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using SQLite;
using Android.Content.Res;
using Android.Util;
using static Android.OS.Build;
using Xamarin.Forms;
using EasyLife.Helpers;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Org.W3c.Dom;
using System.IO;
using EasyLife.Interfaces;
using System.Text;
using EasyLife.Droid;
using Android.Media;
using AndroidX.Core.OS;
using Android.Support.V4.App;
using Android;
using Android.Support.V4.Content;
using Xamarin.Essentials;
using EasyLife.Services;
using Xamarin.Forms.PlatformConfiguration;
using System.Linq;
using EasyLife.PageModels;
using System.Globalization;
using System.Threading.Tasks;
using Android.Webkit;

[assembly: Dependency(typeof(CloseApplication))]
[assembly: Dependency(typeof(AccessFileImplement))]
namespace EasyLife.Droid
{
    [Activity(Label = "EasyLife", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Rg.Plugins.Popup.Popup.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            if (Preferences.Get("App_first_Created", false) == false)
            {
                Preferences.Set("App_first_Created", true);

                Preferences.Set("Next_Backup_Date", DateTime.Now.AddMonths(1).ToString("dd.MM.yyyy"));

                const int requestLocationId = 2023;

                string[] notiPermission = { Manifest.Permission.PostNotifications /*, Manifest.Permission.ReadMediaImages , Manifest.Permission.ReadMediaAudio , Manifest.Permission.ReadMediaVideo , Manifest.Permission.WriteExternalStorage , Manifest.Permission.ManageExternalStorage , Manifest.Permission.ReadExternalStorage*/};

                if ((int)Build.VERSION.SdkInt < 33) return;

                if (this.CheckSelfPermission(Manifest.Permission.PostNotifications) != Permission.Granted /*|| this.CheckSelfPermission(Manifest.Permission.ReadExternalStorage) != Permission.Granted || this.CheckSelfPermission(Manifest.Permission.ReadMediaImages) != Permission.Granted || this.CheckSelfPermission(Manifest.Permission.ManageExternalStorage) != Permission.Granted || this.CheckSelfPermission(Manifest.Permission.ReadMediaAudio) != Permission.Granted || this.CheckSelfPermission(Manifest.Permission.ReadMediaVideo) != Permission.Granted || this.CheckSelfPermission(Manifest.Permission.WriteExternalStorage) != Permission.Granted*/)
                {
                    this.RequestPermissions(notiPermission, requestLocationId);
                }
            }
        }

        protected override async void OnStart()
        {
            base.OnStart();

            if (Preferences.Get("Next_Backup_Date", "") != "")
            {
                if (DateTime.ParseExact(Preferences.Get("Next_Backup_Date", ""), "dd.MM.yyyy", new CultureInfo("de-DE")).Date <= DateTime.Now.Date)
                {
                    Preferences.Set("Next_Backup_Date", DateTime.Now.Date.ToString("dd.MM.yyyy"));

                    var result = await Services.BackupService.Create_Backup(Preferences.Get("Next_Backup_Date", ""));

                    if (result == true)
                    {
                        Preferences.Set("Next_Backup_Date", DateTime.Now.AddMonths(1).ToString("dd.MM.yyyy"));

                        Preferences.Set("Last_Backup_Date", DateTime.Now.ToString("dd.MM.yyyy"));

                        await ToastHelper.ShowToast("Es wurde erfolgreich ein automatisches Backup erstellt.");
                    }
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public override Resources Resources
        {
            get
            {
                Resources resource = base.Resources;
                Configuration configuration = new Configuration();
                configuration.SetToDefaults();
                if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.NMr1)
                {
                    return CreateConfigurationContext(configuration).Resources;
                }
                else
                {
                    resource.UpdateConfiguration(configuration, resource.DisplayMetrics);
                    return resource;
                }
            }
        }
    }

    public class AccessFileImplement : IAccessFile
    {
        public string CreateFile(string FileName)
        {
            //var DirectoryPath = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, "EasyLife Bilanzen");

            var DirectoryPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath;

            if (!Directory.Exists(DirectoryPath))
            {
                Directory.CreateDirectory(DirectoryPath);
            }

            if(FileName != null)
            {
                return Path.Combine(DirectoryPath.ToString(), FileName);
            }

            return DirectoryPath.ToString();
        }
    }

    public class CloseApplication : ICloseApplication
    {
        public void closeApplication()
        {
            var activity = (Activity)Forms.Context;
            activity.FinishAffinity();
        }
    }
}