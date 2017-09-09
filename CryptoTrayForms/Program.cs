using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Text;

namespace CryptoTrayForms
{
    static class Program
    {
        static Api client;

        [STAThread]
        static void Main()
        {
            client = new CryptoTrayForms.Api();

            Thread t = new Thread((f) =>
            {
                System.Timers.Timer apiTimer = new System.Timers.Timer();
                apiTimer.Elapsed += T_Elapsed;

#if DEBUG
                apiTimer.Interval = new TimeSpan(0, 1, 0).TotalMilliseconds;
#endif
#if !DEBUG
                apiTimer.Interval = new TimeSpan(0, 10, 0).TotalMilliseconds;
#endif

                apiTimer.Start();

                //Keep this thread alive
                while(true)
                {
                    Thread.Sleep(10000);
                }                                   
            });
            t.Start();            
        }

        private static void T_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                var prices = client.GetMajorCryptoPrices();

                StringBuilder sb = new StringBuilder();

                foreach (var price in prices)
                {
                    if (price.IsSuccess)
                    {
                        sb.AppendLine($"{price.Response.Base} - {decimal.Round(price.Response.Price, 2)} - Change: {decimal.Round(price.Response.Change, 2)}");
                    }
                }

                var msg = sb.ToString();
                if (msg.Length > 0)
                {
                    ShowNotification(sb.ToString());
                }
            }
            catch(Exception ex)
            {
                ShowNotification($"An error occured while obtaining crypto results: {ex.Message}");
            }         
        }


        static void ShowNotification(string msg)
        {
            var notification = new System.Windows.Forms.NotifyIcon()
            {
                Visible = true,
                Icon = System.Drawing.SystemIcons.Information,
                BalloonTipTitle = "Current Crypto Rates",
                BalloonTipText = msg,
            };

            var path = Environment.CurrentDirectory + "\\coin_VAc_icon.ico";
            notification.Icon = new System.Drawing.Icon(path);

            notification.ShowBalloonTip(10000);
            Thread.Sleep(10000);
            notification.Dispose();
        }
    }
}
