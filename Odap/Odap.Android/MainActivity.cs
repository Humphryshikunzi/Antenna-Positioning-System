﻿using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Telephony;
using Xamarin.Forms;
using System.Collections.Generic;
using System;

namespace Odap.Droid
{
    [Activity(Label = "Odap", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        [System.Obsolete]
        public void GetCellTowerInfor( )
        {
            var infor = new Dictionary<string, string>();
            TelephonyManager tel =(TelephonyManager)Forms.Context.GetSystemService(TelephonyService);
            infor.Add("SignalStrength", tel.SignalStrength.ToString());
            infor.Add("DeviceId", tel.DeviceId);
            infor.Add("Line1Number", tel.Line1Number);
            infor.Add("phoneType", tel.PhoneType.ToString()); // gsm, cdma 

            Dictionary<string,  int> cellObj = new Dictionary<string, int>();
            List<NeighboringCellInfo> neighCells = (List<NeighboringCellInfo>)tel.NeighboringCellInfo;
            for (int i = 0; i < neighCells.Count; i++)
            {
                try
                { 
                    NeighboringCellInfo thisCell = neighCells[i];
                    cellObj.Add("celId-" + i, thisCell.Cid);
                    cellObj.Add("lac-"+i, thisCell.Lac);
                    cellObj.Add("rssi-"+i, thisCell.Rssi);  
                }
                catch (Exception e)
                {
                }
            }

            infor.Add("cellsInfor", cellObj.ToString());
        }
    }
}