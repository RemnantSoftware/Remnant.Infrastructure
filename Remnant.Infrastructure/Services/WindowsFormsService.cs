#if (!NETCOREAPP1_0 && !NETCOREAPP1_1 && !NETCOREAPP2_0 && !NETCOREAPP2_1)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Remnant.Core.Services
{
  public static class WindowsFormsService
  {
    public static DialogResult ShowFormModal<TForm>(params object[] args)
      where TForm : Form, new()
    {
      var form = (Form)DomainsService.Instance.CreateInstance(typeof (TForm), args);
      try
      {
        return form.ShowDialog();
      }      
      finally 
      {
      form.Dispose();
      }
    }

    public static void HourGlassOn(Form form)
    {
      form.Cursor = Cursors.WaitCursor;
    }

    public static void HourGlassOff(Form form)
    {
      form.Cursor = Cursors.Default;
    }

    public static class Keys
    {
      public const string Space = " ";
      public const string DoubleQuote = "\"";
      public const string SingleQuote = "'";
			public const string OpenBracket = "[";
			public const string CloseBracket = "]";
    }

  }
}
#endif