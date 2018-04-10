using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ClientePeatonXamarin.Code
{
    public interface IQrCodeScanningService
    {
        Task<string> ScanAsync();
    }
}