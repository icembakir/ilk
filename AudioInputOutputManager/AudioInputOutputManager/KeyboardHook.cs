using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AudioInputOutputManager
{
    class KeyboardHook
    {
        public static KeyboardHook GetInstance()
        {
            if (keyboardHook == null )
            {
                keyboardHook = new KeyboardHook();
            }
            return keyboardHook;
        }

        public void AddListener(char key, IKeyboardObserver observer)
        {
            if (!observers.ContainsKey(key))
                observers.Add(key, new ArrayList());
            observers[key].Add(observer);
        }

        public void RemoveListener(char key, IKeyboardObserver observer)
        {
            if(observers.ContainsKey(key))
                observers[key].Remove(observer);
        }

        private KeyboardHook()
        {
            _hookID = SetHook(_proc);
            observers = new Dictionary<char, ArrayList>();
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                char ch = (char)vkCode;
                if (observers[ch].Count != 0)
                {
                    for (int i = 0; i < observers[ch].Count; i++)
                    {
                        ((IKeyboardObserver)observers[ch][i]).KeyPressed();
                    }
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private static KeyboardHook keyboardHook = null;

        private static Dictionary< Char, ArrayList > observers;

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
