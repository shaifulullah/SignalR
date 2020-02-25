namespace MDA.Security
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Security.Principal;

    public class Impersonator : IDisposable
    {
        private const int LOGON32_LOGON_INTERACTIVE = 2;
        private const int LOGON32_LOGON_NEW_CREDENTIALS = 9;
        private const int LOGON32_PROVIDER_DEFAULT = 0;

        /// <summary>
        /// Impersonation Context
        /// </summary>
        private WindowsImpersonationContext impersonationContext = null;

        /// <summary>
        /// Initializes a new instance of the Impersonator class.
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="domain">Domain</param>
        /// <param name="password">Password</param>
        public Impersonator(string userName, string domain, string password)
        {
            ImpersonateValidUser(userName, domain, password);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        public void Dispose()
        {
            UndoImpersonation();
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int DuplicateToken(IntPtr hToken, int impersonationLevel, ref IntPtr hNewToken);

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern int LogonUser(string lpszUserName, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool RevertToSelf();

        /// <summary>
        /// Impersonate Valid User
        /// </summary>
        /// <param name="userName">User Name</param>
        /// <param name="domain">Domain</param>
        /// <param name="password">Password</param>
        private void ImpersonateValidUser(string userName, string domain, string password)
        {
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;

            try
            {
                if (RevertToSelf())
                {
                    if (LogonUser(userName, domain, password, LOGON32_LOGON_NEW_CREDENTIALS, LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                    {
                        if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                        {
                            var windowsIdentity = new WindowsIdentity(tokenDuplicate);
                            impersonationContext = windowsIdentity.Impersonate();
                        }
                        else
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error());
                        }
                    }
                    else
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error());
                    }
                }
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            finally
            {
                if (token != IntPtr.Zero)
                {
                    CloseHandle(token);
                }

                if (tokenDuplicate != IntPtr.Zero)
                {
                    CloseHandle(tokenDuplicate);
                }
            }
        }

        /// <summary>
        /// Undo Impersonation
        /// </summary>
        private void UndoImpersonation()
        {
            if (impersonationContext != null)
            {
                impersonationContext.Undo();
            }
        }
    }
}