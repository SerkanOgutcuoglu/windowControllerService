using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

class Program
{
    // WinAPI fonksiyonları
    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SetWindowText(IntPtr hWnd, string lpString);

    [DllImport("user32.dll")]
    public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    const int SW_MINIMIZE = 6;
    const int SW_MAXIMIZE = 3;
    const uint WM_CLOSE = 0x0010;

    static void Main(string[] args)
    {
        Thread.Sleep(5000);
        // 1. Aktif pencereyi bul
        IntPtr hwnd = GetForegroundWindow();

        if (hwnd == IntPtr.Zero)
        {
            Console.WriteLine("Aktif pencere bulunamadı.");
            return;
        }

        // 2. Başlığını al
        StringBuilder sb = new StringBuilder(256);
        GetWindowText(hwnd, sb, sb.Capacity);
        string originalTitle = sb.ToString();

        // 3. Başlığı dosyaya kaydet
        string path = AppDomain.CurrentDomain.BaseDirectory + "PencereBasligi.txt";
        File.WriteAllText(path, $"Orijinal Başlık: {originalTitle}");
        Console.WriteLine($"Başlık kaydedildi: {originalTitle}");

        // 4. Minimize et
        ShowWindow(hwnd, SW_MINIMIZE);
        Thread.Sleep(1000);

        // 5. Maximize et
        ShowWindow(hwnd, SW_MAXIMIZE);
        Thread.Sleep(1000);

        // 6. Başlığı değiştir
        SetWindowText(hwnd, "Yeni Başlık");
        Thread.Sleep(2000);

        // 7. Kapat
        PostMessage(hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
       ///consol kendini kapatır
        Environment.Exit(0);
    }
}
