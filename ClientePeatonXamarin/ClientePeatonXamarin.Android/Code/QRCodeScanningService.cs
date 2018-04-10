using ClientePeatonXamarin.Droid.Code;
using System.Threading.Tasks;
using ZXing.Mobile;
using Xamarin.Forms;
using ClientePeatonXamarin.Code;

[assembly: Dependency(typeof(QrCodeScanningService))]
namespace ClientePeatonXamarin.Droid.Code
{
    public class QrCodeScanningService : IQrCodeScanningService
    {
        public async Task<string> ScanAsync()
        {
            var scanner = new MobileBarcodeScanner()
            {
                TopText = "Acerca la cámara al elemento",
                BottomText = "Toca la pantalla para enfocar"
            };

            var scanResults = await scanner.Scan();
            return (scanResults != null) ? scanResults.Text : string.Empty;
        }
    }
}