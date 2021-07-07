using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace flow
{
  public class Program
  {

    private static flowContext _fc = new flowContext();

    public static void Main(string[] args)
    {

      // f.Samples.Add(new Sample()
      // {
      //   CountryCode = "RU",
      //   ClientNumber = "01",
      //   Year = "21",
      //   SetNumber = "01",
      //   SetIndex = "01",
      //   SampleNumber = "01"

      // }
      // );

      // f.Samples.Add(new Sample()
      // {
      //   CountryCode = "RU",
      //   ClientNumber = "01",
      //   Year = "21",
      //   SetNumber = "01",
      //   SetIndex = "01",
      //   SampleNumber = "02"

      // }
      // );


      //   f.Irradiations.Add(new Irradiation() { Sample = f.Samples.OrderBy(s => s.Id).First() });
      //   f.Irradiations.Add(new Irradiation() { Sample = f.Samples.OrderBy(s => s.Id).Last() });
      //   f.SaveChanges();
      // }

      Application.SetHighDpiMode(HighDpiMode.SystemAware);
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      var frm = new Form()
      {
        Size = new System.Drawing.Size(600, 400)
      };

      frm.Disposed += (s, e) => { _fc.Dispose(); };

      var flp = new TableLayoutPanel()
      {
        Dock = DockStyle.Fill,
        RowCount = 2,
        ColumnCount = 1
      };

      flp.RowStyles.Add(new RowStyle(SizeType.Percent, 90F));
      flp.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));

      var dgv = new DataGridView()
      {
        Dock = DockStyle.Fill
      };

      var btn = new Button();

      flp.Controls.Add(btn, 0, 1);
      flp.Controls.Add(dgv, 0, 0);

      btn.Click += (s, e) =>
        {
          _fc.SaveChanges();
        };
      dgv.DataSource = _fc.Samples.Join(_fc.Irradiations, sample => sample.Id, ir => ir.SampleId, (sample, ir) => new
      {
        sample.Id,
        ir.Duration
      }).ToArray();


      // dgv.DataSource = _fc.Irradiations.ToArray();

      frm.Controls.Add(flp);

      Application.Run(frm);

    }


  } // public class Program
}   // namespace flow

