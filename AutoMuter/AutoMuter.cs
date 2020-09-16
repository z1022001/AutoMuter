using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoMuter
{
    public partial class AutoMuter : Form
    {
        //[DllImport("kernel32.dll")] private static extern bool AllocConsole();
        [DllImport("Kernel32")] public static extern void FreeConsole();

        public AutoMuter()
        {
            InitializeComponent();
            this.notifyIcon.Visible = true;
            ReadConfig();

            if (!this.debug)
            {
                FreeConsole();
            }
        }

        private void AutoMuter_Load(object sender, EventArgs e)
        {
            RegisterWinEvent();
        }

        private void AutoMuter_Shown(object sender, EventArgs e) { this.Hide(); }
        private void AutoMuter_SizeChanged(object sender, EventArgs e) { if (this.WindowState == FormWindowState.Minimized) { this.Hide(); } }
        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible)
            {
                this.Hide();
                this.WindowState = FormWindowState.Minimized;
            }
            else if (this.debug)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }

            //System.Media.SoundPlayer simpleSound = new System.Media.SoundPlayer(@"C:\Windows\Media\Windows Logon Sound.wav");
            //simpleSound.Play();
        }
        private void NotifyIconMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "notifyIconMenuItem1") { this.Close(); }
        }

        // Config
        private Regex TargetRx;
        private bool debug = false;
        private IniManager config;
        private void ReadConfig()
        {
            this.config = new IniManager("./AutoMuter.ini");

            string reg = config.ReadIniFile("Config", "TargetRegex", @"AutoMuter\.exe$");
            this.config.WriteIniFile("Config", "TargetRegex", reg);
            this.TargetRx = new Regex(reg, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            this.debug = (config.ReadIniFile("Config", "DEBUG", "false").ToLower() == "true");
            if (this.debug)
            {
                this.Text += " [debug]";
                this.notifyIcon.Text += " [debug]";
            }
        }

        // Register Event
        private void RegisterWinEvent()
        {
            dele = new WinEventDelegate(WinEventProc);
            //IntPtr m_hhook = SetWinEventHook(0x0003, 0x0003, IntPtr.Zero, dele, 0, 0, 0);
            SetWinEventHook(0x0003, 0x0003, IntPtr.Zero, dele, 0, 0, 0);
            SetWinEventHook(0x0017, 0x0017, IntPtr.Zero, dele, 0, 0, 0);
        }
        // event when change active window 
        public void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (this.debug)
            {
                Console.Clear();
                Console.WriteLine("WinEventProc");
            }    // debug log

            //try
            //{
            // get active window pid/path
            //GetWindowThreadProcessId(GetForegroundWindow(), out uint activePID);
            //string activePath = Process.GetProcessById((int)activePID).MainModule.FileName.ToString();
            string activePath = GetForegroundWindowProcess().MainModule.FileName.ToString();

            foreach (AudioSession session in AudioUtilities.GetAllSessions())
            {
                string filePath = ""; try { filePath = session.FileName; } catch { }
                string fileName = Path.GetFileName(filePath);
                //int pid = 0; try { pid = session.ProcessId; } catch { }

                // check target
                if (this.TargetRx.IsMatch(filePath))
                {
                    //bool mute = (pid != activePID && filePath != activePath);
                    //bool mute = pid != activePID;
                    bool mute = filePath != activePath;
                    session.SetApplicationMute(mute);
                    if (this.debug) { Console.WriteLine((mute ? "[-] " : "[+] ") + fileName); }    // debug log
                }
                else if (filePath != "")
                {
                    if (this.debug) { Console.WriteLine("[ ] " + fileName); }    // debug log
                }

                session.Dispose();
            }
            //}
            //catch (Exception e) { Console.WriteLine(e); }

            return;
        }

        delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
        private WinEventDelegate dele = null;
        [DllImport("user32.dll")] private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);
        [DllImport("user32.dll")] private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")] private static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);
        private Process GetForegroundWindowProcess()
        {
            IntPtr hwnd = GetForegroundWindow();
            GetWindowThreadProcessId(hwnd, out uint pid);
            return Process.GetProcessById((int)pid);
        }
    }
}
