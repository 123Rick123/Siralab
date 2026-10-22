using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace COM_WS_SIRALAB
{
    public class ReportingCredentials : Microsoft.Reporting.WebForms.IReportServerCredentials
    {
        private string _UserName;
        private string _PassWord;
        private string _DomainName;

        public ReportingCredentials(string userName, string passWord, string domainName)
        {
            _UserName = userName;
            _PassWord = passWord;
            _DomainName = domainName;
        }

        public WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;
            }
        }

        public ICredentials NetworkCredentials
        {
            get
            {
                return new NetworkCredential(_UserName, _PassWord, _DomainName);
            }
        }

        public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
        {
            authCookie = null;
            user = password = authority = null;
            return false;
        }
    }
}
