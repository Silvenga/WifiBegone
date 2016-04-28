namespace WifiBegone.Tray.Helpers
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Security;

    [SuppressUnmanagedCodeSecurity]
    public static class ConsoleManager
    {
        // http://stackoverflow.com/questions/160587/no-output-to-console-from-a-wpf-application

        private const string Kernel32Dll = "kernel32.dll";

        [DllImport(Kernel32Dll)]
        private static extern bool AllocConsole();

        [DllImport(Kernel32Dll)]
        private static extern bool FreeConsole();

        [DllImport(Kernel32Dll)]
        private static extern IntPtr GetConsoleWindow();

        [DllImport(Kernel32Dll)]
        private static extern int GetConsoleOutputCP();

        public static bool HasConsole => GetConsoleWindow() != IntPtr.Zero;

        public static void Show()
        {
            //#if DEBUG
            if (!HasConsole)
            {
                AllocConsole();
                InvalidateOutAndError();
            }
            //#endif
        }

        public static void Hide()
        {
            //#if DEBUG
            if (HasConsole)
            {
                SetOutAndErrorNull();
                FreeConsole();
            }
            //#endif
        }

        public static void Toggle()
        {
            if (HasConsole)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        private static void InvalidateOutAndError()
        {
            var type = typeof(Console);

            var @out = type.GetField("_out",
                BindingFlags.Static | BindingFlags.NonPublic);

            var error = type.GetField("_error",
                BindingFlags.Static | BindingFlags.NonPublic);

            var initializeStdOutError = type.GetMethod("InitializeStdOutError",
                BindingFlags.Static | BindingFlags.NonPublic);

            Debug.Assert(@out != null);
            Debug.Assert(error != null);

            Debug.Assert(initializeStdOutError != null);

            @out.SetValue(null, null);
            error.SetValue(null, null);

            initializeStdOutError.Invoke(null, new object[] {true});
        }

        private static void SetOutAndErrorNull()
        {
            Console.SetOut(TextWriter.Null);
            Console.SetError(TextWriter.Null);
        }
    }
}