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
        public AutoMuter()
        {
            InitializeComponent();
            this.notifyIcon.Visible = true;
            ReadConfig();
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
        // last active window process
        private Process lastActiveProcess = null;
        // event when change active window 
        public void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            if (this.debug) { textBox.Text = "WinEventProc" + "\r\n\r\n" + textBox.Text; }    // debug log

            string filePath, fileName;
            Process activeProcess = GetForegroundWindowProcess();

            Process[] ps = { activeProcess, lastActiveProcess };
            for (int i = 1; i >= 0; --i)
            {
                Process p = ps[i];
                if (p == null) { continue; }

                // check target
                filePath = p.MainModule.FileName.ToString();
                fileName = Path.GetFileName(filePath);
                AudioSession audioSession = AudioUtilities.GetProcessSession(p);
                if (!this.TargetRx.IsMatch(filePath) || audioSession == null)
                {
                    if (this.debug) { textBox.Text = "[ ] " + fileName + "\r\n" + textBox.Text; }    // debug log
                    continue;
                }

                // unmute avtive window
                // mute inactive window
                audioSession.SetApplicationMute(i == 1);
                if (this.debug) { textBox.Text = (i == 1 ? "[-] " : "[+] ") + fileName + "\r\n" + textBox.Text; }    // debug log

                audioSession.Dispose();
            }

            if (this.lastActiveProcess != null) this.lastActiveProcess.Dispose();
            this.lastActiveProcess = activeProcess;
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
