using ClientePeatonXamarin.Code;
using System.Threading.Tasks;
using Xamarin.Forms;
using ClientePeatonXamarin.iOS;
using ZXing.Mobile;

[assembly: Dependency(typeof(QrCodeScanningService))]
namespace ClientePeatonXamarin.iOS
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